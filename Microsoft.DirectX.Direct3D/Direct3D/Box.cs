using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000C8 RID: 200
	[MiscellaneousBits(1)]
	public struct Box
	{
		// Token: 0x06000325 RID: 805 RVA: 0x0005FF20 File Offset: 0x0005F320
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$92$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$93$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0006004C File Offset: 0x0005F44C
		// (set) Token: 0x06000327 RID: 807 RVA: 0x00060064 File Offset: 0x0005F464
		public int Left
		{
			get
			{
				return this.m_Left;
			}
			set
			{
				this.m_Left = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00060080 File Offset: 0x0005F480
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00060098 File Offset: 0x0005F498
		public int Top
		{
			get
			{
				return this.m_Top;
			}
			set
			{
				this.m_Top = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600032A RID: 810 RVA: 0x000600B4 File Offset: 0x0005F4B4
		// (set) Token: 0x0600032B RID: 811 RVA: 0x000600CC File Offset: 0x0005F4CC
		public int Right
		{
			get
			{
				return this.m_Right;
			}
			set
			{
				this.m_Right = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000600E8 File Offset: 0x0005F4E8
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00060100 File Offset: 0x0005F500
		public int Bottom
		{
			get
			{
				return this.m_Bottom;
			}
			set
			{
				this.m_Bottom = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0006011C File Offset: 0x0005F51C
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00060134 File Offset: 0x0005F534
		public int Front
		{
			get
			{
				return this.m_Front;
			}
			set
			{
				this.m_Front = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00060150 File Offset: 0x0005F550
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00060168 File Offset: 0x0005F568
		public int Back
		{
			get
			{
				return this.m_Back;
			}
			set
			{
				this.m_Back = value;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00060184 File Offset: 0x0005F584
		public Box()
		{
			ref Box box& = ref this;
			initblk(ref box&, 0, 24);
		}

		// Token: 0x04000F35 RID: 3893
		private int m_Left;

		// Token: 0x04000F36 RID: 3894
		private int m_Top;

		// Token: 0x04000F37 RID: 3895
		private int m_Right;

		// Token: 0x04000F38 RID: 3896
		private int m_Bottom;

		// Token: 0x04000F39 RID: 3897
		private int m_Front;

		// Token: 0x04000F3A RID: 3898
		private int m_Back;
	}
}
