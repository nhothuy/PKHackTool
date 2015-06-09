using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private List<Friend> FRIENDS = new List<Friend>();
        private UrlCaptureConfiguration CaptureConfiguration { get; set; }
        private const string SEPARATOR = "-------------------------------------------------------------------------------";
        private const string CAPTUREDOMAIN = "http://prod.cashkinggame.com/CKService.svc/v3.0/login";
        BackgroundWorker M_OWORKER;
        BackgroundWorker M_KILL;
        BackgroundWorker M_PLAY;
        BackgroundWorker M_ATTACK;
        private const Int32 RANKPOINT_STEAL = 200;
        private List<Int32> IDATTACKER = new List<int>();
        private Friend VICTIM = null;
        private List<String> NAMEVIPS = new List<string>(new String[] {"THUY NHO", "NHOKHOA"});
        private List<String> FIDVIPS = new List<string>(new String[] { "10153223750579791", "917613761592912" });
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
        private List<Int32> BASEPRICES = new List<Int32>();
        private List<double> PRICESTEPS = new List<double>();
        private List<Item> ITEMS = new List<Item>();
        private List<String> FRIENDFBIDS = new List<string>();
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
            M_OWORKER = new BackgroundWorker();
            M_OWORKER.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            M_OWORKER.ProgressChanged += new ProgressChangedEventHandler(m_oWorker_ProgressChanged);
            M_OWORKER.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);
            M_OWORKER.WorkerReportsProgress = true;
            M_OWORKER.WorkerSupportsCancellation = true;
            //
            M_ATTACK = new BackgroundWorker();
            M_ATTACK.DoWork += new DoWorkEventHandler(m_Attack_DoWork);
            M_ATTACK.ProgressChanged += new ProgressChangedEventHandler(m_Attack_ProgressChanged);
            M_ATTACK.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_Attack_RunWorkerCompleted);
            M_ATTACK.WorkerReportsProgress = true;
            M_ATTACK.WorkerSupportsCancellation = true;
            //
            M_KILL = new BackgroundWorker();
            M_KILL.DoWork += new DoWorkEventHandler(m_Kill_DoWork);
            M_KILL.ProgressChanged += new ProgressChangedEventHandler(m_Kill_ProgressChanged);
            M_KILL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_Kill_RunWorkerCompleted);
            M_KILL.WorkerReportsProgress = true;
            M_KILL.WorkerSupportsCancellation = true;
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
            Boolean isStealAuto = Convert.ToBoolean(dic["isStealAuto"]);
            Boolean isFullShields = Convert.ToBoolean(dic["isFullShields"]);
            Boolean isAutoUpgrade = Convert.ToBoolean(dic["isAutoUpgrade"]);
            while (true)
            {
                String retWheel = wheel(SECRETKEY, SESSIONTOKEN);
                JToken data = JObject.Parse(retWheel);
                int wheelResult = Convert.ToInt16(data["WheelResult"]);
                int spins = Convert.ToInt16(data["PlayerState"]["Spins"]);
                int shields = Convert.ToInt16(data["PlayerState"]["Shields"]);
                String playerInfo = String.Format("Rank:{0} Shields:{1} Spins:{2} Cash:{3} NextSpin: {4}", data["PlayerState"]["RankPoints"], data["PlayerState"]["Shields"], data["PlayerState"]["Spins"], Convert.ToInt64(data["PlayerState"]["Cash"]).ToString("#,#", CultureInfo.InvariantCulture), getTimes(Convert.ToInt32(data["NextSpinClaimSeconds"])));
                String cashKingInfo = String.Format("Name:{1} Rank:{2} Cash:{3}", data["PlayerState"]["CashKing"]["FBID"], data["PlayerState"]["CashKing"]["Name"], data["PlayerState"]["CashKing"]["RankPoints"], Convert.ToInt64(data["PlayerState"]["CashKingCash"]).ToString("#,#", CultureInfo.InvariantCulture));
                Int64 cash = Convert.ToInt64(data["PlayerState"]["Cash"]);
                if (wheelResult == 6)
                {
                    String retSteal = String.Empty;
                    String stealInfo = steal(SECRETKEY, SESSIONTOKEN, data, isStealAuto, out retSteal);
                    String retInfo = String.Empty;
                    if (isStealAuto)
                    {
                        JToken jTokenSteal = JObject.Parse(retSteal);
                        cashKingInfo = String.Format("Name:{1} Rank:{2} Cash:{3}", jTokenSteal["PlayerState"]["CashKing"]["FBID"], jTokenSteal["PlayerState"]["CashKing"]["Name"], jTokenSteal["PlayerState"]["CashKing"]["RankPoints"], Convert.ToInt64(jTokenSteal["PlayerState"]["CashKingCash"]).ToString("#,#", CultureInfo.InvariantCulture));
                        cash = Convert.ToInt64(jTokenSteal["PlayerState"]["Cash"]);
                        retInfo = "[Steal]" + "\r\n" + playerInfo + "\r\n" + cashKingInfo;
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
                    String attackInfo = attackRandom(SECRETKEY, SESSIONTOKEN, data);
                    JToken jTokenAttack = JObject.Parse(attackInfo);
                    cash = Convert.ToInt64(jTokenAttack["PlayerState"]["Cash"]);
                    Data info = new Data();
                    info.Msg = "[Attack]" + "\r\n" + playerInfo + "\r\n" + cashKingInfo;
                    M_PLAY.ReportProgress(50, info);
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

                                Data info = new Data();
                                info.Msg = "[Repair] " + itemsDamaged[0].Name + " \r\n" + JObject.Parse(retRepair)["Island"][getItemName(itemsDamaged[0].Name)];
                                M_PLAY.ReportProgress(50, info);

                                ITEMS = getItems(JObject.Parse(retRepair));
                                setPrices(ITEMS);
                                cash = cash - itemsDamaged[0].Price;
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

                                Data info = new Data();
                                info.Msg = "[Upgrade] " + itemsUpgrade[0].Name + "\r\n" + JObject.Parse(retUpgrade)["Island"][getItemName(itemsUpgrade[0].Name)];
                                M_PLAY.ReportProgress(50, info);
                                ITEMS = getItems(JObject.Parse(retUpgrade));
                                setPrices(ITEMS);
                                cash = cash - itemsUpgrade[0].Price;
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
                            ITEMS = getItems(JObject.Parse(retFinish));
                            //
                            BASEPRICES = JsonConvert.DeserializeObject<List<Int32>>(JObject.Parse(retFinish)["NewIslandShopPrice"]["BasePrices"].ToString());
                            PRICESTEPS = JsonConvert.DeserializeObject<List<double>>(JObject.Parse(retFinish)["NewIslandShopPrice"]["PriceSteps"].ToString());
                            //
                            setPrices(ITEMS);
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
                    if (wheelResult == 6) info.Rank = Convert.ToInt32(data["PlayerState"]["CashKing"]["RankPoints"]);
                    M_PLAY.ReportProgress(100, info);
                    return;
                };

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
                    avatarPre.Visible = true;
                    avatar.Visible = true;
                    avatarNext.Visible = true;
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
            }
            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                MessageBox.Show("Error.", "PKTool", MessageBoxButtons.OK);
            }
            else
            {
            }
            btnPlay.Text = "Play all";
        }
        #endregion

        #region "M_KILL"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Kill_DoWork(object sender, DoWorkEventArgs e)
        {
            Friend friend = (Friend)e.Argument;
            setLogin(friend, true);
            while (true)
            {
                String retWheel = wheel(friend.Key, friend.SToken);
                JToken data = JObject.Parse(retWheel);
                int wheelResult = Convert.ToInt16(data["WheelResult"]);
                int spins = Convert.ToInt16(data["PlayerState"]["Spins"]);
                String playerInfo = String.Format("Rank:{0} Shields:{1} Spins:{2} Cash:{3} NextSpin: {4}", data["PlayerState"]["RankPoints"], data["PlayerState"]["Shields"], data["PlayerState"]["Spins"], Convert.ToInt64(data["PlayerState"]["Cash"]).ToString("#,#", CultureInfo.InvariantCulture), getTimes(Convert.ToInt32(data["NextSpinClaimSeconds"])));
                if (wheelResult == 6)
                {
                    String retSteal = String.Empty;
                    steal(friend.Key, friend.SToken, data, true, out retSteal);
                    M_KILL.ReportProgress(50, playerInfo);
                }
                else if (wheelResult == 7)
                {
                    attackRandom(friend.Key, friend.SToken, data);
                }
                    M_KILL.ReportProgress(50, playerInfo);
                //
                if (spins == 0)
                {
                    M_KILL.ReportProgress(100);
                    return;
                };
                if (M_KILL.CancellationPending)
                {
                    e.Cancel = true;
                    M_KILL.ReportProgress(0);
                    return;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Kill_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState == null) return;
                displayInfo("KILL SPIN", (String) e.UserState);
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
        void m_Kill_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled.", "PKTool", MessageBoxButtons.OK);
            }
            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                MessageBox.Show("Error.", "PKTool", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Done.");
            }

            btnKill.Text = "Kill spin";
        }
        #endregion

        #region "M_ATTACK"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Attack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled.", "PKTool", MessageBoxButtons.OK);
            }
            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                MessageBox.Show("Error.", "PKTool", MessageBoxButtons.OK);
            }
            else
            {
                if (e.Result != null && Convert.ToInt32(e.Result) == 1)
                {
                    MessageBox.Show("Destroyed.");
                }
                else
                {
                    MessageBox.Show("Damaged.");
                }
            }
            
            btnAttack.Text = "Attack";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Attack_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>) e.Argument;
            List<Friend> attackers = (List<Friend>) dic["attackers"];
            Friend victim = (Friend) dic["victim"];
            Int32 num = (Int32) dic["num"];
            Random rnd = new Random();
            while (attackers.Count > 0)
            {
                int index = rnd.Next(attackers.Count); //0->count-1
                Friend attacker = attackers[index];
                setLogin(attacker, true);
                if (attacker.Key != String.Empty)
                {
                    Int32 count = 0;
                    while (true)
                    {
                        String retWheel = wheel(attacker.Key, attacker.SToken);
                        JToken data = JObject.Parse(retWheel);
                        List<Item> itemTypes = getItemTypeAttack(victim.Id);
                        if (itemTypes.Count == 0)
                        {
                            M_ATTACK.ReportProgress(100);
                            e.Result = 1;
                            return;
                        }
                        int wheelResult = Convert.ToInt16(data["WheelResult"]);
                        int spins = Convert.ToInt16(data["PlayerState"]["Spins"]);
                        int rank = Convert.ToInt16(data["PlayerState"]["RankPoints"]);
                        if (rank == 5) break;
                        if (wheelResult == 6)
                        {
                            String retSteal = String.Empty;
                            steal(attacker.Key, attacker.SToken, data, true, out retSteal);
                        }
                        if (wheelResult == 7)
                        {
                            count = count + 1;
                            String ret = attackFriend(attacker, victim.Id, itemTypes[0].Name);
                            JToken jToken = JObject.Parse(ret);
                            M_ATTACK.ReportProgress(50, "Attacker: " + attacker.Name + "\r\n" + String.Format("GoldGained: {0} Result: {1}", Convert.ToInt64(jToken["GoldGained"].ToString()).ToString("#,#", CultureInfo.InvariantCulture), jToken["Result"].ToString()));
                        }
                        //num
                        if (num > 0)
                        {
                            if (count >= num)
                            {
                                count = 0;
                                break;
                            }
                        }
                        if (spins == 0)
                        {
                            break; //to next attacker
                        };
                        if (M_ATTACK.CancellationPending)
                        {
                            e.Cancel = true;
                            M_ATTACK.ReportProgress(0);
                            return;
                        }
                    }
                }
                attackers.RemoveAt(index);
                if (M_ATTACK.CancellationPending)
                {
                    e.Cancel = true;
                    M_ATTACK.ReportProgress(0);
                    return;
                }
            }
            M_ATTACK.ReportProgress(100);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Attack_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState == null) return;
                displayInfo("ATTACK", (String)e.UserState);
            }
            catch
            {
            }
        }
        #endregion

        #region "M_OWORKER"
        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Cancelled.", "PKTool", MessageBoxButtons.OK);
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error.", "PKTool", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("All done.", "PKTool", MessageBoxButtons.OK);
            }
            
            btnHTML.Text = "Start";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState == null) return;
                Data data = (Data)e.UserState;
                String info = String.Format("{0}.[FORCE] {1}:{2}{3}", data.Friend.Index, data.Friend.Name, data.Friend.Key, Environment.NewLine);
                rtbRetIn.Text = rtbRetIn.Text.Insert(0, info);
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
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Friend> lst = getFriends((String) e.Argument);
            Int32 counter = 0;
            foreach (var friend in lst)
            {
                //
                String data = getDataLogin(friend.Id, JsonConvert.SerializeObject(new List<String> { FBID }));
                String urlLogin = String.Format(URLLOGIN, DateTime.Now.ToOADate().ToString());
                String ret = doPost(urlLogin, data);
                try
                {
                    JToken jToken = JObject.Parse(ret);
                    if (jToken["Key"] != null) friend.Key = jToken["Key"].ToString();
                }
                catch
                { 
                }
                //
                counter += 1;
                friend.Index = counter;
                Int32 percentage = (counter * 100) / lst.Count();
                Data info = new Data();
                info.Friend = friend;
                M_OWORKER.ReportProgress(percentage, info);
                if (M_OWORKER.CancellationPending)
                {
                    e.Cancel = true;
                    M_OWORKER.ReportProgress(0);
                    return;
                }
            }
        }
        #endregion
        
        #region "EVENTS ON CONTROLS"
        private void btnHTML_Click(object sender, EventArgs e)
        {
            String html = rtbHTML.Text;
            String txtBtn = btnHTML.Text;
            switch (txtBtn.ToUpper())
            {
                case "START":
                    if (html == String.Empty) return;
                    if (BUSINESSTOKEN == String.Empty) return;
                    if (ACCESSTOKEN == String.Empty) return;
                    rtbRet.Text = String.Empty;
                    btnHTML.Text = "Stop";
                    M_OWORKER.RunWorkerAsync(html);
                    break;
                case "STOP":
                    M_OWORKER.CancelAsync();
                    break;
                default:
                    break;
            }

        }
        private void btnSetVicTim_Click(object sender, EventArgs e)
        {
            if (lbFriends.SelectedItem == null) return;
            if (IDATTACKER.Contains(((Friend)lbFriends.SelectedItem).Index)) return;
            VICTIM = (Friend)lbFriends.SelectedItem;
            txtVictim.Text = ((Friend)lbFriends.SelectedItem).Name;
        }
        private void btnSetAttacker_Click(object sender, EventArgs e)
        {
            if (lbFriends.SelectedItem == null) return;
            if (VICTIM != null)
            {
                if (((Friend)lbFriends.SelectedItem).Index == VICTIM.Index) return;
            }
            if (IDATTACKER.Contains(((Friend)lbFriends.SelectedItem).Index)) return;
            IDATTACKER.Add(((Friend)lbFriends.SelectedItem).Index);
            lbAttackers.Items.Add(((Friend)lbFriends.SelectedItem));
        }
        private void btnAttack_Click(object sender, EventArgs e)
        {
            if (VICTIM == null) return;
            if (FRIENDS.Count == 0) return;
            if (SECRETKEY == String.Empty) return;
            if (rdoOther.Checked && IDATTACKER.Count == 0) return;
            String txtBtn = btnAttack.Text;
            switch (txtBtn.ToUpper())
            {
                case "ATTACK":
                    List<Item> itemTypes = getItemTypeAttack(VICTIM.Id);
                    if (itemTypes.Count == 0)
                    {
                        MessageBox.Show("No items.");
                        return;
                    }
                    //
                    btnAttack.Text = "Cancel";
                    List<Friend> attackers = new List<Friend>();
                    if (rdoRandom.Checked)
                    {
                        attackers.AddRange(FRIENDS);
                    }
                    if (rdoByme.Checked)
                    {
                        Friend att = new Friend();
                        att.Index = -1;
                        att.Key = SECRETKEY;
                        att.Id = FBID;
                        att.Name = NAME;
                        attackers.Add(att);
                    }
                    if (rdoOther.Checked)
                    {
                        foreach (Friend att in FRIENDS)
                        {
                            if (IDATTACKER.Contains(att.Index))
                            {
                                attackers.Add(att);
                            }
                        }
                    }
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("attackers", attackers);
                    data.Add("victim", VICTIM);
                    data.Add("num", Convert.ToInt32(nudNum.Value));
                    M_ATTACK.RunWorkerAsync(data);
                    break;
                case "CANCEL":
                    M_ATTACK.CancelAsync();
                    break;
                default:
                    break;
            }

        }
        private void rdoOther_CheckedChanged(object sender, EventArgs e)
        {
            btnSetAttacker.Enabled = rdoOther.Checked;
            btnRemove.Enabled = rdoOther.Checked;
            lbAttackers.Enabled = rdoOther.Checked;
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbAttackers.SelectedItem == null) return;
            Friend atacker = (Friend)lbAttackers.SelectedItem;
            lbAttackers.Items.Remove(atacker);
            IDATTACKER.Remove(atacker.Index);
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
                setLogin(friend, true);
                if (friend.Key == String.Empty) return;
                String retWheel = wheel(friend.Key, friend.SToken);
                JToken jToken = JObject.Parse(retWheel);
                int wheelResult = Convert.ToInt16(jToken["WheelResult"]);
                int spins = Convert.ToInt16(jToken["PlayerState"]["Spins"]);
                String friendInfo = String.Format("Name:{4} Rank:{0} Shields:{1} Spins:{2} Cash:{3}", jToken["PlayerState"]["RankPoints"], jToken["PlayerState"]["Shields"], jToken["PlayerState"]["Spins"], Convert.ToInt64(jToken["PlayerState"]["Cash"]).ToString("#,#", CultureInfo.InvariantCulture), friend.Name);
                displayInfo("INFO: " + friend.Name, friendInfo);
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
            String txtBtn = btnPlay.Text;
            switch (txtBtn.ToUpper())
            {
                case "PLAY ALL":
                    btnPlay.Text = "Cancel";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("isAttackRandom", chkAutoAttack.Checked);
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
        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lbFriends.SelectedItem == null) return;
            Friend friend = (Friend)lbFriends.SelectedItem;
            String txtBtn = btnKill.Text;
            switch (txtBtn.ToUpper())
            {
                case "KILL SPIN":
                    if (MessageBox.Show("Are you sure to kill spin of " + friend.Name + "?", "PKTool", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        btnKill.Text = "Cancel";
                        M_KILL.RunWorkerAsync(friend);
                    }
                    break;
                case "CANCEL":
                    M_KILL.CancelAsync();
                    break;
                default:
                    break;
            }

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFBID.Text.Trim() == String.Empty) return;
            var lstGet = (from i in FRIENDS
                                where i.Id == txtFBID.Text.Trim()
                                select i).ToList();
            if (lstGet.Count > 0) return;
            Friend att = getFriendByFBID(txtFBID.Text.Trim());
            if (att == null)
            {
                MessageBox.Show("Invalid FBID.");
                txtFBID.Text = String.Empty;
            }
            else
            {
                att.Index = FRIENDS.Count + 1;
                FRIENDS.Add(att);
                FRIENDFBIDS.Insert(0, att.Id);
                lbFriends.DataSource = null;
                lbFriends.ValueMember = "Name";
                lbFriends.DataSource = FRIENDS;
                lbFriends.Refresh();
                txtFBID.Text = att.Name;
            }
        }
        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                if (SECRETKEY == String.Empty) return;
                String retWheel = wheel(SECRETKEY, SESSIONTOKEN);
                JToken jToken = JObject.Parse(retWheel);
                int wheelResult = Convert.ToInt16(jToken["WheelResult"]);
                int spins = Convert.ToInt16(jToken["PlayerState"]["Spins"]);
                String friendInfo = String.Format("Rank:{0} Shields:{1} Spins:{2} Cash:{3} NextSpin: {4}", jToken["PlayerState"]["RankPoints"], jToken["PlayerState"]["Shields"], jToken["PlayerState"]["Spins"], Convert.ToInt64(jToken["PlayerState"]["Cash"]).ToString("#,#", CultureInfo.InvariantCulture), getTimes(Convert.ToInt32(jToken["NextSpinClaimSeconds"])));
                displayInfo("INFO: " + NAME, friendInfo);
            }
            catch
            {
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //Re login to get cashking
                String data = getDataLogin(FBID, JsonConvert.SerializeObject(FRIENDFBIDS));
                String urlLogin = String.Format(URLLOGIN, DateTime.Now.ToOADate().ToString());
                String retLogin = doPost(urlLogin, data);
                JToken jTokenLogin = JObject.Parse(retLogin);
                //
                SECRETKEY = jTokenLogin["Key"].ToString();
                SESSIONTOKEN = jTokenLogin["SessionToken"].ToString();
                //
                String infoRet = "PlayerState" + "\r\n" + jTokenLogin["PlayerState"].ToString() + "\r\n" + "Island" + "\r\n" + jTokenLogin["Island"].ToString();
                displayInfo("REFRESH", infoRet);
            }
            catch
            {

            }
        }
        #endregion

        #region "EVENT ON FORM"
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            shutdowFiddlerApp();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            startFiddlerApp();
        }
        #endregion

        #region "METHOD"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fbId"></param>
        /// <param name="bToken"></param>
        /// <param name="aToken"></param>
        /// <param name="fbids"></param>
        /// <returns></returns>
        private string getDataLogin(String fbId, String friendFBIDs)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("CampaignReferral", "");
            dicResult.Add("DeviceToken", null);
            dicResult.Add("Email", "nhothuy48cb@gmail.com");
            dicResult.Add("FBID", fbId);
            dicResult.Add("FBName", "Thuy Nho");
            dicResult.Add("FriendFBIDs", friendFBIDs);
            dicResult.Add("GCID", null);
            dicResult.Add("GameVersion", 215);
            dicResult.Add("Platform", 2);
            dicResult.Add("UDID", "108a61cda531152f01e5436ba1a5b4fcf0acc23f");
            dicResult.Add("BusinessToken", BUSINESSTOKEN);
            dicResult.Add("AccessToken", ACCESSTOKEN);
            return JsonConvert.SerializeObject(dicResult);
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
                if (!NAMEVIPS.Contains(name.ToUpper()))
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
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        private int attack(Friend attacker, Friend victim)
        {
            if (attacker == null) return -2;
            if (victim == null) return -2;
            setLogin(attacker, true);
            if (attacker.Key == String.Empty) return -1;
            while (true)
            {
                String retWheel = wheel(attacker.Key, attacker.SToken);
                JToken data = JObject.Parse(retWheel);
                int wheelResult = Convert.ToInt16(data["WheelResult"]);
                int spins = Convert.ToInt16(data["PlayerState"]["Spins"]);
                if (wheelResult == 6)
                {
                    String retSteal = String.Empty;
                    steal(attacker.Key, attacker.SToken, data, true, out retSteal);
                }
                if (wheelResult == 7)
                {
                    List<Item> itemTypes = getItemTypeAttack(victim.Id);
                    if (itemTypes.Count == 0)
                    {
                        return 1;
                    }
                    String ret = attackFriend(attacker, victim.Id, itemTypes[0].Name);
                    rtbHTML.Text = rtbHTML.Text.Insert(0, ret + "\r\n\r\n");
                }
                if (spins == 0)
                {
                    return 0;
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="isStealAuto"></param>
        /// <returns></returns>
        private string steal(String secretKey, String sessionToken, JToken data, Boolean isStealAuto, out String retSteal)
        {
            retSteal = String.Empty;
            String ret = String.Empty;
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
            Int32 rank = Convert.ToInt32(data["PlayerState"]["CashKing"]["RankPoints"]);
            List<Key> lstKeysOrder = new List<Key>();
            if (rank >= RANKPOINT_STEAL)
            {
                //descending
                var query = from item in lstKeys
                            orderby item.Level descending
                            select item;
                lstKeysOrder = query.ToList();
            }
            else
            {
                //ascending
                var query = from item in lstKeys
                            orderby item.Level ascending
                            select item;
                lstKeysOrder = query.ToList();
            }
            if (isStealAuto)
            {
                try
                {
                    steal(lstKeysOrder[0].Index, secretKey, sessionToken);
                }
                catch
                {

                }
            }
            return String.Format("Steal: {0} No-Level: 1-{1} 2-{2} 3-{3}", lstKeysOrder[0].Index + 1, lstKeys[0].Level, lstKeys[1].Level, lstKeys[2].Level); ;
        }
        /// <summary>
        /// Set login
        /// </summary>
        /// <param name="friend"></param>
        private String setLogin(Friend friend, bool isUpdate)
        {
            if (friend.Key == String.Empty || friend.SToken == String.Empty)
            {
                String data = getDataLogin(friend.Id, JsonConvert.SerializeObject(new List<String> {}));
                String urlLogin = String.Format(URLLOGIN, DateTime.Now.ToOADate().ToString());
                String retLogin = doPost(urlLogin, data);
                try
                {
                    friend.Key = JObject.Parse(retLogin)["Key"].ToString();
                    friend.SToken = JObject.Parse(retLogin)["SessionToken"].ToString();
                    //Update FRIENDS
                    if (isUpdate && friend.Type == 1)
                    {
                        foreach (Friend f in FRIENDS)
                        {
                            if (f.Index == friend.Index)
                            {
                                f.Key = friend.Key;
                                f.SToken = friend.SToken;
                            }
                        }
                    }
                    return retLogin;
                }
                catch
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Item> getItemTypeAttack(JToken data)
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

            var query = from item in itemTypes
                        orderby item.Level descending, item.Isdamaged descending
                        where item.Level > 0
                        select item;
            return query.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string attackRandom(String secretKey, String sessionToken, JToken data)
        {
            List<Item> itemTypes = getItemTypeAttack(data);
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
        private Friend getFriendByFBID(String fbID)
        {
            if (fbID.Trim() == String.Empty) return null;
            if (FIDVIPS.Contains(fbID.Trim())) return null;
            try
            {
                Friend friend = new Friend();
                friend.Id = txtFBID.Text.Trim();
                String retLogin = setLogin(friend, false);
                if (friend.Key == String.Empty) return null;
                String retWheel = wheel(friend.Key, friend.SToken);
                JToken jToken = JObject.Parse(retWheel);
                int rank = 0;
                try
                {
                    rank = Convert.ToInt16(jToken["PlayerState"]["RankPoints"]);
                }
                catch
                {
                }
                if (rank > 0)
                {
                    JToken jData = JObject.Parse(retLogin);
                    if (jData["PlayerMetaData"]["Name"] == null) return null;
                    friend.Name = jData["PlayerMetaData"]["Name"] != null ? jData["PlayerMetaData"]["Name"].ToString() : "";
                    friend.Type = 2;
                    return friend;
                }
                else
                {
                    return null;
                }
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
        private void displayAvatar(Int32 rank)
        {
            try
            {
                int baseName = rank / 30 + 1;
                int baseNamePre = baseName > 1 ? baseName - 1 : baseName;
                int baseNameNext = baseName < 23 ? baseName + 1 : baseName;
                avatarPre.Image = imageList.Images[String.Format("{0}.png", baseNamePre)];
                avatar.Image = imageList.Images[String.Format("{0}.png", baseName)];
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
                    JToken jTokenReq = JObject.Parse(dic["reqBody"].ToString());
                    JToken jTokenResp = JObject.Parse(dic["respBody"].ToString());
                    FRIENDFBIDS = JsonConvert.DeserializeObject<List<String>>(jTokenReq["FriendFBIDs"].ToString());
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

        

        
    }
}
