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
	class ShapeBuilder
	{
		internal static Color GetShapeColor(ShapeType type)
		{
			Color result;

			switch (type)
			{
				case ShapeType.Bar:
					result = Color.Yellow;
					break;
				case ShapeType.LeftL:
					result = Color.Red;
					break;
				case ShapeType.LeftZ:
					result = Color.Blue;
					break;
				case ShapeType.RightL:
					result = Color.Green;
					break;
				case ShapeType.RightZ:
					result = Color.Cyan;
					break;
				case ShapeType.Square:
					result = Color.Purple;
					break;
				case ShapeType.T:
					result = Color.Pink;
					break;
				default:
					result = Color.White;
					break;
			}

			return result;
		}

		internal static Block[,] BuildShape(ShapeType type)
		{
			var result = new Block[4,4];

			var color = GetShapeColor(type);

			switch (type)
			{
				case ShapeType.Bar:
					/*    0  1  2  3
					 * 0 [X][X][X][X]
					 * 1 [ ][ ][ ][ ]
					 * 2 [ ][ ][ ][ ]
					 * 3 [ ][ ][ ][ ]
					 */
					for (int i = 0; i < 4; i++)
						result[i, 0] = new Block(color);
					break;
				case ShapeType.LeftL:
					/*    0  1  2  3
					 * 0 [ ][ ][ ][X]
					 * 1 [ ][ ][ ][X]
					 * 2 [ ][ ][ ][X]
					 * 3 [ ][ ][X][X]
					 */
					for (int i = 0; i < 4; i++)
						result[3, i] = new Block(color);
					result[2, 3] = new Block(color);
					break;
				case ShapeType.RightL:
					/*    0  1  2  3
					 * 0 [X][ ][ ][ ]
					 * 1 [X][ ][ ][ ]
					 * 2 [X][ ][ ][ ]
					 * 3 [X][X][ ][ ]
					 */
					for (int i = 0; i < 4; i++)
						result[0, i] = new Block(color);
					result[1, 3] = new Block(color);
					break;
				case ShapeType.LeftZ:
					/*    0  1  2  3
					 * 0 [X][X][ ][ ]
					 * 1 [ ][X][X][ ]
					 * 2 [ ][ ][ ][ ]
					 * 3 [ ][ ][ ][ ]
					 */
					result[0, 0] = new Block(color);
					result[1, 0] = new Block(color);
					result[1, 1] = new Block(color);
					result[2, 1] = new Block(color);
					break;
				case ShapeType.RightZ:
					/*    0  1  2  3
					 * 0 [ ][ ][X][X]
					 * 1 [ ][X][X][ ]
					 * 2 [ ][ ][ ][ ]
					 * 3 [ ][ ][ ][ ]
					 */
					result[3, 0] = new Block(color);
					result[2, 0] = new Block(color);
					result[2, 1] = new Block(color);
					result[1, 1] = new Block(color);
					break;
				case ShapeType.Square:
					/*    0  1  2  3
					 * 0 [X][X][ ][ ]
					 * 1 [X][X][ ][ ]
					 * 2 [ ][ ][ ][ ]
					 * 3 [ ][ ][ ][ ]
					 */
					for (int i = 0; i < 1; i++)
					{
						result[i, 0] = new Block(color);
						result[0, i] = new Block(color);
					}
					break;
				case ShapeType.T:
					/*    0  1  2  3
					 * 0 [X][X][X][ ]
					 * 1 [ ][X][ ][ ]
					 * 2 [ ][ ][ ][ ]
					 * 3 [ ][ ][ ][ ]
					 */
					for (int i = 0; i < 2; i++)
						result[i, 0] = new Block(color);
					result[1, 1] = new Block(color);
					break;
				default:
					throw new Exception("Unsupported shape type: " + type);
			}
			
			return result;
		}

		internal static Point GetPivotPoint(ShapeType type)
		{
			Point result;

			switch (type)
			{
				case ShapeType.LeftL:
				case ShapeType.RightZ:
					result = new Point(3, 0);
					break;
				default:
					result = new Point(0, 0);
					break;
			}

			return result;
		}

		internal static Facing GetDefaultFacing(ShapeType type)
		{
			Facing result;

			switch (type)
			{
				case ShapeType.LeftL:
					result = Facing.East;
					break;
				case ShapeType.RightL:
					result = Facing.West;
					break;
				default:
					result = Facing.North;
					break;
			}

			return result;
		}
	}
}
