using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000E3 RID: 227
	[MiscellaneousBits(1)]
	public struct LockedBox
	{
		// Token: 0x0600039A RID: 922 RVA: 0x000619FC File Offset: 0x00060DFC
		public unsafe override string ToString()
		{
			object obj = this;
			Type type = obj.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null && !getMethod.IsStatic)
					{
						object obj2 = getMethod.Invoke(obj, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$118$];
						array[0] = properties[num].Name;
						string text;
						if (obj2 != null)
						{
							text = obj2.ToString();
						}
						else
						{
							text = string.Empty;
						}
						array[1] = text;
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array));
					}
					num++;
				}
				while (num < properties.Length);
			}
			FieldInfo[] fields = type.GetFields();
			int num2 = 0;
			if (0 < fields.Length)
			{
				do
				{
					object value = fields[num2].GetValue(obj);
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$119$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00061B28 File Offset: 0x00060F28
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00061B40 File Offset: 0x00060F40
		public int RowPitch
		{
			get
			{
				return this.iRowPitch;
			}
			set
			{
				this.iRowPitch = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00061B5C File Offset: 0x00060F5C
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00061B74 File Offset: 0x00060F74
		public int SlicePitch
		{
			get
			{
				return this.iSlicePitch;
			}
			set
			{
				this.iSlicePitch = value;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00061B90 File Offset: 0x00060F90
		public LockedBox()
		{
			ref LockedBox lockedBox& = ref this;
			initblk(ref lockedBox&, 0, sizeof(LockedBox));
		}

		// Token: 0x04000F7E RID: 3966
		internal int iRowPitch;

		// Token: 0x04000F7F RID: 3967
		internal int iSlicePitch;
	}
}
