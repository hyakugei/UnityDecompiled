using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Sharing
{
	[MovedFrom("UnityEngine.VR.WSA.Sharing")]
	public enum SerializationCompletionReason
	{
		Succeeded,
		NotSupported,
		AccessDenied,
		UnknownError
	}
}
