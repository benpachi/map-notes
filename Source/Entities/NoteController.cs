using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Mod.MapNotes.Entities {
    public class NoteController : Entity {
        public Vector2 mousePos;
        public Vector2 cameraOffset; // Top left position of camera
        public Vector2 levelPos;
        public Level level;

        public class Tool : Entity {
            public NoteController Parent;

            public override void Update() {
                if (Parent.currentTool != this) {
                    RemoveSelf();
                }
                base.Update();
            }
        }

        public Tool currentTool;

        public NoteController() {
            Visible = true;
            Tag = Tags.Global | Tags.PauseUpdate | Tags.FrozenUpdate;
        }

        public override void Added(Scene scene) {
            base.Added(scene);
            level = SceneAs<Level>();
        }

        public override void Awake(Scene scene) {
            base.Awake(scene);
            scene.Add(currentTool = new Tools.Pencil(this));
        }

        public override void Update() {
            base.Update();

            mousePos.X = MInput.Mouse.Position.X * (level.Camera.Viewport.Width / 1920f);
            mousePos.Y = MInput.Mouse.Position.Y * (level.Camera.Viewport.Height / 1080f);

            cameraOffset.X = level.Camera.Left;
            cameraOffset.Y = level.Camera.Top;

            Position.X = MathF.Min(cameraOffset.X + mousePos.X, level.Camera.Right);
            Position.Y = MathF.Min(cameraOffset.Y + mousePos.Y, level.Camera.Bottom);

            levelPos.X = (int)Math.Abs(level.Bounds.Left - Position.X);
            levelPos.Y = (int)Math.Abs(level.Bounds.Bottom - Position.Y - level.Session.LevelData.Bounds.Height);
        }

        // for debugging, not to be shipped
        public override void Render() {
            base.Render();
            ActiveFont.Draw(levelPos.ToString(), cameraOffset, default, new Vector2(0.2f, 0.2f), Color.Black);
        }
    }
}
