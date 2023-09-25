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
using I2.Loc.SimpleJSON;
using System;

namespace COM3D2.EventFilter
{
    [BepInPlugin("COM3D2.EventFilter", "Event Filter", "1.0")]
    public partial class EventFilter : BaseUnityPlugin
    {
        //Main Instance
        internal static EventFilter Instance { get; private set; }
        //internal static FilterManager FilterManager = new();

        //Logger
        internal static new ManualLogSource Logger => Instance?.logger;
        private ManualLogSource logger => base.Logger;

        //Data
        internal Datas datas = new();

        //config
        internal ConfigEntry<bool> EnableNTRFilter;
        internal ConfigEntry<bool> EnablePlayedFilter;

       

        private void Awake()
        {
            Instance = this;

            // Harmony
            Harmony.CreateAndPatchAll(typeof(Patches));

            SceneManager.sceneLoaded += OnSceneLoaded;

            // BepinEx config
            EnableNTRFilter = Config.Bind("Filters", "Enable NTR Filter", false, "Add an option to filter NTR events");
            EnablePlayedFilter = Config.Bind("Filters", "Enable Already Played Filter", false, "Add an option to filter already played Events");
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
    }
}
