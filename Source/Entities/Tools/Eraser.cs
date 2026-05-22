using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.MapNotes.Entities.Tools {
    public class Eraser : FreehandTool {
        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;

        public int SurfaceSize;
        public Color[] SurfaceTextureData;
        public Texture2D SurfaceTexture;

        public Eraser(EditController Parent) {
            this.Parent = Parent;
            Visible = Settings.NoteOverlayEnabled;
            Depth = Parent.Depth;
            Tag = Parent.Tag;
            SurfaceSize = Settings.SurfaceSize;
            Surface = GenerateSurface(SurfaceSize);
            SurfaceTexture = new Texture2D(Engine.Graphics.GraphicsDevice, SurfaceSize, SurfaceSize);
            SurfaceTextureData = new Color[SurfaceSize * SurfaceSize];

            int index = 0;
            for (int i = 0; i < Surface.GetLength(0); ++i) {
                for (int j = 0; j < Surface.GetLength(1); ++j) {
                    SurfaceTextureData[index++] = (Color.White * (Surface[i, j] / 2));
                }
            }

            SurfaceTexture.SetData(SurfaceTextureData);
        }

        public override void Update()
        {
            base.Update();
            if (Settings.ButtonCursorInteract.Check)
            {
                Vector2 prevRoomPosition = Parent.mouseRoomPosition - Parent.mouseDelta;
                Vector2[] inputArea = GetBresenhamLine(prevRoomPosition, Parent.mouseRoomPosition);
                var output = ApplySurface(inputArea, Surface, SurfaceSize, Color.Transparent);

                MapNotesModule.SetPixelData(Parent.level, output);
            }
        }

        public override void Render()
        {
            base.Render();
            Draw.SpriteBatch.Draw(SurfaceTexture, Parent.mouseWorldPosition, Color.White);
        }
    }
}