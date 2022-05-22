using System;
using System.Collections.Generic;

namespace Convolutional_Neural_Network
{
    public static class Action
    {
        private static readonly int[,] comper1 = new int[,]
        {{ 1, -1, -1},
        { -1, 1, -1},
        { -1, -1, 1},};

        private static readonly int[,] comper2 = new int[,]
        {{ 1, -1, 1},
        { -1, 1, -1},
        { 1, -1, 1},};

        private static readonly int[,] comper3 = new int[,]
        {{ -1, -1, 1},
        { -1, 1, -1},
        { 1, -1, -1},};
        public static string Dicaver(double[,] list)
        {
            list = Closing(list);
            var temp1 = Convolution(list, comper1);
            var temp2 = Convolution(list, comper2);
            var temp3 = Convolution(list, comper3);

            temp1 = ReLU(temp1);
            temp2 = ReLU(temp2);
            temp3 = ReLU(temp3);

            temp1 = Convolution(list, comper1);
            temp2 = Convolution(list, comper2);
            temp3 = Convolution(list, comper3);

            temp1 = ReLU(temp1);
            temp2 = ReLU(temp2);
            temp3 = ReLU(temp3);

            while (temp1.GetLength(0) > 2)
            {
                temp1 = Pooling(temp1);
                temp2 = Pooling(temp2);
                temp3 = Pooling(temp3);

                temp1 = ReLU(temp1);
                temp2 = ReLU(temp2);
                temp3 = ReLU(temp3);
            }

            var final = Union(temp1, temp2, temp3);
            double X = (final[0] + final[3] + final[4] + final[9]+ final[10])/5;
            double O = (final[1] + final[2] + final[5]+ final[11])/4;
            if (Math.Abs(X-O) < 0.1)
                return $"not X or O";
            if (X>O)
                return $"X";
            if (O>X)
                return $"O";
            return "cant tell";
        }

        public static double[,] TrimArray(double rowToRemove, double columnToRemove, double[,] originalArray)
        {
            double[,] result = new double[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }

        public static double[,] Closing(double[,] list)
        {
            int sumRow = 0;
            int sumColumn = 0;
            for (int i = list.GetLength(0)-1; i >= 0; i--)
            {
                for (int q = list.GetLength(0)-1; q >= 0; q--)
                {
                    if (list[i, q]==-1)
                        sumColumn++;
                    if (list[q, i]==-1)
                        sumRow++;
                }
                if (sumRow==list.GetLength(0) && sumColumn==list.GetLength(0))
                    list = TrimArray(i, i, list);
                sumRow = 0;
                sumColumn = 0;
            }
            return list;
        }

        public static double[,] Convolution(double[,] list, int[,] comper)
        {
            double count = 0;
            int b, a, s;
            s=0;
            double[,] returned = new double[list.GetLength(0)-2, list.GetLength(0)-2];
            for (int w = 0; w < list.GetLength(0)-2; w++)
            {
                for (int h = 0; h < list.GetLength(0)-2; h++)
                {
                    a=0;
                    for (int i = w; i < 3+w; i++)
                    {
                        b=0;
                        for (int q = h; q < 3+h; q++)
                        {
                            try
                            {
                                count += list[i, q] * comper[a, b];
                                b++;
                                s++;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                continue;
                            }
                        }
                        a++;
                    }
                    returned[w, h] = count/s;
                    s=0;
                    count = 0;
                }
            }
            return returned;
        }
        public static double[,] Pooling(double[,] list)
        {
            double comper = 0;
            int b = 0;
            int a = 0;
            double[,] returned = new double[list.GetLength(0)/2 + list.GetLength(0)%2, list.GetLength(0)/2 + list.GetLength(0)%2];
            for (int w = 0; w < (list.GetLength(0)/2 + list.GetLength(0)%2)*2; w+=2)
            {
                for (int h = 0; h < (list.GetLength(0)/2 + list.GetLength(0)%2)*2; h+=2)
                {
                    for (int i = w; i < 2+w; i++)
                    {
                        for (int q = h; q < 2+h; q++)
                        {
                            try
                            {
                                if (comper < list[i, q])
                                {
                                    comper = list[i, q];
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                continue;
                            }

                        }
                    }
                    returned[a, b] =comper;
                    comper = 0;
                    b++;
                }
                a++;
                b=0;
            }
            return returned;
        }
        public static double[,] ReLU(double[,] list)
        {
            for (int i = 0; i < list.GetLength(0); i++)
            {
                for (int w = 0; w < list.GetLength(0); w++)
                {
                    if (list[i, w] < 0)
                    {
                        list[i, w] = 0;
                    }
                }
            }
            return list;
        }

        public static List<double> Union(double[,] list1, double[,] list2, double[,] list3)
        {
            List<double> returned = new();
            for (int i = 0; i < 2; i++)
            {
                for (int q = 0; q < 2; q++)
                {
                    returned.Add(list1[i, q]);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int q = 0; q < 2; q++)
                {
                    returned.Add(list2[i, q]);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int q = 0; q < 2; q++)
                {
                    returned.Add(list3[i, q]);
                }
            }
            return returned;
        }
    }
}
