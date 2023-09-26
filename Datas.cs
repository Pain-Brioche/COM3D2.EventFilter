using I2.Loc.SimpleJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COM3D2.EventFilter
{
    internal static class Datas
    {
        private static readonly string jsonPath = BepInEx.Paths.ConfigPath + "\\COM3D2.EventFilter.json";
        public static List<int> CustomFilterIDS { get; set; }
        public static HashSet<int> AlreadyPlayedIDs { get; set; }

        //Called when the class is accessed by anything and before anything is allowed into the class.
        static Datas()
        {
            //File doesn't exist, make collections.
            if (!File.Exists(jsonPath))
            {
                CustomFilterIDS = new List<int>();
                AlreadyPlayedIDs = new HashSet<int>();
                return;
            }

            //Read the text and create a JToken. See google on JTokens
            var json = File.ReadAllText(jsonPath);
            var loadedDatas = JToken.Parse(json);

            //Access the properties of the JToken directly and parse them to objects we can set. If the property is null or the ToObject is null, just set a new list.
            CustomFilterIDS = loadedDatas["CustomFilterIDS"]?.ToObject<List<int>>() ?? new List<int>();
            AlreadyPlayedIDs = loadedDatas["AlreadyPlayedIDs"]?.ToObject<HashSet<int>>() ?? new HashSet<int>();
        }

        public static void SaveJson()
        {
            //Create a json object and fill it with properties, these are our collections we want to serialize.
            var jsonObject = new JObject()
            {
                new JProperty("CustomFilterIDS", CustomFilterIDS),
                new JProperty("AlreadyPlayedIDs", AlreadyPlayedIDs),
            };

            //Save it.
            File.WriteAllText(jsonPath, jsonObject.ToString(Formatting.None));
        }
    }


    /*
    internal class Datas
    {
        private readonly string jsonPath = BepInEx.Paths.ConfigPath + "\\COM3D2.EventFilter.json";
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
    */
}
