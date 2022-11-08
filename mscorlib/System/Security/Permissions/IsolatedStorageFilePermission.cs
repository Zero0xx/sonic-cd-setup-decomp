using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000638 RID: 1592
	[ComVisible(true)]
	[Serializable]
	public sealed class IsolatedStorageFilePermission : IsolatedStoragePermission, IBuiltInPermission
	{
		// Token: 0x0600395E RID: 14686 RVA: 0x000C1B42 File Offset: 0x000C0B42
		public IsolatedStorageFilePermission(PermissionState state) : base(state)
		{
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x000C1B4B File Offset: 0x000C0B4B
		internal IsolatedStorageFilePermission(IsolatedStorageContainment UsageAllowed, long ExpirationDays, bool PermanentData) : base(UsageAllowed, ExpirationDays, PermanentData)
		{
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x000C1B58 File Offset: 0x000C0B58
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (!base.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			IsolatedStorageFilePermission isolatedStorageFilePermission = (IsolatedStorageFilePermission)target;
			if (base.IsUnrestricted() || isolatedStorageFilePermission.IsUnrestricted())
			{
				return new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			}
			return new IsolatedStorageFilePermission(PermissionState.None)
			{
				m_userQuota = IsolatedStoragePermission.max(this.m_userQuota, isolatedStorageFilePermission.m_userQuota),
				m_machineQuota = IsolatedStoragePermission.max(this.m_machineQuota, isolatedStorageFilePermission.m_machineQuota),
				m_expirationDays = IsolatedStoragePermission.max(this.m_expirationDays, isolatedStorageFilePermission.m_expirationDays),
				m_permanentData = (this.m_permanentData || isolatedStorageFilePermission.m_permanentData),
				m_allowed = (IsolatedStorageContainment)IsolatedStoragePermission.max((long)this.m_allowed, (long)isolatedStorageFilePermission.m_allowed)
			};
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x000C1C44 File Offset: 0x000C0C44
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_userQuota == 0L && this.m_machineQuota == 0L && this.m_expirationDays == 0L && !this.m_permanentData && this.m_allowed == IsolatedStorageContainment.None;
			}
			bool result;
			try
			{
				IsolatedStorageFilePermission isolatedStorageFilePermission = (IsolatedStorageFilePermission)target;
				if (isolatedStorageFilePermission.IsUnrestricted())
				{
					result = true;
				}
				else
				{
					result = (isolatedStorageFilePermission.m_userQuota >= this.m_userQuota && isolatedStorageFilePermission.m_machineQuota >= this.m_machineQuota && isolatedStorageFilePermission.m_expirationDays >= this.m_expirationDays && (isolatedStorageFilePermission.m_permanentData || !this.m_permanentData) && isolatedStorageFilePermission.m_allowed >= this.m_allowed);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			return result;
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x000C1D30 File Offset: 0x000C0D30
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (!base.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			IsolatedStorageFilePermission isolatedStorageFilePermission = (IsolatedStorageFilePermission)target;
			if (isolatedStorageFilePermission.IsUnrestricted())
			{
				return this.Copy();
			}
			if (base.IsUnrestricted())
			{
				return target.Copy();
			}
			IsolatedStorageFilePermission isolatedStorageFilePermission2 = new IsolatedStorageFilePermission(PermissionState.None);
			isolatedStorageFilePermission2.m_userQuota = IsolatedStoragePermission.min(this.m_userQuota, isolatedStorageFilePermission.m_userQuota);
			isolatedStorageFilePermission2.m_machineQuota = IsolatedStoragePermission.min(this.m_machineQuota, isolatedStorageFilePermission.m_machineQuota);
			isolatedStorageFilePermission2.m_expirationDays = IsolatedStoragePermission.min(this.m_expirationDays, isolatedStorageFilePermission.m_expirationDays);
			isolatedStorageFilePermission2.m_permanentData = (this.m_permanentData && isolatedStorageFilePermission.m_permanentData);
			isolatedStorageFilePermission2.m_allowed = (IsolatedStorageContainment)IsolatedStoragePermission.min((long)this.m_allowed, (long)isolatedStorageFilePermission.m_allowed);
			if (isolatedStorageFilePermission2.m_userQuota == 0L && isolatedStorageFilePermission2.m_machineQuota == 0L && isolatedStorageFilePermission2.m_expirationDays == 0L && !isolatedStorageFilePermission2.m_permanentData && isolatedStorageFilePermission2.m_allowed == IsolatedStorageContainment.None)
			{
				return null;
			}
			return isolatedStorageFilePermission2;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x000C1E50 File Offset: 0x000C0E50
		public override IPermission Copy()
		{
			IsolatedStorageFilePermission isolatedStorageFilePermission = new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			if (!base.IsUnrestricted())
			{
				isolatedStorageFilePermission.m_userQuota = this.m_userQuota;
				isolatedStorageFilePermission.m_machineQuota = this.m_machineQuota;
				isolatedStorageFilePermission.m_expirationDays = this.m_expirationDays;
				isolatedStorageFilePermission.m_permanentData = this.m_permanentData;
				isolatedStorageFilePermission.m_allowed = this.m_allowed;
			}
			return isolatedStorageFilePermission;
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x000C1EA9 File Offset: 0x000C0EA9
		int IBuiltInPermission.GetTokenIndex()
		{
			return IsolatedStorageFilePermission.GetTokenIndex();
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x000C1EB0 File Offset: 0x000C0EB0
		internal static int GetTokenIndex()
		{
			return 3;
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x000C1EB3 File Offset: 0x000C0EB3
		[ComVisible(false)]
		public override SecurityElement ToXml()
		{
			return base.ToXml("System.Security.Permissions.IsolatedStorageFilePermission");
		}
	}
}
