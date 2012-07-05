using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace Sleepwalker
{
    internal class HttpBrowser
    {
        #region UserAgentEnum enum

        public enum UserAgentEnum
        {
            Chrome,
            Firefox,
            InternetExplorer,
            IPhone
        }

        #endregion

        private const string ChromeUserAgent =
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.47 Safari/536.11";

        private const string FirefoxUserAgent =
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:13.0) Gecko/20120703 Firefox/13.0";

        private const string InternetExplorerUserAgent =
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";

        private const string IPhoneUserAgent =
            "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5";

        public HttpBrowser()
        {
            Cookies = new CookieContainer();
            UserAgent = FirefoxUserAgent;
        }

        public HttpBrowser(UserAgentEnum userAgent)
        {
            switch (userAgent)
            {
                case UserAgentEnum.Chrome:
                    UserAgent = ChromeUserAgent;
                    break;
                case UserAgentEnum.Firefox:
                    UserAgent = FirefoxUserAgent;
                    break;
                case UserAgentEnum.InternetExplorer:
                    UserAgent = InternetExplorerUserAgent;
                    break;
                case UserAgentEnum.IPhone:
                    UserAgent = IPhoneUserAgent;
                    break;
                default:
                    UserAgent = FirefoxUserAgent;
                    break;
            }

            Cookies = new CookieContainer();
        }

        public HttpBrowser(string userAgent)
        {
            switch (userAgent)
            {
                case "Chrome":
                    UserAgent = ChromeUserAgent;
                    break;
                case "Firefox":
                    UserAgent = FirefoxUserAgent;
                    break;
                case "InternetExplorer":
                    UserAgent = InternetExplorerUserAgent;
                    break;
                case "IPhone":
                    UserAgent = IPhoneUserAgent;
                    break;
                default:
                    UserAgent = FirefoxUserAgent;
                    break;
            }

            Cookies = new CookieContainer();
        }

        public CookieContainer Cookies { get; set; }

        public string UserAgent { get; set; }

        public string LastUrl { get; set; }

        public WebProxy Proxy { get; set; }

        public string GetData(string url, bool useRef)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.CookieContainer = Cookies;
                request.UserAgent = UserAgent;
                request.ServicePoint.Expect100Continue = false;
                request.Method = "GET";
                if (useRef)
                    request.Referer = LastUrl;
                if (Proxy != null)
                    request.Proxy = Proxy;
                var response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    var reader = new StreamReader(responseStream);
                    string str = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                    response.Close();
                    LastUrl = url;
                    return str;
                }
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Console.WriteLine(e);
                //return e.Message
                return null;
            }
            return "";
        }

        public string GetData(string url)
        {
            return GetData(url, LastUrl != null);
        }

        public string PostData(string url, string postData, bool useRef)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.CookieContainer = Cookies;
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = UserAgent;
                request.ContentLength = postData.Length;
                if (useRef)
                    request.Referer = LastUrl;
                if (Proxy != null)
                    request.Proxy = Proxy;
                using (Stream writeStream = request.GetRequestStream())
                {
                    var encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(postData);
                    writeStream.Write(bytes, 0, bytes.Length);
                }
                string result = null;
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    Cookies.Add(response.Cookies);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                result = readStream.ReadToEnd();
                            }
                    }
                }
                LastUrl = url;
                return result;
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Console.WriteLine(e);
                //return e.Message
                return null;
            }
        }

        public string PostData(string url, string postData)
        {
            return PostData(url, postData, LastUrl != null);
        }

        public void QuickLoad(string url)
        {
            var wr = (HttpWebRequest) WebRequest.Create(url);
            if (Proxy != null)
                wr.Proxy = Proxy;
            wr.CookieContainer = Cookies;
            wr.GetResponse().Close();
        }

        public void ClearCookies()
        {
            Cookies = new CookieContainer();
        }

        public Image GetImage(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.CookieContainer = Cookies;
            request.UserAgent = UserAgent;
            request.ServicePoint.Expect100Continue = false;
            request.Method = "GET";
            var response = (HttpWebResponse) request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                Image i = Image.FromStream(responseStream);
                responseStream.Close();
                return i;
            }
            return null;
        }

        #region UrlEncode

        public static string UrlEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return UrlEncode(str, Encoding.UTF8);
        }

        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
        }

        public static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            byte[] bytes = e.GetBytes(str);
            return UrlEncode(bytes, 0, bytes.Length, false);
        }

        private static byte[] UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
        {
            byte[] buffer = UrlEncode(bytes, offset, count);
            if ((alwaysCreateNewReturnValue && (buffer != null)) && (buffer == bytes))
            {
                return (byte[]) buffer.Clone();
            }
            return buffer;
        }


        private static byte[] UrlEncode(byte[] bytes, int offset, int count)
        {
            if (!ValidateUrlEncodingParameters(bytes, offset, count))
            {
                return null;
            }
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                var ch = (char) bytes[offset + i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsUrlSafeChar(ch))
                {
                    num2++;
                }
            }
            if ((num == 0) && (num2 == 0))
            {
                return bytes;
            }
            var buffer = new byte[count + (num2*2)];
            int num4 = 0;
            for (int j = 0; j < count; j++)
            {
                byte num6 = bytes[offset + j];
                var ch2 = (char) num6;
                if (IsUrlSafeChar(ch2))
                {
                    buffer[num4++] = num6;
                }
                else if (ch2 == ' ')
                {
                    buffer[num4++] = 0x2b;
                }
                else
                {
                    buffer[num4++] = 0x25;
                    buffer[num4++] = (byte) IntToHex((num6 >> 4) & 15);
                    buffer[num4++] = (byte) IntToHex(num6 & 15);
                }
            }
            return buffer;
        }

        public static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char) (n + 0x30);
            }
            return (char) ((n - 10) + 0x61);
        }

        public static bool IsUrlSafeChar(char ch)
        {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
            {
                return true;
            }
            switch (ch)
            {
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
        {
            if ((bytes == null) && (count == 0))
            {
                return false;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return true;
        }

        #endregion
    }
}