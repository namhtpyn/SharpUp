using System;
using System.Collections.Generic;
using System.Text;

namespace SharpUp.Extension
{
    public class ListItem<T1, T2>
    {
        public ListItem(T1 value, T2 text)
        {
            Value = value;
            Text = text;
        }

        public T1 Value { get; set; }
        public T2 Text { get; set; }
    }
}