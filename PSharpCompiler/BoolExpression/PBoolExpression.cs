using System;
using Calculating;
using Polynomials;
using PolynomialComparers;

namespace PBool
{
    /// <summary>
    /// Реализует преобразование логического выражения со сравнением полиномов в логическое выражение от логических значений.
    /// </summary>
    class PBoolExpression
    {
        /// <summary>
        /// Операторы логического выражения.
        /// </summary>
        private static string[] oper = new string[] { ">", "<", "/=", @"\=", "==", "|=", "&", "|", "!", "r", "l" };

        /// <summary>
        /// Элементы логического выражения.
        /// </summary>
        private string[] tokenBool;

        /// <summary>
        /// Инициализирует обработчик логического выражения по строке с логическим выражением.
        /// </summary>
        /// <param name="boolExp">Строка с логическим выражением.</param>
        public PBoolExpression(string boolExp) => tokenBool = parserBool(boolExp);

        /// <summary>
        /// Вспомогательный метод для замены парной к текущей открывающей скобке закрывающей на символ 'r'.
        /// </summary>
        /// <param name="numChar">Номер текущей открывающей скобки.</param>
        /// <param name="boolExp">Обрабатываемое логическое выражение.</param>
        /// <returns>Изменённую строку.</returns>
        private static string rightReplace(int numChar, string boolExp)
        {
            int control = 1;
            boolExp = boolExp.Remove(numChar, 1);
            boolExp = boolExp.Insert(numChar, "l");

            for (int i = numChar + 1; i < boolExp.Length; i ++)
            {
                if (boolExp[i] == '(')
                    control++;

                if (boolExp[i] == ')')
                    control--;

                if (control == 0)
                {
                    boolExp = boolExp.Remove(i, 1);
                    boolExp = boolExp.Insert(i, "r");

                    return boolExp;
                }
            }

            return boolExp;
        }

        /// <summary>
        /// Вспомогательный метод для замены парной к текущей закрывающей скобке открывающей на символ 'l'.
        /// </summary>
        /// <param name="numChar">Номер текущей закрывающей скобки.</param>
        /// <param name="boolExp">Обрабатываемое логическое выражение.</param>
        /// <returns>Изменённую строку.</returns>
        private static string leftReplace(int numChar, string boolExp)
        {
            int control = 1;
            boolExp = boolExp.Remove(numChar, 1);
            boolExp = boolExp.Insert(numChar, "r");

            for (int i = numChar - 1; i >= 0; i--)
            {
                if (boolExp[i] == '(')
                    control--;

                if (boolExp[i] == ')')
                    control++;

                if (control == 0)
                {
                    boolExp = boolExp.Remove(i, 1);
                    boolExp = boolExp.Insert(i, "l");

                    return boolExp;
                }
            }

            return boolExp;
        }

        /// <summary>
        /// Сравнивает полиномы.
        /// </summary>
        /// <param name="boolExpCalc">Строка, в которой составляется искомое логическое выражение.</param>
        /// <param name="calc">Калькулятор, вычисляющий математические выражения.</param>
        /// <param name="numToken">Номер оператора сравнения.</param>
        /// <param name="compareHelp1">Вспомогательное значение, сравниваемое с полученным результатом сравнения полиномов.</param>
        /// <param name="compareHelp2">Вспомогательное значение, сравниваемое с полученным результатом сравнения полиномов.</param>
        private void ComparePol(ref string boolExpCalc, CalculatingExpressions calc,
                                int numToken, int compareHelp1, int compareHelp2)
        {
            int compareHelp = (new PolynomialComparer()).Compare((Polynomial)calc.Calculate(tokenBool[numToken - 1]),
                                                                 (Polynomial)calc.Calculate(tokenBool[numToken + 1]));

            if (compareHelp == compareHelp1 || compareHelp == compareHelp2)
                boolExpCalc += "true";
            else
                boolExpCalc += "false";
        }

        /// <summary>
        /// Делит строку с логическим выражением на элементы.
        /// </summary>
        /// <param name="boolExp">Строка с логическим выражением.</param>
        /// <returns>Разделённый элементы выражения.</returns>
        private static string[] parserBool(string boolExp)
        {
            boolExp = boolExp.Replace(" ", "");
            boolExp = boolExp.Replace(">=", "/=");
            boolExp = boolExp.Replace("<=", @"\=");
            boolExp = boolExp.Replace("!=", "|=");
            boolExp = rightReplace(0, boolExp);

            for (int i = 0; i < boolExp.Length; i ++)
            {
                if (boolExp[i] == '&' || boolExp[i] == '|')
                {
                    boolExp = rightReplace(i + 1, boolExp);
                    boolExp = leftReplace(i - 1, boolExp);
                }

                if (boolExp[i] == '!')
                    boolExp = rightReplace(i + 1, boolExp);
            }

            foreach (var i in oper)
                boolExp = boolExp.Replace(i, " " + i + " ");

            boolExp = boolExp.Replace("/=", ">=");
            boolExp = boolExp.Replace(@"\=", "<=");
            boolExp = boolExp.Replace("|=", "!=");

            return boolExp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Вычисляет логическое выражение.
        /// </summary>
        /// <param name="calc">Калькулятор, вычисляющий математические выражения.</param>
        /// <returns>Результат логического выражения.</returns>
        public bool Value(CalculatingExpressions calc)
        {
            string boolExpCalc = "";

            for (int i = 0; i < tokenBool.Length; i++)
            {
                switch (tokenBool[i])
                {
                    case ">":
                        ComparePol(ref boolExpCalc, calc, i, 1, 1);

                        break;

                    case "<":
                        ComparePol(ref boolExpCalc, calc, i, -1, -1);

                        break;

                    case "<=":
                        ComparePol(ref boolExpCalc, calc, i, -1, 0);
                        
                        break;

                    case ">=":
                        ComparePol(ref boolExpCalc, calc, i, 1, 0);
                        
                        break;

                    case "==":
                        ComparePol(ref boolExpCalc, calc, i, 0, 0);
                        
                        break;

                    case "!=":
                        ComparePol(ref boolExpCalc, calc, i, 1, -1);
                        
                        break;

                    case "&":
                    case "|":
                    case "!":
                    case "r":
                    case "l":
                        if (tokenBool[i] == "r")
                            tokenBool[i] = ")";

                        if (tokenBool[i] == "l")
                            tokenBool[i] = "(";

                        boolExpCalc += tokenBool[i];

                        break;

                    default:
                        break;    
                }
            }

            return (new BoolCalculating()).CalculateBool(boolExpCalc);
        }
    }
}
