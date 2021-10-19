using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicAlg
{
    // The class that implements the solution of Diophantine equations via the Euclid algorithm.
    class ExtendedEuclid
    {
        // The method for finding the greatest common divisor by the Euclid algorithm.
        private static int AlgEuclid (int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a + b;
        }

        // The method for finding the greatest common divisor by Euclid's algorithm for n numbers.
        private static int AlgEuclidN(SortedList<int, int> coeff)
        {
            int nod = coeff[coeff.Keys.First()];

            foreach (var i in coeff)
                nod = AlgEuclid(nod, i.Value);

            return nod;
        }

        // The method for solving a Diophantine equation from two variables by an extended Euclidean algorithm.
        private static void Solve2(int a, int b, int c, out int x, out int y)
        {
            bool isSwap = false;

            if (a < b)
            {
                int t = a;
                a = b;
                b = t;
                isSwap = true;
            }

            int a1 = a, b1 = b, boof1, boof2, sign = 1;
            int[] m = { 1, 0, 0, 1 };

            while (b1 != 0)
            {
                boof1 = m[0];
                boof2 = m[2];
                m[0] = (a1 / b1) * m[0] + m[1];
                m[2] = (a1 / b1) * m[2] + m[3];

                int t = a1 % b1;
                a1 = b1;
                b1 = t;
                m[1] = boof1;
                m[3] = boof2;
                sign *= -1;
            }

            if (c % a1 != 0)
                throw new Exception("No solve!");

            x = sign * m[3] * c / a1;
            y = (-1) * sign * m[1] * c / a1;

            if (isSwap)
            {
                int t = x;
                x = y;
                y = t;
            }
        }

        /// <summary>
        /// The method for solving a Diophantine equation from n variables by an extended Euclidean algorithm.
        /// </summary>
        /// <param name="coeff">The coefficients of the diophantine equation of n variables.</param>
        /// <returns>The solution of the Diophantine equation of n variables.</returns>
        public static SortedList<int, int> SolveN(SortedList<int, int> coeff)
        {
            int x = 0, y = 0, a, c = (int)coeff[coeff.Keys.Last()], count = 0, gcd;
            coeff.Remove(coeff.Keys.Last());
            SortedList<int, int> solution = new SortedList<int, int>();
            SortedList<int, int> coeffCopy = new SortedList<int, int>(coeff);

            if (ExtendedEuclid.AlgEuclidN(coeff) == 0)
                throw new Exception("No solution!");

            for (int i = 0; i < coeff.Count() - 1; i++)
            {
                a = coeffCopy[coeffCopy.Keys.ElementAt(0)];
                coeffCopy.RemoveAt(0);

                gcd = ExtendedEuclid.AlgEuclidN(coeffCopy);

                if (a == 0)
                {
                    x = 0;
                    y = c;
                }

                if (gcd == 0 && a != 0)
                {
                    x = c / a;
                    y = 0;
                }

                if (a != 0 && gcd != 0)
                    ExtendedEuclid.Solve2(a, gcd, c, out x, out y);

                solution.Add(count, x);
                count++;
                c = y;

                for (int j = 0; j < coeffCopy.Count; j++)
                {
                    int index = coeffCopy.Keys.ElementAt(j);

                    if (gcd != 0)
                        coeffCopy[index] = coeffCopy[index] / gcd;
                }
            }

            x = coeffCopy.Last().Value;

            if (x != 0)
                c /= x;

            solution.Add(count, c);

            return solution;
        }
    }
}
