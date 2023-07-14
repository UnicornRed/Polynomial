using System;
using System.Collections.Generic;
using System.Text;
using MatrixEnum;

namespace MatrixWork
{
    /// <summary>
    /// Реализует обработку матриц общего типа.
    /// </summary>
    /// <typeparam name="T">Тип коэффициентов матрицы.</typeparam>
    public class Matrix<T> where T : IComparable<T>
    {
        /// <summary>
        /// Высота матрицы.
        /// </summary>
        protected int height;

        /// <summary>
        /// Ширина матрицы.
        /// </summary>
        protected int width;

        /// <summary>
        /// Значение коэффициентов по умолчанию.
        /// </summary>
        protected virtual T DefaultT { get; set; }

        /// <summary>
        /// Коэффициенты матрицы.
        /// </summary>
        protected SortedList<int, SortedList<int, T>> coeff;

        /// <summary>
        /// Доступ к коэффициентам матрицы по индексам.
        /// </summary>
        /// <param name="i">Первый индекс.</param>
        /// <param name="j">Второй индекс.</param>
        /// <returns>Коэффициент матрицы по индексам.</returns>
        public T this[int i, int j]
        {
            get
            {
                if (coeff.ContainsKey(i) && coeff[i].ContainsKey(j))
                    return coeff[i][j];
                else
                    return DefaultT;
            }
            protected set
            {
                if (i >= this.height || j >= this.width)
                    throw new Exception("The index of the element exceeds the size of the matrix.\n");

                if (!coeff.ContainsKey(i))
                {
                    coeff.Add(i, new SortedList<int, T>());
                    coeff[i].Add(j, value);
                }
                else
                {
                    if (coeff[i].ContainsKey(j))
                        coeff[i][j] = value;
                    else
                        coeff[i].Add(j, value);
                }

                if (coeff[i][j].CompareTo(DefaultT) == 0)
                {
                    coeff[i].Remove(j);

                    if (coeff[i].Count == 0)
                        coeff.Remove(i);
                }
            }
        }

        /// <summary>
        /// Инициализирует матрицу по умолчанию.
        /// </summary>
        public Matrix()
        {
            height = 0;
            width = 0;
            DefaultT = default(T);
            coeff = new SortedList<int, SortedList<int, T>>();
        }

        /// <summary>
        /// Инициализирует матрицу по высоте, ширине и коэффициентам.
        /// </summary>
        /// <param name="height">Высота матрицы.</param>
        /// <param name="width">Ширина матрицы.</param>
        /// <param name="coeff">Переданные коэффициенты матрицы.</param>
        public Matrix(int height, int width, SortedList<int, SortedList<int, T>> coeff)
        {
            this.coeff = new SortedList<int, SortedList<int, T>>();
            
            foreach (var i in coeff)
            {
                if (i.Value.Count != 0 && i.Key < height)
                {
                    this.coeff.Add(i.Key, new SortedList<int, T>());

                    foreach (var j in coeff[i.Key])
                    {
                        if (j.Key < width && j.Value.CompareTo(DefaultT) != 0)
                            this.coeff[i.Key].Add(j.Key, coeff[i.Key][j.Key]);
                    }
                }
            }

            DefaultT = default(T);
            this.height = height;
            this.width = width;
        }

        /// <summary>
        /// Инициализирует матрицу по другой матрице.
        /// </summary>
        /// <param name="matrix">Переданная матрица.</param>
        public Matrix(Matrix<T> matrix)
        {
            this.coeff = new SortedList<int, SortedList<int, T>>();
            
            foreach (var i in matrix)
            {
                if (!coeff.Keys.Contains(i.Item1))
                    this.coeff.Add(i.Item1, new SortedList<int, T>());

                this.coeff[i.Item1].Add(i.Item2, matrix[i.Item1, i.Item2]);
            }

            this.DefaultT = matrix.DefaultT;
            this.height = matrix.height;
            this.width = matrix.width;
        }

        /// <summary>
        /// Устанавливает значения коэффициента матрицы.
        /// </summary>
        /// <param name="i">Первый индекс.</param>
        /// <param name="j">Второй индекс.</param>
        /// <param name="coeff">Устанавливаемое значение.</param>
        public void SetIJ(int i, int j, T coeff) => this[i, j] = coeff;

        /// <summary>
        /// Переопределение вывода матрицы.
        /// </summary>
        /// <returns>Строку с матрицей.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder("");
            
            for (int i = 0; i < height; i ++)
            {
                for (int j = 0; j < width; j++)
                    str.Append(this[i, j].ToString() + " ");

                str.Append("\n");
            }

            return str.ToString();
        }

        /// <summary>
        /// Определяет перечисление коэффициентов матрицы.
        /// </summary>
        /// <returns>Перечислитель коэффициентов.</returns>
        public IEnumerator<(int, int)> GetEnumerator()
        {
            return new MatrixEnumerator<T>(coeff);
        }
    }
}
