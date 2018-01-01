using System;
using System.Collections.Generic;
using UnityEngine.Experimental.U2D;

namespace UnityEditor.Experimental.U2D
{
	public interface ISpriteBoneDataProvider
	{
		List<SpriteBone> GetBones(GUID guid);

		void SetBones(GUID guid, List<SpriteBone> bones);
	}
}
