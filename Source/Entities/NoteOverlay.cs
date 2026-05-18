using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Celeste.Mod.MapNotes.Entities {
    internal class NoteOverlay : Entity {

        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;

        public float mouseX, mouseY;
        public float brushSize;

        public NoteOverlay() {
            Tag = TagsExt.SubHUD | Tags.Global | Tags.PauseUpdate | Tags.FrozenUpdate | Tags.Persistent;
            Visible = Settings.NoteOverlayEnabled;
            Depth = -1000;
        }

        public override void Update() {
            base.Update();
            Level level = SceneAs<Level>();
            Position = MInput.Mouse.Position;
            Position.X = MathF.Round(Position.X / 6f) * 6;
            Position.Y = MathF.Round(Position.Y / 6f) * 6;
            Visible = Settings.NoteOverlayEnabled;
            brushSize = 24;
        }

        public override void Render() {
            base.Render();
            Draw.Rect(Position, brushSize, brushSize, Color.Black);
        }
    }
}
