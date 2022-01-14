namespace Nbody2D.Core
{
	internal struct Vector2
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector2()
		{
			X = 0;
			Y = 0;
		}

		public static Vector2 operator *(double left, Vector2 right)
			=> new(left * right.X, left * right.Y);

		public static Vector2 operator *(Vector2 left, double right)
			=> new(right * left.X, right * left.Y);

		public static Vector2 operator +(Vector2 left, Vector2 right)
			=> new(left.X + right.X, left.Y + right.Y);

		public static Vector2 operator -(Vector2 left, Vector2 right)
			=> new(left.X - right.X, left.Y - right.Y);
	}
}