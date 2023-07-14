using System;
using System.Text.RegularExpressions;

namespace ParserWork
{
    /// <summary>
    /// Реализует деление выражения на части.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Делит выражение на части.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="format">Формат выражения, которому должено соответствовать выражение.</param>
        /// <param name="replaceArray">Массив пар подстрок. Первая заменяется на вторую в выражении.</param>
        /// <param name="token">Массив элементов выражения, на которые следует делить выражение.</param>
        /// <param name="splitChar">Массив символов, по которым следует делить выражение.</param>
        /// <returns>Массив строк, на которые разделено выражение.</returns>
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
    }
}
