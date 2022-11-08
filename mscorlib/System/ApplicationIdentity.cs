using System;
using System.Deployment.Internal.Isolation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000069 RID: 105
	[ComVisible(false)]
	[Serializable]
	public sealed class ApplicationIdentity : ISerializable
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x00015548 File Offset: 0x00014548
		private ApplicationIdentity()
		{
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00015550 File Offset: 0x00014550
		private ApplicationIdentity(SerializationInfo info, StreamingContext context)
		{
			string text = (string)info.GetValue("FullName", typeof(string));
			if (text == null)
			{
				throw new ArgumentNullException("fullName");
			}
			this._appId = IsolationInterop.AppIdAuthority.TextToDefinition(0U, text);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001559E File Offset: 0x0001459E
		public ApplicationIdentity(string applicationIdentityFullName)
		{
			if (applicationIdentityFullName == null)
			{
				throw new ArgumentNullException("applicationIdentityFullName");
			}
			this._appId = IsolationInterop.AppIdAuthority.TextToDefinition(0U, applicationIdentityFullName);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000155C6 File Offset: 0x000145C6
		internal ApplicationIdentity(IDefinitionAppId applicationIdentity)
		{
			this._appId = applicationIdentity;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000155D5 File Offset: 0x000145D5
		public string FullName
		{
			get
			{
				return IsolationInterop.AppIdAuthority.DefinitionToText(0U, this._appId);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000155E8 File Offset: 0x000145E8
		public string CodeBase
		{
			get
			{
				return this._appId.get_Codebase();
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000155F5 File Offset: 0x000145F5
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000155FD File Offset: 0x000145FD
		internal IDefinitionAppId Identity
		{
			get
			{
				return this._appId;
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00015605 File Offset: 0x00014605
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("FullName", this.FullName, typeof(string));
		}

		// Token: 0x040001F0 RID: 496
		private IDefinitionAppId _appId;
	}
}
