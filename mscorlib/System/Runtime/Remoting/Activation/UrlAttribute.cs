using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020007A9 RID: 1961
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlAttribute : ContextAttribute
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x000ED2B7 File Offset: 0x000EC2B7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public UrlAttribute(string callsiteURL) : base(UrlAttribute.propertyName)
		{
			if (callsiteURL == null)
			{
				throw new ArgumentNullException("callsiteURL");
			}
			this.url = callsiteURL;
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x000ED2D9 File Offset: 0x000EC2D9
		public override bool Equals(object o)
		{
			return o is IContextProperty && o is UrlAttribute && ((UrlAttribute)o).UrlValue.Equals(this.url);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x000ED303 File Offset: 0x000EC303
		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x000ED310 File Offset: 0x000EC310
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return false;
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x000ED313 File Offset: 0x000EC313
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x000ED315 File Offset: 0x000EC315
		public string UrlValue
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this.url;
			}
		}

		// Token: 0x040022A4 RID: 8868
		private string url;

		// Token: 0x040022A5 RID: 8869
		private static string propertyName = "UrlAttribute";
	}
}
