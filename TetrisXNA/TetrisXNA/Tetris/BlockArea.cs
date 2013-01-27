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

namespace TetrisXNA.Tetris
{
	public class BlockArea : IBlockArea
	{
		private readonly Block[,] _blocks;
		private readonly Texture2D _blockTexture;

		internal BlockArea(Texture2D blockTexture)
		{
			_blocks = new Block[Constants.BlockAreaSizeX,Constants.BlockAreaSizeY];
			_blockTexture = blockTexture;
		}

		public bool IsOccupied(int x, int y)
		{
			return _blocks[x, y] != null;
		}

		public bool IsOccupied(Point point)
		{
			return IsOccupied(point.X, point.Y);
		}

		public Vector2 GridToScreenCoordinates(int x, int y)
		{
			float screenX = x * Constants.BlockWidth + Constants.BlockAreaOffsetX;
			float screenY = x * Constants.BlockHeight + Constants.BlockAreaOffsetY;

			return new Vector2(screenX, screenY);
		}

		public Vector2 GridToScreenCoordinates(Point point)
		{
			return GridToScreenCoordinates(point.X, point.Y);
		}

		public void PlaceAt(Block block, int x, int y)
		{
			_blocks[x, y] = block;
		}

		public void PlaceAt(Block block, Point point)
		{
			PlaceAt(block, point.X, point.Y);
		}

		public void Update(GameTime gameTime)
		{
			
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var block in _blocks)
			{
				spriteBatch.Draw(_blockTexture, GridToScreenCoordinates(block.Position.X, block.Position.Y), block.Color);
			}
		}
	}
}
