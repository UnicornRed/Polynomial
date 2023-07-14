using System;
using TokenParsers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polynomials
{
    /// <summary>
    /// Делегат умножения объекта общего типа на скаляр слева.
    /// </summary>
    /// <typeparam name="T">Общий тип.</typeparam>
    /// <param name="a">Скаляр.</param>
    /// <param name="b">Объект общего типа.</param>
    /// <returns>Результат умножения объекта общего типа на скаляр слева.</returns>
    public delegate T MultyScalT<T>(double a, T b);

    /// <summary>
    /// Делегат бинарной операции объектов общего типа.
    /// </summary>
    /// <typeparam name="T">Общий тип.</typeparam>
    /// <param name="a">Первый объект общего типа.</param>
    /// <param name="b">Второй объект общего типа.</param>
    /// <returns>Результат бинарной операции объектов общего типа.</returns>
    public delegate T BinOperTT<T>(T a, T b);

    /// <summary>
    /// Класс хранящий делегаты вычисления операций общего типа.
    /// </summary>
    /// <typeparam name="T">Общий тип.</typeparam>
    public class ArithmeticType<T>
    {
        /// <summary>
        /// Делегат умножения двух элементов общего типа.
        /// </summary>
        readonly public BinOperTT<T> multy;

        /// <summary>
        /// Делегат сложения двух элементов общего типа.
        /// </summary>
        readonly public BinOperTT<T> sum;

        /// <summary>
        /// Делегат умножения элемента общего типа на скаляр слева.
        /// </summary>
        readonly public MultyScalT<T> multyScal;

        /// <summary>
        /// Нейтральный элемент умножения общего типа.
        /// </summary>
        readonly public T neutralElem;

        /// <summary>
        /// Инициализирует объект класса, хранящего делегаты вычисления операций общего типа.
        /// </summary>
        /// <param name="neutralElem">Нейтральный элемент умножения общего типа.</param>
        /// <param name="multyScal">Делегат умножения объекта общего типа на скаляр слева.</param>
        /// <param name="multy">Делегат перемножения объектов общего типа.</param>
        /// <param name="sum">Делегат сложения объектов общего типа.</param>
        public ArithmeticType(BinOperTT<T> multy, BinOperTT<T> sum, MultyScalT<T> multyScal, T neutralElem)
        {
            this.multy = multy;
            this.sum = sum;
            this.multyScal = multyScal;
            this.neutralElem = neutralElem;
        }
    }

    /// <summary>
    /// Реализует обработку полиномов.
    /// </summary>
    public class Polynomial : IEnumerable<int>, IComparable<Polynomial>
    {
        /// <summary>
        /// Степень полинома.
        /// </summary>
        public int Deg { get; private set; }

        /// <summary>
        /// Коэффициенты полинома.
        /// </summary>
        internal readonly SortedList<int, double> coeff;

        /// <summary>
        /// Значение коэффициента полинома при x с данной степенью.
        /// </summary>
        /// <param name="i">Степень x.</param>
        /// <returns>Значение коэффициента.</returns>
        public double this[int i]
        {
            get
            {
                if (coeff.ContainsKey(i))
                    return coeff[i];
                else
                    return 0;
            }
            private set
            {
                if (coeff.ContainsKey(i))
                    coeff[i] = value;
                else
                    coeff.Add(i, value);

                if (coeff[i] == 0)
                    coeff.Remove(i);

                Deg = DegCoeff(coeff);
            }
        }

        /// <summary>
        /// Возвращает степень полинома по его коэффициентам.
        /// </summary>
        /// <param name="coeff">Коэффициенты полинома.</param>
        /// <returns>Степень полинома.</returns>
        private int DegCoeff(SortedList<int, double> coeff)
        {
            int deg;

            if (coeff.Count != 0)
                deg = coeff.Keys.LastOrDefault();
            else
                deg = -1;

            return deg;
        }

        /// <summary>
        /// Инициализирует полином по умолчанию.
        /// </summary>
        public Polynomial()
        {
            coeff = new SortedList<int, double>();
            Deg = -1;

            coeff.Add(0, 0);
        }

        /// <summary>
        /// Инициализирует полином по коэффициентам.
        /// </summary>
        /// <param name="coeff">Коэффициенты полинома.</param>
        public Polynomial(SortedList<int, double> coeff)
        {
            int MaxDeg = 0;
            this.coeff = new SortedList<int, double>();

            foreach (var i in coeff)
            {
                if (i.Value != 0)
                {
                    if (MaxDeg < i.Key)
                        MaxDeg = i.Key;

                    this.coeff.Add(i.Key, i.Value);
                }
            }

            Deg = MaxDeg;
        }

        /// <summary>
        /// Инициализирует полином по переданному полиному.
        /// </summary>
        /// <param name="pol">Полином.</param>
        public Polynomial(Polynomial pol)
        {
            coeff = new SortedList<int, double>(pol.coeff);

            Deg = pol.Deg;
        }

        /// <summary>
        /// Инициализирует полином по строке.
        /// </summary>
        /// <param name="str">Строка формата a_1*x^b_1+...+a_n*x^b_n.</param>
        public Polynomial(string str) : this()
        {
            try
            {
                coeff = PolynomialParser.PolParser(str);
                Deg = DegCoeff(coeff);
            }
            catch (Exception e)
            {
                coeff = null;
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Неявное преобразование строки в полином.
        /// </summary>
        /// <param name="poly">Строка формата a_1*x^b_1+...+a_n*x^b_n.</param>
        public static implicit operator Polynomial(string poly)
        {
            return new Polynomial(poly);
        }

        /// <summary>
        /// Переопределение сложения для полиномов.
        /// </summary>
        /// <param name="pol1">Первое слагаемое.</param>
        /// <param name="pol2">Второе слагаемое.</param>
        /// <returns>Результат сложения pol1 и pol2.</returns>
        public static Polynomial operator + (Polynomial pol1, Polynomial pol2)
        {
            Polynomial polRes = new Polynomial(pol1);

            foreach (var i in pol2)
                polRes[i] += pol2[i];

            return polRes;
        }

        /// <summary>
        /// Переопределение вычитания для полиномов.
        /// </summary>
        /// <param name="pol1">Уменьшаемое.</param>
        /// <param name="pol2">Вычитаемое.</param>
        /// <returns>Результат вычитания pol2 из pol1.</returns>
        public static Polynomial operator - (Polynomial pol1, Polynomial pol2)
        {
            Polynomial polRes = new Polynomial(pol1);

            foreach (var i in pol2)
                polRes[i] -= pol2[i];

            return polRes;
        }

        /// <summary>
        /// Переопределение умножения для полиномов.
        /// </summary>
        /// <param name="pol1">Первый множитель.</param>
        /// <param name="pol2">Второй множитель.</param>
        /// <returns>Результат умножения pol1 на pol2.</returns>
        public static Polynomial operator * (Polynomial pol1, Polynomial pol2)
        {
            Polynomial polRes = new Polynomial();

            foreach (var i in pol1)
            {
                foreach (var j in pol2)
                    polRes[i + j] += pol1[i] * pol2[j];
            }

            return polRes;
        }

        /// <summary>
        /// Переопределение умножения полинома на число.
        /// </summary>
        /// <param name="pol">Множитель, являющийся полиномом.</param>
        /// <param name="a">Множитель, являющийся числом.</param>
        /// <returns>Результат умножения pol на a.</returns>
        public static Polynomial operator * (Polynomial pol, double a)
        {
            Polynomial polRes = new Polynomial(pol);

            foreach (var i in pol)
                polRes[i] *= a;

            return polRes;
        }

        /// <summary>
        /// Переопределение умножения числа на полином.
        /// </summary>
        /// <param name="a">Множитель, являющийся числом.</param>
        /// <param name="pol">Множитель, являющийся полиномом.</param>
        /// <returns>Результат умножения a на pol.</returns>
        public static Polynomial operator * (double a, Polynomial pol) => pol * a;

        /// <summary>
        /// Переопределение деления для полиномов.
        /// </summary>
        /// <param name="pol1">Делимое.</param>
        /// <param name="pol2">Делитель.</param>
        /// <returns>Результат деления pol1 на pol2.</returns>
        public static Polynomial operator / (Polynomial pol1, Polynomial pol2)
        {
            Polynomial polRes = new Polynomial();
            Polynomial polHelp;
            Polynomial polRemains = new Polynomial(pol1);

            while (polRemains.Deg >= pol2.Deg)
            {
                polHelp = new Polynomial(new SortedList<int, double> { { polRemains.Deg - pol2.Deg, polRemains[polRemains.Deg] / pol2[pol2.Deg] } });
                polRemains -= polHelp * pol2;
                polRes += polHelp;
            }

            return polRes;
        }

        /// <summary>
        /// Переопределение взятия остатка при делении для полиномов.
        /// </summary>
        /// <param name="pol1">Делимое.</param>
        /// <param name="pol2">Делитель.</param>
        /// <returns>Результат взятия остатка при делении pol1 на pol2.</returns>
        public static Polynomial operator % (Polynomial pol1, Polynomial pol2)
        {
            Polynomial polRes = pol1 - pol1 / pol2 * pol2;

            polRes = (1 / polRes[polRes.Deg]) * polRes;

            return polRes;
        }

        /// <summary>
        /// Подстановка переданного полинома в данный.
        /// </summary>
        /// <param name="pol">Полином.</param>
        /// <returns>Полином, полученный в результате подстановки.</returns>
        public Polynomial Eval(Polynomial pol)
        {
            ArithmeticType<Polynomial> at = new ArithmeticType<Polynomial>((x, y) => x * y, (x, y) => x + y, (a, x) => a * x, new Polynomial("1"));

            return Eval<Polynomial>(pol, at);
        }

        /// <summary>
        /// Подстановка объекта общего типа в полином.
        /// </summary>
        /// <typeparam name="T">Общий тип.</typeparam>
        /// <param name="obj">Объект подстановки.</param>
        /// <param name="at">Объект класса, хранящего делегаты вычисления операций общего типа.</param>
        /// <returns>Объект общего типа - результат подстановки.</returns>
        public T Eval<T>(T obj, ArithmeticType<T> at)
        {
            double help = coeff[Deg];

            T res = at.multyScal(help, at.neutralElem);

            for (int i = Deg - 1; i >= 0; i--)
            {
                res = at.multy(res, obj);

                if (coeff.ContainsKey(i))
                {
                    help = coeff[i];
                    res = at.sum(res, at.multyScal(help, at.neutralElem));
                }
            }

            return res;
        }

        /// <summary>
        /// Дифференцирование полинома.
        /// </summary>
        /// <returns>Дифференцированный полином.</returns>
        public Polynomial Diff()
        {
            Polynomial polRes = new Polynomial();

            foreach (int deg in this)
                if (deg != 0)
                    polRes[deg - 1] = (deg) * this[deg];

            polRes.Deg = Deg - 1;

            return polRes;
        }

        /// <summary>
        /// Переопределение преобразования полинома в строку.
        /// </summary>
        /// <returns>Строку, соответствующую данному полиному.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder("");

            foreach (int deg in this)
            {
                if (coeff[deg] < 0)
                    str.Append("-");
                else if (str.ToString() != "")
                    str.Append("+");

                if (coeff[deg] != 0)
                {
                    if (deg > 0)
                    {
                        if (Math.Abs(coeff[deg]) != 1)
                            str.Append(String.Format("{0:f}", Math.Abs(coeff[deg])) + "*x");
                        else
                            str.Append("x");

                        if (deg != 1)
                            str.Append("^" + deg);
                    }
                    else
                        str.Append(String.Format("{0:f}", Math.Abs(coeff[deg])));
                }
            }
            
            if (str.ToString() == "")
                str.Append("0");

            return str.ToString();
        }

        /// <summary>
        /// Реализация интерфейста перечисления.
        /// </summary>
        /// <returns>Коллекцию ключей коэффициентов.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            return coeff.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Реализация интерфейса сравнения.
        /// </summary>
        /// <param name="other">Полином, который сравнивается с данным.</param>
        /// <returns>1, если перый полином "больше" второго, -1, если второй "больше" первого, 0, если они равны.</returns>
        public int CompareTo(Polynomial other)
        {
            if (Deg > other.Deg)
                return 1;
            else if (Deg < other.Deg)
                return -1;
            else
                return 0;
        }
    }
}