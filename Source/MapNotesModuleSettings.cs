namespace Celeste.Mod.MapNotes;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Microsoft.Xna.Framework;

public class MapNotesModuleSettings : EverestModuleSettings {
    public bool NoteOverlayEnabled { get; set; }
    public Color PrimaryColor { get; set; } = Color.Black;
    public int SurfaceSize { get; set; } = 5;

    public ButtonBinding ButtonToggleNoteOverlay { get; set; }
    public ButtonBinding ButtonCursorInteract { get; set; }
    public ButtonBinding ButtonToggleTool { get; set; }
}