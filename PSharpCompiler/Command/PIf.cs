using PSharpCompiler;
using System;
using PBool;

namespace PCommand
{
    /// <summary>
    /// Реализует логический оператор ветвления.
    /// </summary>
    class PIf : Command
    {
        /// <summary>
        /// Результат логического выражения, используемый оператором.
        /// </summary>
        private bool solution;

        /// <summary>
        /// Инициализирует логический оператор.
        /// </summary>
        /// <param name="compiler"></param>
        public PIf(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand > compiler.ListCommand.Count - 3)
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(50);

            if (compiler.NumCommand < compiler.ListCommand.Count - 1 && (compiler.ListCommand[compiler.NumCommand + 1][0] != '(' ||
                compiler.ListCommand[compiler.NumCommand + 1][^1] != ')'))
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

            if (compiler.NumCommand < compiler.ListCommand.Count - 2 && compiler.ListCommand[compiler.NumCommand + 2] != "{")
            {
                outputError += (compiler.NumCommand + 2) + ErrorProcessingStr.ErrorCodeToStr(51);

                if (compiler.RewindBrace(compiler.NumCommand + 3) == compiler.ListCommand.Count)
                    outputError += (compiler.NumCommand + 2) + ErrorProcessingStr.ErrorCodeToStr(52);
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
        /// Исполняет логический оператор.
        /// </summary>
        public override void Run()
        {
            if (!Check())
                compiler.NumCommand ++;
            else
            {
                if (solution)
                    compiler.NumCommand += 3;
                else
                    compiler.NumCommand = compiler.RewindBrace(compiler.NumCommand + 3);
            }
        }
    }
}
