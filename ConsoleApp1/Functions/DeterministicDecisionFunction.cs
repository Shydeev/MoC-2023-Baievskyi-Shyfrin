using ConsoleApp1.Models;
using ConsoleApp1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Functions
{
    public static class DeterministicDecisionFunction
    {
        public static double[,] MakeDecision()
        {
            var result = new double[20,20];

            // M_{i}
            for (int i = 0; i < 20; i++)
            {
                // C_{j} = j
                for (int j = 0; j < 20; j++)
                {
                    // Key - k
                    for (int k = 0; k < 20; k++)
                    {
                        if (Variant.table_02[k, i] == j)
                        {
                            // Console.WriteLine("i = {0}\nj = {1}\nk = {2}\nP(M_{0}, C_{1}) = {3}", i,j,k, Variant.table_02[k, i]);
                            // Console.WriteLine("Result += {0} * {1}", Variant.prob_02_M[0, i], Variant.prob_02_K[0, k]);
                            result[j, i] += Variant.prob_02_M[0, i] * Variant.prob_02_K[0, k];
                        }
                    }
                    //Console.WriteLine(result);
                }
            }

            return result;  
        }
    }
}
