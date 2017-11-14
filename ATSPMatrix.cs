using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public class ATSPMatrix
    {
        private int? _dimension;

        private int[,] _matrix;
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


    }
}
