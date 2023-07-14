using System;
using ParserWork;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TokenParsers
{
    /// <summary>
    /// Реализует получение коэффициентов полинома по строке.
    /// </summary>
    class PolynomialParser
    {
        /// <summary>
        /// Получает коэффициенты многочлена по строке.
        /// </summary>
        /// <param name="polyStr">Строка с полиномом.</param>
        /// <returns>Коэффициенты многочлена.</returns>
        public static SortedList<int, double> PolParser(string polyStr)
        {
            SortedList<int, double> coeff = new SortedList<int, double>();

            int degNow;
            double coeffNow;

            polyStr = polyStr.ToLower();
            polyStr = polyStr.Replace(" ", "");
            polyStr = "+" + polyStr + "+";

            string[] monoms = Parser.ParserForAll(polyStr, @"^\+?(\+-?\d+(.\d+)?(\*?x\^\d+)?)+\+$",
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
        /// Проверяет имя полинома на соответствие формату.
        /// </summary>
        /// <param name="id">Имя полинома.</param>
        /// <returns>true, если имя соответствует формату, false, если не соответствует.</returns>
        public static bool IdParse(string id)
        {
            string check = @"^[A-Z]\d?$";

            return (Regex.IsMatch(id, check));
        }
    }
}