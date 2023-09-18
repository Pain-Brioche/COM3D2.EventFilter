using BepInEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;
using static COM3D2.EventFilter.ScenarioManager;

namespace COM3D2.EventFilter
{
    internal static class FilterManager
    {
        public static ScenarioManager ScenarioManager { get; set; }

        public static void FilterList(int personality, bool isFilterSpecial, bool isFilterNPC, bool isFilterCustom, string searchString = null)
        {
            List<ScenarioManager.Scenario>  ScnList = ScenarioManager.Scenarios;
            int personalityID = 0;

            EventFilter.Logger.LogInfo($"------------------------------- Starting new Filter -------------------------------");

            //Checking is a personality filter was set in the dropdown and converting it to the game's personality ID
            if (personality != 0)
            {
                personalityID = Personalities.GetId(personality);
                EventFilter.Logger.LogInfo($"Showing only {Personalities.PersonalityDic[personalityID]} ({personalityID}) events");
            }
            if (!searchString.IsNullOrWhiteSpace())
            {
                EventFilter.Logger.LogInfo($"Searching for {searchString}");
            }
            EventFilter.Logger.LogInfo($"Filter special Events: {isFilterSpecial}, Filter NPC Events: {isFilterNPC}");
            EventFilter.Logger.LogInfo($"-----------------------------------------------------------------------------------");



            //Actual filtering, disable all Events that do not match the filters.
            foreach (ScenarioManager.Scenario scn in ScnList)
            {
                //if an event IsFiltered, it will be hidden
                bool isHidden = false;

                //Custom Filter overrides every other type of filter except filter by custom
                if (isFilterCustom)
                    isHidden = scn.IsCustom;

               if (isFilterSpecial && !isHidden)
                   isHidden = scn.IsSpecial;

               if (isFilterNPC && !isHidden)
                   isHidden = scn.IsNPC;

               if (!searchString.IsNullOrWhiteSpace() && !isHidden)
                   isHidden = !scn.TextBlob.Contains(searchString.ToLower());

               if(personality > 0 && !isHidden)
               {
                    switch (personalityID)
                    {
                        case 999:
                            isHidden = (scn.MaidIDs.Length > 0 && !scn.IsNPC);
                            break;

                        case 1000:
                            isHidden = !scn.IsCustom;
                            break;

                        case > 0:
                            isHidden = !scn.MaidIDs.Contains(personalityID);

                            //special case as all NPCs are considered Mukus
                            if (personalityID == 80 && scn.IsNPC)
                                isHidden = true;

                            break;

                        default:
                            isHidden = false;
                            break;
                    }
               }
               

                #region logging
                //logging things, to be deleted

                string ml = $"{scn.Title.Replace("\n", "")} - {scn.Maids.Count} Maids: ";

                foreach (Maid maid in scn.Maids)
                {
                    int id = maid.status.personal.id;
                    string pers = MaidStatus.Personal.IdToUniqueName(id);
                    string tempStr = $"{id}=>{pers}  ";
                    ml = $"{ml}{tempStr}";
                }
                if (isHidden)
                    EventFilter.Logger.LogInfo($"OUT\t\t{ml}");
                else
                    EventFilter.Logger.LogInfo(ml);
                #endregion

                //Filtering out elements from the list
                scn.ButtonObject.SetActive(!isHidden);
            }

            //Hide disabled Events from the list and refresh it
            ScenarioManager.sceneScenarioSelect.m_ScenarioScroll.Grid.Reposition();
        }

        public static void ResetList()
        {
            ScenarioManager ??= new ScenarioManager();

            //Enable back all events.
            foreach (ScenarioManager.Scenario scn in ScenarioManager.Scenarios)
            {
                if (!scn.IsCustom)
                    scn.ButtonObject.SetActive(true);
            }

            ScenarioManager.sceneScenarioSelect.m_ScenarioScroll.Grid.Reposition();
        }

        public static void AddCutomFilterID(string idString)
        {
            int id;
            try
            {
                id = Int32.Parse(idString);
            }
            catch (FormatException)
            {
                EventFilter.Logger.LogWarning($"{idString} is not a valid Event ID!");
                EventFilter.Instance.EventFilterPluginPanel.customField.Text = "";
                return;
            }

            //Remove the ID from the inputField
            EventFilter.Instance.EventFilterPluginPanel.customField.Text = "";

            Scenario Scn = ScenarioManager.Scenarios.First(s => s.ID == id);

            //Add to the list for future checks and saving between sessions
            if (!Scn.IsCustom)
                EventFilter.Instance.CustomFilterIDS.Add(id);

            Scn.IsCustom = true;
            EventFilter.Logger.LogInfo($"Added Event: {Scn.Title} (ID: {Scn.ID}) to custom Filter");

            //Immediatly hide the event
            Scn.ButtonObject.SetActive(false);

            ScenarioManager.sceneScenarioSelect.m_ScenarioScroll.Grid.Reposition();

            EventFilter.SaveJson();
        }
    }
}
