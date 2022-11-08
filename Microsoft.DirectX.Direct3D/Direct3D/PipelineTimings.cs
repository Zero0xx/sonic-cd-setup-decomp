using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000D9 RID: 217
	[MiscellaneousBits(1)]
	public struct PipelineTimings
	{
		// Token: 0x06000379 RID: 889 RVA: 0x00061108 File Offset: 0x00060508
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$108$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$109$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00061234 File Offset: 0x00060634
		public float VertexProcessingTimePercent
		{
			get
			{
				return this.fVertexProcessingTimePercent;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0006124C File Offset: 0x0006064C
		public float PixelProcessingTimePercent
		{
			get
			{
				return this.fPixelProcessingTimePercent;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00061264 File Offset: 0x00060664
		public float OtherGpuProcessingTimePercent
		{
			get
			{
				return this.fOtherGPUProcessingTimePercent;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0006127C File Offset: 0x0006067C
		public float GpuIdleTimePercent
		{
			get
			{
				return this.fGPUIdleTimePercent;
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00061294 File Offset: 0x00060694
		public PipelineTimings()
		{
			ref PipelineTimings pipelineTimings& = ref this;
			initblk(ref pipelineTimings&, 0, 16);
		}

		// Token: 0x04000F67 RID: 3943
		internal float fVertexProcessingTimePercent;

		// Token: 0x04000F68 RID: 3944
		internal float fPixelProcessingTimePercent;

		// Token: 0x04000F69 RID: 3945
		internal float fOtherGPUProcessingTimePercent;

		// Token: 0x04000F6A RID: 3946
		internal float fGPUIdleTimePercent;
	}
}
