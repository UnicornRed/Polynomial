using PSharpCompiler;
using System.Text.RegularExpressions;

namespace PCommand
{
    /// <summary>
    /// Реализует оператор последовательного исполнения команд.
    /// </summary>
    class PPlusOne : Command
    {
        /// <summary>
        /// Инициализирует оператор последовательного исполнения команд.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PPlusOne(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";
            string commandNow = compiler.ListCommand[compiler.NumCommand];

            if (commandNow != ";" && commandNow != "{" && commandNow != "}" && commandNow != "to" &&
                !Regex.IsMatch(commandNow, @"^[a-zA-Z][a-zA-Z0-9_]*\[(\d+|[a-zA-Z][a-zA-Z0-9_]*)\]\[(\d+|[a-zA-Z][a-zA-Z0-9_]*)\]$") &&
                !Regex.IsMatch(commandNow, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(30);

            if (outputError == "")
                return true;
            else
            {
                compiler.OutputError += outputError;

                return false;
            }
        }

        /// <summary>
        /// Исполняет оператор последовательного исполнения команд.
        /// </summary>
        public override void Run()
        {
            Check();
            compiler.NumCommand++;
        }
    }
}
