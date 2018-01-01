using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public class RectField : BaseCompoundField<Rect>
	{
		internal override BaseCompoundField<Rect>.FieldDescription[] DescribeFields()
		{
			BaseCompoundField<Rect>.FieldDescription[] expr_07 = new BaseCompoundField<Rect>.FieldDescription[4];
			expr_07[0] = new BaseCompoundField<Rect>.FieldDescription("X", (Rect r) => (double)r.x, delegate(ref Rect r, double v)
			{
				r.x = (float)v;
			});
			expr_07[1] = new BaseCompoundField<Rect>.FieldDescription("Y", (Rect r) => (double)r.y, delegate(ref Rect r, double v)
			{
				r.y = (float)v;
			});
			expr_07[2] = new BaseCompoundField<Rect>.FieldDescription("W", (Rect r) => (double)r.width, delegate(ref Rect r, double v)
			{
				r.width = (float)v;
			});
			expr_07[3] = new BaseCompoundField<Rect>.FieldDescription("H", (Rect r) => (double)r.height, delegate(ref Rect r, double v)
			{
				r.height = (float)v;
			});
			return expr_07;
		}
	}
}
