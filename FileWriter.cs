using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public class FileWriter
    {
        private const string _directory = "ALL_atsp\\";
        public bool SaveMatrixToFile(ATSPMatrix atspMatrix)
        {
            var now = DateTime.Now;
            string fileName = _directory + "tsp" + atspMatrix.Dimension + "_generated_" 
                + now.Year +"_" + now.Month + "_" + now.Day + "_"
                + now.Hour +"_" + now.Minute+ "_" + now.Second;


            try
            {
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(atspMatrix.Dimension);

                    for (int i = 0; i < atspMatrix.Dimension; i++)
                    {
                        for (int j = 0; j < atspMatrix.Dimension; j++)
                        {
                            sw.Write(atspMatrix.Matrix[i, j]);

                            if (j != atspMatrix.Dimension - 1)
                                sw.Write(" ");
                        }

                        if (i != atspMatrix.Dimension - 1)
                            sw.Write("\n");
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;                          
        }
    }
}
