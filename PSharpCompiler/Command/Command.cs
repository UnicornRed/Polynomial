using PSharpCompiler;

namespace PCommand
{
    /// <summary>
    /// Реализует общий тип команд.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Ссылка на исполняемый транслятор.
        /// </summary>
        protected Compiler compiler;

        /// <summary>
        /// Инициализирует оператор безусловного перехода.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public Command(Compiler compiler) => this.compiler = compiler;

        /// <summary>
        /// Метод проверки команд.
        /// </summary>
        public abstract bool Check();

        /// <summary>
        /// Метод исполнения команд.
        /// </summary>
        public abstract void Run();
    }
}
