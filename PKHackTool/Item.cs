using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class Item
    {
        private int index = 0;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        private String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private bool isdamaged = false;
        public bool Isdamaged
        {
            get { return isdamaged; }
            set { isdamaged = value; }
        }
        private int level = 0;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        private Int32 price = 0;
        public Int32 Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
