using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004C3 RID: 1219
	[ComVisible(true)]
	[Serializable]
	public sealed class PublisherMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x000A7787 File Offset: 0x000A6787
		internal PublisherMembershipCondition()
		{
			this.m_element = null;
			this.m_certificate = null;
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000A779D File Offset: 0x000A679D
		public PublisherMembershipCondition(X509Certificate certificate)
		{
			PublisherMembershipCondition.CheckCertificate(certificate);
			this.m_certificate = new X509Certificate(certificate);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000A77B7 File Offset: 0x000A67B7
		private static void CheckCertificate(X509Certificate certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x000A77DB File Offset: 0x000A67DB
		// (set) Token: 0x060030D4 RID: 12500 RVA: 0x000A77C7 File Offset: 0x000A67C7
		public X509Certificate Certificate
		{
			get
			{
				if (this.m_certificate == null && this.m_element != null)
				{
					this.ParseCertificate();
				}
				if (this.m_certificate != null)
				{
					return new X509Certificate(this.m_certificate);
				}
				return null;
			}
			set
			{
				PublisherMembershipCondition.CheckCertificate(value);
				this.m_certificate = new X509Certificate(value);
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000A7808 File Offset: 0x000A6808
		public override string ToString()
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			if (this.m_certificate == null)
			{
				return Environment.GetResourceString("Publisher_ToString");
			}
			string subject = this.m_certificate.Subject;
			if (subject != null)
			{
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Publisher_ToStringArg"), new object[]
				{
					Hex.EncodeHexString(this.m_certificate.GetPublicKey())
				});
			}
			return Environment.GetResourceString("Publisher_ToString");
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000A7888 File Offset: 0x000A6888
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000A78A0 File Offset: 0x000A68A0
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
				Publisher publisher = obj as Publisher;
				if (publisher != null)
				{
					if (this.m_certificate == null && this.m_element != null)
					{
						this.ParseCertificate();
					}
					if (publisher.Equals(new Publisher(this.m_certificate)))
					{
						usedEvidence = publisher;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000A7904 File Offset: 0x000A6904
		public IMembershipCondition Copy()
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			return new PublisherMembershipCondition(this.m_certificate);
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000A7927 File Offset: 0x000A6927
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000A7930 File Offset: 0x000A6930
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000A793C File Offset: 0x000A693C
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.PublisherMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_certificate != null)
			{
				securityElement.AddAttribute("X509Certificate", this.m_certificate.GetRawCertDataString());
			}
			return securityElement;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000A79AC File Offset: 0x000A69AC
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
			lock (this)
			{
				this.m_element = e;
				this.m_certificate = null;
			}
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000A7A18 File Offset: 0x000A6A18
		private void ParseCertificate()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("X509Certificate");
					this.m_certificate = ((text == null) ? null : new X509Certificate(Hex.DecodeHexString(text)));
					PublisherMembershipCondition.CheckCertificate(this.m_certificate);
					this.m_element = null;
				}
			}
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000A7A8C File Offset: 0x000A6A8C
		public override bool Equals(object o)
		{
			PublisherMembershipCondition publisherMembershipCondition = o as PublisherMembershipCondition;
			if (publisherMembershipCondition != null)
			{
				if (this.m_certificate == null && this.m_element != null)
				{
					this.ParseCertificate();
				}
				if (publisherMembershipCondition.m_certificate == null && publisherMembershipCondition.m_element != null)
				{
					publisherMembershipCondition.ParseCertificate();
				}
				if (Publisher.PublicKeyEquals(this.m_certificate, publisherMembershipCondition.m_certificate))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000A7AE5 File Offset: 0x000A6AE5
		public override int GetHashCode()
		{
			if (this.m_certificate == null && this.m_element != null)
			{
				this.ParseCertificate();
			}
			if (this.m_certificate != null)
			{
				return this.m_certificate.GetHashCode();
			}
			return typeof(PublisherMembershipCondition).GetHashCode();
		}

		// Token: 0x0400187B RID: 6267
		private X509Certificate m_certificate;

		// Token: 0x0400187C RID: 6268
		private SecurityElement m_element;
	}
}
