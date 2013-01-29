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

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using TetrisXNA.Components;
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
		private StatsOverlay _statsOverlay;

		private Cue _themeCue;

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
			_blockArea.UserDrop += OnUserDrop;
			_blockArea.LineCleared += OnLineClear;
			_blockArea.GameOver += OnGameOver;

			_statsOverlay = new StatsOverlay(0, _game.GameFont);

			if (_themeCue == null)
				_themeCue = _game.SoundBank.GetCue("theme");
			_themeCue.Play();
		}

		protected override void OnLeaving()
		{
			_blockArea.UserDrop -= OnUserDrop;
			_blockArea.LineCleared -= OnLineClear;
			_blockArea.GameOver -= OnGameOver;

			_themeCue.Stop(AudioStopOptions.Immediate);
		}

		public override void Update(GameTime gameTime)
		{
			_blockArea.Update(gameTime);
			_statsOverlay.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			_game.SpriteBatch.Draw(_background, _bgPos, Color.White);
			_blockArea.Draw(_game.SpriteBatch);
			_statsOverlay.Draw(_game.SpriteBatch);
		}

		private void OnUserDrop(object sender, EventArgs e)
		{
			_statsOverlay.AddScore(Constants.UserDropPoints);
		}

		private void OnLineClear(object sender, EventArgs e)
		{
			_statsOverlay.AddScore(Constants.LineClearPoints);
		}

		private void OnGameOver(object sender, EventArgs e)
		{
			_game.GameOverState.SetScore(_statsOverlay.Score);
			_game.StateManager.Switch(_game.GameOverState);
		}
	}
}
