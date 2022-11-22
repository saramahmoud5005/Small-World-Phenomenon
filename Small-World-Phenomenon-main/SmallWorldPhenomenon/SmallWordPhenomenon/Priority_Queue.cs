using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallWordPhenomenon
{
    public class triple
    {
        public int first;//DoS
        public int second;//RS
        public string third;//Node name
        public triple()
        {
            first = 0;
            second = 0;
            third = "";
        }
        public triple(int first, int second, string third)
        {
            this.first = first;
            this.second = second;
            this.third = third;
        }
    }
    class Priority_Queue
    {
        public List<triple> list;
        public int Count { get { return list.Count; } }
        public Priority_Queue()
        {
            list = new List<triple>();
        }
        public Priority_Queue(int count)
        {
            list = new List<triple>(count);
        }
        public triple top()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0];
        }
        public void push(triple x)
        {
            list.Add(x);

            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (list[p].first < x.first)
                {
                    break;
                }
                else if (list[p].first == x.first && list[p].second >= x.second)
                {
                    break;
                }

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = x;
        }
        public void pop()
        {
            triple root = list[Count - 1];
            list.RemoveAt(Count - 1);

            int i = 0;
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1;
                int b = i * 2 + 2;
                int c;
                if (b < Count && list[b].first < list[a].first)
                    c = b;
                else if (b < Count && list[b].first == list[a].first && list[b].second >= list[a].second)
                    c = b;
                else
                    c = a;

                if (list[c].first > root.first)
                    break;
                else if (list[c].first == root.first && list[c].second <= root.second)
                    break;

                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = root;
        }
        public bool empty()
        {
            if (Count == 0)
                return true;
            else
                return false;
        }
    }
}
