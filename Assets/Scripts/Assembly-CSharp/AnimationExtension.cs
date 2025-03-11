using UnityEngine;

public static class AnimationExtension
{
	public static bool Contains(this Animation inAnimation, AnimationClip inClip)
	{
		foreach (AnimationState item in inAnimation)
		{
			if (item.clip == inClip)
			{
				return true;
			}
		}
		return false;
	}
}
