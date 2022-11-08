using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace System.Security.Permissions
{
	// Token: 0x0200065E RID: 1630
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntry
	{
		// Token: 0x06003ABE RID: 15038 RVA: 0x000C6582 File Offset: 0x000C5582
		internal KeyContainerPermissionAccessEntry(KeyContainerPermissionAccessEntry accessEntry) : this(accessEntry.KeyStore, accessEntry.ProviderName, accessEntry.ProviderType, accessEntry.KeyContainerName, accessEntry.KeySpec, accessEntry.Flags)
		{
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x000C65AE File Offset: 0x000C55AE
		public KeyContainerPermissionAccessEntry(string keyContainerName, KeyContainerPermissionFlags flags) : this(null, null, -1, keyContainerName, -1, flags)
		{
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x000C65BC File Offset: 0x000C55BC
		public KeyContainerPermissionAccessEntry(CspParameters parameters, KeyContainerPermissionFlags flags) : this(((parameters.Flags & CspProviderFlags.UseMachineKeyStore) == CspProviderFlags.UseMachineKeyStore) ? "Machine" : "User", parameters.ProviderName, parameters.ProviderType, parameters.KeyContainerName, parameters.KeyNumber, flags)
		{
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x000C65F4 File Offset: 0x000C55F4
		public KeyContainerPermissionAccessEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec, KeyContainerPermissionFlags flags)
		{
			this.m_providerName = ((providerName == null) ? "*" : providerName);
			this.m_providerType = providerType;
			this.m_keyContainerName = ((keyContainerName == null) ? "*" : keyContainerName);
			this.m_keySpec = keySpec;
			this.KeyStore = keyStore;
			this.Flags = flags;
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x000C6649 File Offset: 0x000C5649
		// (set) Token: 0x06003AC3 RID: 15043 RVA: 0x000C6654 File Offset: 0x000C5654
		public string KeyStore
		{
			get
			{
				return this.m_keyStore;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(value, this.ProviderName, this.ProviderType, this.KeyContainerName, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				if (value == null)
				{
					this.m_keyStore = "*";
					return;
				}
				if (value != "User" && value != "Machine" && value != "*")
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKeyStore", new object[]
					{
						value
					}), "value");
				}
				this.m_keyStore = value;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003AC4 RID: 15044 RVA: 0x000C66EF File Offset: 0x000C56EF
		// (set) Token: 0x06003AC5 RID: 15045 RVA: 0x000C66F8 File Offset: 0x000C56F8
		public string ProviderName
		{
			get
			{
				return this.m_providerName;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, value, this.ProviderType, this.KeyContainerName, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				if (value == null)
				{
					this.m_providerName = "*";
					return;
				}
				this.m_providerName = value;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x000C674B File Offset: 0x000C574B
		// (set) Token: 0x06003AC7 RID: 15047 RVA: 0x000C6753 File Offset: 0x000C5753
		public int ProviderType
		{
			get
			{
				return this.m_providerType;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, this.ProviderName, value, this.KeyContainerName, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				this.m_providerType = value;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x000C678C File Offset: 0x000C578C
		// (set) Token: 0x06003AC9 RID: 15049 RVA: 0x000C6794 File Offset: 0x000C5794
		public string KeyContainerName
		{
			get
			{
				return this.m_keyContainerName;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, this.ProviderName, this.ProviderType, value, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				if (value == null)
				{
					this.m_keyContainerName = "*";
					return;
				}
				this.m_keyContainerName = value;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x000C67E7 File Offset: 0x000C57E7
		// (set) Token: 0x06003ACB RID: 15051 RVA: 0x000C67EF File Offset: 0x000C57EF
		public int KeySpec
		{
			get
			{
				return this.m_keySpec;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, this.ProviderName, this.ProviderType, this.KeyContainerName, value))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				this.m_keySpec = value;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x000C6828 File Offset: 0x000C5828
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x000C6830 File Offset: 0x000C5830
		public KeyContainerPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				KeyContainerPermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x000C6840 File Offset: 0x000C5840
		public override bool Equals(object o)
		{
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = o as KeyContainerPermissionAccessEntry;
			return keyContainerPermissionAccessEntry != null && !(keyContainerPermissionAccessEntry.m_keyStore != this.m_keyStore) && !(keyContainerPermissionAccessEntry.m_providerName != this.m_providerName) && keyContainerPermissionAccessEntry.m_providerType == this.m_providerType && !(keyContainerPermissionAccessEntry.m_keyContainerName != this.m_keyContainerName) && keyContainerPermissionAccessEntry.m_keySpec == this.m_keySpec;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x000C68BC File Offset: 0x000C58BC
		public override int GetHashCode()
		{
			int num = 0;
			num |= (this.m_keyStore.GetHashCode() & 255) << 24;
			num |= (this.m_providerName.GetHashCode() & 255) << 16;
			num |= (this.m_providerType & 15) << 12;
			num |= (this.m_keyContainerName.GetHashCode() & 255) << 4;
			return num | (this.m_keySpec & 15);
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x000C692C File Offset: 0x000C592C
		internal bool IsSubsetOf(KeyContainerPermissionAccessEntry target)
		{
			return (!(target.m_keyStore != "*") || !(this.m_keyStore != target.m_keyStore)) && (!(target.m_providerName != "*") || !(this.m_providerName != target.m_providerName)) && (target.m_providerType == -1 || this.m_providerType == target.m_providerType) && (!(target.m_keyContainerName != "*") || !(this.m_keyContainerName != target.m_keyContainerName)) && (target.m_keySpec == -1 || this.m_keySpec == target.m_keySpec);
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x000C69E4 File Offset: 0x000C59E4
		internal static bool IsUnrestrictedEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec)
		{
			return (!(keyStore != "*") || keyStore == null) && (!(providerName != "*") || providerName == null) && providerType == -1 && (!(keyContainerName != "*") || keyContainerName == null) && keySpec == -1;
		}

		// Token: 0x04001E79 RID: 7801
		private string m_keyStore;

		// Token: 0x04001E7A RID: 7802
		private string m_providerName;

		// Token: 0x04001E7B RID: 7803
		private int m_providerType;

		// Token: 0x04001E7C RID: 7804
		private string m_keyContainerName;

		// Token: 0x04001E7D RID: 7805
		private int m_keySpec;

		// Token: 0x04001E7E RID: 7806
		private KeyContainerPermissionFlags m_flags;
	}
}
