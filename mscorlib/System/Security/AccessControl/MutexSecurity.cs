using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x020008FC RID: 2300
	public sealed class MutexSecurity : NativeObjectSecurity
	{
		// Token: 0x06005352 RID: 21330 RVA: 0x0012C278 File Offset: 0x0012B278
		public MutexSecurity() : base(true, ResourceType.KernelObject)
		{
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x0012C282 File Offset: 0x0012B282
		public MutexSecurity(string name, AccessControlSections includeSections) : base(true, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(MutexSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x0012C29B File Offset: 0x0012B29B
		internal MutexSecurity(SafeWaitHandle handle, AccessControlSections includeSections) : base(true, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(MutexSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x0012C2B4 File Offset: 0x0012B2B4
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception result = null;
			if (errorCode == 2 || errorCode == 6 || errorCode == 123)
			{
				if (name != null && name.Length != 0)
				{
					result = new WaitHandleCannotBeOpenedException(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException_InvalidHandle", new object[]
					{
						name
					}));
				}
				else
				{
					result = new WaitHandleCannotBeOpenedException();
				}
			}
			return result;
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x0012C302 File Offset: 0x0012B302
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new MutexAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x0012C312 File Offset: 0x0012B312
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new MutexAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x0012C324 File Offset: 0x0012B324
		internal AccessControlSections GetAccessControlSectionsFromChanges()
		{
			AccessControlSections accessControlSections = AccessControlSections.None;
			if (base.AccessRulesModified)
			{
				accessControlSections = AccessControlSections.Access;
			}
			if (base.AuditRulesModified)
			{
				accessControlSections |= AccessControlSections.Audit;
			}
			if (base.OwnerModified)
			{
				accessControlSections |= AccessControlSections.Owner;
			}
			if (base.GroupModified)
			{
				accessControlSections |= AccessControlSections.Group;
			}
			return accessControlSections;
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x0012C364 File Offset: 0x0012B364
		internal void Persist(SafeWaitHandle handle)
		{
			base.WriteLock();
			try
			{
				AccessControlSections accessControlSectionsFromChanges = this.GetAccessControlSectionsFromChanges();
				if (accessControlSectionsFromChanges != AccessControlSections.None)
				{
					base.Persist(handle, accessControlSectionsFromChanges);
					base.OwnerModified = (base.GroupModified = (base.AuditRulesModified = (base.AccessRulesModified = false)));
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x0012C3C8 File Offset: 0x0012B3C8
		public void AddAccessRule(MutexAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x0012C3D1 File Offset: 0x0012B3D1
		public void SetAccessRule(MutexAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x0012C3DA File Offset: 0x0012B3DA
		public void ResetAccessRule(MutexAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x0012C3E3 File Offset: 0x0012B3E3
		public bool RemoveAccessRule(MutexAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x0012C3EC File Offset: 0x0012B3EC
		public void RemoveAccessRuleAll(MutexAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x0012C3F5 File Offset: 0x0012B3F5
		public void RemoveAccessRuleSpecific(MutexAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x0012C3FE File Offset: 0x0012B3FE
		public void AddAuditRule(MutexAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x0012C407 File Offset: 0x0012B407
		public void SetAuditRule(MutexAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x0012C410 File Offset: 0x0012B410
		public bool RemoveAuditRule(MutexAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0012C419 File Offset: 0x0012B419
		public void RemoveAuditRuleAll(MutexAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x0012C422 File Offset: 0x0012B422
		public void RemoveAuditRuleSpecific(MutexAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005365 RID: 21349 RVA: 0x0012C42B File Offset: 0x0012B42B
		public override Type AccessRightType
		{
			get
			{
				return typeof(MutexRights);
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x0012C437 File Offset: 0x0012B437
		public override Type AccessRuleType
		{
			get
			{
				return typeof(MutexAccessRule);
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x0012C443 File Offset: 0x0012B443
		public override Type AuditRuleType
		{
			get
			{
				return typeof(MutexAuditRule);
			}
		}
	}
}
