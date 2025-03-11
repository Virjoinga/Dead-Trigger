using System;

[AttributeUsage(AttributeTargets.Class)]
public class NESEventAttribute : Attribute
{
	public readonly string[] events;

	public NESEventAttribute(params string[] inEvents)
	{
		events = inEvents;
	}
}
