using ConsoleApp1.Data;

namespace ConsoleApp1.Functions
{
    public static class DeterministicDecisionFunction
    {
        public static int[] MakeDecision()
        {
            var result = new int[20];

            var conditional_probabilities = Variant.GetConditionalProbabilities();

            var max = new decimal[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int column = 0; column < 20; column++)
            {
                for (int row = 0; row < 20; row++)
                {
                    if (conditional_probabilities[row, column] > max[column])
                    {
                        max[column] = conditional_probabilities[row, column];
                        result[column] = row;
                    }
                }
            }

            return result;
        }

        public static decimal GetLossFunctionResult(int[] decision)
        {
            var result = 0m;

            var P_M_C = Variant.GetCiphertextMessagesProbabilities();

            for (int messageId = 0; messageId < 20; messageId++)
            {
                for (int cipherId = 0; cipherId < 20; cipherId++)
                {
                    result += P_M_C[messageId, cipherId] * (decision[cipherId] == messageId ? 0 : 1);
                }
            }

            return result;
        }
    }
}
