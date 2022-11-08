using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000706 RID: 1798
	internal class Lease : MarshalByRefObject, ILease
	{
		// Token: 0x06003FED RID: 16365 RVA: 0x000D9968 File Offset: 0x000D8968
		internal Lease(TimeSpan initialLeaseTime, TimeSpan renewOnCallTime, TimeSpan sponsorshipTimeout, MarshalByRefObject managedObject)
		{
			this.id = Lease.nextId++;
			this.renewOnCallTime = renewOnCallTime;
			this.sponsorshipTimeout = sponsorshipTimeout;
			this.initialLeaseTime = initialLeaseTime;
			this.managedObject = managedObject;
			this.leaseManager = LeaseManager.GetLeaseManager();
			this.sponsorTable = new Hashtable(10);
			this.state = LeaseState.Initial;
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x000D99CC File Offset: 0x000D89CC
		internal void ActivateLease()
		{
			this.leaseTime = DateTime.UtcNow.Add(this.initialLeaseTime);
			this.state = LeaseState.Active;
			this.leaseManager.ActivateLease(this);
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x000D9A05 File Offset: 0x000D8A05
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x000D9A08 File Offset: 0x000D8A08
		// (set) Token: 0x06003FF1 RID: 16369 RVA: 0x000D9A10 File Offset: 0x000D8A10
		public TimeSpan RenewOnCallTime
		{
			get
			{
				return this.renewOnCallTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				if (this.state == LeaseState.Initial)
				{
					this.renewOnCallTime = value;
					return;
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_InitialStateRenewOnCall"), new object[]
				{
					this.state.ToString()
				}));
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x000D9A62 File Offset: 0x000D8A62
		// (set) Token: 0x06003FF3 RID: 16371 RVA: 0x000D9A6C File Offset: 0x000D8A6C
		public TimeSpan SponsorshipTimeout
		{
			get
			{
				return this.sponsorshipTimeout;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				if (this.state == LeaseState.Initial)
				{
					this.sponsorshipTimeout = value;
					return;
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_InitialStateSponsorshipTimeout"), new object[]
				{
					this.state.ToString()
				}));
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x000D9ABE File Offset: 0x000D8ABE
		// (set) Token: 0x06003FF5 RID: 16373 RVA: 0x000D9AC8 File Offset: 0x000D8AC8
		public TimeSpan InitialLeaseTime
		{
			get
			{
				return this.initialLeaseTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				if (this.state != LeaseState.Initial)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_InitialStateInitialLeaseTime"), new object[]
					{
						this.state.ToString()
					}));
				}
				this.initialLeaseTime = value;
				if (TimeSpan.Zero.CompareTo(value) >= 0)
				{
					this.state = LeaseState.Null;
					return;
				}
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x000D9B33 File Offset: 0x000D8B33
		public TimeSpan CurrentLeaseTime
		{
			get
			{
				return this.leaseTime.Subtract(DateTime.UtcNow);
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x000D9B45 File Offset: 0x000D8B45
		public LeaseState CurrentState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x000D9B4D File Offset: 0x000D8B4D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public void Register(ISponsor obj)
		{
			this.Register(obj, TimeSpan.Zero);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x000D9B5C File Offset: 0x000D8B5C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public void Register(ISponsor obj, TimeSpan renewalTime)
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired && !(this.sponsorshipTimeout == TimeSpan.Zero))
				{
					object sponsorId = this.GetSponsorId(obj);
					lock (this.sponsorTable)
					{
						if (renewalTime > TimeSpan.Zero)
						{
							this.AddTime(renewalTime);
						}
						if (!this.sponsorTable.ContainsKey(sponsorId))
						{
							this.sponsorTable[sponsorId] = new Lease.SponsorStateInfo(renewalTime, Lease.SponsorState.Initial);
						}
					}
				}
			}
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x000D9C08 File Offset: 0x000D8C08
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public void Unregister(ISponsor sponsor)
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired)
				{
					object sponsorId = this.GetSponsorId(sponsor);
					lock (this.sponsorTable)
					{
						if (sponsorId != null)
						{
							this.leaseManager.DeleteSponsor(sponsorId);
							Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
							this.sponsorTable.Remove(sponsorId);
						}
					}
				}
			}
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x000D9C98 File Offset: 0x000D8C98
		private object GetSponsorId(ISponsor obj)
		{
			object result = null;
			if (obj != null)
			{
				if (RemotingServices.IsTransparentProxy(obj))
				{
					result = RemotingServices.GetRealProxy(obj);
				}
				else
				{
					result = obj;
				}
			}
			return result;
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x000D9CC0 File Offset: 0x000D8CC0
		private ISponsor GetSponsorFromId(object sponsorId)
		{
			RealProxy realProxy = sponsorId as RealProxy;
			object obj;
			if (realProxy != null)
			{
				obj = realProxy.GetTransparentProxy();
			}
			else
			{
				obj = sponsorId;
			}
			return (ISponsor)obj;
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x000D9CEA File Offset: 0x000D8CEA
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public TimeSpan Renew(TimeSpan renewalTime)
		{
			return this.RenewInternal(renewalTime);
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x000D9CF4 File Offset: 0x000D8CF4
		internal TimeSpan RenewInternal(TimeSpan renewalTime)
		{
			TimeSpan result;
			lock (this)
			{
				if (this.state == LeaseState.Expired)
				{
					result = TimeSpan.Zero;
				}
				else
				{
					this.AddTime(renewalTime);
					result = this.leaseTime.Subtract(DateTime.UtcNow);
				}
			}
			return result;
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x000D9D4C File Offset: 0x000D8D4C
		internal void Remove()
		{
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			this.state = LeaseState.Expired;
			this.leaseManager.DeleteLease(this);
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x000D9D6C File Offset: 0x000D8D6C
		internal void Cancel()
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired)
				{
					this.Remove();
					RemotingServices.Disconnect(this.managedObject, false);
					RemotingServices.Disconnect(this);
				}
			}
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x000D9DC0 File Offset: 0x000D8DC0
		internal void RenewOnCall()
		{
			lock (this)
			{
				if (this.state != LeaseState.Initial && this.state != LeaseState.Expired)
				{
					this.AddTime(this.renewOnCallTime);
				}
			}
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x000D9E10 File Offset: 0x000D8E10
		internal void LeaseExpired(DateTime now)
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired)
				{
					if (this.leaseTime.CompareTo(now) < 0)
					{
						this.ProcessNextSponsor();
					}
				}
			}
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x000D9E60 File Offset: 0x000D8E60
		internal void SponsorCall(ISponsor sponsor)
		{
			bool flag = false;
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			lock (this.sponsorTable)
			{
				try
				{
					object sponsorId = this.GetSponsorId(sponsor);
					this.sponsorCallThread = Thread.CurrentThread.GetHashCode();
					Lease.AsyncRenewal asyncRenewal = new Lease.AsyncRenewal(sponsor.Renewal);
					Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
					sponsorStateInfo.sponsorState = Lease.SponsorState.Waiting;
					asyncRenewal.BeginInvoke(this, new AsyncCallback(this.SponsorCallback), null);
					if (sponsorStateInfo.sponsorState == Lease.SponsorState.Waiting && this.state != LeaseState.Expired)
					{
						this.leaseManager.RegisterSponsorCall(this, sponsorId, this.sponsorshipTimeout);
					}
					this.sponsorCallThread = 0;
				}
				catch (Exception)
				{
					flag = true;
					this.sponsorCallThread = 0;
				}
			}
			if (flag)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
			}
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x000D9F4C File Offset: 0x000D8F4C
		internal void SponsorTimeout(object sponsorId)
		{
			lock (this)
			{
				if (this.sponsorTable.ContainsKey(sponsorId))
				{
					lock (this.sponsorTable)
					{
						Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
						if (sponsorStateInfo.sponsorState == Lease.SponsorState.Waiting)
						{
							this.Unregister(this.GetSponsorFromId(sponsorId));
							this.ProcessNextSponsor();
						}
					}
				}
			}
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x000D9FDC File Offset: 0x000D8FDC
		private void ProcessNextSponsor()
		{
			object obj = null;
			TimeSpan timeSpan = TimeSpan.Zero;
			lock (this.sponsorTable)
			{
				IDictionaryEnumerator enumerator = this.sponsorTable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					object key = enumerator.Key;
					Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)enumerator.Value;
					if (sponsorStateInfo.sponsorState == Lease.SponsorState.Initial && timeSpan == TimeSpan.Zero)
					{
						timeSpan = sponsorStateInfo.renewalTime;
						obj = key;
					}
					else if (sponsorStateInfo.renewalTime > timeSpan)
					{
						timeSpan = sponsorStateInfo.renewalTime;
						obj = key;
					}
				}
			}
			if (obj != null)
			{
				this.SponsorCall(this.GetSponsorFromId(obj));
				return;
			}
			this.Cancel();
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x000DA098 File Offset: 0x000D9098
		internal void SponsorCallback(object obj)
		{
			this.SponsorCallback((IAsyncResult)obj);
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x000DA0A8 File Offset: 0x000D90A8
		internal void SponsorCallback(IAsyncResult iar)
		{
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			int hashCode = Thread.CurrentThread.GetHashCode();
			if (hashCode == this.sponsorCallThread)
			{
				WaitCallback callBack = new WaitCallback(this.SponsorCallback);
				ThreadPool.QueueUserWorkItem(callBack, iar);
				return;
			}
			AsyncResult asyncResult = (AsyncResult)iar;
			Lease.AsyncRenewal asyncRenewal = (Lease.AsyncRenewal)asyncResult.AsyncDelegate;
			ISponsor sponsor = (ISponsor)asyncRenewal.Target;
			Lease.SponsorStateInfo sponsorStateInfo = null;
			if (!iar.IsCompleted)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
				return;
			}
			bool flag = false;
			TimeSpan renewalTime = TimeSpan.Zero;
			try
			{
				renewalTime = asyncRenewal.EndInvoke(iar);
			}
			catch (Exception)
			{
				flag = true;
			}
			if (flag)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
				return;
			}
			object sponsorId = this.GetSponsorId(sponsor);
			lock (this.sponsorTable)
			{
				if (this.sponsorTable.ContainsKey(sponsorId))
				{
					sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
					sponsorStateInfo.sponsorState = Lease.SponsorState.Completed;
					sponsorStateInfo.renewalTime = renewalTime;
				}
			}
			if (sponsorStateInfo == null)
			{
				this.ProcessNextSponsor();
				return;
			}
			if (sponsorStateInfo.renewalTime == TimeSpan.Zero)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
				return;
			}
			this.RenewInternal(sponsorStateInfo.renewalTime);
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x000DA204 File Offset: 0x000D9204
		private void AddTime(TimeSpan renewalSpan)
		{
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			DateTime dateTime = DateTime.UtcNow.Add(renewalSpan);
			if (this.leaseTime.CompareTo(dateTime) < 0)
			{
				this.leaseManager.ChangedLeaseTime(this, dateTime);
				this.leaseTime = dateTime;
				this.state = LeaseState.Active;
			}
		}

		// Token: 0x04002043 RID: 8259
		internal int id;

		// Token: 0x04002044 RID: 8260
		internal DateTime leaseTime;

		// Token: 0x04002045 RID: 8261
		internal TimeSpan initialLeaseTime;

		// Token: 0x04002046 RID: 8262
		internal TimeSpan renewOnCallTime;

		// Token: 0x04002047 RID: 8263
		internal TimeSpan sponsorshipTimeout;

		// Token: 0x04002048 RID: 8264
		internal bool isInfinite;

		// Token: 0x04002049 RID: 8265
		internal Hashtable sponsorTable;

		// Token: 0x0400204A RID: 8266
		internal int sponsorCallThread;

		// Token: 0x0400204B RID: 8267
		internal LeaseManager leaseManager;

		// Token: 0x0400204C RID: 8268
		internal MarshalByRefObject managedObject;

		// Token: 0x0400204D RID: 8269
		internal LeaseState state;

		// Token: 0x0400204E RID: 8270
		internal static int nextId;

		// Token: 0x02000707 RID: 1799
		// (Invoke) Token: 0x0600400A RID: 16394
		internal delegate TimeSpan AsyncRenewal(ILease lease);

		// Token: 0x02000708 RID: 1800
		[Serializable]
		internal enum SponsorState
		{
			// Token: 0x04002050 RID: 8272
			Initial,
			// Token: 0x04002051 RID: 8273
			Waiting,
			// Token: 0x04002052 RID: 8274
			Completed
		}

		// Token: 0x02000709 RID: 1801
		internal sealed class SponsorStateInfo
		{
			// Token: 0x0600400D RID: 16397 RVA: 0x000DA254 File Offset: 0x000D9254
			internal SponsorStateInfo(TimeSpan renewalTime, Lease.SponsorState sponsorState)
			{
				this.renewalTime = renewalTime;
				this.sponsorState = sponsorState;
			}

			// Token: 0x04002053 RID: 8275
			internal TimeSpan renewalTime;

			// Token: 0x04002054 RID: 8276
			internal Lease.SponsorState sponsorState;
		}
	}
}
