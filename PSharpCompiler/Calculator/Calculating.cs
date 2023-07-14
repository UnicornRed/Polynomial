using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Polynomials;
using MatrixType;

namespace Calculating
{
    /// <summary>
    /// Реализует вычисление математических выражений.
    /// </summary>
    public class CalculatingExpressions : IEnumerable<VarsCalc>
    {
        /// <summary>
        /// Именованные переменные, используемые в выражении.
        /// </summary>
        private readonly List<VarsCalc> vars;

        /// <summary>
        /// Именованные переменные, используемые в выражении.
        /// </summary>
        public List<VarsCalc> Vars
        {
            get
            {
                return vars;
            }
        }

        /// <summary>
        /// Получение именнованной переменной по имени.
        /// </summary>
        /// <param name="i">Имя переменной.</param>
        /// <returns>Именнованную переменную.</returns>
        public VarsCalc this[string i]
        {
            get
            {
                return VarsCalc.GetVarsCalcByKey(i, vars);
            }
        }

        /// <summary>
        /// Интерфейс перечисления для переменных.
        /// </summary>
        /// <returns>Список переменных.</returns>
        public IEnumerator<VarsCalc> GetEnumerator()
        {
            return vars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        /// <summary>
        /// Инициализирует калькулятор по умолчанию.
        /// </summary>
        public CalculatingExpressions()
        {
            vars = new List<VarsCalc>();
        }

        /// <summary>
        /// Добавляет новую полиномиальную переменную по переданной строке.
        /// </summary>
        /// <param name="variable">Строка формата: [Name] = [Polynomial]</param>
        public void NewPolyVar(string variable)
        {
            string[] nameAndPoly = variable.Split(new char[] { '=' });

            if (nameAndPoly.Length != 2)
                throw new Exception("Incorrect format!\n" +
                                    "The variable should have the form: [Name] = [Object]");
            else
            {
                Polynomial polyNow = new Polynomial(nameAndPoly[1]);

                if (Regex.IsMatch(nameAndPoly[0], @"^\s*[a-zA-Z][a-zA-Z0-9_]*\s*$"))
                    vars.Add(new VarsCalc(nameAndPoly[0].Replace(" ", ""), polyNow));
                else
                    throw new Exception("Variable names must begin with a letter and contain letters, numbers, and underscores.");
            }
        }

        /// <summary>
        /// Добавляет новую матричную переменную по переданной строке.
        /// </summary>
        /// <param name="variable">Строка формата: [Name] = ([height], [width])</param>
        public void NewMatrixVar(string variable)
        {
            string[] nameHeightWidth = variable.Split(new char[] { '=' , '(' , ')' , ',' , ' '}, StringSplitOptions.RemoveEmptyEntries);

            if (nameHeightWidth.Length != 3)
                throw new Exception("Incorrect format!\n" +
                                    "The variable should have the form: [Name] = ([height], [width])");
            else
            {
                MatrixPolynomial matrixNow = new MatrixPolynomial(Convert.ToInt32(nameHeightWidth[1]), Convert.ToInt32(nameHeightWidth[2]),
                                                                new SortedList<int, SortedList<int, Polynomial>>());

                if (Regex.IsMatch(nameHeightWidth[0], @"^\s*[a-zA-Z][a-zA-Z0-9_]*\s*$"))
                    vars.Add(new VarsCalc(nameHeightWidth[0].Replace(" ", ""), matrixNow));
                else
                    throw new Exception("Variable names must begin with a letter and contain letters, numbers, and underscores.");
            }
        }

        /// <summary>
        /// Удаляет переменную по её имени.
        /// </summary>
        /// <param name="variable">Имя переменной.</param>
        public void DeleteVar(string variable)
        {
            if (!vars.Remove(VarsCalc.GetVarsCalcByKey(variable, vars)))
                throw new Exception("The object does not exist in the list of variables.");
        }

        /// <summary>
        /// Определяет нахождение переменной в калькулятор.
        /// </summary>
        /// <param name="variable">Имя переменной.</param>
        /// <returns>true, если переменная с таким именем существует, false, если нет.</returns>
        public bool ContainVar(string variable) => !VarsCalc.IsIdFree(variable, vars);

        /// <summary>
        /// Присваивает значение переменной.
        /// </summary>
        /// <param name="variable">Имя переменной.</param>
        /// <param name="valueVar">Присваиваемое значение.</param>
        public void AssignVar(string variable, object valueVar)
        {
            if (this.ContainVar(variable))
                VarsCalc.GetVarsCalcByKey(variable, vars).VarCalc = valueVar;
            else
                throw new Exception("The object does not exist in the list of variables.");
        }

        /// <summary>
        /// Присваивает значение коэффициенту матрицы.
        /// </summary>
        /// <param name="variable">Имя матрицы.</param>
        /// <param name="i">Первый коэффициент.</param>
        /// <param name="j">Второй коэффициент.</param>
        /// <param name="valueVar">Присваиваемое значение.</param>
        public void AssignCoeffMatrix(string variable, int i, int j, object valueVar)
        {
            if (this.ContainVar(variable))
                ((MatrixPolynomial)(VarsCalc.GetVarsCalcByKey(variable, vars).VarCalc)).SetIJ(i, j, (Polynomial)valueVar);
            else
                throw new Exception("The object does not exist in the list of variables.");
        }

        /// <summary>
        /// Переводит выражение в обратную польскую запись.
        /// </summary>
        /// <param name="tokens">Элементы выражения.</param>
        /// <returns>Список элементов выражения в обратной польской записи.</returns>
        private List<string> ToRPN(List<Token> tokens)
        {
            List<string> rpn = new List<string>();
            Stack<string> oper = new Stack<string>();

            foreach (var i in tokens)
            {
                TypeOperation whatToken = Token.WhatToken(i.Name, this);

                switch (whatToken)
                {
                    case TypeOperation.Id:
                        rpn.Add(i.Name);

                        break;

                    case TypeOperation.OpeningParenthesis:
                    case TypeOperation.UnaFunc:
                    case TypeOperation.BinFunc:
                    case TypeOperation.TerFunc:
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

                            if (isFun == TypeOperation.UnaFunc ||
                                isFun == TypeOperation.BinFunc ||
                                isFun == TypeOperation.TerFunc)
                                rpn.Add(oper.Pop());
                        }

                        break;

                    case TypeOperation.BinOper:
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
        /// Вычисляет математическое выражение, используя обратную польскую запись.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Значение выражения.</returns>
        public object Calculate(string expression)
        {
            List<Token> tokens = Token.GetTokens(expression, this);
            List<string> rpn = ToRPN(tokens);
            Stack<object> stackVars = new Stack<object>();
            object solution;

            for (int i = 0; i < rpn.Count; i++)
            {
                if (!Token.IsOper(rpn[i]))
                {
                    if (VarsCalc.IsIdFree(rpn[i], vars))
                        stackVars.Push((Polynomial)rpn[i]);
                    else
                        stackVars.Push(VarsCalc.GetVarsCalcByKey(rpn[i], vars).VarCalc);
                }
                else
                    stackVars.Push(Token.Solution(stackVars, rpn[i]));
            }

            if (stackVars.Count > 1)
                throw new Exception("The number of variables is more than the operators require!");

            solution = stackVars.Pop();

            return solution;
        }
    }
}
