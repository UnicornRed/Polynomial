using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixWork
{
    /// <summary>
    /// Реализует квадратные матрицы.
    /// </summary>
    /// <typeparam name="T">Тип коэффициентов матрицы.</typeparam>
    class MatrixSquare<T> : MatrixCalculated<T>
                  where T : IComparable<T>
    {
        /// <summary>
        /// Инициализирует диагональную матрицу по умолчанию.
        /// </summary>
        public MatrixSquare() : base() { }

        /// <summary>
        /// Инициализирует квадратную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Переданные коэффициенты матрицы.</param>
        public MatrixSquare(int height, SortedList<int, SortedList<int, T>> coeff) : base(height, height, coeff) { }

        /// <summary>
        /// Инициализирует квадратную матрицу по переданной матрице без строки i и столбца j.
        /// </summary>
        /// <param name="notIJ">Индекс элемента пересечения i и j.</param>
        /// <param name="matrix">Переданная матрица.</param>
        public MatrixSquare((int, int) notIJ, MatrixSquare<T> matrix) : base(notIJ, matrix) { }

        /// <summary>
        /// Рекурентное вычисление определителя квадратной матрицы.
        /// </summary>
        /// <returns>Определитель матрицы.</returns>
        private T MatrixDeterminantR()
        {
            if (this.height == 1)
                return this[0, 0];
            else
            {
                T determinant = DefaultT;

                foreach (var i in this)
                {
                    if (i.Item1 > 0)
                        break;

                    if (i.Item2 % 2 == 0)
                        determinant = add(determinant, multy(this[0, i.Item2], (new MatrixSquare<T>((0, i.Item2), this)).MatrixDeterminantR()));
                    else
                        determinant = sub(determinant, multy(this[0, i.Item2], (new MatrixSquare<T>((0, i.Item2), this)).MatrixDeterminantR()));
                }

                return determinant;
            }
        }

        /// <summary>
        /// Вычисляет определитель квадратной матрицы.
        /// </summary>
        /// <returns>Определитель матрицы.</returns>
        public override T MatrixDeterminant()
        {
            if (add == null || multy == null || sub == null)
                throw new Exception("Addition, subtraction or multiplication is not defined for the data type of the matrix coefficients.");

            return this.MatrixDeterminantR();
        }
    }
}
