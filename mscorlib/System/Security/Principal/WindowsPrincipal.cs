using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x020004D4 RID: 1236
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SecurityInfrastructure = true)]
	[Serializable]
	public class WindowsPrincipal : IPrincipal
	{
		// Token: 0x06003126 RID: 12582 RVA: 0x000A8D3C File Offset: 0x000A7D3C
		private WindowsPrincipal()
		{
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000A8D44 File Offset: 0x000A7D44
		public WindowsPrincipal(WindowsIdentity ntIdentity)
		{
			if (ntIdentity == null)
			{
				throw new ArgumentNullException("ntIdentity");
			}
			this.m_identity = ntIdentity;
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x000A8D61 File Offset: 0x000A7D61
		public virtual IIdentity Identity
		{
			get
			{
				return this.m_identity;
			}
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000A8D6C File Offset: 0x000A7D6C
		public virtual bool IsInRole(string role)
		{
			if (role == null || role.Length == 0)
			{
				return false;
			}
			NTAccount identity = new NTAccount(role);
			IdentityReferenceCollection identityReferenceCollection = NTAccount.Translate(new IdentityReferenceCollection(1)
			{
				identity
			}, typeof(SecurityIdentifier), false);
			SecurityIdentifier securityIdentifier = identityReferenceCollection[0] as SecurityIdentifier;
			return !(securityIdentifier == null) && this.IsInRole(securityIdentifier);
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000A8DCC File Offset: 0x000A7DCC
		public virtual bool IsInRole(WindowsBuiltInRole role)
		{
			if (role < WindowsBuiltInRole.Administrator || role > WindowsBuiltInRole.Replicator)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[]
				{
					(int)role
				}), "role");
			}
			return this.IsInRole((int)role);
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000A8E18 File Offset: 0x000A7E18
		public virtual bool IsInRole(int rid)
		{
			SecurityIdentifier sid = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[]
			{
				32,
				rid
			});
			return this.IsInRole(sid);
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000A8E48 File Offset: 0x000A7E48
		[ComVisible(false)]
		public virtual bool IsInRole(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.m_identity.TokenHandle.IsInvalid)
			{
				return false;
			}
			SafeTokenHandle invalidHandle = SafeTokenHandle.InvalidHandle;
			if (this.m_identity.ImpersonationLevel == TokenImpersonationLevel.None && !Win32Native.DuplicateTokenEx(this.m_identity.TokenHandle, 8U, IntPtr.Zero, 2U, 2U, ref invalidHandle))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			bool result = false;
			if (!Win32Native.CheckTokenMembership((this.m_identity.ImpersonationLevel != TokenImpersonationLevel.None) ? this.m_identity.TokenHandle : invalidHandle, sid.BinaryForm, ref result))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			invalidHandle.Dispose();
			return result;
		}

		// Token: 0x040018DA RID: 6362
		private WindowsIdentity m_identity;

		// Token: 0x040018DB RID: 6363
		private string[] m_roles;

		// Token: 0x040018DC RID: 6364
		private Hashtable m_rolesTable;

		// Token: 0x040018DD RID: 6365
		private bool m_rolesLoaded;
	}
}
