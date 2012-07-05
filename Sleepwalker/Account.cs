namespace Sleepwalker
{
    internal class Account
    {
        private readonly HttpBrowser browser = new HttpBrowser();
        private Berries berries;
        private bool is_downloaded;
        private int play_status;
        private bool sleepingFlag;
        private string token;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; private set; }

        // todo: fix ref, only for sake of potential detection
        public bool Login()
        {
            // load the login page
            browser.GetData("http://ja.pokemon-gl.com/");
            // todo: make a function to build the strings like this for me
            string postData = "DATA=" +
                              HttpBrowser.UrlEncode("{" +
                                                    string.Format(
                                                        "\"URL\":\"\",\"LOGIN_ROOT\":\"PGL\",\"ISMORE_FLG\":1,\"PASSWORD\":\"{1}\",\"MID\":\"{0}\"",
                                                        Username, Cryptography.MD5Hash(Password)) + "}");
            string response = response = browser.PostData("https://members.pokemon.jp/api/login", postData);
            Nickname = Utilities.AllInstances("NICKNAME\":\"", "\"", response)[0];
            // this is needed to set the cookies correctly
            browser.GetData("https://members.pokemon.jp/common/loginRedirect?LOGIN_ROOT=PGL");
            return response.Contains("\"ERROR\":{\"CODE\":\"ok\"");
        }

        public void Start()
        {
            string response = browser.GetData("http://ja.pokemon-gl.com/pdw");
            response = browser.GetData("http://ja.pokemon-gl.com/api/?p=pgl.top.init&ping=0&now=1341475609946");
                // + Utilities.EpochTime());
            ParseStart(response);

            // login to dw
            string postData = string.Format("token={0}&p=pgl%2Emember%2Eprofile%2Epdw%5Flogin",
                                            token);
            response = browser.PostData("http://ja.pokemon-gl.com/api/", postData);

            // time check
            postData = string.Format("token={0}&p=pdw%2Ehome%2Epdw%5Ftimecheck",
                                     token);
            response = browser.PostData("http://pdw1.pokemon-gl.com/api/", postData);

            // re check state
            response =
                browser.GetData("http://pdw1.pokemon-gl.com/api/?token=" + token +
                                "&state=1&p=pgl%2Emember%2Eprofile%2Emy%5Fstate");

            // start
            postData = string.Format("token={0}&p=pdw%2Ehome%2Epdw%5Fstart", token);
            browser.PostData("http://pdw1.pokemon-gl.com/api/", token);

            // news, don't need for now
            // response = browser.GetData("http://pdw1.pokemon-gl.com/api/?token=" + token + "&p=pgl%2Enews%2Einformation%5Flist&rowcount=8&weekly%5Fflag=0&offset=0

            response = browser.GetData("http://pdw1.pokemon-gl.com/api/?token=" + token + "&p=pdw%2Ehome%2Emy%5Fisland");
        }

        private void ParseStart(string data)
        {
            sleepingFlag = Utilities.AllInstances("\"sleeping_flag\":\"", "\"", data)[0] == "1";
            play_status = int.Parse(Utilities.AllInstances("\"play_status\":\"", "\"", data)[0]);
            is_downloaded = Utilities.AllInstances("\"is_downloaded\":", ",", data)[0] == "1";
            token = HttpBrowser.UrlEncode(Utilities.AllInstances("\"token\":\"", "\"", data)[0]);
        }

        public void Berries()
        {
            GetBerries();
        }

        public void GetBerries()
        {
            string response =
                browser.GetData(
                    "http://pdw1.pokemon-gl.com/api/?token=" + token + "&p=pdw%2Ecroft%2Emy%5Fcroft%5Flist");

            berries = new Berries(response, browser, token);

            string postData = string.Format("token={0}&p=pdw%2Ecroft%2Ewaterpot%5Flist", token);
            response = browser.PostData("http://pdw1.pokemon-gl.com/api/", postData);
        }

        public bool CheckStatus()
        {
            string source =
                browser.GetData("http://ja.pokemon-gl.com/traffic/product_5556/status.json?time=" +
                                Utilities.EpochTime());
            return source.Contains("{\"is_over_capacity\":0, \"condition\":0");
        }
    }
}