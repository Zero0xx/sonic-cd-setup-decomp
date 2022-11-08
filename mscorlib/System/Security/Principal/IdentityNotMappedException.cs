using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.Principal
{
	// Token: 0x02000919 RID: 2329
	[ComVisible(false)]
	[Serializable]
	public sealed class IdentityNotMappedException : SystemException
	{
		// Token: 0x0600546C RID: 21612 RVA: 0x00130D14 File Offset: 0x0012FD14
		public IdentityNotMappedException() : base(Environment.GetResourceString("IdentityReference_IdentityNotMapped"))
		{
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x00130D26 File Offset: 0x0012FD26
		public IdentityNotMappedException(string message) : base(message)
		{
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x00130D2F File Offset: 0x0012FD2F
		public IdentityNotMappedException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x00130D39 File Offset: 0x0012FD39
		internal IdentityNotMappedException(string message, IdentityReferenceCollection unmappedIdentities) : this(message)
		{
			this.unmappedIdentities = unmappedIdentities;
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x00130D49 File Offset: 0x0012FD49
		internal IdentityNotMappedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x00130D53 File Offset: 0x0012FD53
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005472 RID: 21618 RVA: 0x00130D5D File Offset: 0x0012FD5D
		public IdentityReferenceCollection UnmappedIdentities
		{
			get
			{
				if (this.unmappedIdentities == null)
				{
					this.unmappedIdentities = new IdentityReferenceCollection();
				}
				return this.unmappedIdentities;
			}
		}

		// Token: 0x04002C01 RID: 11265
		private IdentityReferenceCollection unmappedIdentities;
	}
}
