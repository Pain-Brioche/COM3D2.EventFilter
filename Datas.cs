using I2.Loc.SimpleJSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COM3D2.EventFilter
{
    internal class Datas
    {
        private readonly static string jsonPath = BepInEx.Paths.ConfigPath + "\\COM3D2.EventFilter.json";
        public List<int> CustomFilterIDS { get; set; }
        public HashSet<int> AlreadyPlayedIDs { get; set; }

        public Datas()
        { 
            if (!File.Exists(jsonPath))
            {
                CustomFilterIDS = new List<int>();
                AlreadyPlayedIDs = new HashSet<int>();
                return;
            }

            string json = File.ReadAllText(jsonPath);
            Datas loadedData = JsonConvert.DeserializeObject<Datas>(json);

            CustomFilterIDS = loadedData.CustomFilterIDS;
            AlreadyPlayedIDs = loadedData.AlreadyPlayedIDs;
        }

        public void SaveJson()
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(EventFilter.Instance.datas));
        }
    }
}
