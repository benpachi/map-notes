using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Celeste.Mod.MapNotes;

public class MapNotesModuleSaveData : EverestModuleSaveData {
    // Indexed by map name, room name
    public Dictionary<(string, string), Color[]> NoteCellDict { get; set; } = [];
}