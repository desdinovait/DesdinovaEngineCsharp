using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsGameLibrary1
{
    //Classe che crea un pool generico di elementi dal quale si può prelevare.
    //Link: http://www.ziggyware.com/readarticle.php?article_id=213
    //Vedere uso in fondo al file
    public class Pool<T> where T : new()
    {
        //Stack (LIFO)
        private Stack<T> _stack;

        //Conto elementi presenti
        public int Count
        {
            get { return _stack.Count; }
        }

        //Crea uno stack vuoto
        public Pool()
        {
            _stack = new Stack<T>();
        }

        //Crea uno stack di dimensioni fisse
        public Pool(int size)
        {
            _stack = new Stack<T>(size);
            for (int i = 0; i < size; i++)
            {
                _stack.Push(new T());
            }
        }

        //Preleva l'elemento
        public T Fetch()
        {
            if (_stack.Count > 0)
            {
                return _stack.Pop();
            }
            return new T();
        }

        //Ritorno l'elemento
        public void Return(T item)
        {
            _stack.Push(item);
        }

        //Cancella lo stack
        public void Clear()
        {
            _stack.Clear();
        }

        //Ritorna lo stack sotto forma di array
        public T[] ToArray()
        {
            return _stack.ToArray();
        }
    }
}


//Uso:
//Crea il pool
//Pool<float> pool;
//pool = new Pool<float>(100);

//Preleva 2 elementi
//float a = pool.Fetch();
//float b = pool.Fetch();
//a = 10.0f;
//b = 33.0f;

//Ritorna i due elementi nello stack
//pool.Return(a);
//pool.Return(b);

//Preleva nuovamente il primo elemento
//float c = pool.Fetch();
