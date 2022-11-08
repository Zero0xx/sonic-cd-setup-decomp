using System;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x02000655 RID: 1621
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNamePublicKeyBlob
	{
		// Token: 0x06003A73 RID: 14963 RVA: 0x000C5242 File Offset: 0x000C4242
		internal StrongNamePublicKeyBlob()
		{
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000C524A File Offset: 0x000C424A
		public StrongNamePublicKeyBlob(byte[] publicKey)
		{
			if (publicKey == null)
			{
				throw new ArgumentNullException("PublicKey");
			}
			this.PublicKey = new byte[publicKey.Length];
			Array.Copy(publicKey, 0, this.PublicKey, 0, publicKey.Length);
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x000C527F File Offset: 0x000C427F
		internal StrongNamePublicKeyBlob(string publicKey)
		{
			this.PublicKey = Hex.DecodeHexString(publicKey);
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000C5294 File Offset: 0x000C4294
		private static bool CompareArrays(byte[] first, byte[] second)
		{
			if (first.Length != second.Length)
			{
				return false;
			}
			int num = first.Length;
			for (int i = 0; i < num; i++)
			{
				if (first[i] != second[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x000C52C6 File Offset: 0x000C42C6
		internal bool Equals(StrongNamePublicKeyBlob blob)
		{
			return blob != null && StrongNamePublicKeyBlob.CompareArrays(this.PublicKey, blob.PublicKey);
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x000C52DE File Offset: 0x000C42DE
		public override bool Equals(object obj)
		{
			return obj != null && obj is StrongNamePublicKeyBlob && this.Equals((StrongNamePublicKeyBlob)obj);
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x000C52FC File Offset: 0x000C42FC
		private static int GetByteArrayHashCode(byte[] baData)
		{
			if (baData == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < baData.Length; i++)
			{
				num = (num << 8 ^ (int)baData[i] ^ num >> 24);
			}
			return num;
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x000C532C File Offset: 0x000C432C
		public override int GetHashCode()
		{
			return StrongNamePublicKeyBlob.GetByteArrayHashCode(this.PublicKey);
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x000C5339 File Offset: 0x000C4339
		public override string ToString()
		{
			return Hex.EncodeHexString(this.PublicKey);
		}

		// Token: 0x04001E59 RID: 7769
		internal byte[] PublicKey;
	}
}
