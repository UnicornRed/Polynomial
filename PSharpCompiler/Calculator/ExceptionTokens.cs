using System;
using System.Collections.Generic;

namespace Calculating
{
    /// <summary>
    /// Реализует проверку выражения.
    /// </summary>
    class ExceptionToken
    {
        /// <summary>
        /// Элементы выражения.
        /// </summary>
        readonly List<Token> tokens;

        /// <summary>
        /// Калькулятор, содержащий переменные.
        /// </summary>
        readonly CalculatingExpressions calc;

        /// <summary>
        /// Инициализирует проверку выражения по элементам выражения и калькулятору.
        /// </summary>
        /// <param name="tokens">Список элементов выражения.</param>
        /// <param name="calc">Калькулятор.</param>
        public ExceptionToken(List<Token> tokens, CalculatingExpressions calc)
        {
            this.tokens = tokens;
            this.calc = calc;
        }

        /// <summary>
        /// Рекурсивная проверка выражения.
        /// </summary>
        /// <param name="numToken">Номер проверяемого элемента выражения.</param>
        /// <param name="indexError">Текущий элемент выражения.</param>
        /// <param name="inParenthesis">Определяет находится ли программа в рекурсии.</param>
        /// <param name="countComma">Количество запятых в функции.</param>
        /// <param name="er">Строка ошибки.</param>
        /// <returns>Номер </returns>
        private int CheckExpressionR(ref int numToken, int indexError, bool inParenthesis, int countComma, out string er)
        {
            int countCommaNow = 0, indexErrorH, numTokenH;
            TypeOperation oper;
            er = "";

            while (numToken < tokens.Count)
            {
                oper = Token.WhatToken(tokens[numToken].Name, calc);

                switch (oper)
                {
                    case TypeOperation.BinOper:
                        if (numToken + 1 == tokens.Count || numToken == 0 ||
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.BinOper ||
                            oper == TypeOperation.Comma ||
                            oper == TypeOperation.СlosingParenthesis)
                        {
                            er = "A binary operator requires after itself a variable that opens a parenthesis or a function!\n";

                            return indexError;
                        }

                        break;

                    case TypeOperation.Id:
                        if (numToken + 1 != tokens.Count && (
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.Id ||
                            oper == TypeOperation.OpeningParenthesis ||
                            oper == TypeOperation.UnaFunc ||
                            oper == TypeOperation.BinFunc))
                        {
                            er = "The variable requires a binary operator, a closing parenthesis, or a comma after it!\n";

                            return indexError;
                        }

                        break;

                    case TypeOperation.OpeningParenthesis:
                        if (numToken + 1 == tokens.Count ||
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.Comma ||
                            oper == TypeOperation.BinOper ||
                            oper == TypeOperation.СlosingParenthesis)
                        {
                            er = "The opening parenthesis requires a variable, an opening parenthesis, or a function after it!\n";

                            return indexError;
                        }
                        else
                        {
                            numToken++;
                            indexErrorH = CheckExpressionR(ref numToken, indexError, true, 0, out er);

                            if (indexErrorH == -1)
                            {
                                er = "The closing parenthesis is missing!\n";

                                return indexError;
                            }

                            indexError = indexErrorH + 1;

                            if (er != "")
                                return indexError;
                        }

                        break;

                    case TypeOperation.СlosingParenthesis:
                        if (numToken + 1 != tokens.Count && (
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.UnaFunc ||
                            oper == TypeOperation.BinFunc ||
                            oper == TypeOperation.OpeningParenthesis ||
                            oper == TypeOperation.Id))
                        {
                            er = "The closing parenthesis requires an operator, comma, or closing parenthesis after it!\n";

                            return indexError;
                        }
                        else
                        {
                            if (inParenthesis)
                            {
                                if (countComma != countCommaNow)
                                    return -1;

                                return indexError;
                            }
                            else
                            {
                                er = "The opening parenthesis is missing!\n";

                                return indexError;
                            }
                        }

                    case TypeOperation.Comma:
                        if (numToken + 1 == tokens.Count ||
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.BinOper ||
                            oper == TypeOperation.СlosingParenthesis ||
                            oper == TypeOperation.Comma)
                        {
                            er += "The comma requires a variable, an opening parenthesis, or a function after it!\n";

                            return indexError;
                        }
                        else
                            countCommaNow++;

                        break;

                    case TypeOperation.UnaFunc:
                        if (numToken + 1 == tokens.Count || numToken + 2 == tokens.Count ||
                            Token.WhatToken(tokens[numToken + 1].Name, calc) != TypeOperation.OpeningParenthesis)
                        {
                            er += "The function requires an opening parenthesis and something after it!\n";

                            return indexError;
                        }
                        else
                        {
                            numTokenH = numToken;
                            numToken += 2;

                            indexErrorH = CheckExpressionR(ref numToken, indexError, true, 0, out er);

                            if (indexErrorH == -1)
                            {
                                er = "There is no closing parenthesis or the number of variables in the function is not equal to the required one!\n";

                                return indexError;
                            }

                            indexError = indexErrorH + tokens[numTokenH].Name.Length + 1;

                            if (er != "")
                                return indexError;
                        }

                        break;

                    case TypeOperation.BinFunc:
                        if (numToken + 1 == tokens.Count || numToken + 2 == tokens.Count ||
                            Token.WhatToken(tokens[numToken + 1].Name, calc) != TypeOperation.OpeningParenthesis)
                        {
                            er += "The function requires an opening parenthesis and something after it!\n";

                            return indexError;
                        }
                        else
                        {
                            numTokenH = numToken;
                            numToken += 2;
                            indexErrorH = CheckExpressionR(ref numToken, indexError, true, 1, out er);

                            if (indexErrorH == -1)
                            {
                                er = "There is no closing parenthesis or the number of variables in the function is not equal to the required one!\n";

                                return indexError;
                            }

                            indexError = indexErrorH + tokens[numTokenH].Name.Length + 1;

                            if (er != "")
                                return indexError;
                        }

                        break;

                    case TypeOperation.TerFunc:
                        if (numToken + 1 == tokens.Count || numToken + 2 == tokens.Count ||
                            Token.WhatToken(tokens[numToken + 1].Name, calc) != TypeOperation.OpeningParenthesis)
                        {
                            er += "The function requires an opening parenthesis and something after it!\n";

                            return indexError;
                        }
                        else
                        {
                            numTokenH = numToken;
                            numToken += 2;
                            indexErrorH = CheckExpressionR(ref numToken, indexError, true, 2, out er);

                            if (indexErrorH == -1)
                            {
                                er = "There is no closing parenthesis or the number of variables in the function is not equal to the required one!\n";

                                return indexError;
                            }

                            indexError = indexErrorH + tokens[numTokenH].Name.Length + 1;

                            if (er != "")
                                return indexError;
                        }

                        break;

                    case TypeOperation.Error:
                        er += "Unknown variable, function or operator!\n";

                        return indexError;
                }

                indexError += tokens[numToken].Name.Length;
                numToken++;
            }

            return -1;
        }

        /// <summary>
        /// Проверяет выражение на корректность.
        /// </summary>
        /// <param name="expression">Проверяемое выражение.</param>
        public void CheckExpression(string expression)
        {
            string er;
            int indexError, numToken = 0;

            if ((indexError = CheckExpressionR(ref numToken, 0, false, 0, out er)) != -1)
            {
                er += expression + "\n";

                for (int j = 0; j < expression.Length; j++)
                    if (j == indexError)
                        er += "^";
                    else
                        er += "~";

                throw new Exception(er + "\n");
            }
        }
    }
}