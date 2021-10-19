using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BasicAlg
{
    // String parsing class.
    class Parser
    {
        /// <summary>
        /// Divides the expression into parts.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="format">The verification format of the expression.</param>
        /// <param name="replaceArray">Array of substitution of substrings for other substrings.</param>
        /// <param name="token">The parts into which the expression is divided.</param>
        /// <param name="splitChar">Array of separators.</param>
        /// <returns>Array of separated parts of the expression.</returns>
        public static string[] ParserForAll(string expression, string format, string[,] replaceArray, string[] token, char[] splitChar)
        {
            int count = 0;
            string helpStr = null;

            foreach (string i in replaceArray)
            {
                if (count % 2 == 0)
                    helpStr = i;
                else
                    expression = expression.Replace(helpStr, i);

                count++;
            }

            if (!Regex.IsMatch(expression, format))
                throw new FormatException("Invalid format!");

            foreach (var i in token)
                expression = expression.Replace(i, splitChar[0] + i + splitChar[0]);

            return expression.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>x
        /// The method gets the coefficients of the polynomial in a row.
        /// </summary>
        /// <param name="polyStr"></param>
        /// <returns></returns>
        public static SortedList<int, double> PolynomialParser(string polyStr)
        {
            SortedList<int, double> coeff = new SortedList<int, double>();

            int degNow = -1;
            double coeffNow = 0;

            polyStr = polyStr.ToLower();
            polyStr = polyStr.Replace(" ", "");
            polyStr = "+" + polyStr + "+";

            string[] monoms = ParserForAll(polyStr, @"^\+?(\+-?\d+(.\d+)?(\*?x\^\d+)?)+\+$", 
                                           new string[,]{ { "." , "," } , { "-" , "+-" } , { "+x" , "+1x" } , 
                                                          { "-x" , "-1x" } , { "x+" , "x^1+" } },
                                           new string[] { }, new char[] { '+' });

            for (int i = 0; i < monoms.Length; i++)
            {
                string[] coeffAndDeg = monoms[i].Split(new char[] { '*', 'x', '^' }, StringSplitOptions.RemoveEmptyEntries);

                coeffNow = Convert.ToDouble(coeffAndDeg[0]);

                if (coeffAndDeg.Length == 1)
                    degNow = 0;
                else
                    degNow = Convert.ToInt32(coeffAndDeg[1]);

                if (coeffNow != 0)
                {
                    if (coeff.ContainsKey(degNow))
                        coeff[degNow] += coeffNow;
                    else
                        coeff.Add(degNow, coeffNow);
                }
            }

            return coeff;
        }

        /// <summary>
        /// The method that gets the coefficients of the Diophantine equation from the passed string.
        /// </summary>
        /// <param name="dioStr">The string with a diophantine equation.</param>
        /// <returns>Coefficients of the diophantine equation.</returns>
        public static SortedList<int, int> DioEquationParser(string dioStr)
        {
            SortedList<int, int> coeff = new SortedList<int, int>();

            int numNow = -1, coeffNow = 0;

            dioStr = dioStr.ToLower();
            dioStr = dioStr.Replace(" ", "");
            dioStr = "+" + dioStr;
            

            string[] monoms = ParserForAll(dioStr, @"^\+?(\+-?\d+\*?x_\d+)+=\+?-?\d+$",
                                           new string[,]{ { "-" , "+-" } , { "+x" , "+1x" } , { "-x" , "-1x" } },
                                           new string[] { }, new char[] { '+' , '=' });

            for (int i = 0; i < monoms.Length - 1; i++)
            {
                string[] coeffAndNum = monoms[i].Split(new char[] { '*', 'x', '_' }, StringSplitOptions.RemoveEmptyEntries);

                coeffNow = Convert.ToInt32(coeffAndNum[0]);
                numNow = Convert.ToInt32(coeffAndNum[1]);

                if (coeff.ContainsKey(numNow))
                    coeff[numNow] += coeffNow;
                else
                    coeff.Add(numNow, coeffNow);
            }

            coeff.Add(coeff.Keys.Last() + 1, Convert.ToInt32(monoms[monoms.Length - 1]));

            return coeff;
        }

        /// <summary>
        /// The method divides expressions into components.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>List of expression elements.</returns>
        public static List<Token> TokenParser(string expression)
        {
            HashSet<Operators> oper = Operators.GetOperators();
            int countOper = 0;
            string[] nameOpers = new string[oper.Count()];

            if (expression[0] == '-')
                expression = "0" + expression;

            foreach (var i in oper)
                nameOpers[countOper++] = i.Name;

            string[] tokensStr = ParserForAll(expression, "",
                                              new string[,] { { "(-", "(0-" }, { ",-", ",0-" }, { "(", " ( " } ,
                                                              { ")", " ) " } , { ",", " , " } },
                                              nameOpers, new char[] { ' ' });
            List<Token> tokens = new List<Token>();

            foreach (var i in tokensStr)
                tokens.Add(new Token(i));

            return tokens;
        }
    }
}
