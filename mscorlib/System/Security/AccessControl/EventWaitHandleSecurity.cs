using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x020008F2 RID: 2290
	public sealed class EventWaitHandleSecurity : NativeObjectSecurity
	{
		// Token: 0x0600530A RID: 21258 RVA: 0x0012B9D5 File Offset: 0x0012A9D5
		public EventWaitHandleSecurity() : base(true, ResourceType.KernelObject)
		{
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x0012B9DF File Offset: 0x0012A9DF
		internal EventWaitHandleSecurity(string name, AccessControlSections includeSections) : base(true, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(EventWaitHandleSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x0012B9F8 File Offset: 0x0012A9F8
		internal EventWaitHandleSecurity(SafeWaitHandle handle, AccessControlSections includeSections) : base(true, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(EventWaitHandleSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x0012BA14 File Offset: 0x0012AA14
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

		// Token: 0x0600530E RID: 21262 RVA: 0x0012BA62 File Offset: 0x0012AA62
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new EventWaitHandleAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x0012BA72 File Offset: 0x0012AA72
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new EventWaitHandleAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x0012BA84 File Offset: 0x0012AA84
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

		// Token: 0x06005311 RID: 21265 RVA: 0x0012BAC4 File Offset: 0x0012AAC4
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

		// Token: 0x06005312 RID: 21266 RVA: 0x0012BB28 File Offset: 0x0012AB28
		public void AddAccessRule(EventWaitHandleAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x0012BB31 File Offset: 0x0012AB31
		public void SetAccessRule(EventWaitHandleAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x0012BB3A File Offset: 0x0012AB3A
		public void ResetAccessRule(EventWaitHandleAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x0012BB43 File Offset: 0x0012AB43
		public bool RemoveAccessRule(EventWaitHandleAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x0012BB4C File Offset: 0x0012AB4C
		public void RemoveAccessRuleAll(EventWaitHandleAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x0012BB55 File Offset: 0x0012AB55
		public void RemoveAccessRuleSpecific(EventWaitHandleAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x0012BB5E File Offset: 0x0012AB5E
		public void AddAuditRule(EventWaitHandleAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x0012BB67 File Offset: 0x0012AB67
		public void SetAuditRule(EventWaitHandleAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x0012BB70 File Offset: 0x0012AB70
		public bool RemoveAuditRule(EventWaitHandleAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x0012BB79 File Offset: 0x0012AB79
		public void RemoveAuditRuleAll(EventWaitHandleAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x0012BB82 File Offset: 0x0012AB82
		public void RemoveAuditRuleSpecific(EventWaitHandleAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x0600531D RID: 21277 RVA: 0x0012BB8B File Offset: 0x0012AB8B
		public override Type AccessRightType
		{
			get
			{
				return typeof(EventWaitHandleRights);
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x0600531E RID: 21278 RVA: 0x0012BB97 File Offset: 0x0012AB97
		public override Type AccessRuleType
		{
			get
			{
				return typeof(EventWaitHandleAccessRule);
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x0600531F RID: 21279 RVA: 0x0012BBA3 File Offset: 0x0012ABA3
		public override Type AuditRuleType
		{
			get
			{
				return typeof(EventWaitHandleAuditRule);
			}
		}
	}
}
