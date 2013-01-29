/* Copyright © 2013 by Adam Hellberg
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Nuclex.Game.States;

namespace TetrisXNA.States
{
	internal class GameOver : DrawableGameState
	{
		private const string ScoreFormat = "Your score was {0}!";

		private Texture2D _background;

		private int _score;
		private Vector2 _scorePos = new Vector2(Constants.GameOverScoreOffsetX, Constants.GameOverScoreOffsetY);
		private string _scoreString;

		private readonly TetrisClone _game;
		private readonly Vector2 _drawPos = new Vector2(0.0f, 0.0f);

		internal GameOver(TetrisClone game)
		{
			_game = game;
		}
		
		internal void SetScore(int score)
		{
			_score = score;
			_scoreString = string.Format(ScoreFormat, _score);
			_scorePos.X = Constants.GameOverScoreOffsetX - (int) _game.GameFont.MeasureString(_scoreString).X / 2;
		}

		protected override void OnEntered()
		{
			if (_background == null)
				_background = _game.Content.Load<Texture2D>(@"GameOverBackground");

			MediaPlayer.Volume = 0.1f;
		}

		protected override void OnLeaving()
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			if (InputHandler.KeyPressed(Keys.Enter))
				_game.StateManager.Switch(_game.MenuState);
		}

		public override void Draw(GameTime gameTime)
		{
			_game.SpriteBatch.Draw(_background, _drawPos, Color.White);
			_game.SpriteBatch.DrawString(_game.GameFont, _scoreString, _scorePos, Color.Black);
		}
	}
}
