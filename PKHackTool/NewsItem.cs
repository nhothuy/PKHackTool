using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class NewsItem : IComparable<NewsItem>
    {
        public AttackNewsData AttackData;
        public SimplePlayer ByWhom;
        public DateTime LocalEventTime;
        public PromoNewsData PromoData;
        public int SecondsAgo;
        public StealNewsData StealData;
        public string StringData;
        public NewsType Type;

        public int CompareTo(NewsItem other)
        {
            return this.SecondsAgo.CompareTo(other.SecondsAgo);
        }

        public override string ToString()
        {
            String type = this.AttackData == null ? (this.StealData == null ? "" : "Steal") : "Attack";
            String message = this.AttackData == null ? (this.StealData == null ? "" : this.StealData.ToString()) : this.AttackData.ToString();
            String byWhom = ByWhom.ToString();
            object[] args = new object[] { type, byWhom, message };
            return string.Format("{0} | {1} | {2}", args);
        }
    }
    public class AttackNewsData
    {
        public IslandItemType AttackedItem;
        public AttackResult Result;
        public AttackType Type;
        public override string ToString()
        {
            object[] args = new object[] { this.AttackedItem.ToString(), Result.ToString(), Type.ToString()};
            return string.Format("Item: {0} Result: {1} Type: {2}", args);
        }
    }
    public enum IslandItemType
    {
        Artifacts,
        Nature,
        Ships,
        Building,
        Animals
    }
    public enum AttackResult
    {
        Shield,
        DamagedItem,
        DestroyedItem
    }
    public enum AttackType
    {
        Random,
        Friend,
        Revenge
    }
    public class PromoNewsData
    {
        public int Amount;
        public PromoType Type;
    }
    public enum PromoType
    {
        Cash,
        Spins
    }
    public enum NewsType
    {
        Attack,
        Steal,
        PromoUse,
        FriendFinishIsland,
        InviteFBFriend,
        AdminMessage
    }
    public class StealNewsData
    {
        public int StolenAmount;
        public override string ToString()
        {
            object[] args = new object[] { this.StolenAmount };
            return string.Format("StolenAmount: {0}", args);
        }
    }
}
