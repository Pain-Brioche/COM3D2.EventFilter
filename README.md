# COM3D2.EventFilter

Adds a graphical Interface to filter the event list according to various parameters.

## How to Install:
- Drop the plugin's .dll inside BepinEx/plugins/

## How to use:
- Select filters and press the filter button, only Events corresponding to the selection will appear in event the list.
- Reset will restore the event list to its initial state.

## Additional options:
- A NTR filter is available if enabled in the F1 config menu.
- A Already played filter is available if enabled in the F1 config menu.
  If enabled it will tint already played events with a slight green, and allow you to remove them from the list. Beware some events can be played several times with multiple maids.

- Custom Filter. Select an event and press the "Add ID" button, this event will always be hidden by default in the list. You can remove it from the list at any time by selecting it again and press the "Remove ID" button.

## Potential issues:
- Filter button doesn't work and an error appears in the log: try deleting the plugin's config file in bepinEx/config
- Interface looks wierd or broken: make sure you don't have any Atlas textures in i18nEx/English/textures.


## Thanks:
- This plugin uses Luvoid's [COM3D2.UGUI library](https://github.com/luvoid/CM3D2.UGUI) as well as the [modified Universelib](https://github.com/luvoid/UniverseUGUI).
- Luvoid and Doc for general code improvement.
