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

            consoleDisplayer.ShowFileList(fileReader.GetATSPFilenames());

            var atspMatrix = fileReader.GetMatrix("ftv44.atsp");

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

            Console.WriteLine("\nKoszt podrozy: "+ solver.FinalCost);
            Console.WriteLine("Czas wykonywania algorytmu: " + difference.TotalMilliseconds+ "ms");
            Console.ReadKey();
        }
    }
}
