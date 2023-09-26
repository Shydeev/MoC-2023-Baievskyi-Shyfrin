using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    // plural matrices
    public class Matrix
    {
        private readonly Double[,] _elements;
        public Double[,] Elements
        {
            get { return _elements; }
            set
            {
                for (int i = 0; i < _elements.GetLength(1); i++)
                    for (int j = 0; j < _elements.GetLength(0); j++)
                        _elements[i, j] = value[i, j];
            }
        }
        public Double this[Int32 index_row, Int32 index_column]
        {
            get { return this._elements[index_row, index_column]; }
            set { this._elements[index_row, index_column] = value; }
        }
        public Int32 Rows { get { return this.Elements.GetLength(0); } }
        public Int32 Columns { get { return this.Elements.GetLength(1); } }

        #region Constructors
        public Matrix() { }
        public Matrix(Int32 dimension) : this(new Double[dimension, dimension]) { }
        public Matrix(Int32 row_dimension, Int32 column_dimension) : this(new Double[row_dimension, column_dimension]) { }
        public Matrix(Int32 row_dimension, Int32 column_dimension, Double number)
        {
            var elements = new Double[row_dimension, column_dimension];

            for (int m = 0; m < elements.GetLength(0); m++)
                for (int n = 0; n < elements.GetLength(1); n++)
                    elements[m, n] = number;

            _elements = elements;
        }
        public Matrix(Double[,] elements)
        {
            _elements = elements;
        }
        #endregion

        #region Overrided methods from System.Object
        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
                return false;

            Matrix matrix = (Matrix)obj;

            if (this.Rows != matrix.Rows || this.Columns != matrix.Columns)
                return false;

            for (int m = 0; m <= this.Rows; m++)
                for (int n = 0; n < this.Columns; m++)
                    if (!this._elements[m, n].Equals(matrix._elements[m, n]))
                        return false;

            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            //if (ForToString != null)
            //    return ForToString;

            var max_unit = 0;
            var max_mantissa = 0;

            foreach (var element in this.Elements)
            {
                var current_element = element.ToString().Split('.');

                if (current_element[0].Length > max_unit)
                    max_unit = current_element[0].Length;

                if (current_element.Length == 2)
                    if (current_element[1].Length > max_mantissa)
                        max_mantissa = current_element[1].Length;
            }

            var length = this._elements.GetLength(1) * (max_unit + max_mantissa + 2 + (max_mantissa == 0 ? 0 : 1)) - 4;
            length = length < 0 ? 0 : length;

            var matrix = "---" + String.Concat(Enumerable.Repeat(" ", length)) + "---\n";

            for (int m = 0; m < this.Rows; m++)
            {
                matrix += "| ";

                for (int n = 0; n < this.Columns; n++)
                {
                    var element = this._elements[m, n].ToString().Split('.');

                    var unit = String.Concat(Enumerable.Repeat(" ", max_unit - element[0].Length)) + element[0];

                    String mantissa;

                    if (element.Length == 2)
                    {
                        mantissa = "." + element[1] + String.Concat(Enumerable.Repeat(" ", max_mantissa - element[1].Length)) + "  ";
                    }
                    else
                    {
                        if (max_mantissa == 0)
                            mantissa = "  ";
                        else
                            mantissa = "." + String.Concat(Enumerable.Repeat("0", max_mantissa)) + "  ";
                    }

                    matrix += String.Format("{0}{1}", unit, mantissa);
                }
                matrix = matrix.Remove(matrix.Length - 1, 1);

                if (m != this.Rows - 1)
                    matrix += "|\n|" + String.Concat(Enumerable.Repeat(" ", length + 4)) + "|\n";
                else
                    matrix += "|\n";
            }

            matrix += "---" + String.Concat(Enumerable.Repeat(" ", length)) + "---\n";

            //ForToString = matrix;

            return matrix;
        }
        #endregion

        #region Overload operators
        public static Matrix operator +(Double addend_a, Matrix addend_b)
        {
            return addend_b + new Matrix(addend_b.Rows, addend_b.Columns, addend_a);
        }
        public static Matrix operator +(Matrix addend_a, Double addend_b)
        {
            return addend_a + new Matrix(addend_a.Rows, addend_a.Columns, addend_b);
        }
        public static Matrix operator +(Matrix addend_a, Matrix addend_b)
        {
            if ((addend_a.Rows != addend_b.Rows) || (addend_a.Columns != addend_b.Columns))
                throw new Exception("Cannot compute sum, matrix dimensions do not match:\n\t" +
                    String.Format("First addend : {0}x{1}\n\t", addend_a.Rows, addend_a.Columns) +
                    String.Format("Second addend : {0}x{1}", addend_b.Rows, addend_b.Columns));

            var sum = new Matrix(addend_a.Rows, addend_a.Columns);

            for (int m = 0; m < addend_a.Rows; m++)
                for (int n = 0; n < addend_a.Columns; n++)
                    sum[m, n] = addend_a[m, n] + addend_b[m, n];

            return sum;
        }
        public static Matrix operator -(Matrix minuend, Double subtrahend)
        {
            return minuend + new Matrix(minuend.Rows, minuend.Columns, -subtrahend);
        }
        public static Matrix operator -(Matrix matrix)
        {
            var _matrix = new Matrix(matrix.Rows, matrix.Columns);

            for (int m = 0; m < matrix.Rows; m++)
                for (int n = 0; n < matrix.Columns; n++)
                    _matrix[m, n] = -matrix[m, n];

            return _matrix;
        }
        public static Matrix operator -(Matrix minuend, Matrix subtrahend)
        {
            return minuend + (-subtrahend);
        }
        public static Matrix operator *(Matrix multiplicand, Matrix multyplier)
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
        public static Matrix operator *(Double multiplicand, Matrix multyplier)
        {
            var product = new Matrix(multyplier.Rows, multyplier.Columns);

            for (int m = 0; m < product.Rows; m++)
                for (int n = 0; n < product.Columns; n++)
                    product[m, n] = multiplicand * multyplier[m, n];

            return product;
        }

        #endregion

        public virtual Matrix Transpose()
        {
            var this_T = new Double[this.Columns, this.Rows];

            for (int i = 0; i < this.Columns; i++)
                for (int j = 0; j < this.Rows; j++)
                    this_T[i, j] = this[j, i];

            return new Matrix(this_T);
        }
        public void SwapRows(Int32 i, Int32 j)
        {
            var temp = new Double[this.Rows, this.Columns];

            for (int m = 0; m < this.Rows; m++)
            {
                for (int n = 0; n < this.Columns; n++)
                {
                    if (m == i)
                    {
                        temp[m, n] = this[j, n];
                    }
                    else if (m == j)
                    {
                        temp[m, n] = this[i, n];
                    }
                    else
                    {
                        temp[m, n] = this[m, n];
                    }
                }
            }

            this.Elements = temp;
        }
        public void SwapColomns(Int32 i, Int32 j)
        {
            var temp = new Double[this.Rows, this.Columns];

            for (int m = 0; m < this.Rows; m++)
            {
                for (int n = 0; n < this.Columns; n++)
                {
                    if (n == i)
                    {
                        temp[m, n] = this[m, j];
                    }
                    else if (n == j)
                    {
                        temp[m, n] = this[m, i];
                    }
                    else
                    {
                        temp[m, n] = this[m, n];
                    }
                }
            }

            this.Elements = temp;
        }
        public void AddColumns(Int32 i, Int32 j, Double number)
        {
            for (int m = i; m < i + 1; m++)
            {
                for (int n = 0; n < this.Rows; n++)
                {
                    this[m, n] += this[j, n] * number;
                }
            }
        }

    }
}
