using System;
using System.Collections.Generic;
using System.Text;

namespace BasicAlg
{
    /// <summary>
    /// Interface for getting the next element of the sequence of the type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IBasicSequence<T>
    {
        public T Next();
    }
}
