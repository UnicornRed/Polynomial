using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace BasicAlg
{
    // The class that implements the calculation of mathematical expressions.
    public class CalculatingExpressions : IEnumerable<string>
    {
        // Variables used in the expression.
        private readonly Dictionary<string, Polynomial> vars;

        public Dictionary<string, Polynomial> Vars
        {
            get
            {
                return vars;
            }
        }

        public Polynomial this[string i]
        {
            get
            {
                if (vars.ContainsKey(i))
                    return vars[i];
                else
                    return null;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return vars.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        /// <summary>
        /// The default calculator constructor.
        /// </summary>
        public CalculatingExpressions()
        {
            vars = new Dictionary<string, Polynomial>();
        }

        /// <summary>
        /// Calculator constructor based on an array of variable assignment strings.
        /// </summary>
        /// <param name="vars">Array of variable assignment strings.</param>
        public CalculatingExpressions(string[] vars)
        {
            this.vars = new Dictionary<string, Polynomial>();

            foreach (var i in vars)
                this.NewVar(i);
        }


        /// <summary>
        /// The method adds a new variable to the calculator based on the received string.
        /// </summary>
        /// <param name="variable">String of the form: [Name] = [Polynomial]</param>
        public void NewVar(string variable)
        {
            string[] nameAndPoly = variable.Split(new char[] { '=' });

            if (nameAndPoly.Length != 2)
                throw new Exception("Incorrect format!\n" +
                                    "The variable should have the form: [Name] = [Polynomial]");
            else
            {
                Polynomial polyNow = new Polynomial(nameAndPoly[1]);

                if (Regex.IsMatch(nameAndPoly[0], @"^\s*[a-zA-Z][a-zA-Z0-9_]*\s*$"))
                    vars.Add(nameAndPoly[0].Replace(" ", ""), polyNow);
                else
                    throw new Exception("Variable names must begin with a letter and contain letters, numbers, and underscores.");
            }
        }

        /// <summary>
        /// The method removes a variable from the calculator by its name.
        /// </summary>
        /// <param name="variable">Name of the variable.</param>
        public void DeleteVar(string variable)
        {
            if (!vars.Remove(variable))
                throw new Exception("The object does not exist in the list of variables.");
        }

        /// <summary>
        /// The method of translating an expression into Reverse Polish Notation.
        /// </summary>
        /// <param name="tokens">Elements of the expression.</param>
        /// <returns>List of expression elements in Reverse Polish Notation.</returns>
        private List<string> ToRPN(List<Token> tokens)
        {
            List<string> rpn = new List<string>();
            Stack<string> oper = new Stack<string>();

            foreach (var i in tokens)
            {
                TypeOperation whatToken = Token.WhatToken(i.Name, this);

                switch(whatToken)
                {
                    case TypeOperation.Variable:
                        rpn.Add(i.Name);

                        break;

                    case TypeOperation.OpeningParenthesis:
                    case TypeOperation.FunctionSingleVariable:
                    case TypeOperation.FunctionTwoVariable:
                        oper.Push(i.Name);

                        break;

                    case TypeOperation.СlosingParenthesis:
                        while (oper.Count != 0 && oper.Peek() != "(")
                            rpn.Add(oper.Pop());

                        if (oper.Count != 0)
                            oper.Pop();

                        if (oper.Count != 0)
                        {
                            TypeOperation isFun = Token.WhatToken(oper.Peek(), this);

                            if (isFun == TypeOperation.FunctionSingleVariable ||
                                isFun == TypeOperation.FunctionTwoVariable)
                                rpn.Add(oper.Pop());
                        }

                        break;

                    case TypeOperation.BinaryOperator:
                        while (oper.Count != 0 &&  
                               Token.Priority(oper.Peek()) >= Token.Priority(i.Name))
                            rpn.Add(oper.Pop());

                        oper.Push(i.Name);

                        break;

                    case TypeOperation.Comma:
                        while (oper.Count != 0 && oper.Peek() != "(")
                            rpn.Add(oper.Pop());

                        break;

                    default:

                        break;
                }
            }

            while (oper.Count != 0)
                rpn.Add(oper.Pop());

            return rpn;
        }

        /// <summary>
        /// The method that evaluates an expression using Reverse Polish Notation.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Value of the expression in the form of a polynomial.</returns>
        public Polynomial CalculatePoly(string expression)
        {
            List<Token> tokens = Token.GetTokens(expression, this);
            List<string> rpn = ToRPN(tokens);
            Stack<Polynomial> stackPoly = new Stack<Polynomial>();
            Polynomial solutionPoly;

            for (int i = 0; i < rpn.Count; i++)
            {
                if (!Token.IsOper(rpn[i]))
                {
                    if (vars.ContainsKey(rpn[i]))
                        stackPoly.Push(vars.GetValueOrDefault(rpn[i]));
                    else
                        stackPoly.Push(rpn[i]);
                }
                else
                    stackPoly.Push(Token.Solution(stackPoly, rpn[i]));
            }

            if (stackPoly.Count > 1)
                throw new Exception("The number of variables is more than the operators require!");

            solutionPoly = stackPoly.Pop();

            return solutionPoly;
        }
    }
}
