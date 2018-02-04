using System;
using System.Collections.Generic;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal abstract class HierarchyTraversal : IHierarchyTraversal
	{
		public struct MatchResultInfo
		{
			public bool success;

			public PseudoStates triggerPseudoMask;

			public PseudoStates dependencyPseudoMask;
		}

		private List<RuleMatcher> m_ruleMatchers = new List<RuleMatcher>();

		public abstract bool ShouldSkipElement(VisualElement element);

		public abstract bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element);

		public virtual void OnBeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
		{
		}

		public void BeginElementTest(VisualElement element, List<RuleMatcher> ruleMatchers)
		{
			this.OnBeginElementTest(element, ruleMatchers);
		}

		public virtual void ProcessMatchedRules(VisualElement element)
		{
		}

		public virtual void OnProcessMatchResult(VisualElement element, ref RuleMatcher matcher, ref HierarchyTraversal.MatchResultInfo matchInfo)
		{
		}

		public virtual void Traverse(VisualElement element)
		{
			this.TraverseRecursive(element, 0, this.m_ruleMatchers);
			this.m_ruleMatchers.Clear();
		}

		public virtual void TraverseRecursive(VisualElement element, int depth, List<RuleMatcher> ruleMatchers)
		{
			if (!this.ShouldSkipElement(element))
			{
				int count = ruleMatchers.Count;
				this.BeginElementTest(element, ruleMatchers);
				int count2 = ruleMatchers.Count;
				for (int i = 0; i < count2; i++)
				{
					RuleMatcher ruleMatcher = ruleMatchers[i];
					if (this.MatchRightToLeft(element, ref ruleMatcher))
					{
						return;
					}
				}
				this.ProcessMatchedRules(element);
				this.Recurse(element, depth, ruleMatchers);
				if (ruleMatchers.Count > count)
				{
					ruleMatchers.RemoveRange(count, ruleMatchers.Count - count);
				}
			}
		}

		private bool MatchRightToLeft(VisualElement element, ref RuleMatcher matcher)
		{
			VisualElement visualElement = element;
			int i = matcher.complexSelector.selectors.Length - 1;
			VisualElement visualElement2 = null;
			int num = -1;
			bool result;
			while (i >= 0)
			{
				if (visualElement == null)
				{
					break;
				}
				HierarchyTraversal.MatchResultInfo matchResultInfo = this.Match(visualElement, ref matcher, i);
				this.OnProcessMatchResult(visualElement, ref matcher, ref matchResultInfo);
				if (!matchResultInfo.success)
				{
					if (i < matcher.complexSelector.selectors.Length - 1 && matcher.complexSelector.selectors[i + 1].previousRelationship == StyleSelectorRelationship.Descendent)
					{
						visualElement = visualElement.parent;
					}
					else
					{
						if (visualElement2 == null)
						{
							break;
						}
						visualElement = visualElement2;
						i = num;
					}
				}
				else
				{
					if (i < matcher.complexSelector.selectors.Length - 1 && matcher.complexSelector.selectors[i + 1].previousRelationship == StyleSelectorRelationship.Descendent)
					{
						visualElement2 = visualElement.parent;
						num = i;
					}
					if (--i < 0)
					{
						if (this.OnRuleMatchedElement(matcher, element))
						{
							result = true;
							return result;
						}
					}
					visualElement = visualElement.parent;
				}
			}
			result = false;
			return result;
		}

		protected virtual void Recurse(VisualElement element, int depth, List<RuleMatcher> ruleMatchers)
		{
			for (int i = 0; i < element.shadow.childCount; i++)
			{
				VisualElement element2 = element.shadow[i];
				this.TraverseRecursive(element2, depth + 1, ruleMatchers);
			}
		}

		protected virtual bool MatchSelectorPart(VisualElement element, StyleSelector selector, StyleSelectorPart part)
		{
			bool flag = true;
			switch (part.type)
			{
			case StyleSelectorType.Wildcard:
				return flag;
			case StyleSelectorType.Type:
				flag = (element.typeName == part.value);
				return flag;
			case StyleSelectorType.Class:
				flag = element.ClassListContains(part.value);
				return flag;
			case StyleSelectorType.PseudoClass:
			{
				int pseudoStates = (int)element.pseudoStates;
				flag = ((selector.pseudoStateMask & pseudoStates) == selector.pseudoStateMask);
				flag &= ((selector.negatedPseudoStateMask & ~pseudoStates) == selector.negatedPseudoStateMask);
				return flag;
			}
			case StyleSelectorType.ID:
				flag = (element.name == part.value);
				return flag;
			}
			flag = false;
			return flag;
		}

		public virtual HierarchyTraversal.MatchResultInfo Match(VisualElement element, ref RuleMatcher matcher, int selectorIndex)
		{
			HierarchyTraversal.MatchResultInfo result;
			if (element == null)
			{
				result = default(HierarchyTraversal.MatchResultInfo);
			}
			else
			{
				bool flag = true;
				StyleSelector styleSelector = matcher.complexSelector.selectors[selectorIndex];
				int num = styleSelector.parts.Length;
				int num2 = 0;
				int num3 = 0;
				bool flag2 = true;
				for (int i = 0; i < num; i++)
				{
					bool flag3 = this.MatchSelectorPart(element, styleSelector, styleSelector.parts[i]);
					if (!flag3)
					{
						if (styleSelector.parts[i].type == StyleSelectorType.PseudoClass)
						{
							num2 |= styleSelector.pseudoStateMask;
							num3 |= styleSelector.negatedPseudoStateMask;
						}
						else
						{
							flag2 = false;
						}
					}
					else if (styleSelector.parts[i].type == StyleSelectorType.PseudoClass)
					{
						num3 |= styleSelector.pseudoStateMask;
						num2 |= styleSelector.negatedPseudoStateMask;
					}
					flag &= flag3;
				}
				HierarchyTraversal.MatchResultInfo matchResultInfo = new HierarchyTraversal.MatchResultInfo
				{
					success = flag
				};
				if (flag || flag2)
				{
					matchResultInfo.triggerPseudoMask = (PseudoStates)num2;
					matchResultInfo.dependencyPseudoMask = (PseudoStates)num3;
				}
				result = matchResultInfo;
			}
			return result;
		}
	}
}
