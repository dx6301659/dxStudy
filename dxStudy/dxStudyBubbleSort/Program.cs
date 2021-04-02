using System;

namespace dxStudyBubbleSort
{
    class Program
    {
        static void Main(string[] args)
        {
            //bubble sort 1
            int[] arrInt1 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble1(arrInt1);

            Console.WriteLine("**********************************************************");

            //bubble sort 2
            int[] arrInt2 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble2(arrInt2);

            Console.WriteLine("**********************************************************");

            //bubble sort 3
            int[] arrInt3 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble3(arrInt3);

            Console.Read();
        }

        static void SortByBubble1(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0; j < intLength - i; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[j]);
                }

                foreach (int item in arrInt)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }

        static void SortByBubble2(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0; j < intLength - i; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[j]);
                }

                foreach (int item in arrInt)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }

        static void SortByBubble3(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0; j < intLength - i; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[j]);
                }

                foreach (int item in arrInt)
                    Console.Write($"{item}\t");

                Console.WriteLine();
            }
        }
    }
}
