using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//TODO: Use LilyPath instead
namespace MercuryEngine.Prototype.Graphics {
  public class PrimitiveBatch {
    private const int DefaultBufferSize = 1000;
    private readonly GraphicsDevice device;
    private readonly VertexPositionColor[] vertices = new VertexPositionColor[DefaultBufferSize];
    private bool hasBegun;
    private int numVerticesPerPrimitive;
    private int positionInBuffer;
    private PrimitiveType primitiveType;

    public PrimitiveBatch(GraphicsDevice graphicsDevice) {
      this.device = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
    }

    //TODO: Proper batching of shapes
    public void Begin(PrimitiveType type, Effect effect, BlendState blendState = null) {
      if (this.hasBegun)
        throw new InvalidOperationException("End must be called before Begin can be called again.");
      this.device.BlendState = blendState ?? new BlendState {
        AlphaSourceBlend = Blend.SourceAlpha,
        AlphaBlendFunction = BlendFunction.Add,
        AlphaDestinationBlend = Blend.InverseSourceAlpha
      };
      this.primitiveType = type;
      this.numVerticesPerPrimitive = NumVerticesPerPrimitive(type);
      effect.CurrentTechnique.Passes[0].Apply();
      this.hasBegun = true;
    }

    public void AddVertex(Vector2 vertex, Color color) {
      if (!this.hasBegun)
        throw new InvalidOperationException("Begin must be called before AddVertex can be called.");
      if (this.positionInBuffer % this.numVerticesPerPrimitive == 0 &&
          this.positionInBuffer + this.numVerticesPerPrimitive >= this.vertices.Length)
        Flush();
      this.vertices[this.positionInBuffer].Position = new Vector3(vertex, 0.0f);
      this.vertices[this.positionInBuffer].Color = color;
      ++this.positionInBuffer;
    }

    public void AddVertex(int x, int y, Color color) {
      AddVertex(new Vector2(x, y), color);
    }

    public void AddVertex(float x, float y, Color color) {
      AddVertex(new Vector2(x, y), color);
    }

    public void End() {
      if (!this.hasBegun)
        throw new InvalidOperationException("Begin must be called before End can be called.");
      Flush();
      this.hasBegun = false;
    }

    private void Flush() {
      if (!this.hasBegun)
        throw new InvalidOperationException("Begin must be called before Flush can be called.");
      if (this.positionInBuffer == 0)
        return;
      var primitiveCount = 0;
      switch (this.primitiveType) {
        case PrimitiveType.TriangleList:
          primitiveCount = this.positionInBuffer / 3;
          break;
        case PrimitiveType.TriangleStrip:
          primitiveCount = this.positionInBuffer - 2;
          break;
        case PrimitiveType.LineList:
          primitiveCount = this.positionInBuffer / 2;
          break;
        case PrimitiveType.LineStrip:
          primitiveCount = this.positionInBuffer - 1;
          break;
      }

      this.device.DrawUserPrimitives(this.primitiveType, this.vertices, 0, primitiveCount);
      this.positionInBuffer = 0;
    }

    private static int NumVerticesPerPrimitive(PrimitiveType primitive) {
      switch (primitive) {
        case PrimitiveType.TriangleList:
        case PrimitiveType.TriangleStrip:
          return 3;
        case PrimitiveType.LineList:
        case PrimitiveType.LineStrip:
          return 2;
        default:
          throw new InvalidOperationException("primitive is not valid");
      }
    }
  }
}