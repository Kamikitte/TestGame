using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Enum;
using TestGame.States;
using TestGame.States.Base;

namespace TestGame
{
    public class MainGame : Game
	{
		private BaseGameState _currentGameState;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		//define target resolution as 640x480
		private RenderTarget2D _renderTarget;
		private Rectangle _renderScaleRectangle;

		private const int DESIGNED_RESOLUTION_WIDTH = 640;
		private const int DESIGNED_RESOLUTION_HEIGHT = 480;

		private const float DESIGNED_RESOLUTION_ASPECT_RATIO =
			DESIGNED_RESOLUTION_WIDTH / (float)DESIGNED_RESOLUTION_HEIGHT;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 1024;
			_graphics.PreferredBackBufferHeight = 768;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();

			_renderTarget = new RenderTarget2D(_graphics.GraphicsDevice,
				DESIGNED_RESOLUTION_HEIGHT, DESIGNED_RESOLUTION_WIDTH,
				false, SurfaceFormat.Color, DepthFormat.None, 0,
				RenderTargetUsage.DiscardContents);
			_renderScaleRectangle = GetScaleRectangle();

			base.Initialize();
		}

		private Rectangle GetScaleRectangle()
		{
			var variance = 0.5;
			var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
			if(actualAspectRatio <= DESIGNED_RESOLUTION_ASPECT_RATIO)
			{
				var presentHeight = (int)(Window.ClientBounds.Width / DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
				var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
				return new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
			}
			else
			{
				var presentWidth = (int)(Window.ClientBounds.Height * DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
				var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
				return new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
			}
		}

		private void SwitchGameState(BaseGameState gameState)
		{
			if (_currentGameState != null)
			{
				_currentGameState.OnStateSwitched -= CurrentGameStateOnStateSwitched;
				_currentGameState.OnEventNotification -= CurrentGameStateOnEventNotification;
				_currentGameState.UnloadContent(Content);
			}
			_currentGameState = gameState;

			_currentGameState.LoadContent(Content);

			_currentGameState.OnStateSwitched += CurrentGameStateOnStateSwitched;
			_currentGameState.OnEventNotification += CurrentGameStateOnEventNotification;
		}

		private void CurrentGameStateOnStateSwitched(object sender, BaseGameState e)
		{
			SwitchGameState(e);
		}

		private void CurrentGameStateOnEventNotification(object sender, Events e)
		{
			switch (e)
			{
				case Events.GAME_QUIT:
					Exit();
					break;
			}
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			SwitchGameState(new SplashState());
		}

		protected override void UnloadContent()
		{
			_currentGameState?.UnloadContent(Content);
		}

		protected override void Update(GameTime gameTime)
		{
			// TODO: Add your update logic here

			_currentGameState.HandleInput();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			//Render to the render target
			GraphicsDevice.SetRenderTarget(_renderTarget);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			_currentGameState.Render(_spriteBatch);

			_spriteBatch.End();

			//Render the scaled content

			_graphics.GraphicsDevice.SetRenderTarget(null);
			_graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

			_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

			_spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}