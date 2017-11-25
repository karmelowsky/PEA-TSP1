using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    class Program
    {
        static void Main(string[] args)
        {
            FileReader fileReader = new FileReader();
            var consoleDisplayer = new ConsoleDisplayer();
            int filenameNumber;
           


            while (true)
            {
                Console.Clear();
                var fileList = fileReader.GetATSPFilenames();
                consoleDisplayer.ShowFileList(fileList);
                Console.WriteLine("Wpisz numer pliku z macierza do rozwiazania: ");
                filenameNumber = int.Parse(Console.ReadLine());
                var atspMatrix = fileReader.GetMatrix(fileList[filenameNumber]);

                // var atspMatrix = ATSPMatrix.GenerateRandomMatrix(40, 2, 200);
                ATSPSolver solver = new ATSPSolver();
                solver.MatrixToSolve = atspMatrix;

                var start = DateTime.Now;
                solver.Solve();
                var stop = DateTime.Now;
                TimeSpan difference = stop - start;

                Console.WriteLine();
                Console.Write(solver.ResultPath[0].CityA);

                foreach (var edge in solver.ResultPath)
                {
                    Console.Write(" ->" + edge.CityB);
                }

                Console.WriteLine("\nKoszt podrozy: " + solver.FinalCost);
                Console.WriteLine("Czas wykonywania algorytmu: " + difference.TotalMilliseconds + "ms");
                Console.WriteLine("Wciśnij dowolny klawisz, aby kontynuować..");
                Console.ReadKey();
            }
            
        }
    }
}
