using System;
using System.Collections.Generic;
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
        }

        public ATSPMatrix AtspMatrix { get; set; }
        public int LowerBound { get; private set; }

        public void Solve()
        {
            ReduceRows();
            ReduceColumns();


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

            return minimum;
        }



    }
}
