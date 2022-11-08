using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000D4 RID: 212
	[MiscellaneousBits(1)]
	public struct ResourceStats
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00060CD4 File Offset: 0x000600D4
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$104$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$105$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00060E00 File Offset: 0x00060200
		public bool Thrashing
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.bThrashing == 1;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00060E1C File Offset: 0x0006021C
		public int ApproximateBytesDownloaded
		{
			get
			{
				return (int)this.dwApproxBytesDownloaded;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00060E34 File Offset: 0x00060234
		public int NumberEvicts
		{
			get
			{
				return (int)this.dwNumEvicts;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00060E4C File Offset: 0x0006024C
		public int NumberObjectsCreatedInVideoMemory
		{
			get
			{
				return (int)this.dwNumVidCreates;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00060E64 File Offset: 0x00060264
		public int LastObjectPriority
		{
			get
			{
				return (int)this.dwLastPri;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00060E7C File Offset: 0x0006027C
		public int NumberObjectsUsed
		{
			get
			{
				return (int)this.dwNumUsed;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00060E94 File Offset: 0x00060294
		public int NumberObjectsUsedInVideoMemory
		{
			get
			{
				return (int)this.dwNumUsedInVidMem;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00060EAC File Offset: 0x000602AC
		public int WorkingSet
		{
			get
			{
				return (int)this.dwWorkingSet;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00060EC4 File Offset: 0x000602C4
		public int WorkingSetBytes
		{
			get
			{
				return (int)this.dwWorkingSetBytes;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00060EDC File Offset: 0x000602DC
		public int TotalManaged
		{
			get
			{
				return (int)this.dwTotalManaged;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00060EF4 File Offset: 0x000602F4
		public int TotalManagedBytes
		{
			get
			{
				return (int)this.dwTotalBytes;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00060F0C File Offset: 0x0006030C
		public ResourceStats()
		{
			ref ResourceStats resourceStats& = ref this;
			initblk(ref resourceStats&, 0, 44);
		}

		// Token: 0x04000F59 RID: 3929
		internal int bThrashing;

		// Token: 0x04000F5A RID: 3930
		internal uint dwApproxBytesDownloaded;

		// Token: 0x04000F5B RID: 3931
		internal uint dwNumEvicts;

		// Token: 0x04000F5C RID: 3932
		internal uint dwNumVidCreates;

		// Token: 0x04000F5D RID: 3933
		internal uint dwLastPri;

		// Token: 0x04000F5E RID: 3934
		internal uint dwNumUsed;

		// Token: 0x04000F5F RID: 3935
		internal uint dwNumUsedInVidMem;

		// Token: 0x04000F60 RID: 3936
		internal uint dwWorkingSet;

		// Token: 0x04000F61 RID: 3937
		internal uint dwWorkingSetBytes;

		// Token: 0x04000F62 RID: 3938
		internal uint dwTotalManaged;

		// Token: 0x04000F63 RID: 3939
		internal uint dwTotalBytes;
	}
}
