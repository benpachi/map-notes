using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Mod.MapNotes.Entities {
    public class NoteOverlayCell : Entity {

        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;
        private static MapNotesModuleSaveData SaveData => MapNotesModule.SaveData;

        private static float Scale => 6f;
        public Texture2D Texture;

        public NoteOverlayCell(Vector2 position, Color[] data, int width, int height) {
            Visible = Settings.NoteOverlayEnabled;
            Depth = -100;
            Tag = Tags.PauseUpdate | Tags.Persistent;
            Texture = new Texture2D(Engine.Graphics.GraphicsDevice, width, height);
            Texture.SetData(data);
            Position = position;
        }

        public override void Update() {
            base.Update();
            Visible = Settings.NoteOverlayEnabled;
        }

        public override void Render() {
            base.Render();
            Draw.SpriteBatch.Draw(Texture, Position, null, Color.White, 0f, Position, Scale, SpriteEffects.None, Depth);
        }
    }
}