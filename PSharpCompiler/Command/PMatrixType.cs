using PSharpCompiler;
using System.Text.RegularExpressions;

namespace PCommand
{
    /// <summary>
    /// Реализует матричный тип данных.
    /// </summary>
    class PMatrixType : Command
    {
        /// <summary>
        /// Инициализирует матричный тип данных.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PMatrixType(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор матричного типа на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand > compiler.ListCommand.Count - 5 || compiler.ListCommand[compiler.NumCommand + 2] != ":=")
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(45);

            if (compiler.Calc.ContainVar(compiler.ListCommand[compiler.NumCommand + 1]))
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(41);

            if (compiler.NumCommand < compiler.ListCommand.Count - 3 && !Regex.IsMatch(compiler.ListCommand[compiler.NumCommand + 3], @"\(\d+,\d+\)"))
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(46);

            if (compiler.NumCommand < compiler.ListCommand.Count - 4 && compiler.ListCommand[compiler.NumCommand + 4] != ";")
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(5);

            if (outputError == "")
                return true;
            else
            {
                compiler.OutputError += outputError;

                return false;
            }
        }

        /// <summary>
        /// Исполняет оператор матричного типа.
        /// </summary>
        public override void Run()
        {
            if (!Check())
                compiler.NumCommand += compiler.RewindSemicolon(compiler.NumCommand);
            else
            {
                compiler.Calc.NewMatrixVar(compiler.ListCommand[compiler.NumCommand + 1] + "=" + compiler.ListCommand[compiler.NumCommand + 3]);
                compiler.NumCommand += 5;
            }
        }
    }
}
