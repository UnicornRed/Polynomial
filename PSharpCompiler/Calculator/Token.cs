using System.Collections.Generic;
using System.Text.RegularExpressions;
using Polynomials;
using MatrixType;
using TokenParsers;

/// <summary>
/// Перечисляемый тип видов элементов выражения.
/// </summary>
enum TypeOperation
{
    /// <summary>
    /// Переменная или полином.
    /// </summary>
    Id,
    /// <summary>
    /// Открывающая скобка.
    /// </summary>
    OpeningParenthesis,
    /// <summary>
    /// Закрывающая скобка.
    /// </summary>
    СlosingParenthesis,
    /// <summary>
    /// Бинарная операция.
    /// </summary>
    BinOper,
    /// <summary>
    /// Тернарная операция.
    /// </summary>
    TerFunc,
    Comma,
    UnaFunc,
    BinFunc,
    ZeroFunc,
    Error
}

namespace Calculating
{
    /// <summary>
    /// Реализует обработку элементов выражения.
    /// </summary>
    class Token
    {
        /// <summary>
        /// Обозначает элемент выражения.
        /// </summary>
        readonly private string token;

        /// <summary>
        /// Операторы, используемые в выражении.
        /// </summary>
        readonly private static HashSet<FuncAndOper> oper = FuncAndOper.GetAllFuncAndOper();

        /// <summary>
        /// Имя элемента выражения.
        /// </summary>
        public string Name
        {
            get
            {
                return token;
            }
        }

        /// <summary>
        /// Инициализирует элемент выражения.
        /// </summary>
        /// <param name="token">Элемент выражения.</param>
        public Token(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// Получает приоритет операции.
        /// </summary>
        /// <param name="name">Название элемента выражения.</param>
        /// <returns>Приоритет операции или -1, если name не является операцией.</returns>
        public static int Priority(string name)
        {
            foreach (var i in oper)
                if (i.Act == name)
                    return i.Priority;

            return -1;
        }

        /// <summary>
        /// Определяет, является ли элемент выражения операцией.
        /// </summary>
        /// <param name="name">Название элемента выражения.</param>
        /// <returns>true, если name является операцией, false, если не является.</returns>
        public static bool IsOper(string name)
        {
            foreach (var i in oper)
                if (i.Act == name)
                    return true;

            if (name == "(" ||
                name == ")" ||
                name == ",")
                return true;

            return false;
        }

        /// <summary>
        /// Определяет вид элемента выражения.
        /// </summary>
        /// <param name="name">Название элемента выражения.</param>
        /// <param name="calc">Калькулятор, хранящий переменные.</param>
        /// <returns>Id, если элемент выражения не является оператором,
        /// OpeningParenthesis, если это открывающая скобка, СlosingParenthesis, если это закрывающая скобка,
        /// Comma, если это запятая, и значение Type в остальных случаях.</returns>
        public static TypeOperation WhatToken(string name, CalculatingExpressions calc)
        {
            if (Regex.IsMatch(name, @"(^-?\d*(.\d+)?\*?x(\^\d+)?$)|(^-?\d+(.\d+)?$)$"))
                return TypeOperation.Id;

            foreach (var i in calc)
                if (i.Id == name)
                    return TypeOperation.Id;

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
                        if (i.Act == name)
                            return i.Type;

                    return TypeOperation.Error;
            }
        }

        /// <summary>
        /// Вычисляет переданную операцию, используя стек переменных.
        /// </summary>
        /// <param name="stackVars">Стек переменных.</param>
        /// <param name="nameOper">Название операции.</param>
        /// <returns>Результат операции.</returns>
        public static object Solution(Stack<object> stackVars, string nameOper)
        {
            foreach (var i in oper)
                if (i.Act == nameOper)
                {
                    if (i.Type == TypeOperation.TerFunc)
                        return i.Ter(stackVars.Pop(), stackVars.Pop(), stackVars.Pop());
                    else if ((i.Type == TypeOperation.BinOper ||
                              i.Type == TypeOperation.BinFunc) && (stackVars.Peek().GetType() == typeof(Polynomial)
                              && i.ArgType == "PolynomialPolynomial" || stackVars.Peek().GetType() == typeof(MatrixPolynomial)
                              && i.ArgType == "MatrixMatrix"))
                        return i.Bin(stackVars.Pop(), stackVars.Pop());
                    else if (i.Type == TypeOperation.UnaFunc)
                        return i.Una(stackVars.Pop());
                }

            return null;
        }

        /// <summary>
        /// Делит выражение на token.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="calc">Используемый калькулятор.</param>
        /// <returns>Список элементов выражения.</returns>
        public static List<Token> GetTokens(string expression, CalculatingExpressions calc)
        {
            List<Token> tokens = TokenCalcParser.TokenParser(expression);

            expression = expression.Replace(" ", "");

            ExceptionToken exceptionToken = new ExceptionToken(tokens, calc);

            exceptionToken.CheckExpression(expression);

            return tokens;
        }
    }
}
