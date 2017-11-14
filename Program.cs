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

            consoleDisplayer.ShowMatrix(atspMatrix);


            Console.ReadKey();
        }
    }
}
