using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace COM3D2.EventFilter
{
    [BepInPlugin("COM3D2.EventFilter", "Event Filter", "1.0.1")]
    public partial class EventFilter : BaseUnityPlugin
    {
        //Main Instance
        internal static EventFilter Instance { get; private set; }
        //internal static FilterManager FilterManager = new();

        //Logger
        internal static new ManualLogSource Logger => Instance?.logger;
        private ManualLogSource logger => base.Logger;

        //Data
        //internal Datas datas = new();

        //config
        internal ConfigEntry<bool> EnableNTRFilter;
        internal ConfigEntry<bool> EnablePlayedFilter;

       

        private void Awake()
        {
            Instance = this;

            // Harmony
            Harmony.CreateAndPatchAll(typeof(Patches));

            //Loading Previous Data
            //Instance.datas.LoadJson();

            // BepinEx config
            EnableNTRFilter = Config.Bind("Filters", "Enable NTR Filter", false, "Add an option to filter NTR events");
            EnablePlayedFilter = Config.Bind("Filters", "Enable Already Played Filter", false, "Add an option to filter already played Events");


            SceneManager.sceneLoaded += OnSceneLoaded;
            Config.SettingChanged += OnSettingChanged;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "SceneScenarioSelect")
            {
                FilterManager.ScenarioManager = null;
                Instance.EnableUI();
                Instance.EventFilterPluginPanel.UpdateOptionalFiltersVisibility();
            }

            else
                Instance.DisableUI();
        }

        private void OnSettingChanged(object sender, SettingChangedEventArgs args)
        {
            Instance.EventFilterPluginPanel.UpdateOptionalFiltersVisibility();
        }
    }
}
