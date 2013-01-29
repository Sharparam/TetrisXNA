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

namespace TetrisXNA
{
	public static class Constants
	{
		public const int BlockWidth = 32;
		public const int BlockHeight = 32;

		public const int BlockAreaOffsetY = 0;
		public const int BlockAreaOffsetX = 32;
		public const int BlockAreaSizeX = 10;
		public const int BlockAreaSizeY = 24;
		public const int BlockAreaWidth = BlockAreaSizeX * BlockWidth;
		public const int BlockAreaHeight = BlockAreaSizeY * BlockHeight;
		public const int BlockAreaNextFieldX = 13;
		public const int BlockAreaNextFieldY = 3;

		public const int StatsOffsetX = 425;
		public const int StatsTimeOffsetY = 274;
		public const int StatsLevelOffsetX = 40;
		public const int StatsLevelOffsetY = 370;
		public const int StatsScoreOffsetY = 465;
		public const int StatsHighScoreOffsetY = 560;

		public const int LineClearPoints = 100;
		public const int UserDropPoints = 1;

		public const int GameOverScoreOffsetX = 320;
		public const int GameOverScoreOffsetY = 340;
	}
}
