using System;

namespace dxStudyQuickSort
{
    class Program
    {
        static void Main(string[] args)
        {
            //quick sort 1
            int[] arrInt1 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt1 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort1 = new QuickSort1();
            quickSort1.QuickSort(arrInt1);

            Console.WriteLine("*******************************************************1");

            //quick sort 2
            int[] arrInt2 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt2 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort2 = new QuickSort2();
            quickSort2.QuickSort(arrInt2);

            Console.WriteLine("*******************************************************2");

            //quick sort 3
            int[] arrInt3 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt3 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort3 = new QuickSort3();
            quickSort3.QuickSort(arrInt3);

            Console.WriteLine("*******************************************************3");

            //quick sort 4
            int[] arrInt4 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt4 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort4 = new QuickSort4();
            quickSort4.QuickSort(arrInt4);

            Console.WriteLine("*******************************************************4");

            //quick sort 5
            int[] arrInt5 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt5 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort5 = new QuickSort5();
            quickSort5.QuickSort(arrInt5);

            Console.WriteLine("*******************************************************5");

            //quick sort 6
            int[] arrInt6 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt6 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort6 = new QuickSort6();
            quickSort6.QuickSort(arrInt6);

            Console.WriteLine("*******************************************************6");

            //quick sort 7
            int[] arrInt7 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt7 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort7 = new QuickSort7();
            quickSort7.QuickSort(arrInt7);

            Console.WriteLine("*******************************************************7");

            //quick sort 8
            int[] arrInt8 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt8 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort8 = new QuickSort8();
            quickSort8.QuickSort(arrInt8);

            Console.WriteLine("*******************************************************8");

            //quick sort 9
            int[] arrInt9 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt9 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort9 = new QuickSort9();
            quickSort9.QuickSort(arrInt9);

            Console.WriteLine("*******************************************************9");

            //quick sort 10
            int[] arrInt10 = { 12, 5, 2, 1, 3, 6, 9, 8, 7, 0, 11, 13 };
            //int[] arrInt10 = { 27, 38, 13, 49, 76, 97, 65 };
            var quickSort10 = new QuickSort10();
            quickSort10.QuickSort(arrInt10);

            Console.WriteLine("*******************************************************10");

            Console.Read();
        }
    }

    public class QuickSort1
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intIndex = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intIndex - 1);

            //right part
            QuickSortUnit(arrInt, intIndex + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

        private int SortUnit2(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (int item in arrInt)
            {
                Console.Write($"{item}\t");
            }
            Console.WriteLine();

            return intRightIndex;
        }
    }

    public class QuickSort2
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intStartIndex, int intEndIndex)
        {
            if (intStartIndex >= intEndIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intStartIndex, intEndIndex);

            //left part
            QuickSortUnit(arrInt, intStartIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intEndIndex);
        }

        private int SortUnit(int[] arrInt, int intStartIndex, int intEndIndex)
        {
            int intTag = arrInt[intStartIndex];
            while (intStartIndex < intEndIndex)
            {
                //from right to left
                while (intStartIndex < intEndIndex && arrInt[intEndIndex] > intTag)
                    intEndIndex--;
                (arrInt[intStartIndex], arrInt[intEndIndex]) = (arrInt[intEndIndex], arrInt[intStartIndex]);

                //from lef to right
                while (intStartIndex < intEndIndex && arrInt[intStartIndex] < intTag)
                    intStartIndex++;
                (arrInt[intStartIndex], arrInt[intEndIndex]) = (arrInt[intEndIndex], arrInt[intStartIndex]);
            }

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intStartIndex;
        }

        private int SortUnit2(int[] arrInt, int intStartIndex, int intEndIndex)
        {
            int intTag = arrInt[intStartIndex];
            while (intStartIndex < intEndIndex)
            {
                //from right to left
                while (intStartIndex < intEndIndex && arrInt[intEndIndex] > intTag)
                    intEndIndex--;
                arrInt[intStartIndex] = arrInt[intEndIndex];

                //from left to right
                while (intStartIndex < intEndIndex && arrInt[intStartIndex] < intTag)
                    intStartIndex++;
                arrInt[intEndIndex] = arrInt[intStartIndex];
            }

            arrInt[intStartIndex] = intTag;

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intEndIndex;
        }
    }

    public class QuickSort3
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort4
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;
            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort5
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort6
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort7
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (var item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort8
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            int intLength = arrInt.Length;
            QuickSortUnit(arrInt, 0, intLength - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            int intTag = arrInt[intLeftIndex];
            while (intLeftIndex < intRightIndex)
            {
                // from right to left
                while (intLeftIndex < intRightIndex && arrInt[intRightIndex] > intTag)
                    intRightIndex--;
                (arrInt[intLeftIndex], arrInt[intRightIndex]) = (arrInt[intRightIndex], arrInt[intLeftIndex]);

                // from left to right
                while (intLeftIndex < intRightIndex && arrInt[intLeftIndex] < intTag)
                    intLeftIndex++;
                (arrInt[intLeftIndex], arrInt[intRightIndex]) = (arrInt[intRightIndex], arrInt[intLeftIndex]);
            }

            foreach (var item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort9
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first round
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            int intTag = arrInt[intLeftIndex];
            while (intLeftIndex < intRightIndex)
            {
                // from right to left
                while (intLeftIndex < intRightIndex && arrInt[intRightIndex] > intTag)
                    intRightIndex--;
                (arrInt[intLeftIndex], arrInt[intRightIndex]) = (arrInt[intRightIndex], arrInt[intLeftIndex]);

                // from left to right
                while (intLeftIndex < intRightIndex && arrInt[intLeftIndex] < intTag)
                    intLeftIndex++;
                (arrInt[intLeftIndex], arrInt[intRightIndex]) = (arrInt[intRightIndex], arrInt[intLeftIndex]);
            }

            foreach (int item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }

    public class QuickSort10
    {
        public void QuickSort(int[] arrInt)
        {
            if (arrInt == null || arrInt.Length == 0)
                return;

            QuickSortUnit(arrInt, 0, arrInt.Length - 1);
        }

        private void QuickSortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
        {
            if (intLeftIndex >= intRightIndex)
                return;

            //first found
            int intResult = SortUnit(arrInt, intLeftIndex, intRightIndex);

            //left part
            QuickSortUnit(arrInt, intLeftIndex, intResult - 1);

            //right part
            QuickSortUnit(arrInt, intResult + 1, intRightIndex);
        }

        private int SortUnit(int[] arrInt, int intLeftIndex, int intRightIndex)
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

            foreach (var item in arrInt)
                Console.Write($"{item}\t");
            Console.WriteLine();

            return intLeftIndex;
        }
    }
}
