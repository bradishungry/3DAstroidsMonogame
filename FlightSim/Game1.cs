	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	using BEPUphysics;
	using BEPUphysics.Entities.Prefabs;

	using EmptyKeys.UserInterface;
	using EmptyKeys.UserInterface.Controls;
	using EmptyKeys.UserInterface.Debug;
	using EmptyKeys.UserInterface.Generated;
	//using EmptyKeys.UserInterface.Input;
	using EmptyKeys.UserInterface.Media;

	namespace FlightSim
	{
		/// <summary>
		/// This is the main type for your game.
		/// </summary>
		public class Game1 : Game
		{
			//Graphics
			GraphicsDeviceManager graphics;

			//Physics
			//public Space space;
			private int nativeScreenWidth;
			private int nativeScreenHeight;
			


			//Game objects
			Camera camera;
			Ship ship;
			Skybox skybox;
			Meteors meteors;
			Weapons weapons;
			public static Space space;


			public Game1()
			{
				graphics = new GraphicsDeviceManager(this);
				graphics.IsFullScreen = true;
				Content.RootDirectory = "Content";
			}

			/// <summary>
			/// Allows the game to perform any initialization it needs to before starting to run.
			/// This is where it can query for any required services and load any non-graphic
			/// related content.  Calling base.Initialize will enumerate through any components
			/// and initialize them as well.
			/// </summary>
			protected override void Initialize()
			{
				// TODO: Add your initialization logic here

				skybox = new Skybox();
				skybox.Initialize(Content);

				ship = new Ship();
				ship.Initialize(Content);

				meteors = new Meteors();
				meteors.setMeteors(10);
				meteors.Initialize(Content);

				weapons = new Weapons();
				weapons.Initialize(Content);

				camera = new Camera(graphics.GraphicsDevice);

				base.Initialize();
			}

			/// <summary>
			/// LoadContent will be called once per game and is the place to load
			/// all of your content.
			/// </summary>
			protected override void LoadContent()
			{
				Viewport viewport = GraphicsDevice.Viewport;
				//SpriteFont font = Content.Load<SpriteFont>("Segoe_UI_15_Bold");
				//FontManager.DefaultFont = Engine.Instance.Renderer.CreateFont(font);
				//root = new UIRoot();
			    //FontManager.Instance.LoadFonts(Content);

				space = new Space();
				meteors.addToSpace(space);
				skybox.addToSpace(space);
				ship.addToSpace(space);
				weapons.setSpace(space);
			}

			/// <summary>
			/// Allows the game to run logic such as updating the world,
			/// checking for collisions, gathering input, and playing audio.
			/// </summary>
			/// <param name="gameTime">Provides a snapshot of timing values.</param>
			protected override void Update(GameTime gameTime)
			{
				// For Mobile devices, this logic will close the Game when the Back button is pressed
				// Exit() is obsolete on iOS
	#if !__IOS__ && !__TVOS__
				if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					Exit();
	#endif

				meteors.update();

				if (ship.game_over == false)
				{
					ship.update();
				}

				camera.Update(gameTime, ship.getLocation(), ship.getWorldMatrix());

				skybox.update();

				weapons.update(ship.getLocation(), ship.getWorldMatrix().Forward);

				space.Update();

				base.Update(gameTime);
			}

			/// <summary>
			/// This is called when the game should draw itself.
			/// </summary>
			/// <param name="gameTime">Provides a snapshot of timing values.</param>
			protected override void Draw(GameTime gameTime)
			{
				graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
				
				GraphicsDevice.RasterizerState = RasterizerState.CullNone;
				skybox.draw(camera);
				GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

				ship.draw(camera);
				meteors.draw(camera);
				weapons.draw(camera);
				base.Draw(gameTime);
			}
		}
	}