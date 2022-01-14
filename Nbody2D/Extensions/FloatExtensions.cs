namespace Nbody2D.Extensions
{
	internal static class FloatExtensions
	{
		public static float Map(this float me, float from1, float to1, float from2, float to2)
		{
			return (me - from1) / (to1 - from1) * (to2 - from2) + from2;
		}
	}
}