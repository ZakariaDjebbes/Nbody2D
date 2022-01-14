using NBody2D.Core;
using NBody2D.UI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Net.NetworkInformation;

namespace NBody2D;

internal class Program : UserWindow
{
	private QuadTree? qt;
	private string fileName = "Universe/galaxy.txt";
	private float updateTime = 0.1f, timer = 0.0f;
	private int numberOfBodies;
	private float scale = 1000;

	private List<Body> bodies;
	private RectangleShape rectangleShape;
	private bool showBounds;
	private bool showCentersOfMass;

	public Program(uint height, uint width, string title) : base(height, width, title)
	{
	}

	protected override void OnStart()
	{
		Window.MouseButtonReleased += Window_MouseButtonReleased;
		Window.KeyReleased += Window_KeyReleased;

		//var lines = File.ReadAllLines(fileName);
		//var split = lines[0].Split();
		//numberOfBodies = int.Parse(split[0]);
		//scale = float.Parse(split[1]);
		//bodies = new List<Body>(numberOfBodies);

		//for (int i = 0; i < numberOfBodies; i++)
		//{
		//	split = lines[i + 1].Split();
		//	var x = float.Parse(split[0]);
		//	var y = float.Parse(split[1]);
		//	var vx = float.Parse(split[2]);
		//	var vy = float.Parse(split[3]);
		//	var mass = float.Parse(split[4]);
		//	var red = byte.Parse(split[5]);
		//	var green = byte.Parse(split[6]);
		//	var blue = byte.Parse(split[7]);

		//	Color color = new Color(red, green, blue);
		//	bodies.Add(new Body(x, y, vx, vy, mass, color));
		//}

		bodies = new List<Body>(numberOfBodies);

		for (int i = 0; i < 500; i++)
		{
			bodies.Add(new Body(new Random().Next(0, 1000), new Random().Next(0, 1000), new Random().Next(-5, 5), new Random().Next(-5, 5), 50, new Color(255, 0, 0)));
		}

		rectangleShape = new RectangleShape
		{
			FillColor = Color.Transparent,
			OutlineColor = Color.Cyan,
			OutlineThickness = 1
		};
	}

	protected override void OnUpdate(float deltaTime)
	{
		Window.Clear();

		qt = new QuadTree(new Rectangle(scale, scale, 0, 0));

		foreach (var body in bodies)
		{
			qt.Insert(body);
		}

		qt.Foreach((me) =>
		{
			if (showBounds)
			{
				rectangleShape.Position = me.Bounds.Position;
				rectangleShape.Size = new Vector2f(me.Bounds.Width, me.Bounds.Height);

				Window.Draw(rectangleShape);
			}

			if (me.Body != null)
			{
				if (me.IsExternal())
					me.Body.Draw(Window, scale);
				else
				{
					me.Body.Color = Color.Yellow;
					if (showCentersOfMass)
						me.Body.Draw(Window, scale);
				}
			}
		});

		if (timer <= 0.0f)
		{
			foreach (var body in bodies)
			{
				body.Force = new Vector2f(0, 0);
				qt.UpdateForce(body);
				body.Update(updateTime);
			}

			timer = updateTime;
		}
		else
			timer -= deltaTime;
	}

	private void Window_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
	{
		Console.WriteLine($"Inserting X: {e.X} | Y: {e.Y}");
		switch (e.Button)
		{
			case Mouse.Button.Left:
				var body = new Body(e.X, e.Y, 0.0f, 0.0f, 2.5f, Color.Blue);
				bodies.Add(body);
				break;
		}
	}

	private void Window_KeyReleased(object? sender, KeyEventArgs e)
	{
		switch (e.Code)
		{
			case Keyboard.Key.D:
				showBounds = !showBounds;
				break;

			case Keyboard.Key.M:
				showCentersOfMass = !showCentersOfMass;
				break;

			case Keyboard.Key.R:
				bodies = new List<Body>();
				break;
		}
	}

	private static void Main(string[] args)
	{
		Program program = new(1000, 1000, "NBody 2D");
		program.Run();
	}
}