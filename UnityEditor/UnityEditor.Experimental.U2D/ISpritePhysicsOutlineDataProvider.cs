using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public interface ISpritePhysicsOutlineDataProvider
	{
		List<Vector2[]> GetOutlines(GUID guid);

		void SetOutlines(GUID guid, List<Vector2[]> data);

		float GetTessellationDetail(GUID guid);

		void SetTessellationDetail(GUID guid, float value);
	}
}
