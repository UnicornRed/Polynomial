using PSharpCompiler;

namespace PCommand
{
    /// <summary>
    /// Реализует оператор полиномиального типа.
    /// </summary>
    class PPolType : Command
    {
        /// <summary>
        /// Инициализирует оператор полиномиального типа.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PPolType(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand > compiler.ListCommand.Count - 3)
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(40);

            if (compiler.Calc.ContainVar(compiler.ListCommand[compiler.NumCommand + 1]))
                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(41);

            if (compiler.NumCommand < compiler.ListCommand.Count - 2 && compiler.ListCommand[compiler.NumCommand + 2] != ";" &&
                compiler.ListCommand[compiler.NumCommand + 2] != ":=")
                outputError += (compiler.NumCommand + 2) + ErrorProcessingStr.ErrorCodeToStr(5);
            else
            {
                if (compiler.NumCommand < compiler.ListCommand.Count - 4 && compiler.ListCommand[compiler.NumCommand + 4] != ";" &&
                    compiler.ListCommand[compiler.NumCommand + 2] == ":=")
                    outputError += (compiler.NumCommand + 4) + ErrorProcessingStr.ErrorCodeToStr(5);
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
        /// Исполняет оператор полиномиального типа.
        /// </summary>
        public override void Run()
        {
            if (!Check())
                compiler.NumCommand += compiler.RewindSemicolon(compiler.NumCommand);
            else
            {
                compiler.Calc.NewPolyVar(compiler.ListCommand[compiler.NumCommand + 1] + "= 0");
                compiler.NumCommand += 2;
            }
        }
    }
}
