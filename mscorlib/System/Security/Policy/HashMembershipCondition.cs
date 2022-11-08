using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Util;
using System.Threading;

namespace System.Security.Policy
{
	// Token: 0x020004C1 RID: 1217
	[ComVisible(true)]
	[Serializable]
	public sealed class HashMembershipCondition : ISerializable, IDeserializationCallback, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x000A6D9C File Offset: 0x000A5D9C
		private object InternalSyncObject
		{
			get
			{
				if (this.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref this.s_InternalSyncObject, value, null);
				}
				return this.s_InternalSyncObject;
			}
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000A6DCB File Offset: 0x000A5DCB
		internal HashMembershipCondition()
		{
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000A6DD4 File Offset: 0x000A5DD4
		private HashMembershipCondition(SerializationInfo info, StreamingContext context)
		{
			this.m_value = (byte[])info.GetValue("HashValue", typeof(byte[]));
			string text = (string)info.GetValue("HashAlgorithm", typeof(string));
			if (text != null)
			{
				this.m_hashAlg = HashAlgorithm.Create(text);
				return;
			}
			this.m_hashAlg = new SHA1Managed();
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000A6E40 File Offset: 0x000A5E40
		public HashMembershipCondition(HashAlgorithm hashAlg, byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			this.m_value = new byte[value.Length];
			Array.Copy(value, this.m_value, value.Length);
			this.m_hashAlg = hashAlg;
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x000A6E93 File Offset: 0x000A5E93
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("HashValue", this.HashValue);
			info.AddValue("HashAlgorithm", this.HashAlgorithm.ToString());
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000A6EBC File Offset: 0x000A5EBC
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000A6ED5 File Offset: 0x000A5ED5
		// (set) Token: 0x060030B1 RID: 12465 RVA: 0x000A6EBE File Offset: 0x000A5EBE
		public HashAlgorithm HashAlgorithm
		{
			get
			{
				if (this.m_hashAlg == null && this.m_element != null)
				{
					this.ParseHashAlgorithm();
				}
				return this.m_hashAlg;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HashAlgorithm");
				}
				this.m_hashAlg = value;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000A6F20 File Offset: 0x000A5F20
		// (set) Token: 0x060030B3 RID: 12467 RVA: 0x000A6EF3 File Offset: 0x000A5EF3
		public byte[] HashValue
		{
			get
			{
				if (this.m_value == null && this.m_element != null)
				{
					this.ParseHashValue();
				}
				if (this.m_value == null)
				{
					return null;
				}
				byte[] array = new byte[this.m_value.Length];
				Array.Copy(this.m_value, array, this.m_value.Length);
				return array;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_value = new byte[value.Length];
				Array.Copy(value, this.m_value, value.Length);
			}
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x000A6F70 File Offset: 0x000A5F70
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000A6F88 File Offset: 0x000A5F88
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				Hash hash = obj as Hash;
				if (hash != null)
				{
					if (this.m_value == null && this.m_element != null)
					{
						this.ParseHashValue();
					}
					if (this.m_hashAlg == null && this.m_element != null)
					{
						this.ParseHashAlgorithm();
					}
					byte[] array = null;
					lock (this.InternalSyncObject)
					{
						array = hash.GenerateHash(this.m_hashAlg);
					}
					if (array != null && HashMembershipCondition.CompareArrays(array, this.m_value))
					{
						usedEvidence = hash;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000A7038 File Offset: 0x000A6038
		public IMembershipCondition Copy()
		{
			if (this.m_value == null && this.m_element != null)
			{
				this.ParseHashValue();
			}
			if (this.m_hashAlg == null && this.m_element != null)
			{
				this.ParseHashAlgorithm();
			}
			return new HashMembershipCondition(this.m_hashAlg, this.m_value);
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000A7077 File Offset: 0x000A6077
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000A7080 File Offset: 0x000A6080
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000A708C File Offset: 0x000A608C
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_value == null && this.m_element != null)
			{
				this.ParseHashValue();
			}
			if (this.m_hashAlg == null && this.m_element != null)
			{
				this.ParseHashAlgorithm();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.HashMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_value != null)
			{
				securityElement.AddAttribute("HashValue", Hex.EncodeHexString(this.HashValue));
			}
			if (this.m_hashAlg != null)
			{
				securityElement.AddAttribute("HashAlgorithm", this.HashAlgorithm.GetType().FullName);
			}
			return securityElement;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000A7134 File Offset: 0x000A6134
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this.InternalSyncObject)
			{
				this.m_element = e;
				this.m_value = null;
				this.m_hashAlg = null;
			}
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000A71AC File Offset: 0x000A61AC
		public override bool Equals(object o)
		{
			HashMembershipCondition hashMembershipCondition = o as HashMembershipCondition;
			if (hashMembershipCondition != null)
			{
				if (this.m_hashAlg == null && this.m_element != null)
				{
					this.ParseHashAlgorithm();
				}
				if (hashMembershipCondition.m_hashAlg == null && hashMembershipCondition.m_element != null)
				{
					hashMembershipCondition.ParseHashAlgorithm();
				}
				if (this.m_hashAlg != null && hashMembershipCondition.m_hashAlg != null && this.m_hashAlg.GetType() == hashMembershipCondition.m_hashAlg.GetType())
				{
					if (this.m_value == null && this.m_element != null)
					{
						this.ParseHashValue();
					}
					if (hashMembershipCondition.m_value == null && hashMembershipCondition.m_element != null)
					{
						hashMembershipCondition.ParseHashValue();
					}
					if (this.m_value.Length != hashMembershipCondition.m_value.Length)
					{
						return false;
					}
					for (int i = 0; i < this.m_value.Length; i++)
					{
						if (this.m_value[i] != hashMembershipCondition.m_value[i])
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000A728C File Offset: 0x000A628C
		public override int GetHashCode()
		{
			if (this.m_hashAlg == null && this.m_element != null)
			{
				this.ParseHashAlgorithm();
			}
			int num = (this.m_hashAlg != null) ? this.m_hashAlg.GetType().GetHashCode() : 0;
			if (this.m_value == null && this.m_element != null)
			{
				this.ParseHashValue();
			}
			return num ^ HashMembershipCondition.GetByteArrayHashCode(this.m_value);
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000A72F0 File Offset: 0x000A62F0
		public override string ToString()
		{
			if (this.m_hashAlg == null)
			{
				this.ParseHashAlgorithm();
			}
			return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Hash_ToString"), new object[]
			{
				this.m_hashAlg.GetType().AssemblyQualifiedName,
				Hex.EncodeHexString(this.HashValue)
			});
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000A7348 File Offset: 0x000A6348
		private void ParseHashValue()
		{
			lock (this.InternalSyncObject)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("HashValue");
					if (text == null)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[]
						{
							"HashValue",
							base.GetType().FullName
						}));
					}
					this.m_value = Hex.DecodeHexString(text);
					if (this.m_value != null && this.m_hashAlg != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000A73F8 File Offset: 0x000A63F8
		private void ParseHashAlgorithm()
		{
			lock (this.InternalSyncObject)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("HashAlgorithm");
					if (text != null)
					{
						this.m_hashAlg = HashAlgorithm.Create(text);
					}
					else
					{
						this.m_hashAlg = new SHA1Managed();
					}
					if (this.m_value != null && this.m_hashAlg != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000A747C File Offset: 0x000A647C
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

		// Token: 0x060030C2 RID: 12482 RVA: 0x000A74B0 File Offset: 0x000A64B0
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

		// Token: 0x04001874 RID: 6260
		private const string s_tagHashValue = "HashValue";

		// Token: 0x04001875 RID: 6261
		private const string s_tagHashAlgorithm = "HashAlgorithm";

		// Token: 0x04001876 RID: 6262
		private byte[] m_value;

		// Token: 0x04001877 RID: 6263
		private HashAlgorithm m_hashAlg;

		// Token: 0x04001878 RID: 6264
		private SecurityElement m_element;

		// Token: 0x04001879 RID: 6265
		private object s_InternalSyncObject;
	}
}
