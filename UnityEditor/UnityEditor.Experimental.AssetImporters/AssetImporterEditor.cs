using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Experimental.AssetImporters
{
	public abstract class AssetImporterEditor : Editor
	{
		private ulong m_AssetTimeStamp = 0uL;

		private bool m_MightHaveModified = false;

		private Editor m_AssetEditor;

		internal Editor assetEditor
		{
			private get
			{
				return this.m_AssetEditor;
			}
			set
			{
				this.m_AssetEditor = value;
			}
		}

		protected internal UnityEngine.Object[] assetTargets
		{
			get
			{
				return (!(this.m_AssetEditor != null)) ? null : this.m_AssetEditor.targets;
			}
		}

		protected internal UnityEngine.Object assetTarget
		{
			get
			{
				return (!(this.m_AssetEditor != null)) ? null : this.m_AssetEditor.target;
			}
		}

		protected internal SerializedObject assetSerializedObject
		{
			get
			{
				return (!(this.m_AssetEditor != null)) ? null : this.m_AssetEditor.serializedObject;
			}
		}

		internal override string targetTitle
		{
			get
			{
				return string.Format(L10n.Tr("{0} Import Settings"), (!(this.assetEditor == null)) ? this.assetEditor.targetTitle : string.Empty);
			}
		}

		internal override int referenceTargetIndex
		{
			get
			{
				return base.referenceTargetIndex;
			}
			set
			{
				base.referenceTargetIndex = value;
				if (this.assetEditor != null)
				{
					this.assetEditor.referenceTargetIndex = value;
				}
			}
		}

		internal override IPreviewable preview
		{
			get
			{
				IPreviewable result;
				if (this.useAssetDrawPreview && this.assetEditor != null)
				{
					result = this.assetEditor;
				}
				else
				{
					result = base.preview;
				}
				return result;
			}
		}

		protected virtual bool useAssetDrawPreview
		{
			get
			{
				return true;
			}
		}

		public virtual bool showImportedObject
		{
			get
			{
				return true;
			}
		}

		internal override void OnHeaderIconGUI(Rect iconRect)
		{
			if (this.assetEditor != null)
			{
				this.assetEditor.OnHeaderIconGUI(iconRect);
			}
		}

		internal override SerializedObject GetSerializedObjectInternal()
		{
			if (this.m_SerializedObject == null)
			{
				this.m_SerializedObject = SerializedObject.LoadFromCache(base.GetInstanceID());
			}
			if (this.m_SerializedObject == null)
			{
				this.m_SerializedObject = new SerializedObject(base.targets);
			}
			return this.m_SerializedObject;
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
			AssetImporter assetImporter = base.target as AssetImporter;
			if (Unsupported.IsDestroyScriptableObject(this) && this.m_MightHaveModified && assetImporter != null && !InternalEditorUtility.ignoreInspectorChanges && this.HasModified() && !this.AssetWasUpdated())
			{
				string message = string.Format(L10n.Tr("Unapplied import settings for '{0}'"), assetImporter.assetPath);
				if (base.targets.Length > 1)
				{
					message = string.Format(L10n.Tr("Unapplied import settings for '{0}' files"), base.targets.Length);
				}
				if (EditorUtility.DisplayDialog(L10n.Tr("Unapplied import settings"), message, L10n.Tr("Apply"), L10n.Tr("Revert")))
				{
					this.Apply();
					this.m_MightHaveModified = false;
					this.ImportAssets(this.GetAssetPaths());
				}
			}
			if (this.m_SerializedObject != null && this.m_SerializedObject.hasModifiedProperties)
			{
				this.m_SerializedObject.Cache(base.GetInstanceID());
				this.m_SerializedObject = null;
			}
		}

		protected virtual void Awake()
		{
			this.ResetTimeStamp();
			this.ResetValues();
		}

		private string[] GetAssetPaths()
		{
			UnityEngine.Object[] targets = base.targets;
			string[] array = new string[targets.Length];
			for (int i = 0; i < targets.Length; i++)
			{
				AssetImporter assetImporter = targets[i] as AssetImporter;
				array[i] = assetImporter.assetPath;
			}
			return array;
		}

		protected virtual void ResetValues()
		{
			base.serializedObject.SetIsDifferentCacheDirty();
			base.serializedObject.Update();
		}

		public virtual bool HasModified()
		{
			return base.serializedObject.hasModifiedProperties;
		}

		protected virtual void Apply()
		{
			base.serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}

		internal bool AssetWasUpdated()
		{
			AssetImporter assetImporter = base.target as AssetImporter;
			if (this.m_AssetTimeStamp == 0uL)
			{
				this.ResetTimeStamp();
			}
			return assetImporter != null && this.m_AssetTimeStamp != assetImporter.assetTimeStamp;
		}

		internal void ResetTimeStamp()
		{
			AssetImporter assetImporter = base.target as AssetImporter;
			if (assetImporter != null)
			{
				this.m_AssetTimeStamp = assetImporter.assetTimeStamp;
			}
		}

		protected internal void ApplyAndImport()
		{
			this.Apply();
			this.m_MightHaveModified = false;
			this.ImportAssets(this.GetAssetPaths());
			this.ResetValues();
		}

		private void ImportAssets(string[] paths)
		{
			for (int i = 0; i < paths.Length; i++)
			{
				string path = paths[i];
				AssetDatabase.WriteImportSettingsIfDirty(path);
			}
			try
			{
				AssetDatabase.StartAssetEditing();
				for (int j = 0; j < paths.Length; j++)
				{
					string path2 = paths[j];
					AssetDatabase.ImportAsset(path2);
				}
			}
			finally
			{
				AssetDatabase.StopAssetEditing();
			}
			this.OnAssetImportDone();
		}

		internal virtual void OnAssetImportDone()
		{
		}

		protected void RevertButton()
		{
			this.RevertButton(L10n.Tr("Revert"));
		}

		protected void RevertButton(string buttonText)
		{
			if (GUILayout.Button(buttonText, new GUILayoutOption[0]))
			{
				this.m_MightHaveModified = false;
				this.ResetTimeStamp();
				this.ResetValues();
				if (this.HasModified())
				{
					Debug.LogError("Importer reports modified values after reset.");
				}
			}
		}

		protected bool ApplyButton()
		{
			return this.ApplyButton(L10n.Tr("Apply"));
		}

		protected bool ApplyButton(string buttonText)
		{
			bool result;
			if (GUILayout.Button(buttonText, new GUILayoutOption[0]))
			{
				this.ApplyAndImport();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		protected virtual bool OnApplyRevertGUI()
		{
			bool result;
			using (new EditorGUI.DisabledScope(!this.HasModified()))
			{
				this.RevertButton();
				result = this.ApplyButton();
			}
			return result;
		}

		protected void ApplyRevertGUI()
		{
			if (this.assetEditor == null || this.assetEditor.target == null)
			{
				this.Apply();
			}
			else
			{
				this.m_MightHaveModified = true;
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				bool flag = this.OnApplyRevertGUI();
				if (this.AssetWasUpdated() && Event.current.type != EventType.Layout)
				{
					IPreviewable preview = this.preview;
					if (preview != null)
					{
						preview.ReloadPreviewInstances();
					}
					this.ResetTimeStamp();
					this.ResetValues();
					base.Repaint();
				}
				GUILayout.EndHorizontal();
				if (flag)
				{
					GUIUtility.ExitGUI();
				}
			}
		}
	}
}
