using Celeste.Mod.MapNotes;
using Celeste.Mod.MapNotes.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Celeste.Mod.MapNotes;

public class MapNotesModule : EverestModule {
    public static MapNotesModule Instance { get; private set; }

    public override Type SettingsType => typeof(MapNotesModuleSettings);
    public static MapNotesModuleSettings Settings => (MapNotesModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(MapNotesModuleSession);
    public static MapNotesModuleSession Session => (MapNotesModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(MapNotesModuleSaveData);
    public static MapNotesModuleSaveData SaveData => (MapNotesModuleSaveData) Instance._SaveData;

    public MapNotesModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(MapNotesModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(MapNotesModule), LogLevel.Info);
#endif
    }

    public override void Load() {
        Everest.Events.Level.OnLoadLevel += Level_OnLoadLevel;
        On.Celeste.Level.Update += OnLevelUpdate;
    }

    public override void Unload() {
        On.Celeste.Level.Update -= OnLevelUpdate;
        Everest.Events.Level.OnLoadLevel -= Level_OnLoadLevel;
    }

    /* Hooks */

    private static void OnLevelUpdate(On.Celeste.Level.orig_Update orig, Level self) {
        orig(self);

        if (Settings.ButtonToggleNoteOverlay.Pressed) {
            Settings.NoteOverlayEnabled = !Settings.NoteOverlayEnabled;
        }
    }

    private static void Level_OnLoadLevel(Level level, Player.IntroTypes playerIntro, bool isFromLoader) {
        string mapName = level.Session.Area.SID;
        string roomName = level.Session.Level;

        if (isFromLoader) {
            var noteController = new NoteController();
            level.Add(noteController);
        }

        Vector2 roomPos = new Vector2(level.Bounds.Left, level.Bounds.Top);
        int roomWidth = level.Bounds.Width;
        int roomHeight = level.Bounds.Height;

        if (!SaveData.NoteCellDict.ContainsKey((mapName, roomName))) {
            SaveData.NoteCellDict[(mapName, roomName)] = new Color[roomWidth * roomHeight];
        }

        level.Add(new NoteOverlayCell(mapName, roomName, roomPos, roomWidth, roomHeight));
    }

    public static void AddPixelData(Level level, Dictionary<Vector2, Color> pixels) {
        string mapName = level.Session.Area.SID;
        string roomName = level.Session.Level;

        foreach (KeyValuePair<Vector2, Color> pixel in pixels) {
            int pixelIndex = (int)(pixel.Key.Y * level.Bounds.Width + pixel.Key.X);
            SaveData.NoteCellDict[(mapName, roomName)][pixelIndex] = pixel.Value;
        }

        /*for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int index = y * width + x;
                Color color = brushData[index];
                Vector2 pixelPosition = new Vector2(x + position.X, y + position.Y);

                int roomWidth = level.Bounds.Width;

                int pixelIndex = (int)(pixelPosition.Y * roomWidth + pixelPosition.X);
                SaveData.NoteCellDict[(mapName, roomName)][pixelIndex] = color;
            }
        }*/
    }
}