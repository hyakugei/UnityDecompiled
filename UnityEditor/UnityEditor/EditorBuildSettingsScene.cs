using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class EditorBuildSettingsScene : IComparable
	{
		[NativeName("enabled")]
		private bool m_enabled;

		[NativeName("path")]
		private string m_path;

		[NativeName("guid")]
		private GUID m_guid;

		public bool enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
			}
		}

		public string path
		{
			get
			{
				return this.m_path;
			}
			set
			{
				this.m_path = value.Replace("\\", "/");
			}
		}

		public GUID guid
		{
			get
			{
				return this.m_guid;
			}
			set
			{
				this.m_guid = value;
			}
		}

		public EditorBuildSettingsScene()
		{
		}

		public EditorBuildSettingsScene(string path, bool enabled)
		{
			this.m_path = path.Replace("\\", "/");
			this.m_enabled = enabled;
			GUID.TryParse(AssetDatabase.AssetPathToGUID(path), out this.m_guid);
		}

		public EditorBuildSettingsScene(GUID guid, bool enabled)
		{
			this.m_guid = guid;
			this.m_enabled = enabled;
			this.m_path = AssetDatabase.GUIDToAssetPath(guid.ToString());
		}

		public static string[] GetActiveSceneList(EditorBuildSettingsScene[] scenes)
		{
			return (from scene in scenes
			where scene.enabled
			select scene.path).ToArray<string>();
		}

		public int CompareTo(object obj)
		{
			if (obj is EditorBuildSettingsScene)
			{
				EditorBuildSettingsScene editorBuildSettingsScene = (EditorBuildSettingsScene)obj;
				return editorBuildSettingsScene.path.CompareTo(this.path);
			}
			throw new ArgumentException("object is not a EditorBuildSettingsScene");
		}
	}
}
