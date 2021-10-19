using System;
using System.Collections.Generic;
using System.Text;

namespace BasicAlg
{
    /// <summary>
    /// Interface for getting the next and previous element and resetting the sequence of the type of T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ISequence<T> : IBasicSequence<T> where T : IComparable<T>
    {
        T Prev();

        void Reset();
    }
}
