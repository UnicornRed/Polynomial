using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixWork
{
    /// <summary>
    /// Реализует диагональные матрицы.
    /// </summary>
    /// <typeparam name="T">Тип коэффициентов матрицы.</typeparam>
    class MatrixDiagonal<T> : MatrixSquare<T>
                    where T : IComparable<T>
    {
        /// <summary>
        /// Инициализирует диагональную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Переданные коэффициенты матрицы.</param>
        public MatrixDiagonal(int height, SortedList<int, SortedList<int, T>> coeff) : base(height, coeff) { }

        /// <summary>
        /// Вычисляет определитель диагональной матрицы.
        /// </summary>
        /// <returns>Определитель матрицы.</returns>
        public override T MatrixDeterminant()
        {
            if (multy == null)
                throw new Exception("Multiplication is not defined for the data type of the matrix coefficients.");

            T determinant = DefaultT;

            foreach (var i in this)
            {
                if (i.Item1 == 0 && i.Item2 == 0)
                    determinant = this[0, 0];
                else
                    determinant = multy(determinant, this[i.Item1, i.Item2]);
            }

            return determinant;
        }
    }
}
