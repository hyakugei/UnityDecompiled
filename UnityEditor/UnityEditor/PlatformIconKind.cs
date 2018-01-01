using System;

namespace UnityEditor
{
	public class PlatformIconKind
	{
		internal static readonly PlatformIconKind Any = new PlatformIconKind(-1, "Any", BuildTargetGroup.Unknown, null);

		internal int kind
		{
			get;
			private set;
		}

		internal string platform
		{
			get;
			private set;
		}

		internal string[] customLayerLabels
		{
			get;
			private set;
		}

		private string kindString
		{
			get;
			set;
		}

		internal PlatformIconKind(int kind, string kindString, BuildTargetGroup platform, string[] customLayerLabels = null)
		{
			this.kind = kind;
			this.platform = PlayerSettings.GetPlatformName(platform);
			this.kindString = kindString;
			this.customLayerLabels = customLayerLabels;
		}

		public override bool Equals(object obj)
		{
			return obj != null && base.GetType() == obj.GetType() && this.kind == ((PlatformIconKind)obj).kind;
		}

		public override int GetHashCode()
		{
			return this.kind.GetHashCode();
		}

		public override string ToString()
		{
			return this.kindString;
		}
	}
}
