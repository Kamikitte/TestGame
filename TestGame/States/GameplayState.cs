using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TestGame.Enum;
using TestGame.States.Base;

namespace TestGame.States
{
    internal class GameplayState : BaseGameState
	{
		public override void HandleInput()
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				NotifyEvent(Events.GAME_QUIT);
			}
		}

		public override void LoadContent(ContentManager contentManager)
		{
		}

		public override void UnloadContent(ContentManager contentManager)
		{
			contentManager.Unload();
		}
	}
}
