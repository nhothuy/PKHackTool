using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class SimplePlayer : IComparable<SimplePlayer>
    {
        public string Name;
        public byte Avatar;
        public string ExtraData;
        public long FBID;
        public byte Flag;
        public SimplePlayerType PlayerType;
        public int RankPoints;

        public int CompareTo(SimplePlayer other)
        {
            return this.RankPoints.CompareTo(other.RankPoints);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.Name, this.RankPoints, this.FBID };
            return string.Format("Name: {0} Rank: {1} FBID: {2}", args);
        }

        //public string Name
        //{
        //    get
        //    {
        //        if (this._name == null)
        //        {
        //        }
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        this._name = value;
        //    }
        //}

        public enum SimplePlayerType
        {
            Random,
            Friend,
            Revenge,
            News,
            Me,
            Invite
        }
    }
}
