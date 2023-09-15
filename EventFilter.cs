using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using UniverseLib;
using CM3D2.UGUI;
using Newtonsoft.Json;

namespace COM3D2.EventFilter
{
    [BepInPlugin("COM3D2.EventFilter", "Event Filter", "0.1")]
    public partial class EventFilter : BaseUnityPlugin
    {
        //Main Instance
        public static EventFilter Instance { get; private set; }
        //internal static FilterManager FilterManager = new();

        //Logger
        internal static new ManualLogSource Logger => Instance?.BaseLogger;
        private ManualLogSource BaseLogger => base.Logger;

        internal List<int> CustomFilterIDS { get; private set; } = new();
        private readonly static string jsonPath = BepInEx.Paths.ConfigPath + "\\COM3D2.EventFilter.json";        

        private void Awake()
        {
            Instance = this;


            LoadJson();

            // Harmony
            Harmony.CreateAndPatchAll(typeof(Patches));

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "SceneScenarioSelect")
            {
                FilterManager.ScenarioManager = null;
                Instance.EnableUI();
            }

            else
                Instance.DisableUI();
        }

        private static void LoadJson()
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                Instance.CustomFilterIDS = JsonConvert.DeserializeObject<List<int>>(json);
            }
        }

        internal static void SaveJson()
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(Instance.CustomFilterIDS));
        }
    }
}
