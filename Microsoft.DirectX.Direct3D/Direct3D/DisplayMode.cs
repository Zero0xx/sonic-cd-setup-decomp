using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000AA RID: 170
	[MiscellaneousBits(1)]
	public struct DisplayMode
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0005CAA4 File Offset: 0x0005BEA4
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0005CABC File Offset: 0x0005BEBC
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0005CAD8 File Offset: 0x0005BED8
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0005CAF0 File Offset: 0x0005BEF0
		public int RefreshRate
		{
			get
			{
				return this.m_RefreshRate;
			}
			set
			{
				this.m_RefreshRate = value;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0005CB0C File Offset: 0x0005BF0C
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0005CB24 File Offset: 0x0005BF24
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0005CB40 File Offset: 0x0005BF40
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0005CB58 File Offset: 0x0005BF58
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

		// Token: 0x06000235 RID: 565 RVA: 0x0005CB74 File Offset: 0x0005BF74
		public DisplayMode()
		{
			ref DisplayMode displayMode& = ref this;
			initblk(ref displayMode&, 0, 16);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0005CB90 File Offset: 0x0005BF90
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$66$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$67$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000EEB RID: 3819
		private int m_Width;

		// Token: 0x04000EEC RID: 3820
		private int m_Height;

		// Token: 0x04000EED RID: 3821
		private int m_RefreshRate;

		// Token: 0x04000EEE RID: 3822
		private Format m_Format;
	}
}
