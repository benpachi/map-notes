using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Mod.MapNotes.Entities.Tools {
    public class Pencil : FreehandTool {

        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;

        public float[,] BrushWeights;
        public Color[] BrushData;
        public Texture2D BrushTexture;

        public enum PencilType {
            square,
            round
        }

        public Pencil(EditController Parent) {
            this.Parent = Parent;
            Visible = Settings.NoteOverlayEnabled;
            Depth = Parent.Depth;
            Tag = Parent.Tag;
            Size = 6;
            BrushData = new Color[Size * Size];
            BrushWeights = GetBrushWeights(Size);

            int index = 0;
            for (int i = 0; i < BrushWeights.GetLength(0); ++i) {
                for (int j = 0; j < BrushWeights.GetLength(1); ++j) {
                    BrushData[index++] = (Color.Black * BrushWeights[i, j]);
                }
            }

            BrushTexture = new Texture2D(Engine.Graphics.GraphicsDevice, Size, Size);
            BrushTexture.SetData(BrushData);
        }

        public override void Update() {
            base.Update();
            if (Settings.ButtonCursorInteract.Check) {
                Vector2 prevRoomPosition = Parent.mouseRoomPosition - Parent.mouseDelta;
                Vector2[] inputArea = GetBresenhamLine(prevRoomPosition, Parent.mouseRoomPosition);
                var output = ApplyBrush(inputArea, BrushWeights, Size);

                MapNotesModule.AddPixelData(Parent.level, output);
            }
        }

        public override void Render() {
            base.Render();
            Draw.SpriteBatch.Draw(BrushTexture, Parent.mouseWorldPosition, Color.White);
        }
    }
}
