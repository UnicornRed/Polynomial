using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TokenParsers;

namespace Diophantine
{
    /// <summary>
    /// Реализует обработку диофантовых уравнений.
    /// </summary>
    public class DiophantineEquations
    {
        /// <summary>
        /// Коэффициенты диофантового уравнения.
        /// </summary>
        private SortedList<int, int> coeff;

        /// <summary>
        /// Инициализирует диофантово уравнение по умолчанию.
        /// </summary>
        public DiophantineEquations()
        {
            coeff = new SortedList<int, int>();

            coeff.Add(0, 1);
            coeff.Add(1, 1);
        }

        /// <summary>
        /// Инициализирует диофантово уравнение по строке.
        /// </summary>
        /// <param name="dioStr">Строка, содержащая диофантово уравнение.</param>
        public DiophantineEquations(string dioStr)
        {
            try
            {
                coeff = DiophantineParser.DioEquationParser(dioStr);
            }
            catch (Exception e)
            {
                coeff = new SortedList<int, int>();
                throw new Exception(e.Message + "\nFailed to create an object.\n" +
                                    "The Diophantine equation should have the form: a_1*x_{b_1} + a_2*x_{b_2} + ... + a_n*x_{b_n} = c");
            }
        }

        /// <summary>
        /// Инициализирует диофантово уравнение по коэффициентам.
        /// </summary>
        /// <param name="coeff">Коэффициенты диофантового уравнения.</param>
        public DiophantineEquations(SortedList<int, int> coeff)
        {
            this.coeff = new SortedList<int, int>(coeff);
        }

        /// <summary>
        /// Решает диофантово уравнение методом Евклида.
        /// </summary>
        /// <returns>Решение диофантового уравнения.</returns>
        public SortedList<int, int> SolveEquation()
        {
            return ExtendedEuclid.SolveN(this.coeff);
        }

        /// <summary>
        /// Переопределённый метод вывода диофантового уравнения.
        /// </summary>
        /// <returns>Строку с диофантовым уравнением.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(16);

            int minNum = coeff.Keys.First(), maxNum = coeff.Keys.Last();

            foreach (var i in coeff)
            {
                if (i.Value != 0)
                {
                    if (i.Key == maxNum)
                        str.Append("=");

                    if (i.Key != minNum && i.Key != maxNum && i.Value >= 0)
                        str.Append("+");

                    if (i.Key != minNum && i.Value == -1)
                        str.Append("-");

                    if (Math.Abs(i.Value) != 1 || i.Key == maxNum)
                        str.Append(i.Value);

                    if (i.Key != maxNum)
                    {
                        if (Math.Abs(i.Value) != 1)
                            str.Append("*");

                        str.Append("x_" + i.Key);
                    }
                }
            }

            if (coeff.Count == 0)
                str.Append(0);

            return str.ToString();
        }
    }
}
