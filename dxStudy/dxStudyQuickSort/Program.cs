using System;

namespace dxStudyQuickSort
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] arrInt = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            int[] arrInt = { 27, 38, 13, 49, 76, 97, 65 };
            QuickSort(arrInt);

            Console.WriteLine("Hello World!");
        }

        static void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSort(arrInt, 0, arrInt.Length - 1);
        }

        static void QuickSort(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intIndex = QuickSortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSort(arrInt, intLeftIndex, intIndex - 1);

            //right part
            QuickSort(arrInt, intIndex + 1, intRightIndex);
        }

        static int QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            int intTag = arrInt[intLeftIndex];

            while (intLeftIndex < intRightIndex)
            {
                //from right to left
                while (intLeftIndex < intRightIndex && arrInt[intRightIndex] > intTag)
                    intRightIndex--;
                arrInt[intLeftIndex] = arrInt[intRightIndex];

                //from left to right
                while (intLeftIndex < intRightIndex && arrInt[intLeftIndex] < intTag)
                    intLeftIndex++;
                arrInt[intRightIndex] = arrInt[intLeftIndex];
            }
            arrInt[intLeftIndex] = intTag;

            foreach (int item in arrInt)
            {
                Console.Write($"{item}\t");
            }
            Console.WriteLine();

            return intRightIndex;
        }

        static int QuickSortUnit2(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            int intTag = arrInt[intLeftIndex];

            while (intLeftIndex < intRightIndex)
            {
                //from right to left
                while (intLeftIndex < intRightIndex && arrInt[intRightIndex] > intTag)
                    intRightIndex--;
                (arrInt[intLeftIndex], arrInt[intRightIndex]) = (arrInt[intRightIndex], arrInt[intLeftIndex]);

                //from left to right
                while (intLeftIndex < intRightIndex && arrInt[intLeftIndex] < intTag)
                    intLeftIndex++;
                (arrInt[intLeftIndex], arrInt[intRightIndex]) = (arrInt[intRightIndex], arrInt[intLeftIndex]);
            }
            arrInt[intLeftIndex] = intTag;

            foreach (int item in arrInt)
            {
                Console.Write($"{item}\t");
            }
            Console.WriteLine();

            return intRightIndex;
        }
    }
}
