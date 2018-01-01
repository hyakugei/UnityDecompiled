using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Build.Reporting
{
	public class StrippingInfo : ScriptableObject, ISerializationCallbackReceiver
	{
		[Serializable]
		internal struct SerializedDependency
		{
			[SerializeField]
			public string key;

			[SerializeField]
			public List<string> value;

			[SerializeField]
			public string icon;

			[SerializeField]
			public int size;
		}

		internal const string RequiredByScripts = "Required by Scripts";

		[SerializeField]
		internal List<StrippingInfo.SerializedDependency> serializedDependencies;

		[SerializeField]
		internal List<string> modules = new List<string>();

		[SerializeField]
		internal List<int> serializedSizes = new List<int>();

		[SerializeField]
		internal Dictionary<string, HashSet<string>> dependencies = new Dictionary<string, HashSet<string>>();

		[SerializeField]
		internal Dictionary<string, int> sizes = new Dictionary<string, int>();

		[SerializeField]
		internal Dictionary<string, string> icons = new Dictionary<string, string>();

		[SerializeField]
		internal int totalSize = 0;

		public IEnumerable<string> includedModules
		{
			get
			{
				return this.modules;
			}
		}

		public IEnumerable<string> GetReasonsForIncluding(string entityName)
		{
			HashSet<string> hashSet;
			return (!this.dependencies.TryGetValue(entityName, out hashSet)) ? Enumerable.Empty<string>() : hashSet;
		}

		private void OnEnable()
		{
			this.SetIcon("Required by Scripts", "class/MonoScript");
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.serializedDependencies = new List<StrippingInfo.SerializedDependency>();
			foreach (KeyValuePair<string, HashSet<string>> current in this.dependencies)
			{
				List<string> list = new List<string>();
				foreach (string current2 in current.Value)
				{
					list.Add(current2);
				}
				StrippingInfo.SerializedDependency item;
				item.key = current.Key;
				item.value = list;
				item.icon = ((!this.icons.ContainsKey(current.Key)) ? "class/DefaultAsset" : this.icons[current.Key]);
				item.size = ((!this.sizes.ContainsKey(current.Key)) ? 0 : this.sizes[current.Key]);
				this.serializedDependencies.Add(item);
			}
			this.serializedSizes = new List<int>();
			foreach (string current3 in this.modules)
			{
				this.serializedSizes.Add((!this.sizes.ContainsKey(current3)) ? 0 : this.sizes[current3]);
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.dependencies = new Dictionary<string, HashSet<string>>();
			this.icons = new Dictionary<string, string>();
			for (int i = 0; i < this.serializedDependencies.Count; i++)
			{
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string current in this.serializedDependencies[i].value)
				{
					hashSet.Add(current);
				}
				this.dependencies.Add(this.serializedDependencies[i].key, hashSet);
				this.icons[this.serializedDependencies[i].key] = this.serializedDependencies[i].icon;
				this.sizes[this.serializedDependencies[i].key] = this.serializedDependencies[i].size;
			}
			this.sizes = new Dictionary<string, int>();
			for (int j = 0; j < this.serializedSizes.Count; j++)
			{
				this.sizes[this.modules[j]] = this.serializedSizes[j];
			}
		}

		internal void RegisterDependency(string obj, string depends)
		{
			if (!this.dependencies.ContainsKey(obj))
			{
				this.dependencies[obj] = new HashSet<string>();
			}
			this.dependencies[obj].Add(depends);
			if (!this.icons.ContainsKey(depends))
			{
				this.SetIcon(depends, "class/" + depends);
			}
		}

		internal void AddModule(string module, bool appendModuleToName = true)
		{
			string text = (!appendModuleToName) ? module : StrippingInfo.ModuleName(module);
			string arg = string.Format("com.unity.modules.{0}", module.ToLower());
			if (!this.modules.Contains(text))
			{
				this.modules.Add(text);
			}
			if (!this.sizes.ContainsKey(text))
			{
				this.sizes[text] = 0;
			}
			if (!this.icons.ContainsKey(text))
			{
				this.SetIcon(text, string.Format("package/{0}", arg));
			}
		}

		internal void SetIcon(string dependency, string icon)
		{
			this.icons[dependency] = icon;
			if (!this.dependencies.ContainsKey(dependency))
			{
				this.dependencies[dependency] = new HashSet<string>();
			}
		}

		internal void AddModuleSize(string module, int size)
		{
			if (this.modules.Contains(module))
			{
				this.sizes[module] = size;
			}
		}

		internal static StrippingInfo GetBuildReportData(BuildReport report)
		{
			StrippingInfo result;
			if (report == null)
			{
				result = null;
			}
			else
			{
				StrippingInfo[] appendices = report.GetAppendices<StrippingInfo>();
				if (appendices.Length > 0)
				{
					result = appendices[0];
				}
				else
				{
					StrippingInfo strippingInfo = ScriptableObject.CreateInstance<StrippingInfo>();
					report.AddAppendix(strippingInfo);
					result = strippingInfo;
				}
			}
			return result;
		}

		internal static string ModuleName(string module)
		{
			return module + " Module";
		}
	}
}
