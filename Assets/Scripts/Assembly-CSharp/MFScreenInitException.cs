using System;
using System.Runtime.Serialization;
using UnityEngine;

public class MFScreenInitException : UnityException
{
	public MFScreenInitException()
		: base("Error during Screen initialization")
	{
	}

	public MFScreenInitException(string message)
		: base(message)
	{
	}

	public MFScreenInitException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MFScreenInitException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
