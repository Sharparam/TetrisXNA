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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;

namespace TetrisXNA.States
{
	class Menu : DrawableGameState
	{
		private readonly TetrisClone _game;

		private Texture2D _background;

		private readonly Vector2 _drawPos = new Vector2(0.0f, 0.0f);

		internal Menu(TetrisClone game)
		{
			_game = game;
		}

		protected override void OnEntered()
		{
			if (_background == null)
				_background = _game.Content.Load<Texture2D>(@"MenuBackground");
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
