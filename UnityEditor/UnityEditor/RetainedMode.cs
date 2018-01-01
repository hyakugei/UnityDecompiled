using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.Scripting;

namespace UnityEditor
{
	internal class RetainedMode : AssetPostprocessor
	{
		private const string k_UielementsUxmllivereloadPrefsKey = "UIElements_UXMLLiveReload";

		private static HashSet<UnityEngine.Object> s_TmpDirtySet;

		private static bool s_FontInitialized;

		[CompilerGenerated]
		private static Action<IMGUIContainer> <>f__mg$cache0;

		[CompilerGenerated]
		private static Action<IMGUIContainer> <>f__mg$cache1;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction <>f__mg$cache2;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction <>f__mg$cache3;

		internal static bool UxmlLiveReloadIsEnabled
		{
			get
			{
				return EditorPrefs.GetBool("UIElements_UXMLLiveReload", true);
			}
			set
			{
				EditorPrefs.SetBool("UIElements_UXMLLiveReload", value);
			}
		}

		static RetainedMode()
		{
			RetainedMode.s_TmpDirtySet = new HashSet<UnityEngine.Object>();
			RetainedMode.s_FontInitialized = false;
			if (RetainedMode.<>f__mg$cache0 == null)
			{
				RetainedMode.<>f__mg$cache0 = new Action<IMGUIContainer>(RetainedMode.OnBeginContainer);
			}
			UIElementsUtility.s_BeginContainerCallback = RetainedMode.<>f__mg$cache0;
			if (RetainedMode.<>f__mg$cache1 == null)
			{
				RetainedMode.<>f__mg$cache1 = new Action<IMGUIContainer>(RetainedMode.OnEndContainer);
			}
			UIElementsUtility.s_EndContainerCallback = RetainedMode.<>f__mg$cache1;
		}

		private static void OnBeginContainer(IMGUIContainer c)
		{
			if (!RetainedMode.s_FontInitialized)
			{
				RetainedMode.s_FontInitialized = true;
				LocalizedEditorFontManager.LocalizeEditorFonts();
			}
			HandleUtility.BeginHandles();
		}

		private static void OnEndContainer(IMGUIContainer c)
		{
			HandleUtility.EndHandles();
		}

		[RequiredByNativeCode]
		private static void UpdateSchedulers()
		{
			try
			{
				RetainedMode.UpdateSchedulersInternal(RetainedMode.s_TmpDirtySet);
			}
			finally
			{
				RetainedMode.s_TmpDirtySet.Clear();
			}
		}

		private static void UpdateSchedulersInternal(HashSet<UnityEngine.Object> tmpDirtySet)
		{
			DataWatchService.sharedInstance.PollNativeData();
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				KeyValuePair<int, Panel> current = panelsIterator.Current;
				Panel value = current.Value;
				if (value.contextType == ContextType.Editor)
				{
					IScheduler scheduler = value.scheduler;
					value.timerEventScheduler.UpdateScheduledEvents();
					if (value.visualTree.IsDirty(ChangeType.Repaint))
					{
						GUIView gUIView = value.ownerObject as GUIView;
						if (gUIView != null)
						{
							gUIView.Repaint();
						}
					}
				}
			}
		}

		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < importedAssets.Length; i++)
			{
				string text = importedAssets[i];
				if (text.EndsWith("uss"))
				{
					if (!flag2)
					{
						flag2 = true;
						RetainedMode.FlagStyleSheetChange();
					}
				}
				else if (text.EndsWith("uxml"))
				{
					if (!flag)
					{
						flag = true;
						UIElementsViewImporter.logger.FinishImport();
						StyleSheetCache.ClearCaches();
						if (RetainedMode.UxmlLiveReloadIsEnabled)
						{
							Delegate arg_92_0 = EditorApplication.update;
							if (RetainedMode.<>f__mg$cache2 == null)
							{
								RetainedMode.<>f__mg$cache2 = new EditorApplication.CallbackFunction(RetainedMode.OneShotUxmlLiveReload);
							}
							EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(arg_92_0, RetainedMode.<>f__mg$cache2);
						}
					}
				}
				if (flag && flag2)
				{
					break;
				}
			}
		}

		private static void OneShotUxmlLiveReload()
		{
			try
			{
				Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
				while (panelsIterator.MoveNext())
				{
					KeyValuePair<int, Panel> current = panelsIterator.Current;
					HostView hostView = current.Value.ownerObject as HostView;
					if (hostView != null && hostView.actualView != null)
					{
						hostView.Reload(hostView.actualView);
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			Delegate arg_96_0 = EditorApplication.update;
			if (RetainedMode.<>f__mg$cache3 == null)
			{
				RetainedMode.<>f__mg$cache3 = new EditorApplication.CallbackFunction(RetainedMode.OneShotUxmlLiveReload);
			}
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(arg_96_0, RetainedMode.<>f__mg$cache3);
		}

		public static void FlagStyleSheetChange()
		{
			StyleSheetCache.ClearCaches();
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				KeyValuePair<int, Panel> current = panelsIterator.Current;
				Panel value = current.Value;
				if (value.contextType == ContextType.Editor)
				{
					value.styleContext.DirtyStyleSheets();
					value.visualTree.Dirty(ChangeType.Styles);
					GUIView gUIView = value.ownerObject as GUIView;
					if (gUIView != null)
					{
						gUIView.Repaint();
					}
				}
			}
		}
	}
}
