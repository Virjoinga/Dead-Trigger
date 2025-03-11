using System;

[AttributeUsage(AttributeTargets.Method)]
public class NESActionAttribute : Attribute
{
	private string m_Argument;

	private string m_Name;

	public string Argument1
	{
		get
		{
			return m_Argument;
		}
		set
		{
			m_Argument = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return m_Name;
		}
		set
		{
			m_Name = value;
		}
	}
}
