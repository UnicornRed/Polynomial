using System;
using System.Collections.Generic;
using System.Linq;
using ParserWork;

namespace TokenParsers
{
    /// <summary>
    /// Реализует получение коэффициентов диофантового уравнения по строке.
    /// </summary>
    class DiophantineParser
    {
        /// <summary>
        /// Получает коэффициенты диофантового уравнения по строке.
        /// </summary>
        /// <param name="dioStr">Строка с диофантовым уравнением.</param>
        /// <returns>Коэффициенты диофантового уравнения.</returns>
        public static SortedList<int, int> DioEquationParser(string dioStr)
        {
            SortedList<int, int> coeff = new SortedList<int, int>();

            int numNow, coeffNow;

            dioStr = dioStr.ToLower();
            dioStr = dioStr.Replace(" ", "");
            dioStr = "+" + dioStr;


            string[] monoms = Parser.ParserForAll(dioStr, @"^\+?(\+-?\d+\*?x_\d+)+=\+?-?\d+$",
                                           new string[,] { { "-", "+-" }, { "+x", "+1x" }, { "-x", "-1x" } },
                                           new string[] { }, new char[] { '+', '=' });

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
    }
}
