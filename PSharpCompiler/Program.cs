using System;
using System.IO;

namespace PSharpCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string code, end, numStrStr, typeDo, filename;
            int numStr;
            bool contProgramm = true;

            while (contProgramm)
            {
                Console.WriteLine("Выберите действие (1 - ручной ввод; 2 - файловый ввод; 3 - выход):");

                code = "";
                numStr = 1;
                typeDo = Console.ReadLine();

                switch (typeDo)
                {
                    case "1":
                        Console.WriteLine("Наберите код (для завершения набора введите на отдельной строке \"end\"):");

                        Console.Write("  1| ");
                        end = Console.ReadLine();

                        while (end != "end")
                        {
                            numStr++;
                            int numStrHelp = numStr;
                            numStrStr = "   " + numStr + "| ";

                            while (numStrHelp != 0)
                            {
                                numStrStr = numStrStr.Substring(1);
                                numStrHelp /= 10;
                            }

                            Console.Write(numStrStr);

                            code += end + "\n";
                            end = Console.ReadLine();
                        }

                        Console.WriteLine("Сохранить код в файл? (да|нет)");
                        string saveIs = Console.ReadLine();

                        if (saveIs == "да")
                        {
                            Console.WriteLine("Введите имя файла:");

                            filename = Console.ReadLine();

                            try
                            {
                                System.IO.File.WriteAllText("..\\..\\..\\" + filename, code);
                            }
                            catch
                            {
                                Console.WriteLine("Не удалось записать в файл.");
                            }
                        }

                        break;

                    case "2":
                        Console.WriteLine("Введите имя файла:");

                        filename = Console.ReadLine();

                        try
                        {
                            using (StreamReader sr = new StreamReader("..\\..\\..\\" + filename))
                            {
                                Console.WriteLine("Прочитанный код:");
                                while ((end = sr.ReadLine()) != null)
                                {
                                    int numStrHelp = numStr;
                                    numStrStr = "   " + numStr + "| ";

                                    while (numStrHelp != 0)
                                    {
                                        numStrStr = numStrStr.Substring(1);
                                        numStrHelp /= 10;
                                    }

                                    numStr++;

                                    Console.WriteLine(numStrStr + end);

                                    code += end + "\n";
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Не удалось прочесть файл.");
                        }

                        break;

                    case "3":
                        contProgramm = false;

                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.");

                        break;
                }

                if (code != "")
                {
                    Compiler c = new Compiler(code);
                    c.CompileCode();

                    Console.WriteLine("\nВывод:\n" + c.OutputStr);
                    Console.WriteLine("Ошибки:\n" + c.OutputError);
                }
            }

            Console.WriteLine("Сеанс завершён.");
        }
    }
}