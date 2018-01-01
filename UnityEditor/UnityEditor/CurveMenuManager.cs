using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
	internal class CurveMenuManager
	{
		private CurveUpdater updater;

		public CurveMenuManager(CurveUpdater updater)
		{
			this.updater = updater;
		}

		public void AddTangentMenuItems(GenericMenu menu, List<KeyIdentifier> keyList)
		{
			bool flag = keyList.Count > 0;
			bool on = flag;
			bool on2 = flag;
			bool on3 = flag;
			bool on4 = flag;
			bool on5 = flag;
			bool flag2 = flag;
			bool flag3 = flag;
			bool on6 = flag;
			bool flag4 = flag;
			bool flag5 = flag;
			bool flag6 = flag;
			bool on7 = flag;
			bool flag7 = flag;
			foreach (KeyIdentifier current in keyList)
			{
				Keyframe keyframe = current.keyframe;
				AnimationUtility.TangentMode keyLeftTangentMode = AnimationUtility.GetKeyLeftTangentMode(keyframe);
				AnimationUtility.TangentMode keyRightTangentMode = AnimationUtility.GetKeyRightTangentMode(keyframe);
				bool keyBroken = AnimationUtility.GetKeyBroken(keyframe);
				if (keyLeftTangentMode != AnimationUtility.TangentMode.ClampedAuto || keyRightTangentMode != AnimationUtility.TangentMode.ClampedAuto)
				{
					on = false;
				}
				if (keyLeftTangentMode != AnimationUtility.TangentMode.Auto || keyRightTangentMode != AnimationUtility.TangentMode.Auto)
				{
					on2 = false;
				}
				if (keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Free || keyRightTangentMode != AnimationUtility.TangentMode.Free)
				{
					on3 = false;
				}
				if (keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Free || keyframe.inTangent != 0f || keyRightTangentMode != AnimationUtility.TangentMode.Free || keyframe.outTangent != 0f)
				{
					on4 = false;
				}
				if (!keyBroken)
				{
					on5 = false;
				}
				if (!keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Free)
				{
					flag3 = false;
				}
				if (!keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Linear)
				{
					on6 = false;
				}
				if (!keyBroken || keyLeftTangentMode != AnimationUtility.TangentMode.Constant)
				{
					flag4 = false;
				}
				if (!keyBroken || keyRightTangentMode != AnimationUtility.TangentMode.Free)
				{
					flag6 = false;
				}
				if (!keyBroken || keyRightTangentMode != AnimationUtility.TangentMode.Linear)
				{
					on7 = false;
				}
				if (!keyBroken || keyRightTangentMode != AnimationUtility.TangentMode.Constant)
				{
					flag7 = false;
				}
				if ((keyframe.weightedMode & WeightedMode.In) == WeightedMode.None)
				{
					flag2 = false;
				}
				if ((keyframe.weightedMode & WeightedMode.Out) == WeightedMode.None)
				{
					flag5 = false;
				}
			}
			if (flag)
			{
				menu.AddItem(EditorGUIUtility.TrTextContent("Clamped Auto", null, null), on, new GenericMenu.MenuFunction2(this.SetClampedAuto), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Auto", null, null), on2, new GenericMenu.MenuFunction2(this.SetAuto), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Free Smooth", null, null), on3, new GenericMenu.MenuFunction2(this.SetEditable), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Flat", null, null), on4, new GenericMenu.MenuFunction2(this.SetFlat), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Broken", null, null), on5, new GenericMenu.MenuFunction2(this.SetBroken), keyList);
				menu.AddSeparator("");
				menu.AddItem(EditorGUIUtility.TrTextContent("Left Tangent/Free", null, null), flag3, new GenericMenu.MenuFunction2(this.SetLeftEditable), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Left Tangent/Linear", null, null), on6, new GenericMenu.MenuFunction2(this.SetLeftLinear), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Left Tangent/Constant", null, null), flag4, new GenericMenu.MenuFunction2(this.SetLeftConstant), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Left Tangent/Weighted", null, null), flag2, new GenericMenu.MenuFunction2(this.ToggleLeftWeighted), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Right Tangent/Linear", null, null), on7, new GenericMenu.MenuFunction2(this.SetRightLinear), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Right Tangent/Constant", null, null), flag7, new GenericMenu.MenuFunction2(this.SetRightConstant), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Both Tangents/Free", null, null), flag6 && flag3, new GenericMenu.MenuFunction2(this.SetBothEditable), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Right Tangent/Weighted", null, null), flag5, new GenericMenu.MenuFunction2(this.ToggleRightWeighted), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Both Tangents/Constant", null, null), flag7 && flag4, new GenericMenu.MenuFunction2(this.SetBothConstant), keyList);
				menu.AddItem(EditorGUIUtility.TrTextContent("Both Tangents/Weighted", null, null), flag5 && flag2, new GenericMenu.MenuFunction2(this.ToggleBothWeighted), keyList);
			}
			else
			{
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Weighted", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Auto", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Free Smooth", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Flat", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Broken", null, null));
				menu.AddSeparator("");
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Left Tangent/Free", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Left Tangent/Linear", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Left Tangent/Constant", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Right Tangent/Free", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Right Tangent/Linear", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Right Tangent/Constant", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Both Tangents/Free", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Both Tangents/Linear", null, null));
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Both Tangents/Constant", null, null));
			}
		}

		public void ToggleLeftWeighted(object keysToSet)
		{
			this.ToggleWeighted(WeightedMode.In, (List<KeyIdentifier>)keysToSet);
		}

		public void ToggleRightWeighted(object keysToSet)
		{
			this.ToggleWeighted(WeightedMode.Out, (List<KeyIdentifier>)keysToSet);
		}

		public void ToggleBothWeighted(object keysToSet)
		{
			this.ToggleWeighted(WeightedMode.Both, (List<KeyIdentifier>)keysToSet);
		}

		public void ToggleWeighted(WeightedMode weightedMode, List<KeyIdentifier> keysToSet)
		{
			bool flag = keysToSet.TrueForAll((KeyIdentifier key) => (key.keyframe.weightedMode & weightedMode) == weightedMode);
			List<ChangedCurve> list = new List<ChangedCurve>();
			foreach (KeyIdentifier current in keysToSet)
			{
				AnimationCurve curve = current.curve;
				Keyframe keyframe = current.keyframe;
				bool flag2 = (keyframe.weightedMode & weightedMode) == weightedMode;
				if (flag2 == flag)
				{
					WeightedMode weightedMode2 = keyframe.weightedMode;
					keyframe.weightedMode = ((!flag2) ? (keyframe.weightedMode | weightedMode) : (keyframe.weightedMode & ~weightedMode));
					if (keyframe.weightedMode != WeightedMode.None)
					{
						AnimationUtility.TangentMode keyRightTangentMode = AnimationUtility.GetKeyRightTangentMode(keyframe);
						AnimationUtility.TangentMode keyLeftTangentMode = AnimationUtility.GetKeyLeftTangentMode(keyframe);
						if ((weightedMode2 & WeightedMode.Out) == WeightedMode.None && (keyframe.weightedMode & WeightedMode.Out) == WeightedMode.Out)
						{
							if (keyRightTangentMode == AnimationUtility.TangentMode.Linear || keyRightTangentMode == AnimationUtility.TangentMode.Constant)
							{
								AnimationUtility.SetKeyRightTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
							}
							if (current.key < curve.length - 1)
							{
								keyframe.outWeight = 0.333333343f;
							}
						}
						if ((weightedMode2 & WeightedMode.In) == WeightedMode.None && (keyframe.weightedMode & WeightedMode.In) == WeightedMode.In)
						{
							if (keyLeftTangentMode == AnimationUtility.TangentMode.Linear || keyLeftTangentMode == AnimationUtility.TangentMode.Constant)
							{
								AnimationUtility.SetKeyLeftTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
							}
							if (current.key > 0)
							{
								keyframe.inWeight = 0.333333343f;
							}
						}
					}
					curve.MoveKey(current.key, keyframe);
					AnimationUtility.UpdateTangentsFromModeSurrounding(curve, current.key);
					ChangedCurve item = new ChangedCurve(curve, current.curveId, current.binding);
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			this.updater.UpdateCurves(list, "Toggle Weighted");
		}

		public void SetClampedAuto(object keysToSet)
		{
			this.SetBoth(AnimationUtility.TangentMode.ClampedAuto, (List<KeyIdentifier>)keysToSet);
		}

		public void SetAuto(object keysToSet)
		{
			this.SetBoth(AnimationUtility.TangentMode.Auto, (List<KeyIdentifier>)keysToSet);
		}

		public void SetEditable(object keysToSet)
		{
			this.SetBoth(AnimationUtility.TangentMode.Free, (List<KeyIdentifier>)keysToSet);
		}

		public void SetFlat(object keysToSet)
		{
			this.SetBoth(AnimationUtility.TangentMode.Free, (List<KeyIdentifier>)keysToSet);
			this.Flatten((List<KeyIdentifier>)keysToSet);
		}

		public void SetBoth(AnimationUtility.TangentMode mode, List<KeyIdentifier> keysToSet)
		{
			List<ChangedCurve> list = new List<ChangedCurve>();
			foreach (KeyIdentifier current in keysToSet)
			{
				AnimationCurve curve = current.curve;
				Keyframe keyframe = current.keyframe;
				AnimationUtility.SetKeyBroken(ref keyframe, false);
				AnimationUtility.SetKeyRightTangentMode(ref keyframe, mode);
				AnimationUtility.SetKeyLeftTangentMode(ref keyframe, mode);
				if (mode == AnimationUtility.TangentMode.Free)
				{
					float num = CurveUtility.CalculateSmoothTangent(keyframe);
					keyframe.inTangent = num;
					keyframe.outTangent = num;
				}
				curve.MoveKey(current.key, keyframe);
				AnimationUtility.UpdateTangentsFromModeSurrounding(curve, current.key);
				ChangedCurve item = new ChangedCurve(curve, current.curveId, current.binding);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			this.updater.UpdateCurves(list, "Set Tangents");
		}

		public void Flatten(List<KeyIdentifier> keysToSet)
		{
			List<ChangedCurve> list = new List<ChangedCurve>();
			foreach (KeyIdentifier current in keysToSet)
			{
				AnimationCurve curve = current.curve;
				Keyframe keyframe = current.keyframe;
				keyframe.inTangent = 0f;
				keyframe.outTangent = 0f;
				curve.MoveKey(current.key, keyframe);
				AnimationUtility.UpdateTangentsFromModeSurrounding(curve, current.key);
				ChangedCurve item = new ChangedCurve(curve, current.curveId, current.binding);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			this.updater.UpdateCurves(list, "Set Tangents");
		}

		public void SetBroken(object _keysToSet)
		{
			List<ChangedCurve> list = new List<ChangedCurve>();
			List<KeyIdentifier> list2 = (List<KeyIdentifier>)_keysToSet;
			foreach (KeyIdentifier current in list2)
			{
				AnimationCurve curve = current.curve;
				Keyframe keyframe = current.keyframe;
				AnimationUtility.SetKeyBroken(ref keyframe, true);
				if (AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
				{
					AnimationUtility.SetKeyRightTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
				}
				if (AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
				{
					AnimationUtility.SetKeyLeftTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
				}
				curve.MoveKey(current.key, keyframe);
				AnimationUtility.UpdateTangentsFromModeSurrounding(curve, current.key);
				ChangedCurve item = new ChangedCurve(curve, current.curveId, current.binding);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			this.updater.UpdateCurves(list, "Set Tangents");
		}

		public void SetLeftEditable(object keysToSet)
		{
			this.SetTangent(0, AnimationUtility.TangentMode.Free, (List<KeyIdentifier>)keysToSet);
		}

		public void SetLeftLinear(object keysToSet)
		{
			this.SetTangent(0, AnimationUtility.TangentMode.Linear, (List<KeyIdentifier>)keysToSet);
		}

		public void SetLeftConstant(object keysToSet)
		{
			this.SetTangent(0, AnimationUtility.TangentMode.Constant, (List<KeyIdentifier>)keysToSet);
		}

		public void SetRightEditable(object keysToSet)
		{
			this.SetTangent(1, AnimationUtility.TangentMode.Free, (List<KeyIdentifier>)keysToSet);
		}

		public void SetRightLinear(object keysToSet)
		{
			this.SetTangent(1, AnimationUtility.TangentMode.Linear, (List<KeyIdentifier>)keysToSet);
		}

		public void SetRightConstant(object keysToSet)
		{
			this.SetTangent(1, AnimationUtility.TangentMode.Constant, (List<KeyIdentifier>)keysToSet);
		}

		public void SetBothEditable(object keysToSet)
		{
			this.SetTangent(2, AnimationUtility.TangentMode.Free, (List<KeyIdentifier>)keysToSet);
		}

		public void SetBothLinear(object keysToSet)
		{
			this.SetTangent(2, AnimationUtility.TangentMode.Linear, (List<KeyIdentifier>)keysToSet);
		}

		public void SetBothConstant(object keysToSet)
		{
			this.SetTangent(2, AnimationUtility.TangentMode.Constant, (List<KeyIdentifier>)keysToSet);
		}

		public void SetTangent(int leftRight, AnimationUtility.TangentMode mode, List<KeyIdentifier> keysToSet)
		{
			List<ChangedCurve> list = new List<ChangedCurve>();
			foreach (KeyIdentifier current in keysToSet)
			{
				AnimationCurve curve = current.curve;
				Keyframe keyframe = current.keyframe;
				AnimationUtility.SetKeyBroken(ref keyframe, true);
				if (leftRight == 2)
				{
					AnimationUtility.SetKeyLeftTangentMode(ref keyframe, mode);
					AnimationUtility.SetKeyRightTangentMode(ref keyframe, mode);
				}
				else if (leftRight == 0)
				{
					AnimationUtility.SetKeyLeftTangentMode(ref keyframe, mode);
					if (AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyRightTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
					{
						AnimationUtility.SetKeyRightTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
					}
				}
				else
				{
					AnimationUtility.SetKeyRightTangentMode(ref keyframe, mode);
					if (AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.ClampedAuto || AnimationUtility.GetKeyLeftTangentMode(keyframe) == AnimationUtility.TangentMode.Auto)
					{
						AnimationUtility.SetKeyLeftTangentMode(ref keyframe, AnimationUtility.TangentMode.Free);
					}
				}
				if (mode == AnimationUtility.TangentMode.Constant && (leftRight == 0 || leftRight == 2))
				{
					keyframe.inTangent = float.PositiveInfinity;
				}
				if (mode == AnimationUtility.TangentMode.Constant && (leftRight == 1 || leftRight == 2))
				{
					keyframe.outTangent = float.PositiveInfinity;
				}
				curve.MoveKey(current.key, keyframe);
				AnimationUtility.UpdateTangentsFromModeSurrounding(curve, current.key);
				ChangedCurve item = new ChangedCurve(curve, current.curveId, current.binding);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			this.updater.UpdateCurves(list, "Set Tangents");
		}
	}
}
