using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class SimplePlayer : IComparable<SimplePlayer>
    {
        public byte Avatar;
        public string ExtraData;
        public long FBID;
        public byte Flag;
        public SimplePlayer.SimplePlayerType PlayerType;
        public int RankPoints;
        private string _name;

        public string Name
        {
            get
            {
                return this._name ?? string.Empty;
            }
            set
            {
                this._name = value;
            }
        }

        public int CompareTo(SimplePlayer other)
        {
            return -this.RankPoints.CompareTo(other.RankPoints);
        }

        //public override string ToString()
        //{
        //    string format = "SimplePlayer - Name: {0} Rank Points: {3} Avatar: {1} Flag: {2} FBID: {4}";
        //    object[] objArray = new object[5];
        //    int index1 = 0;
        //    string name = this.Name;
        //    objArray[index1] = (object)name;
        //    int index2 = 1;
        //    // ISSUE: variable of a boxed type
        //    __Boxed<byte> local1 = (ValueType)this.Avatar;
        //    objArray[index2] = (object)local1;
        //    int index3 = 2;
        //    // ISSUE: variable of a boxed type
        //    __Boxed<byte> local2 = (ValueType)this.Flag;
        //    objArray[index3] = (object)local2;
        //    int index4 = 3;
        //    // ISSUE: variable of a boxed type
        //    __Boxed<int> local3 = (ValueType)this.RankPoints;
        //    objArray[index4] = (object)local3;
        //    int index5 = 4;
        //    // ISSUE: variable of a boxed type
        //    __Boxed<long> local4 = (ValueType)this.FBID;
        //    objArray[index5] = (object)local4;
        //    return string.Format(format, objArray);
        //}

        public enum SimplePlayerType
        {
            Random,
            Friend,
            Revenge,
            News,
            Me,
            Invite,
        }
    }
}
