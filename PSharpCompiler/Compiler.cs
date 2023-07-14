using System;
using Calculating;
using PCommand;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PSharpCompiler
{
    /// <summary>
    /// Реализует транслятор кода на языке P#.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// Строки кода.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Номер текущей команды.
        /// </summary>
        public int NumCommand { get; set; }

        /// <summary>
        /// Исполняемые команды.
        /// </summary>
        public Dictionary<string, Command> Orders { get; }

        /// <summary>
        /// Список имён команд.
        /// </summary>
        public List<string> ListCommand { get; }

        /// <summary>
        /// Калькулятор полиномов и матриц.
        /// </summary>
        public CalculatingExpressions Calc { get; }

        /// <summary>
        /// Строки вывода.
        /// </summary>
        public string OutputStr { get; set; }

        /// <summary>
        /// Строки ошибок.
        /// </summary>
        public string OutputError { get; set; }

        /// <summary>
        /// Инициализирует транслятор по строке.
        /// </summary>
        /// <param name="code">Строки кода.</param>
        public Compiler(string code)
        {
            OutputStr = "";
            OutputError = "";
            this.Code = code;
            NumCommand = 0;
            Calc = new CalculatingExpressions();

            Orders = new Dictionary<string, Command>()
            { { ":=", new PAssign(this)},
              { "if", new PIf(this)},
              { "goto", new PGoto(this)},
              { "matrix", new PMatrixType(this)},
              { "pol", new PPolType(this)},
              { "while", new PWhile(this)},
              { "write", new PWrite(this)},
              { "to", new PPlusOne(this)},
              { "{", new PPlusOne(this)},
              { "}", new PPlusOne(this)},
              { ";", new PPlusOne(this)}};

            ListCommand = ParserCommandClass.ParserCommand(code, Orders);
        }

        /// <summary>
        /// Команда по имени.
        /// </summary>
        /// <param name="nameCommand">Имя команды.</param>
        /// <returns>Исполняемую команду по её имени.</returns>
        private Command GetCommand(string nameCommand)
        {
            if (Orders.ContainsKey(nameCommand))
                return Orders[nameCommand];
            else
                return Orders["to"];
        }

        /// <summary>
        /// Находит номер строки кода по номеру команды.
        /// </summary>
        /// <param name="numCommand">Номер команды.</param>
        /// <returns>Номер строки, на которой находится команда.</returns>
        private int NumString(int numCommand)
        {
            int numString = 0;
            string codeWithoutSpaces = Code.Replace(" ", "");

            for (int i = 0, j = 0; i <= numCommand && j < codeWithoutSpaces.Length; i ++)
            {
                while (j < codeWithoutSpaces.Length && codeWithoutSpaces[j] == '\n')
                {
                    if (codeWithoutSpaces[j] == '\n')
                        numString++;

                    j++;
                }

                j += ListCommand[i].Replace(" ", "").Length;
            }

            return numString;
        }

        /// <summary>
        /// Преобразовывает строку ошибок с номерами команд в строку ошибок с номерами строк.
        /// </summary>
        private void ErrorProcessing()
        {
            if (OutputError == "")
                return;

            string[] errorStr = OutputError.Split(new char[] { '\n', '#' }, StringSplitOptions.RemoveEmptyEntries);
            OutputError = "";

            for (int i = 0; i < errorStr.Length; i ++)
            {
                while (!Regex.IsMatch(errorStr[i], @"^\d+$"))
                    i++;

                int numString = NumString(Convert.ToInt32(errorStr[i]));
                OutputError += "String [" + (numString + 1) + "]: ";
                i++;

                while (i < errorStr.Length && !Regex.IsMatch(errorStr[i], @"^\d+$"))
                {
                    OutputError += errorStr[i] + "\n";
                    i++;
                }

                i--;
            }
        }

        /// <summary>
        /// Определяет конец области, ограниченной фигурными скобками.
        /// </summary>
        /// <param name="numCommand">Номер текущей команды.</param>
        /// <returns>Номер команды конца области.</returns>
        public int RewindBrace(int numCommand)
        {
            for (int i = numCommand; i < ListCommand.Count; i++)
            {
                if (ListCommand[i] == "}")
                    return i;

                if (ListCommand[i] == "{")
                    i = RewindBrace(i + 1);
            }

            return ListCommand.Count;
        }

        /// <summary>
        /// Определяет место первой точки с запятой, начиная с переданного момента.
        /// </summary>
        /// <param name="numCommand">Номер текущей команды.</param>
        /// <returns>Номер команды первой встреченной точки с запятой.</returns>
        public int RewindSemicolon(int numCommand)
        {
            for (int i = numCommand; i < ListCommand.Count; i++)
                if (ListCommand[i] == ";")
                    return i + 1;

            return ListCommand.Count;
        }

        /// <summary>
        /// Транслирует код.
        /// </summary>
        public void CompileCode()
        {
            while (NumCommand < ListCommand.Count)
                GetCommand(ListCommand[NumCommand]).Run();

            ErrorProcessing();
        }
    }
}
