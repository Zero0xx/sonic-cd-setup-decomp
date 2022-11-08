using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000C0 RID: 192
	[MiscellaneousBits(1)]
	public struct Viewport
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0005F3F8 File Offset: 0x0005E7F8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$84$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$85$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0005F524 File Offset: 0x0005E924
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0005F53C File Offset: 0x0005E93C
		public int X
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0005F558 File Offset: 0x0005E958
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0005F570 File Offset: 0x0005E970
		public int Y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0005F58C File Offset: 0x0005E98C
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0005F5A4 File Offset: 0x0005E9A4
		public int Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0005F5C0 File Offset: 0x0005E9C0
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0005F5D8 File Offset: 0x0005E9D8
		public int Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0005F5F4 File Offset: 0x0005E9F4
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x0005F60C File Offset: 0x0005EA0C
		public float MinZ
		{
			get
			{
				return this.m_MinZ;
			}
			set
			{
				this.m_MinZ = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0005F628 File Offset: 0x0005EA28
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0005F640 File Offset: 0x0005EA40
		public float MaxZ
		{
			get
			{
				return this.m_MaxZ;
			}
			set
			{
				this.m_MaxZ = value;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0005F65C File Offset: 0x0005EA5C
		public Viewport()
		{
			ref Viewport viewport& = ref this;
			initblk(ref viewport&, 0, 24);
		}

		// Token: 0x04000F18 RID: 3864
		private int m_X;

		// Token: 0x04000F19 RID: 3865
		private int m_Y;

		// Token: 0x04000F1A RID: 3866
		private int m_Width;

		// Token: 0x04000F1B RID: 3867
		private int m_Height;

		// Token: 0x04000F1C RID: 3868
		private float m_MinZ;

		// Token: 0x04000F1D RID: 3869
		private float m_MaxZ;
	}
}
