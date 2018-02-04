using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEditor.Experimental.UIElements.Debugger
{
	internal class MatchedRulesExtractor : HierarchyTraversal
	{
		internal struct MatchedRule
		{
			private sealed class LineNumberFullPathEqualityComparer : IEqualityComparer<MatchedRulesExtractor.MatchedRule>
			{
				public bool Equals(MatchedRulesExtractor.MatchedRule x, MatchedRulesExtractor.MatchedRule y)
				{
					return x.lineNumber == y.lineNumber && string.Equals(x.fullPath, y.fullPath);
				}

				public int GetHashCode(MatchedRulesExtractor.MatchedRule obj)
				{
					return obj.lineNumber * 397 ^ ((obj.fullPath == null) ? 0 : obj.fullPath.GetHashCode());
				}
			}

			public readonly RuleMatcher ruleMatcher;

			public readonly string displayPath;

			public readonly int lineNumber;

			public readonly string fullPath;

			public static IEqualityComparer<MatchedRulesExtractor.MatchedRule> lineNumberFullPathComparer = new MatchedRulesExtractor.MatchedRule.LineNumberFullPathEqualityComparer();

			public MatchedRule(RuleMatcher ruleMatcher)
			{
				this = default(MatchedRulesExtractor.MatchedRule);
				this.ruleMatcher = ruleMatcher;
				this.fullPath = AssetDatabase.GetAssetPath(ruleMatcher.sheet);
				this.lineNumber = ruleMatcher.complexSelector.rule.line;
				if (this.fullPath != null)
				{
					if (this.fullPath == "Library/unity editor resources")
					{
						this.displayPath = ruleMatcher.sheet.name + ":" + this.lineNumber;
					}
					else
					{
						this.displayPath = Path.GetFileNameWithoutExtension(this.fullPath) + ":" + this.lineNumber;
					}
				}
			}
		}

		internal List<RuleMatcher> ruleMatchers = new List<RuleMatcher>();

		internal HashSet<MatchedRulesExtractor.MatchedRule> selectedElementRules = new HashSet<MatchedRulesExtractor.MatchedRule>(MatchedRulesExtractor.MatchedRule.lineNumberFullPathComparer);

		internal HashSet<string> selectedElementStylesheets = new HashSet<string>();

		private VisualElement m_Target;

		private List<VisualElement> m_Hierarchy = new List<VisualElement>();

		private int m_Index;

		private void Setup(VisualElement cursor)
		{
			this.m_Hierarchy.Add(cursor);
			if (cursor.shadow.parent != null)
			{
				this.Setup(cursor.shadow.parent);
			}
			if (cursor.styleSheets != null)
			{
				foreach (StyleSheet current in cursor.styleSheets)
				{
					this.selectedElementStylesheets.Add(AssetDatabase.GetAssetPath(current) ?? "<unknown>");
					this.PushStyleSheet(current);
				}
			}
		}

		public void SetupTarget(VisualElement target)
		{
			this.m_Target = target;
			this.m_Hierarchy.Clear();
			this.Setup(target);
			this.m_Index = this.m_Hierarchy.Count - 1;
		}

		private void PushStyleSheet(StyleSheet styleSheetData)
		{
			StyleComplexSelector[] complexSelectors = styleSheetData.complexSelectors;
			int val = this.ruleMatchers.Count + complexSelectors.Length;
			this.ruleMatchers.Capacity = Math.Max(this.ruleMatchers.Capacity, val);
			for (int i = 0; i < complexSelectors.Length; i++)
			{
				StyleComplexSelector complexSelector = complexSelectors[i];
				this.ruleMatchers.Add(new RuleMatcher
				{
					sheet = styleSheetData,
					complexSelector = complexSelector
				});
			}
		}

		public override bool ShouldSkipElement(VisualElement element)
		{
			return false;
		}

		public override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
		{
			if (element == this.m_Target)
			{
				this.selectedElementRules.Add(new MatchedRulesExtractor.MatchedRule(matcher));
			}
			return false;
		}

		protected override void Recurse(VisualElement element, int depth, List<RuleMatcher> allRuleMatchers)
		{
			this.m_Index--;
			if (this.m_Index >= 0)
			{
				this.TraverseRecursive(this.m_Hierarchy[this.m_Index], depth + 1, allRuleMatchers);
			}
		}
	}
}
