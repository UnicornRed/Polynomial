using Polynomials;
using System.Collections.Generic;
using MatrixWork;
using System;

namespace MatrixType
{
    /// <summary>
    /// Реализует целочисленные матрицы.
    /// </summary>
    class MatrixInt : MatrixCalculated<int>
    {
        /// <summary>
        /// Инициализирует целочисленную матрицу по высоте, ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Высота матрицы.</param>
        /// <param name="width">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixInt(int height, int width, SortedList<int, SortedList<int, int>> coeff) : base(height, width, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует целочисленные квадратные матрицы.
    /// </summary>
    class MatrixSquareInt : MatrixSquare<int>
    {
        /// <summary>
        /// Инициализирует целочисленную квадратную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixSquareInt(int height, SortedList<int, SortedList<int, int>> coeff) : base(height, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует целочисленные диагональные матрицы.
    /// </summary>
    class MatrixDiagonalInt : MatrixDiagonal<int>
    {
        /// <summary>
        /// Инициализирует целочисленную диагональную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixDiagonalInt(int height, SortedList<int, SortedList<int, int>> coeff) : base(height, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует вещественные матрицы.
    /// </summary>
    class MatrixDouble : MatrixCalculated<double>
    {
        /// <summary>
        /// Инициализирует вещественную матрицу по высоте, ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Высота матрицы.</param>
        /// <param name="width">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixDouble(int height, int width, SortedList<int, SortedList<int, double>> coeff) : base(height, width, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует вещественные квадратные матрицы.
    /// </summary>
    class MatrixSquareDouble : MatrixSquare<double>
    {
        /// <summary>
        /// Инициализирует вещественную квадратную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixSquareDouble(int height, SortedList<int, SortedList<int, double>> coeff) : base(height, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует вещественные диагональные матрицы.
    /// </summary>
    class MatrixDiagonalDouble : MatrixDiagonal<double>
    {
        /// <summary>
        /// Инициализирует вещественную диагональную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixDiagonalDouble(int height, SortedList<int, SortedList<int, double>> coeff) : base(height, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует полиномиальные матрицы.
    /// </summary>
    public class MatrixPolynomial : MatrixCalculated<Polynomial>
    {
        /// <summary>
        /// Значение коэффициентов по умолчанию.
        /// </summary>
        protected override Polynomial DefaultT { get => new Polynomial(); }

        /// <summary>
        /// Инициализирует полиномиальную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixPolynomial(int height, int width, SortedList<int, SortedList<int, Polynomial>> coeff) : base(height, width, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }

        /// <summary>
        /// Инициализирует полиномиальную матрицу по переданной матрице.
        /// </summary>
        /// <param name="matrix">Переданная матрица.</param>
        public MatrixPolynomial(MatrixPolynomial matrix) : base(matrix)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }

        /// <summary>
        /// Инициализирует полиномиальную матрицу по переданной вычислимой от полиномов матрице.
        /// </summary>
        /// <param name="matrix">Переданная вычислимая от полиномов матрица.</param>
        private MatrixPolynomial(MatrixCalculated<Polynomial> matrix) : base(matrix)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }

        /// <summary>
        /// Сложение полиномиальных матриц.
        /// </summary>
        /// <param name="matrix1">Первая матрица.</param>
        /// <param name="matrix2">Вторая матрица</param>
        /// <returns>Результат сложения.</returns>
        public static MatrixPolynomial operator +(MatrixPolynomial matrix1, MatrixPolynomial matrix2) =>
            new MatrixPolynomial((MatrixCalculated<Polynomial>)matrix1 + (MatrixCalculated<Polynomial>)matrix2);

        /// <summary>
        /// Вычетание полиномиальных матриц.
        /// </summary>
        /// <param name="matrix1">Первая матрица.</param>
        /// <param name="matrix2">Вторая матрица</param>
        /// <returns>Результат вычетания.</returns>
        public static MatrixPolynomial operator -(MatrixPolynomial matrix1, MatrixPolynomial matrix2) =>
            new MatrixPolynomial((MatrixCalculated<Polynomial>)matrix1 - (MatrixCalculated<Polynomial>)matrix2);

        /// <summary>
        /// Умножение полиномиальных матриц.
        /// </summary>
        /// <param name="matrix1">Первая матрица.</param>
        /// <param name="matrix2">Вторая матрица</param>
        /// <returns>Результат умножения.</returns>
        public static MatrixPolynomial operator *(MatrixPolynomial matrix1, MatrixPolynomial matrix2) =>
            new MatrixPolynomial((MatrixCalculated<Polynomial>)matrix1 * (MatrixCalculated<Polynomial>)matrix2);

        /// <summary>
        /// Умножение на скаляр матриц слева.
        /// </summary>
        /// <param name="scal">Скаляр.</param>
        /// <param name="matrix">Матрица.</param>
        /// <returns>Матрицу, умноженную на скаляр.</returns>
        public static MatrixPolynomial operator *(double scal, MatrixPolynomial matrix)
        {
            MatrixPolynomial matrixRes = new MatrixPolynomial(matrix);

            for (int i = 0; i < matrixRes.width; i++)
                for (int j = 0; j < matrixRes.height; j++)
                    matrixRes[i, j] *= scal;

            return matrixRes;
        }

        /// <summary>
        /// Умножение на скаляр матриц справа.
        /// </summary>
        /// <param name="scal">Скаляр.</param>
        /// <param name="matrix">Матрица.</param>
        /// <returns>Матрицу, умноженную на скаляр.</returns>
        public static MatrixPolynomial operator *(MatrixPolynomial matrix, double scal) => scal * matrix;

        /// <summary>
        /// Вычисление характеристического полинома матриц.
        /// </summary>
        /// <returns>Характеристический полином.</returns>
        public Polynomial CharactPolynomial()
        {
            if (this.height != this.width)
                throw new Exception("The matrix should be square for characteristic polynomial.\n");

            for (int i = 0; i < this.width; i++)
                for (int j = 0; j < this.height; j++)
                    if (this[i, j].Deg > 0)
                        throw new Exception("The matrix should have only polynomials with a coefficient of 0 degree.\n");

            MatrixPolynomial chartMatrix = new MatrixPolynomial(this.height, this.height, this.coeff);
            Polynomial helper = new Polynomial("x");

            for (int i = 0; i < this.height; i++)
                chartMatrix[i, i] -= helper;

             return chartMatrix.MatrixDeterminant();
        }

        /// <summary>
        /// Подстановка матрицы в полином.
        /// </summary>
        /// <param name="pol">Переданный полином.</param>
        /// <returns>Матрицу - результат подстановки.</returns>
        public MatrixPolynomial Eval(Polynomial pol)
        {
            if (this.height != this.width)
                throw new Exception("The matrix should be square for eval to polynomial.\n");

            SortedList<int, SortedList<int, Polynomial>> coeffs = new SortedList<int, SortedList<int, Polynomial>>();
            Polynomial one = new Polynomial("1");

            for (int i = 0; i < this.width; i++)
            {
                coeffs.Add(i, new SortedList<int, Polynomial>());
                coeffs[i].Add(i, one);
            }

            MatrixPolynomial neutralMatr = new MatrixPolynomial(this.height, this.width, coeffs);
            ArithmeticType<MatrixPolynomial> at = new ArithmeticType<MatrixPolynomial>((x, y) => x * y, (x, y) => x + y, (a, x) => a * x, neutralMatr);

            return pol.Eval<MatrixPolynomial>(this, at);
        }
    }

    /// <summary>
    /// Реализует полиномиальные квадратные матрицы.
    /// </summary>
    class MatrixSquarePolynomial : MatrixSquare<Polynomial>
    {
        /// <summary>
        /// Значение коэффициентов по умолчанию.
        /// </summary>
        protected override Polynomial DefaultT { get => new Polynomial(); }

        /// <summary>
        /// Инициализирует полиномиальную квадратную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixSquarePolynomial(int height, SortedList<int, SortedList<int, Polynomial>> coeff) : base(height, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }

    /// <summary>
    /// Реализует полиномиальные диагональные матрицы.
    /// </summary>
    class MatrixDiagonalPolynomial : MatrixDiagonal<Polynomial>
    {
        /// <summary>
        /// Значение коэффициентов по умолчанию.
        /// </summary>
        protected override Polynomial DefaultT { get => new Polynomial(); }

        /// <summary>
        /// Инициализирует полиномиальную диагональную матрицу по ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Ширина матрицы.</param>
        /// <param name="coeff">Коэффициенты матрицы.</param>
        public MatrixDiagonalPolynomial(int height, SortedList<int, SortedList<int, Polynomial>> coeff) : base(height, coeff)
        {
            AddDelegate((x, y) => x + y);
            MultyDelegate((x, y) => x * y);
            SubDelegate((x, y) => x - y);
        }
    }
}
