using PolynomialInterfaces;

namespace Polynomials
{
    /// <summary>
    /// Реализует полиномы Эрмита.
    /// </summary>
    class PolynomialErmit : ISequence<Polynomial>
    {
        /// <summary>
        /// Следующий и текущий полином Эрмита.
        /// </summary>
        private Polynomial nEpol, epol;

        /// <summary>
        /// Номер текущего полинома Эрмита.
        /// </summary>
        private int num;

        /// <summary>
        /// Вспомогательный полином в вычислениях.
        /// </summary>
        readonly Polynomial twoX = new Polynomial("2x");

        /// <summary>
        /// Инициализирует конструктор полиномов Эрмита.
        /// </summary>
        public PolynomialErmit()
        {
            epol = new Polynomial("1");
            nEpol = new Polynomial("2x");
            num = 0;
        }

        /// <summary>
        /// Получает следующий полином Эрмита.
        /// </summary>
        /// <returns>Полином Эрмита.</returns>
        public Polynomial Next()
        {
            Polynomial newNext;

            newNext = twoX * nEpol + new Polynomial((2 * num).ToString()) * epol;

            epol = nEpol;
            nEpol = newNext;

            num++;

            return epol;
        }

        /// <summary>
        /// Получает предыдущий полином Эрмита.
        /// </summary>
        /// <returns>Полином Эрмита.</returns>
        public Polynomial Prev()
        {
            Polynomial newAct;

            if (num != 0)
            {
                num--;

                newAct = new Polynomial((1 / 2 * num).ToString()) * (twoX * epol - nEpol);

                nEpol = epol;
                epol = newAct;
            }

            return epol;
        }

        /// <summary>
        /// Перезапускает конструктор Эрмита.
        /// </summary>
        public void Reset()
        {
            epol = new Polynomial("1");
            nEpol = new Polynomial("2x");
            num = 0;
        }
    }
}
