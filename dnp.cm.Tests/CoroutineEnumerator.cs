using System;
using System.Collections.Generic;
using CaliburnMicro.Framework;

namespace dnp.cm.Tests
{
    /// <summary>
    /// Enumeriert die einzelnen Schritte (yields) einer Coroutine zum Testen des Ablaufs. Die <see cref="IResult"/>s werden nur zur 
    /// Überprüfung abgerufen, jedoch nicht mit Execute ausgeführt. Hierdurch lassen sich auch asynchrone Aufrufe einfach testen.
    /// </summary>
    public class CoroutineEnumerator
    {
        private readonly IEnumerator<IResult> _Enumerator;

        /// <summary>
        /// Konstruktor der <see cref="CoroutineEnumerator"/> Klasse.
        /// </summary>
        /// <param name="enumerable">Die coroutine zum Testen</param>
        public CoroutineEnumerator(IEnumerable<IResult> enumerable)
        {
            _Enumerator = enumerable.GetEnumerator();
        }

        /// <summary>
        /// Konstruktor der <see cref="CoroutineEnumerator"/> Klasse.
        /// </summary>
        /// <param name="enumerator">Die coroutine zum Testen</param>
        public CoroutineEnumerator(IEnumerator<IResult> enumerator)
        {
            _Enumerator = enumerator;
        }

        /// <summary>
        /// Sucht das nächste IResult der Coroutine ab.
        /// </summary>
        /// <typeparam name="TCoroutine">Typ des IResults.</typeparam>
        /// <returns></returns>
        public TCoroutine FindNext<TCoroutine>()
        {
            while (_Enumerator.MoveNext())
            {
                if (_Enumerator.Current is TCoroutine)
                    return (TCoroutine)_Enumerator.Current;
            }

            throw new InvalidOperationException("Liste der Coroutines enthält kein " + typeof(TCoroutine).Name);
        }

        /// <summary>
        /// Ruft das nächste IResult der Coroutine ab.
        /// </summary>
        /// <typeparam name="TCoroutine">Typ des IResults.</typeparam>
        /// <returns></returns>
        public TCoroutine Next<TCoroutine>()
        {
            _Enumerator.MoveNext();
            if (_Enumerator.Current is TCoroutine)
                return (TCoroutine)_Enumerator.Current;
            
            throw new InvalidOperationException("Der nächste Schritt ist nicht " + typeof(TCoroutine).Name);
        }

        /// <summary>
        /// Lässt den Enumerator zum Ende durchlaufen.
        /// </summary>
        public void Finish()
        {
            while (_Enumerator.MoveNext()) { }
        }
    }
}