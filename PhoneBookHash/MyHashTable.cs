using System;
using System.Collections.Generic;
using System.Collections;

namespace PhoneBookHash
{
    class MyHashTable : IEnumerable<Note>
    {
        HashItem[] items;
        public int Size { get { return items.Length; } }
        static int step = 3;
        int count;

        public HashItem this[int index]
        {
            get
            {
                return items[index];
            }
        }

        public MyHashTable()
        {
            items = new HashItem[19];

            InitHash();
        }

        private void InitHash()
        {
            count = 0;

            for (int i = 0; i < Size; i++)
            {
                items[i].IsEmpty = true;
                items[i].IsVisited = false;
            }
        }

        public int Add(string name, string phone)
        {
            int index = -1;

            if (count < Size)
            {
                index = GetHash(phone);

                while (!items[index].IsEmpty)
                    index = (index + step) % Size;

                items[index].IsEmpty = false;
                items[index].Note.Name = name;
                items[index].Note.Phone = phone;

                count++;
            }

            return index;
        }

        private void ClearVisit()
        {
            for (int i = 0; i < Size; i++)
            {
                items[i].IsVisited = false;
            }
        }

        public bool Remove(string phone)
        {
            if (count != 0)
            {
                int i = FindIndex(phone);

                if (i != -1)
                {
                    items[i].IsEmpty = true;
                    items[i].Note = new Note();
                    this.count--;

                    return true;
                }
            }

            return false;
        }

        public int FindIndex(string phone)
        {
            ClearVisit();

            int i = GetHash(phone);

            do
            {
                if (items[i].Note.Phone == phone)
                {
                    return i;
                }

                items[i].IsVisited = true;
                i = (i + step) % Size;
            }
            while (!items[i].IsVisited);

            return -1;
        }

        public string Find(string phone)
        {
            int index = FindIndex(phone);

            if (index == -1)
                return $"Абонент с номером {phone} не найден";
            else
                return $"Абонент найден -> Номер:  {items[index].Note.Phone}. Фамилия: {items[index].Note.Name}";
        }

        private int GetHash(string s)
        {
            int result = 0;

            for (int i = 0; i < s.Length; i++)
            {
                result += Convert.ToInt32(s[i]) * i;
                result %= Size;
            }
            return result;
        }

        public IEnumerator<Note> GetEnumerator()
        {
            foreach (var item in items)
            {
                if (!item.IsEmpty)
                    yield return item.Note;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}