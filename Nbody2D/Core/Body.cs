using Nbody2D.Extensions;
using SFML.Graphics;
using SFML.System;

namespace NBody2D.Core;

internal class Body
{
	private static CircleShape circleShape = new CircleShape();
	private static float Gravity = 6.0f;
	public Vector2f Position { get; set; }
	public Vector2f Velocity { get; set; }
	public Vector2f Force { get; set; }
	public float Mass { get; set; }
	public Color Color { get; set; }

	public Body(float x, float y, float vx, float vy, float mass, Color color)
	{
		Position = new Vector2f(x, y);
		Velocity = new Vector2f(vx, vy);
		Mass = mass;
		Color = color;
	}

	public Body(Vector2f position, Vector2f velocity, float mass, Color color)
	{
		Position = position;
		Velocity = velocity;
		Mass = mass;
		Color = color;
	}

	public void Update(float deltaTime)
	{
		Velocity += deltaTime * Force / Mass;
		Position += deltaTime * Velocity;
	}

	public float Distance(Body other)
		=> MathF.Sqrt(MathF.Pow((Position - other.Position).X, 2) + MathF.Pow((Position - other.Position).Y, 2));

	public void Attract(Body other)
	{
		var soft = 500;      // softening parameter
		var distance = other.Position - Position;
		var dist = MathF.Sqrt(distance.X * distance.X + distance.Y * distance.Y);
		var force = (Gravity * Mass * other.Mass) / (dist * dist + soft * soft);
		Force += force * distance / dist;
	}

	public void Draw(RenderWindow window)
	{
		circleShape.Radius = Mass.Map(1, 2000, 1.50f, 5);
		circleShape.Position = Position;
		circleShape.FillColor = Color;

		window.Draw(circleShape);
	}

	public static Body operator +(Body left, Body right)
	{
		var newMass = left.Mass + right.Mass;
		var newX = (left.Position.X * left.Mass + right.Position.X * right.Mass) / newMass;
		var newY = (left.Position.Y * left.Mass + right.Position.Y * right.Mass) / newMass;

		return new Body(new Vector2f(newX, newY), new Vector2f(), newMass, left.Color);
	}
}