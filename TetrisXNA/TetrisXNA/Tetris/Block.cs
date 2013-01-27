using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace TetrisXNA.Tetris
{
	class Block
	{
		public Color Color { get; private set; }
		public Point Position { get; private set; }

		internal Block(Color color, Point? position = null)
		{
			Color = color;
			if (position.HasValue)
				Position = new Point(position.Value.X, position.Value.Y);
		}

		internal void SetPosition(int x, int y)
		{
			Position = new Point(x, y);
		}

		internal void SetColor(Color color)
		{
			Color = color;
		}
	}
}
