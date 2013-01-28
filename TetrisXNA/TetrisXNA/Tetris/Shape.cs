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
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TetrisXNA.Tetris
{
	class Shape
	{
		private Block[,] _blocks;
		private Point _pivot;
		private Facing _facing; // This is probably useless

		internal Point Position { get; private set; }

		internal Shape(ShapeType type)
		{
			_blocks = ShapeBuilder.BuildShape(type);
			_pivot = ShapeBuilder.GetPivotPoint(type);
		}

		internal bool HasBlockAt(int blockX, int blockY)
		{
			for (int x = 0; x < 4; x++)
				for (int y = 0; y < 4; y++)
				{
					if (_blocks[x, y] == null)
						continue;
					if (x + Position.X == blockX && y + Position.Y == blockY)
						return true;
				}
			return false;
		}

		internal bool HasBlockAt(Point point)
		{
			return HasBlockAt(point.X, point.Y);
		}

		internal bool Move(Direction direction, IBlockArea blockArea)
		{
			if (direction != Direction.Right && direction != Direction.Left)
				throw new ArgumentException("Only left and right directions are supported", "direction");

			int shift = direction == Direction.Right ? 1 : -1;
			var newPos = new Point(Position.X + shift, Position.Y);
			for (int x = 0; x < 4; x++)
				for (int y = 0; y < 4; y++)
					if (_blocks[x, y] != null &&
					    (x + newPos.X >= Constants.BlockAreaSizeX || x + newPos.X < 0 || blockArea.IsOccupied(x + newPos.X, y + newPos.Y)))
						return false;
			Position = new Point(newPos.X, newPos.Y);
			return true;
		}

		internal bool Drop(IBlockArea blockArea)
		{
			var newPos = new Point(Position.X, Position.Y + 1);
			for (int x = 0; x < 4; x++)
				for (int y = 0; y < 4; y++)
					if (_blocks[x, y] != null &&
						(y + newPos.Y >= Constants.BlockAreaSizeY || blockArea.IsOccupied(x + newPos.X, y + newPos.Y)))
						return false;
			Position = new Point(newPos.X, newPos.Y);
			return true;
		}

		// Thanks to stromdotcom for the rotation and pivot algorithms
		// http://www.stromcode.com/2008/03/23/xna-tetris-in-24-hours/
		internal void Rotate(Direction direction, IBlockArea blockArea)
		{
			if (direction != Direction.Right && direction != Direction.Left)
				throw new ArgumentException("Only left and right directions are supported", "direction");

			var newBlocks = new Block[4,4];
			Point newPivot;
			Facing newFacing;

			if (direction == Direction.Left)
			{
				// Swap block (x,y) with block (y, x - 3);
				for (int x = 0; x < 3; x++)
					for (int y = 0; y < 3; y++)
						newBlocks[x, y] = _blocks[y, x - 3];
				newPivot = new Point(_pivot.Y, 3 - _pivot.X);

				newFacing = _facing - 1;
				if (newFacing == Facing.Min)
					newFacing = Facing.West;
			}
			else // direction == Direction.Right
			{
				// Swap block (x,y) with block (3-y,x)
				for (int x = 0; x < 3; x++)
					for (int y = 0; y < 3; y++)
						newBlocks[x, y] = _blocks[3 - y, x];
				newPivot = new Point(3 - _pivot.Y, _pivot.X);

				newFacing = _facing + 1;
				if (newFacing == Facing.Max)
					newFacing = Facing.North;
			}

			var pivotShift = new Point(_pivot.X - newPivot.X, _pivot.Y - newPivot.Y);

			for (int x = 0; x < 3; x++)
				for (int y = 0; y < 3; y++)
					if (newBlocks[x, y] != null && blockArea.IsOccupied(x, y))
						return;

			// Somewhere here the new position of each block needs to be set

			_blocks = newBlocks;
			_pivot = newPivot;
			_facing = newFacing;
		}

		internal Block[,] GetBlocks()
		{
			var result = new Block[4,4];
			for (int x = 0; x < 4; x++)
				for (int y = 0; y < 4; y++)
					result[x, y] = _blocks[x, y];
			return result;
		}

		internal void SetPosition(int x, int y)
		{
			Position = new Point(x, y);
		}

		internal void SetPosition(Point point)
		{
			SetPosition(point.X, point.Y);
		}
	}
}
