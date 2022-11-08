using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000BC RID: 188
	[MiscellaneousBits(1)]
	public struct RectanglePatchInformation
	{
		// Token: 0x060002CA RID: 714 RVA: 0x0005EED4 File Offset: 0x0005E2D4
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$80$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$81$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0005F000 File Offset: 0x0005E400
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0005F018 File Offset: 0x0005E418
		public int StartVertexOffsetWidth
		{
			get
			{
				return this.m_StartVertexOffsetWidth;
			}
			set
			{
				this.m_StartVertexOffsetWidth = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0005F034 File Offset: 0x0005E434
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0005F04C File Offset: 0x0005E44C
		public int StartVertexOffsetHeight
		{
			get
			{
				return this.m_StartVertexOffsetHeight;
			}
			set
			{
				this.m_StartVertexOffsetHeight = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0005F068 File Offset: 0x0005E468
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0005F080 File Offset: 0x0005E480
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

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0005F09C File Offset: 0x0005E49C
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0005F0B4 File Offset: 0x0005E4B4
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0005F0D0 File Offset: 0x0005E4D0
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0005F0E8 File Offset: 0x0005E4E8
		public int Stride
		{
			get
			{
				return this.m_Stride;
			}
			set
			{
				this.m_Stride = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0005F104 File Offset: 0x0005E504
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0005F11C File Offset: 0x0005E51C
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

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0005F138 File Offset: 0x0005E538
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0005F150 File Offset: 0x0005E550
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

		// Token: 0x060002D9 RID: 729 RVA: 0x0005F16C File Offset: 0x0005E56C
		public RectanglePatchInformation()
		{
			ref RectanglePatchInformation rectanglePatchInformation& = ref this;
			initblk(ref rectanglePatchInformation&, 0, 28);
		}

		// Token: 0x04000F0B RID: 3851
		private int m_StartVertexOffsetWidth;

		// Token: 0x04000F0C RID: 3852
		private int m_StartVertexOffsetHeight;

		// Token: 0x04000F0D RID: 3853
		private int m_Width;

		// Token: 0x04000F0E RID: 3854
		private int m_Height;

		// Token: 0x04000F0F RID: 3855
		private int m_Stride;

		// Token: 0x04000F10 RID: 3856
		private BasisType m_Basis;

		// Token: 0x04000F11 RID: 3857
		private DegreeType m_Order;
	}
}
