using System;

namespace dxStudyBubbleSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arrInt = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            SortByBubble(arrInt);

            foreach (int item in arrInt)
            {
                Console.Write(item);
                Console.Write("\t");
            }

            Console.WriteLine("Hello World!");
        }

        static void SortByBubble(int[] arrInt)
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
            }
        }
    }
}
