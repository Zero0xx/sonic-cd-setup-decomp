using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008F5 RID: 2293
	public sealed class FileSystemAuditRule : AuditRule
	{
		// Token: 0x06005328 RID: 21288 RVA: 0x0012BCA6 File Offset: 0x0012ACA6
		public FileSystemAuditRule(IdentityReference identity, FileSystemRights fileSystemRights, AuditFlags flags) : this(identity, fileSystemRights, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x0012BCB3 File Offset: 0x0012ACB3
		public FileSystemAuditRule(IdentityReference identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(identity, FileSystemAuditRule.AccessMaskFromRights(fileSystemRights), false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x0012BCC8 File Offset: 0x0012ACC8
		public FileSystemAuditRule(string identity, FileSystemRights fileSystemRights, AuditFlags flags) : this(new NTAccount(identity), fileSystemRights, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x0012BCDA File Offset: 0x0012ACDA
		public FileSystemAuditRule(string identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(new NTAccount(identity), FileSystemAuditRule.AccessMaskFromRights(fileSystemRights), false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x0012BCF4 File Offset: 0x0012ACF4
		internal FileSystemAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x0012BD08 File Offset: 0x0012AD08
		private static int AccessMaskFromRights(FileSystemRights fileSystemRights)
		{
			if (fileSystemRights < (FileSystemRights)0 || fileSystemRights > FileSystemRights.FullControl)
			{
				throw new ArgumentOutOfRangeException("fileSystemRights", Environment.GetResourceString("Argument_InvalidEnumValue", new object[]
				{
					fileSystemRights,
					"FileSystemRights"
				}));
			}
			return (int)fileSystemRights;
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x0600532E RID: 21294 RVA: 0x0012BD50 File Offset: 0x0012AD50
		public FileSystemRights FileSystemRights
		{
			get
			{
				return FileSystemAccessRule.RightsFromAccessMask(base.AccessMask);
			}
		}
	}
}
