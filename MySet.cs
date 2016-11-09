using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BinTree
{
    class MySet<T> 
        where T:IComparable<T>
    {
        #region Classes
        public class SetItem
        {
            public T Value { get; set; }
            public SetItem Parent { get; set; }
            public SetItem Left { get; set; }
            public SetItem Right { get; set; }
        }
        #endregion

        #region constructors
        public MySet() { }
        #endregion

        #region fields
        private SetItem root=null;
        #endregion

        #region Metods
        public void Add(T value)
        {
            if (root == null)
            {
                root = new SetItem { Value = value };
            }
            else
            {
                Add(value, root);
            }
        }

        protected void Add(T value, SetItem item)
        {
            //not repeated
            if (value.CompareTo(item.Value) == 0) return;

            if(value.CompareTo(item.Value)>0)
            {
                if (item.Right == null)
                {
                    item.Right = new SetItem { Parent = item, Value = value };
                }
                else
                {
                    Add(value, item.Right);
                }
            }
            else
            {
                if (item.Left == null)
                {
                    item.Left = new SetItem { Parent = item, Value = value };
                }
                else
                {
                    Add(value, item.Left);
                }
            }
        }

        public void Print()
        {
            if(root == null)
            {
                Console.WriteLine("Tree is Empty!!!");
            }
            else
            {
                Print(root);
                Console.WriteLine();
            }
        }

        private void Print(SetItem item)
        {
            if (item.Left != null)
            {
                Print(item.Left);
            }

            Console.Write("{0}; ", item.Value);

            if (item.Right != null)
            {
                Print(item.Right);
            }
        }
        
        public void ForEach(Action<T> action)
        {
            if (root != null)
            {
                ForEach(action, root);                
            }
        }

        public void ForEach(Action<T> action, SetItem item)
        {
            if (item.Left != null)
            {
                ForEach(action, item.Left);
            }

            action( item.Value);

            if (item.Right != null)
            {
                ForEach(action, item.Right);
            }
        }
        #endregion
    }
}
