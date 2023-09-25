using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.EventFilter
{
    internal static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SceneScenarioSelect), "OnSelectScenario")]
        private static void DisplayID()
        {
            ScenarioManager scnM = FilterManager.ScenarioManager;
            if (scnM == null) return;

            // prevents spamming because of Kiss' code.
            if (!UIWFSelectButton.current.isSelected) return; 

            //hopefully each scenario has an unique ID
            ScenarioManager.Scenario scenario = scnM.Scenarios.First(s => s.ID == scnM.sceneScenarioSelect.m_CurrentScenario.ID);
            //EventFilter.Logger.LogWarning($"Selected ID: {scenario.ID}");

            EventFilter.Instance.EventFilterPluginPanel.customField.Text = scenario.ID.ToString();

            EventFilter.Instance.EventFilterPluginPanel.addCustomButton.Label.text = scnM.Scenarios.Contains(scenario) ? "Remove ID" : "Add ID";
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SceneScenarioSelect), "Start")]
        private static void InitScenarioManager()
        {
            FilterManager.ScenarioManager ??= new ScenarioManager();

            //Recolor the already played events
            if (EventFilter.Instance.EnablePlayedFilter.Value)
            {
                //EventFilter.Logger.LogWarning("IS ENABLED PLAYED");
                foreach (var scn in FilterManager.ScenarioManager.Scenarios.Where(s => s.IsPlayed))
                {
                    //EventFilter.Logger.LogWarning($"{scn.ID} is played.");
                    scn.UIWFTabButton.defaultColor = new Color(0.8f, 1, 0.8f, 0.8f);
                }
            }

            //apply/reapply the filter upon loading the Event scene.
            PluginPanel pp = EventFilter.Instance.EventFilterPluginPanel;
            FilterManager.FilterList(pp.selectedPersonality, pp.isFilterSpecial, pp.isFilterNPC, pp.isFilterCustom, pp.isFilterNTR, pp.isFilterPlayed, pp.searchText);

        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SceneScenarioSelect), "PushOkButton")]
        private static void ConsiderEventPlayed()
        {
            ScenarioManager scnM = FilterManager.ScenarioManager;
            if (scnM == null) return;


            ScenarioManager.Scenario scenario = scnM.Scenarios.First(s => s.ID == scnM.sceneScenarioSelect.m_CurrentScenario.ID);
            scenario.IsPlayed = true;

            EventFilter.Instance.datas.AlreadyPlayedIDs.Add(scenario.ID);
            EventFilter.Logger.LogInfo($"Event {scenario.ID} is now considered as played.");
            EventFilter.Instance.datas.SaveJson();
        }
        /*
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SceneScenarioSelect), "SetScenarioPlate")]
        private static void SetPlayedState()
        {
            if (!EventFilter.Instance.EnablePlayedFilter.Value) return;

            foreach(var scn in FilterManager.ScenarioManager.Scenarios)
            {
                if (scn.IsPlayed)
                {
                    scn.UIWFTabButton.defaultColor = new Color(0,1,0,0.5f);


                    //UISprite bg = scn.ButtonObject.transform.Find("BG")?.gameObject.GetComponent<UISprite>();
                    //bg.color
                }
            }
        }
        */
    }
}
