using SFML.System;

namespace NBody2D.Core
{
	internal class QuadTree
	{
		public Rectangle Bounds { get; set; }
		public Body Body { get; private set; }
		public float Theta { get; set; } = 0.8f;

		public delegate void QuadTreeAction(QuadTree obj);

		private List<QuadTree> nodes;

		public QuadTree(Rectangle bounds)
		{
			Bounds = bounds;
		}

		public void Insert(Body element)
		{
			if (Body == null)
			{
				Body = element;
				return;
			}

			if (!IsExternal())
			{
				Body += element;
				SetBody(element);
			}
			else
			{
				Subdivide();
				SetBody(Body);
				SetBody(element);

				Body += element;
			}
		}

		public void UpdateForce(Body other)
		{
			if (Body == null || Body.Equals(other))
				return;

			if (IsExternal())
			{
				other.Attract(Body);
			}
			else
			{
				var s = (Bounds.Width + Bounds.Height) / 2;
				var d = Body.Distance(other);
				if ((s / d) < Theta)
				{
					other.Attract(Body);
				}
				else
				{
					SetForce(other);
				}
			}
		}

		public void Foreach(QuadTreeAction action)
		{
			action(this);

			if (nodes != null)
				foreach (var node in nodes)
				{
					node.Foreach(action);
				}
		}

		public bool IsExternal() => nodes == null;

		private void SetForce(Body other)
		{
			foreach (var node in nodes)
			{
				node.UpdateForce(other);
			}
		}

		private void SetBody(Body body)
		{
			foreach (var node in nodes)
			{
				if (node.Bounds.Contains(body))
				{
					node.Insert(body);
					return;
				}
			}
		}

		private void Subdivide()
		{
			nodes = new List<QuadTree>(4)
			{
				//NE
				new QuadTree(new Rectangle
				{
						Width = Bounds.Width / 2,
						Height = Bounds.Height / 2,
						Position = new Vector2f(Bounds.Position.X + Bounds.Width / 2, Bounds.Position.Y)
				}),

				//NW
				new QuadTree(new Rectangle
				{
						Width = Bounds.Width / 2,
						Height = Bounds.Height / 2,
						Position = new Vector2f(Bounds.Position.X, Bounds.Position.Y)
				}),
				//SE
				new QuadTree(new Rectangle
				{
						Width = Bounds.Width / 2,
						Height = Bounds.Height / 2,
						Position = new Vector2f(Bounds.Position.X, Bounds.Position.Y + Bounds.Height / 2)
				}),
				//SW
				new QuadTree( new Rectangle
				{
						Width = Bounds.Width / 2,
						Height = Bounds.Height / 2,
						Position = new Vector2f(Bounds.Position.X + Bounds.Width / 2, Bounds.Position.Y + Bounds.Height / 2)
				}),
			};
		}
	}
}