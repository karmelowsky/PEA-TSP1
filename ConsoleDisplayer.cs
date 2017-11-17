using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public class ConsoleDisplayer
    {
        public void ShowFileList(List<string> fileList)
        {
            int index = 0;

            foreach (var fileName in fileList)
            {
                Console.WriteLine(index + ". " + fileName);
                index++;
            }
            Console.WriteLine();
        }

        public void ShowMatrix(ATSPMatrix atspMatrix)
        {
            for (int i = 0; i < atspMatrix.Dimension; i++)
            {
                for (int j = 0; j < atspMatrix.Dimension; j++)
                {
                    Console.Write(atspMatrix.Matrix[i, j]+ "  ");
                }
                Console.WriteLine();
            }
            
        }
    }
}
