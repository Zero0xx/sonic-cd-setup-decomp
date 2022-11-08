using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000A7 RID: 167
	[MiscellaneousBits(1)]
	public struct AdapterDetails
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0005C6E4 File Offset: 0x0005BAE4
		public string DriverName
		{
			get
			{
				return this.m_Driver;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0005C6FC File Offset: 0x0005BAFC
		public string Description
		{
			get
			{
				return this.m_Description;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0005C714 File Offset: 0x0005BB14
		public string DeviceName
		{
			get
			{
				return this.m_DeviceName;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0005C72C File Offset: 0x0005BB2C
		public Version DriverVersion
		{
			get
			{
				return this.m_DriverVersion;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0005C744 File Offset: 0x0005BB44
		public int VendorId
		{
			get
			{
				return this.m_VendorId;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0005C75C File Offset: 0x0005BB5C
		public int DeviceId
		{
			get
			{
				return this.m_DeviceId;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0005C774 File Offset: 0x0005BB74
		public int SubSystemId
		{
			get
			{
				return this.m_SubSysId;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0005C78C File Offset: 0x0005BB8C
		public int Revision
		{
			get
			{
				return this.m_Revision;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0005C7A4 File Offset: 0x0005BBA4
		public int WhqlLevel
		{
			get
			{
				return this.m_WHQLLevel;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0005C7BC File Offset: 0x0005BBBC
		public Guid DeviceIdentifier
		{
			get
			{
				return this.m_DeviceIdentifier;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0005C7D4 File Offset: 0x0005BBD4
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$64$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$65$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0005C900 File Offset: 0x0005BD00
		public AdapterDetails()
		{
			this.m_Description = null;
			this.m_Driver = null;
			this.m_DriverVersion = null;
			this.m_VendorId = 0;
			this.m_DeviceId = 0;
			this.m_Revision = 0;
			this.m_DeviceIdentifier = Guid.Empty;
			this.m_WHQLLevel = 0;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0005C950 File Offset: 0x0005BD50
		internal unsafe static AdapterDetails op_explicit(_D3DADAPTER_IDENTIFIER9 val)
		{
			AdapterDetails result = default(AdapterDetails);
			result = new AdapterDetails();
			result.m_DriverVersion = new Version((int)((ushort)((uint)(*(ref val + 1060)) >> 16)), (int)((ushort)(*(ref val + 1060))), (int)((ushort)((uint)(*(ref val + 1056)) >> 16)), (int)((ushort)(*(ref val + 1056))));
			result.m_DeviceId = *(ref val + 1068);
			cpblk(ref result.m_DeviceIdentifier, ref val + 1080, 16);
			result.m_Revision = *(ref val + 1076);
			result.m_SubSysId = *(ref val + 1072);
			result.m_VendorId = *(ref val + 1064);
			result.m_WHQLLevel = *(ref val + 1096);
			IntPtr ptr = 0;
			ptr = new IntPtr(ref val + 512);
			result.m_Description = Marshal.PtrToStringAnsi(ptr);
			IntPtr ptr2 = 0;
			ptr2 = new IntPtr((void*)(&val));
			result.m_Driver = Marshal.PtrToStringAnsi(ptr2);
			IntPtr ptr3 = 0;
			ptr3 = new IntPtr(ref val + 1024);
			result.m_DeviceName = Marshal.PtrToStringAnsi(ptr3);
			return result;
		}

		// Token: 0x04000EE0 RID: 3808
		private string m_Driver;

		// Token: 0x04000EE1 RID: 3809
		private string m_Description;

		// Token: 0x04000EE2 RID: 3810
		private string m_DeviceName;

		// Token: 0x04000EE3 RID: 3811
		private Version m_DriverVersion;

		// Token: 0x04000EE4 RID: 3812
		private int m_VendorId;

		// Token: 0x04000EE5 RID: 3813
		private int m_DeviceId;

		// Token: 0x04000EE6 RID: 3814
		private int m_SubSysId;

		// Token: 0x04000EE7 RID: 3815
		private int m_Revision;

		// Token: 0x04000EE8 RID: 3816
		private Guid m_DeviceIdentifier;

		// Token: 0x04000EE9 RID: 3817
		private int m_WHQLLevel;
	}
}
