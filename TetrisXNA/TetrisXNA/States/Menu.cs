using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;

namespace TetrisXNA.States
{
	class Menu : DrawableGameState
	{
		private readonly TetrisClone _game;
		private readonly GraphicsDevice _graphics;
		private SpriteBatch _spriteBatch;
		private ContentManager _content;

		private Texture2D _background;

		private readonly Vector2 _drawPos = new Vector2(0.0f, 0.0f);

		internal Menu(TetrisClone game)
		{
			_game = game;
			_graphics = _game.GraphicsDevice;
			_spriteBatch = _game.SpriteBatch;
			_content = _game.Content;
		}

		protected override void OnEntered()
		{
			if (_spriteBatch == null)
				_spriteBatch = _game.SpriteBatch;

			if (_content == null)
				_content = _game.Content;

			if (_background == null)
				_background = _content.Load<Texture2D>(@"MenuBackground");
		}

		protected override void OnLeaving()
		{
			
		}

		public override void Draw(GameTime gameTime)
		{
			_game.SpriteBatch.Draw(_background, _drawPos, Color.White);
		}

		public override void Update(GameTime gameTime)
		{
			
		}
	}
}
