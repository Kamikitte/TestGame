using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Objects;
using TestGame.States.Base;

namespace TestGame.States
{
    internal class SplashState : BaseGameState
	{
		public override void HandleInput()
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				SwitchState(new GameplayState());
			}
		}

		public override void LoadContent(ContentManager contentManager)
		{
			AddGameObject(new SplashImage(contentManager.Load<Texture2D>("Barren")));
		}

		public override void UnloadContent(ContentManager contentManager)
		{
			contentManager.Unload();
		}
	}
}
