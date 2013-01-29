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

namespace TetrisXNA.Tetris
{
	class Shape
	{
		private Block[,] _blocks;
		private Facing _facing; // This is probably useless

		internal int Size { get; private set; }
		internal ShapeType Type { get; private set; }
		internal Point Position { get; private set; }
		internal Point Pivot { get; private set; }

		internal Shape(ShapeType type)
		{
			Type = type;
			_blocks = ShapeBuilder.BuildShape(type);
			Size = ShapeBuilder.GetShapeSize(type);
			Pivot = ShapeBuilder.GetPivotPoint(type);
		}

		internal bool Move(Direction direction, IBlockArea blockArea)
		{
			if (direction != Direction.Right && direction != Direction.Left)
				throw new ArgumentException("Only left and right directions are supported", "direction");

			int shift = direction == Direction.Right ? 1 : -1;
			var newPos = new Point(Position.X + shift, Position.Y);
			for (int x = 0; x < Size; x++)
				for (int y = 0; y < Size; y++)
					if (_blocks[x, y] != null &&
					    (x + newPos.X >= Constants.BlockAreaSizeX || x + newPos.X < 0 || blockArea.IsOccupied(x + newPos.X, y + newPos.Y)))
						return false;
			Position = new Point(newPos.X, newPos.Y);
			return true;
		}

		internal bool Drop(IBlockArea blockArea)
		{
			var newPos = new Point(Position.X, Position.Y + 1);
			for (int x = 0; x < Size; x++)
				for (int y = 0; y < Size; y++)
					if (_blocks[x, y] != null &&
						(y + newPos.Y >= Constants.BlockAreaSizeY || blockArea.IsOccupied(x + newPos.X, y + newPos.Y)))
						return false;
			Position = new Point(newPos.X, newPos.Y);
			return true;
		}

		// Rotation algorithm from Stackoverflow
		internal bool Rotate(Direction direction, IBlockArea blockArea)
		{
			if (direction != Direction.Right && direction != Direction.Left)
				throw new ArgumentException("Only left and right directions are supported", "direction");

			if (Position.Y < 0)
				return false;

			var newBlocks = new Block[Size,Size];
			Point newPivot;
			Facing newFacing;

			if (direction == Direction.Left)
			{
				for (int x = 0; x < Size; ++x)
					for (int y = 0; y < Size; y++)
						newBlocks[x, y] = _blocks[Size - y - 1, x];

				newPivot = new Point(Pivot.Y, Size - Pivot.X);

				newFacing = _facing - 1;
				if (newFacing == Facing.Min)
					newFacing = Facing.West;
			}
			else // direction == Direction.Right
			{
				for (int x = 0; x < Size; ++x)
					for (int y = 0; y < Size; y++)
						newBlocks[x, y] = _blocks[y, Size - x - 1]; //_blocks[Size - y - 1, x];

				newPivot = new Point(Size - Pivot.Y, Pivot.X);

				newFacing = _facing + 1;
				if (newFacing == Facing.Max)
					newFacing = Facing.North;
			}

			var pivotShift = new Point(Pivot.X - newPivot.X, Pivot.Y - newPivot.Y);

			for (int x = 0; x < Size; x++)
				for (int y = 0; y < Size; y++)
					if (newBlocks[x, y] != null && blockArea.IsOccupied(x + Position.X, y + Position.Y))
						return false;

			// Somewhere here the new position of each block needs to be set
			// disregard above, BlockArea class should handle this

			_blocks = newBlocks;
			Pivot = newPivot;
			_facing = newFacing;

			return true;
		}

		internal Block[,] GetBlocks()
		{
			var result = new Block[Size,Size];
			for (int x = 0; x < Size; x++)
				for (int y = 0; y < Size; y++)
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
