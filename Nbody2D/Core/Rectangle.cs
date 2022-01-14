using SFML.System;

namespace NBody2D.Core
{
	internal class Rectangle
	{
		public float Height { get; set; }
		public float Width { get; set; }
		public Vector2f Position { get; set; }

		public Rectangle(float width, float height, float x, float y)
		{
			Height = height;
			Width = width;
			Position = new Vector2f(x, y);
		}

		public Rectangle()
		{
			Height = 0.0f;
			Width = 0.0f;
			Position = new Vector2f();
		}

		public bool Contains(Body point)
			=> point.Position.X >= Position.X && point.Position.X <= Position.X + Width
			&& point.Position.Y >= Position.Y && point.Position.Y <= Position.Y + Height;

		public bool Intersects(Rectangle other)
		=> !(other.Position.X > Position.X + Width
		|| other.Position.X + other.Width < Position.X
		|| other.Position.Y > Position.Y + Height
		|| other.Position.Y + other.Height < Position.Y);
	}
}