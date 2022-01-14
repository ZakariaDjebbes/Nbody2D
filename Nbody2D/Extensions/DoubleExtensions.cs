namespace Nbody2D.Extensions
{
	internal static class DoubleExtensions
	{
		public static double Map(this double me, double from1, double to1, double from2, double to2)
		{
			return (me - from1) / (to1 - from1) * (to2 - from2) + from2;
		}
	}
}