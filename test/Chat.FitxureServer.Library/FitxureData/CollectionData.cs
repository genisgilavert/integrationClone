using Chat.Common.Entities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Chat.FitxureServer.Library.Extensions;

namespace Chat.FitxureServer.Library.FitxureData
{
    public class CollectionData<T> : IEnumerable<T> where T : class
    {
        private T[] _items;

        public CollectionData(T[] items){
            _items = items;
        }
        public IEnumerable<T> GetEnumerator(){
            for(int i=0;i<this._items.Length;++i){
                yield return this._items[i];
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ItemsEnumerator<T>(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

        public async Task<T> FirstOrDefaultAsync(Func<T, bool> predicate)
        {
            await Task.Delay(1);
            return this._items.FirstOrDefault(p => predicate(p));
        }

        public async Task<T> FirstAsync(Func<T, bool> predicate)
        {
            await Task.Delay(1);
            try{
                return this._items.First(p => predicate(p));
            }
            catch(Exception ex){
                return null;
            }
        }

        public void Add(T item){
            try{
                this._items = _items.Add(item).ToArray();
            }
            catch(Exception ex){
                /*
                 {"Unable to cast object of type '<Add>d__0`1[Chat.Common.Entities.ChatUser]' 
                    to type 'Chat.Common.Entities.ChatUser[]'."}
                 */
            }
        }

        public async IAsyncEnumerable<T> WhereAsync(Func<T, bool> predicate)
        {
            await Task.Delay(1);
            foreach (var item in this._items)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            foreach (var item in this._items)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<T> OrderBy(Func<T, bool> predicate)
        {
            var items = _items.OrderBy(predicate);
            foreach (var item in items){
                yield return item;
            }
        }

        public async Task<List<T>> ToListAsync(){
            await Task.Delay(1);
            return _items.ToList();
        }

        public List<T> ToList()
        {
            return _items.ToList();
        }

    }
}
