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

        public (string, string) Key;
        public Texture2D Texture;
        public string MapName;
        public string RoomName;

        public NoteOverlayCell(string mapName, string roomName, Vector2 position, int width, int height) {
            Visible = Settings.NoteOverlayEnabled;
            Depth = -100000;
            Tag = Tags.PauseUpdate | Tags.FrozenUpdate;
            Texture = new Texture2D(Engine.Graphics.GraphicsDevice, width, height);
            Position = position;
            MapName = mapName;
            RoomName = roomName;
            Key = (mapName, roomName);
        }

        public override void Update() {
            base.Update();
            Visible = Settings.NoteOverlayEnabled;
            Texture.SetData(SaveData.NoteCellDict[(MapName, RoomName)]);
        }

        public override void Render() {
            base.Render();
            Draw.SpriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}