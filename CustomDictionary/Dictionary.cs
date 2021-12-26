using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDictionary
{
     class Dictionionary<Tk, Tv> : IDictionary<Tk, Tv>
    {
        internal struct NewEntry
        {
            internal int hash_code;
            internal Tk k;
            internal Tv v;
        }

        private List<List<NewEntry>> hash_table;
        private List<Tk> key_collection;
        private List<Tv> value_collection;
        private int kol;

        public Dictionionary()
        {
            hash_table = new List<List<NewEntry>>(10);
            for(int i = 0; i < 10; ++i)
            {
                hash_table.Add(new List<NewEntry>());
            }
            key_collection = new List<Tk>();
            value_collection = new List<Tv>();
            kol = 0;
        }

        public ICollection<Tk> Keys => key_collection;

        public ICollection<Tv> Values => value_collection;

        public int Count => kol;

        public bool IsReadOnly => false;

        public object Exception { get; private set; }

        public Tv this[Tk k]
        {
            get
            {
                int temp = k.GetHashCode();
                int ind = temp % hash_table.Capacity;
                if (hash_table[ind].Count != 0)
                {
                    foreach (NewEntry i in hash_table[ind])
                    {
                        if (i.hash_code == temp)
                        {
                            return i.v;
                        }
                    }
                }
                throw new Exception("Нет");
            }
            set
            {
                int temp = k.GetHashCode();
                int ind = temp % hash_table.Capacity;
                if (hash_table[ind].Count != 0)
                {
                    for(int i=0;i< hash_table[ind].Count; ++i)
                    {
                        if (hash_table[ind][i].k.Equals(k))
                        {
                            hash_table[ind][i] = new NewEntry { hash_code = temp, k = k, v = value };
                        }
                    }
                }
            }
        }

        public void tAdd(Tk k, Tv v, int tmp, int ind)
        {
            NewEntry temp = new NewEntry { hash_code = tmp, k = k, v = v };
            hash_table[ind].Add(temp);
            key_collection.Add(k);
            value_collection.Add(v);
            ++kol;
        }
        public void Add(Tk k, Tv v)
        {
            int temp = k.GetHashCode();
            int ind = Math.Abs(temp % hash_table.Capacity);
            if (hash_table[ind].Count != 0)
            {
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.hash_code == temp)
                    {
                        throw new Exception("Добавление дубликата");
                    }
                }
            }
            tAdd(k, v, temp, ind);
        }

        public void Add(KeyValuePair<Tk, Tv> it)
        {
            if (Contains(it))
            {
                   throw new Exception("Добавление дубликата");
            }
            int temp = it.Key.GetHashCode();
            int ind = temp % hash_table.Capacity;
            tAdd(it.Key, it.Value, temp, ind);
        }

        public void Clear()
        {
            hash_table = new List<List<NewEntry>>();
            key_collection = new List<Tk>();
            value_collection = new List<Tv>();
            kol = 0;
        }

        public bool Remove(Tk k)
        {
            int temp = k.GetHashCode();
            int ind = temp % hash_table.Capacity;
            NewEntry temp_ent = new NewEntry();
            bool check = false;
            if (hash_table[ind].Count != 0)
            {
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.hash_code == temp)
                    {
                        check = true;
                        temp_ent = i;
                        break;
                    }
                }
            }
            if (check)
            {
                key_collection.Remove(k);
                value_collection.Remove(temp_ent.v);
                hash_table[ind].Remove(temp_ent);
                --kol;
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<Tk, Tv> item)
        {
            Tk k = item.Key;
            int temp = k.GetHashCode();
            int ind = temp % hash_table.Capacity;
            NewEntry temp_ent = new NewEntry();
            bool check = false;
            if (hash_table[ind].Count != 0)
            {
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.hash_code == temp)
                    {
                        check = true;
                        temp_ent = i;
                        break;
                    }
                }
            }
            if (check)
            {
                key_collection.Remove(k);
                value_collection.Remove(temp_ent.v);
                hash_table[ind].Remove(temp_ent);
                --kol;
                return true;
            }
            return false;
        }

        public bool Contains(KeyValuePair<Tk, Tv> item)
        {
            Tk key = item.Key;
            Tv value = item.Value;
            int tempHash = key.GetHashCode();
            int index = tempHash % hash_table.Capacity;
            if (hash_table[index].Count != 0)
            {
                foreach (NewEntry i in hash_table[index])
                {
                    if (i.hash_code == tempHash && i.v.Equals(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsKey(Tk key)
        {
            int temp = key.GetHashCode();
            int ind = temp % hash_table.Capacity;
            if (hash_table[ind].Count != 0)
            {
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.hash_code == temp)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<Tk, Tv>[] mas, int mas_ind)
        {
            for(int i = 0; i < kol; ++i)
            {
                mas[mas_ind + i] = new KeyValuePair<Tk, Tv>(key_collection[i], value_collection[i]);
            }
        }

        public bool TryGetValue(Tk k, [MaybeNullWhen(false)] out Tv v)
        {
            int temp = k.GetHashCode();
            int ind = temp % hash_table.Capacity;
            if (hash_table[ind].Count != 0)
            {
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.hash_code == temp)
                    {
                        v = i.v;
                        return true;
                    }
                }
            }
            v = default(Tv);
            return false;
        }

        public IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
