using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_TSP1
{
    public static class ConvertTools
    {
        public static int[] GetIntTabFromLine(string line, char separator)
        {
            string[] stringNumbers = line.Split(separator);
            int[] numbers = new int[stringNumbers.Length];

            int index = 0;

            try
            {
                foreach (var stringNumber in stringNumbers)
                {
                    numbers[index] = int.Parse(stringNumber);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return numbers;
        }

        public static bool HasStringOnlyNumbersOrSpacebars(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9' || c != ' ')
                    return false;
            }

            return true;
        }

        public static List<Edge> CopyEdges(List<Edge> edgeList)
        {
            var newList = new List<Edge>();

            foreach (var edge in edgeList)
            {
                newList.Add(edge);
            }

            return newList;
        }
    }
}
