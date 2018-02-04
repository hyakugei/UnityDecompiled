using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public class Vector4Field : BaseCompoundField<Vector4>
	{
		internal override BaseCompoundField<Vector4>.FieldDescription[] DescribeFields()
		{
			BaseCompoundField<Vector4>.FieldDescription[] expr_07 = new BaseCompoundField<Vector4>.FieldDescription[4];
			expr_07[0] = new BaseCompoundField<Vector4>.FieldDescription("X", (Vector4 r) => (double)r.x, delegate(ref Vector4 r, double v)
			{
				r.x = (float)v;
			});
			expr_07[1] = new BaseCompoundField<Vector4>.FieldDescription("Y", (Vector4 r) => (double)r.y, delegate(ref Vector4 r, double v)
			{
				r.y = (float)v;
			});
			expr_07[2] = new BaseCompoundField<Vector4>.FieldDescription("Z", (Vector4 r) => (double)r.z, delegate(ref Vector4 r, double v)
			{
				r.z = (float)v;
			});
			expr_07[3] = new BaseCompoundField<Vector4>.FieldDescription("W", (Vector4 r) => (double)r.w, delegate(ref Vector4 r, double v)
			{
				r.w = (float)v;
			});
			return expr_07;
		}
	}
}
