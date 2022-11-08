using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000DF RID: 223
	[MiscellaneousBits(1)]
	public struct BandwidthTimings
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0006166C File Offset: 0x00060A6C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$114$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$115$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00061798 File Offset: 0x00060B98
		public float MaxBandwidthUtilized
		{
			get
			{
				return this.fMaxBandwidthUtilized;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600038F RID: 911 RVA: 0x000617B0 File Offset: 0x00060BB0
		public float FrontEndUploadMemoryUtilizedPercent
		{
			get
			{
				return this.fFrontEndUploadMemoryUtilizedPercent;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000617C8 File Offset: 0x00060BC8
		public float VertexRateUtilizedPercent
		{
			get
			{
				return this.fVertexRateUtilizedPercent;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000391 RID: 913 RVA: 0x000617E0 File Offset: 0x00060BE0
		public float TriangleSetupRateUtilizedPercent
		{
			get
			{
				return this.fTriangleSetupRateUtilizedPercent;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000392 RID: 914 RVA: 0x000617F8 File Offset: 0x00060BF8
		public float FillRateUtilizedPercent
		{
			get
			{
				return this.fFillRateUtilizedPercent;
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00061810 File Offset: 0x00060C10
		public BandwidthTimings()
		{
			ref BandwidthTimings bandwidthTimings& = ref this;
			initblk(ref bandwidthTimings&, 0, 20);
		}

		// Token: 0x04000F75 RID: 3957
		internal float fMaxBandwidthUtilized;

		// Token: 0x04000F76 RID: 3958
		internal float fFrontEndUploadMemoryUtilizedPercent;

		// Token: 0x04000F77 RID: 3959
		internal float fVertexRateUtilizedPercent;

		// Token: 0x04000F78 RID: 3960
		internal float fTriangleSetupRateUtilizedPercent;

		// Token: 0x04000F79 RID: 3961
		internal float fFillRateUtilizedPercent;
	}
}
