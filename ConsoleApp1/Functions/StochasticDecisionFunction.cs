using ConsoleApp1.Data;

namespace ConsoleApp1.Functions
{
    public static class StochasticDecisionFunction
    {
        public static decimal[,] MakeDecision()
        {
            var result = new decimal[20, 20];

            var conditional_prob = Variant.GetConditionalProbabilities();

            for (int column = 0; column < 20; column++)
            {
                var max_value_in_column = 0m;
                var max_entrance = 0m;

                for (int row = 0; row < 20; row++)
                {
                    if (max_value_in_column < conditional_prob[row, column])
                    {
                        max_value_in_column = conditional_prob[row, column];
                    }
                }

                for (int row = 0; row < 20; row++)
                {
                    if (max_value_in_column == conditional_prob[row, column])
                    {
                        max_entrance++;
                    }
                }

                for (int row = 0; row < 20; row++)
                {
                    result[column, row] = conditional_prob[row, column] == max_value_in_column
                                            ? 1m / max_entrance
                                            : 0m;
                }
            }

            return result;
        }

        public static decimal GetLossFunctionResult(decimal[,] decision)
        {
            var result = 0m;

            var P_M_C = Variant.GetCiphertextMessagesProbabilities();

            for (int messageId = 0; messageId < 20; messageId++)
            {
                for (int cipherId = 0; cipherId < 20; cipherId++)
                {
                    result += P_M_C[messageId, cipherId] * GetLossFunctionHelperResult(messageId, cipherId, decision);
                }
            }

            return result;
        }

        public static decimal GetLossFunctionHelperResult(int _messageId, int _cipherId, decimal[,] decision)
        {
            var result = 0m;

            for (int messageId = 0; messageId < 20; messageId++)
            {
                if (messageId != _messageId)
                {
                    result += decision[_cipherId, messageId];
                }
            }

            return result;
        }
    }
}
