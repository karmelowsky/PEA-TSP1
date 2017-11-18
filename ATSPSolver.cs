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
        public ATSPSolver()
        {
            LowerBound = 0;
            ignoredColumns = new List<int>();
            ignoredRows = new List<int>();
            ResultPath = new List<Edge>();
        }

        public ATSPMatrix AtspMatrix { get; set; }
        public int LowerBound { get; private set; }

        public List<Edge> ResultPath { get; private set; }

        private List<int> ignoredRows;
        private List<int> ignoredColumns;

        public void Solve()
        {
            new ConsoleDisplayer().ShowMatrix(AtspMatrix);
            while (ignoredColumns.Count < AtspMatrix.Dimension - 2)
            {
                
                Console.WriteLine();

                ReduceRows();
                ReduceColumns();


                // przeszukiwanie wierszów, minimalna wartość

                int allRowsMinsMax = -1;
                int rowIndex = -1;
                int rowMin;

                for (int i = 0; i < AtspMatrix.Dimension; i++)
                {
                    if (ignoredRows.Contains(i))
                        continue;

                    rowMin = FindRowMinimum(i);
                    if (rowMin > allRowsMinsMax)
                    {
                        allRowsMinsMax = rowMin;
                        rowIndex = i;
                    }


                }

                // przeszukiwanie kolumn, minimalna wartość
                int allColumnsMinsMax = -1;
                int columnIndex = -1;
                int columnMin;

                for (int i = 0; i < AtspMatrix.Dimension; i++)
                {
                    if (ignoredColumns.Contains(i))
                        continue;

                    columnMin = FindColumnMinimum(i);
                    if (columnMin > allColumnsMinsMax)
                    {
                        allColumnsMinsMax = columnMin;
                        columnIndex = i;
                    }

                }

                if (allRowsMinsMax > allColumnsMinsMax)
                {
                    IgnoreByRow(rowIndex);
                }
                else
                {
                    IgnoreByColumn(columnIndex);
                }

                new ConsoleDisplayer().ShowMatrix(AtspMatrix);
            }

            IgnoredRowsAndColumnsToResult();
        }

        private void IgnoredRowsAndColumnsToResult()
        {
            var edgeList = new List<Edge>();

            for (int i = 0; i < AtspMatrix.Dimension-2; i++)
            {
                var edge = new Edge();
                edge.CityA = ignoredRows[i];
                edge.CityB = ignoredColumns[i];
                edgeList.Add(edge);    
            }

            foreach (Edge edge in edgeList)
            {
                Console.WriteLine(edge.CityA + " "+edge.CityB);
            }

             ResultPath.Add(edgeList[0]);

            for (int i = 0; i < AtspMatrix.Dimension-2; i++)
            {
                var resultEdge = edgeList.FirstOrDefault(x => x.CityA == ResultPath[i].CityB);
                ResultPath.Add(resultEdge);
                // edgeList.Remove(resultEdge);
            }

        }

        private void IgnoreByRow(int row)
        {
            int columnToIgnore = -1;

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                if (AtspMatrix.Matrix[row, i] == 0 && columnToIgnore == -1)
                {
                    columnToIgnore = i;
                }

                AtspMatrix.Matrix[row, i] = -1;

            }

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                AtspMatrix.Matrix[i, columnToIgnore] = -1; 
            }

            AtspMatrix.Matrix[columnToIgnore, row] = -1;

            ignoredRows.Add(row);
            ignoredColumns.Add(columnToIgnore);
        }

        private void IgnoreByColumn(int column)
        {
            int rowToIgnore = -1;

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                if (AtspMatrix.Matrix[i, column] == 0 && rowToIgnore == -1)
                {
                    rowToIgnore = i;
                }

                AtspMatrix.Matrix[i, column] = -1;

            }

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                AtspMatrix.Matrix[rowToIgnore,i] = -1;
            }

            AtspMatrix.Matrix[column, rowToIgnore] = -1;
            ignoredColumns.Add(column);
            ignoredRows.Add(rowToIgnore);
        }

        private void ReduceRows()
        {
            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                int min= 99999;

                for (int j = 0; j < AtspMatrix.Dimension; j++)
                {
                    if (min > AtspMatrix.Matrix[i, j] && AtspMatrix.Matrix[i,j] != -1)
                    {
                        min = AtspMatrix.Matrix[i, j];
                    }
                }

                for (int j = 0; j < AtspMatrix.Dimension; j++)
                {
                    if(AtspMatrix.Matrix[i,j]!= -1)
                    AtspMatrix.Matrix[i, j] -= min;
                }

                if(min!=99999)
                    LowerBound += min;
            }
  
        }

        private void ReduceColumns()
        {

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                int min = 99999;

                for (int j = 0; j < AtspMatrix.Dimension; j++)
                {
                    if (min > AtspMatrix.Matrix[j, i] && AtspMatrix.Matrix[j, i] != -1)
                    {
                        min = AtspMatrix.Matrix[j, i];
                    }
                }

                for (int j = 0; j < AtspMatrix.Dimension; j++)
                {
                    if (AtspMatrix.Matrix[j, i] != -1)
                        AtspMatrix.Matrix[j, i] -= min;
                }
                if (min != 99999)
                    LowerBound += min;
            }

        }

        private int FindRowMinimum(int row)
        {
            int minimum = 99999;
            bool firstZero = true;

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                int cost = AtspMatrix.Matrix[row, i];

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

            for (int i = 0; i < AtspMatrix.Dimension; i++)
            {
                int cost = AtspMatrix.Matrix[i, column];

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
