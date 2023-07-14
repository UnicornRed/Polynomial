using System.Collections.Generic;
using Polynomials;

namespace PolynomialComparers
{
    /// <summary>
    /// Реализует интерфейс сравнения полиномов.
    /// </summary>
    public class PolynomialComparer : IComparer<Polynomial>
    {
        /// <summary>
        /// Сравнивает полиномы.
        /// </summary>
        /// <param name="pol1">Первый полином.</param>
        /// <param name="pol2">Второй полином.</param>
        /// <returns>1, если первый полином больше второго, -1, если первый полином меньше второго, 0, если они равны.</returns>
        public int Compare(Polynomial pol1, Polynomial pol2)
        {
            if (pol1.Deg > pol2.Deg)
                return 1;
            else if (pol1.Deg < pol2.Deg)
                return -1;
            else
            {
                for (int i = pol1.Deg; i >= 0; i--)
                {
                    if (pol1[i] > pol2[i])
                        return 1;
                    else if (pol1[i] < pol2[i])
                        return -1;
                }

                return 0;
            }
        }
    }
}