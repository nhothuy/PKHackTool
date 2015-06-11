using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    class CompletedIslandsData
    {
        private int islandIndex = 0;

        public int IslandIndex
        {
            get { return islandIndex; }
            set { islandIndex = value; }
        }
        private int upgradeLevel = 0;

        public int UpgradeLevel
        {
            get { return upgradeLevel; }
            set { upgradeLevel = value; }
        }

    }
}
