using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace BasicAlg
{
    // Сlass that processes polynomials.
    public class Polynomial : IEnumerable<int>, IComparable<Polynomial>
    {
        /// <summary>
        /// The degree of the polynomial.
        /// </summary>
        private int deg;
        //Coefficients of the polynomial.
        private SortedList<int, double> coeff;

        /// <summary>
        /// The property of obtaining the degree of a polynomial.
        /// </summary>
        public int Deg
        {
            get
            {
                return deg;
            }
        }

        public double this[int i]
        {
            get
            {
                if (coeff.ContainsKey(i))
                    return coeff[i];
                else
                    return 0;
            }
            private set
            {
                if (coeff.ContainsKey(i))
                    coeff[i] = value;
                else
                    coeff.Add(i, value);

                if (coeff[i] == 0)
                    coeff.Remove(i);

                deg = DegCoeff(coeff);
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return coeff.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        public int CompareTo(Polynomial p)
        {
            if (this[0] > p[0])
                return 1;
            else if (this[0] < p[0])
                return -1;
            else
                return 0;
        }

        public static implicit operator Polynomial (string poly)
        {
            return new Polynomial(poly);
        }

        /// <summary>
        /// The constructor of the polynomial 0.
        /// </summary>
        public Polynomial()
        {
            deg = -1;
            coeff = new SortedList<int, double>();
        }

        /// <summary>
        /// The constructor of the polynomial by coefficients.
        /// </summary>
        /// <param name="coeff"></param>
        public Polynomial(SortedList<int, double> coeff)
        {
            this.coeff = new SortedList<int, double>();

            foreach (var i in coeff)
            {
                if (i.Value != 0)
                    this.coeff.Add(i.Key, i.Value);
            }

            deg = DegCoeff(coeff);
        }

        /// <summary>
        /// The constructor of the polynomial by string.
        /// </summary>
        /// <param name="polStr"></param>
        public Polynomial(string polStr)
        {
            try
            {
                coeff = Parser.PolynomialParser(polStr);

                deg = DegCoeff(coeff);
            }
            catch (Exception e)
            {
                deg = -1;
                coeff = new SortedList<int, double>();
                throw new Exception(e.Message + "\nFailed to create an object.\n" +
                                    "The polynomial should have the form: a_1*x^b_1 + a_2*x^b_2 + ... + a_n*x^b_n");
            }
        }

        /// <summary>
        /// The constructor of a polynomial based on the passed polynomial.
        /// </summary>
        /// <param name="polynomial"></param>
        public Polynomial(Polynomial polynomial)
        {
            deg = polynomial.Deg;

            coeff = new SortedList<int, double>();

            foreach (var i in polynomial)
                coeff.Add(i, polynomial[i]);
        }

        /// <summary>
        /// The minimum degree of the monome.
        /// </summary>
        /// <returns></returns>
        public int MinDeg()
        {
            int minDeg = -1;

            foreach(var i in coeff)
            {
                minDeg = i.Key;

                break;
            }

            return minDeg;
        }

        private int DegCoeff(SortedList<int, double> coeff)
        {
            int deg;

            if (coeff.Count != 0)
                deg = coeff.Keys.LastOrDefault();
            else
                deg = -1;

            return deg;
        }

        /// <summary>
        /// The method that differentiates a polynomial.
        /// </summary>
        /// <returns></returns>
        public Polynomial Diff()
        {
            SortedList<int, double> coeffDiff = new SortedList<int, double>();

            foreach (var i in coeff)
                coeffDiff.Add(i.Key - 1, i.Value * i.Key);
            
            Polynomial pDiff = new Polynomial(coeffDiff);

            return pDiff;
        }

        /// <summary>
        /// The method that takes a polynomial from a polynomial.
        /// </summary>
        /// <param name="polynomial"></param>
        /// <returns></returns>
        public Polynomial Eval(Polynomial polynomial)
        {
            Polynomial polynomial1 = new Polynomial();

            int minDeg = MinDeg();
            polynomial1[0] = coeff[deg];

            Polynomial polynomialEval = new Polynomial(polynomial1);
          
            for (int i = deg - 1; i >= 0; i --)
            {
                polynomialEval = polynomialEval * polynomial;
                if (coeff.ContainsKey(i))
                {
                    polynomial1[0] = coeff[i];
                    polynomialEval = polynomialEval + polynomial1;
                }
            }

            return polynomialEval;
        }

        // Overriding the addition operator for working with polynomials.
        public static Polynomial operator + (Polynomial polynomial1, Polynomial polynomial2)
        {
            Polynomial polynomialPlus = new Polynomial(polynomial1);

            foreach (var i in polynomial2)
                polynomialPlus[i] += polynomial2[i];

            return polynomialPlus;
        }

        // Overriding the subtraction operator for working with polynomials.
        public static Polynomial operator - (Polynomial polynomial1, Polynomial polynomial2)
        {
            Polynomial polynomialMinus = new Polynomial(polynomial1);

            foreach (var i in polynomial2)
                polynomialMinus[i] -= polynomial2[i];

            return polynomialMinus;
        }

        // Overriding the multiplication operator for working with polynomials.
        public static Polynomial operator * (Polynomial polynomial1, Polynomial polynomial2)
        {
            Polynomial polynomialMulti = new Polynomial();

            foreach (var i in polynomial1)
            {
                foreach (var j in polynomial2)
                    polynomialMulti[i + j] += polynomial1[i] * polynomial2[j];
            }

            return polynomialMulti;
        }

        // Overriding the method ToString for the correct output of the polynomial.
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(16);

            int minDeg = MinDeg();

            foreach (var i in coeff)
            {
                if (i.Value != 0)
                {
                    if (i.Key != minDeg && i.Value >= 0)
                        str.Append("+");

                    if (i.Key != 0 && i.Value == -1)
                        str.Append("-");

                    if (Math.Abs(i.Value) != 1 || i.Key == 0)
                        str.Append(String.Format("{0:f}", i.Value));

                    if (Math.Abs(i.Value) != 1 && i.Key != 0)
                        str.Append("*");

                    if (i.Key != 0)
                        str.Append("x");

                    if (i.Key > 1)
                        str.Append("^" + i.Key);
                }
            }

            if (coeff.Count == 0)
                str.Append(0);

            return str.ToString();
        }
    }

    // The class that implements a storage operator for working with polynomials.
    class Operators
    {
        // Refers to a binary function.
        public delegate Polynomial BinaryOperator(Polynomial p1, Polynomial p2);
        // Refers to a unary function.
        public delegate Polynomial UnaryOperator(Polynomial p);

        // List of operators.
        private static readonly HashSet<Operators> operators = new HashSet<Operators>
        {
            {new Operators("+", TypeOperation.BinaryOperator, 1, (x, y) => x + y)},
            {new Operators("-", TypeOperation.BinaryOperator, 1, (x, y) => y - x)},
            {new Operators("*", TypeOperation.BinaryOperator, 2, (x, y) => x * y)},
            {new Operators("Eval", TypeOperation.FunctionTwoVariable, 0, (x, y) => y.Eval(x))},
            {new Operators("Diff", TypeOperation.FunctionSingleVariable, 0, (x) => x.Diff())}
        };

        /// <summary>
        /// The method for getting a list of operators.
        /// </summary>
        /// <returns>List of operators.</returns>
        public static HashSet<Operators> GetOperators()
        {
            return operators;
        }

        // Name of operator.
        public string Name { get; private set; }
        // Type of operator.
        public TypeOperation WhatOper { get; private set; }
        // Operator priority.
        public int Priority { get; private set; }
        // Reference to a binary function.
        public BinaryOperator binaryOperator;
        // Reference to a unary function.
        public UnaryOperator unaryOperator;

        // The constructor of the binary operator.
        private Operators(string name, TypeOperation whatOper, int priority, BinaryOperator binaryOperator)
        {
            this.Name = name;
            this.WhatOper = whatOper;
            this.Priority = priority;
            this.binaryOperator = binaryOperator;
        }

        // The constructor of the unary operator.
        private Operators(string name, TypeOperation whatOper, int priority, UnaryOperator singleOperator)
        {
            this.Name = name;
            this.WhatOper = whatOper;
            this.Priority = priority;
            this.unaryOperator = singleOperator;
        }
    }

    // The class that implements the interface for comparing polynomials.
    class PolynomialComparer : IComparer<Polynomial>
    {
        /// <summary>
        /// The method for comparing polynomials.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public int Compare(Polynomial p1, Polynomial p2)
        {
            if (p1.Deg > p2.Deg)
                return 1;
            else if (p1.Deg < p2.Deg)
                return -1;
            else
            {
                for (int i = p1.Deg; i >= 0; i--)
                {
                    if (p1[i] > p2[i])
                        return 1;
                    else if (p1[i] < p2[i])
                        return -1;
                }

                return 0;
            }
        }
    }

    // The class that implements getting a random polynomial.
    class RandomPolynomial : IBasicSequence<Polynomial>
    {
        // The maximum degree of a random polynomial.
        private int maxDeg;
        // The boundaries of the range of values for the coefficients.
        private double toRight, toLeft;

        /// <summary>
        /// The default random polynomial constructor.
        /// </summary>
        public RandomPolynomial()
        {
            maxDeg = (int)10e4;
            toRight = 10e9;
            toLeft = -10e9;
        }

        /// <summary>
        /// The constructor of random polynomials with a given maximum degree.
        /// </summary>
        /// <param name="maxDeg">The maximum degree of a random polynomial.</param>
        public RandomPolynomial(int maxDeg = (int)10e4)
        {
            this.maxDeg = maxDeg;
            toRight = 10e9;
            toLeft = -10e9;
        }

        /// <summary>
        /// The constructor of random polynomials with a given maximum degree and boundaries of the interval.
        /// </summary>
        /// <param name="maxDeg">The maximum degree of a random polynomial.</param>
        /// <param name="a">The left boundary of the range of values for the coefficients.</param>
        /// <param name="b">The right boundary of the range of values for the coefficients.</param>
        public RandomPolynomial(int maxDeg = (int)10e4, double a = -10e9, double b = 10e9)
        {
            this.maxDeg = maxDeg;
            if (a < b)
            {
                toRight = b;
                toLeft = a;
            }
            else
            {
                a = -10e9;
                b = 10e9;
            }
        }

        /// <summary>
        /// The method creates a new random polynomial.
        /// </summary>
        /// <returns>Next random polynomial.</returns>
        public Polynomial Next()
        {
            SortedList<int, double> coeff = new SortedList<int, double>();
            Random rand = new Random();

            for (int i = 0; i < maxDeg; i++)
            {
                coeff.Add(i, rand.NextDouble() * (toRight - toLeft) + toLeft);
            }

            Polynomial p = new Polynomial(coeff);

            return p;
        }
    }

    // The class that implements Hermite polynomials.
    class PolynomialHermite : ISequence<Polynomial>
    {
        // The current and previous polynomials.
        Polynomial pHermiteNow, pHermitePrev;
        // The number of the current polynomial.
        int numPoly;
        // The constant polynomial for use in calculations.
        private Polynomial polConst = new Polynomial("2x");

        /// <summary>
        /// The constructor of the Hermite polynomial numeration class.
        /// </summary>
        public PolynomialHermite()
        {
            numPoly = 0;
            pHermiteNow = new Polynomial("1");
            pHermitePrev = new Polynomial("0");
        }

        /// <summary>
        /// The method that gets the current Hermite polynomial.
        /// </summary>
        /// <returns>Сurrent Hermite polynomial.</returns>
        public Polynomial Now()
        {
            return pHermiteNow;
        }

        /// <summary>
        /// The method that gets the following Hermite polynomial.
        /// </summary>
        /// <returns>Following Hermite polynomial.</returns>
        public Polynomial Next()
        {
            Polynomial pCopy = new Polynomial(pHermiteNow);
            pHermiteNow = polConst * pHermiteNow - (new Polynomial((2 * numPoly).ToString())) * pHermitePrev;
            pHermitePrev = new Polynomial(pCopy);
            numPoly++;

            return pHermiteNow;
        }

        /// <summary>
        /// The method that gets the previous Hermite polynomial.
        /// </summary>
        /// <returns>Previous Hermite polynomial.</returns>
        public Polynomial Prev()
        {
            if (numPoly != 0)
            {
                numPoly--;
                Polynomial pCopy = new Polynomial(pHermitePrev);
                pHermitePrev = (polConst * pHermitePrev - pHermiteNow) * (new Polynomial((1.0 / (2 * numPoly)).ToString()));
                pHermiteNow = new Polynomial(pCopy);
            }

            return pHermiteNow;
        }

        /// <summary>
        /// The method that resets the current and previous polynomials.
        /// </summary>
        public void Reset()
        {
            numPoly = 0;
            pHermiteNow = new Polynomial("1");
            pHermitePrev = new Polynomial("0");
        }
    }
}
