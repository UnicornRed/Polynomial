using System;
using Polynomials;
using PSharpCompiler;
using MatrixType;
using System.Text.RegularExpressions;

namespace PCommand
{
    /// <summary>
    /// Реализует оператор присваивания.
    /// </summary>
    class PAssign : Command
    {
        /// <summary>
        /// Результат выражения, присваиваемый оператором.
        /// </summary>
        private object solution;

        /// <summary>
        /// Разбиение команды перед оператором присваивания на составные части.
        /// </summary>
        private string[] nameHeightWidth;

        /// <summary>
        /// Первый индекс коэффициента матрицы.
        /// </summary>
        int height;

        /// <summary>
        /// Второй индекс коэффициента матрицы
        /// </summary>
        int width;

        /// <summary>
        /// Инициализирует оператор присваивания.
        /// </summary>
        /// <param name="compiler">Транслятор.</param>
        public PAssign(Compiler compiler) : base(compiler) { }

        /// <summary>
        /// Проверяет оператор на корректность его вызова.
        /// </summary>
        /// <returns>true, если оператор выполняется корректно, false в остальных случаях.</returns>
        public override bool Check()
        {
            string outputError = "";

            if (compiler.NumCommand == 0 || compiler.NumCommand > compiler.ListCommand.Count - 3)
                compiler.OutputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(10);

            if (compiler.NumCommand < compiler.ListCommand.Count - 2 && compiler.ListCommand[compiler.NumCommand + 2] != ";")
                compiler.OutputError += (compiler.NumCommand + 2) + ErrorProcessingStr.ErrorCodeToStr(5);

            if (compiler.NumCommand > 0)
            {
                string varNow = compiler.ListCommand[compiler.NumCommand - 1];
                nameHeightWidth = varNow.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                if (!compiler.Calc.ContainVar(nameHeightWidth[0]))
                    outputError += (compiler.NumCommand - 1) + ErrorProcessingStr.ErrorCodeToStr(11) + nameHeightWidth[0] + "\"\n";
                else
                {
                    try
                    {
                        solution = compiler.Calc.Calculate(compiler.ListCommand[compiler.NumCommand + 1]);
                    }
                    catch (Exception e)
                    {
                        outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(60);
                        outputError += e.Message + "\n";
                    }

                    if (compiler.Calc[nameHeightWidth[0]].VarCalc.GetType() == typeof(Polynomial))
                    {
                        if (Regex.IsMatch(varNow, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
                        {
                            if (solution.GetType() != typeof(Polynomial))
                                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(12);
                        }
                        else
                            outputError += (compiler.NumCommand - 1) + ErrorProcessingStr.ErrorCodeToStr(16);
                    }

                    if (compiler.Calc[nameHeightWidth[0]].VarCalc.GetType() == typeof(MatrixPolynomial))
                    {
                        if (Regex.IsMatch(varNow, @"^[a-zA-Z][a-zA-Z0-9_]*\[(\d+|[a-zA-Z][a-zA-Z0-9_]*)\]\[(\d+|[a-zA-Z][a-zA-Z0-9_]*)\]$"))
                        {
                            if (solution.GetType() != typeof(Polynomial))
                                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(13);

                            try
                            {
                                height = (int)((Polynomial)compiler.Calc.Calculate(nameHeightWidth[1]))[0];
                            }
                            catch (Exception e)
                            {
                                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(60);
                                outputError += e.Message + "\n";
                            }

                            try
                            {
                                width = (int)((Polynomial)compiler.Calc.Calculate(nameHeightWidth[2]))[0];
                            }
                            catch (Exception e)
                            {
                                outputError += (compiler.NumCommand + 1) + ErrorProcessingStr.ErrorCodeToStr(60);
                                outputError += e.Message + "\n";
                            }

                            try
                            {
                                compiler.Calc.AssignCoeffMatrix(nameHeightWidth[0], height, width, (Polynomial)"0");
                            }
                            catch
                            {
                                outputError += (compiler.NumCommand - 1) + ErrorProcessingStr.ErrorCodeToStr(61);
                            }
                        }
                        else if (Regex.IsMatch(varNow, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
                        {
                            if (solution.GetType() != typeof(MatrixPolynomial))
                                outputError += compiler.NumCommand + ErrorProcessingStr.ErrorCodeToStr(14);
                        }
                        else
                            outputError += (compiler.NumCommand - 1) + ErrorProcessingStr.ErrorCodeToStr(15);
                    }
                }

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
        /// Исполняет оператор присваивания.
        /// </summary>
        public override void Run()
        {
            if (!Check())
                compiler.NumCommand = compiler.RewindSemicolon(compiler.NumCommand);
            else
            {
                if (compiler.Calc[nameHeightWidth[0]].VarCalc.GetType() == typeof(Polynomial))
                {
                    compiler.Calc.AssignVar(nameHeightWidth[0], (Polynomial)solution);
                }
                else if (compiler.Calc[nameHeightWidth[0]].VarCalc.GetType() == typeof(MatrixPolynomial))
                {
                    if (nameHeightWidth.Length == 1)
                        compiler.Calc.AssignVar(nameHeightWidth[0], (MatrixPolynomial)solution);
                    else
                        compiler.Calc.AssignCoeffMatrix(nameHeightWidth[0], height, width, (Polynomial)solution);
                }

                compiler.NumCommand += 3;
            }
        }
    }
}
