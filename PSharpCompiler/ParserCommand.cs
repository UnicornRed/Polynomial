using System;
using System.Collections.Generic;
using ParserWork;
using PCommand;

namespace PSharpCompiler
{
    /// <summary>
    /// Реализует деление строк кода на команды.
    /// </summary>
    class ParserCommandClass
    {
        /// <summary>
        /// Делит строки кода по фиксированным словам.
        /// </summary>
        /// <param name="code">Строки кода.</param>
        /// <param name="reservedWord">Фиксированные слова.</param>
        /// <returns></returns>
        public static List<string> ParserCommand(string code, Dictionary<string, Command> orders)
        {
            int index, indexCode;
            string subStr;
            bool flag;
            code = " " + code + "\n";

            foreach (var i in orders)
            {
                subStr = code;
                indexCode = 0;

                do
                {
                    flag = false;
                    index = subStr.IndexOf(i.Key);

                    if (index != -1 && (!Char.IsLetter(subStr[index - 1]) &&
                        !Char.IsLetter(subStr[index + i.Key.Length]) || !Char.IsLetter(i.Key[0])))
                    {
                        code = code.Insert(indexCode + index, "\n");
                        index++;
                        indexCode += index + i.Key.Length;
                        code = code.Insert(indexCode, "\n");

                        subStr = code[indexCode..];
                        flag = true;
                    }

                } while (flag);
            }
            
            code = code.Replace(" ", "");

            string[] tokens = Parser.ParserForAll(code, "", new string[,] { },
                                       new string[] { ":=", "{", "}", ";"},
                                       new char[] { '\n' });

            List<string> listCommand = new List<string>();

            foreach (var i in tokens)
                listCommand.Add(i);

            return listCommand;
        }
    }
}
