using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Celeste.Mod.MapNotes;

public class MapNotesModuleSaveData : EverestModuleSaveData {
    public record struct NoteCellData(Color[] TextureData, int Width, int Height) {
        public Color[] TextureData { get; set; } = TextureData;
    }

    public Dictionary<Level, Dictionary<Vector2, NoteCellData>> NoteCellDict { get; set; } = [];
}