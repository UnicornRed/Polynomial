namespace PolynomialInterfaces
{
    /// <summary>
    /// Интерфейс получения следующего элемента последовательности.
    /// </summary>
    /// <typeparam name="T">Тип элементов последовательности.</typeparam>
    interface IBasicSequence<T>
    {
        /// <summary>
        /// Получает следующий элемент последовательности.
        /// </summary>
        /// <returns>Следующий элемент последовательности.</returns>
        T Next();
    }
}
