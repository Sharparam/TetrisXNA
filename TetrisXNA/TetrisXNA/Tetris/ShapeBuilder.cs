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
				case ShapeType.I:
					result = Color.Cyan;
					break;
				case ShapeType.J:
					result = Color.Blue;
					break;
				case ShapeType.Z:
					result = Color.Red;
					break;
				case ShapeType.L:
					result = Color.Orange;
					break;
				case ShapeType.S:
					result = Color.Green;
					break;
				case ShapeType.O:
					result = Color.Yellow;
					break;
				case ShapeType.T:
					result = Color.Purple;
					break;
				default:
					result = Color.White;
					break;
			}

			return result;
		}

		internal static Block[,] BuildShape(ShapeType type)
		{
			var size = GetShapeSize(type);
			var result = new Block[size,size];

			var color = GetShapeColor(type);

			switch (type)
			{
				case ShapeType.I:
					/*    0  1  2  3
					 * 0 [ ][ ][ ][ ]
					 * 1 [X][X][X][X]
					 * 2 [ ][ ][ ][ ]
					 * 3 [ ][ ][ ][ ]
					 */
					for (int i = 0; i < 4; i++)
						result[i, 1] = new Block(color);
					break;
				case ShapeType.J:
					/*    0  1  2
					 * 0 [X][ ][ ]
					 * 1 [X][X][X]
					 * 2 [ ][ ][ ]
					 */
					for (int i = 0; i < 3; i++)
						result[i, 1] = new Block(color);
					result[0, 0] = new Block(color);
					break;
				case ShapeType.L:
					/*    0  1  2
					 * 0 [ ][ ][X]
					 * 1 [X][X][X]
					 * 2 [ ][ ][ ]
					 */
					for (int i = 0; i < 3; i++)
						result[i, 1] = new Block(color);
					result[2, 0] = new Block(color);
					break;
				case ShapeType.Z:
					/*    0  1  2
					 * 0 [X][X][ ]
					 * 1 [ ][X][X]
					 * 2 [ ][ ][ ]
					 */
					result[0, 0] = new Block(color);
					result[1, 0] = new Block(color);
					result[1, 1] = new Block(color);
					result[2, 1] = new Block(color);
					break;
				case ShapeType.S:
					/*    0  1  2
					 * 0 [ ][X][X]
					 * 1 [X][X][ ]
					 * 2 [ ][ ][ ]
					 */
					result[1, 0] = new Block(color);
					result[2, 0] = new Block(color);
					result[0, 1] = new Block(color);
					result[1, 1] = new Block(color);
					break;
				case ShapeType.O:
					/*    0  1
					 * 0 [X][X]
					 * 1 [X][X]
					 */
					result[0, 0] = new Block(color);
					result[1, 0] = new Block(color);
					result[0, 1] = new Block(color);
					result[1, 1] = new Block(color);
					break;
				case ShapeType.T:
					/*    0  1  2
					 * 0 [ ][X][ ]
					 * 1 [X][X][X]
					 * 2 [ ][ ][ ]
					 */
					for (int i = 0; i < 3; i++)
						result[i, 1] = new Block(color);
					result[1, 0] = new Block(color);
					break;
				default:
					throw new Exception("Unsupported shape type: " + type);
			}
			
			return result;
		}

		internal static int GetShapeSize(ShapeType type)
		{
			int result;
			switch (type)
			{
				case ShapeType.I:
					result = 4;
					break;
				case ShapeType.O:
					result = 2;
					break;
				default:
					result = 3;
					break;
			}
			return result;
		}
	}
}
