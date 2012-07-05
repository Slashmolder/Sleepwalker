using System;
using System.Reflection;
using System.Windows.Forms;
using Sleepwalker.Properties;

namespace Sleepwalker
{
    public partial class frmMain : Form
    {
        private Account account;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Text += " v" + Assembly.GetExecutingAssembly().GetName().Version;

            LoadSettings();
        }

        private void LoadSettings()
        {
            txtUser.Text = Settings.Default.Username;
            txtPass.Text = Settings.Default.Password;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            account = new Account {Username = txtUser.Text, Password = txtPass.Text};
            account.Login();
            account.Start();
        }

        private void btnWater_Click(object sender, EventArgs e)
        {
            account.GetBerries();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            Settings.Default.Username = txtUser.Text;
            Settings.Default.Password = txtPass.Text;
            Settings.Default.Save();
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            Berries berries =
                new Berries(
                    "{\"croft_list\":[{\"my_croft_id\":\"9772135\",\"kinomi_id\":\"13\",\"pokeitem_id\":161,\"kinomi_state\":4,\"kinomi\":\"\u30de\u30b4\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30d4\u30f3\u30c1\u306e\u3068\u304d\u3000\uff28\uff30\u3092\u3000\u304b\u3044\u3075\u304f\u3059\u308b\u3002\",\"desc3\":\"\u304d\u3089\u3044\u306a\u3000\u3042\u3058\u3060\u3068\u3000\u3053\u3093\u3089\u3093\u3000\u3059\u308b\u3002\",\"dirt_hp\":80,\"x\":\"1\",\"y\":\"1\"},{\"my_croft_id\":\"9772136\",\"kinomi_id\":\"52\",\"pokeitem_id\":200,\"kinomi_state\":1,\"kinomi\":\"\u30db\u30ba\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30ce\u30fc\u30de\u30eb\u3000\u308f\u3056\u3092\u3000\u3046\u3051\u305f\u3068\u304d\",\"desc3\":\"\u3044\u308a\u3087\u304f\u304c\u3000\u3088\u308f\u307e\u308b\u3002\",\"dirt_hp\":4,\"x\":\"2\",\"y\":\"1\"},{\"my_croft_id\":\"9772137\",\"kinomi_id\":\"12\",\"pokeitem_id\":160,\"kinomi_state\":4,\"kinomi\":\"\u30a6\u30a4\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30d4\u30f3\u30c1\u306e\u3068\u304d\u3000\uff28\uff30\u3092\u3000\u304b\u3044\u3075\u304f\u3059\u308b\u3002\",\"desc3\":\"\u304d\u3089\u3044\u306a\u3000\u3042\u3058\u3060\u3068\u3000\u3053\u3093\u3089\u3093\u3000\u3059\u308b\u3002\",\"dirt_hp\":80,\"x\":\"3\",\"y\":\"1\"},{\"my_croft_id\":\"9772138\",\"kinomi_id\":\"12\",\"pokeitem_id\":160,\"kinomi_state\":4,\"kinomi\":\"\u30a6\u30a4\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30d4\u30f3\u30c1\u306e\u3068\u304d\u3000\uff28\uff30\u3092\u3000\u304b\u3044\u3075\u304f\u3059\u308b\u3002\",\"desc3\":\"\u304d\u3089\u3044\u306a\u3000\u3042\u3058\u3060\u3068\u3000\u3053\u3093\u3089\u3093\u3000\u3059\u308b\u3002\",\"dirt_hp\":80,\"x\":\"1\",\"y\":\"2\"},{\"my_croft_id\":\"9772139\",\"kinomi_id\":\"52\",\"pokeitem_id\":200,\"kinomi_state\":1,\"kinomi\":\"\u30db\u30ba\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30ce\u30fc\u30de\u30eb\u3000\u308f\u3056\u3092\u3000\u3046\u3051\u305f\u3068\u304d\",\"desc3\":\"\u3044\u308a\u3087\u304f\u304c\u3000\u3088\u308f\u307e\u308b\u3002\",\"dirt_hp\":4,\"x\":\"2\",\"y\":\"2\"},{\"my_croft_id\":\"9772140\",\"kinomi_id\":\"52\",\"pokeitem_id\":200,\"kinomi_state\":1,\"kinomi\":\"\u30db\u30ba\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30ce\u30fc\u30de\u30eb\u3000\u308f\u3056\u3092\u3000\u3046\u3051\u305f\u3068\u304d\",\"desc3\":\"\u3044\u308a\u3087\u304f\u304c\u3000\u3088\u308f\u307e\u308b\u3002\",\"dirt_hp\":4,\"x\":\"3\",\"y\":\"2\"},{\"my_croft_id\":\"9825670\",\"kinomi_id\":\"12\",\"pokeitem_id\":160,\"kinomi_state\":4,\"kinomi\":\"\u30a6\u30a4\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30d4\u30f3\u30c1\u306e\u3068\u304d\u3000\uff28\uff30\u3092\u3000\u304b\u3044\u3075\u304f\u3059\u308b\u3002\",\"desc3\":\"\u304d\u3089\u3044\u306a\u3000\u3042\u3058\u3060\u3068\u3000\u3053\u3093\u3089\u3093\u3000\u3059\u308b\u3002\",\"dirt_hp\":80,\"x\":\"1\",\"y\":\"3\"},{\"my_croft_id\":\"9825671\",\"kinomi_id\":\"12\",\"pokeitem_id\":160,\"kinomi_state\":4,\"kinomi\":\"\u30a6\u30a4\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30d4\u30f3\u30c1\u306e\u3068\u304d\u3000\uff28\uff30\u3092\u3000\u304b\u3044\u3075\u304f\u3059\u308b\u3002\",\"desc3\":\"\u304d\u3089\u3044\u306a\u3000\u3042\u3058\u3060\u3068\u3000\u3053\u3093\u3089\u3093\u3000\u3059\u308b\u3002\",\"dirt_hp\":80,\"x\":\"2\",\"y\":\"3\"},{\"my_croft_id\":\"9825672\",\"kinomi_id\":\"12\",\"pokeitem_id\":160,\"kinomi_state\":4,\"kinomi\":\"\u30a6\u30a4\u306e\u307f\",\"desc1\":\"\u30dd\u30b1\u30e2\u30f3\u306b\u3000\u3082\u305f\u305b\u308b\u3068\",\"desc2\":\"\u30d4\u30f3\u30c1\u306e\u3068\u304d\u3000\uff28\uff30\u3092\u3000\u304b\u3044\u3075\u304f\u3059\u308b\u3002\",\"desc3\":\"\u304d\u3089\u3044\u306a\u3000\u3042\u3058\u3060\u3068\u3000\u3053\u3093\u3089\u3093\u3000\u3059\u308b\u3002\",\"dirt_hp\":80,\"x\":\"3\",\"y\":\"3\"}],\"diglett_flag\":0}",
                    null, null);
        }
    }
}