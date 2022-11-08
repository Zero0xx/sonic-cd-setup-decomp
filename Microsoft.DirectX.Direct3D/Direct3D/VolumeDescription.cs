using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000C6 RID: 198
	[MiscellaneousBits(1)]
	public struct VolumeDescription
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0005FC40 File Offset: 0x0005F040
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$90$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$91$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0005FD6C File Offset: 0x0005F16C
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0005FD84 File Offset: 0x0005F184
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

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0005FDA0 File Offset: 0x0005F1A0
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0005FDB8 File Offset: 0x0005F1B8
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0005FDD4 File Offset: 0x0005F1D4
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0005FDEC File Offset: 0x0005F1EC
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0005FE08 File Offset: 0x0005F208
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0005FE20 File Offset: 0x0005F220
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

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0005FE3C File Offset: 0x0005F23C
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0005FE54 File Offset: 0x0005F254
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

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0005FE70 File Offset: 0x0005F270
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0005FE88 File Offset: 0x0005F288
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

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0005FEA4 File Offset: 0x0005F2A4
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0005FEBC File Offset: 0x0005F2BC
		public int Depth
		{
			get
			{
				return this.m_Depth;
			}
			set
			{
				this.m_Depth = value;
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0005FED8 File Offset: 0x0005F2D8
		public VolumeDescription()
		{
			ref VolumeDescription volumeDescription& = ref this;
			initblk(ref volumeDescription&, 0, 28);
		}

		// Token: 0x04000F2D RID: 3885
		private Format m_Format;

		// Token: 0x04000F2E RID: 3886
		private ResourceType m_Type;

		// Token: 0x04000F2F RID: 3887
		private Usage m_Usage;

		// Token: 0x04000F30 RID: 3888
		private Pool m_Pool;

		// Token: 0x04000F31 RID: 3889
		private int m_Width;

		// Token: 0x04000F32 RID: 3890
		private int m_Height;

		// Token: 0x04000F33 RID: 3891
		private int m_Depth;
	}
}
