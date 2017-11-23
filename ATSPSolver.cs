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

        public int FinalCost { get; set; } 

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

            while (_currentAtspMatrix.TakenEdges.Count < _currentAtspMatrix.Dimension -2)
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
                Console.WriteLine("Poniżej wzieto krawedz: "+ (edgeToDelete.CityA) +" "+(edgeToDelete.CityB));
                //Console.WriteLine();
                //new ConsoleDisplayer().ShowMatrix(_currentAtspMatrix);
            }


            //dodanie koncowych dwoch krawedzi
            ReduceRows();
            ReduceColumns();
            TakeLastTwoEdges();
            DeletedRowsAndColumnsToResult();
            FinalCost = _currentAtspMatrix.LowerBound;
            new ConsoleDisplayer().ShowMatrix(_currentAtspMatrix);


        }

        private void TakeLastTwoEdges()
        {
            if (!(_currentAtspMatrix.TakenEdges.Count >= _currentAtspMatrix.Dimension - 2))
            {
                Console.WriteLine("Błąd TakeLastTwoEdges");
                return;
            }

            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                bool oneZero = false;
                int zeroX=-1;
                int zeroY=-1;

                for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                {
                    if (_currentAtspMatrix.Matrix[i, j] == 0)
                    {
                        oneZero = !oneZero;
                        zeroX = i;
                        zeroY = j;
                    }
                }

                if (oneZero)
                {
                    var edge = new Edge();
                    edge.CityA = zeroX;
                    edge.CityB = zeroY;
                    _currentAtspMatrix.TakenEdges.Add(edge);

                    for (int j = 0; j < _currentAtspMatrix.Dimension; j++)
                    {
                        _currentAtspMatrix.Matrix[j, zeroY] = -1;
                    }
                }

            }
            if (_currentAtspMatrix.Finished)
            {
                return;
            }
            TakeLastTwoEdges();

        }

        private void SetInfToRowAndColumn(int row, int col)
        {
            for (int i = 0; i < _currentAtspMatrix.Dimension; i++)
            {
                _currentAtspMatrix.Matrix[row, i] = -1;
                _currentAtspMatrix.Matrix[i, col] = -1;
            }  

            //eliminacja podtras

            //znalezienie poczatku i konca subtrasy
            int subTourEnd = col;
            int subTourStart = row;
            bool tourChanged=false;

            do
            {
                tourChanged = false;
                foreach (var edge in _currentAtspMatrix.TakenEdges)
                {
                    if (edge.CityB == subTourStart)
                    {
                        subTourStart = edge.CityA;
                        tourChanged = true;
                    }

                    if (edge.CityA == subTourEnd)
                    {
                        subTourEnd = edge.CityB;
                        tourChanged = true;
                    }
                }
            } while (tourChanged);

            //wstawienie inf w celu zablokowania przejscia powrotnego
            if (_currentAtspMatrix.Matrix[subTourEnd, subTourStart] != -1)
            {
                _currentAtspMatrix.Matrix[subTourEnd, subTourStart] = -1;
                return;
            }

            Console.WriteLine("Błąd SetInfRowAndColumn");
        }

        private void DeletedRowsAndColumnsToResult()
        {
            if (_currentAtspMatrix.Dimension != _currentAtspMatrix.TakenEdges.Count)
            {
                Console.WriteLine("Błąd DeletedRowsAndColumnToResultPath");
            }

            var edgeListToReturn = new List<Edge>();
            edgeListToReturn.Add(_currentAtspMatrix.TakenEdges[0]);

            for (int i = 0; i < _currentAtspMatrix.Dimension- 1; i++)
            {
                edgeListToReturn.Add(_currentAtspMatrix.TakenEdges.First(e => e.CityA == edgeListToReturn[i].CityB));
            }

            ResultPath = edgeListToReturn;
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
