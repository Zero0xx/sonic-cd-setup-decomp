using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000DB RID: 219
	[MiscellaneousBits(1)]
	public struct InterfaceTimings
	{
		// Token: 0x06000380 RID: 896 RVA: 0x000612DC File Offset: 0x000606DC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$110$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$111$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00061408 File Offset: 0x00060808
		public float WaitingForGpuToUseApplicationResourceTimePercent
		{
			get
			{
				return this.fWaitingForGPUToUseApplicationResourceTimePercent;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00061420 File Offset: 0x00060820
		public float WaitingForGpuToAcceptMoreCommandsTimePercent
		{
			get
			{
				return this.fWaitingForGPUToAcceptMoreCommandsTimePercent;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00061438 File Offset: 0x00060838
		public float WaitingForGpuToStayWithinLatencyTimePercent
		{
			get
			{
				return this.fWaitingForGPUToStayWithinLatencyTimePercent;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00061450 File Offset: 0x00060850
		public float WaitingForGpuExclusiveResourceTimePercent
		{
			get
			{
				return this.fWaitingForGPUExclusiveResourceTimePercent;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00061468 File Offset: 0x00060868
		public float WaitingForGpuOtherTimePercent
		{
			get
			{
				return this.fWaitingForGPUOtherTimePercent;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00061480 File Offset: 0x00060880
		public InterfaceTimings()
		{
			ref InterfaceTimings interfaceTimings& = ref this;
			initblk(ref interfaceTimings&, 0, 20);
		}

		// Token: 0x04000F6C RID: 3948
		internal float fWaitingForGPUToUseApplicationResourceTimePercent;

		// Token: 0x04000F6D RID: 3949
		internal float fWaitingForGPUToAcceptMoreCommandsTimePercent;

		// Token: 0x04000F6E RID: 3950
		internal float fWaitingForGPUToStayWithinLatencyTimePercent;

		// Token: 0x04000F6F RID: 3951
		internal float fWaitingForGPUExclusiveResourceTimePercent;

		// Token: 0x04000F70 RID: 3952
		internal float fWaitingForGPUOtherTimePercent;
	}
}
