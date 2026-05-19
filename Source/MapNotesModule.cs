using System;
using Celeste.Mod.MapNotes.Entities;
using Celeste.Mod.MapNotes;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
            var noteOverlayCursor = new NoteOverlayCursor();
            level.Add(noteOverlayCursor);
            if (!SaveData.NoteCellDict.ContainsKey(mapName)) {
                SaveData.NoteCellDict[mapName] = new Dictionary<string, Color[]>();
            }
        }

        Vector2 roomPos = new Vector2(level.Bounds.Left, level.Bounds.Top);
        int roomWidth = level.Bounds.Width;
        int roomHeight = level.Bounds.Height;

        if (!SaveData.NoteCellDict[mapName].ContainsKey(roomName)) {
            SaveData.NoteCellDict[mapName][roomName] = new Color[roomWidth * roomHeight];
        }

        level.Add(new NoteOverlayCell(mapName, roomName, roomPos, roomWidth, roomHeight));
    }

    public static void AddPixelData(Level level, Vector2 position, Color[] brushData, int width, int height) {
        string mapName = level.Session.Area.SID;
        string levelName = level.Session.Level;

        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int index = y * width + x;
                Color color = brushData[index];
                Vector2 pixelPosition = new Vector2(x + position.X, y + position.Y);

                int roomWidth = level.Bounds.Width;

                int pixelIndex = (int)(pixelPosition.Y * roomWidth + pixelPosition.X);
                SaveData.NoteCellDict[mapName][levelName][pixelIndex] = color;
            }
        }
    }
}