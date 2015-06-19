using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class Key
    {
        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private Int32 level;

        public Int32 Level
        {
            get { return level; }
            set { level = value; }
        }

        private Int32 levelEx;

        public Int32 LevelEx
        {
            get { return levelEx; }
            set { levelEx = value; }
        }

        private String data;

        public String Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
