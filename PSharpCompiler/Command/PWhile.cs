using PSharpCompiler;
using PBool;
using System;
using System.Text.RegularExpressions;

namespace PCommand
{
    /// <summary>
    /// Реализует оператор цикла.
    /// </summary>
    class PWhile : Command
    {
        /// <summary>
        /// Результат логического выражения, используемого оператором.
        /// </summary>
        private bool solution;

        /// <summary>
        /// Инициализирует оператор оператора цикла.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PWhile(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Определяет, поставлен ли оператор goto для зацикливания оператора.
        /// </summary>
        /// <param name="numGoto">Номер анализируемой команды.</param>
        /// <returns>true, если goto поставлен для нужд оператора, false в остальных случаях.</returns>
        private bool My_Goto(int numGoto)
        {
            if (numGoto > compiler.ListCommand.Count - 3 || compiler.ListCommand[numGoto] != "goto" ||
                !Regex.IsMatch(compiler.ListCommand[numGoto + 1], @"^\(\d+\)$") || compiler.ListCommand[numGoto + 2] != ";")
                return false;
            else
            {
                if (Convert.ToInt32(compiler.ListCommand[numGoto + 1].Substring(1, compiler.ListCommand[numGoto + 1].Length - 2)) != compiler.NumCommand)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand > compiler.ListCommand.Count - 3)
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(80);

            if (compiler.NumCommand < compiler.ListCommand.Count - 1 && (compiler.ListCommand[compiler.NumCommand + 1][0] != '(' ||
                compiler.ListCommand[compiler.NumCommand + 1][compiler.ListCommand[compiler.NumCommand + 1].Length - 1] != ')'))
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(53);

            try
            {
                solution = (new PBoolExpression(compiler.ListCommand[compiler.NumCommand + 1])).Value(compiler.Calc);
            }
            catch (Exception e)
            {
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(60);
                outputError += e.Message + "\n";
            }

            if (outputError == "")
                return true;
            else
            {
                compiler.OutputError += outputError;

                return false;
            }
        }

        /// <summary>
        /// Исполняет оператор цикла.
        /// </summary>
        public override void Run()
        {
            if (!Check())
                compiler.NumCommand += 2;
            else
            {
                int endWhile = compiler.RewindBrace(compiler.NumCommand + 3) + 1;

                if (!My_Goto(endWhile))
                {
                    compiler.ListCommand.Insert(endWhile, ";");
                    compiler.ListCommand.Insert(endWhile, "(" + compiler.NumCommand + ")");
                    compiler.ListCommand.Insert(endWhile, "goto");
                }

                if (solution)
                    compiler.NumCommand += 3;
                else
                {
                    if (My_Goto(endWhile))
                        compiler.ListCommand.RemoveRange(endWhile, 3);

                    compiler.NumCommand = endWhile;
                }
            }
        }
    }
}