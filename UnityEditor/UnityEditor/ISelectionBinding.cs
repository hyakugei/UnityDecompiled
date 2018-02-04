using System;
using UnityEngine;

namespace UnityEditor
{
	internal interface ISelectionBinding
	{
		GameObject rootGameObject
		{
			get;
		}

		AnimationClip animationClip
		{
			get;
		}

		bool clipIsEditable
		{
			get;
		}

		bool animationIsEditable
		{
			get;
		}

		int id
		{
			get;
		}
	}
}
