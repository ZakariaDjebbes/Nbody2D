using Nbody2D.Extensions;
using NBody2D.Core;
using NBody2D.UI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace NBody2D;

internal class Nbody : UserWindow
{
	private QuadTree? qt;
	private readonly string universePath = "Universe/spiral.txt";
	private readonly string colorsPath = "Universe/colors.txt";
	private readonly Dictionary<string, Color> colorMap = new();
	private float updateTime = 1.0f;
	private float elpasedTime = 0.0f;
	private float scale;
	private readonly Font arial = new("Fonts/arial.ttf");
	private List<Body> bodies;
	private RectangleShape rectangleShape;
	private bool showBounds = false;
	private bool showCentersOfMass = false;
	private bool update = false;

	public Nbody(uint height, uint width, string title) : base(height, width, title)
	{
	}

	protected override void OnStart()
	{
		Window.MouseButtonReleased += Window_MouseButtonReleased;
		Window.KeyReleased += Window_KeyReleased;

		ParseColors();
		ParseUniverse();

		rectangleShape = new RectangleShape
		{
			FillColor = Color.Transparent,
			OutlineColor = colorMap["Yellow"],
			OutlineThickness = 1
		};
	}

	protected override void OnUpdate(float deltaTime)
	{
		Window.Clear(colorMap["Black"]);
		DrawUI();

		qt = new QuadTree(new Rectangle(Window.Size.X, Window.Size.Y, 0, 0));

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
					me.Body.Draw(Window);
				else
				{
					me.Body.Color = colorMap["Brown"];
					if (showCentersOfMass)
						me.Body.Draw(Window);
				}
			}
		});
		if (update)
		{
			foreach (var body in bodies)
			{
				body.Force = new Vector2f(0, 0);
				qt.UpdateForce(body);
				body.Update(updateTime);
			}

			elpasedTime += deltaTime;
		}
	}

	private void DrawUI()
	{
		Text bodiesText = new($" {(update ? "Running" : "Paused")}", arial)
		{
			CharacterSize = 20,
			FillColor = update ? colorMap["Green"] : colorMap["Red"]
		};

		Text fileText = new($" Current File: {universePath} | Bodies: {bodies?.Count}", arial)
		{
			CharacterSize = 20,
			FillColor = colorMap["Green"],
			Position = new Vector2f(bodiesText.Position.X, bodiesText.Position.Y + 26)
		};

		Text timeText = new($" Delta time: {updateTime:0.##}s | Elapsed : {elpasedTime:0.##}s", arial)
		{
			CharacterSize = 20,
			FillColor = colorMap["Green"],
			Position = new Vector2f(bodiesText.Position.X, fileText.Position.Y + 26)
		};

		Window.Draw(bodiesText);
		Window.Draw(fileText);
		Window.Draw(timeText);
	}

	private void ParseUniverse()
	{
		var lines = File.ReadAllLines(universePath);
		var split = lines[0].Split();
		var numberOfBodies = int.Parse(split[0]);
		scale = float.Parse(split[1]);
		bodies = new List<Body>(numberOfBodies);

		for (int i = 0; i < numberOfBodies; i++)
		{
			split = lines[i + 1].Split();
			var x = float.Parse(split[0]).Map(-scale, scale, 0, Window.Size.X);
			var y = float.Parse(split[1]).Map(-scale, scale, 0, Window.Size.Y);
			var vx = float.Parse(split[2]).Map(-scale, scale, -5, 5);
			var vy = float.Parse(split[3]).Map(-scale, scale, -5, 5);
			var mass = float.Parse(split[4]);
			var color = split[5];

			bodies.Add(new Body(x, y, vx, vy, mass, colorMap[color]));
		}
	}

	private void ParseColors()
	{
		var lines = File.ReadAllLines(colorsPath);

		for (int i = 0; i < lines.Length; i++)
		{
			var split = lines[i].Split();
			var colorName = split[0];
			var red = byte.Parse(split[1]);
			var green = byte.Parse(split[2]);
			var blue = byte.Parse(split[3]);

			Color color = new(red, green, blue);
			colorMap.Add(colorName, color);
		}
	}

	private void Window_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
	{
		Console.WriteLine($"Inserting X: {e.X} | Y: {e.Y}");
		switch (e.Button)
		{
			case Mouse.Button.Left:
				var body = new Body(e.X, e.Y, 0.0f, 0.0f, 50.0f, colorMap["Red"]);
				bodies.Add(body);
				break;

			case Mouse.Button.Right:
				var large = new Body(e.X, e.Y, 0.0f, 0.0f, 2000.0f, colorMap["Brown"]);
				bodies.Add(large);
				break;
		}
	}

	private void Window_KeyReleased(object? sender, KeyEventArgs e)
	{
		switch (e.Code)
		{
			case Keyboard.Key.B:
				showBounds = !showBounds;
				break;

			case Keyboard.Key.M:
				showCentersOfMass = !showCentersOfMass;
				break;

			case Keyboard.Key.P:
				update = !update;
				break;

			case Keyboard.Key.R:
				bodies = new List<Body>();
				break;

			case Keyboard.Key.T:
				ParseUniverse();
				break;

			case Keyboard.Key.Add:
				updateTime += 0.1f;
				break;

			case Keyboard.Key.Subtract:
				updateTime -= 0.1f;
				break;
		}
	}
}