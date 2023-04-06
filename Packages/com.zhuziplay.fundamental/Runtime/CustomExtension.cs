using UnityEngine;

namespace Fundamental
{
	public static class CustomExtension
	{
		public static bool Contains(this string[] array, string t)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == t)
					return true;
			}
			return false;
		}

		public static float GetLastValue(this AnimationCurve curve)
		{
			var keys = curve.keys;
			return curve[keys.Length - 1].value;
		}

		public static float GetFirstValue(this AnimationCurve curve)
		{
			var keys = curve.keys;
			return curve[0].value;
		}
	}
}