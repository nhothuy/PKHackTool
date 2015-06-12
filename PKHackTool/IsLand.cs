using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class IsLand
    {
        private int index = 0;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        private List<Item> items = new List<Item>();

        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
