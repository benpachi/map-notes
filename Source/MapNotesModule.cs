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
        On.Celeste.Level.Update += OnLevelUpdate;
        Everest.Events.Level.OnLoadLevel += Level_OnLoadLevel;
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

        if (isFromLoader) {
            var noteOverlay = new NoteOverlay();
            level.Add(noteOverlay);
        }

        var levelNoteCellData = GetLevelNoteCellData(level);
        foreach (KeyValuePair<Vector2, MapNotesModuleSaveData.NoteCellData> noteCellData in levelNoteCellData) {
            level.Add(new NoteOverlayCell(noteCellData.Key, noteCellData.Value.TextureData, noteCellData.Value.Width, noteCellData.Value.Height));
        }
    }

    /* Note cells */

    public static Dictionary<Vector2, MapNotesModuleSaveData.NoteCellData> GetLevelNoteCellData(Level level) {
        if (!SaveData.NoteCellDict.ContainsKey(level)) {
            SaveData.NoteCellDict[level] = [];
        }
        return SaveData.NoteCellDict[level];
    }

    public static void AddNoteCellData(Level level, Vector2 position, int width, int height) {
        SaveData.NoteCellDict[level][position] = new MapNotesModuleSaveData.NoteCellData(new Color[width * height], width, height);
        level.Add(new NoteOverlayCell(position, SaveData.NoteCellDict[level][position].TextureData, width, height));
    }
}