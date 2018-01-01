using System;

namespace UnityEditor.Scripting.ScriptCompilation
{
	internal struct CustomScriptOptinalUnityAssembly
	{
		public string DisplayName
		{
			get;
			private set;
		}

		public OptionalUnityReferences OptionalUnityReferences
		{
			get;
			private set;
		}

		public string AdditinalInformationWhenEnabled
		{
			get;
			private set;
		}

		public CustomScriptOptinalUnityAssembly(string displayName, OptionalUnityReferences optionalUnityReferences, string additinalInformationWhenEnabled = "")
		{
			this = default(CustomScriptOptinalUnityAssembly);
			this.DisplayName = displayName;
			this.OptionalUnityReferences = optionalUnityReferences;
			this.AdditinalInformationWhenEnabled = additinalInformationWhenEnabled;
		}
	}
}
