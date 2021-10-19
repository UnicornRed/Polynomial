using System;
using System.Collections.Generic;
using System.Text;

namespace BasicAlg
{
    // Checks the expression for correctness.
    class ExceptionToken
    {
        // Significant parts of the expression.
        List<Token> tokens;
        // List of operators and functions.
        HashSet<Operators> oper;
        // Сalculator containing variables.
        CalculatingExpressions calc;

        public ExceptionToken(List<Token> tokens, HashSet<Operators> oper, CalculatingExpressions calc)
        {
            this.tokens = tokens;
            this.oper = oper;
            this.calc = calc;
        }

        // Recursive expression validation.
        private int CheckExpressionR(ref int numToken, int indexError, bool inParenthesis, int countComma, out string er)
        {
            int countCommaNow = 0, indexErrorH, numTokenH;
            TypeOperation oper;
            er = "";

            while(numToken < tokens.Count)
            {
                oper = Token.WhatToken(tokens[numToken].Name, calc);

                switch (oper)
                {
                    case TypeOperation.BinaryOperator:
                        if (numToken + 1 == tokens.Count || numToken == 0 ||
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.BinaryOperator ||
                            oper == TypeOperation.Comma ||
                            oper == TypeOperation.СlosingParenthesis)
                        {
                            er = "A binary operator requires after itself a variable that opens a parenthesis or a function!\n";

                            return indexError;
                        }
                        
                        break;

                    case TypeOperation.Variable:
                        if (numToken + 1 != tokens.Count && (
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.Variable ||
                            oper == TypeOperation.OpeningParenthesis ||
                            oper == TypeOperation.FunctionSingleVariable ||
                            oper == TypeOperation.FunctionTwoVariable))
                        {
                            er = "The variable requires a binary operator, a closing parenthesis, or a comma after it!\n";

                            return indexError;
                        }

                        break;

                    case TypeOperation.OpeningParenthesis:
                        if (numToken + 1 == tokens.Count ||
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.Comma ||
                            oper == TypeOperation.BinaryOperator ||
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
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.FunctionSingleVariable ||
                            oper == TypeOperation.FunctionTwoVariable ||
                            oper == TypeOperation.OpeningParenthesis ||
                            oper == TypeOperation.Variable))
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
                            (oper = Token.WhatToken(tokens[numToken + 1].Name, calc)) == TypeOperation.BinaryOperator ||
                            oper == TypeOperation.СlosingParenthesis ||
                            oper == TypeOperation.Comma)
                        {
                            er += "The comma requires a variable, an opening parenthesis, or a function after it!\n";

                            return indexError;
                        }
                        else
                            countCommaNow++;

                        break;

                    case TypeOperation.FunctionSingleVariable:
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

                    case TypeOperation.FunctionTwoVariable:
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
        /// Checks the expression for correctness.
        /// </summary>
        /// <param name="expression"></param>
        public void CheckExpression (string expression)
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

                throw new Exception(er);
            }
        }
    }
}
