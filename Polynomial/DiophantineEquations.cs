using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicAlg
{
    // The class that implements linear Diophantine equations.
    public class DiophantineEquations
    {
        // Coefficients of the diophantine equation.
        private SortedList<int, int> coeff;

        /// <summary>
        /// The default constructor of Diophantine equations.
        /// </summary>
        public DiophantineEquations()
        {
            coeff = new SortedList<int, int>();

            coeff.Add(0, 1);
            coeff.Add(1, 1);
        }

        /// <summary>
        /// The constructor of Diophantine equations by string.
        /// </summary>
        /// <param name="dioStr">The string with a diophantine equation.</param>
        public DiophantineEquations(string dioStr)
        {
            try
            {
                coeff = Parser.DioEquationParser(dioStr);
            }
            catch (Exception e)
            {
                coeff = new SortedList<int, int>();
                throw new Exception(e.Message + "\nFailed to create an object.\n" +
                                    "The Diophantine equation should have the form: a_1*x_{b_1} + a_2*x_{b_2} + ... + a_n*x_{b_n} = c");
            }
        }

        /// <summary>
        /// The constructor of Diophantine equations for an array of coefficients.
        /// </summary>
        /// <param name="coeff">Coefficients of the diophantine equation.</param>
        public DiophantineEquations(SortedList<int, int> coeff)
        {
            this.coeff = new SortedList<int, int>(coeff);
        }

        /// <summary>
        /// The method for solving a Diophantine equation from n variables by an extended Euclidean algorithm.
        /// </summary>
        /// <returns>The solution of the Diophantine equation of n variables.</returns>
        public SortedList<int, int> SolveEquation()
        {
            return ExtendedEuclid.SolveN(this.coeff);
        }

        // Overriding the method ToString for the correct output of the Diophantine equations.
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
