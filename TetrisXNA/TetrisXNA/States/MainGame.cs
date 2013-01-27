using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using TetrisXNA.Tetris;

namespace TetrisXNA.States
{
	public class MainGame : DrawableGameState
	{
		private readonly TetrisClone _game;

		private Texture2D _background;
		private Texture2D _blockTexture;

		private readonly Vector2 _bgPos = new Vector2(0.0f, 0.0f);

		private BlockArea _blockArea;

		internal MainGame(TetrisClone game)
		{
			_game = game;
		}

		protected override void OnEntered()
		{
			if (_background == null)
				_background = _game.Content.Load<Texture2D>(@"GameBackground");

			if (_blockTexture == null)
				_blockTexture = _game.Content.Load<Texture2D>(@"block");

			_blockArea = new BlockArea(_blockTexture);
		}

		protected override void OnLeaving()
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			_blockArea.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			_game.SpriteBatch.Draw(_background, _bgPos, Color.White);
			_blockArea.Draw(_game.SpriteBatch);
		}
	}
}
