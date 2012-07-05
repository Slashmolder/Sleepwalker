using System;
using System.Collections.Generic;

namespace Sleepwalker
{
    internal class Berries
    {
        private const int dataOffset = 16;
        private readonly List<Berry> berries;
        private readonly HttpBrowser browser;
        private readonly string token;

        // state:
        // null = empty
        // 0 = mound
        // 1 = bud
        // 2 = tree
        // 3 = flowering
        // 4 = can harvest
        // HP4 needs to be watered
        public Berries(string data, HttpBrowser browser, string token)
        {
            this.browser = browser;
            this.token = token;

            //remove data at begining and end
            int end = data.IndexOf(']');
            data = data.Substring(dataOffset, end - dataOffset - 1);
            string[] split = {"},{"};
            string[] berriesData = data.Split(split, StringSplitOptions.RemoveEmptyEntries);

            berries = new List<Berry>();
            foreach (string berryData in berriesData)
            {
                berries.Add(new Berry(berryData));
            }

            /*berries = new List<Berry>
                          {new Berry {ID = "9825670"}, new Berry {ID = "9825671"}, new Berry {ID = "9825672"}};*/
        }


        public void HarvestAllBerries()
        {
            foreach (Berry berry in berries)
            {
                if (berry.Empty || berry.BerryState < 4) continue;
                string postData = string.Format("token={0}&p=pdw%2Ecroft%2Ekinomi%5Fharvesting&my%5Fcroft%5Fid={1}",
                                                token, berry.ID);
                browser.PostData("http://pdw1.pokemon-gl.com/api/", postData);
            }
        }

        public void WaterAllBerries()
        {
            foreach (Berry berry in berries)
            {
                // empty berries should have HP 100 but checking to be safe
                if (berry.Empty || berry.HP > 10) continue;
                string postData = string.Format("token={0}&p=pdw%2Ecroft%2Ekinomi%5Fwatering&my%5Fcroft%5Fid={1}",
                                                token, berry.ID);
                browser.PostData("http://pdw1.pokemon-gl.com/api/", postData);
            }
        }

        public void GetInventoryBerries()
        {
            string response =
                browser.GetData("http://pdw1.pokemon-gl.com/api/?token=" + token +
                                "&p=pdw%2Eitem%2Eitem%5Flist&status=0&item%5Fkind%5Fid=1&rowcount=10&offset=0&sort%5Fkey=2");
            // not sure what this is for but might as well do it
            response =
                browser.GetData("http://pdw1.pokemon-gl.com/api/?token=" + token +
                                "&p=pdw%2Eitem%2Eitem%5Flist&status=2&item%5Fkind%5Fid=0&rowcount=9999&offset=0");
        }

        public void PlantBerry(Berry berry, string itemID)
        {
            // ensure we are't planting to a spot that's already in use
            if (!berry.Empty) return;
            string postData =
                string.Format("token={0}&p=pdw%2Ecroft%2Ekinomi%5Fsowing&my%5Fcroft%5Fid={1}&pokeitem%5Fid={2}", token,
                              berry.ID, itemID);
            browser.PostData("http://pdw1.pokemon-gl.com/api/", postData);
        }

        public void PlantAllBerry(string itemID)
        {
            foreach (Berry berry in berries)
            {
                PlantBerry(berry, itemID);
            }
        }
    }

    internal class Berry
    {
        // ID of the berry slot
        public Berry(string data)
        {
            ID = Utilities.AllInstances("\"my_croft_id\":\"", "\"", data)[0];
            Empty = data.Contains("\"kinomi_id\":null");
            HP = int.Parse(Utilities.AllInstances("\"dirt_hp\":", ",", data)[0]);
            X = Utilities.AllInstances("\"x\":\"", "\"", data)[0];
            Y = Utilities.AllInstances("\"y\":\"", "\"", data)[0];
            if (Empty) return;
            // rest will be null and error otherwise
            BerryID = Utilities.AllInstances("\"kinomi_id\":\"", "\"", data)[0];
            PokeItemID = int.Parse(Utilities.AllInstances("\"pokeitem_id\":", ",", data)[0]);
            BerryState = int.Parse(Utilities.AllInstances("\"kinomi_state\":", ",", data)[0]);
            // check for utf escape sequences and store as full utf strings
            Name = Utilities.DecodeEncodedNonAsciiCharacters(Utilities.AllInstances("\"kinomi\":\"", "\"", data)[0]);
            Des1 = Utilities.DecodeEncodedNonAsciiCharacters(Utilities.AllInstances("\"desc1\":\"", "\"", data)[0]);
            Des2 = Utilities.DecodeEncodedNonAsciiCharacters(Utilities.AllInstances("\"desc2\":\"", "\"", data)[0]);
            Des3 = Utilities.DecodeEncodedNonAsciiCharacters(Utilities.AllInstances("\"desc3\":\"", "\"", data)[0]);
        }

        public string ID { get; set; }
        public bool Empty { get; set; }
        public string BerryID { get; set; }
        public int PokeItemID { get; set; }
        public int BerryState { get; set; }
        public string Name { get; set; }
        public string Des1 { get; set; }
        public string Des2 { get; set; }
        public string Des3 { get; set; }
        public int HP { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }
}