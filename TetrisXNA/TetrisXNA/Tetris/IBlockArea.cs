using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisXNA.Tetris
{
	interface IBlockArea
	{
		int GetWidth();
		int GetHeight();
		bool IsOccupied(int x, int y);
	}
}
