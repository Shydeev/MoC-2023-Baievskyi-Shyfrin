using ConsoleApp1.Functions;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var decision = DeterministicDecisionFunction.MakeDecision();

            Console.WriteLine(new Matrix(decision));

            Console.ReadLine();
        }
    }
}
