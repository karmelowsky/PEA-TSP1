using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public class Menu
    {
        private int _menuChoice;
        private string _matrixName = null;
        private ATSPMatrix _atspMatrix;

        private void ShowMenu()
        {
            Console.WriteLine("1. Generuj losowa macierz");
            Console.WriteLine("2. Wybierz macierz do wczytania");
            Console.WriteLine("3. Wyświetl macierz");
            Console.WriteLine("4. Rozpocznij algorytm");
            if(_matrixName?.Contains("Wygenerowana") ?? false)
            {
                Console.WriteLine("5. Zapisz wygenerowana macierz do pliku");
            }
            Console.WriteLine();
            Console.WriteLine("Macierz kosztów: " + (_matrixName ?? "brak\n"));
            Console.WriteLine("Twoj wybor: ");
        }

        private bool MakeMenuChoice()
        {
            try
            {
                _menuChoice = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                return false;
            }

            if(_menuChoice >0 && _menuChoice < 6)
            return true;

            return false;
        }


        public void Start()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    ShowMenu();

                    if (!MakeMenuChoice())
                    {
                        continue;
                    }

                    switch (_menuChoice)
                    {
                        case 1:
                            RandomMatrixGenerate();
                            break;
                        case 2:
                            ChooseFile();
                            break;
                        case 3:
                            ShowMatrix();
                            break;
                        case 4:
                            RunAlgorithm();
                            break;
                        case 5:
                            SaveGeneratedMatrixToFile();
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Błąd wprowadzania danych. Wciśnij dowolny klawisz aby kontynuować..");
                    continue;
                }
                
            }            
        }

        private void SaveGeneratedMatrixToFile()
        {
            if(!_matrixName.Contains("Wygenerowana") || _atspMatrix?.Matrix==null)
            {
                return;
            }

            var fileWriter = new FileWriter();
            if(fileWriter.SaveMatrixToFile(_atspMatrix))
            {
                Console.WriteLine("Zapisano wygenerowaną macierz kosztów");
            }
            else
            {
                Console.WriteLine("Błąd przy zapisie wygenerowanej macierzy kosztów");
            }

            Console.WriteLine("Wciśnij dowolny klawisz, aby kontynuować..");
            Console.ReadKey();
        }

        private void RunAlgorithm()
        {
            ATSPSolver solver = new ATSPSolver();
            solver.MatrixToSolve = _atspMatrix;

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

        private void ShowMatrix()
        {
            if (_atspMatrix?.Matrix == null)
            { 
                return;
            }

            new ConsoleDisplayer().ShowMatrix(_atspMatrix);
            Console.WriteLine("Wciśnij dowolny klawisz, aby kontynuować..");
            Console.ReadKey();
        }

        private void RandomMatrixGenerate()
        {
            Console.WriteLine("Podaj wymiar dla wygenerowanej macierzy kosztów: ");
            var dimension = int.Parse(Console.ReadLine());
            _atspMatrix = ATSPMatrix.GenerateRandomMatrix(dimension,0,200);
            _matrixName = "Wygenerowana,  " + dimension + " miast";
        }

        private void ChooseFile()
        {
            var fileReader = new FileReader();
            var consoleDisplayer = new ConsoleDisplayer();
            var fileList = fileReader.GetATSPFilenames();

            Console.WriteLine("Lista plików do wczytania: ");
            consoleDisplayer.ShowFileList(fileList);

            Console.WriteLine("Wpisz numer pliku, który ma być załadowany: ");
            var filenameNumber = int.Parse(Console.ReadLine());
            _atspMatrix = fileReader.GetMatrix(fileList[filenameNumber]);
            _matrixName = fileList[filenameNumber];
        }
    }
}
