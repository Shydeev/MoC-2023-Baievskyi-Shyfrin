using System;

namespace ConsoleApp1.Models
{
    internal class SquareMatrix : Matrix
    {
        Double? _determinant;
        public Double Determinant
        {
            get
            {
                if (_determinant.HasValue)
                    return (Double)_determinant;

                if (this.Rows != this.Columns)
                    throw new Exception("It's impossible to find a determinant of non-square matrix.");

                Double determinant = 0;

                if (this.Rows == 1)
                    return this[0, 0];

                if (this.Rows == 2)
                    return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

                for (int N = 0; N < this.Rows; N++)
                {
                    var current_minor = new SquareMatrix(this.Rows - 1, this.Rows - 1);

                    for (int i = 0; i < this.Rows - 1; i++)
                        for (int j = 0; j < this.Rows; j++)
                            if (j < N)
                                current_minor[i, j] = this[i + 1, j];
                            else if (j > N)
                                current_minor[i, j - 1] = this[i + 1, j];

                    int minor_sign = (N % 2 == 0) ? 1 : -1;
                    determinant += minor_sign * this[0, N] * current_minor.Determinant;
                }

                _determinant = determinant;
                return determinant;
            }
        }
        public Boolean IsDiagonallyDominant
        {
            get 
            {
                for (int m = 0; m < this.Rows; m++)
                {
                    Double sum = 0;
                    
                    for (int n = 0; n < this.Columns; n++)
                        if (m != n)
                            sum += Math.Abs(this[m, n]);
                    
                    if (Math.Abs(this[m, m]) <= sum)
                        return false;
                }

                return true;
            }
        }

        #region Constructors
        public SquareMatrix(Int32 dimension) 
            : base(new Double[dimension, dimension]) { }
        public SquareMatrix(Int32 dimension, Double number) 
            : base(dimension, dimension, number) { }
        public SquareMatrix(Double[,] elements) 
            : base(elements) { }
        #endregion

        #region Overload operators
        public static SquareMatrix operator +(Double addend_a, SquareMatrix addend_b)
        {
            return addend_b + new SquareMatrix(addend_b.Rows, addend_a);
        }
        public static SquareMatrix operator +(SquareMatrix addend_a, Double addend_b)
        {
            return addend_a + new SquareMatrix(addend_a.Rows, addend_b);
        }
        public static SquareMatrix operator +(SquareMatrix addend_a, SquareMatrix addend_b)
        {
            if ((addend_a.Rows != addend_b.Rows) || (addend_a.Columns != addend_b.Columns))
                throw new Exception("Cannot compute sum, matrix dimensions do not match:\n\t" +
                    String.Format("First addend : {0}x{1}\n\t", addend_a.Rows, addend_a.Columns) +
                    String.Format("Second addend : {0}x{1}", addend_b.Rows, addend_b.Columns));

            var sum = new Double[addend_a.Rows, addend_a.Columns];

            for (int m = 0; m < addend_a.Rows; m++)
                for (int n = 0; n < addend_a.Columns; n++)
                    sum[m, n] = addend_a[m, n] + addend_b[m, n];

            return new SquareMatrix(sum);
        }
        public static SquareMatrix operator -(SquareMatrix minuend, Double subtrahend)
        {
            return minuend + new SquareMatrix(minuend.Rows, -subtrahend);
        }
        public static SquareMatrix operator -(SquareMatrix matrix)
        {
            var _matrix = new Double[matrix.Rows, matrix.Columns];

            for (int m = 0; m < matrix.Rows; m++)
                for (int n = 0; n < matrix.Columns; n++)
                    _matrix[m, n] = -matrix[m, n];

            return new SquareMatrix(_matrix);
        }
        public static SquareMatrix operator -(SquareMatrix minuend, SquareMatrix subtrahend)
        {
            return minuend + (-subtrahend);
        }
        public static SquareMatrix operator *(SquareMatrix multiplicand, SquareMatrix multyplier)
        {
            if (multiplicand.Columns != multyplier.Rows)
                throw new Exception("Can't multiply matrices because of dimensions.");

            var product = new Double[multiplicand.Rows, multyplier.Columns];

            for (int m = 0; m < multiplicand.Rows; m++)
                for (int n = 0; n < multyplier.Columns; n++)
                    for (int t = 0; t < multiplicand.Columns; t++)
                        product[m, n] += multiplicand[m, t] * multyplier[t, n];

            return new SquareMatrix(product);
        }
        public static SquareMatrix operator *(Double multiplicand, SquareMatrix multyplier)
        {
            var product = new Double[multyplier.Rows, multyplier.Columns];

            for (int m = 0; m < multyplier.Rows; m++)
                for (int n = 0; n < multyplier.Columns; n++)
                    product[m, n] = multiplicand * multyplier[m, n];

            return new SquareMatrix(product);
        }
        public static SquareMatrix operator /(SquareMatrix divident, SquareMatrix divisor)
        {
            return divident * (divisor ^ -1);
        }
        public static SquareMatrix operator ^(SquareMatrix matrix, Int32 exponent)
        {
            if (exponent == -1)
            {
                var _matrix = new Double[matrix.Rows, matrix.Rows];
                var determinant = matrix.Determinant;

                for (var i = 0; i < matrix.Rows; i++)
                    for (var j = 0; j < matrix.Columns; j++)
                        _matrix[i, j] = ((i + j) % 2 == 0 ? 1 : -1) * matrix.GetMinor(i, j) / determinant;

                return (new SquareMatrix(_matrix)).TransposeMatrix();
            }
            
            if (exponent < -1)
            {
                var neg_result = (1d / matrix.Determinant) * matrix.TransposeMatrix();
                var temp = (1d / matrix.Determinant) * matrix.TransposeMatrix();

                for (int q = 0; q < -exponent; q++)
                    neg_result *= temp;
                
                return neg_result;
            }

            if (exponent == 0)
            {
                var zero_result = new Double[matrix.Rows, matrix.Columns];

                for (int m = 0; m < matrix.Rows; m++)
                    for (int n = 0; n < matrix.Columns; n++)
                        if (m == n)
                            zero_result[m, n] = 1;
                        else
                            zero_result[m, n] = 0;

                return new SquareMatrix(zero_result);
            }

            var pos_result = matrix;

            for (int q = 0; q < exponent; q++)
                pos_result *= matrix;

            return pos_result;
        }

        #endregion

        public Double GetMinor(Int32 m, Int32 n)
        {
            if (this.Rows == 1)
                return this[0, 0];

            if (this.Rows == 2)
                return this[m == 0 ? 1 : 0, n == 0 ? 1 : 0];
                     
            var minor = new Double[this.Rows - 1, this.Columns - 1];

            for (int i = 0, j = 0; i < this.Rows; i++)
            {
                if (i == m)
                    continue;

                for (int k = 0, u = 0; k < this.Columns; k++)
                {
                    if (k == n)
                        continue;

                    minor[j, u] = this[i, k];
                    u++;
                }
                j++;
            }

            return new SquareMatrix(minor).Determinant;
        }
        public SquareMatrix TransposeMatrix()
        {
            var this_T = new double[this.Columns, this.Rows];

            for (int i = 0; i < this.Columns; i++)
                for (int j = 0; j < this.Rows; j++)
                    this_T[i, j] = this[j, i];
                
            return new SquareMatrix(this_T);
        }
    }
}
