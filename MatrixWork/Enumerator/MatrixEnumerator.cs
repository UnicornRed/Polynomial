using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MatrixEnum
{
    /// <summary>
    /// Реализует интерфейс перечисления.
    /// </summary>
    /// <typeparam name="T">Тип коэффициентов матрицы.</typeparam>
    class MatrixEnumerator<T> : IEnumerator<(int, int)>
    {
        /// <summary>
        /// Коэффициенты матрицы.
        /// </summary>
        private SortedList<int, SortedList<int, T>> coeff;

        /// <summary>
        /// Индексы строки и столбца матрицы.
        /// </summary>
        int i = -1, j = -1;

        /// <summary>
        /// Инициализирует перечислитель по переданным коэффициентам.
        /// </summary>
        /// <param name="coeff">Переданные коэффициенты матрицы.</param>
        public MatrixEnumerator(SortedList<int, SortedList<int, T>> coeff)
        {
            this.coeff = coeff;
        }

        /// <summary>
        /// Текущий элемент.
        /// </summary>
        public (int, int) Current
        {
            get
            {
                if (i > coeff.Keys.LastOrDefault() || j > coeff[i].Keys.LastOrDefault())
                    throw new InvalidOperationException();
                return (i, j);
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        /// <summary>
        /// Получение следующего элемента.
        /// </summary>
        /// <returns>Следующий элемент матрицы.</returns>
        public bool MoveNext()
        {
            if (coeff.Keys.Count != 0 && i <= coeff.Keys.LastOrDefault())
            {
                if (j == -1 && i == -1)
                {
                    i = coeff.Keys.FirstOrDefault();
                    j = coeff[i].Keys.FirstOrDefault();
                }
                else
                {
                    if (j < coeff[i].Keys.LastOrDefault())
                        j = coeff[i].Keys[coeff[i].Keys.IndexOf(j) + 1];
                    else
                    {
                        if (i < coeff.Keys.LastOrDefault())
                        {
                            i = coeff.Keys[coeff.Keys.IndexOf(i) + 1];
                            j = coeff[i].Keys.FirstOrDefault();
                        }
                        else
                            return false;
                        
                    }
                }

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Сброс перечисления.
        /// </summary>
        public void Reset()
        {
            i = -1;
            j = -1;
        }
        public void Dispose() { }
    }
}
