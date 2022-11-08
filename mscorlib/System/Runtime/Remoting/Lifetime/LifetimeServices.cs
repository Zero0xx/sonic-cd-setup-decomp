using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x0200070E RID: 1806
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public sealed class LifetimeServices
	{
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x000DA7F8 File Offset: 0x000D97F8
		private static object LifetimeSyncObject
		{
			get
			{
				if (LifetimeServices.s_LifetimeSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref LifetimeServices.s_LifetimeSyncObject, value, null);
				}
				return LifetimeServices.s_LifetimeSyncObject;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x000DA824 File Offset: 0x000D9824
		// (set) Token: 0x06004022 RID: 16418 RVA: 0x000DA82C File Offset: 0x000D982C
		public static TimeSpan LeaseTime
		{
			get
			{
				return LifetimeServices.m_leaseTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					if (LifetimeServices.isLeaseTime)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_SetOnce"), new object[]
						{
							"LeaseTime"
						}));
					}
					LifetimeServices.m_leaseTime = value;
					LifetimeServices.isLeaseTime = true;
				}
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x000DA89C File Offset: 0x000D989C
		// (set) Token: 0x06004024 RID: 16420 RVA: 0x000DA8A4 File Offset: 0x000D98A4
		public static TimeSpan RenewOnCallTime
		{
			get
			{
				return LifetimeServices.m_renewOnCallTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					if (LifetimeServices.isRenewOnCallTime)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_SetOnce"), new object[]
						{
							"RenewOnCallTime"
						}));
					}
					LifetimeServices.m_renewOnCallTime = value;
					LifetimeServices.isRenewOnCallTime = true;
				}
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06004025 RID: 16421 RVA: 0x000DA914 File Offset: 0x000D9914
		// (set) Token: 0x06004026 RID: 16422 RVA: 0x000DA91C File Offset: 0x000D991C
		public static TimeSpan SponsorshipTimeout
		{
			get
			{
				return LifetimeServices.m_sponsorshipTimeout;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					if (LifetimeServices.isSponsorshipTimeout)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_SetOnce"), new object[]
						{
							"SponsorshipTimeout"
						}));
					}
					LifetimeServices.m_sponsorshipTimeout = value;
					LifetimeServices.isSponsorshipTimeout = true;
				}
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x000DA98C File Offset: 0x000D998C
		// (set) Token: 0x06004028 RID: 16424 RVA: 0x000DA994 File Offset: 0x000D9994
		public static TimeSpan LeaseManagerPollTime
		{
			get
			{
				return LifetimeServices.m_pollTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					LifetimeServices.m_pollTime = value;
					if (LeaseManager.IsInitialized())
					{
						LeaseManager.GetLeaseManager().ChangePollTime(LifetimeServices.m_pollTime);
					}
				}
			}
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x000DA9E4 File Offset: 0x000D99E4
		internal static ILease GetLeaseInitial(MarshalByRefObject obj)
		{
			LeaseManager leaseManager = LeaseManager.GetLeaseManager(LifetimeServices.LeaseManagerPollTime);
			ILease lease = leaseManager.GetLease(obj);
			if (lease == null)
			{
				lease = LifetimeServices.CreateLease(obj);
			}
			return lease;
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x000DAA14 File Offset: 0x000D9A14
		internal static ILease GetLease(MarshalByRefObject obj)
		{
			LeaseManager leaseManager = LeaseManager.GetLeaseManager(LifetimeServices.LeaseManagerPollTime);
			return leaseManager.GetLease(obj);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x000DAA37 File Offset: 0x000D9A37
		internal static ILease CreateLease(MarshalByRefObject obj)
		{
			return LifetimeServices.CreateLease(LifetimeServices.LeaseTime, LifetimeServices.RenewOnCallTime, LifetimeServices.SponsorshipTimeout, obj);
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x000DAA4E File Offset: 0x000D9A4E
		internal static ILease CreateLease(TimeSpan leaseTime, TimeSpan renewOnCallTime, TimeSpan sponsorshipTimeout, MarshalByRefObject obj)
		{
			LeaseManager.GetLeaseManager(LifetimeServices.LeaseManagerPollTime);
			return new Lease(leaseTime, renewOnCallTime, sponsorshipTimeout, obj);
		}

		// Token: 0x04002067 RID: 8295
		private static bool isLeaseTime = false;

		// Token: 0x04002068 RID: 8296
		private static bool isRenewOnCallTime = false;

		// Token: 0x04002069 RID: 8297
		private static bool isSponsorshipTimeout = false;

		// Token: 0x0400206A RID: 8298
		private static TimeSpan m_leaseTime = TimeSpan.FromMinutes(5.0);

		// Token: 0x0400206B RID: 8299
		private static TimeSpan m_renewOnCallTime = TimeSpan.FromMinutes(2.0);

		// Token: 0x0400206C RID: 8300
		private static TimeSpan m_sponsorshipTimeout = TimeSpan.FromMinutes(2.0);

		// Token: 0x0400206D RID: 8301
		private static TimeSpan m_pollTime = TimeSpan.FromMilliseconds(10000.0);

		// Token: 0x0400206E RID: 8302
		private static object s_LifetimeSyncObject = null;
	}
}
