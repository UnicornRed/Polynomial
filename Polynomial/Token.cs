using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

enum TypeOperation
{
    Variable,
    OpeningParenthesis,
    СlosingParenthesis,
    BinaryOperator,
    Comma,
    FunctionSingleVariable,
    FunctionTwoVariable,
    Error
}

namespace BasicAlg
{
    // The class that implements the storage of expression elements.
    class Token
    {
        // List of expression elements.
        private string token;
        // Operators for working with polynomials.
        private static HashSet<Operators> oper = Operators.GetOperators();

        public string Name
        {
            get
            {
                return token;
            }
        }

        /// <summary>
        /// Constructor of expression elements by string.
        /// </summary>
        /// <param name="expression"></param>
        public Token(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// The method for getting the priority of the operation.
        /// </summary>
        /// <param name="name">Name of expression element.</param>
        /// <returns>Priority of the operation or -1 if the expression element is not one.</returns>
        public static int Priority(string name)
        {
            foreach (var i in oper)
                if (i.Name == name)
                    return i.Priority;

            return -1;
        }

        /// <summary>
        /// The method determines whether an expression element is an operator.
        /// </summary>
        /// <param name="name">Name of expression element.</param>
        /// <returns>True if it is an operator, and false if it is not.</returns>
        public static bool IsOper(string name)
        {
            foreach (var i in oper)
                if (i.Name == name)
                    return true;

            if (name == "(" ||
                name == ")" ||
                name == ",")
                return true;

            return false;
        }

        /// <summary>
        /// The method determines the type of the expression element.
        /// </summary>
        /// <param name="name">Name of expression element.</param>
        /// <returns>Returns 0 if the expression element is not an operator,
        /// 1 if it is an opening parenthesis, 2 if it is a closing parenthesis,
        /// 4 if it is a comma, and the value of the property WhatOper in other cases.</returns>
        public static TypeOperation WhatToken(string name, CalculatingExpressions calc)
        {
            if (Regex.IsMatch(name, @"(^-?\d*(.\d+)?\*?x(\^\d+)?$)|(^-?\d+(.\d+)?$)$"))
                return TypeOperation.Variable;

            foreach(var i in calc)
                if (i == name)
                    return TypeOperation.Variable;

            switch (name)
            {
                case "(":
                    return TypeOperation.OpeningParenthesis;

                case ")":
                    return TypeOperation.СlosingParenthesis;

                case ",":
                    return TypeOperation.Comma;

                default:
                    foreach (var i in oper)
                        if (i.Name == name)
                            return i.WhatOper;

                    return TypeOperation.Error;
            }
        }

        /// <summary>
        /// The method calculates the value of the operator by stack.
        /// </summary>
        /// <param name="stackPoly">Stack of polynomials.</param>
        /// <param name="nameOper">Name of operator.</param>
        /// <returns>The value of the operator.</returns>
        public static Polynomial Solution(Stack<Polynomial> stackPoly, string nameOper)
        {
            foreach (var i in oper)
                if (i.Name == nameOper)
                {
                    if (i.WhatOper == TypeOperation.BinaryOperator ||
                        i.WhatOper == TypeOperation.FunctionTwoVariable)
                        return i.binaryOperator(stackPoly.Pop(), stackPoly.Pop());
                    else
                        return i.unaryOperator(stackPoly.Pop());
                }

            return null;
        }

        //A+(Diff(B-C*Diff(D))*Eval(A, Diff(A)))
        /// <summary>
        /// Splits the expression into tokens.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="vars">Possible parameters in the expression.</param>
        /// <returns></returns>
        public static List<Token> GetTokens(string expression, CalculatingExpressions calc)
        {
            List<Token> tokens = Parser.TokenParser(expression);

            expression = expression.Replace(" ", "");

            ExceptionToken exceptionToken = new ExceptionToken(tokens, oper, calc);

            exceptionToken.CheckExpression(expression);

            return tokens;
        }
    }
}
