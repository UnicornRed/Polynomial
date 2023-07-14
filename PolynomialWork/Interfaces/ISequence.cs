namespace PolynomialInterfaces
{
    /// <summary>
    /// Интерфейс получения последовательности типа T.
    /// </summary>
    /// <typeparam name="T">Тип элементов последовательности.</typeparam>
    interface ISequence<T> : IBasicSequence<T>
    {
        /// <summary>
        /// Получает предыдущий элемент последовательности.
        /// </summary>
        /// <returns>Предыдущий элемент последовательности.</returns>
        T Prev();

        /// <summary>
        /// Перезапускает получение последовательности.
        /// </summary>
        void Reset();
    }
}

