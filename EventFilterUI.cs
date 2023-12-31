﻿using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Styles;
using CM3D2.UGUI;
using CM3D2.UGUI.Resources;
using CM3D2.UGUI.Panels;
using UniverseLib.UI.Controls;
using UniverseLib.UI.Models;

namespace COM3D2.EventFilter
{
    public partial class EventFilter : BaseUnityPlugin
    {
        //Using Luvoid's ButtJiggle plugin as an example on how to proceed with CM3D2.UGUI and the modified UniverseLib
        public static UIBase UIBase => Instance?.EventFilterUIBase;
        private UIBase EventFilterUIBase;
        internal PluginPanel EventFilterPluginPanel;

        // Initialize the universe through CM3D2Universe.
        // This ensures correct default settings,
        // and prevents issues with VR and karaokee.
        void Start()
        {
            CM3D2Universe.Init(OnUIStart);
        }

        //Run once on creation
        void OnUIStart()
        {
            // Create a UIBase and specify update callback
            EventFilterUIBase = CM3D2UniversalUI.RegisterUI(PluginInfo.PLUGIN_GUID);

            // Is the panel displayed by default?
            EventFilterUIBase.Enabled = false;

            // Create the main panel
            EventFilterPluginPanel = new PluginPanel(EventFilterUIBase);
        }

        internal void EnableUI()
        {
            if (EventFilterUIBase != null) EventFilterUIBase.Enabled = true;
            if (EventFilterPluginPanel != null)
            {
                EventFilterPluginPanel.Enabled = true;
                EventFilterPluginPanel.EnsureValidPosition();
                EventFilterPluginPanel.EnsureValidSize();
            }
        }

        internal void DisableUI()
        {
            if (EventFilterUIBase != null) EventFilterUIBase.Enabled = false;
            if (EventFilterPluginPanel != null) EventFilterPluginPanel.Enabled = false;
        }

        internal void ToggleUI()
        {
            if (EventFilterUIBase == null) return;

            if (EventFilterUIBase.Enabled) { DisableUI(); }
            else { EnableUI(); }
        }
    }

    internal class PluginPanel : CM3D2Panel
    {
        public PluginPanel(UIBase owner ) : base(owner) { }

        //Various 
        public override string Name => "Event Filter";
        public override Vector2 PreferredSize => _preferedSize;
        public override Vector2 DefaultAnchorMin => new(1f, 0.5f);
        public override Vector2 DefaultAnchorMax => DefaultAnchorMin;
        public override Vector2 DefaultPosition => new(-Screen.width / 3, -Screen.height / 6);
        public override bool CanDragAndResize => true;

        public override IReadOnlyUISkin Skin => Styles.StandardSkin;


        //values
        public Vector2 _preferedSize = new(400, 340);
        public int selectedPersonality = 0;
        public string searchText = string.Empty;
        public bool isFilterSpecial = false;
        public bool isFilterNPC = false;
        public bool isFilterCustom = true;
        public bool isFilterNTR = false;
        public bool isFilterPlayed = false;
        public InputFieldModel customField;
        public ButtonModel addCustomButton;
        public BoolControl NTRFilterBoolControl;
        public BoolControl PlayedFilterBoolControl;

        private int GetHeight()
        {
            int h = 340;

            if (EventFilter.Instance.EnableNTRFilter.Value)
                h += 34;

            if (EventFilter.Instance.EnablePlayedFilter.Value)
                h += 34;

            return h;
        }

        protected override void OnClosePanelClicked()
        {
            EventFilter.Instance.DisableUI();
        }

        public void UpdateOptionalFiltersVisibility()
        {
            NTRFilterBoolControl.Enabled = EventFilter.Instance.EnableNTRFilter.Value;
            PlayedFilterBoolControl.Enabled = EventFilter.Instance.EnablePlayedFilter.Value;

            _preferedSize = new Vector2(400, GetHeight());
            EventFilter.Instance.EventFilterPluginPanel.SetDefaultSizeAndPosition();

        }

        protected override void ConstructPanelContent()
        {
            //Make each element occupy the full witdth of the panel
            using (Create.LayoutContext(flexibleWidth: 1))
            {
                EventFilter.Logger.LogWarning($"x:{DefaultPosition.x} y:{DefaultPosition.y}");

                //Default layout is a vertical stack, each new element will be placed bellow.
                var personalityDropdown = Create.Dropdown(ContentRoot, "Personality", null, selectedPersonality, Personalities.GetPersonalityArray());
                var searchField = Create.InputField(ContentRoot, "Search", "Search...");

                Create.BoolControl(ContentRoot, "FilterEvents", "Remove Special Events", refGet: () => ref isFilterSpecial);
                Create.BoolControl(ContentRoot, "FilterExtra", "Remove NPC Events", refGet: () => ref isFilterNPC);

                //Optional NTR Filter
                NTRFilterBoolControl = Create.BoolControl(ContentRoot, "FilterNTR", "Remove NTR Events", refGet: () => ref isFilterNTR);

                //Optional Already Played Filter
                PlayedFilterBoolControl = Create.BoolControl(ContentRoot, "FilterPlayed", "Remove Played Events", refGet: () => ref isFilterPlayed);

                //Custom Filter Frame content with a sub group to place the textfield and button in
                var customFilterFrame = Create.VerticalFrame(ContentRoot, "CustomFrame");

                Create.BoolControl(customFilterFrame.ContentRoot, "FilterCustom", "Custom Filter", refGet: () => ref isFilterCustom);

                var customFilterAddEventGroup = Create.HorizontalGroup(customFilterFrame.ContentRoot, "CustomFilterAddEventGroup", childAlignment: TextAnchor.MiddleLeft);
                customField = Create.InputField(customFilterAddEventGroup, "CustomID", "Event ID");

                addCustomButton = Create.Button(customFilterAddEventGroup, "AddID", "Add ID");

                UIFactory.SetCanvasGroup(customFilterAddEventGroup, null, isFilterCustom);

                addCustomButton.OnClick += delegate ()
                {
                    FilterManager.AddCutomFilterID(customField.Text);
                };

                //General Filter Button
                var filterButton = Create.Button(ContentRoot, "Filter", "Filter");

                filterButton.OnClick += delegate ()
                {
                    selectedPersonality = personalityDropdown.Value;
                    searchText = searchField.Text;
                    FilterManager.FilterList(selectedPersonality, isFilterSpecial, isFilterNPC, isFilterCustom, isFilterNTR, isFilterPlayed, searchText);
                };

                //Reset all Filters Button
                var resetButton = Create.Button(ContentRoot, "Reset", "Reset");

                resetButton.OnClick += delegate ()
                {
                    FilterManager.ResetList();
                };
            }
        }
    }
}
