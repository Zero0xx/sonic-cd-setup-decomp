using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000BE RID: 190
	[MiscellaneousBits(1)]
	public struct TrianglePatchInformation
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0005F1B4 File Offset: 0x0005E5B4
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$82$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$83$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0005F2E0 File Offset: 0x0005E6E0
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0005F2F8 File Offset: 0x0005E6F8
		public int StartVertexOffset
		{
			get
			{
				return this.m_StartVertexOffset;
			}
			set
			{
				this.m_StartVertexOffset = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0005F314 File Offset: 0x0005E714
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0005F32C File Offset: 0x0005E72C
		public int NumberVertices
		{
			get
			{
				return this.m_NumVertices;
			}
			set
			{
				this.m_NumVertices = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0005F348 File Offset: 0x0005E748
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0005F360 File Offset: 0x0005E760
		public BasisType BasisType
		{
			get
			{
				return this.m_Basis;
			}
			set
			{
				this.m_Basis = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0005F37C File Offset: 0x0005E77C
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0005F394 File Offset: 0x0005E794
		public DegreeType Degree
		{
			get
			{
				return this.m_Order;
			}
			set
			{
				this.m_Order = value;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0005F3B0 File Offset: 0x0005E7B0
		public TrianglePatchInformation()
		{
			ref TrianglePatchInformation trianglePatchInformation& = ref this;
			initblk(ref trianglePatchInformation&, 0, 16);
		}

		// Token: 0x04000F13 RID: 3859
		private int m_StartVertexOffset;

		// Token: 0x04000F14 RID: 3860
		private int m_NumVertices;

		// Token: 0x04000F15 RID: 3861
		private BasisType m_Basis;

		// Token: 0x04000F16 RID: 3862
		private DegreeType m_Order;
	}
}
