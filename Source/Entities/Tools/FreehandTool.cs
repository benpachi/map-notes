using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.MapNotes.Entities.Tools {
    public class FreehandTool : EditController.Tool {

        public int Size;
        /*public enum BrushShape {
            square,
            round
        }*/

        public static float[,] GetBrushWeights(int size) {
            float[,] weights = new float[size, size];
            for (int i = 0; i < size; ++i) {
                for (int j = 0; j < size; ++j) {
                    weights[i, j] = 1f;
                }
            }
            return weights;
        }

        public static Dictionary<Vector2, Color> ApplyBrush(Vector2[] points, float[,] weights, int size) {
            var drawnPoints = new Dictionary<Vector2, Color>();

            foreach (Vector2 point in points ) {
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        float weight = weights[i, j];
                        Vector2 drawn = new Vector2(x: point.X + i, y: point.Y + j);
                        drawnPoints[drawn] = Color.Black * weight;
                    }
                }
            }

            return drawnPoints;
        }

        public static Vector2[] GetBresenhamLine(Vector2 start, Vector2 end) {
            var points = new List<Vector2>();

            int x0 = (int)start.X;
            int y0 = (int)start.Y;
            int x1 = (int)end.X;
            int y1 = (int)end.Y;

            int dx = Math.Abs(x1 - x0);
            int sx = (x0 < x1) ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = (y0 < y1) ? 1 : -1;
            int error = dx + dy;

            while (true) {
                points.Add(new Vector2(x: x0, y: y0));

                if (x0 == x1 && y0 == y1) {
                    break;
                }

                int e2 = error * 2;

                if (e2 >= dy) {
                    error += dy;
                    x0 += sx;
                }
                if (e2 <= dx) {
                    error += dx;
                    y0 += sy;
                }
            }

            return points.ToArray();
        }
    }
}