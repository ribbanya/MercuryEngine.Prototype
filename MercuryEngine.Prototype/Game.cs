using System.Collections.Generic;
using MercuryEngine.Prototype.Components;
using MercuryEngine.Prototype.EntityComponentSystem;
using MercuryEngine.Prototype.Graphics;
using MercuryEngine.Prototype.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MercuryEngine.Prototype {
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game : Microsoft.Xna.Framework.Game {
    // ReSharper disable once NotAccessedField.Local
    private readonly GraphicsDeviceManager graphics;
    private readonly SystemManager updateSystemManager, drawSystemManager;

    public Game() {
      this.graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      this.updateSystemManager = new SystemManager(
        new RudimentaryGravitySystem(),
        new CameraTransformationSystem()
      );

      this.drawSystemManager = new SystemManager(
        new CircleRenderSystem()
      );
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      CreateActors();
      CreateRenderContexts();
      base.Initialize();
    }

    private void CreateRenderContexts() {
      var shapeContext = EntityManager.GetNewEntityId();
      EntityManager.AttachComponent(shapeContext,
        new ShapeRenderContext {
          ShapeBatch = new PrimitiveShapeBatch(GraphicsDevice),
          Effect = new BasicEffect(GraphicsDevice) {
            VertexColorEnabled = true,
            TextureEnabled = false,
          }
        });
    }

    private static void CreateActors() {
      var actor1 = EntityManager.GetNewEntityId();
      var actor2 = EntityManager.GetNewEntityId();
      const float radius = 100;
      {
        var position = EntityManager.GetComponentDictionary<Position>();
        position[actor1] = new Position {X = -radius, Y = 0};
        position[actor2] = new Position {X = radius, Y = 0};
      }
      {
        var circleRenderData = EntityManager.GetComponentDictionary<CircleRenderData>();

        circleRenderData[actor1] = new CircleRenderData {
          Color = Color.Magenta,
          Fill = true,
          Radius = radius,
          Scale = Vector2.One
        };
        circleRenderData[actor2] = new CircleRenderData {
          Color = Color.BlueViolet,
          Fill = true,
          Radius = radius,
          Scale = new Vector2(0.5f, 1.5f)
        };
      }
      {
        EntityManager.AttachComponent<Visible>(actor1);
        EntityManager.AttachComponent<HasGravity>(actor1);
        EntityManager.AttachComponent<Visible>(actor2);
        EntityManager.AttachComponent<HasGravity>(actor2);
      }
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      CreatePlatformInfo();
      CreateCamera();
      GraphicsDevice.BlendState = BlendState.Opaque;
      GraphicsDevice.DepthStencilState = DepthStencilState.Default;
      GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
    }

    private void CreatePlatformInfo() {
      var id = EntityManager.GetNewEntityId();
      EntityManager.AttachComponent(id,
        new PlatformInfo {
          Viewport = this.graphics.GraphicsDevice.Viewport
        }
      );
    }

    public void CreateCamera() {
      var id = EntityManager.GetNewEntityId();
      Vector2 origin;
      {
        var viewport = GraphicsDevice.Viewport;
        var x = (viewport.Width - viewport.X) / 2f;
        var y = (viewport.Height - viewport.Y) / 2f;
        origin = new Vector2(x, y);
      }
      EntityManager.AttachComponent(id,
        new CameraData {
          Origin = origin,
          Rotation = 0,
          Scale = new Vector2(1)
        }
      );
      EntityManager.AttachComponent(id, new Position {X = 0, Y = 0});
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      this.updateSystemManager.Update();
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      this.drawSystemManager.Update();
    }
  }
}