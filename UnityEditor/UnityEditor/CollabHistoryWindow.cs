using System;
using System.Collections.Generic;
using UnityEditor.Collaboration;
using UnityEditor.Connect;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor
{
	internal class CollabHistoryWindow : EditorWindow, ICollabHistoryWindow
	{
		private const string kWindowTitle = "Collab History";

		private const string kServiceUrl = "developer.cloud.unity3d.com";

		private CollabHistoryPresenter m_Presenter;

		private Dictionary<HistoryState, VisualElement> m_Views;

		private List<CollabHistoryItem> m_HistoryItems = new List<CollabHistoryItem>();

		private HistoryState m_State;

		private VisualElement m_Container;

		private PagedListView m_Pager;

		private int m_ItemsPerPage = 5;

		private string m_ErrMessage;

		private string m_InProgressRev;

		private bool m_RevisionActionsEnabled;

		public bool revisionActionsEnabled
		{
			get
			{
				return this.m_RevisionActionsEnabled;
			}
			set
			{
				if (this.m_RevisionActionsEnabled != value)
				{
					this.m_RevisionActionsEnabled = value;
					foreach (CollabHistoryItem current in this.m_HistoryItems)
					{
						current.RevisionActionsEnabled = value;
					}
				}
			}
		}

		public string inProgressRevision
		{
			get
			{
				return this.m_InProgressRev;
			}
			set
			{
				this.m_InProgressRev = value;
				foreach (CollabHistoryItem current in this.m_HistoryItems)
				{
					current.SetInProgressStatus(value);
				}
			}
		}

		public int itemsPerPage
		{
			set
			{
				if (this.m_ItemsPerPage != value)
				{
					this.m_Pager.pageSize = this.m_ItemsPerPage;
				}
			}
		}

		public string errMessage
		{
			private get
			{
				return this.m_ErrMessage;
			}
			set
			{
				this.m_ErrMessage = value;
			}
		}

		public PageChangeAction OnPageChangeAction
		{
			set
			{
				this.m_Pager.OnPageChange = value;
			}
		}

		public RevisionAction OnGoBackAction
		{
			set
			{
				CollabHistoryItem.s_OnGoBack = value;
			}
		}

		public RevisionAction OnUpdateAction
		{
			set
			{
				CollabHistoryItem.s_OnUpdate = value;
			}
		}

		public RevisionAction OnRestoreAction
		{
			set
			{
				CollabHistoryItem.s_OnRestore = value;
			}
		}

		public ShowBuildAction OnShowBuildAction
		{
			set
			{
				CollabHistoryItem.s_OnShowBuild = value;
			}
		}

		public Action OnShowServicesAction
		{
			set
			{
				CollabHistoryItem.s_OnShowServices = value;
			}
		}

		public CollabHistoryWindow()
		{
			base.minSize = new Vector2(275f, 50f);
		}

		[MenuItem("Window/Collab History", false, 2011)]
		public static void ShowHistoryWindow()
		{
			EditorWindow.GetWindow<CollabHistoryWindow>("Collab History");
		}

		[MenuItem("Window/Collab History", true)]
		public static bool ValidateShowHistoryWindow()
		{
			return Collab.instance.IsCollabEnabledForCurrentProject();
		}

		public void OnEnable()
		{
			this.SetupGUI();
			base.name = "CollabHistory";
			if (this.m_Presenter == null)
			{
				this.m_Presenter = new CollabHistoryPresenter(this, new CollabHistoryItemFactory(), new RevisionsService(Collab.instance, UnityConnect.instance));
			}
			this.m_Presenter.OnWindowEnabled();
		}

		public void OnDisable()
		{
			this.m_Presenter.OnWindowDisabled();
		}

		public void SetupGUI()
		{
			VisualElement rootVisualContainer = this.GetRootVisualContainer();
			rootVisualContainer.AddStyleSheetPath("StyleSheets/CollabHistoryCommon.uss");
			if (EditorGUIUtility.isProSkin)
			{
				rootVisualContainer.AddStyleSheetPath("StyleSheets/CollabHistoryDark.uss");
			}
			else
			{
				rootVisualContainer.AddStyleSheetPath("StyleSheets/CollabHistoryLight.uss");
			}
			this.m_Container = new VisualElement();
			this.m_Container.StretchToParentSize();
			rootVisualContainer.Add(this.m_Container);
			this.m_Pager = new PagedListView
			{
				name = "PagedElement",
				PagerLoc = PagerLocation.Top,
				pageSize = this.m_ItemsPerPage
			};
			StatusView value = new StatusView
			{
				message = "An Error Occurred",
				icon = EditorGUIUtility.LoadIconRequired("Collab.Warning")
			};
			StatusView value2 = new StatusView
			{
				message = "No Internet Connection",
				icon = EditorGUIUtility.LoadIconRequired("Collab.NoInternet")
			};
			StatusView value3 = new StatusView
			{
				message = "Maintenance"
			};
			StatusView value4 = new StatusView
			{
				message = "Sign in to access Collaborate",
				buttonText = "Sign in...",
				callback = new Action(this.SignInClick)
			};
			StatusView value5 = new StatusView
			{
				message = "Ask your project owner for access to Unity Teams",
				buttonText = "Learn More",
				callback = new Action(this.NoSeatClick)
			};
			StatusView value6 = new StatusView
			{
				message = "Connecting..."
			};
			ScrollView scrollView = new ScrollView
			{
				name = "HistoryContainer",
				showHorizontal = false
			};
			scrollView.contentContainer.StretchToParentWidth();
			scrollView.Add(this.m_Pager);
			this.m_Views = new Dictionary<HistoryState, VisualElement>
			{
				{
					HistoryState.Error,
					value
				},
				{
					HistoryState.Offline,
					value2
				},
				{
					HistoryState.Maintenance,
					value3
				},
				{
					HistoryState.LoggedOut,
					value4
				},
				{
					HistoryState.NoSeat,
					value5
				},
				{
					HistoryState.Waiting,
					value6
				},
				{
					HistoryState.Ready,
					scrollView
				}
			};
		}

		public void UpdateState(HistoryState state, bool force)
		{
			if (state != this.m_State || force)
			{
				if (state != HistoryState.Ready)
				{
					if (state == HistoryState.Disabled)
					{
						base.Close();
						return;
					}
				}
				else
				{
					this.UpdateHistoryView(this.m_Pager);
				}
				this.m_State = state;
				this.m_Container.Clear();
				this.m_Container.Add(this.m_Views[this.m_State]);
			}
		}

		public void UpdateRevisions(IEnumerable<RevisionData> datas, string tip, int totalRevisions)
		{
			List<VisualElement> list = new List<VisualElement>();
			bool isFullDateObtained = false;
			this.m_HistoryItems.Clear();
			DateTime dateTime = DateTime.MinValue;
			foreach (RevisionData current in datas)
			{
				if (current.timeStamp.Date != dateTime.Date)
				{
					list.Add(new CollabHistoryRevisionLine(current.timeStamp, isFullDateObtained));
					dateTime = current.timeStamp;
				}
				CollabHistoryItem collabHistoryItem = new CollabHistoryItem(current);
				this.m_HistoryItems.Add(collabHistoryItem);
				VisualContainer visualContainer = new VisualContainer();
				visualContainer.style.flexDirection = FlexDirection.Row;
				if (current.current)
				{
					isFullDateObtained = true;
					visualContainer.AddToClassList("currentRevision");
					visualContainer.AddToClassList("obtainedRevision");
				}
				else if (current.obtained)
				{
					visualContainer.AddToClassList("obtainedRevision");
				}
				else
				{
					visualContainer.AddToClassList("absentRevision");
				}
				visualContainer.Add(new CollabHistoryRevisionLine(current.index));
				visualContainer.Add(collabHistoryItem);
				list.Add(visualContainer);
			}
			this.m_Pager.totalItems = totalRevisions;
			this.m_Pager.items = list;
		}

		private void UpdateHistoryView(VisualElement history)
		{
		}

		private void NoSeatClick()
		{
			UnityConnect instance = UnityConnect.instance;
			string text = instance.GetEnvironment();
			if (text == "production")
			{
				text = "";
			}
			else
			{
				text += "-";
			}
			string url = string.Concat(new string[]
			{
				"https://",
				text,
				"developer.cloud.unity3d.com/orgs/",
				instance.GetOrganizationId(),
				"/projects/",
				instance.GetProjectName(),
				"/unity-teams/"
			});
			Application.OpenURL(url);
		}

		private void SignInClick()
		{
			UnityConnect.instance.ShowLogin();
		}
	}
}
