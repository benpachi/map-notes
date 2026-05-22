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

        public Texture2D SurfaceTexture;
        public Color[] SurfaceTextureData;
        public int SurfaceSize;
        public Dictionary<Vector2, Color> PixelBuffer = [];
        public Color[] PixelBufferData = [];
        public Texture2D PixelBufferTexture;

        public bool clearFlag = false;

        public Level level;

        public Pencil(EditController Parent) {
            this.Parent = Parent;
            level = Parent.level;
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
                    SurfaceTextureData[index++] = (Settings.PrimaryColor * Surface[i, j]);
                }
            }

            SurfaceTexture.SetData(SurfaceTextureData);
        }

        public override void Update() {
            base.Update();

            // keeps the buffer visible for one more frame to prevent flicker
            if (clearFlag == true) {
                PixelBuffer = [];
                PixelBufferData = [];
                PixelBufferTexture = null;
                clearFlag = false;
            }

            if (Settings.ButtonCursorInteract.Pressed) {
                PixelBufferTexture = new Texture2D(Engine.Graphics.GraphicsDevice, level.Bounds.Width, level.Bounds.Height);
                PixelBufferData = new Color[level.Bounds.Width * level.Bounds.Height];
            }

            if (Settings.ButtonCursorInteract.Check) {
                Vector2 prevRoomPosition = Parent.mouseRoomPosition - Parent.mouseDelta;
                Vector2[] inputArea = GetBresenhamLine(prevRoomPosition, Parent.mouseRoomPosition);
                var output = ApplySurface(inputArea, Surface, SurfaceSize, Settings.PrimaryColor);

                foreach (KeyValuePair<Vector2, Color> pixel in output) {
                    PixelBuffer[pixel.Key] = pixel.Value;
                    int pixelIndex = (int)(pixel.Key.Y * level.Bounds.Width + pixel.Key.X);
                    PixelBufferData[pixelIndex] = pixel.Value;
                }

                PixelBufferTexture.SetData(PixelBufferData);
            }

            if (Settings.ButtonCursorInteract.Released) {
                MapNotesModule.AddPixelData(Parent.level, PixelBuffer);
                clearFlag = true;
            }
        }

        public override void Render() {
            base.Render();
            if (PixelBufferTexture != null) {
                Draw.SpriteBatch.Draw(PixelBufferTexture, new Vector2(Parent.level.Bounds.Left, Parent.level.Bounds.Top), Color.White);
            }
            Draw.SpriteBatch.Draw(SurfaceTexture, Parent.mouseWorldPosition, Color.White);
        }
    }
}
