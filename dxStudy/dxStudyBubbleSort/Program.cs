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

            Console.WriteLine("**********************************************************1");

            //bubble sort 2
            int[] arrInt2 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble2(arrInt2);

            Console.WriteLine("**********************************************************2");

            //bubble sort 3
            int[] arrInt3 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble3(arrInt3);

            Console.WriteLine("**********************************************************3");

            //bubble sort 4
            int[] arrInt4 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble4(arrInt4);

            Console.WriteLine("**********************************************************4");

            //bubble sort 5
            int[] arrInt5 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble5(arrInt5);

            Console.WriteLine("**********************************************************5");

            //bubble sort 6
            int[] arrInt6 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble6(arrInt6);

            Console.WriteLine("**********************************************************6");

            //bubble sort 7
            int[] arrInt7 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble7(arrInt7);

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

        static void SortByBubble4(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0, k = intLength - i; j < k; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[j]);
                }

                foreach (int item in arrInt)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }

        static void SortByBubble5(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0, k = intLength - i; j < k; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[i]);
                }

                foreach (int item in arrInt)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }

        static void SortByBubble6(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0, k = intLength - i; j < k; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[j]);
                }

                foreach (var item in arrInt)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }

        static void SortByBubble7(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            for (int i = 1; i < intLength; i++)
            {
                for (int j = 0, k = intLength - i; j < k; j++)
                {
                    if (arrInt[j] > arrInt[j + 1])
                        (arrInt[j], arrInt[j + 1]) = (arrInt[j + 1], arrInt[j]);
                }

                foreach (var item in arrInt)
                    Console.Write($"{item}\t");
                Console.WriteLine();
            }
        }
    }
}