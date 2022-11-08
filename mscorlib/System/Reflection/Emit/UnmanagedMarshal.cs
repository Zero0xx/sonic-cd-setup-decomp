using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000855 RID: 2133
	[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[Serializable]
	public sealed class UnmanagedMarshal
	{
		// Token: 0x06004E44 RID: 20036 RVA: 0x0010F892 File Offset: 0x0010E892
		public static UnmanagedMarshal DefineUnmanagedMarshal(UnmanagedType unmanagedType)
		{
			if (unmanagedType == UnmanagedType.ByValTStr || unmanagedType == UnmanagedType.SafeArray || unmanagedType == UnmanagedType.ByValArray || unmanagedType == UnmanagedType.LPArray || unmanagedType == UnmanagedType.CustomMarshaler)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotASimpleNativeType"));
			}
			return new UnmanagedMarshal(unmanagedType, Guid.Empty, 0, (UnmanagedType)0);
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x0010F8CA File Offset: 0x0010E8CA
		public static UnmanagedMarshal DefineByValTStr(int elemCount)
		{
			return new UnmanagedMarshal(UnmanagedType.ByValTStr, Guid.Empty, elemCount, (UnmanagedType)0);
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x0010F8DA File Offset: 0x0010E8DA
		public static UnmanagedMarshal DefineSafeArray(UnmanagedType elemType)
		{
			return new UnmanagedMarshal(UnmanagedType.SafeArray, Guid.Empty, 0, elemType);
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x0010F8EA File Offset: 0x0010E8EA
		public static UnmanagedMarshal DefineByValArray(int elemCount)
		{
			return new UnmanagedMarshal(UnmanagedType.ByValArray, Guid.Empty, elemCount, (UnmanagedType)0);
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x0010F8FA File Offset: 0x0010E8FA
		public static UnmanagedMarshal DefineLPArray(UnmanagedType elemType)
		{
			return new UnmanagedMarshal(UnmanagedType.LPArray, Guid.Empty, 0, elemType);
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x0010F90A File Offset: 0x0010E90A
		public UnmanagedType GetUnmanagedType
		{
			get
			{
				return this.m_unmanagedType;
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x0010F912 File Offset: 0x0010E912
		public Guid IIDGuid
		{
			get
			{
				if (this.m_unmanagedType != UnmanagedType.CustomMarshaler)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotACustomMarshaler"));
				}
				return this.m_guid;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06004E4B RID: 20043 RVA: 0x0010F934 File Offset: 0x0010E934
		public int ElementCount
		{
			get
			{
				if (this.m_unmanagedType != UnmanagedType.ByValArray && this.m_unmanagedType != UnmanagedType.ByValTStr)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NoUnmanagedElementCount"));
				}
				return this.m_numElem;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x0010F960 File Offset: 0x0010E960
		public UnmanagedType BaseType
		{
			get
			{
				if (this.m_unmanagedType != UnmanagedType.LPArray && this.m_unmanagedType != UnmanagedType.SafeArray)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NoNestedMarshal"));
				}
				return this.m_baseType;
			}
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x0010F98C File Offset: 0x0010E98C
		private UnmanagedMarshal(UnmanagedType unmanagedType, Guid guid, int numElem, UnmanagedType type)
		{
			this.m_unmanagedType = unmanagedType;
			this.m_guid = guid;
			this.m_numElem = numElem;
			this.m_baseType = type;
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x0010F9B4 File Offset: 0x0010E9B4
		internal byte[] InternalGetBytes()
		{
			if (this.m_unmanagedType == UnmanagedType.SafeArray || this.m_unmanagedType == UnmanagedType.LPArray)
			{
				int num = 2;
				byte[] array = new byte[num];
				array[0] = (byte)this.m_unmanagedType;
				array[1] = (byte)this.m_baseType;
				return array;
			}
			if (this.m_unmanagedType == UnmanagedType.ByValArray || this.m_unmanagedType == UnmanagedType.ByValTStr)
			{
				int num2 = 0;
				int num3;
				if (this.m_numElem <= 127)
				{
					num3 = 1;
				}
				else if (this.m_numElem <= 16383)
				{
					num3 = 2;
				}
				else
				{
					num3 = 4;
				}
				num3++;
				byte[] array = new byte[num3];
				array[num2++] = (byte)this.m_unmanagedType;
				if (this.m_numElem <= 127)
				{
					array[num2++] = (byte)(this.m_numElem & 255);
				}
				else if (this.m_numElem <= 16383)
				{
					array[num2++] = (byte)(this.m_numElem >> 8 | 128);
					array[num2++] = (byte)(this.m_numElem & 255);
				}
				else if (this.m_numElem <= 536870911)
				{
					array[num2++] = (byte)(this.m_numElem >> 24 | 192);
					array[num2++] = (byte)(this.m_numElem >> 16 & 255);
					array[num2++] = (byte)(this.m_numElem >> 8 & 255);
					array[num2++] = (byte)(this.m_numElem & 255);
				}
				return array;
			}
			return new byte[]
			{
				(byte)this.m_unmanagedType
			};
		}

		// Token: 0x04002859 RID: 10329
		internal UnmanagedType m_unmanagedType;

		// Token: 0x0400285A RID: 10330
		internal Guid m_guid;

		// Token: 0x0400285B RID: 10331
		internal int m_numElem;

		// Token: 0x0400285C RID: 10332
		internal UnmanagedType m_baseType;
	}
}
