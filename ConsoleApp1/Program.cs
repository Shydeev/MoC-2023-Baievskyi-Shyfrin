using ConsoleApp1.Functions;
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var deterministicDecision = DeterministicDecisionFunction.MakeDecision();
            var deterministicLosses = DeterministicDecisionFunction.GetLossFunctionResult(deterministicDecision);
            Console.WriteLine("Deterministic Losses Function: {0}", deterministicLosses);


            var stochasticDecision = StochasticDecisionFunction.MakeDecision();
            var stochasticLosses = StochasticDecisionFunction.GetLossFunctionResult(stochasticDecision);
            Console.WriteLine("Stochastic Losses Function: {0}", stochasticLosses);

            Console.ReadLine();
        }
    }
}
