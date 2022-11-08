using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000075 RID: 117
	[DefaultMember("Value")]
	[MiscellaneousBits(1)]
	public struct BehaviorFlags
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00059F64 File Offset: 0x00059364
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$35$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$36$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0005A0C8 File Offset: 0x000594C8
		public BehaviorFlags(params CreateFlags[] newBehavior)
		{
			this.behavior = 0;
			if (newBehavior != null)
			{
				int num = 0;
				if (0 < newBehavior.Length)
				{
					do
					{
						this.behavior = (int)(newBehavior[num] | (CreateFlags)this.behavior);
						num++;
					}
					while (num < newBehavior.Length);
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0005A0AC File Offset: 0x000594AC
		public BehaviorFlags(CreateFlags initialBehavior)
		{
			this.behavior = (int)initialBehavior;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0005A090 File Offset: 0x00059490
		public BehaviorFlags()
		{
			this.behavior = 0;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0005A104 File Offset: 0x00059504
		// (set) Token: 0x06000187 RID: 391 RVA: 0x0005A11C File Offset: 0x0005951C
		[IndexerName("Value")]
		public CreateFlags Value
		{
			get
			{
				return (CreateFlags)this.behavior;
			}
			set
			{
				this.behavior = (int)value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0005A138 File Offset: 0x00059538
		public bool FpuPreserve
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 1 & 1U) != 0;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0005A158 File Offset: 0x00059558
		public bool PureDevice
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 4 & 1U) != 0;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0005A178 File Offset: 0x00059578
		public bool SoftwareVertexProcessing
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 5 & 1U) != 0;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0005A198 File Offset: 0x00059598
		public bool HardwareVertexProcessing
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 6 & 1U) != 0;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0005A1B8 File Offset: 0x000595B8
		public bool MultiThreaded
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 2 & 1U) != 0;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0005A1D8 File Offset: 0x000595D8
		public bool MixedVertexProcessing
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 7 & 1U) != 0;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0005A1F8 File Offset: 0x000595F8
		public bool DisableDriverManagement
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 8 & 1U) != 0;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0005A218 File Offset: 0x00059618
		public bool AdapterGroupDevice
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.behavior >> 9 & 1U) != 0;
			}
		}

		// Token: 0x04000E76 RID: 3702
		private int behavior;
	}
}
