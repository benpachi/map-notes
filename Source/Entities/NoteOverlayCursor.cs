using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Celeste.Mod.MapNotes.Entities {
    internal class NoteOverlayCursor : Entity {

        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;

        public Vector2 mousePos;
        public Vector2 cameraOffset; // Top left position of camera
        public Vector2 levelPos;
        public Color[] BrushData;
        public Texture2D BrushTexture;
        public int brushSize = 6;


        public NoteOverlayCursor() {
            Tag = Tags.Global | Tags.PauseUpdate | Tags.FrozenUpdate;
            Visible = Settings.NoteOverlayEnabled;
            Depth = -100000;
            BrushData = new Color[brushSize*brushSize];
            for (int i = 0; i < BrushData.Length; ++i) {
                BrushData[i] = Color.Black;
            }
            BrushTexture = new Texture2D(Engine.Graphics.GraphicsDevice, brushSize, brushSize);
            BrushTexture.SetData(BrushData);
        }

        public override void Update() {
            base.Update();

            Level level = SceneAs<Level>();

            mousePos.X = MInput.Mouse.Position.X * (level.Camera.Viewport.Width / 1920f);
            mousePos.Y = MInput.Mouse.Position.Y * (level.Camera.Viewport.Height / 1080f);

            cameraOffset.X = level.Camera.Left;
            cameraOffset.Y = level.Camera.Top;

            Position.X = MathF.Min(cameraOffset.X + mousePos.X, level.Camera.Right);
            Position.Y = MathF.Min(cameraOffset.Y + mousePos.Y, level.Camera.Bottom);

            Visible = Settings.NoteOverlayEnabled;

            levelPos.X = (int)Math.Abs(level.Bounds.Left - Position.X);
            levelPos.Y = (int)Math.Abs(level.Bounds.Bottom - Position.Y - level.Session.LevelData.Bounds.Height);

            if (Settings.ButtonCursorInteract.Check) {
                MapNotesModule.AddPixelData(level, levelPos, BrushData, brushSize, brushSize);
            }
        }

        public override void Render() {
            base.Render();
            Draw.SpriteBatch.Draw(BrushTexture, Position, Color.White);
            ActiveFont.Draw(levelPos.ToString(), cameraOffset, default, new Vector2(0.2f, 0.2f), Color.Black);
        }
    }
}
