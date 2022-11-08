using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography
{
	// Token: 0x02000873 RID: 2163
	[ComVisible(true)]
	public sealed class CspParameters
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x00110CF5 File Offset: 0x0010FCF5
		// (set) Token: 0x06004ECB RID: 20171 RVA: 0x00110D00 File Offset: 0x0010FD00
		public CspProviderFlags Flags
		{
			get
			{
				return (CspProviderFlags)this.m_flags;
			}
			set
			{
				uint num = 2147483775U;
				if ((value & (CspProviderFlags)(~(CspProviderFlags)num)) != CspProviderFlags.NoFlags)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[]
					{
						(int)value
					}), "value");
				}
				this.m_flags = (uint)value;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x00110D48 File Offset: 0x0010FD48
		// (set) Token: 0x06004ECD RID: 20173 RVA: 0x00110D50 File Offset: 0x0010FD50
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				return this.m_cryptoKeySecurity;
			}
			set
			{
				this.m_cryptoKeySecurity = value;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004ECE RID: 20174 RVA: 0x00110D59 File Offset: 0x0010FD59
		// (set) Token: 0x06004ECF RID: 20175 RVA: 0x00110D61 File Offset: 0x0010FD61
		public SecureString KeyPassword
		{
			get
			{
				return this.m_keyPassword;
			}
			set
			{
				this.m_keyPassword = value;
				this.m_parentWindowHandle = IntPtr.Zero;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004ED0 RID: 20176 RVA: 0x00110D75 File Offset: 0x0010FD75
		// (set) Token: 0x06004ED1 RID: 20177 RVA: 0x00110D7D File Offset: 0x0010FD7D
		public IntPtr ParentWindowHandle
		{
			get
			{
				return this.m_parentWindowHandle;
			}
			set
			{
				this.m_parentWindowHandle = value;
				this.m_keyPassword = null;
			}
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x00110D8D File Offset: 0x0010FD8D
		public CspParameters() : this(Utils.DefaultRsaProviderType, null, null)
		{
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x00110D9C File Offset: 0x0010FD9C
		public CspParameters(int dwTypeIn) : this(dwTypeIn, null, null)
		{
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x00110DA7 File Offset: 0x0010FDA7
		public CspParameters(int dwTypeIn, string strProviderNameIn) : this(dwTypeIn, strProviderNameIn, null)
		{
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x00110DB2 File Offset: 0x0010FDB2
		public CspParameters(int dwTypeIn, string strProviderNameIn, string strContainerNameIn) : this(dwTypeIn, strProviderNameIn, strContainerNameIn, CspProviderFlags.NoFlags)
		{
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x00110DBE File Offset: 0x0010FDBE
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, SecureString keyPassword) : this(providerType, providerName, keyContainerName)
		{
			this.m_cryptoKeySecurity = cryptoKeySecurity;
			this.m_keyPassword = keyPassword;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x00110DD9 File Offset: 0x0010FDD9
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, IntPtr parentWindowHandle) : this(providerType, providerName, keyContainerName)
		{
			this.m_cryptoKeySecurity = cryptoKeySecurity;
			this.m_parentWindowHandle = parentWindowHandle;
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x00110DF4 File Offset: 0x0010FDF4
		internal CspParameters(int providerType, string providerName, string keyContainerName, CspProviderFlags flags)
		{
			this.ProviderType = providerType;
			this.ProviderName = providerName;
			this.KeyContainerName = keyContainerName;
			this.KeyNumber = -1;
			this.Flags = flags;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x00110E20 File Offset: 0x0010FE20
		internal CspParameters(CspParameters parameters)
		{
			this.ProviderType = parameters.ProviderType;
			this.ProviderName = parameters.ProviderName;
			this.KeyContainerName = parameters.KeyContainerName;
			this.KeyNumber = parameters.KeyNumber;
			this.Flags = parameters.Flags;
			this.m_cryptoKeySecurity = parameters.m_cryptoKeySecurity;
			this.m_keyPassword = parameters.m_keyPassword;
			this.m_parentWindowHandle = parameters.m_parentWindowHandle;
		}

		// Token: 0x040028B4 RID: 10420
		public int ProviderType;

		// Token: 0x040028B5 RID: 10421
		public string ProviderName;

		// Token: 0x040028B6 RID: 10422
		public string KeyContainerName;

		// Token: 0x040028B7 RID: 10423
		public int KeyNumber;

		// Token: 0x040028B8 RID: 10424
		private uint m_flags;

		// Token: 0x040028B9 RID: 10425
		private CryptoKeySecurity m_cryptoKeySecurity;

		// Token: 0x040028BA RID: 10426
		private SecureString m_keyPassword;

		// Token: 0x040028BB RID: 10427
		private IntPtr m_parentWindowHandle;
	}
}
