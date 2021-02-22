using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.FitxureServer.Library.FitxureData
{
    public class ItemsEnumerator<T> : IEnumerator<T> where T : class
    {
        public T[] _items;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ItemsEnumerator(T[] list)
        {
            _items = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _items.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
            _items = null;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try
                {
                    return _items[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

    }
}
