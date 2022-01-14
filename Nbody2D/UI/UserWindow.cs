using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace NBody2D.UI;

internal abstract class UserWindow
{
	protected uint Height { get; set; }
	protected uint Width { get; set; }

	protected string Title
	{
		get => title;
		set
		{
			title = value;
			if (Window != null)
				Window.SetTitle(title);
		}
	}

	protected RenderWindow Window { get; set; }
	public float Fps { get; private set; }

	private string title;
	private string originalTitle;
	private float titleTimer = 0.0f;

	public UserWindow(uint height, uint width, string title = "Title !")
	{
		Height = height;
		Width = width;
		Title = title;
		originalTitle = title;
	}

	public void Run()
	{
		Window = new RenderWindow(new VideoMode(Height, Width), Title);
		Window.Closed += Window_Closed;
		OnStart();
		var deltaTime = 0.0f;
		Clock clock = new Clock();
		while (Window.IsOpen)
		{
			titleTimer -= deltaTime;
			if (titleTimer <= 0.0f)
			{
				Title = $"{originalTitle} - FPS: {Fps:0.00}";
				titleTimer = 1.0f;
			}

			Window.DispatchEvents();
			OnUpdate(deltaTime);
			Window.Display();
			deltaTime = clock.Restart().AsSeconds();
			Fps = 1 / deltaTime;
		}
		OnEnd();
	}

	protected virtual void OnStart()
	{ }

	protected virtual void OnEnd()
	{ }

	protected virtual void OnUpdate(float deltaTime)
	{ }

	private void Window_Closed(object? sender, EventArgs e)
	{
		if (sender == null)
			throw new ArgumentNullException("The window provided in the close context in null");

		var window = (Window)sender ?? throw new ArgumentNullException("The window provided in the close context in null");
		window.Close();
	}
}