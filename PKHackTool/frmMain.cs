using Fiddler;
using MyUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using HAP = HtmlAgilityPack;
namespace PKTool
{
    /// <summary>
    /// Pirate Kings Tool
    /// Author  : thuyln
    /// Email   : nhothuy48cb@gmail.com
    /// </summary>
    public partial class frmMain : Form
    {
        #region "PRAMS"
        private Regex REGEX_ID = new Regex(@"ajax/hovercard/user.php\?id=(?<fid>[\d].*)&.*");
        private Regex REGEX_NAME = new Regex(@"https://www.facebook.com/(?<fname>.*)\?.*");
        private String BUSINESSTOKEN = "";
        private String ACCESSTOKEN = "";
        private String SECRETKEY = "";
        private String SESSIONTOKEN = "";
        public List<Friend> FRIENDS = new List<Friend>();
        private UrlCaptureConfiguration CaptureConfiguration { get; set; }
        private const string SEPARATOR = "-------------------------------------------------------------------------------";
        private const string CAPTUREDOMAIN = "http://prod.cashkinggame.com/CKService.svc/v3.0/login";
        private BackgroundWorker M_PLAY;
        private const Int32 RANKPOINT_STEAL = 200;
        private Friend VICTIM = null;
        private String NAME = String.Empty;
        private String FBID = String.Empty;
        private static String[] ARRITEMS = { "Animals", "Nature", "Building", "Ships", "Artifacts" };
        private const String URLLOGIN = "http://prod.cashkinggame.com/CKService.svc/v3.0/login/?{0}";
        private const String URLWHEEL = "http://prod.cashkinggame.com/CKService.svc/v3.0/spin/wheel/?{0}";
        private const String URLATTACKFRIEND = "http://prod.cashkinggame.com/CKService.svc/v3.0/attack/friend/?";
        private const String URLATTACKRANDOM = "http://prod.cashkinggame.com/CKService.svc/v3.0/attack/random/?{0}";
        private const String URLVIEW = "http://prod.cashkinggame.com/CKService.svc/v3.0/island/view/friend/?";
        private const String URLSTEAL = "http://prod.cashkinggame.com/CKService.svc/v3.0/attack/steal/?{0}";
        private const String URLUPGRADE = "http://prod.cashkinggame.com/CKService.svc/v3.0/island/upgrade/?{0}";
        private const String URLREPAIR = "http://prod.cashkinggame.com/CKService.svc/v3.0/island/repair/?{0}";
        private const String URLFINISH = "http://prod.cashkinggame.com/CKService.svc/v3.0/island/finish/?{0}";
        private const String URLINVITEBONUS = "http://prod.cashkinggame.com/CKService.svc/v3.0/invite/complete/?{0}";
        private const String URLVIDEOCLAIMBONUS = "http://prod.cashkinggame.com/CKService.svc/v3.0/incentivized/video/claimbonus/?{0}";
        private const String URLCOMPLETEDUPGRADE = "http://prod.cashkinggame.com/CKService.svc/v3.0/island/completed/upgrade/?{0}";
        private const String URLSPINSLOT = "http://prod.cashkinggame.com/CKService.svc/v3.0/spin/slot/?{0}";
        private const String URLSLOTCHEST = "http://prod.cashkinggame.com/CKService.svc/v3.0/slot/chest/?{0}";
        ////private const String URLCHANGENAME = "http://prod.cashkinggame.com/CKService.svc/v3.0/change/name/?{0}";
        ////private const String URLISLANDCOMPLETEDCLAIM = "http://prod.cashkinggame.com/CKService.svc/v3.0/island/completed/claim/?{0}";
        private const String URLCHEAT = "http://prod.cashkinggame.com/CKService.svc/v3.0/cheat/?{0}";
        private const String URL = "http://222.255.29.210/ws_mbox/pk.json";
        private List<Int32> BASEPRICES = new List<Int32>();
        private List<double> PRICESTEPS = new List<double>();
        private List<Item> ITEMS = new List<Item>();
        private List<String> FRIENDFBIDS = new List<string>();
        private int ISLANDINDEX = 0;
        List<NewsItem> LISTNEWS = new List<NewsItem>();
        private String UDID = "108a61cda531152f01e5436ba1a5b4fcf0acc23f";
        private const String KEY = "LEnHOtHuY";
        private Boolean ISVIP = false;
        private String REQBODY = String.Empty;
        private int RETRYCOUNT = 0;
        #endregion

        #region "INIT"
        public frmMain()
        {
            InitializeComponent();
            //
            CaptureConfiguration = new UrlCaptureConfiguration();
            CaptureConfiguration.IgnoreResources = false;
            CaptureConfiguration.CaptureDomain = CAPTUREDOMAIN;
            //
            M_PLAY = new BackgroundWorker();
            M_PLAY.DoWork += new DoWorkEventHandler(m_Play_DoWork);
            M_PLAY.ProgressChanged += new ProgressChangedEventHandler(m_Play_ProgressChanged);
            M_PLAY.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_Play_RunWorkerCompleted);
            M_PLAY.WorkerReportsProgress = true;
            M_PLAY.WorkerSupportsCancellation = true;
        }
        #endregion
        #region "M_PLAY"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Play_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>)e.Argument;
            
            Boolean isAttackRandom = Convert.ToBoolean(dic["isAttackRandom"]);
            Int16 typeAttack = Convert.ToInt16(dic["typeAttack"]);
            Friend victim = (Friend)dic["victim"];
            Int32 num = Convert.ToInt32(dic["num"]);
            Boolean isStealAuto = Convert.ToBoolean(dic["isStealAuto"]);
            Boolean isFullShields = Convert.ToBoolean(dic["isFullShields"]);
            Boolean isAutoUpgrade = Convert.ToBoolean(dic["isAutoUpgrade"]);
            Int32 count = 0;
            while (true)
            {
                String retWheel = wheel(SECRETKEY, SESSIONTOKEN);
                JToken data = JObject.Parse(retWheel);
                int wheelResult = Convert.ToInt16(data["WheelResult"]);
                Int64 spins = Convert.ToInt64(data["PlayerState"]["Spins"]);
                int shields = Convert.ToInt16(data["PlayerState"]["Shields"]);
                String playerInfo = "PlayerState:" + "\r\n" + String.Format("Rank:{0} Shields:{1} Spins:{2} Cash:{3} NextSpin: {4}", data["PlayerState"]["RankPoints"], data["PlayerState"]["Shields"], data["PlayerState"]["Spins"], Convert.ToInt64(data["PlayerState"]["Cash"]).ToString("#,#", CultureInfo.InvariantCulture), getTimes(Convert.ToInt32(data["NextSpinClaimSeconds"])));
                String cashKingInfo = "CashKing:" + "\r\n" + String.Format("Name:{1} Rank:{2} Cash:{3}", data["PlayerState"]["CashKing"]["FBID"], data["PlayerState"]["CashKing"]["Name"], data["PlayerState"]["CashKing"]["RankPoints"], Convert.ToInt64(data["PlayerState"]["CashKingCash"]).ToString("#,#", CultureInfo.InvariantCulture));
                Int64 cash = Convert.ToInt64(data["PlayerState"]["Cash"]);
                int levelIsland = 0;
                Boolean isOK = false;
                //Set retry
                RETRYCOUNT = 0;
                //
                if (wheelResult == 6)
                {
                    String retSteal = String.Empty;
                    String stealInfo = steal(SECRETKEY, SESSIONTOKEN, data, isStealAuto, out retSteal, out levelIsland, out isOK);
                    String retInfo = String.Empty;
                    if (isStealAuto)
                    {
                        JToken jTokenSteal = JObject.Parse(retSteal);
                        cashKingInfo = "CashKing:" + "\r\n" + String.Format("Name:{1} Rank:{2} Cash:{3}", jTokenSteal["PlayerState"]["CashKing"]["FBID"], jTokenSteal["PlayerState"]["CashKing"]["Name"], jTokenSteal["PlayerState"]["CashKing"]["RankPoints"], Convert.ToInt64(jTokenSteal["PlayerState"]["CashKingCash"]).ToString("#,#", CultureInfo.InvariantCulture));
                        cash = Convert.ToInt64(jTokenSteal["PlayerState"]["Cash"]);
                        retInfo = "[Steal]" + "\r\n" + playerInfo + "\r\n" + cashKingInfo + "\r\n" + "StolenAmount: " + Convert.ToInt64(jTokenSteal["StolenAmount"].ToString()).ToString("#,#", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        retInfo = "[Steal]" + "\r\n" + playerInfo + "\r\n" + cashKingInfo + "\r\n" + stealInfo;
                    }
                    Data info = new Data();
                    info.Msg = retInfo;
                    M_PLAY.ReportProgress(50, info);
                }
                if (isAttackRandom && wheelResult == 7)
                {
                    if (typeAttack == 1)
                    {
                        String attackInfo = attackRandom(SECRETKEY, SESSIONTOKEN, data);
                        JToken jTokenAttack = JObject.Parse(attackInfo);
                        cash = Convert.ToInt64(jTokenAttack["PlayerState"]["Cash"]);
                        Data info = new Data();
                        info.Msg = "[Attack]" + "\r\n" + playerInfo + "\r\n" + cashKingInfo + "\r\n" + String.Format("GoldGained: {0} Result: {1}", Convert.ToInt64(jTokenAttack["GoldGained"].ToString()).ToString("#,#", CultureInfo.InvariantCulture), jTokenAttack["Result"].ToString());
                        M_PLAY.ReportProgress(50, info);
                    }

                    if (typeAttack == 2)
                    {
                        Data info = new Data();
                        //
                        List<Item> itemTypes = getItemTypeAttack(victim.Id);
                        if (itemTypes.Count == 0)
                        {
                            info.Msg = "[Attack]" + "\r\n" + victim.Name + "'s island destroyed.";
                            M_PLAY.ReportProgress(100, info);
                            return;
                        }
                        count = count + 1;
                        String ret = attackFriend(victim.Id, itemTypes[0].Name);
                        JToken jToken = JObject.Parse(ret);
                        info.Msg = "[Attack]" + "\r\n" + playerInfo + "\r\n" + cashKingInfo + "\r\n" + String.Format("GoldGained: {0} Result: {1}", Convert.ToInt64(jToken["GoldGained"].ToString()).ToString("#,#", CultureInfo.InvariantCulture), jToken["Result"].ToString());
                        M_PLAY.ReportProgress(50, info);
                        if (num > 0)
                        {
                            if (count > num)
                            {
                                info.Msg = "[Attack]" + "\r\n" + String.Format("Attack {0}'s island {1} times", victim.Name,num);
                                M_PLAY.ReportProgress(100, info);
                                return;
                            }
                        }
                    }
                }
                if (isAutoUpgrade)
                {
                    while (true)
                    {
                        //repair
                        var itemsDamaged = (from i in ITEMS
                                            orderby i.Level ascending
                                            where i.Isdamaged == true
                                            select i).ToList();
                        if (itemsDamaged.Count > 0 && cash >= itemsDamaged[0].Price)
                        {
                            while (true)
                            {
                                String retRepair = repair(itemsDamaged[0].Name);
                                JToken jRetRepair = JObject.Parse(retRepair);
                                if (jRetRepair["Island"] == null || Convert.ToInt32(jRetRepair["ErrorCode"]) == 101) break;
                                Int32 cashSpent = Convert.ToInt32(jRetRepair["CashSpent"]);
                                Data info = new Data();
                                info.Msg = "[Repair] " + itemsDamaged[0].Name + " \r\n" + JObject.Parse(retRepair)["Island"][getItemName(itemsDamaged[0].Name)];
                                M_PLAY.ReportProgress(50, info);

                                ITEMS = getItems(JObject.Parse(retRepair));
                                setPrices(ITEMS);
                                cash = cash - cashSpent;
                                itemsDamaged = (from i in ITEMS
                                                orderby i.Level ascending
                                                where i.Isdamaged == true
                                                select i).ToList();
                                if (itemsDamaged.Count == 0 || cash < itemsDamaged[0].Price)
                                {
                                    break;
                                }
                            }
                        }
                        //upgrade
                        var itemsUpgrade = (from i in ITEMS
                                            orderby i.Price ascending
                                            where i.Level < 5
                                            select i).ToList();
                        if (itemsUpgrade.Count > 0 && cash >= itemsUpgrade[0].Price)
                        {
                            while (true)
                            {
                                String retUpgrade = upgrade(itemsUpgrade[0].Name);
                                JToken jRetUpgrade = JObject.Parse(retUpgrade);
                                if (jRetUpgrade["Island"] == null || Convert.ToInt32(jRetUpgrade["ErrorCode"]) == 101) break;
                                Int32 cashSpent = Convert.ToInt32(jRetUpgrade["CashSpent"]);
                                Data info = new Data();
                                info.Msg = "[Upgrade] " + itemsUpgrade[0].Name + "\r\n" + JObject.Parse(retUpgrade)["Island"][getItemName(itemsUpgrade[0].Name)];
                                M_PLAY.ReportProgress(50, info);
                                ITEMS = getItems(JObject.Parse(retUpgrade));
                                setPrices(ITEMS);
                                cash = cash - cashSpent;
                                itemsUpgrade = (from i in ITEMS
                                                orderby i.Price ascending
                                                where i.Level < 5
                                                select i).ToList();
                                if (itemsUpgrade.Count == 0 || cash < itemsUpgrade[0].Price)
                                {
                                    break;
                                }
                            }
                        }
                        //finish
                        if (isFinish())
                        {
                            //finish
                            String retFinish = finish();
                            JToken jTokenFinish = JObject.Parse(retFinish);
                            if (Convert.ToInt32(jTokenFinish["ErrorCode"]) == 113)
                            {
                                Data info = new Data();
                                info.Msg = "All finish, next island coming soon...";
                                M_PLAY.ReportProgress(100, info);
                                return;
                            }
                            //
                            if (jTokenFinish["Island"] == null) break;
                            ITEMS = getItems(jTokenFinish);
                            //
                            BASEPRICES = JsonConvert.DeserializeObject<List<Int32>>(JObject.Parse(retFinish)["NewIslandShopPrice"]["BasePrices"].ToString());
                            PRICESTEPS = JsonConvert.DeserializeObject<List<double>>(JObject.Parse(retFinish)["NewIslandShopPrice"]["PriceSteps"].ToString());
                            //
                            setPrices(ITEMS);
                            //
                            String retCompleted = completedUpgrade(ISLANDINDEX);
                            JToken jTokenCompleted = JObject.Parse(retCompleted);
                            if (Convert.ToInt32(jTokenCompleted["ErrorCode"]) == 100)
                            {
                                ISLANDINDEX = ISLANDINDEX + 1; 
                            }
                        }
                        //
                        if (!isNextUpgrade(cash)) break;
                    }
                }
                if (spins == 0 || (wheelResult == 6 && !isStealAuto) || (wheelResult == 7 && !isAttackRandom) || (isFullShields && shields == 3))
                {
                    Data info = new Data();
                    if (wheelResult == 6 || wheelResult == 7)
                    {
                        info.Msg = String.Empty;
                    }
                    else
                    {
                        info.Msg = playerInfo + "\r\n" + cashKingInfo;
                    }
                    if (wheelResult == 6)
                    {
                        info.Rank = levelIsland;
                        info.IsOK = isOK;
                    }
                    M_PLAY.ReportProgress(100, info);
                    return;
                }
                else
                {
                    Data info = new Data();
                    if (wheelResult == 6 || wheelResult == 7)
                    {
                        info.Msg = String.Empty;
                    }
                    else
                    {
                        info.Msg = playerInfo + "\r\n" + cashKingInfo + "\r\n" + "WheelResult: " + wheelResult;
                    }
                    M_PLAY.ReportProgress(50, info);
                }

                if (M_PLAY.CancellationPending)
                {
                    e.Cancel = true;
                    M_PLAY.ReportProgress(0);
                    return;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Play_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState == null) return;
                Data data = (Data) e.UserState;
                if (data.Msg != String.Empty)
                {
                    displayInfo("PLAY", data.Msg);
                }
                if (data.Rank > 0)
                {
                    if (data.IsOK)
                    {
                        avatar.Visible = true;
                    }
                    else
                    {
                        avatarPre.Visible = true;
                        avatar.Visible = true;
                        avatarNext.Visible = true;
                    }
                    displayAvatar(data.Rank);
                }
                else
                {
                    avatarPre.Visible = false;
                    avatar.Visible = false;
                    avatarNext.Visible = false;
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Play_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled.", "PKTool", MessageBoxButtons.OK);
                btnPlay.Text = "Play all";
                return;
            }
            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                RETRYCOUNT = RETRYCOUNT + 1;
                if (RETRYCOUNT == 2)
                {
                    MessageBox.Show("Error.", "PKTool", MessageBoxButtons.OK);
                }
                else
                {
                    refresh();
                    btnPlay.Text = "Play all";
                    btnPlay_Click(null, null);
                    return;
                }
            }
            else
            {
                btnPlay.Text = "Play all";
            }
        }
        #endregion

        #region "EVENTS ON CONTROLS"
        private void chkAutoAttack_CheckedChanged(object sender, EventArgs e)
        {
            rdoRandom.Enabled = chkAutoAttack.Checked;
            rdoFriend.Enabled = chkAutoAttack.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit PKTool?", "PKTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void saveFriendsToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FRIENDS == null || FRIENDS.Count == 0) return;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName == String.Empty) return;
                    String pkXML = serialize(FRIENDS).ToString();
                    File.WriteAllText(saveFileDialog.FileName, pkXML);
                }
            }
            catch
            {

            }
        }

        private void addFriendsFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialogF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialogF.FileName == String.Empty) return;
                    String pkXML = MyFile.ReadFile(openFileDialogF.FileName);
                    List<Friend> friendsAdd = deserialize<List<Friend>>(pkXML);
                    Int32 idxSelect = FRIENDS.Count - 1;
                    int count = 0;
                    foreach (Friend att in friendsAdd)
                    {
                        var lstGet = (from i in FRIENDS
                                      where i.Id == att.Id
                                      select i).ToList();
                        if (lstGet.Count == 0)
                        {
                            if (ISVIP)
                            {
                                if (att.Id != FBID)
                                {
                                    count = count + 1;
                                    //
                                    att.Index = FRIENDS.Count + 1;
                                    //Reset value
                                    att.SToken = String.Empty;
                                    att.BToken = String.Empty;
                                    //
                                    FRIENDS.Add(att);
                                    FRIENDFBIDS.Add(att.Id);
                                }
                            }
                            else
                            {
                                count = count + 1;
                                //
                                att.Index = FRIENDS.Count + 1;
                                //Reset value
                                att.SToken = String.Empty;
                                att.BToken = String.Empty;
                                //
                                FRIENDS.Add(att);
                                FRIENDFBIDS.Add(att.Id);
                            }
                        }
                    }
                    ((CurrencyManager)lbFriends.BindingContext[FRIENDS]).Refresh();
                    if (count > 0)
                    {
                        MessageBox.Show(String.Format("Added {0} friends", count));
                    }
                    lbFriends.SelectedIndex = idxSelect;
                }
            }
            catch
            {

            }
        }

        private void lbFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFriends.SelectedItem == null) return;
            Friend friend = (Friend)lbFriends.SelectedItem;
            txtID.Text = friend.Id;
        }

        private void btnNews_Click(object sender, EventArgs e)
        {
            if (SECRETKEY == String.Empty) return;
            frmNews frmNews = new frmNews(LISTNEWS);
            frmNews.ShowDialog();
        }
        private void btnViewFB_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbFriends.SelectedItem == null) return;
                Friend friend = (Friend)lbFriends.SelectedItem;
                String urlFB = String.Empty;
                if (friend.Id.StartsWith("100000"))
                {
                    urlFB = String.Format("https://www.facebook.com/profile.php?id={0}", friend.Id);
                }
                else
                {
                    urlFB = String.Format("https://www.facebook.com/app_scoped_user_id/{0}", friend.Id);
                }
                openURL(urlFB);
            }
            catch
            {

            }
        }

        private void btnSetVicTim_Click(object sender, EventArgs e)
        {
            if (lbFriends.SelectedItem == null) return;
            VICTIM = (Friend)lbFriends.SelectedItem;
            txtVictim.Text = ((Friend)lbFriends.SelectedItem).Name;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbRet.Text = String.Empty;
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbFriends.SelectedItem == null) return;
                //
                Friend friend = (Friend)lbFriends.SelectedItem;
                String retView = islandViewFriend(friend.Id);
                JToken jRetView = JObject.Parse(retView);
                displayInfo("INFO: " + friend.Name, "Rank: " + jRetView["PlayerRankPoints"].ToString() + "\r\n" + "Island:" + "\r\n" + jRetView["WantedIsland"].ToString());
            }
            catch
            {
            }
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            avatarPre.Visible = false;
            avatar.Visible = false;
            avatarNext.Visible = false;
            if (SECRETKEY == String.Empty) return;
            if (chkAutoAttack.Checked && rdoFriend.Checked && VICTIM == null)
            {
                MessageBox.Show("Plz set victim..", "PKTool");
                return;
            }
            if (chkAutoAttack.Checked && rdoFriend.Checked)
            {
                List<Item> itemTypes = getItemTypeAttack(VICTIM.Id);
                if (itemTypes.Count == 0)
                {
                    MessageBox.Show("Victim's island no items. Plz set other victim to next step..", "PKTool");
                    return;
                }
            }
            String txtBtn = btnPlay.Text;
            switch (txtBtn.ToUpper())
            {
                case "PLAY ALL":
                    btnPlay.Text = "Cancel";
                    int typeAttack = 1;
                    if (rdoRandom.Checked) typeAttack = 1;
                    if (rdoFriend.Checked) typeAttack = 2;
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("isAttackRandom", chkAutoAttack.Checked);
                    dic.Add("typeAttack", typeAttack);
                    dic.Add("num", nudNum.Value);
                    dic.Add("victim", VICTIM);
                    dic.Add("isStealAuto", chkAutoSteal.Checked);
                    dic.Add("isFullShields", chkFull.Checked);
                    dic.Add("isAutoUpgrade", chkAutoUpgrade.Checked);
                    M_PLAY.RunWorkerAsync(dic);
                    break;
                case "CANCEL":
                    M_PLAY.CancelAsync();
                    break;
                default:
                    break;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //Re login to get cashking
                if (SECRETKEY == String.Empty) return;
                JToken jTokenResp = refresh();
                displayInfo("REFRESH", "PlayerState:" + "\r\n" + jTokenResp["PlayerState"] + "\r\n" + "Island:" + "\r\n" + jTokenResp["Island"]);
            }
            catch
            {

            }
        }
        
        private void btnVideo_Click(object sender, EventArgs e)
        {
            try
            {
                if (SECRETKEY == String.Empty) return;
                String ret = videoClaimbonus();
                JToken jToken = JObject.Parse(ret);
                displayInfo("VIDEO CLAIM BONUS", jToken.ToString());
            }
            catch
            {

            }
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            try
            {
                if (SECRETKEY == String.Empty) return;
                String ret = inviteComplete();
                JToken token = JObject.Parse(ret);
                JToken jToken = JObject.Parse(ret);
                displayInfo("INVITE COMPLETE", jToken.ToString());
            }
            catch
            {

            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (SECRETKEY == String.Empty) return;
                String ret = openChest();
                JToken jToken = JObject.Parse(ret);
                displayInfo("OPEN CHEST", jToken.ToString());
            }
            catch
            {

            }
        }

        private void btnSpinSlot_Click(object sender, EventArgs e)
        {
            try
            {
                if (SECRETKEY == String.Empty) return;
                String ret = spinSlot();
                JToken jToken = JObject.Parse(ret);
                displayInfo("SPIN SLOT", jToken.ToString());
            }
            catch
            {

            }
        }
        #endregion

        #region "EVENT ON FORM"
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                shutdowFiddlerApp();
            }
            catch
            { 
            
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region "METHOD"
        /// <summary>
        /// Set login
        /// </summary>
        /// <param name="req"></param>
        /// <param name="resp"></param>
        private void setLogin(String req, String resp)
        {
            try
            {
                JToken jTokenReq = JObject.Parse(req);
                JToken jTokenResp = JObject.Parse(resp);
                UDID = jTokenReq["UDID"].ToString();
                FRIENDFBIDS = JsonConvert.DeserializeObject<List<String>>(jTokenReq["FriendFBIDs"].ToString());
                if (jTokenResp["News"] != null)
                {
                    List<NewsItem> listNews = JsonConvert.DeserializeObject<List<NewsItem>>(jTokenResp["News"].ToString());
                    if (listNews != null)
                    {
                        LISTNEWS = (from news in listNews
                                    where news.Type == NewsType.Attack || news.Type == NewsType.Steal
                                    select news).ToList();
                    }
                }
                setIslandIndex(jTokenResp);
                getFriends(jTokenResp);
                ITEMS = getItems(jTokenResp);
                BASEPRICES = JsonConvert.DeserializeObject<List<Int32>>(jTokenResp["IslandShopPrice"]["BasePrices"].ToString());
                PRICESTEPS = JsonConvert.DeserializeObject<List<double>>(jTokenResp["IslandShopPrice"]["PriceSteps"].ToString());
                setPrices(ITEMS);
                BUSINESSTOKEN = jTokenReq["BusinessToken"].ToString();
                ACCESSTOKEN = jTokenReq["AccessToken"].ToString();
                FBID = jTokenReq["FBID"].ToString();
                SECRETKEY = jTokenResp["Key"].ToString();
                SESSIONTOKEN = jTokenResp["SessionToken"].ToString();
                NAME = jTokenResp["PlayerMetaData"]["Name"].ToString();
                REQBODY = req;
                lblName.Text = NAME;
            }
            catch
            {
                lblName.Text = "Please login to next...";
            }
        }
        /// <summary>
        /// s
        /// </summary>
        /// <returns></returns>
        private JToken refresh()
        {
            try
            {
                String urlLogin = String.Format(URLLOGIN, DateTime.Now.ToOADate().ToString());
                String retLogin = doPost(urlLogin, REQBODY);
                JToken jTokenResp = JObject.Parse(retLogin);
                //
                if (jTokenResp["News"] != null)
                {
                    List<NewsItem> listNews = JsonConvert.DeserializeObject<List<NewsItem>>(jTokenResp["News"].ToString());
                    if (listNews != null)
                    {
                        LISTNEWS = (from news in listNews
                                    where news.Type == NewsType.Attack || news.Type == NewsType.Steal
                                    select news).ToList();
                    }
                }
                setIslandIndex(jTokenResp);
                getFriends(jTokenResp);
                ITEMS = getItems(jTokenResp);
                BASEPRICES = JsonConvert.DeserializeObject<List<Int32>>(jTokenResp["IslandShopPrice"]["BasePrices"].ToString());
                PRICESTEPS = JsonConvert.DeserializeObject<List<double>>(jTokenResp["IslandShopPrice"]["PriceSteps"].ToString());
                setPrices(ITEMS);
                SECRETKEY = jTokenResp["Key"].ToString();
                SESSIONTOKEN = jTokenResp["SessionToken"].ToString();
                return jTokenResp;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static StringWriter serialize(object o)
        {
            var xs = new XmlSerializer(o.GetType());
            var xml = new StringWriter();
            xs.Serialize(xml, o);
            return xml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T deserialize<T>(string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(new StringReader(xml));
        }
        /// <summary>
        /// Open url
        /// </summary>
        /// <param name="url"></param>
        private void openURL(String url)
        {
            try
            {
                if (url == String.Empty) return;
                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private List<Friend> getFriends(String html)
        {
            List<Friend> lst = new List<Friend>();
            try
            {
                if (html != String.Empty)
                {
                    var doc = new HAP.HtmlDocument();
                    doc.LoadHtml(html);
                    var root = doc.DocumentNode;
                    var row_nodes = root.Descendants()
                                    .Where(n => n.Name == "a")
                                    .Where(n => n.GetAttributeValue("class", null) == "_5q6s _8o _8t lfloat _ohe");
                    foreach (var a_node in row_nodes)
                    {
                        Friend friend = new Friend();
                        friend.Id = REGEX_ID.Match(a_node.GetAttributeValue("data-hovercard", "")).Groups["fid"].Value;
                        friend.Name = REGEX_NAME.Match(a_node.GetAttributeValue("href", "")).Groups["fname"].Value;
                        lst.Add(friend);
                    }
                }
            }
            catch
            {
            }
            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string doPost(string uri, string parameters)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                System.Net.ServicePointManager.Expect100Continue = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                request.ContentLength = bytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                var result = reader.ReadToEnd();
                stream.Dispose();
                reader.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void getFriends(JToken data)
        {
            //
            Int32 index = 1;
            foreach (JToken child in data["FriendScores"])
            {
                String name = child["Name"] != null ? child["Name"].ToString() : "";
                if (name != "")
                {
                    String fbid = child["FBID"] != null ? child["FBID"].ToString() : "";
                    //Check again
                    if (ISVIP)
                    {
                        if (fbid.ToUpper() != FBID.ToUpper())
                        {
                            Friend friend = new Friend();
                            friend.Name = name;
                            friend.Id = child["FBID"] != null ? child["FBID"].ToString() : "";
                            friend.Index = index;
                            friend.Rank = Convert.ToInt32(child["RankPoints"]);
                            index = index + 1;
                            FRIENDS.Add(friend);
                        }
                    }
                    else
                    {
                        Friend friend = new Friend();
                        friend.Name = name;
                        friend.Id = child["FBID"] != null ? child["FBID"].ToString() : "";
                        friend.Index = index;
                        friend.Rank = Convert.ToInt32(child["RankPoints"]);
                        index = index + 1;
                        FRIENDS.Add(friend);
                    }
                }
            }
            //
            lbFriends.DataSource = FRIENDS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        private String islandViewFriend(string fid)
        {
            String ret = String.Empty;
            String url = URLVIEW + DateTime.Now.ToOADate().ToString();
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("FriendScopedId", fid);
            dicResult.Add("secretKey", SECRETKEY);
            dicResult.Add("sessionToken", SESSIONTOKEN);
            dicResult.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dicResult));
            return ret;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="fbidVictim"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private string attackFriend(String fbidVictim, String itemType)
        {
            String ret = String.Empty;
            String url = URLATTACKFRIEND + DateTime.Now.ToOADate().ToString();
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("FriendScopedId", fbidVictim);
            dicResult.Add("ItemType", itemType);
            dicResult.Add("secretKey", SECRETKEY);
            dicResult.Add("sessionToken", SESSIONTOKEN);
            dicResult.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dicResult));
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="fbidVictim"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private string attackFriend(Friend attacker, String fbidVictim, String itemType)
        {
            String ret = String.Empty;
            String url = URLATTACKFRIEND + DateTime.Now.ToOADate().ToString();
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("FriendScopedId", fbidVictim);
            dicResult.Add("ItemType", itemType);
            dicResult.Add("secretKey", attacker.Key);
            dicResult.Add("sessionToken", attacker.SToken);
            dicResult.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dicResult));
            return ret;
        }

        /// <summary>
        /// Get item type acttack
        /// </summary>
        /// <param name="fbidVictim"></param>
        /// <returns></returns>
        private List<Item> getItemTypeAttack(String fbidVictim)
        {
            List<Item> itemTypes = new List<Item>();
            String retView = islandViewFriend(fbidVictim);
            if (retView == String.Empty) return itemTypes;
            JToken data = JObject.Parse(retView);
            //
            Item i = new Item();
            i.Name = "Animals";
            i.Isdamaged = Convert.ToBoolean(data["WantedIsland"]["Animal"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["WantedIsland"]["Animal"]["Level"]);
            itemTypes.Add(i);

            i = new Item();
            i.Name = "Nature";
            i.Isdamaged = Convert.ToBoolean(data["WantedIsland"]["Nature"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["WantedIsland"]["Nature"]["Level"]);
            itemTypes.Add(i);


            i = new Item();
            i.Name = "Building";
            i.Isdamaged = Convert.ToBoolean(data["WantedIsland"]["Building"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["WantedIsland"]["Building"]["Level"]);
            itemTypes.Add(i);

            i = new Item();
            i.Name = "Ships";
            i.Isdamaged = Convert.ToBoolean(data["WantedIsland"]["Ship"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["WantedIsland"]["Ship"]["Level"]);
            itemTypes.Add(i);

            i = new Item();
            i.Name = "Artifacts";
            i.Isdamaged = Convert.ToBoolean(data["WantedIsland"]["Artifact"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["WantedIsland"]["Artifact"]["Level"]);
            itemTypes.Add(i);

            var query = from item in itemTypes
                        orderby item.Level descending, item.Isdamaged descending
                        where item.Level > 0
                        select item;

            return query.ToList();
        }
        /// <summary>
        /// doGet
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private String doGet(string url)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                string data = reader.ReadToEnd();

                reader.Close();
                stream.Close();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="isStealAuto"></param>
        /// <returns></returns>
        private string steal(String secretKey, String sessionToken, JToken data, Boolean isStealAuto, out String retSteal, out Int32 levelIsland, out Boolean isOK)
        {
            levelIsland = 1;
            isOK = false;
            retSteal = String.Empty;
            String ret = String.Empty;
            int sealIndex = 0;
            //
            Int32 rank = Convert.ToInt32(data["PlayerState"]["CashKing"]["RankPoints"]);
            int index = 0;
            List<Key> lstKeys = new List<Key>();
            foreach (JToken child in data["StealIslands"])
            {
                Key keyItem = new Key();
                keyItem.Index = index;
                keyItem.Level = Convert.ToInt32(child["Level"]);
                keyItem.Data = child.ToString();
                lstKeys.Add(keyItem);
                index = index + 1;
            }
            //Get steal index
            if (data["PlayerState"]["CashKing"]["FBID"] != null)
            {
                String fbId = data["PlayerState"]["CashKing"]["FBID"].ToString();
                String retView = islandViewFriend(fbId);
                JToken jRetView = JObject.Parse(retView);
                Int32 levelView = Convert.ToInt32(jRetView["WantedIsland"]["Level"]);
                levelIsland = levelView;
                //
                var lstKeysFind = (from item in lstKeys
                                    where item.Level == levelView
                                    select item).ToList();
                if (lstKeysFind.Count > 0)
                {
                    //Level of island equals with rank island view
                    Int32 levelBest = lstKeysFind[0].Level;
                    Random rnd = new Random();
                    int idx = rnd.Next(lstKeysFind.Count); //0,count-1
                    sealIndex = lstKeysFind[idx].Index;
                    isOK = true;
                }
                else
                {
                    //Other
                    List<Key> lstKeysFindEx = new List<Key>();
                    if (rank >= RANKPOINT_STEAL)
                    {
                        //descending
                        lstKeysFindEx = (from item in lstKeys
                                        orderby item.Level descending
                                        select item).ToList();
                    }
                    else
                    {
                        //ascending
                        lstKeysFindEx = (from item in lstKeys
                                        orderby item.Level ascending
                                        select item).ToList();
                    }
                    sealIndex = lstKeysFindEx[0].Index;
                    isOK = false;
                }
            }
            else
            {
                List<Key> lstKeysFindEx = new List<Key>();
                if (rank >= RANKPOINT_STEAL)
                {
                    //descending
                    lstKeysFindEx = (from item in lstKeys
                                     orderby item.Level descending
                                     select item).ToList();
                }
                else
                {
                    //ascending
                    lstKeysFindEx = (from item in lstKeys
                                     orderby item.Level ascending
                                     select item).ToList();
                }
                sealIndex = lstKeysFindEx[0].Index;
                isOK = false;
            }
            //
            if (isStealAuto)
            {
                try
                {
                    retSteal = steal(sealIndex, secretKey, sessionToken);
                }
                catch
                {

                }
            }
            return String.Format("Steal: {0} No-Level: 1-{1} 2-{2} 3-{3}", sealIndex + 1, lstKeys[0].Level, lstKeys[1].Level, lstKeys[2].Level); ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Item> getItemTypeAttack(JToken data, bool isSort)
        {
            List<Item> itemTypes = new List<Item>();
            Item i = new Item();
            i.Name = "Animals";
            i.Isdamaged = Convert.ToBoolean(data["RandomAttackIsland"]["Animal"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["RandomAttackIsland"]["Animal"]["Level"]);
            itemTypes.Add(i);

            i = new Item();
            i.Name = "Nature";
            i.Isdamaged = Convert.ToBoolean(data["RandomAttackIsland"]["Nature"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["RandomAttackIsland"]["Nature"]["Level"]);
            itemTypes.Add(i);


            i = new Item();
            i.Name = "Building";
            i.Isdamaged = Convert.ToBoolean(data["RandomAttackIsland"]["Building"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["RandomAttackIsland"]["Building"]["Level"]);
            itemTypes.Add(i);

            i = new Item();
            i.Name = "Ships";
            i.Isdamaged = Convert.ToBoolean(data["RandomAttackIsland"]["Ship"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["RandomAttackIsland"]["Ship"]["Level"]);
            itemTypes.Add(i);

            i = new Item();
            i.Name = "Artifacts";
            i.Isdamaged = Convert.ToBoolean(data["RandomAttackIsland"]["Artifact"]["IsDamaged"]);
            i.Level = Convert.ToInt16(data["RandomAttackIsland"]["Artifact"]["Level"]);
            itemTypes.Add(i);

            if (isSort)
            {
                var query = from item in itemTypes
                            orderby item.Level descending, item.Isdamaged descending
                            where item.Level > 0
                            select item;
                return query.ToList();
            }
            else
            {
                return itemTypes;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string attackRandom(String secretKey, String sessionToken, JToken data)
        {
            List<Item> itemTypes = getItemTypeAttack(data, true);
            String itemAttackName = "";
            if (itemTypes.Count > 0)
            {
                itemAttackName = itemTypes[0].Name;
            }
            else
            {
                var rnd = new Random(DateTime.Now.Millisecond);
                int idxItem = rnd.Next(0, 4);
                itemAttackName = ARRITEMS[idxItem];
            }

            return attackRandom(itemAttackName, secretKey, sessionToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private string getTimes(Int32 time)
        {
            //00:00
            int min = (int)time / 60;
            int second = time - min * 60;
            return String.Format("{0}:{1}", min.ToString("00"), second.ToString("00"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fbID"></param>
        /// <returns></returns>
        private Friend getFriendByFBIDEx(String fbID, out JToken jRetView)
        {
            jRetView = null;
            if (fbID.Trim() == String.Empty) return null;
            if (ISVIP && fbID.Trim() == FBID) return null;
            try
            {
                String retView = islandViewFriend(fbID.Trim());
                jRetView = JObject.Parse(retView);
                if (jRetView["WantedIsland"] != null)
                {
                    Friend friend = new Friend();
                    friend.Id = fbID;
                    friend.Name = fbID;
                    friend.Type = 2;
                    return friend;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Display logs info
        /// </summary>
        /// <param name="header"></param>
        /// <param name="content"></param>
        private void displayInfo(String header, String content)
        {
            if (content == String.Empty) return;
            String data = String.Empty;
            if (header == String.Empty)
            {
                data = content + "\r\n" + SEPARATOR + "\r\n\r\n";
            }
            else
            {
                data = header + "\r\n" + content + "\r\n" + SEPARATOR + "\r\n\r\n";
            }
            rtbRet.Text = rtbRet.Text.Insert(0, data);
        }
        /// <summary>
        /// Display img for steal
        /// </summary>
        private void displayAvatar(Int32 level)
        {
            try
            {
                int baseName = level;
                int baseNamePre = baseName > 1 ? baseName - 1 : baseName;
                int baseNameNext = baseName < 23 ? baseName + 1 : baseName;
                avatarPre.Image = imageList.Images[String.Format("{0}.png", baseNamePre)];
                avatar.Image = imageList.Images[String.Format("{0}.png", level)];
                avatarNext.Image = imageList.Images[String.Format("{0}.png", baseNameNext)];
            }
            catch
            {
            }
        }
        /// <summary>
        /// Damaged, Level
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        private List<Item> getItems(JToken jToken)
        {
            List<Item> items = new List<Item>();
            //0
            Item item = new Item();
            item.Index = 0;
            item.Name = "Artifacts";
            item.Level = Convert.ToInt32(jToken["Island"]["Artifact"]["Level"]);
            item.Isdamaged = Convert.ToBoolean(jToken["Island"]["Artifact"]["IsDamaged"]);
            items.Add(item);
            //1
            item = new Item();
            item.Index = 1;
            item.Name = "Nature";
            item.Level = Convert.ToInt32(jToken["Island"]["Nature"]["Level"]);
            item.Isdamaged = Convert.ToBoolean(jToken["Island"]["Nature"]["IsDamaged"]);
            items.Add(item);
            //2
            item = new Item();
            item.Index = 2;
            item.Name = "Ships";
            item.Level = Convert.ToInt32(jToken["Island"]["Ship"]["Level"]);
            item.Isdamaged = Convert.ToBoolean(jToken["Island"]["Ship"]["IsDamaged"]);
            items.Add(item);
            //3
            item = new Item();
            item.Index = 3;
            item.Name = "Building";
            item.Level = Convert.ToInt32(jToken["Island"]["Building"]["Level"]);
            item.Isdamaged = Convert.ToBoolean(jToken["Island"]["Building"]["IsDamaged"]);
            items.Add(item);
            //4
            item = new Item();
            item.Index = 3;
            item.Name = "Animals";
            item.Level = Convert.ToInt32(jToken["Island"]["Animal"]["Level"]);
            item.Isdamaged = Convert.ToBoolean(jToken["Island"]["Animal"]["IsDamaged"]);
            items.Add(item);

            return items;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="basePrices"></param>
        /// <param name="priceSteps"></param>
        private void setPrices(List<Item> items)
        {
            foreach (Item item in items)
            {
                Int32 basePrice = BASEPRICES[item.Index];
                double priceStep = PRICESTEPS[item.Index];
                Int32 price = Convert.ToInt32(basePrice * (1 + item.Level * priceStep));
                item.Price = price;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private String upgrade(String itemType)
        {
            String ret = String.Empty;
            String url = String.Format(URLUPGRADE, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ItemType", itemType);
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);            
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private String repair(String itemType)
        {
            String ret = String.Empty;
            String url = String.Format(URLREPAIR, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ItemType", itemType);
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String cheat()
        {
            String ret = String.Empty;
            String url = String.Format(URLCHEAT, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String finish()
        {
            String ret = String.Empty;
            String url = String.Format(URLFINISH, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("FriendFBIDs", JsonConvert.SerializeObject(FRIENDFBIDS));
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// Check finish 
        /// </summary>
        /// <returns></returns>
        private bool isFinish()
        {
            var items = (from i in ITEMS
                         orderby i.Level ascending
                         where i.Isdamaged == false && i.Level == 5
                         select i).ToList();
            return items.Count == 5;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        private bool isNextUpgrade(Int64 cash)
        {
            var itemsDamaged = (from i in ITEMS
                                orderby i.Level ascending
                                where i.Isdamaged == true
                                select i).ToList();
            if (itemsDamaged.Count > 0 && cash >= itemsDamaged[0].Price)
            {
                return true;
            }
            //upgrade
            var itemsUpgrade = (from i in ITEMS
                                orderby i.Price ascending
                                where i.Level < 5
                                select i).ToList();
            if (itemsUpgrade.Count > 0 && cash >= itemsUpgrade[0].Price)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private String wheel(String secretKey, String sessionToken)
        {
            String ret = String.Empty;
            String url = String.Format(URLWHEEL, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ExpectedResult", "NONE");
            dic.Add("secretKey", secretKey);
            dic.Add("sessionToken", sessionToken);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private String attackRandom(String itemType, String secretKey, String sessionToken)
        {
            String ret = String.Empty;
            String url = String.Format(URLATTACKRANDOM, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ItemType", itemType);
            dic.Add("secretKey", secretKey);
            dic.Add("sessionToken", sessionToken);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stealIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private String steal(int stealIndex, String secretKey, String sessionToken)
        {
            String ret = String.Empty;
            String url = String.Format(URLSTEAL, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StealIndex", stealIndex);
            if (secretKey == SECRETKEY)
            {
                dic.Add("FriendFBIDs", JsonConvert.SerializeObject(FRIENDFBIDS));
            }
            else
            {
                dic.Add("FriendFBIDs", JsonConvert.SerializeObject(new List<String> { }));
            }
            dic.Add("secretKey", secretKey);
            dic.Add("sessionToken", sessionToken);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private String getItemName(String itemType)
        {
            //"Animals", "Nature", "Building", "Ships", "Artifacts"
            switch (itemType)
            {
                case "Animals":
                    return "Animal";
                case "Nature":
                    return "Nature";
                case "Building":
                    return "Building";
                case "Ships":
                    return "Ship";
                case "Artifacts":
                    return "Artifact";
                default:
                    return String.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String inviteComplete()
        {
            String ret = String.Empty;
            String url = String.Format(URLINVITEBONUS, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String videoClaimbonus()
        {
            String ret = String.Empty;
            String url = String.Format(URLVIDEOCLAIMBONUS, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="islandIndex"></param>
        /// <returns></returns>
        private String completedUpgrade(Int32 islandIndex)
        {
            String ret = String.Empty;
            String url = String.Format(URLCOMPLETEDUPGRADE, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            dic.Add("islandIndex", islandIndex);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String spinSlot()
        {
            String ret = String.Empty;
            String url = String.Format(URLSPINSLOT, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String openChest()
        {
            String ret = String.Empty;
            String url = String.Format(URLSLOTCHEST, DateTime.Now.ToOADate().ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("secretKey", SECRETKEY);
            dic.Add("sessionToken", SESSIONTOKEN);
            dic.Add("businessToken", BUSINESSTOKEN);
            ret = doPost(url, JsonConvert.SerializeObject(dic));
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void setIslandIndex(JToken data) {
            if (data["IslandCompletions"] != null)
            {
                List<IslandCompletions> completedIsland = JsonConvert.DeserializeObject<List<IslandCompletions>>(data["IslandCompletions"].ToString());
                if (completedIsland != null)
                {
                    var datas = (from i in completedIsland
                                 orderby i.IslandLevel descending
                                 select i).ToList();
                    ISLANDINDEX = datas[0].IslandLevel;
                }
            }
        }
        #endregion

        #region "FIDDLERAPP"
        /// <summary>
        /// 
        /// </summary>
        void startFiddlerApp()
        {
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
            FiddlerApplication.Startup(8080, false, true, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sess"></param>
        private void FiddlerApplication_AfterSessionComplete(Session sess)
        {
            // Ignore HTTPS connect requests
            if (sess.RequestMethod == "CONNECT")
                return;

            if (CaptureConfiguration.ProcessId > 0)
            {
                if (sess.LocalProcessID != 0 && sess.LocalProcessID != CaptureConfiguration.ProcessId)
                    return;
            }

            if (!string.IsNullOrEmpty(CaptureConfiguration.CaptureDomain))
            {
                //if (sess.hostname.ToLower() != CaptureConfiguration.CaptureDomain.Trim().ToLower())
                //    return;
                if (!sess.fullUrl.ToLower().Contains(CaptureConfiguration.CaptureDomain.Trim().ToLower()))
                    return;
            }

            if (CaptureConfiguration.IgnoreResources)
            {
                string url = sess.fullUrl.ToLower();

                var extensions = CaptureConfiguration.ExtensionFilterExclusions;
                foreach (var ext in extensions)
                {
                    if (url.Contains(ext))
                        return;
                }

                var filters = CaptureConfiguration.UrlFilterExclusions;
                foreach (var urlFilter in filters)
                {
                    if (url.Contains(urlFilter))
                        return;
                }
            }

            if (sess == null || sess.oRequest == null || sess.oRequest.headers == null)
                return;
            var reqBody = Encoding.UTF8.GetString(sess.RequestBody);
            var respBody = Encoding.UTF8.GetString(sess.ResponseBody);
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("reqBody", reqBody);
            dicResult.Add("respBody", respBody);
            // must marshal to UI thread
            BeginInvoke(new Action<Dictionary<string, object>>((dic) =>
            {
                try
                {
                    shutdowFiddlerApp();
                    REQBODY = reqBody;
                    JToken jTokenReq = JObject.Parse(dic["reqBody"].ToString());
                    JToken jTokenResp = JObject.Parse(dic["respBody"].ToString());
                    UDID = jTokenReq["UDID"].ToString();
                    FRIENDFBIDS = JsonConvert.DeserializeObject<List<String>>(jTokenReq["FriendFBIDs"].ToString());
                    if (jTokenResp["News"] != null)
                    {
                        List<NewsItem> listNews = JsonConvert.DeserializeObject<List<NewsItem>>(jTokenResp["News"].ToString());
                        if (listNews != null)
                        {
                            LISTNEWS = (from news in listNews
                                        where news.Type == NewsType.Attack || news.Type == NewsType.Steal
                                        select news).ToList();
                        }
                    }
                    setIslandIndex(jTokenResp);
                    getFriends(jTokenResp);
                    ITEMS = getItems(jTokenResp);
                    BASEPRICES = JsonConvert.DeserializeObject<List<Int32>>(jTokenResp["IslandShopPrice"]["BasePrices"].ToString());
                    PRICESTEPS = JsonConvert.DeserializeObject<List<double>>(jTokenResp["IslandShopPrice"]["PriceSteps"].ToString());
                    setPrices(ITEMS);
                    BUSINESSTOKEN = jTokenReq["BusinessToken"].ToString();
                    ACCESSTOKEN = jTokenReq["AccessToken"].ToString();
                    FBID = jTokenReq["FBID"].ToString();
                    SECRETKEY = jTokenResp["Key"].ToString();
                    SESSIONTOKEN = jTokenResp["SessionToken"].ToString();
                    NAME = jTokenResp["PlayerMetaData"]["Name"].ToString();
                    lblName.Text = NAME;
                }
                catch
                {
                    lblName.Text = "Please login to next...";
                }
            }), dicResult);
        }
        /// <summary>
        /// 
        /// </summary>
        private void shutdowFiddlerApp()
        {
            FiddlerApplication.AfterSessionComplete -= FiddlerApplication_AfterSessionComplete;
            if (FiddlerApplication.IsStarted()) FiddlerApplication.Shutdown();
        }
        #endregion

        private void toolStripMenuItemCharles_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripMenuItemProxy.Checked = false;
                shutdowFiddlerApp();
                toolStripMenuItemCharles.Checked = true;
                frmLogin frmLogin = new frmLogin();
                if (frmLogin.ShowDialog() == DialogResult.OK)
                {
                    String req = frmLogin.Req;
                    String resp = frmLogin.Resp;
                    if (req == String.Empty || resp == String.Empty) return;
                    setLogin(req, resp);
                }
            }
            catch
            {
                MessageBox.Show("Failled login. Please try again.", "PKTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemProxy_Click(object sender, EventArgs e)
        {
            toolStripMenuItemProxy.Checked = true;
            shutdowFiddlerApp();
            startFiddlerApp();
            MessageBox.Show("Config proxy 127.0.0.1 port 8080. Plz, login on web to next...!", "PKTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            toolStripMenuItemCharles.Checked = false;
        }

        
    }
}
