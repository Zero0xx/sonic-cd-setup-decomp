using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Security
{
	// Token: 0x02000691 RID: 1681
	[ComVisible(true)]
	[Serializable]
	public class HostProtectionException : SystemException
	{
		// Token: 0x06003CD7 RID: 15575 RVA: 0x000D0299 File Offset: 0x000CF299
		public HostProtectionException()
		{
			this.m_protected = HostProtectionResource.None;
			this.m_demanded = HostProtectionResource.None;
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x000D02AF File Offset: 0x000CF2AF
		public HostProtectionException(string message) : base(message)
		{
			this.m_protected = HostProtectionResource.None;
			this.m_demanded = HostProtectionResource.None;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x000D02C6 File Offset: 0x000CF2C6
		public HostProtectionException(string message, Exception e) : base(message, e)
		{
			this.m_protected = HostProtectionResource.None;
			this.m_demanded = HostProtectionResource.None;
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x000D02E0 File Offset: 0x000CF2E0
		protected HostProtectionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_protected = (HostProtectionResource)info.GetValue("ProtectedResources", typeof(HostProtectionResource));
			this.m_demanded = (HostProtectionResource)info.GetValue("DemandedResources", typeof(HostProtectionResource));
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x000D0343 File Offset: 0x000CF343
		public HostProtectionException(string message, HostProtectionResource protectedResources, HostProtectionResource demandedResources) : base(message)
		{
			base.SetErrorCode(-2146232768);
			this.m_protected = protectedResources;
			this.m_demanded = demandedResources;
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x000D0365 File Offset: 0x000CF365
		private HostProtectionException(HostProtectionResource protectedResources, HostProtectionResource demandedResources) : base(SecurityException.GetResString("HostProtection_HostProtection"))
		{
			base.SetErrorCode(-2146232768);
			this.m_protected = protectedResources;
			this.m_demanded = demandedResources;
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x000D0390 File Offset: 0x000CF390
		public HostProtectionResource ProtectedResources
		{
			get
			{
				return this.m_protected;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003CDE RID: 15582 RVA: 0x000D0398 File Offset: 0x000CF398
		public HostProtectionResource DemandedResources
		{
			get
			{
				return this.m_demanded;
			}
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x000D03A0 File Offset: 0x000CF3A0
		private string ToStringHelper(string resourceString, object attr)
		{
			if (attr == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(Environment.GetResourceString(resourceString));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(attr);
			return stringBuilder.ToString();
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x000D03FC File Offset: 0x000CF3FC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(this.ToStringHelper("HostProtection_ProtectedResources", this.ProtectedResources));
			stringBuilder.Append(this.ToStringHelper("HostProtection_DemandedResources", this.DemandedResources));
			return stringBuilder.ToString();
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x000D045C File Offset: 0x000CF45C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ProtectedResources", this.ProtectedResources, typeof(HostProtectionResource));
			info.AddValue("DemandedResources", this.DemandedResources, typeof(HostProtectionResource));
		}

		// Token: 0x04001F43 RID: 8003
		private const string ProtectedResourcesName = "ProtectedResources";

		// Token: 0x04001F44 RID: 8004
		private const string DemandedResourcesName = "DemandedResources";

		// Token: 0x04001F45 RID: 8005
		private HostProtectionResource m_protected;

		// Token: 0x04001F46 RID: 8006
		private HostProtectionResource m_demanded;
	}
}
