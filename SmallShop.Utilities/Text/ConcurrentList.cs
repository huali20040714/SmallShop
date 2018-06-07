using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmallShop.Utilities
{
    /// <summary>
    /// a thread-safe list with support for:
    /// 1) negative indexes (read from end).  "myList[-1]" gets the last value
    /// 2) modification while enumerating: enumerates a copy of the collection.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ConcurrentList<TValue> : IList<TValue>
    {
        private object _lock = new object();
        private List<TValue> _storage = new List<TValue>();

        /// <summary>
        /// support for negative indexes (read from end).  "myList[-1]" gets the last value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>        
        public TValue this[int index]
        {
            get
            {
                lock (_lock)
                {
                    if (index < 0)
                    {
                        index = this.Count() - index;
                    }
                    return _storage[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    if (index < 0)
                    {
                        index = this.Count() - index;
                    }
                    _storage[index] = value;
                }
            }
        }

        public void Sort()
        {
            lock (_lock)
            {
                _storage.Sort();
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return _storage.Count();
            }
        }

        public int Count(Func<TValue, bool> predicate)
        {
            lock (_lock)
            {
                return _storage.Count(predicate);
            }
        }

        public void Add(TValue item)
        {
            lock (_lock)
            {
                _storage.Add(item);
            }
        }

        public void AddRange(IEnumerable<TValue> items)
        {
            lock (_lock)
            {
                _storage.AddRange(items);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _storage.Clear();
            }
        }

        public bool Contains(TValue item)
        {
            lock (_lock)
            {
                return _storage.Contains(item);
            }
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _storage.CopyTo(array, arrayIndex);
            }
        }

        public TValue FirstOrDefault()
        {
            lock (_lock)
            {
                return _storage.FirstOrDefault();
            }
        }

        public TValue FirstOrDefault(Func<TValue, bool> predicate)
        {
            lock (_lock)
            {
                return _storage.FirstOrDefault(predicate);
            }
        }

        public bool Exists(Predicate<TValue> predicate)
        {
            lock (_lock)
            {
                return _storage.Exists(predicate);
            }
        }

        public int IndexOf(TValue item)
        {
            lock (_lock)
            {
                return _storage.IndexOf(item);
            }
        }

        public void Insert(int index, TValue item)
        {
            lock (_lock)
            {
                _storage.Insert(index, item);
            }
        }

        public bool Remove(TValue item)
        {
            lock (_lock)
            {
                return _storage.Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _storage.RemoveAt(index);
            }
        }

        public int RemoveAll(Predicate<TValue> match)
        {
            lock (_lock)
            {
                return _storage.RemoveAll(match);
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            lock (_lock)
            {
                return this.Clone().GetEnumerator();
            }
        }

        public IList<TValue> Clone()
        {
            lock (_lock)
            {
                return _storage.Select<TValue, TValue>(p => p).ToList();
            }
        }

        bool ICollection<TValue>.IsReadOnly
        {
            get
            {
                return ((IList<TValue>)_storage).IsReadOnly;
            }
        }

        int ICollection<TValue>.Count => this._storage.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
