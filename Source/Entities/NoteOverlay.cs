using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.MapNotes.Entities {
    public class NoteOverlay : Entity {

        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;

        private static string text = "pachi pachi pachi";
        private static Vector2 position = new Vector2(Engine.Width/2, Engine.Height/2);
        private static Vector2 justify = new Vector2(0.5f, 0.5f);
        private static Vector2 scale = new Vector2(1, 1);
        private static Color testColor = Color.Black;

        public NoteOverlay() {
            Visible = Settings.NoteOverlayEnabled;
            Depth = -100;
            Tag = TagsExt.SubHUD | Tags.Global;
        }

        public override void Update() {
            base.Update();
            Visible = Settings.NoteOverlayEnabled;
        }

        public override void Render() {
            base.Render();

            ActiveFont.Draw(text, position, justify, scale, testColor);
        }
    }
}