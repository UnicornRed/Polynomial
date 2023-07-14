using System;
using System.Collections.Generic;

namespace PBool
{
    /// <summary>
    /// Реализует вычисление логических выражений.
    /// </summary>
    class BoolCalculating
    {
        /// <summary>
        /// Используемые в выражении операторы.
        /// </summary>
        private static readonly string[] oper = new string[] { "&", "|", "!", ")", "(" };

        /// <summary>
        /// Преобразование строки в логическое значение.
        /// </summary>
        /// <param name="boolToken">Преобразуемая строка.</param>
        /// <returns>Полученное логическое значение.</returns>
        private bool StringToBool(string boolToken)
        {
            if (boolToken == "true")
                return true;

            if (boolToken == "false")
                return false;

            return false;
        }

        /// <summary>
        /// Приоритет логической операции.
        /// </summary>
        /// <param name="operToken">Имя операции.</param>
        /// <returns>Приоритет операции.</returns>
        private int Priority(string operToken)
        {
            if (operToken == "&")
                return 2;

            if (operToken == "|")
                return 1;

            return 0;
        }

        /// <summary>
        /// Определяет, является ли элемент выражения операцией.
        /// </summary>
        /// <param name="token">Операция.</param>
        /// <returns>true, если элемент выражение - это операция, false в остальных случаях.</returns>
        private bool IsBoolOper(string token)
        {
            foreach (var i in oper)
                if (token == i)
                    return true;

            return false;
        }

        /// <summary>
        /// Вычисляет значение логической операции.
        /// </summary>
        /// <param name="stackBool">Стек логических переменных.</param>
        /// <param name="operToken">Операция.</param>
        /// <returns>Значение операции.</returns>
        private bool Solution(Stack<bool> stackBool, string operToken)
        {
            if (stackBool.Count == 0)
                throw new Exception("Incorrect spelling of a logical expression.");

            if (operToken == "!")
                return !stackBool.Pop();

            if (stackBool.Count == 0)
                throw new Exception("Incorrect spelling of a logical expression.");

            if (operToken == "&")
                return stackBool.Pop() && stackBool.Pop();

            if (operToken == "|")
                return stackBool.Pop() | stackBool.Pop();

            return false;
        }

        /// <summary>
        /// Делит логическое выражение на элементы.
        /// </summary>
        /// <param name="boolExp">Строка с логическим выражением.</param>
        /// <returns>Список элементов выражения.</returns>
        private List<string> ParserBool(string boolExp)
        {
            boolExp = boolExp.Replace(" ", "");

            foreach (var i in oper)
                boolExp = boolExp.Replace(i, " " + i + " ");

            List<string> boolToken = new List<string>();
            string[] boolTokenStr = boolExp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var i in boolTokenStr)
                boolToken.Add(i);

            return boolToken;
        }

        /// <summary>
        /// Переводит список элементов выражения в обратную польскую запись.
        /// </summary>
        /// <param name="tokens">Список элементов выражения.</param>
        /// <returns>Список элементов выражения в обратной польской записи.</returns>
        private List<string> ToRPN(List<string> tokens)
        {
            List<string> rpn = new List<string>();
            Stack<string> oper = new Stack<string>();

            foreach (var i in tokens)
            {
                switch (i)
                {
                    case "(":
                        oper.Push(i);

                        break;

                    case ")":
                        while (oper.Count != 0 && oper.Peek() != "(")
                            rpn.Add(oper.Pop());

                        if (oper.Count != 0)
                            oper.Pop();

                        if (oper.Count != 0 && oper.Peek() == "!")
                            rpn.Add(oper.Pop());

                        break;

                    case "!":
                        oper.Push(i);

                        break;

                    case "&":
                    case "|":
                        while (oper.Count != 0 &&
                               Priority(oper.Peek()) >= Priority(i))
                            rpn.Add(oper.Pop());

                        oper.Push(i);

                        break;

                    default:
                        rpn.Add(i);

                        break;
                }
            }

            while (oper.Count != 0)
                rpn.Add(oper.Pop());

            return rpn;
        }

        /// <summary>
        /// Вычисляет логическое выражение, используя обратную польскую запись.
        /// </summary>
        /// <param name="boolExp">Строка с логическим выражением.</param>
        /// <returns>Значение выражения.</returns>
        public bool CalculateBool(string boolExp)
        {
            List<string> tokens = ParserBool(boolExp);
            List<string> rpn = ToRPN(tokens);
            Stack<bool> stackBool = new Stack<bool>();
            bool solutionBool;

            for (int i = 0; i < rpn.Count; i++)
            {
                if (IsBoolOper(rpn[i]))
                    stackBool.Push(Solution(stackBool, rpn[i]));
                else
                    stackBool.Push(StringToBool(rpn[i]));
            }

            if (stackBool.Count > 1)
                throw new Exception("The number of variables is more than the operators require!");

            solutionBool = stackBool.Pop();

            return solutionBool;
        }
    }
}
