using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public class Vector3Field : BaseCompoundField<Vector3>
	{
		internal override BaseCompoundField<Vector3>.FieldDescription[] DescribeFields()
		{
			BaseCompoundField<Vector3>.FieldDescription[] expr_07 = new BaseCompoundField<Vector3>.FieldDescription[3];
			expr_07[0] = new BaseCompoundField<Vector3>.FieldDescription("X", (Vector3 r) => (double)r.x, delegate(ref Vector3 r, double v)
			{
				r.x = (float)v;
			});
			expr_07[1] = new BaseCompoundField<Vector3>.FieldDescription("Y", (Vector3 r) => (double)r.y, delegate(ref Vector3 r, double v)
			{
				r.y = (float)v;
			});
			expr_07[2] = new BaseCompoundField<Vector3>.FieldDescription("Z", (Vector3 r) => (double)r.z, delegate(ref Vector3 r, double v)
			{
				r.z = (float)v;
			});
			return expr_07;
		}
	}
}
