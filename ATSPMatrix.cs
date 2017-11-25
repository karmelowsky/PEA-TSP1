using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public class ATSPMatrix
    {
        private int[,] _matrix;
        private int? _dimension;

        public List<int> DeletedRows { get; set; }
        public List<int> DeletedColumns { get; set; }
        public List<Edge> TakenEdges { get; set; }
        public int LowerBound { get; set; }

        public bool Finished => TakenEdges.Count == Dimension;

        public int? Dimension
        {
            get => _dimension ?? -1;
            private set => _dimension = value;
        }

        public int[,] Matrix {
            get => _matrix;
            set
            {
                _matrix = value;
                Dimension = (int?)Math.Sqrt(value.Length);
            } }

        public ATSPMatrix()
        {
            LowerBound = 0;
            DeletedColumns = new List<int>();
            DeletedRows = new List<int>();
            TakenEdges = new List<Edge>();
        }

        public void ReduceRows()
        {
            for (int i = 0; i < this.Dimension; i++)
            {
                int min = 99999;

                for (int j = 0; j < this.Dimension; j++)
                {
                    if (min > this.Matrix[i, j] && this.Matrix[i, j] != -1)
                    {
                        min = this.Matrix[i, j];
                    }
                }

                for (int j = 0; j < this.Dimension; j++)
                {
                    if (this.Matrix[i, j] != -1)
                        this.Matrix[i, j] -= min;
                }

                if (min != 99999)
                    this.LowerBound += min;
            }

        }

        public void ReduceColumns()
        {

            for (int i = 0; i < this.Dimension; i++)
            {
                int min = 99999;

                for (int j = 0; j < this.Dimension; j++)
                {
                    if (min > this.Matrix[j, i] && this.Matrix[j, i] != -1)
                    {
                        min = this.Matrix[j, i];
                    }
                }

                for (int j = 0; j < this.Dimension; j++)
                {
                    if (this.Matrix[j, i] != -1)
                        this.Matrix[j, i] -= min;
                }
                if (min != 99999)
                    this.LowerBound += min;
            }

        }

        public static ATSPMatrix GenerateRandomMatrix(int dimension, int minValue, int maxValue)
        {
            var atspMatrix = new ATSPMatrix();
            var random = new Random();
            atspMatrix.Matrix = new int[dimension, dimension];

            for(int i = 0; i< dimension; i++)
            {
                for(int j=0; j< dimension; j++)
                {
                    if(j==i)
                    {
                        atspMatrix.Matrix[i, j] = -1;
                        continue;
                    }
                    atspMatrix.Matrix[i, j] = random.Next(minValue, maxValue);
                }
            }


            return atspMatrix;
        }
    }
}
