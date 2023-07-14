using System;
using PolynomialInterfaces;
using System.Collections.Generic;

namespace Polynomials
{
    /// <summary>
    /// Реализует получение случайных полиномов.
    /// </summary>
    public class RandPolynomial : IBasicSequence<Polynomial>
    {
        /// <summary>
        /// Рандомизатор.
        /// </summary>
        private readonly Random rand;

        /// <summary>
        /// Наибольшая и наименьшая степени случайного полинома.
        /// </summary>
        private readonly int MinDeg, MaxDeg;

        /// <summary>
        /// Наибольшее и наименьшее значения коэффициентов случаного полинома.
        /// </summary>
        private readonly int MinMean, MaxMean;

        /// <summary>
        /// Инициализирует конструктор случайных полиномов по умолчанию.
        /// </summary>
        public RandPolynomial()
        {
            rand = new Random();

            MinDeg = 0;
            MaxDeg = 10;
            MinMean = -100;
            MaxMean = 100;
        }

        /// <summary>
        /// Инициализирует конструктор случайных полиномов с переданными значениями степени и коэффициентов.
        /// </summary>
        /// <param name="MinDeg">Минимальная степень случайного полинома.</param>
        /// <param name="MaxDeg">Максимальная степень случайного полинома.</param>
        /// <param name="MinMean">Минимальное значение коэффициентов случайного полинома.</param>
        /// <param name="MaxMean">Максимальное значение коэффициентов случайного полинома.</param>
        public RandPolynomial(int MinDeg, int MaxDeg, int MinMean, int MaxMean)
        {
            rand = new Random();

            if (MinDeg >= 0)
                this.MinDeg = MinDeg;
            else
                this.MinDeg = 0;

            this.MaxDeg = MaxDeg;
            this.MinMean = MinMean;
            this.MaxMean = MaxMean;
        }

        /// <summary>
        /// Получает новый случайный полином.
        /// </summary>
        /// <returns>Новый случайный полином.</returns>
        public Polynomial Next()
        {
            double mean;
            SortedList<int, double>  coeffs = new SortedList<int, double>();

            for (int i = MinDeg; i <= MaxDeg; i++)
            {
                mean = rand.Next(MinMean, MaxMean - 1) + rand.NextDouble();
                coeffs.Add(i, mean);
            }

            Polynomial pol = new Polynomial(coeffs);

            return pol;
        }
    }
}
