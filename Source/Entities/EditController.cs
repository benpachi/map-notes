using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celeste.Mod.MapNotes.Entities {
    public class EditController : Entity {
        public Vector2 mouseViewportPosition;
        public Vector2 mouseWorldPosition;
        public Vector2 mouseRoomPosition;
        public Vector2 mouseDelta; // In Celeste pixels


        private static MapNotesModuleSettings Settings => MapNotesModule.Settings;
        public Level level;

        public class Tool : Entity {
            public EditController Parent;

            public override void Update() {
                if (Parent.currentTool != this) {
                    RemoveSelf();
                }
                base.Update();
            }
        }

        public Tool currentTool;

        public EditController() {
            Visible = true;
            Tag = Tags.Global | Tags.PauseUpdate | Tags.FrozenUpdate;
            Depth = -100000;
        }

        public override void Added(Scene scene) {
            base.Added(scene);
            level = SceneAs<Level>();
            scene.Add(currentTool = new Tools.Pencil(this));

        }

        public override void Update() {
            base.Update();

            if (Settings.ButtonToggleTool.Pressed) {
                if (currentTool is Tools.Pencil) {
                    SceneAs<Level>().Add(currentTool = new Tools.Eraser(this));
                } else {
                    SceneAs<Level>().Add(currentTool = new Tools.Pencil(this));
                }
            }

            mouseViewportPosition.X = MInput.Mouse.Position.X * (level.Camera.Viewport.Width / 1920f);
            mouseViewportPosition.Y = MInput.Mouse.Position.Y * (level.Camera.Viewport.Height / 1080f);

            Vector2 prevMouseWorldPosition = mouseWorldPosition;

            mouseWorldPosition.X = MathF.Min(level.Camera.Left + mouseViewportPosition.X, level.Camera.Right);
            mouseWorldPosition.Y = MathF.Min(level.Camera.Top + mouseViewportPosition.Y, level.Camera.Bottom);

            mouseDelta = mouseWorldPosition - prevMouseWorldPosition;

            mouseRoomPosition.X = (int)Math.Abs(level.Bounds.Left - mouseWorldPosition.X);
            mouseRoomPosition.Y = (int)Math.Abs(level.Bounds.Bottom - mouseWorldPosition.Y - level.Session.LevelData.Bounds.Height);
        }

        // for debugging, not to be shipped
        public override void Render() {
            base.Render();
            ActiveFont.Draw(mouseRoomPosition.ToString(), level.Camera.Position, default, new Vector2(0.2f, 0.2f), Color.Black);
        }
    }
}
