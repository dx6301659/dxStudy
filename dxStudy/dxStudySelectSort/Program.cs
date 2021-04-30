using System;

namespace dxStudySelectSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arrInt1 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SelectSort1(arrInt1);

            Console.WriteLine("*******************************************************1");

            Console.Read();
        }

        static void SelectSort1(int[] dataArray)
        {
            for (int i = 0; i < dataArray.Length - 1; i++)
            {
                int minIndex = 1;
                for (int j = i + 1; j < dataArray.Length; j++)
                {
                    if (dataArray[j] < dataArray[minIndex])
                        minIndex = j;
                }

                if (minIndex != i)
                    (dataArray[i], dataArray[minIndex]) = (dataArray[minIndex], dataArray[i]);

                foreach (int item in dataArray)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }
    }
}
