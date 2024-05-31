using System;
using System.Text;

namespace bt7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Random rand = new Random();
            List<int> randomList = new List<int>();
            for (int i = 0; i < 1000000; i++)
            {
                randomList.Add(rand.Next(1, 100001));
            }

  
            List<int> primeListNot = randomList.Where(num => !IsPrime(num)).ToList();

            Console.WriteLine("Danh sách các số còn lại:");
            PrintList(primeListNot);

            List<int> randomList2 = new List<int>(); 
            for (int i = 0; i < 10000; i++)
            {
                randomList2.Add(rand.Next(1, 100001));
            }

            List<int> primeList = new List<int>();

            foreach (int i in randomList2) 
            {
                if (IsPrime(i) && !primeList.Contains(i))
                {
                    primeList.Add(i);
                }
            }

            Console.WriteLine("Danh sách cuối cùng: ");
            PrintList(primeList);
        }

        static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;
            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }
            return true;
        }

        static void PrintList(List<int> list)
        {
            foreach (var num in list)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
        }
    }
}
