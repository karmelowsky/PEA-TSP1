using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PEA_TSP1
{
    public class FileReader
    {
        private const string ATSPPath = "ALL_atsp\\";
        private const string TSPPath = "ALL_tsp\\";

        public List<string> GetATSPFilenames()
        {
            var listToReturn = new List<string>();

            listToReturn = Directory.GetFiles(ATSPPath).ToList();

            return listToReturn;
        }

        public ATSPMatrix GetMatrix(string filename)
        {

            int[,] matrix = null;
            string text = null;

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    text = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var lines = text.Split('\n');

            var dimensionString = lines[3].Substring(10);
            int dimension;

                     
            if (!int.TryParse(dimensionString, out dimension))
            {
                Console.WriteLine("Blad odczytu pliku");
                return null;
            }



            matrix = new int[dimension, dimension];

            int firstNumberIndex = text.LastIndexOf('N') + 3;

            text = text.Substring(firstNumberIndex);

            var stringNumbers = text.Split(new char[] {' ','\t', '\r' , 'E', 'O', 'F','\n'}, StringSplitOptions.RemoveEmptyEntries);

            int index = 0;
            for(int i = 0; i < dimension; i++)
            for (int j = 0; j < dimension; j++)
            {
                matrix[i,j] = int.Parse(stringNumbers[index]);

                if (i == j)
                {
                    matrix[i, j] = -1;
                }

                index++;
            }

            ATSPMatrix atspMatrix = new ATSPMatrix();
            atspMatrix.Matrix = matrix;

            return atspMatrix;
        }

    }
}
