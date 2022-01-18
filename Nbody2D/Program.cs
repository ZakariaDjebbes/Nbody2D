using NBody2D;

namespace Nbody2D;

internal class Program
{
	private static void Main(string[] args)
	{
		Nbody program = new(1000, 1000, "NBody 2D");
		program.Run();
	}
}