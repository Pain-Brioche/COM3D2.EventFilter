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
        public List<int> CustomFilterIDS { get; set; } = new();
        public HashSet<int> AlreadyPlayedIDs { get; set; } = new();

        public void LoadJson()
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                EventFilter.Instance.datas = JsonConvert.DeserializeObject<Datas>(json);
            }
        }

        public void SaveJson()
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(EventFilter.Instance.datas));
        }
    }
    /*
    internal static class JsonHelper
    {
        private readonly static string jsonPath = BepInEx.Paths.ConfigPath + "\\COM3D2.EventFilter.json";

        public static void LoadJson()
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                EventFilter.Instance.datas = JsonConvert.DeserializeObject<Datas>(json);
            }
        }

        public static void SaveJson()
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(EventFilter.Instance.datas));
        }
    }
    */
}
