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
using Microsoft.Xna.Framework.Graphics;

namespace TetrisXNA.Tetris
{
	public class BlockArea : IBlockArea
	{
		private const double ShapeMoveDelay = 0.7;

		private readonly Block[,] _blocks;
		private readonly Texture2D _blockTexture;

		private Shape _currentShape;
		private Block[,] _currentShapeBlocks;
		private Shape _nextShape;
		private Random _random;

		private double _moveTimeElapsed;

		internal BlockArea(Texture2D blockTexture)
		{
			_blocks = new Block[Constants.BlockAreaSizeX,Constants.BlockAreaSizeY];
			_blockTexture = blockTexture;
			_moveTimeElapsed = 0.0f;
			_random = new Random();
		}

		public bool IsOccupied(int x, int y)
		{
			if (x >= Constants.BlockAreaSizeX || y >= Constants.BlockAreaSizeY)
				return true;
			return _blocks[x, y] != null;
		}

		public bool IsOccupied(Point point)
		{
			return IsOccupied(point.X, point.Y);
		}

		public Vector2 GridToScreenCoordinates(int x, int y)
		{
			float screenX = x * Constants.BlockWidth + Constants.BlockAreaOffsetX;
			float screenY = y * Constants.BlockHeight + Constants.BlockAreaOffsetY;

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

		public void RemoveAt(int x, int y)
		{
			_blocks[x, y] = null;
		}

		public void RemoveAt(Point point)
		{
			RemoveAt(point.X, point.Y);
		}

		private Shape GenerateShape()
		{
			return new Shape((ShapeType)_random.Next(0, ((int)ShapeType.Z) + 1));
		}

		public void Update(GameTime gameTime)
		{
			if (_currentShape == null)
			{
				_currentShape = _nextShape ?? GenerateShape();
				_currentShapeBlocks = _currentShape.GetBlocks();
				_nextShape = GenerateShape();
				_moveTimeElapsed = 0.0;
			}

			_moveTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

			if (!(_moveTimeElapsed >= ShapeMoveDelay))
				return;

			_moveTimeElapsed -= ShapeMoveDelay;
			var shapePos = new Point(_currentShape.Position.X, _currentShape.Position.Y);
			if (_currentShape.Drop(this))
			{
				var newShapePos = new Point(_currentShape.Position.X, _currentShape.Position.Y);
				for (int x = 0; x < 4; x++)
					for (int y = 0; y < 4; y++)
					{
						var block = _currentShapeBlocks[x, y];
						if (block == null)
							continue;
						RemoveAt(x + shapePos.X, y + shapePos.Y);
						PlaceAt(block, x + newShapePos.X, y + newShapePos.Y);
					}
			}
			else
			{
				Console.WriteLine("Collided");
				for (int x = 0; x < 4; x++)
					for (int y = 0; y < 4; y++)
					{
						var block = _currentShapeBlocks[x, y];
						if (block == null)
							continue;
						PlaceAt(block, x + shapePos.X, y + shapePos.Y);
					}
				_currentShape = null;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int x = 0; x < Constants.BlockAreaSizeX; x++)
				for (int y = 0; y < Constants.BlockAreaSizeY; y++)
					if (_blocks[x, y] != null)
						spriteBatch.Draw(_blockTexture, GridToScreenCoordinates(x, y), _blocks[x, y].Color);
		}
	}
}
