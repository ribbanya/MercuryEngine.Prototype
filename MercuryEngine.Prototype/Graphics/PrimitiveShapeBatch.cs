using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MercuryEngine.Prototype.Graphics {
  public class PrimitiveShapeBatch : PrimitiveBatch {
    private const int FilledCircleSides = 663, UnfilledCircleSides = 997;
    public PrimitiveShapeBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

    public void DrawRectangle(
      float left, float bottom, float right, float top, Color color, bool fill, Effect effect,
      BlendState blendState = null) {
      if (fill) {
        Begin(PrimitiveType.TriangleStrip, effect, blendState);
        AddVertex(right, top, color);
        AddVertex(left, top, color);
        AddVertex(right, bottom, color);
        AddVertex(left, bottom, color);
        End();
      }
      else {
        Begin(PrimitiveType.LineList, effect, blendState);

        AddVertex(left, top, color);
        AddVertex(right, top, color);

        AddVertex(right, top, color);
        AddVertex(right, bottom, color);

        AddVertex(right, bottom, color);
        AddVertex(left, bottom, color);

        AddVertex(left, bottom, color);
        AddVertex(left, top, color);


        End();
      }
    }

    public void DrawRectangle(Rectangle rectangle, Color color, bool fill, Effect effect) {
      DrawRectangle(rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Top, color, fill, effect);
    }

    public void DrawScalableCircle(
      Vector2 center, float radius, Color color, bool fill,
      Vector2 scale, Effect effect) {
      DrawRegularPolygon(center,
        radius,
        fill ? FilledCircleSides : UnfilledCircleSides,
        color,
        fill, 0, scale, effect);
    }

    public void DrawRegularPolygon(
      Vector2 center, float radius, int numSides,
      Color color, bool fill, double rotation, Vector2 scale, Effect effect,
      BlendState blendState = null) {
      if (numSides < 2) throw new ArgumentOutOfRangeException(nameof(numSides));
      if (fill && numSides < 3) return;
      Begin(fill ? PrimitiveType.TriangleStrip : PrimitiveType.LineStrip, effect, blendState);
      var index = numSides;
      for (var count = 0;; count++) {
        var phi = index * Math.PI / numSides + rotation;
        var (x, y) = center;
        var (sx, sy) = scale;
        AddVertex((float) (x + radius * Math.Cos(phi) * sx),
          (float) (y - radius * Math.Sin(phi) * sy), color);
        if (count >= numSides) break;
        if (fill) {
          if (count > 0) index = -index;
          if (index > 0) index -= 2;
        }
        else {
          index -= 2;
        }
      }

      End();
    }
  }
}