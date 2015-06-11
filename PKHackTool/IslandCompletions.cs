using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    class IslandCompletions
    {
        private String completeTime;

        public String CompleteTime
        {
            get { return completeTime; }
            set { completeTime = value; }
        }

        private int islandLevel = 0;

        public int IslandLevel
        {
            get { return islandLevel; }
            set { islandLevel = value; }
        }
    }
}
