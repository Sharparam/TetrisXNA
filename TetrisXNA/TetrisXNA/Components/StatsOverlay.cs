﻿/* Copyright © 2013 by Adam Hellberg
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

namespace TetrisXNA.Components
{
	internal class StatsOverlay
	{
		private const string TimeFormat = "{0:00}m {1:00}s";
		private const string LevelFormat = "{0:000}";
		private const string ScoreFormat = "{0:0000000}";

		private TimeSpan _time;

		private readonly SpriteFont _font;
		private readonly Vector2 _timePosition;
		private readonly Vector2 _levelPosition;
		private readonly Vector2 _scorePosition;
		private readonly Vector2 _highScorePosition;
		private readonly Color _textColor = Color.Black;

		internal int Level { get; private set; }
		internal int Score { get; private set; }
		internal int HighScore { get; private set; }

		internal StatsOverlay(int highScore, SpriteFont font)
		{
			Level = 0;
			Score = 0;
			HighScore = highScore;
			_time = new TimeSpan(0);
			_font = font;

			_timePosition = new Vector2(Constants.StatsOffsetX, Constants.StatsTimeOffsetY);
			_levelPosition = new Vector2(Constants.StatsOffsetX + Constants.StatsLevelOffsetX, Constants.StatsLevelOffsetY);
			_scorePosition = new Vector2(Constants.StatsOffsetX, Constants.StatsScoreOffsetY);
			_highScorePosition = new Vector2(Constants.StatsOffsetX, Constants.StatsHighScoreOffsetY);
		}

		internal void AddLevel(int levels)
		{
			Level += levels;
		}

		internal void AddScore(int score)
		{
			Score += score;
			if (Score > HighScore)
				UpdateHighScore(Score);
		}

		private void UpdateHighScore(int score)
		{
			HighScore = score;
		}

		public void Update(GameTime gameTime)
		{
			_time += gameTime.ElapsedGameTime;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(_font, string.Format(TimeFormat, Math.Floor(_time.TotalMinutes), _time.Seconds), _timePosition, _textColor);
			spriteBatch.DrawString(_font, string.Format(LevelFormat, Level), _levelPosition, _textColor);
			spriteBatch.DrawString(_font, string.Format(ScoreFormat, Score), _scorePosition, _textColor);
			spriteBatch.DrawString(_font, string.Format(ScoreFormat, HighScore), _highScorePosition, _textColor);
		}
	}
}
