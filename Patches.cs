using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.EventFilter
{
    internal class Patches
    {
        /*
 [HarmonyPostfix]
 [HarmonyPatch(typeof(ScenarioSelectMgr), "GetAllScenarioData")]
 private static ScenarioData[] EditScenarioList(ScenarioData[] originalScenarioArray)
 {
     foreach (ScenarioData scenarioData in originalScenarioArray)
     {
         //Get valid maids for each listed event.
         List<Maid> maidList = scenarioData.GetEventMaidList();
         string ml = $"{scenarioData.Title} - {maidList.Count} Maids: ";

         //Check if a maid correspond to the set filters
         foreach (Maid maid in maidList)
         {
             int id = maid.status.personal.id;
             string personality = MaidStatus.Personal.IdToUniqueName(id);
             string tempStr = $"{id}=>{personality} | ";
             ml = $"{ml}{tempStr}";
         }
         EventFilter.Logger.LogMessage(ml);
     }

     if (EventFilter.firstRun)
     {
         EventFilter.firstRun = false;
         return originalScenarioArray;
     }

     //Select only events where a maid matches the filter.
     List<ScenarioData> editedScenarioList = (from ScenarioData scenarioData in originalScenarioArray
                                              where scenarioData.GetEventMaidList().Select(m => m.status.personal.id).Contains(80)
                                              select scenarioData).ToList();

     return editedScenarioList.ToArray();
 }
 */


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
            EventFilter.Logger.LogWarning($"Selected ID: {scenario.ID}");

            EventFilter.Instance.EventFilterPluginPanel.customField.Text = scenario.ID.ToString();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SceneScenarioSelect), "Start")]
        private static void InitScenarioManager()
        {
            FilterManager.ScenarioManager ??= new ScenarioManager();

            //apply/reapply the filter upon loading the Event scene.
            PluginPanel pp = EventFilter.Instance.EventFilterPluginPanel;
            FilterManager.FilterList(pp.selectedPersonality, pp.isFilterSpecial, pp.isFilterNPC, pp.isFilterCustom, pp.searchText);
        }
    }
}
