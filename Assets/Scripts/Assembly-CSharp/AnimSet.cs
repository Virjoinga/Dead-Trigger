using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AnimSet : MonoBehaviour
{
	protected class Randomizer
	{
		private List<string> List1 = new List<string>();

		private List<string> List2 = new List<string>();

		private List<string> Current;

		public Randomizer()
		{
			Current = List1;
		}

		public void Add(string s)
		{
			List1.Add(s);
		}

		public string Get()
		{
			int index = UnityEngine.Random.Range(0, Current.Count);
			string text = Current[index];
			if (Current.Count == 1)
			{
				if (List2.Count > 0)
				{
					if (Current == List1)
					{
						Current = List2;
					}
					else
					{
						Current = List1;
					}
				}
			}
			else
			{
				Current.RemoveAt(index);
				if (Current == List1)
				{
					List2.Add(text);
				}
				else
				{
					List1.Add(text);
				}
			}
			return text;
		}
	}

	protected class KeyedAnimList
	{
		private List<KeyValuePair<float, string>> Animations;

		public KeyedAnimList()
		{
			Animations = new List<KeyValuePair<float, string>>();
		}

		public void AddAnimation(KeyValuePair<float, string> anim)
		{
			Animations.Add(anim);
		}

		public void Clear()
		{
			Animations.Clear();
		}

		public List<string> GetAnimations(float key, float threshold)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<float, string> animation in Animations)
			{
				if (Mathf.Abs(animation.Key - key) <= threshold)
				{
					list.Add(animation.Value);
				}
			}
			return list;
		}
	}

	public enum E_TeleportAnim
	{
		In = 0,
		Out = 1
	}

	public abstract string GetIdleAnim();

	public abstract string GetIdleActionAnim();

	public abstract string GetMoveAnim(E_MotionSide motionSide = E_MotionSide.Center);

	public abstract string GetStrafeAnim(E_StrafeDirection dir);

	public abstract string GetRotateAnim(E_RotationType rotationType, float angle);

	public abstract string GetDodgeAnim(E_StrafeDirection dir);

	public abstract string GetBlockAnim(E_BlockState block);

	public abstract string GetKnockdowAnim(E_KnockdownState knockdownState);

	public abstract string GetContestAnim(E_ContestState state);

	public abstract string GetWeaponAnim(E_WeaponAction action);

	public abstract string GetInjuryAnim(E_BodyPart bodyPart, bool bDestroy, E_Direction direction);

	public abstract string GetInjuryCritAnim();

	public abstract string GetStandToCrawlAnim(E_MotionSide side);

	public abstract string GetDeathAnim(E_BodyPart bodyPart);

	public abstract string GetTeleportAnim(E_TeleportAnim type);

	public abstract string GetGadgetAnim(E_ItemID item);
}
