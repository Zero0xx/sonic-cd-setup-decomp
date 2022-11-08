using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000C4 RID: 196
	[MiscellaneousBits(1)]
	public struct SurfaceDescription
	{
		// Token: 0x06000301 RID: 769 RVA: 0x0005F92C File Offset: 0x0005ED2C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$88$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$89$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0005FA58 File Offset: 0x0005EE58
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0005FA70 File Offset: 0x0005EE70
		public Format Format
		{
			get
			{
				return this.m_Format;
			}
			set
			{
				this.m_Format = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0005FA8C File Offset: 0x0005EE8C
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0005FAA4 File Offset: 0x0005EEA4
		public ResourceType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0005FAC0 File Offset: 0x0005EEC0
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0005FAD8 File Offset: 0x0005EED8
		public Usage Usage
		{
			get
			{
				return this.m_Usage;
			}
			set
			{
				this.m_Usage = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0005FAF4 File Offset: 0x0005EEF4
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0005FB0C File Offset: 0x0005EF0C
		public Pool Pool
		{
			get
			{
				return this.m_Pool;
			}
			set
			{
				this.m_Pool = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0005FB28 File Offset: 0x0005EF28
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0005FB40 File Offset: 0x0005EF40
		public MultiSampleType MultiSampleType
		{
			get
			{
				return this.m_MultiSampleType;
			}
			set
			{
				this.m_MultiSampleType = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0005FB5C File Offset: 0x0005EF5C
		// (set) Token: 0x0600030D RID: 781 RVA: 0x0005FB74 File Offset: 0x0005EF74
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

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0005FB90 File Offset: 0x0005EF90
		// (set) Token: 0x0600030F RID: 783 RVA: 0x0005FBA8 File Offset: 0x0005EFA8
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

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0005FBC4 File Offset: 0x0005EFC4
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0005FBDC File Offset: 0x0005EFDC
		public int MultiSampleQuality
		{
			get
			{
				return this.m_MultiSampleQuality;
			}
			set
			{
				this.m_MultiSampleQuality = value;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0005FBF8 File Offset: 0x0005EFF8
		public SurfaceDescription()
		{
			ref SurfaceDescription surfaceDescription& = ref this;
			initblk(ref surfaceDescription&, 0, 32);
		}

		// Token: 0x04000F24 RID: 3876
		private Format m_Format;

		// Token: 0x04000F25 RID: 3877
		private ResourceType m_Type;

		// Token: 0x04000F26 RID: 3878
		private Usage m_Usage;

		// Token: 0x04000F27 RID: 3879
		private Pool m_Pool;

		// Token: 0x04000F28 RID: 3880
		private MultiSampleType m_MultiSampleType;

		// Token: 0x04000F29 RID: 3881
		private int m_MultiSampleQuality;

		// Token: 0x04000F2A RID: 3882
		private int m_Width;

		// Token: 0x04000F2B RID: 3883
		private int m_Height;
	}
}
