using System;
using System.Collections.Generic;

namespace MatrixWork
{
    /// <summary>
    /// Реализует вычислимые матрицы.
    /// </summary>
    /// <typeparam name="T">Тип коэффициентов матрицы.</typeparam>
    public class MatrixCalculated<T> : Matrix<T>
                      where T : IComparable<T>
    {
        /// <summary>
        /// Бинарная операция для вычисления коэффициентов.
        /// </summary>
        /// <param name="Oper1">Первый аргумент операции.</param>
        /// <param name="Oper2">Второй аргумент операции.</param>
        /// <returns>Результат операции.</returns>
        public delegate T BinaryOp(T Oper1, T Oper2);

        /// <summary>
        /// Сложение коэффициентов.
        /// </summary>
        protected static BinaryOp add = null;

        /// <summary>
        /// Умножение коэффициентов.
        /// </summary>
        protected static BinaryOp multy = null;

        /// <summary>
        /// Вычитание коэффициентов.
        /// </summary>
        protected static BinaryOp sub = null;

        /// <summary>
        /// Инициализирует вычислимую матрицу по умолчанию.
        /// </summary>
        public MatrixCalculated() : base() { }

        /// <summary>
        /// Инициализирует вычислимую матрицу по высоте, ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Высота матрицы.</param>
        /// <param name="width">Ширина матрицы.</param>
        /// <param name="coeff">Переданные коэффициенты матрицы.</param>
        public MatrixCalculated(int height, int width, SortedList<int, SortedList<int, T>> coeff) : base(height, width, coeff) { }

        /// <summary>
        /// Инициализирует вычислимую матрицу по другой матрице.
        /// </summary>
        /// <param name="matrixCalculated">Переданная матрица.</param>
        public MatrixCalculated(MatrixCalculated<T> matrixCalculated) : base(matrixCalculated) { }

        /// <summary>
        /// Устанавливает сложение для коэффициентов.
        /// </summary>
        /// <param name="add">Операция сложения.</param>
        public static void AddDelegate(BinaryOp add) => MatrixCalculated<T>.add = add;

        /// <summary>
        /// Устанавливает умножение для коэффициентов.
        /// </summary>
        /// <param name="multy">Операция умножения.</param>
        public static void MultyDelegate(BinaryOp multy) => MatrixCalculated<T>.multy = multy;

        /// <summary>
        /// Устанавливает вычитание для коэффициентов.
        /// </summary>
        /// <param name="sub">Операция вычитания.</param>
        public static void SubDelegate(BinaryOp sub) => MatrixCalculated<T>.sub = sub;

        /// <summary>
        /// Сложение матриц.
        /// </summary>
        /// <param name="matrix1">Первая матрица.</param>
        /// <param name="matrix2">Вторая матрица.</param>
        /// <returns>Результат сложения.</returns>
        public static MatrixCalculated<T> operator + (MatrixCalculated<T> matrix1, MatrixCalculated<T> matrix2)
        {
            if (add == null)
                throw new Exception("Addition is not defined for the data type of the matrix coefficients.");

            if (matrix1.height != matrix2.height || matrix1.width != matrix2.width)
                throw new Exception("The sizes of the matrices do not match.");

            MatrixCalculated<T> matrixResult = new MatrixCalculated<T>(matrix1);

            foreach (var i in matrix2)
                matrixResult[i.Item1, i.Item2] = add(matrixResult[i.Item1, i.Item2], matrix2[i.Item1, i.Item2]);

            return matrixResult;
        }

        /// <summary>
        /// Вычитание матриц.
        /// </summary>
        /// <param name="matrix1">Первая матрица.</param>
        /// <param name="matrix2">Вторая матрица.</param>
        /// <returns>Результат вычитания.</returns>
        public static MatrixCalculated<T> operator - (MatrixCalculated<T> matrix1, MatrixCalculated<T> matrix2)
        {
            if (sub == null)
                throw new Exception("Subtraction is not defined for the data type of the matrix coefficients.");

            if (matrix1.height != matrix2.height || matrix1.width != matrix2.width)
                throw new Exception("The sizes of the matrices do not match.");

            MatrixCalculated<T> matrixResult = new MatrixCalculated<T>(matrix1);

            foreach (var i in matrix2)
                matrixResult[i.Item1, i.Item2] = sub(matrixResult[i.Item1, i.Item2], matrix2[i.Item1, i.Item2]);

            return matrixResult;
        }

        /// <summary>
        /// Умножение матриц.
        /// </summary>
        /// <param name="matrix1">Первая матрица.</param>
        /// <param name="matrix2">Вторая матрица.</param>
        /// <returns>Результат умножения.</returns>
        public static MatrixCalculated<T> operator * (MatrixCalculated<T> matrix1, MatrixCalculated<T> matrix2)
        {
            if (multy == null || add == null)
                throw new Exception("Subtraction and multiplication is not defined for the data type of the matrix coefficients.");

            if (matrix1.width != matrix2.height)
                throw new Exception("It is impossible to multiply matrices due to size mismatch.");

            MatrixCalculated<T> matrixResult = new MatrixCalculated<T>(matrix1.height, matrix2.width, new SortedList<int, SortedList<int, T>>());
            matrixResult.DefaultT = matrix1.DefaultT;

            for (int i = 0; i < matrix1.height; i++)
                for (int j = 0; j < matrix2.width; j++)
                {
                    matrixResult[i, j] = matrix1.DefaultT;

                    for (int l = 0; l < matrix1.width; l++)
                        matrixResult[i, j] = add(matrixResult[i, j], multy(matrix1[i, l], matrix2[l, j]));
                }

            return matrixResult;
        }

        /// <summary>
        /// Инициализирует матрицу по переданной матрице без строки i и столбца j.
        /// </summary>
        /// <param name="notIJ">Индекс элемента пересечения i и j.</param>
        /// <param name="matrix">Переданная матрица.</param>
        public MatrixCalculated((int, int) notIJ, MatrixCalculated<T> matrix)
        {
            this.coeff = new SortedList<int, SortedList<int, T>>();
            int indexI, indexJ;

            foreach (var i in matrix)
            {
                indexI = i.Item1;
                indexJ = i.Item2;

                if (i.Item1 > notIJ.Item1)
                    indexI = i.Item1 - 1;
                else if (i.Item1 == notIJ.Item1)
                    continue;

                if (i.Item2 > notIJ.Item2)
                    indexJ = i.Item2 - 1;
                else if (i.Item2 == notIJ.Item2)
                    continue;

                if (!coeff.Keys.Contains(indexI))
                    this.coeff.Add(indexI, new SortedList<int, T>());

                this.coeff[indexI].Add(indexJ, matrix[i.Item1, i.Item2]);
            }

            this.height = matrix.height - 1;
            this.width = matrix.width - 1;
        }

        /// <summary>
        /// Рекурентное вычисление определителя матрицы.
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

                    MatrixCalculated<T> helper = new MatrixCalculated<T>((0, i.Item2), this);
                    helper.DefaultT = DefaultT;

                    if (i.Item2 % 2 == 0)
                        determinant = add(determinant, multy(this[0, i.Item2], helper.MatrixDeterminantR()));
                    else
                        determinant = sub(determinant, multy(this[0, i.Item2], helper.MatrixDeterminantR()));
                }

                return determinant;
            }
        }

        /// <summary>
        /// Вычисляет определитель матрицы.
        /// </summary>
        /// <returns>Определитель матрицы.</returns>
        public virtual T MatrixDeterminant()
        {
            if (add == null || multy == null || sub == null)
                throw new Exception("Addition, subtraction or multiplication is not defined for the data type of the matrix coefficients.");

            if (this.width != this.height)
                throw new Exception("The height must be equal to the width.");

            return this.MatrixDeterminantR();
        }
    }
}
