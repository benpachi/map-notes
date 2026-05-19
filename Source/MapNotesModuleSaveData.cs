using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Celeste.Mod.MapNotes;

public class MapNotesModuleSaveData : EverestModuleSaveData {
    // Outer string: map name
    // Inner string: room name
    public Dictionary<string, Dictionary<string, Color[]>> NoteCellDict { get; set; } = [];
}