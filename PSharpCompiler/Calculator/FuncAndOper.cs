using System.Collections.Generic;
using Polynomials;
using MatrixType;

namespace Calculating
{
    /// <summary>
    /// Реализует набор операций для полиномов.
    /// </summary>
    class FuncAndOper
    {
        /// <summary>
        /// Тернарная операция.
        /// </summary>
        /// <param name="var1">Первый аргумент.</param>
        /// <param name="var2">Второй аргумент.</param>
        /// <param name="var3">Третий аргумент.</param>
        /// <returns>Результат операции.</returns>
        public delegate object TerOper(object var1, object var2, object var3);

        /// <summary>
        /// Бинарная операция.
        /// </summary>
        /// <param name="var1">Первый аргумент.</param>
        /// <param name="var2">Второй аргумент.</param>
        /// <returns>Результат операции.</returns>
        public delegate object BinOper(object var1, object var2);

        /// <summary>
        /// Унарная операция.
        /// </summary>
        /// <param name="var1">Аргумент операции.</param>
        /// <returns>Результат операции.</returns>
        public delegate object UnaOper(object var1);

        /// <summary>
        /// Операция, не требующая операндов.
        /// </summary>
        /// <returns>Результат операции.</returns>
        public delegate object ZeroOper();

        /// <summary>
        /// Унарная операция.
        /// </summary>
        public UnaOper Una { get; private set; }

        /// <summary>
        /// Бинарная операция.
        /// </summary>
        public BinOper Bin { get; private set; }

        /// <summary>
        /// Тернарная операция.
        /// </summary>
        public TerOper Ter { get; private set; }

        /// <summary>
        /// Операция, не требующая операндов.
        /// </summary>
        public ZeroOper Zero { get; private set; }

        /// <summary>
        /// Типы аргументов.
        /// </summary>
        public string ArgType { get; private set; }

        /// <summary>
        /// Имя операции.
        /// </summary>
        public string Act { get; private set; }
        
        /// <summary>
        /// Приоритет операции.
        /// </summary>
        public int Priority { get; private set; }
        
        /// <summary>
        /// Тип операции.
        /// </summary>
        public TypeOperation Type { get; private set; }

        /// <summary>
        /// Инициализирует оператор с тернарной операцией.
        /// </summary>
        /// <param name="act">Имя операции.</param>
        /// <param name="priority">Приоритет операции.</param>
        /// <param name="argType">Типы аргументов.</param>
        /// <param name="ter">Тернарная операция.</param>
        /// <param name="type">Тип операции.</param>
        private FuncAndOper(string act, int priority, string argType, TerOper ter, TypeOperation type)
        {
            Act = act;
            Priority = priority;
            ArgType = argType;
            Ter = ter;
            Type = type;
        }

        /// <summary>
        /// Инициализирует оператор с унарной операцией.
        /// </summary>
        /// <param name="act">Имя операции.</param>
        /// <param name="priority">Приоритет операции.</param>
        /// <param name="argType">Типы аргументов.</param>
        /// <param name="una">Унарная операция.</param>
        /// <param name="type">Тип операции.</param>
        private FuncAndOper(string act, int priority, string argType, UnaOper una, TypeOperation type)
        {
            Act = act;
            Priority = priority;
            ArgType = argType;
            Una = una;
            Type = type;
        }

        /// <summary>
        /// Инициализирует оператор с бинарной операцией.
        /// </summary>
        /// <param name="act">Имя операции.</param>
        /// <param name="priority">Приоритет операции.</param>
        /// <param name="argType">Типы аргументов.</param>
        /// <param name="bin">Бинарная операция.</param>
        /// <param name="type">Тип операции.</param>
        private FuncAndOper(string act, int priority, string argType, BinOper bin, TypeOperation type)
        {
            Act = act;
            Priority = priority;
            ArgType = argType;
            Bin = bin;
            Type = type;
        }

        /// <summary>
        /// Инициализирует оператор с операцией, не требующей операндов.
        /// </summary>
        /// <param name="act">Имя операции.</param>
        /// <param name="priority">Приоритет операции.</param>
        /// <param name="argType">Типы аргументов.</param>
        /// <param name="zero">Операция не требующая операндов.</param>
        /// <param name="type">Тип операции.</param>
        private FuncAndOper(string act, int priority, string argType, ZeroOper zero, TypeOperation type)
        {
            Act = act;
            Priority = priority;
            ArgType = argType;
            Zero = zero;
            Type = type;
        }

        /// <summary>
        /// Список операторов.
        /// </summary>
        public static readonly HashSet<FuncAndOper> allAct = new HashSet<FuncAndOper>()
        {
            new FuncAndOper("+", 1, "PolynomialPolynomial", (x,y)=>(Polynomial)x+(Polynomial)y, TypeOperation.BinOper),
            new FuncAndOper("-", 1, "PolynomialPolynomial", (x,y)=>(Polynomial)y-(Polynomial)x, TypeOperation.BinOper),
            new FuncAndOper("*", 2, "PolynomialPolynomial", (x,y)=>(Polynomial)x*(Polynomial)y, TypeOperation.BinOper),
            new FuncAndOper("EvalP", 0, "PolynomialPolynomial", (x,y)=>((Polynomial)x).Eval((Polynomial)y), TypeOperation.BinFunc),
            new FuncAndOper("Diff", 0, "Polynomial", (x)=>((Polynomial)x).Diff(), TypeOperation.UnaFunc),
            new FuncAndOper("Rand", 0, "", ()=>(new RandPolynomial()).Next(), TypeOperation.ZeroFunc),
            new FuncAndOper("%", 2, "PolynomialPolynomial", (x,y)=>(Polynomial)y%(Polynomial)x, TypeOperation.BinOper),
            new FuncAndOper("/", 2, "PolynomialPolynomial", (x,y)=>(Polynomial)y/(Polynomial)x, TypeOperation.BinOper),
            new FuncAndOper("+", 1, "MatrixMatrix", (x,y)=>(MatrixPolynomial)x+(MatrixPolynomial)y, TypeOperation.BinOper),
            new FuncAndOper("-", 1, "MatrixMatrix", (x,y)=>(MatrixPolynomial)x-(MatrixPolynomial)y, TypeOperation.BinOper),
            new FuncAndOper("*", 2, "MatrixMatrix", (x,y)=>(MatrixPolynomial)x*(MatrixPolynomial)y, TypeOperation.BinOper),
            new FuncAndOper("det", 0, "MatrixMatrix", (x)=>((MatrixPolynomial)x).MatrixDeterminant(), TypeOperation.UnaFunc),
            new FuncAndOper("coef", 0, "PolynomialPolynomialMatrix", (i, j, x)=>((MatrixPolynomial)x)[(int)((Polynomial)j)[0], (int)((Polynomial)i)[0]], TypeOperation.TerFunc),
            new FuncAndOper("ChPol", 0, "Matrix", (x)=>(Polynomial)((MatrixPolynomial)x).CharactPolynomial(), TypeOperation.UnaFunc),
            new FuncAndOper("EvalM", 0, "MatrixMatrix", (x,y)=>((MatrixPolynomial)x).Eval((Polynomial)y), TypeOperation.BinFunc)
        };

        /// <summary>
        /// Передаёт список операторов.
        /// </summary>
        /// <returns>Список операторов.</returns>
        public static HashSet<FuncAndOper> GetAllFuncAndOper() => allAct;
    }
}
