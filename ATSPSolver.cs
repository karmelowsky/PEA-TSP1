using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public class ATSPSolver
    {
        private ATSPMatrix _currentAtspMatrix;
        private List<ATSPMatrix> _matrixQueue;

        public List<Edge> ResultPath { get; private set; }
        public ATSPMatrix MatrixToSolve { get; set; }

        public ATSPSolver()
        {
            ResultPath = new List<Edge>();
            _matrixQueue = new List<ATSPMatrix>();
        }

        public void Solve()
        {
            if (MatrixToSolve == null)
            {
                Console.WriteLine("Nie przydzielono macierzy do rozwiązania");
                return;
            }

            _currentAtspMatrix = new ATSPMatrix();
            _currentAtspMatrix.Matrix = (int[,])MatrixToSolve.Matrix.Clone();

            while (_currentAtspMatrix.TakenEdges.Count <= _currentAtspMatrix.Dimension -2)
            {
                ReduceRows();
                ReduceColumns();

                Console.WriteLine();
                new ConsoleDisplayer().ShowMatrix(_currentAtspMatrix);

                var edgeToDelete = FindEdgeToDelete();

                // Xij = 0 Przygotowanie macierzy
                var matrixB = new ATSPMatrix();
                matrixB.Matrix = (int[,])_currentAtspMatrix.Matrix.Clone();

                // zablokowanie Xij = INF
                matrixB.Matrix[edgeToDelete.CityA, edgeToDelete.CityB] = -1;

                // redukcja alternatywnej drogi i redukcja (aktualizacja LB)
                matrixB.ReduceRows();
                matrixB.ReduceColumns();

                //dodanie macierzy B listy oczekujacych
                _matrixQueue.Add(matrixB);

                //wstawienie nieskonczonosci do obecnej macierzy w wyszukanym wierszu i kolumnie
                SetInfToRowAndColumn(edgeToDelete.CityA, edgeToDelete.CityB);
                _currentAtspMatrix.TakenEdges.Add(edgeToDelete);

                //Console.WriteLine();
                //new ConsoleDisplayer().ShowMatrix(_currentAtspMatrix);
            }
            
        }

        private void SetInfToRowAndColumn(int row, int col)
        {
            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                _currentAtspMatrix.Matrix[row, i] = -1;
                _currentAtspMatrix.Matrix[i, col] = -1;
            }  

            //eliminacja podtras
            if (_currentAtspMatrix.Matrix[col, row] != -1)
            {
                _currentAtspMatrix.Matrix[col, row] = -1;
                return;
            }

            for (int i = _currentAtspMatrix.TakenEdges.Count-1; i >= 0; i--)
            {
                var cityA = _currentAtspMatrix.TakenEdges[i].CityA;
                var cityB = _currentAtspMatrix.TakenEdges[i].CityB;
                if (_currentAtspMatrix.Matrix[col, cityA] != -1)
                {
                    _currentAtspMatrix.Matrix[col, cityA] = -1;
                    return;
                }

                if (_currentAtspMatrix.Matrix[cityB, row] != -1)
                {
                    _currentAtspMatrix.Matrix[cityB, row] = -1;
                    return;
                }
            }
                
            

            Console.WriteLine("Błąd SetInfRowAndColumn");
        }

        private void DeletedRowsAndColumnsToResult()
        {
            var edgeList = new List<Edge>();

            for (int i = 0; i < _currentAtspMatrix.Dimension-2; i++)
            {
                var edge = new Edge();
                edge.CityA = _currentAtspMatrix.DeletedRows[i];
                edge.CityB = _currentAtspMatrix.DeletedColumns[i];
                edgeList.Add(edge);    
            }

            foreach (Edge edge in edgeList)
            {
                Console.WriteLine(edge.CityA + " "+edge.CityB);
            }

             ResultPath.Add(edgeList[0]);

            for (int i = 0; i < _currentAtspMatrix.Dimension-2; i++)
            {
                var resultEdge = edgeList.FirstOrDefault(x => x.CityA == ResultPath[i].CityB);
                ResultPath.Add(resultEdge);
            }

        }


        private void ReduceRows()
        {
            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                int min= 99999;

                for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                {
                    if (min > _currentAtspMatrix.Matrix[i, j] && _currentAtspMatrix.Matrix[i,j] != -1)
                    {
                        min = _currentAtspMatrix.Matrix[i, j];
                    }
                }

                for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                {
                    if(_currentAtspMatrix.Matrix[i,j]!= -1)
                    _currentAtspMatrix.Matrix[i, j] -= min;
                }

                if(min!=99999)
                    _currentAtspMatrix.LowerBound += min;
            }
  
        }

        private void ReduceColumns()
        {

            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                int min = 99999;

                for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                {
                    if (min > _currentAtspMatrix.Matrix[j, i] && _currentAtspMatrix.Matrix[j, i] != -1)
                    {
                        min = _currentAtspMatrix.Matrix[j, i];
                    }
                }

                for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                {
                    if (_currentAtspMatrix.Matrix[j, i] != -1)
                        _currentAtspMatrix.Matrix[j, i] -= min;
                }
                if (min != 99999)
                    _currentAtspMatrix.LowerBound += min;
            }

        }

        private Edge FindEdgeToDelete()
        {
            var edgeToReturn = new Edge();
            int maxPenalty = -1;

            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                {
                    if (_currentAtspMatrix.Matrix[i, j] == 0)
                    {
                        int penalty = FindRowMinimum(i) + FindColumnMinimum(j);
                        if (penalty > maxPenalty)
                        {
                            maxPenalty = penalty;
                            edgeToReturn.CityA = i;
                            edgeToReturn.CityB = j;
                        }
                           
                    }
                }
            }
            return edgeToReturn;
        }

        private int FindRowMinimum(int row)
        {
            int minimum = 99999;
            bool firstZero = true;

            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                int cost = _currentAtspMatrix.Matrix[row, i];

                if(cost == -1 )
                    continue;

                if(cost == 0 && firstZero)
                {
                    firstZero = false;
                    continue;
                }

                if (cost < minimum)
                    minimum = cost;

            }
            if (minimum == 99999)
                Console.WriteLine("blad");

            return minimum;
        }

        private int FindColumnMinimum(int column)
        {
            int minimum = 99999;
            bool firstZero = true;

            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                int cost = _currentAtspMatrix.Matrix[i, column];

                if (cost == -1)
                    continue;

                if (cost == 0 && firstZero)
                {
                    firstZero = false;
                    continue;
                }

                if (cost < minimum)
                    minimum = cost;

            }


            if (minimum == 99999)
                Console.WriteLine("blad");

            return minimum;
        }



    }
}
