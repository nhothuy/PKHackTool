using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PKTool
{
    [Serializable()]
    public class Friend
    {
        private int index = 1;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private string id = String.Empty;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name = String.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string key = String.Empty;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private Int32 rank = 0;

        public Int32 Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        private Int32 spin = 0;

        public Int32 Spin
        {
            get { return spin; }
            set { spin = value; }
        }

        private Int32 cash = 0;

        public Int32 Cash
        {
            get { return cash; }
            set { cash = value; }
        }
        private String sToken = String.Empty;

        public String SToken
        {
            get { return sToken; }
            set { sToken = value; }
        }
        private String bToken = String.Empty;

        public String BToken
        {
            get { return bToken; }
            set { bToken = value; }
        }

        private int type = 1;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
