using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x020004BE RID: 1214
	[ComVisible(true)]
	[Serializable]
	public sealed class GacInstalled : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06003084 RID: 12420 RVA: 0x000A679E File Offset: 0x000A579E
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new GacIdentityPermission();
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000A67A5 File Offset: 0x000A57A5
		public override bool Equals(object o)
		{
			return o is GacInstalled;
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x000A67B2 File Offset: 0x000A57B2
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x000A67B5 File Offset: 0x000A57B5
		public object Copy()
		{
			return new GacInstalled();
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000A67BC File Offset: 0x000A57BC
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement(base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000A67EB File Offset: 0x000A57EB
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position] = '\t';
			return position + 1;
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000A67F5 File Offset: 0x000A57F5
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return 1;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000A67F8 File Offset: 0x000A57F8
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return position;
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x000A67FB File Offset: 0x000A57FB
		public override string ToString()
		{
			return this.ToXml().ToString();
		}
	}
}
