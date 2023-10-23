using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Data
{
    public static class Variant
    {
        public static double[,] prob_02_M = new double[,]
        {
            { 0.14, 0.14, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04, 0.04 }
        };

        public static double[,] prob_02_K = new double[,]
        {
            { 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05 }
        };
        public static double[,] table_02 { get; set; } = new double[,]
        {
            { 19, 13, 3, 16, 5, 10, 8, 1, 12, 14, 7, 2, 17, 11, 15, 18, 6, 0, 4, 9 },
            { 15, 13, 17, 5, 2, 9, 7, 11, 3, 18, 0, 12, 8, 10, 6, 19, 1, 16, 4, 14 },
            { 3, 4, 5, 12, 16, 14, 9, 10, 7, 11, 1, 13, 6, 18, 0, 2, 17, 19, 8, 15 },
            { 3, 13, 19, 6, 9, 1, 2, 16, 14, 10, 5, 8, 18, 12, 17, 0, 11, 4, 15, 7 },
            { 11, 7, 5, 12, 18, 1, 8, 14, 2, 6, 17, 9, 16, 10, 15, 13, 4, 19, 0, 3 },
            { 5, 0, 1, 12, 9, 11, 4, 19, 7, 15, 14, 8, 6, 18, 13, 2, 3, 10, 16, 17 },
            { 10, 18, 13, 11, 0, 17, 1, 14, 9, 15, 3, 5, 16, 12, 4, 2, 6, 19, 7, 8 },
            { 6, 4, 15, 16, 10, 1, 8, 17, 2, 18, 11, 19, 3, 14, 0, 9, 5, 13, 12, 7},
            { 13, 0, 11, 14, 9, 10, 19, 4, 17, 12, 18, 7, 15, 5, 6, 1, 2, 3, 8, 16},
            { 16, 6, 15, 19, 2, 4, 14, 11, 9, 3, 1, 10, 17, 5, 0, 18, 12, 8, 7, 13},
            { 11, 7, 19, 18, 9, 2, 5, 10, 3, 15, 14, 16, 1, 17, 0, 12, 8, 6, 13, 4},
            { 11, 9, 0, 19, 17, 13, 12, 2, 10, 6, 18, 1, 5, 4, 8, 7, 15, 3, 16, 14},
            { 5, 9, 0, 7, 12, 11, 18, 17, 3, 13, 1, 16, 15, 8, 6, 2, 10, 14, 19, 4},
            { 12, 10, 17, 8, 3, 9, 4, 13, 0, 1, 15, 18, 19, 2, 6, 14, 5, 16, 7, 11},
            { 16, 15, 8, 13, 0, 11, 18, 17, 19, 12, 3, 4, 5, 9, 6, 10, 2, 1, 7, 14},
            { 16, 10, 17, 2, 0, 9, 19, 1, 5, 7, 4, 12, 13, 8, 18, 11, 6, 14, 15, 3},
            { 1, 13, 17, 15, 3, 14, 2, 10, 16, 7, 5, 6, 18, 8, 0, 19, 12, 9, 4, 11 },
            { 17, 18, 7, 10, 14, 12, 3, 9, 15, 1, 19, 8, 2, 4, 11, 6, 13, 5, 16, 0},
            { 8, 2, 6, 4, 0, 7, 17, 19, 10, 14, 16, 9, 13, 3, 1, 12, 11, 18, 5, 15 },
            { 17, 15, 8, 11, 2, 1, 10, 4, 12, 5, 18, 6, 16, 3, 7, 19, 9, 13, 14, 0}
        };

        public static void WriteData()
        {
            Matrix prob_02_M_matrix = new Matrix(prob_02_M);
            Matrix prob_02_K_matrix = new Matrix(prob_02_K);
            Matrix table_02_matrix = new Matrix(table_02);

            Console.WriteLine("\nprop_02.csv for M:\n\n");
            Console.WriteLine(prob_02_M_matrix);

            Console.WriteLine("\nprop_02.csv for K:\n\n");
            Console.WriteLine(prob_02_K_matrix);

            Console.WriteLine("\n\ntable_02.csv:\n\n");
            Console.WriteLine(table_02_matrix);
        }

        public static Pair[,] GetPairs()
        {
            var result = new Pair[20, 20];

            for (int row = 0; row < 20; row++)
            {
                for (int column = 0; column < 20; column++)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        if (k == ((int)table_02[row, column]))
                        {
                            result[row, k] = new Pair
                            {
                                KeyId = row,
                                MessageId = column
                            };
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static decimal[,] GetCiphertextsProbabilities()
        {
            var pairs = GetPairs();
            var result = new decimal[20, 1];

            for (int column = 0; column < 20; column++)
            {
                for (int row = 0; row < 20; row++)
                {
                    result[column, 0] += (decimal)prob_02_M[0, pairs[row, column].MessageId] * (decimal)prob_02_K[0, pairs[row, column].KeyId];
                }
            }

            return result;
        }

        public static decimal[,] GetCiphertextMessagesProbabilities()
        {
            var pairs = GetPairs();
            var result = new decimal[20, 20];

            for (int column = 0; column < 20; column++)
            {
                for (int messageId = 0; messageId < 20; messageId++)
                {
                    var keys = new List<int>();

                    for (int row = 0; row < 20; row++)
                    {
                        if (pairs[row, column].MessageId == messageId)
                        {
                            keys.Add(pairs[row, column].KeyId);
                        }
                    }

                    var keysSum = keys.Select(indexKey => (decimal)prob_02_K[0, indexKey]).ToList().Sum();
                    decimal product = (decimal)prob_02_M[0, messageId] * keysSum;
                    result[messageId, column] = product;
                }
            }

            return result;
        }

        public static decimal[,] GetConditionalProbabilities()
        {
            var P_C = GetCiphertextsProbabilities();
            var P_M_C = GetCiphertextMessagesProbabilities();

            var result = new decimal[20, 20];

            for (int messageId = 0; messageId < 20; messageId++)
            {
                for (int cipherId = 0; cipherId < 20; cipherId++)
                {
                    result[messageId, cipherId] = P_M_C[messageId, cipherId] / P_C[cipherId,0];
                }
            }

            return result;
        }

        

    }
}
