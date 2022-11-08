using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x0200088C RID: 2188
	[ComVisible(true)]
	public sealed class CspKeyContainerInfo
	{
		// Token: 0x06004F98 RID: 20376 RVA: 0x00114D55 File Offset: 0x00113D55
		private CspKeyContainerInfo()
		{
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x00114D60 File Offset: 0x00113D60
		internal CspKeyContainerInfo(CspParameters parameters, bool randomKeyContainer)
		{
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Open);
			keyContainerPermission.AccessEntries.Add(accessEntry);
			keyContainerPermission.Demand();
			this.m_parameters = new CspParameters(parameters);
			if (this.m_parameters.KeyNumber == -1)
			{
				if (this.m_parameters.ProviderType == 1 || this.m_parameters.ProviderType == 24)
				{
					this.m_parameters.KeyNumber = 1;
				}
				else if (this.m_parameters.ProviderType == 13)
				{
					this.m_parameters.KeyNumber = 2;
				}
			}
			this.m_randomKeyContainer = randomKeyContainer;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00114DFC File Offset: 0x00113DFC
		public CspKeyContainerInfo(CspParameters parameters) : this(parameters, false)
		{
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06004F9B RID: 20379 RVA: 0x00114E06 File Offset: 0x00113E06
		public bool MachineKeyStore
		{
			get
			{
				return (this.m_parameters.Flags & CspProviderFlags.UseMachineKeyStore) == CspProviderFlags.UseMachineKeyStore;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x00114E1B File Offset: 0x00113E1B
		public string ProviderName
		{
			get
			{
				return this.m_parameters.ProviderName;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06004F9D RID: 20381 RVA: 0x00114E28 File Offset: 0x00113E28
		public int ProviderType
		{
			get
			{
				return this.m_parameters.ProviderType;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004F9E RID: 20382 RVA: 0x00114E35 File Offset: 0x00113E35
		public string KeyContainerName
		{
			get
			{
				return this.m_parameters.KeyContainerName;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004F9F RID: 20383 RVA: 0x00114E44 File Offset: 0x00113E44
		public string UniqueKeyContainerName
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				string result = (string)Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 8U);
				invalidHandle.Dispose();
				return result;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06004FA0 RID: 20384 RVA: 0x00114EB0 File Offset: 0x00113EB0
		public KeyNumber KeyNumber
		{
			get
			{
				return (KeyNumber)this.m_parameters.KeyNumber;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06004FA1 RID: 20385 RVA: 0x00114EC0 File Offset: 0x00113EC0
		public bool Exportable
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				if (this.HardwareDevice)
				{
					return false;
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 3U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06004FA2 RID: 20386 RVA: 0x00114F3C File Offset: 0x00113F3C
		public bool HardwareDevice
		{
			get
			{
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				CspParameters cspParameters = new CspParameters(this.m_parameters);
				cspParameters.KeyContainerName = null;
				cspParameters.Flags = (((cspParameters.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
				uint num = 0U;
				if (Utils.Win2KCrypto == 1)
				{
					num |= 4026531840U;
				}
				int num2 = Utils._OpenCSP(cspParameters, num, ref invalidHandle);
				if (num2 != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, cspParameters.KeyNumber, 5U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06004FA3 RID: 20387 RVA: 0x00114FC8 File Offset: 0x00113FC8
		public bool Removable
		{
			get
			{
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				CspParameters cspParameters = new CspParameters(this.m_parameters);
				cspParameters.KeyContainerName = null;
				cspParameters.Flags = (((cspParameters.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
				uint num = 0U;
				if (Utils.Win2KCrypto == 1)
				{
					num |= 4026531840U;
				}
				int num2 = Utils._OpenCSP(cspParameters, num, ref invalidHandle);
				if (num2 != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, cspParameters.KeyNumber, 4U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06004FA4 RID: 20388 RVA: 0x00115054 File Offset: 0x00114054
		public bool Accessible
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					return false;
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 6U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x001150B8 File Offset: 0x001140B8
		public bool Protected
		{
			get
			{
				if (this.HardwareDevice)
				{
					return true;
				}
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 7U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06004FA6 RID: 20390 RVA: 0x00115134 File Offset: 0x00114134
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(this.m_parameters, KeyContainerPermissionFlags.ViewAcl | KeyContainerPermissionFlags.ChangeAcl);
				keyContainerPermission.AccessEntries.Add(accessEntry);
				keyContainerPermission.Demand();
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				CryptoKeySecurity keySetSecurityInfo;
				using (invalidHandle)
				{
					keySetSecurityInfo = Utils.GetKeySetSecurityInfo(invalidHandle, AccessControlSections.All);
				}
				return keySetSecurityInfo;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06004FA7 RID: 20391 RVA: 0x001151DC File Offset: 0x001141DC
		public bool RandomlyGenerated
		{
			get
			{
				return this.m_randomKeyContainer;
			}
		}

		// Token: 0x0400290B RID: 10507
		private CspParameters m_parameters;

		// Token: 0x0400290C RID: 10508
		private bool m_randomKeyContainer;
	}
}
