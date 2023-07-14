using Polynomials;
using PSharpCompiler;
using System;

namespace PCommand
{
    /// <summary>
    /// Реализует оператор вывода вычисляемых выражений.
    /// </summary>
    class PWrite : Command
    {
        /// <summary>
        /// Результат выражения, выводимый оператором.
        /// </summary>
        private object solution;

        /// <summary>
        /// Инициализирует оператор вывода.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PWrite(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand > compiler.ListCommand.Count - 3 || compiler.ListCommand[compiler.NumCommand + 1][0] != '(' ||
                compiler.ListCommand[compiler.NumCommand + 1][compiler.ListCommand[compiler.NumCommand + 1].Length - 1] != ')')
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(70);

            try
            {
                solution = compiler.Calc.Calculate(compiler.ListCommand[compiler.NumCommand + 1]);

                if (solution.GetType() != typeof(Polynomial))
                    outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(62);
            }
            catch (Exception e)
            {
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(60);
                outputError += e.Message + "\n";
            }

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
        /// Исполняет оператор вывода.
        /// </summary>
        public override void Run()
        {
            if (!Check())
                compiler.NumCommand += compiler.RewindSemicolon(compiler.NumCommand);
            else
            {
                compiler.OutputStr += ((Polynomial)solution).ToString() + "\n";
                compiler.NumCommand += 3;
            }
        }
    }
}
