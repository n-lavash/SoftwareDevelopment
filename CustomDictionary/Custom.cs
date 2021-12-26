using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CustomDictionary
{
    class Custom<Tk, Tv> : IDictionary<Tk, Tv>
    {
        internal struct NewEntry
        {
            internal int hash_code;
            internal Tk k;
            internal Tv v;
        }
        private LinkedList<NewEntry>[] hash_table;

        public Custom()
        {
            hash_table = new LinkedList<NewEntry>[10];
            for (int i = 0; i < 10; ++i)
            {
                hash_table[i] = new LinkedList<NewEntry>();
            }
        }

        public Tv this[Tk k]
        {
            get
            {
                int temp = k.GetHashCode();
                int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.k.Equals(k))
                    {
                        return i.v;
                    }
                }
                throw new Exception($"Ключ {k} не найден");
            }
            set
            {
                int temp = k.GetHashCode();
                int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
                NewEntry temp_ent = new NewEntry { hash_code = temp, k = k, v = value };
                for (var c = hash_table[ind].First; !c.Equals(hash_table[ind].Last); c = c.Next)
                {
                    if (c.Value.k.Equals(k))
                    {
                        c.Value = temp_ent;
                        return;
                    }
                }
                Add(k, value);
            }
        }

        public ICollection<Tk> Keys
        {
            get
            {
                List<Tk> ks = new List<Tk>();
                foreach (var l in hash_table)
                {
                    foreach (var e in l)
                    {
                        ks.Add(e.k);
                    }
                }
                return ks;
            }
        }

        public ICollection<Tv> Values
        {
            get
            {
                List<Tv> vs = new List<Tv>();
                foreach (var l in hash_table)
                {
                    foreach (var e in l)
                    {
                        vs.Add(e.v);
                    }
                }
                return vs;
            }
        }

        public int Count => hash_table.Where(x => x != null).Select(x => x.Count).Sum();

        public bool IsReadOnly => false;

        public void Add(Tk k, Tv v)
        {
            int temp = k.GetHashCode();
            int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
            if (hash_table[ind].Any(x => x.k.Equals(k)))
            {
                throw new ArgumentException("Добавление дубликата");
            }
            hash_table[ind].AddLast(new NewEntry { hash_code = temp, k = k, v = v });
            int count = hash_table.Count(x => x.Count == 0);
            if (hash_table[ind].Count >= 5 || count < hash_table.Length / 3)
            {
                LinkedList<NewEntry>[] new_hash = new LinkedList<NewEntry>[hash_table.Length * 2];
                for (int i = 0; i < new_hash.Length; ++i)
                {
                    new_hash[i] = new LinkedList<NewEntry>();
                }
                foreach (var l in hash_table)
                {
                    foreach (var elem in l)
                    {
                        temp = elem.hash_code;
                        ind = ((temp % new_hash.Length) + new_hash.Length) % new_hash.Length;
                        new_hash[ind].AddLast(elem);
                    }
                }
                hash_table = new_hash;
            }

        }

        public void Add(KeyValuePair<Tk, Tv> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            hash_table = new LinkedList<NewEntry>[10];
        }

        public bool Remove(Tk k)
        {
            int temp = k.GetHashCode();
            int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
            if (hash_table[ind].Any(x => x.k.Equals(k)))
            {
                NewEntry temp_ent = new NewEntry();
                foreach (NewEntry i in hash_table[ind])
                {
                    if (i.k.Equals(k))
                    {
                        temp_ent = i;
                        break;
                    }
                }
                return hash_table[ind].Remove(temp_ent);
            }
            return false;
        }

        public bool Remove(KeyValuePair<Tk, Tv> item)
        {
            int temp = item.Key.GetHashCode();
            int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
            NewEntry temp_ent = new NewEntry { hash_code = temp, k = item.Key, v = item.Value };
            if (hash_table[ind].Contains(temp_ent))
            {
                return hash_table[ind].Remove(temp_ent);
            }
            return false;
        }

        public bool Contains(KeyValuePair<Tk, Tv> item)
        {
            int temp = item.Key.GetHashCode();
            int index = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
            NewEntry temp_ent = new NewEntry { hash_code = temp, k = item.Key, v = item.Value };
            return hash_table[index].Contains(temp_ent);
        }

        public bool ContainsKey(Tk k)
        {
            int temp = k.GetHashCode();
            int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
            return hash_table[ind].Any(x => x.k.Equals(k));
        }

        public void CopyTo(KeyValuePair<Tk, Tv>[] mas, int mas_ind)
        {
            foreach (var l in hash_table)
            {
                foreach (var e in l)
                {
                    mas[mas_ind] = new KeyValuePair<Tk, Tv>(e.k, e.v);
                    mas_ind++;
                }
            }
        }

        public bool TryGetValue(Tk k, [MaybeNullWhen(false)] out Tv v)
        {
            int temp = k.GetHashCode();
            int ind = ((temp % hash_table.Length) + hash_table.Length) % hash_table.Length;
            foreach (var e in hash_table[ind])
            {
                if (e.k.Equals(k))
                {
                    v = e.v;
                    return true;
                }
            }
            v = default(Tv);
            return false;
        }

        public IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            foreach (var l in hash_table)
            {
                foreach (var e in l)
                {
                    yield return new KeyValuePair<Tk, Tv>(e.k, e.v);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Tk, Tv>>)this).GetEnumerator();
        }
    }
}
