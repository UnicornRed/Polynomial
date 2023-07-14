using System.Collections.Generic;
using Calculating;
using ParserWork;
using System.Linq;

namespace TokenParsers
{
    /// <summary>
    /// Реализует деление выражения на составные части.
    /// </summary>
    class TokenCalcParser
    {
        /// <summary>
        /// Делит выражение на составные части.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Список элементов выражения.</returns>
        public static List<Token> TokenParser(string expression)
        {
            HashSet<FuncAndOper> oper = FuncAndOper.GetAllFuncAndOper();
            int countOper = 0;
            string[] nameOpers = new string[oper.Count()];

            if (expression[0] == '-')
                expression = "0" + expression;

            foreach (var i in oper)
                nameOpers[countOper++] = i.Act;

            string[] tokensStr = Parser.ParserForAll(expression, "",
                                              new string[,] { { "(-", "(0-" }, { ",-", ",0-" }, { "(", " ( " } ,
                                                              { ")", " ) " } , { ",", " , " } },
                                              nameOpers, new char[] { ' ' });
            List<Token> tokens = new List<Token>();

            foreach (var i in tokensStr)
                tokens.Add(new Token(i));

            return tokens;
        }
    }
}
