using System;
using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal class StyleContext
	{
		internal struct RuleRef
		{
			public StyleComplexSelector selector;

			public StyleSheet sheet;
		}

		internal class StyleContextHierarchyTraversal : HierarchyTraversal
		{
			private List<StyleContext.RuleRef> m_MatchedRules = new List<StyleContext.RuleRef>(0);

			private long m_MatchingRulesHash;

			public float currentPixelsPerPoint
			{
				get;
				set;
			}

			public override bool ShouldSkipElement(VisualElement element)
			{
				return !element.IsDirty(ChangeType.Styles) && !element.IsDirty(ChangeType.StylesPath);
			}

			public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				StyleRule rule = matcher.complexSelector.rule;
				int specificity = matcher.complexSelector.specificity;
				this.m_MatchingRulesHash = (this.m_MatchingRulesHash * 397L ^ (long)rule.GetHashCode());
				this.m_MatchingRulesHash = (this.m_MatchingRulesHash * 397L ^ (long)specificity);
				this.m_MatchedRules.Add(new StyleContext.RuleRef
				{
					selector = matcher.complexSelector,
					sheet = matcher.sheet
				});
				return false;
			}

			public override void OnBeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
			{
				if (element.IsDirty(ChangeType.Styles))
				{
					element.triggerPseudoMask = (PseudoStates)0;
					element.dependencyPseudoMask = (PseudoStates)0;
				}
				if (element != null && element.styleSheets != null)
				{
					for (int i = 0; i < element.styleSheets.Count; i++)
					{
						StyleSheet styleSheet = element.styleSheets[i];
						StyleComplexSelector[] complexSelectors = styleSheet.complexSelectors;
						int val = ruleMatchers.Count + complexSelectors.Length;
						ruleMatchers.Capacity = Math.Max(ruleMatchers.Capacity, val);
						for (int j = 0; j < complexSelectors.Length; j++)
						{
							StyleComplexSelector complexSelector = complexSelectors[j];
							ruleMatchers.Add(new RuleMatcher
							{
								sheet = styleSheet,
								complexSelector = complexSelector
							});
						}
					}
				}
				this.m_MatchedRules.Clear();
				string fullTypeName = element.fullTypeName;
				long num = (long)fullTypeName.GetHashCode();
				this.m_MatchingRulesHash = (num * 397L ^ (long)this.currentPixelsPerPoint.GetHashCode());
			}

			public override void OnProcessMatchResult(VisualElement element, ref RuleMatcher matcher, ref HierarchyTraversal.MatchResultInfo matchInfo)
			{
				element.triggerPseudoMask |= matchInfo.triggerPseudoMask;
				element.dependencyPseudoMask |= matchInfo.dependencyPseudoMask;
			}

			public override void ProcessMatchedRules(VisualElement element)
			{
				VisualElementStylesData visualElementStylesData;
				if (StyleContext.s_StyleCache.TryGetValue(this.m_MatchingRulesHash, out visualElementStylesData))
				{
					element.SetSharedStyles(visualElementStylesData);
				}
				else
				{
					visualElementStylesData = new VisualElementStylesData(true);
					int i = 0;
					int count = this.m_MatchedRules.Count;
					while (i < count)
					{
						StyleContext.RuleRef ruleRef = this.m_MatchedRules[i];
						StylePropertyID[] propertyIDs = StyleSheetCache.GetPropertyIDs(ruleRef.sheet, ruleRef.selector.ruleIndex);
						visualElementStylesData.ApplyRule(ruleRef.sheet, ruleRef.selector.specificity, ruleRef.selector.rule, propertyIDs);
						i++;
					}
					StyleContext.s_StyleCache[this.m_MatchingRulesHash] = visualElementStylesData;
					element.SetSharedStyles(visualElementStylesData);
				}
			}
		}

		private VisualElement m_VisualTree;

		private static Dictionary<long, VisualElementStylesData> s_StyleCache = new Dictionary<long, VisualElementStylesData>();

		internal static StyleContext.StyleContextHierarchyTraversal styleContextHierarchyTraversal = new StyleContext.StyleContextHierarchyTraversal();

		public float currentPixelsPerPoint
		{
			get;
			set;
		}

		public StyleContext(VisualElement tree)
		{
			this.<currentPixelsPerPoint>k__BackingField = 1f;
			base..ctor();
			this.m_VisualTree = tree;
		}

		public void DirtyStyleSheets()
		{
			StyleContext.PropagateDirtyStyleSheets(this.m_VisualTree);
		}

		public void ApplyStyles()
		{
			Debug.Assert(this.m_VisualTree.panel != null);
			StyleContext.styleContextHierarchyTraversal.currentPixelsPerPoint = this.currentPixelsPerPoint;
			StyleContext.styleContextHierarchyTraversal.Traverse(this.m_VisualTree);
		}

		private static void PropagateDirtyStyleSheets(VisualElement element)
		{
			if (element != null)
			{
				if (element.styleSheets != null)
				{
					element.LoadStyleSheetsFromPaths();
				}
				foreach (VisualElement current in element.shadow.Children())
				{
					StyleContext.PropagateDirtyStyleSheets(current);
				}
			}
		}

		public static void ClearStyleCache()
		{
			StyleContext.s_StyleCache.Clear();
		}
	}
}
