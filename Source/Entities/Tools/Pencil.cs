using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Mod.MapNotes.Entities.Tools {
    public class Pencil : NoteController.Tool {

        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;

        public Color[] BrushData;
        public Texture2D BrushTexture;
        public int Size = 6;

        public enum PencilType {
            square,
            round
        }

        public Pencil(NoteController Parent) {
            this.Parent = Parent;
            Visible = Settings.NoteOverlayEnabled;
            Depth = Parent.Depth;
            Tag = Parent.Tag;
            BrushData = new Color[Size * Size];
            for (int i = 0; i < BrushData.Length; ++i)
            {
                BrushData[i] = Color.Black;
            }
            BrushTexture = new Texture2D(Engine.Graphics.GraphicsDevice, Size, Size);
            BrushTexture.SetData(BrushData);
        }

        public override void Update() {
            base.Update();
            if (Settings.ButtonCursorInteract.Check) {
                MapNotesModule.AddPixelData(Parent.level, Parent.levelPos, BrushData, Size, Size);
            }
        }

        public override void Render() {
            base.Render();
            Draw.SpriteBatch.Draw(BrushTexture, Parent.Position, Color.White);
        }
    }
}
