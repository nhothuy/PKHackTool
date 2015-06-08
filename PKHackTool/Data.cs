using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKTool
{
    class Data
    {
        private int rank = 0;

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        private String msg;

        public String Msg
        {
            get { return msg; }
            set { msg = value; }
        }
        private Friend friend;

        internal Friend Friend
        {
            get { return friend; }
            set { friend = value; }
        }
    }
}
