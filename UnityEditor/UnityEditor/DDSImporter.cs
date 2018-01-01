using System;
using UnityEngine;

namespace UnityEditor
{
	[Obsolete("DDSImporter is obsolete. Use IHVImageFormatImporter instead (UnityUpgradable) -> IHVImageFormatImporter", true), NativeClass(null)]
	public class DDSImporter : AssetImporter
	{
		public bool isReadable
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
