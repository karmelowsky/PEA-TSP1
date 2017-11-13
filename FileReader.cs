using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            
            matrix = new int[dimension, dimension];

            foreach (var line in lines)
            {
                
            }


            return matrix;
        }

    }
}
