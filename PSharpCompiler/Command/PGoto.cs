using System;
using PSharpCompiler;
using System.Text.RegularExpressions;

namespace PCommand
{
    /// <summary>
    /// Реализует оператор безусловного перехода.
    /// </summary>
    class PGoto : Command
    {
        /// <summary>
        /// Инициализирует оператор безусловного перехода.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PGoto(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand > compiler.ListCommand.Count - 3)
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(20);

            if (compiler.NumCommand < compiler.ListCommand.Count - 1 && !Regex.IsMatch(compiler.ListCommand[compiler.NumCommand + 1], @"^\(\d+\)$"))
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(21);

            if (compiler.NumCommand < compiler.ListCommand.Count - 2 && compiler.ListCommand[compiler.NumCommand + 2] != ";")
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(5);

            if (outputError == "")
                return true;
            else
            {
                compiler.OutputError += outputError;

                return false;
            }
        }

        /// <summary>
        /// Исполняет оператор безусловного перехода.
        /// </summary>
        public override void Run()
        {
            if (Check())
                compiler.NumCommand = Convert.ToInt32(compiler.ListCommand[compiler.NumCommand + 1].Substring(1, compiler.ListCommand[compiler.NumCommand + 1].Length - 2));
            else
                compiler.NumCommand = compiler.RewindSemicolon(compiler.NumCommand);
        }
    }
}
