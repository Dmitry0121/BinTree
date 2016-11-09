using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BinTree
{
    class MyMap<K, T> : IDictionary<K, T>, IEnumerator, IEnumerator<KeyValuePair<K, T>>
        where K : IComparable<K>
    {
        #region constructors
        public MyMap()
        {

        }
        #endregion

        #region classes
        public class TreeItem
        {
            public K Key { get; set; }
            public T Value { get; set; }
            public TreeItem Parent { get; set; }
            public TreeItem Left { get; set; }
            public TreeItem Right { get; set; }

            public int CompareTo(K other)
            {
                return Key.CompareTo(other);
            }
        }
        #endregion

        #region fields

        private KeyValuePair<K, T> _current;
        private TreeItem _root = null;

        #endregion

        #region Properties
        public int Count
        {
            get;
            private set;
        }

        public ICollection<K> Keys
        {
            get
            {
                List<K> keys = new List<K>();
                ForEach((x, y) => keys.Add(x));
                return keys;
            }
        }

        public ICollection<T> Values
        {
            get
            {
                List<T> values = new List<T>();
                ForEach((x, y) => values.Add(y));
                return values;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public KeyValuePair<K, T> Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _current;
            }
        }
        #endregion

        #region Indexators
        public T this[K key]
        {
            get
            {
                TreeItem item = GetItem(key);
                if (item == null)
                {
                    throw new IndexOutOfRangeException(String.Format("key {0} not found", key));
                }
                return item.Value;
            }

            set
            {
                Add(key, value);
            }
        }
        #endregion

        #region Metods
        public void Print(K key)
        {
            if (_root == null)
            {
                Console.WriteLine("Tree is Empty!!!");
            }
            else
            {
                TreeItem current, parent;
                current = FindWithParent(key, out parent);

                TreeItem item = GetItem(key);
                Console.WriteLine(current.Value.ToString());
            }
        }

        public void Add(K key, T value)
        {
            if (_root == null)
            {
                _root = new TreeItem { Key = key, Value = value };
                ++Count;
            }
            else
            {
                Add(key, value, _root);
            }
        }
        protected void Add(K key, T value, TreeItem item)
        {
            //not repeated
            if (key.CompareTo(item.Key) == 0)
            {
                item.Value = value;
                return;
            }

            if (key.CompareTo(item.Key) > 0)
            {
                if (item.Right == null)
                {
                    item.Right = new TreeItem { Parent = item, Key = key, Value = value };
                    ++Count;
                }
                else
                {
                    Add(key, value, item.Right);
                }
            }
            else
            {
                if (item.Left == null)
                {
                    item.Left = new TreeItem { Parent = item, Key = key, Value = value };
                    ++Count;
                }
                else
                {
                    Add(key, value, item.Left);
                }
            }
        }

        public void ForEach(Action<K, T> action)
        {
            if (_root != null)
            {
                ForEach(action, _root);
            }
        }
        public void ForEach(Action<K, T> action, TreeItem item)
        {
            if (item.Left != null)
            {
                ForEach(action, item.Left);
            }

            action(item.Key, item.Value);

            if (item.Right != null)
            {
                ForEach(action, item.Right);
            }
        }
        public void ForEach(Action<KeyValuePair<K, T>> action, TreeItem item)
        {
            if (item.Left != null)
            {
                ForEach(action, item.Left);
            }

            action(new KeyValuePair<K, T>(item.Key, item.Value));

            if (item.Right != null)
            {
                ForEach(action, item.Right);
            }
        }

        public bool ContainsKey(K key)
        {
            TreeItem item = GetItem(key);
            return item != null;

        }

        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public int CountItems()
        {

            return Count;
        }

        public TreeItem GetItem(K key)
        {
            if (_root == null)
                return null;

            return GetItem(key, _root);
        }
        public TreeItem GetItem(K key, TreeItem item)
        {
            if (key.CompareTo(item.Key) == 0)
            {
                return item;
            }

            if (key.CompareTo(item.Key) > 0)
            {
                if (item.Right == null)
                {
                    return null;
                }
                else
                {
                    return GetItem(key, item.Right);
                }
            }
            else
            {
                if (item.Left == null)
                {
                    return null;
                }
                else
                {
                    return GetItem(key, item.Left);
                }
            }
        }

        private TreeItem FindWithParent(K key, out TreeItem parent)
        {
            // looking item
            TreeItem current = _root;
            parent = null;

            while (current != null)
            {
                int result = current.CompareTo(key);

                if (result > 0)
                {
                    // item < 0 (left)
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // item > 0 (right)
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }

            return current;
        }

        public bool Remove(K key)
        {
            TreeItem current, parent;
            current = FindWithParent(key, out parent);

            if (current == null)
            {
                return false;
            }

            Count--;

            // Case 1: If there are no children on the right, left the child falls into place removed.
            if (current.Right == null)
            {
                if (parent == null)
                {
                    _root = current.Left;
                }
                else
                {
                    var result = parent.CompareTo(current.Key);
                    if (result > 0)
                    {
                        // If the current value is greater than the parent,
                        // Left child of the current node becomes the left child of the parent.
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // If the current value is less than the parent,
                        // Left child of the current node becomes the right child of the parent.
                        parent.Right = current.Left;
                    }
                }
            }

            // Case 2: If the child has no right to the left children
            //         It takes the place of the deleted node.
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    _root = current.Right;
                }
                else
                {
                    var result = parent.CompareTo(current.Key);
                    if (result > 0)
                    {
                        // If the current value is greater than the parent,
                        // Right child of the current node becomes the left child of the parent.
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // If the current value is less than the parent,
                        // Right child of the current node becomes the right child of the parent.
                        parent.Right = current.Right;
                    }
                }
            }

            // Case 3: If the rights of the child have children left, the leftmost child
            //         Right subtree of node replaces deleted.
            else
            {
                // Find the leftmost node.
                TreeItem leftmost = current.Right.Left;
                TreeItem leftmostParent = current.Right;

                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // The left subtree right subtree of the parent becomes the leftmost node.
                leftmostParent.Left = leftmost.Right;

                // Left and right child of the current node becomes the left and right of the leftmost child.
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                {
                    _root = leftmost;
                }
                else
                {
                    var result = parent.CompareTo(current.Key);
                    if (result > 0)
                    {
                        // If the current value is greater than the parent,
                        // Leftmost node becomes the left child rodielya.
                        parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // If the current value is less than the parent,
                        // Leftmost node becomes the right child rodielya.
                        parent.Right = leftmost;
                    }
                }
            }
        return true;
        }

        public bool Remove(KeyValuePair<K, T> item)
        {
            return false;
        }

        public bool TryGetValue(K key, out T value)
        {
            TreeItem item = GetItem(key);
            if (item == null)
            {
                value = default(T);
                return false;
            }
            else
            {
                value = item.Value;
                return true;
            }
        }

        public void Add(KeyValuePair<K, T> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public bool Contains(KeyValuePair<K, T> pair)
        {
            TreeItem item = GetItem(pair.Key);

            return item != null && item.GetHashCode() == pair.GetHashCode() && item.Value.GetHashCode() == pair.Value.GetHashCode();
        }

        public void CopyTo(KeyValuePair<K, T>[] array, int arrayIndex)
        {

            ForEach((k, t) => array[arrayIndex++] = new KeyValuePair<K, T>(k, t));
        }

        public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {

            _current = default(KeyValuePair<K, T>);
        }
        #endregion
    }
}
