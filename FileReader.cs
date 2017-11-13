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

        public int[,] GetMatrix(string filename)
        {
            int[,] matrix = null;
            string text = null;

            try
            {
                using (StreamReader sr = new StreamReader(ATSPPath + filename + "\\" + filename))
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

            //string[] numberLines = new string[dimension * dimension];
            //Array.Copy(lines, 7, numberLines, 0, lines.Length - 7);

            //matrix = new int[dimension, dimension];


            //for (int i = 0; i < dimension; i++)
            //{

            //}

            int firstNumberIndex = text.LastIndexOf('N') + 3;

            text = text.Substring(firstNumberIndex);

            var stringNumbers = text.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            

            return matrix;
        }

    }
}
