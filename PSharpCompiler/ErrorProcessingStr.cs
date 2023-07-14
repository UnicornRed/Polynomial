namespace PSharpCompiler
{
    /// <summary>
    /// Реализует получение ошибки по её коду.
    /// </summary>
    class ErrorProcessingStr
    {
        /// <summary>
        /// Соотносит строку ошибки с кодом:
        /// 5 - отсутствие точки с запятой;
        /// 10 - неверный вызов присваивания;
        /// 11 - неизвестная переменная;
        /// 12 - присваивание полиномиальной переменной значения, не являющегося полиномом;
        /// 13 - присваивание коэффициенту матрицы значения, не являющегося полиномом;
        /// 14 - присваивание матричной переменной значения, не являющегося матрицей;
        /// 15 - неверное написание имени матрицы или матричного коэффициента в присваивании;
        /// 16 - неверное написание имени полинома в присваивании;
        /// 20 - неверной вызов goto;
        /// 21 - неверное написание номера команды в goto;
        /// 30 - неверное употребление команды;
        /// 40 - неверный вызов инициализации полинома;
        /// 41 - повторная инициализация переменной;
        /// 45 - неверный вызов инициализации матрицы;
        /// 46 - неверное написание высоты и ширины матрицы в инициализации;
        /// 50 - неверный вызов условного оператора;
        /// 51 - отсутствует открывающая скобка;
        /// 52 - отсутствует закрывающая скобка;
        /// 53 - неверное написание логического выражения;
        /// 60 - выражение не может быть вычислено;
        /// 61 - индекс превышает размер матрицы;
        /// 70 - неверный вызов оператора вывода;
        /// 80 - неверный вызов оператора цикла;
        /// default - неизвестный код ошибки.
        /// </summary>
        /// <param name="code">Код ошибки.</param>
        /// <returns>Строку ошибки.</returns>
        public static string ErrorCodeToStr(int code)
        {
            switch (code)
            {
                case 5:
                    return "#The semicolon is missing.\n";

                case 10:
                    return "#Expected: \"[name_variable] := [calculating_expression];\" or " +
                           "\"[name_matrix_variable][[height]][[width]] := [calculating_expression];\".\n";

                case 11:
                    return "#Nonexistent variable: \"";

                case 12:
                    return "#A polynomial variable is assigned a value that is not a polynomial.\n";

                case 13:
                    return "#The coefficients of the matrix should be assigned only a polynomial value.\n";

                case 14:
                    return "#A matrix variable is assigned a value that is not a matrix.\n";

                case 15:
                    return "#Expected: \"[name_matrix_variable]\" or \"[name_matrix_variable][[height]][[width]]\".\n";

                case 16:
                    return "#Expected: \"[name_polynomial_variable]\".\n";

                case 20:
                    return "#Expected: \"goto([number]);\".\n";

                case 21:
                    return "#Expected: \"([number])\".\n";

                case 30:
                    return "#Inappropriate use of the command.\n";

                case 40:
                    return "#Invalid format. Expected: \"pol [name_polynomial]\".\n";

                case 41:
                    return "#The variable name is occupied.\n";

                case 45:
                    return "#Invalid format. Expected: \"matrix[name_matrix]:=([number_height],[number_width]);\".\n";

                case 46:
                    return "#Expected: \"([number],[number])\".\n";

                case 50:
                    return "#Invalid format. Expected: \"if([bool_expression]){[commands]}\".\n";

                case 51:
                    return "#The opening parenthesis is missing.\n";

                case 52:
                    return "#The closing parenthesis is missing.\n";

                case 53:
                    return "#Expected: \"([bool_expression])\".\n"; ;

                case 60:
                    return "#The expression could not be calculated.\n";

                case 61:
                    return "#The element with these indexes is missing.\n";

                case 70:
                    return "#Invalid format. Expected: \"write([polynomial_calculating_expression]);\".\n";

                case 80:
                    return "#Invalid format. Expected: \"while([bool_expression]){[commands]}\".\n";

                default:
                    return "#Unknown error code.\n";
            }
        }
    }
}
