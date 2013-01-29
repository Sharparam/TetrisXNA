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
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TetrisXNA.Tetris.Events;

namespace TetrisXNA.Tetris
{
	public class BlockArea : IBlockArea
	{
		public event UserDropEventHandler UserDrop;
		public event LineClearedEventHandler LineCleared;
		public event GameOverEventHandler GameOver;

		private const double ShapeMoveDelay = 0.7;

		private readonly Block[,] _blocks;
		private readonly Texture2D _blockTexture;

		private Shape _currentShape;
		private Block[,] _currentShapeBlocks;
		private Shape _nextShape;
		private Block[,] _nextShapeBlocks;
		private Random _random;

		private double _moveTimeElapsed;
		private bool _gameOver;

		internal BlockArea(Texture2D blockTexture)
		{
			_blocks = new Block[Constants.BlockAreaSizeX,Constants.BlockAreaSizeY];
			_blockTexture = blockTexture;
			_moveTimeElapsed = 0.0f;
			_random = new Random();
		}

		private void OnUserDrop()
		{
			var func = UserDrop;
			if (func != null)
				func(this, null);
		}

		private void OnLineCleared()
		{
			var func = LineCleared;
			if (func != null)
				func(this, null);
		}

		private void OnGameOver()
		{
			var func = GameOver;
			if (func != null)
				func(this, null);
		}

		public bool IsOutOfRange(int x, int y)
		{
			return x < 0 || x >= Constants.BlockAreaSizeX || y < 0 || y >= Constants.BlockAreaSizeY;
		}

		public bool IsOccupied(int x, int y)
		{
			if (IsOutOfRange(x, y))
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
			if (IsOutOfRange(x, y))
				return;

			_blocks[x, y] = block;
		}

		public void PlaceAt(Block block, Point point)
		{
			PlaceAt(block, point.X, point.Y);
		}

		public void RemoveAt(int x, int y)
		{
			if (IsOutOfRange(x, y))
				return;

			_blocks[x, y] = null;
		}

		public void RemoveAt(Point point)
		{
			RemoveAt(point.X, point.Y);
		}

		private Shape GenerateShape()
		{
			var shape = new Shape((ShapeType)_random.Next(0, ((int)ShapeType.Z) + 1));
			int xOffset = -2;
			if (shape.Type == ShapeType.O)
				xOffset = -1;
			shape.SetPosition(Constants.BlockAreaSizeX / 2 + xOffset, 0 - shape.Pivot.Y);
			return shape;
		}

		public void Update(GameTime gameTime)
		{
			if (_gameOver)
				return;

			if (_currentShape == null)
			{
				_currentShape = _nextShape ?? GenerateShape();
				_currentShapeBlocks = _currentShape.GetBlocks();
				_nextShape = GenerateShape();
				_nextShapeBlocks = _nextShape.GetBlocks();
				_moveTimeElapsed = 0.0;
			}

			if (InputHandler.KeyPressed(Keys.Right))
				_currentShape.Move(Direction.Right, this);
			else if (InputHandler.KeyPressed(Keys.Left))
				_currentShape.Move(Direction.Left, this);
			else if (InputHandler.KeyPressed(Keys.Up))
				if (_currentShape.Rotate(Direction.Right, this))
					_currentShapeBlocks = _currentShape.GetBlocks();

			_moveTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

			var userDrop = InputHandler.KeyDown(Keys.Down);

			if (!(_moveTimeElapsed >= ShapeMoveDelay) && !userDrop)
				return;
			
			if (_moveTimeElapsed >= ShapeMoveDelay)
				_moveTimeElapsed -= ShapeMoveDelay;
			
			var shapePos = new Point(_currentShape.Position.X, _currentShape.Position.Y);
			if (!_currentShape.Drop(this))
			{
				for (int x = 0; x < _currentShape.Size; x++)
					for (int y = 0; y < _currentShape.Size; y++)
					{
						var block = _currentShapeBlocks[x, y];
						if (block == null)
							continue;
						PlaceAt(block, x + shapePos.X, y + shapePos.Y);
					}
				_currentShape = null;
			}
			else if (userDrop)
				OnUserDrop();

			// Check for filled rows
			var rows = new Queue<int>(); // Queue: first-in -> first-out
			// Loop through all rows, top to bottom, adding any full rows to the queue
			for (int y = 0; y < Constants.BlockAreaSizeY; y++)
				for (int x = 0; x < Constants.BlockAreaSizeX; x++)
				{
					if (_blocks[x, y] == null)
						break;
					if (x == Constants.BlockAreaSizeX - 1)
						rows.Enqueue(y);
				}
			
			while (rows.Count > 0)
			{
				var row = rows.Dequeue();
				for (int x = 0; x < Constants.BlockAreaSizeX; x++)
					RemoveAt(x, row);

				for (int y = row - 1; y > 0; y--)
					for (int x = 0; x < Constants.BlockAreaSizeX; x++)
						if (_blocks[x, y] != null)
						{
							PlaceAt(_blocks[x, y], x, y + 1);
							RemoveAt(x, y);
						}

				OnLineCleared();
			}

			for (int x = 0; x < Constants.BlockAreaSizeX; x++)
				if (_blocks[x, 0] != null)
				{
					_gameOver = true;
					OnGameOver();
				}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the fixed blocks
			for (int x = 0; x < Constants.BlockAreaSizeX; x++)
				for (int y = 0; y < Constants.BlockAreaSizeY; y++)
					if (_blocks[x, y] != null)
						spriteBatch.Draw(_blockTexture, GridToScreenCoordinates(x, y), _blocks[x, y].Color);

			// Draw the active shape
			if (_currentShape != null && _nextShapeBlocks != null)
				for (int x = 0; x < _currentShape.Size; x++)
					for (int y = 0; y < _currentShape.Size; y++)
						if (_currentShapeBlocks[x, y] != null)
							spriteBatch.Draw(_blockTexture,
								GridToScreenCoordinates(x + _currentShape.Position.X, y + _currentShape.Position.Y),
								_currentShapeBlocks[x, y].Color);
			
			// Draw the next shape in the upper right
			if (_nextShape != null && _nextShapeBlocks != null)
				for (int x = 0; x < _nextShape.Size; x++)
					for (int y = 0; y < _nextShape.Size; y++)
						if (_nextShapeBlocks[x, y] != null)
							spriteBatch.Draw(_blockTexture,
								GridToScreenCoordinates(x + Constants.BlockAreaNextFieldX, y + Constants.BlockAreaNextFieldY - (_nextShape.Type == ShapeType.T ? 1 : _nextShape.Pivot.Y)),
								_nextShapeBlocks[x, y].Color);
		}
	}
}
