using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.PlatformSupport
{
	internal class PlatformIconFieldGroup
	{
		internal class IconFieldGroupInfo
		{
			public PlatformIconKind m_Kind;

			public string m_Label;

			public bool m_State;

			public int m_SetIconSlots;

			public int m_IconSlotCount;

			public override bool Equals(object obj)
			{
				return ((PlatformIconFieldGroup.IconFieldGroupInfo)obj).m_Label == this.m_Label;
			}

			public override int GetHashCode()
			{
				return this.m_Label.GetHashCode();
			}
		}

		internal Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, PlatformIconField[]>> m_IconsFields = new Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, PlatformIconField[]>>();

		internal Dictionary<PlatformIconKind, PlatformIcon[]> m_PlatformIconsByKind = new Dictionary<PlatformIconKind, PlatformIcon[]>();

		public BuildTargetGroup targetGroup
		{
			get;
			protected set;
		}

		internal PlatformIconFieldGroup(BuildTargetGroup targetGroup)
		{
			this.targetGroup = targetGroup;
		}

		internal PlatformIconField CreatePlatformIconField(PlatformIcon icon)
		{
			PlatformIconField result;
			if (icon.maxLayerCount > 1)
			{
				result = new PlatformIconFieldMultiLayer(icon, this.targetGroup);
			}
			else
			{
				result = new PlatformIconFieldSingleLayer(icon, this.targetGroup);
			}
			return result;
		}

		public bool IsEmpty()
		{
			return this.m_IconsFields.Count <= 0;
		}

		public void AddPlatformIcons(PlatformIcon[] icons, PlatformIconKind kind)
		{
			this.m_PlatformIconsByKind[kind] = icons;
			PlatformIconFieldGroup.IconFieldGroupInfo iconFieldGroupInfo = new PlatformIconFieldGroup.IconFieldGroupInfo();
			iconFieldGroupInfo.m_Kind = kind;
			iconFieldGroupInfo.m_Label = kind.ToString();
			Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, PlatformIconField[]> dictionary;
			if (!this.m_IconsFields.ContainsKey(iconFieldGroupInfo))
			{
				iconFieldGroupInfo.m_State = false;
				dictionary = new Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, PlatformIconField[]>();
			}
			else
			{
				dictionary = this.m_IconsFields[iconFieldGroupInfo];
			}
			IEnumerable<IGrouping<string, PlatformIcon>> enumerable = from i in icons
			group i by i.iconSubKind;
			foreach (IGrouping<string, PlatformIcon> current in enumerable)
			{
				PlatformIconField[] array = (from i in current.ToArray<PlatformIcon>()
				select this.CreatePlatformIconField(i)).ToArray<PlatformIconField>();
				PlatformIconFieldGroup.IconFieldGroupInfo iconFieldGroupInfo2 = new PlatformIconFieldGroup.IconFieldGroupInfo();
				iconFieldGroupInfo2.m_Kind = null;
				iconFieldGroupInfo2.m_Label = current.Key;
				iconFieldGroupInfo2.m_IconSlotCount = array.Length;
				iconFieldGroupInfo2.m_SetIconSlots = PlayerSettings.GetNonEmptyPlatformIconCount(current.ToArray<PlatformIcon>());
				if (!dictionary.ContainsKey(iconFieldGroupInfo2))
				{
					iconFieldGroupInfo2.m_State = false;
				}
				iconFieldGroupInfo.m_IconSlotCount += iconFieldGroupInfo2.m_IconSlotCount;
				iconFieldGroupInfo.m_SetIconSlots += iconFieldGroupInfo2.m_SetIconSlots;
				dictionary[iconFieldGroupInfo2] = array;
			}
			this.m_IconsFields[iconFieldGroupInfo] = dictionary;
		}
	}
}
