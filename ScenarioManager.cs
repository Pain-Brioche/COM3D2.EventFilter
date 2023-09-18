using MaidStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.EventFilter
{
    internal class ScenarioManager
    {
        public List<Scenario> Scenarios {  get; private set; } = new();
        public SceneScenarioSelect sceneScenarioSelect;

        public ScenarioManager()
        {
            EventFilter.Logger.LogWarning("ScenarioManager Init");
            sceneScenarioSelect ??= UnityEngine.Object.FindObjectOfType<SceneScenarioSelect>();

            foreach (KeyValuePair<UIWFTabButton, ScenarioData> scn in sceneScenarioSelect.m_ScenarioButtonpair)
            {
                if (scn.Value == null || scn.Key == null)
                {
                    EventFilter.Logger.LogWarning($"{scn.Value.Title} UIFTabButton/ScenarioData returned NULL");
                    continue;
                }

                Scenario scenario = new(scn);
                Scenarios.Add(scenario);
            }

            sceneScenarioSelect.m_ScenarioScroll.Grid.hideInactive = true;
            //sceneScenarioSelect.m_ScenarioScroll.Grid.Reposition();
        }

        public class Scenario
        {
            public int ID { get; private set; }
            public string Title { get; private set; }
            public string ScenarioScript { get; private set; }
            public List<Maid> Maids { get; private set; }
            public int[] MaidIDs { get; private set; }
            public UIWFTabButton UIWFTabButton { get; private set; }
            public GameObject ButtonObject { get; private set; }
            public bool IsNPC { get; private set; } = false;
            public bool IsSpecial { get; private set; } = false;
            public bool IsCustom { get; set; } = false;
            public string TextBlob { get; private set; }

            public Scenario(KeyValuePair<UIWFTabButton, ScenarioData> scn)
            {
                ID = scn.Value.ID;
                Title = scn.Value.Title;
                ScenarioScript = scn.Value.ScenarioScript;
                Maids = scn.Value.m_EventMaid;
                UIWFTabButton = scn.Key;
                ButtonObject = UIWFTabButton.transform.parent.gameObject;

                //Recover Maid's ID
                MaidIDs = Maids.Select(m => m.status.personal.id).ToArray();

                //NPC should all have either a NPC or a Extra icon
                UI2DSprite sprite = ButtonObject.GetComponentInChildren<UI2DSprite>();
                if (sprite != null)
                {
                    if (sprite.mainTexture.name == "event_extra_icon" || sprite.mainTexture.name == "event_npc_icon")
                        IsNPC = true;
                }
                else
                {
                    EventFilter.Logger.LogWarning("sprite is null");
                }

                //Let's consider that special events don't have Maids in their list, it's more complicated than that, but it'll do for now
                if (Maids.Count == 0 )
                    IsSpecial = true;

                //CustomID list is recovered between sessions from a .json
                IsCustom = EventFilter.Instance.CustomFilterIDS.Contains(scn.Value.ID);
                if (IsCustom )
                {
                    ButtonObject.SetActive(false);
                }

                //building text blob, a concatenation of all relevant text to search into
                TextBlob = $"{Title} {ScenarioScript}";
                foreach (var maid in Maids.Select(m => m.status))
                {
                    string str = $"{maid.firstName} {maid.lastName} {maid.nickName}";
                    TextBlob = $"{TextBlob} {str}";
                }
                TextBlob = TextBlob.ToLower() ;
            }
        }
    }
}
