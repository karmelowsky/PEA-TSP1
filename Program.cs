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

            var atspMatrix = fileReader.GetMatrix("ftv33.atsp");

            ATSPSolver solver = new ATSPSolver();
            solver.MatrixToSolve = atspMatrix;
            solver.Solve();

            Console.WriteLine();
            Console.Write(solver.ResultPath[0].CityA);

            foreach (var edge in solver.ResultPath)
            {
                Console.Write(" ->" + edge.CityB);
            }

            Console.WriteLine("\nKoszt podrozy: "+ solver.FinalCost);
            Console.ReadKey();
        }
    }
}
