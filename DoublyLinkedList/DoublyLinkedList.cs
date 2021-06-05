using System;
using System.Collections;
using System.Collections.Generic;

namespace DoublyLinkedList
{
    class DoublyLinkedList<T> : ICollection<T>,
        IEnumerable<T>, IList<T>
    {
        private int _count;
        private DoublyLinkedListNode<T> _head;
        private DoublyLinkedListNode<T> _tail;

        public DoublyLinkedListNode<T> First => _head;

        public DoublyLinkedListNode<T> Last => _tail;

        public T this[int index]
        {
            get
            {
                if (index > _count - 1)
                    throw new IndexOutOfRangeException("Index can't be more then count");

                if (index < 0)
                    throw new IndexOutOfRangeException("Index can't be less then 0");

                var result = GoToIndex(index);
                return result.value; 
            }

            set
            {
                if (index > _count - 1)
                    throw new IndexOutOfRangeException("Index can't be more then count");

                if (index < 0)
                    throw new IndexOutOfRangeException("Index can't be less then 0");

                var result = GoToIndex(index);
                result.value = value;
            }

        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException("Argument can't be null");

            var current = new DoublyLinkedListNode<T>
            {
                value = item
            };

            if (_head == null)
            {
                _head = current;
            }

            else
            {
                _tail.next = current;
                current.previous = _tail;
            }

            _tail = current;
            _count++;
        }

        public void Clear()
        {
            _count = 0;
            _head = null;
            _tail = null;
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException("Argument can't be null");

            if (Find(item) == null)            
                return false;
            
            return item.Equals(Find(item).value);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in array) 
            {
                if (item == null)
                    throw new ArgumentNullException("Argument can't be null");
            }

            if (arrayIndex < 0)
                throw new IndexOutOfRangeException("Index can't be less then 0");

            if (arrayIndex + _count > array.Length)
                throw new ArgumentException("Destination array was not long enough.Check the destination index, length, and the array's lower bounds. (Parameter 'destinationArray')");

            var node = _head;

            while (node.next != null)
            {
                array[arrayIndex] = node.value;
                arrayIndex++;
                node = node.next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DoublyLinkedListEnumerator<T>(this);
        }

        public int IndexOf(T item)
        {
            if (item == null)
                throw new ArgumentNullException("Argument can't be null");

            Find(item, out int result);

            return result;
        }

        //TODO null in item
        public void Insert(int index, T item)
        {
            if(item == null)
                throw new ArgumentNullException("Argument can't be null");

            if (index > _count - 1)
                throw new IndexOutOfRangeException("Index can't be more then count");

            if (index < 0)
                throw new IndexOutOfRangeException("Index can't be less then 0");

            var newNode = new DoublyLinkedListNode<T>
            {
                value = item
            };

            var oldNode = GoToIndex(index);

            newNode.next = oldNode;
            newNode.previous = oldNode.previous;

            oldNode.previous.next = newNode;
            oldNode.previous = newNode;
        }

        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException("Argument can't be null");

            if (!Contains(item))
            {
                return false;
            }

            var removableNode = Find(item);

            if (removableNode.previous != null)
            {
                var prevNode = removableNode.previous;
                prevNode.next = removableNode.next;
            }
            
            if (removableNode.next != null)
            {
                var nextNode = removableNode.next;
                nextNode.previous = removableNode.previous;
            }

            return true;            
        }

        public void RemoveAt(int index)
        {
            if (index > _count - 1)
                throw new IndexOutOfRangeException("Index can't be more then count");

            if (index < 0)
                throw new IndexOutOfRangeException("Index can't be less then 0");

            var item = GoToIndex(index).value;
            Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DoublyLinkedListNode<T> Find (T value)
        {
            if (value == null)
                throw new ArgumentNullException("Argument can't be null");

            var node = _head;

            while (node != null)
            {
                if (node.value.Equals(value))
                    return node;

                node = node.next;           
            }

            return node;   
        }

        public DoublyLinkedListNode<T> Find(T value, out int index)
        {
            if (value == null)
                throw new ArgumentNullException("Argument can't be null");

            var node = _head;
            index = -1;

            while (node != null)
            {
                index++;

                if (node.value.Equals(value))
                    return node;

                node = node.next;
            }

            index = -1;
            return node;
        }

        private DoublyLinkedListNode<T> GoToIndex (int index)
        {
            if (index > _count - 1)
                throw new IndexOutOfRangeException("Index can't be more then count");

            if (index < 0)
                throw new IndexOutOfRangeException("Index can't be less then 0");

            var result = _head;

            for (int i = 1; i <= index; i++)
            {
                result = result.next;
            }

            return result;
        }


    }

    internal class DoublyLinkedListNode<T>
    {
        internal DoublyLinkedListNode<T> next;
        internal DoublyLinkedListNode<T> previous;
        internal T value;
    }

    internal class DoublyLinkedListEnumerator<T> : IEnumerator<T>
    {
        private int _index = 0;
        private DoublyLinkedList<T> _list;
        private DoublyLinkedListNode<T> _node;
        private T _current;
       
        public DoublyLinkedListEnumerator(DoublyLinkedList<T> list)
        {
            _list = list;
            _node = list.First;
            _current = default;
        }

        public T Current => _current;

        object IEnumerator.Current => Current;

        public void Dispose() 
        {
            _list.Clear();
            _index = 0;
            _node = null;
            _current = default;
        }
       
        public bool MoveNext()
        {
            if (_node == null)          
                return false;         

            _index++;
            _current = _node.value;
            _node = _node.next;

            return true;
        }

        public void Reset()
        {
            _current = default;
            _node = _list.First;
            _index = 0;
        }
    }
}
