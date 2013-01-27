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

using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TetrisXNA.Tetris
{
	class Shape
	{
		private Block[,] _blocks;
		private Point _pivot;
		private Facing _facing; // This is probably useless

		internal Shape(ShapeType type)
		{
			_blocks = ShapeBuilder.BuildShape(type);
			_pivot = ShapeBuilder.GetPivotPoint(type);
		}

		// Thanks to stromdotcom for the rotation and pivot algorithms
		// http://www.stromcode.com/2008/03/23/xna-tetris-in-24-hours/
		internal void Rotate(Direction direction, IBlockArea blockArea)
		{
			Debug.Assert(direction != Direction.Up && direction != Direction.Down, "Direction can only be right or left");

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
		}
	}
}
