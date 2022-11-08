using System;
using System.Text;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000348 RID: 840
	public class ExtendedProtectionPolicy
	{
		// Token: 0x06001A6F RID: 6767 RVA: 0x0005C514 File Offset: 0x0005B514
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ServiceNameCollection customServiceNames)
		{
			if (policyEnforcement == PolicyEnforcement.Never)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_UseDifferentConstructorForNever"), "policyEnforcement");
			}
			if (customServiceNames != null && customServiceNames.Count == 0)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_NoEmptyServiceNameCollection"), "customServiceNames");
			}
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = protectionScenario;
			this.customServiceNames = customServiceNames;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0005C574 File Offset: 0x0005B574
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ChannelBinding customChannelBinding)
		{
			if (policyEnforcement == PolicyEnforcement.Never)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_UseDifferentConstructorForNever"), "policyEnforcement");
			}
			if (customChannelBinding == null)
			{
				throw new ArgumentNullException("customChannelBinding");
			}
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = ProtectionScenario.TransportSelected;
			this.customChannelBinding = customChannelBinding;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0005C5C2 File Offset: 0x0005B5C2
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement)
		{
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = ProtectionScenario.TransportSelected;
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0005C5D8 File Offset: 0x0005B5D8
		public ServiceNameCollection CustomServiceNames
		{
			get
			{
				return this.customServiceNames;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x0005C5E0 File Offset: 0x0005B5E0
		public PolicyEnforcement PolicyEnforcement
		{
			get
			{
				return this.policyEnforcement;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x0005C5E8 File Offset: 0x0005B5E8
		public ProtectionScenario ProtectionScenario
		{
			get
			{
				return this.protectionScenario;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x0005C5F0 File Offset: 0x0005B5F0
		public ChannelBinding CustomChannelBinding
		{
			get
			{
				return this.customChannelBinding;
			}
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0005C5F8 File Offset: 0x0005B5F8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProtectionScenario=");
			stringBuilder.Append(this.protectionScenario.ToString());
			stringBuilder.Append("; PolicyEnforcement=");
			stringBuilder.Append(this.policyEnforcement.ToString());
			stringBuilder.Append("; CustomChannelBinding=");
			if (this.customChannelBinding == null)
			{
				stringBuilder.Append("<null>");
			}
			else
			{
				stringBuilder.Append(this.customChannelBinding.ToString());
			}
			stringBuilder.Append("; ServiceNames=");
			if (this.customServiceNames == null)
			{
				stringBuilder.Append("<null>");
			}
			else
			{
				bool flag = true;
				foreach (object obj in this.customServiceNames)
				{
					string value = (string)obj;
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(value);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001B3B RID: 6971
		private ServiceNameCollection customServiceNames;

		// Token: 0x04001B3C RID: 6972
		private PolicyEnforcement policyEnforcement;

		// Token: 0x04001B3D RID: 6973
		private ProtectionScenario protectionScenario;

		// Token: 0x04001B3E RID: 6974
		private ChannelBinding customChannelBinding;
	}
}
