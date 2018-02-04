using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class NewScriptDropdownElement : DropdownElement
	{
		private enum Language
		{
			CSharp
		}

		private readonly char[] kInvalidPathChars = new char[]
		{
			'<',
			'>',
			':',
			'"',
			'|',
			'?',
			'*',
			'\0'
		};

		private readonly char[] kPathSepChars = new char[]
		{
			'/',
			'\\'
		};

		private readonly string kResourcesTemplatePath = "Resources/ScriptTemplates";

		private string m_Directory = string.Empty;

		private string m_ClassName = "NewBehaviourScript";

		public string className
		{
			get
			{
				return this.m_ClassName;
			}
			set
			{
				this.m_ClassName = value;
			}
		}

		public NewScriptDropdownElement() : base("New Script")
		{
		}

		public override void Draw(bool selected, bool isSearching)
		{
			GUILayout.Label("Name", EditorStyles.label, new GUILayoutOption[0]);
			EditorGUI.FocusTextInControl("NewScriptName");
			GUI.SetNextControlName("NewScriptName");
			this.m_ClassName = EditorGUILayout.TextField(this.m_ClassName, new GUILayoutOption[0]);
			EditorGUILayout.Space();
			EditorGUILayout.EnumPopup("Language", NewScriptDropdownElement.Language.CSharp, new GUILayoutOption[0]);
			EditorGUILayout.Space();
			bool flag = this.CanCreate();
			if (!flag && this.m_ClassName != "")
			{
				GUILayout.Label(this.GetError(), EditorStyles.helpBox, new GUILayoutOption[0]);
			}
			GUILayout.FlexibleSpace();
			using (new EditorGUI.DisabledScope(!flag))
			{
				if (GUILayout.Button("Create and Add", new GUILayoutOption[0]))
				{
					this.Create(AddComponentWindow.s_AddComponentWindow.m_GameObjects);
				}
			}
			EditorGUILayout.Space();
		}

		public override bool OnAction()
		{
			this.Create(AddComponentWindow.s_AddComponentWindow.m_GameObjects);
			GUIUtility.ExitGUI();
			return true;
		}

		private bool CanCreate()
		{
			return this.m_ClassName.Length > 0 && !File.Exists(this.TargetPath()) && !this.ClassAlreadyExists() && !this.ClassNameIsInvalid() && !this.InvalidTargetPath();
		}

		private string GetError()
		{
			string result = string.Empty;
			if (this.m_ClassName != string.Empty)
			{
				if (File.Exists(this.TargetPath()))
				{
					result = "A script called \"" + this.m_ClassName + "\" already exists at that path.";
				}
				else if (this.ClassAlreadyExists())
				{
					result = "A class called \"" + this.m_ClassName + "\" already exists.";
				}
				else if (this.ClassNameIsInvalid())
				{
					result = "The script name may only consist of a-z, A-Z, 0-9, _.";
				}
				else if (this.InvalidTargetPath())
				{
					result = "The folder path contains invalid characters.";
				}
			}
			return result;
		}

		private void Create(GameObject[] gameObjects)
		{
			if (this.CanCreate())
			{
				this.CreateScript();
				for (int i = 0; i < gameObjects.Length; i++)
				{
					GameObject gameObject = gameObjects[i];
					MonoScript monoScript = AssetDatabase.LoadAssetAtPath(this.TargetPath(), typeof(MonoScript)) as MonoScript;
					monoScript.SetScriptTypeWasJustCreatedFromComponentMenu();
					InternalEditorUtility.AddScriptComponentUncheckedUndoable(gameObject, monoScript);
				}
				AddComponentWindow.SendUsabilityAnalyticsEvent(new AddComponentWindow.AnalyticsEventData
				{
					name = this.m_ClassName,
					filter = AddComponentWindow.s_AddComponentWindow.searchString,
					isNewScript = true
				});
				AddComponentWindow.s_AddComponentWindow.Close();
			}
		}

		private bool InvalidTargetPath()
		{
			return this.m_Directory.IndexOfAny(this.kInvalidPathChars) >= 0 || this.TargetDir().Split(this.kPathSepChars, StringSplitOptions.None).Contains(string.Empty);
		}

		private string TargetPath()
		{
			return Path.Combine(this.TargetDir(), this.m_ClassName + ".cs");
		}

		private string TargetDir()
		{
			return Path.Combine("Assets", this.m_Directory.Trim(this.kPathSepChars));
		}

		private bool ClassNameIsInvalid()
		{
			return !CodeGenerator.IsValidLanguageIndependentIdentifier(this.m_ClassName);
		}

		private bool ClassExists(string className)
		{
			return AppDomain.CurrentDomain.GetAssemblies().Any((Assembly a) => a.GetType(className, false) != null);
		}

		private bool ClassAlreadyExists()
		{
			return !(this.m_ClassName == string.Empty) && this.ClassExists(this.m_ClassName);
		}

		private void CreateScript()
		{
			string path = Path.Combine(EditorApplication.applicationContentsPath, this.kResourcesTemplatePath);
			string resourceFile = Path.Combine(path, "81-C# Script-NewBehaviourScript.cs.txt");
			ProjectWindowUtil.CreateScriptAssetFromTemplate(this.TargetPath(), resourceFile);
			AssetDatabase.Refresh();
		}
	}
}
