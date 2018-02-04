using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public class Vector2Field : BaseCompoundField<Vector2>
	{
		internal override BaseCompoundField<Vector2>.FieldDescription[] DescribeFields()
		{
			BaseCompoundField<Vector2>.FieldDescription[] expr_07 = new BaseCompoundField<Vector2>.FieldDescription[2];
			expr_07[0] = new BaseCompoundField<Vector2>.FieldDescription("X", (Vector2 r) => (double)r.x, delegate(ref Vector2 r, double v)
			{
				r.x = (float)v;
			});
			expr_07[1] = new BaseCompoundField<Vector2>.FieldDescription("Y", (Vector2 r) => (double)r.y, delegate(ref Vector2 r, double v)
			{
				r.y = (float)v;
			});
			return expr_07;
		}
	}
}
