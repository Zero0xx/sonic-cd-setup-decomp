using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000A5 RID: 165
	public sealed class PresentParameters : ICloneable
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0005C5FC File Offset: 0x0005B9FC
		public PresentParameters()
		{
			initblk(ref this.m_Internal, 0, 56);
			this.forceNoMP = false;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000648A4 File Offset: 0x00063CA4
		public unsafe PresentParameters(PresentParameters original)
		{
			_D3DPRESENT_PARAMETERS_* realStruct = original.RealStruct;
			cpblk(ref this.m_Internal, realStruct, 56);
			this.forceNoMP = original.forceNoMP;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000648DC File Offset: 0x00063CDC
		public object Clone()
		{
			return new PresentParameters(this);
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0005C200 File Offset: 0x0005B600
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0005C218 File Offset: 0x0005B618
		public bool ForceNoMultiThreadedFlag
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.forceNoMP;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.forceNoMP = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0005C234 File Offset: 0x0005B634
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0005C250 File Offset: 0x0005B650
		public int BackBufferWidth
		{
			get
			{
				return this.m_Internal;
			}
			set
			{
				this.m_Internal = value;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0005C26C File Offset: 0x0005B66C
		// (set) Token: 0x06000202 RID: 514 RVA: 0x0005C288 File Offset: 0x0005B688
		public unsafe int BackBufferHeight
		{
			get
			{
				return *(ref this.m_Internal + 4);
			}
			set
			{
				*(ref this.m_Internal + 4) = value;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0005C2A4 File Offset: 0x0005B6A4
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0005C2C0 File Offset: 0x0005B6C0
		public unsafe Format BackBufferFormat
		{
			get
			{
				return (Format)(*(ref this.m_Internal + 8));
			}
			set
			{
				*(ref this.m_Internal + 8) = (int)value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0005C2DC File Offset: 0x0005B6DC
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0005C2F8 File Offset: 0x0005B6F8
		public unsafe int BackBufferCount
		{
			get
			{
				return *(ref this.m_Internal + 12);
			}
			set
			{
				*(ref this.m_Internal + 12) = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0005C318 File Offset: 0x0005B718
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0005C334 File Offset: 0x0005B734
		public unsafe MultiSampleType MultiSample
		{
			get
			{
				return (MultiSampleType)(*(ref this.m_Internal + 16));
			}
			set
			{
				*(ref this.m_Internal + 16) = (int)value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0005C354 File Offset: 0x0005B754
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0005C370 File Offset: 0x0005B770
		public unsafe SwapEffect SwapEffect
		{
			get
			{
				return (SwapEffect)(*(ref this.m_Internal + 24));
			}
			set
			{
				*(ref this.m_Internal + 24) = (int)value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0005C390 File Offset: 0x0005B790
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0005C3F0 File Offset: 0x0005B7F0
		public unsafe Control DeviceWindow
		{
			get
			{
				IntPtr handle = 0;
				handle = new IntPtr(*(ref this.m_Internal + 28));
				return Control.FromHandle(handle);
			}
			set
			{
				int num;
				if (value != null)
				{
					num = value.Handle.ToPointer();
				}
				else
				{
					num = 0;
				}
				*(ref this.m_Internal + 28) = num;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0005C3C4 File Offset: 0x0005B7C4
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0005C424 File Offset: 0x0005B824
		public unsafe IntPtr DeviceWindowHandle
		{
			get
			{
				IntPtr result = 0;
				result = new IntPtr(*(ref this.m_Internal + 28));
				return result;
			}
			set
			{
				*(ref this.m_Internal + 28) = value.ToPointer();
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0005C448 File Offset: 0x0005B848
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0005C46C File Offset: 0x0005B86C
		public unsafe bool Windowed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return *(ref this.m_Internal + 32) != 0;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				*(ref this.m_Internal + 32) = (value ? 1 : 0);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0005C48C File Offset: 0x0005B88C
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0005C4B0 File Offset: 0x0005B8B0
		public unsafe bool EnableAutoDepthStencil
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return *(ref this.m_Internal + 36) != 0;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				*(ref this.m_Internal + 36) = (value ? 1 : 0);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0005C4D0 File Offset: 0x0005B8D0
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0005C4EC File Offset: 0x0005B8EC
		public unsafe DepthFormat AutoDepthStencilFormat
		{
			get
			{
				return (DepthFormat)(*(ref this.m_Internal + 40));
			}
			set
			{
				*(ref this.m_Internal + 40) = (int)value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0005C50C File Offset: 0x0005B90C
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0005C528 File Offset: 0x0005B928
		public unsafe PresentFlag PresentFlag
		{
			get
			{
				return (PresentFlag)(*(ref this.m_Internal + 44));
			}
			set
			{
				*(ref this.m_Internal + 44) = (int)value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0005C548 File Offset: 0x0005B948
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0005C564 File Offset: 0x0005B964
		public unsafe int FullScreenRefreshRateInHz
		{
			get
			{
				return *(ref this.m_Internal + 48);
			}
			set
			{
				*(ref this.m_Internal + 48) = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0005C584 File Offset: 0x0005B984
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0005C5A0 File Offset: 0x0005B9A0
		public unsafe PresentInterval PresentationInterval
		{
			get
			{
				return (PresentInterval)(*(ref this.m_Internal + 52));
			}
			set
			{
				*(ref this.m_Internal + 52) = (int)value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0005C5C0 File Offset: 0x0005B9C0
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0005C5DC File Offset: 0x0005B9DC
		public unsafe int MultiSampleQuality
		{
			get
			{
				return *(ref this.m_Internal + 20);
			}
			set
			{
				*(ref this.m_Internal + 20) = value;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0005C628 File Offset: 0x0005BA28
		public unsafe override string ToString()
		{
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null)
					{
						object obj = getMethod.Invoke(this, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$63$];
						array[0] = properties[num].Name;
						string text;
						if (obj != null)
						{
							text = obj.ToString();
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
			return stringBuilder.ToString();
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0005C6C8 File Offset: 0x0005BAC8
		internal unsafe _D3DPRESENT_PARAMETERS_* RealStruct
		{
			get
			{
				return &this.m_Internal;
			}
		}

		// Token: 0x04000EDD RID: 3805
		private _D3DPRESENT_PARAMETERS_ m_Internal;

		// Token: 0x04000EDE RID: 3806
		private bool forceNoMP;

		// Token: 0x04000EDF RID: 3807
		public const int DefaultPresentRate = 0;
	}
}
