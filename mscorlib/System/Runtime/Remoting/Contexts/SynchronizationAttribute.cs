using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200079A RID: 1946
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class SynchronizationAttribute : ContextAttribute, IContributeServerContextSink, IContributeClientContextSink
	{
		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600454F RID: 17743 RVA: 0x000EBE16 File Offset: 0x000EAE16
		// (set) Token: 0x06004550 RID: 17744 RVA: 0x000EBE1E File Offset: 0x000EAE1E
		public virtual bool Locked
		{
			get
			{
				return this._locked;
			}
			set
			{
				this._locked = value;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004551 RID: 17745 RVA: 0x000EBE27 File Offset: 0x000EAE27
		public virtual bool IsReEntrant
		{
			get
			{
				return this._bReEntrant;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x000EBE2F File Offset: 0x000EAE2F
		// (set) Token: 0x06004553 RID: 17747 RVA: 0x000EBE37 File Offset: 0x000EAE37
		internal string SyncCallOutLCID
		{
			get
			{
				return this._syncLcid;
			}
			set
			{
				this._syncLcid = value;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x000EBE40 File Offset: 0x000EAE40
		internal ArrayList AsyncCallOutLCIDList
		{
			get
			{
				return this._asyncLcidList;
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x000EBE48 File Offset: 0x000EAE48
		internal bool IsKnownLCID(IMessage reqMsg)
		{
			string logicalCallID = ((LogicalCallContext)reqMsg.Properties[Message.CallContextKey]).RemotingData.LogicalCallID;
			return logicalCallID.Equals(this._syncLcid) || this._asyncLcidList.Contains(logicalCallID);
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x000EBE91 File Offset: 0x000EAE91
		public SynchronizationAttribute() : this(4, false)
		{
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x000EBE9B File Offset: 0x000EAE9B
		public SynchronizationAttribute(bool reEntrant) : this(4, reEntrant)
		{
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x000EBEA5 File Offset: 0x000EAEA5
		public SynchronizationAttribute(int flag) : this(flag, false)
		{
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x000EBEB0 File Offset: 0x000EAEB0
		public SynchronizationAttribute(int flag, bool reEntrant) : base("Synchronization")
		{
			this._bReEntrant = reEntrant;
			switch (flag)
			{
			case 1:
			case 2:
			case 4:
				break;
			case 3:
				goto IL_38;
			default:
				if (flag != 8)
				{
					goto IL_38;
				}
				break;
			}
			this._flavor = flag;
			return;
			IL_38:
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "flag");
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x000EBF09 File Offset: 0x000EAF09
		internal void Dispose()
		{
			if (this._waitHandle != null)
			{
				this._waitHandle.Unregister(null);
			}
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x000EBF20 File Offset: 0x000EAF20
		[ComVisible(true)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (msg == null)
			{
				throw new ArgumentNullException("msg");
			}
			bool result = true;
			if (this._flavor == 8)
			{
				result = false;
			}
			else
			{
				SynchronizationAttribute synchronizationAttribute = (SynchronizationAttribute)ctx.GetProperty("Synchronization");
				if ((this._flavor == 1 && synchronizationAttribute != null) || (this._flavor == 4 && synchronizationAttribute == null))
				{
					result = false;
				}
				if (this._flavor == 4)
				{
					this._cliCtxAttr = synchronizationAttribute;
				}
			}
			return result;
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x000EBF94 File Offset: 0x000EAF94
		[ComVisible(true)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (this._flavor == 1 || this._flavor == 2 || ctorMsg == null)
			{
				return;
			}
			if (this._cliCtxAttr != null)
			{
				ctorMsg.ContextProperties.Add(this._cliCtxAttr);
				this._cliCtxAttr = null;
				return;
			}
			ctorMsg.ContextProperties.Add(this);
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x000EBFE8 File Offset: 0x000EAFE8
		internal virtual void InitIfNecessary()
		{
			lock (this)
			{
				if (this._asyncWorkEvent == null)
				{
					this._asyncWorkEvent = new AutoResetEvent(false);
					this._workItemQueue = new Queue();
					this._asyncLcidList = new ArrayList();
					WaitOrTimerCallback callBack = new WaitOrTimerCallback(this.DispatcherCallBack);
					this._waitHandle = ThreadPool.RegisterWaitForSingleObject(this._asyncWorkEvent, callBack, null, SynchronizationAttribute._timeOut, false);
				}
			}
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x000EC068 File Offset: 0x000EB068
		private void DispatcherCallBack(object stateIgnored, bool ignored)
		{
			WorkItem work;
			lock (this._workItemQueue)
			{
				work = (WorkItem)this._workItemQueue.Dequeue();
			}
			this.ExecuteWorkItem(work);
			this.HandleWorkCompletion();
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x000EC0BC File Offset: 0x000EB0BC
		internal virtual void HandleThreadExit()
		{
			this.HandleWorkCompletion();
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x000EC0C4 File Offset: 0x000EB0C4
		internal virtual void HandleThreadReEntry()
		{
			WorkItem workItem = new WorkItem(null, null, null);
			workItem.SetDummy();
			this.HandleWorkRequest(workItem);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x000EC0E8 File Offset: 0x000EB0E8
		internal virtual void HandleWorkCompletion()
		{
			WorkItem workItem = null;
			bool flag = false;
			lock (this._workItemQueue)
			{
				if (this._workItemQueue.Count >= 1)
				{
					workItem = (WorkItem)this._workItemQueue.Peek();
					flag = true;
					workItem.SetSignaled();
				}
				else
				{
					this._locked = false;
				}
			}
			if (flag)
			{
				if (workItem.IsAsync())
				{
					this._asyncWorkEvent.Set();
					return;
				}
				lock (workItem)
				{
					Monitor.Pulse(workItem);
				}
			}
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x000EC18C File Offset: 0x000EB18C
		internal virtual void HandleWorkRequest(WorkItem work)
		{
			if (!this.IsNestedCall(work._reqMsg))
			{
				if (work.IsAsync())
				{
					bool flag = true;
					lock (this._workItemQueue)
					{
						work.SetWaiting();
						this._workItemQueue.Enqueue(work);
						if (!this._locked && this._workItemQueue.Count == 1)
						{
							work.SetSignaled();
							this._locked = true;
							this._asyncWorkEvent.Set();
						}
						return;
					}
				}
				lock (work)
				{
					bool flag;
					lock (this._workItemQueue)
					{
						if (!this._locked && this._workItemQueue.Count == 0)
						{
							this._locked = true;
							flag = false;
						}
						else
						{
							flag = true;
							work.SetWaiting();
							this._workItemQueue.Enqueue(work);
						}
					}
					if (flag)
					{
						Monitor.Wait(work);
						if (!work.IsDummy())
						{
							this.DispatcherCallBack(null, true);
							goto IL_122;
						}
						lock (this._workItemQueue)
						{
							this._workItemQueue.Dequeue();
							goto IL_122;
						}
					}
					if (!work.IsDummy())
					{
						work.SetSignaled();
						this.ExecuteWorkItem(work);
						this.HandleWorkCompletion();
					}
					IL_122:
					return;
				}
			}
			work.SetSignaled();
			work.Execute();
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x000EC304 File Offset: 0x000EB304
		internal void ExecuteWorkItem(WorkItem work)
		{
			work.Execute();
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x000EC30C File Offset: 0x000EB30C
		internal bool IsNestedCall(IMessage reqMsg)
		{
			bool flag = false;
			if (!this.IsReEntrant)
			{
				string syncCallOutLCID = this.SyncCallOutLCID;
				if (syncCallOutLCID != null)
				{
					LogicalCallContext logicalCallContext = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
					if (logicalCallContext != null && syncCallOutLCID.Equals(logicalCallContext.RemotingData.LogicalCallID))
					{
						flag = true;
					}
				}
				if (!flag && this.AsyncCallOutLCIDList.Count > 0)
				{
					LogicalCallContext logicalCallContext2 = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
					if (this.AsyncCallOutLCIDList.Contains(logicalCallContext2.RemotingData.LogicalCallID))
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x000EC3A0 File Offset: 0x000EB3A0
		public virtual IMessageSink GetServerContextSink(IMessageSink nextSink)
		{
			this.InitIfNecessary();
			return new SynchronizedServerContextSink(this, nextSink);
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x000EC3BC File Offset: 0x000EB3BC
		public virtual IMessageSink GetClientContextSink(IMessageSink nextSink)
		{
			this.InitIfNecessary();
			return new SynchronizedClientContextSink(this, nextSink);
		}

		// Token: 0x0400226F RID: 8815
		public const int NOT_SUPPORTED = 1;

		// Token: 0x04002270 RID: 8816
		public const int SUPPORTED = 2;

		// Token: 0x04002271 RID: 8817
		public const int REQUIRED = 4;

		// Token: 0x04002272 RID: 8818
		public const int REQUIRES_NEW = 8;

		// Token: 0x04002273 RID: 8819
		private const string PROPERTY_NAME = "Synchronization";

		// Token: 0x04002274 RID: 8820
		private static readonly int _timeOut = -1;

		// Token: 0x04002275 RID: 8821
		[NonSerialized]
		internal AutoResetEvent _asyncWorkEvent;

		// Token: 0x04002276 RID: 8822
		[NonSerialized]
		private RegisteredWaitHandle _waitHandle;

		// Token: 0x04002277 RID: 8823
		[NonSerialized]
		internal Queue _workItemQueue;

		// Token: 0x04002278 RID: 8824
		[NonSerialized]
		internal bool _locked;

		// Token: 0x04002279 RID: 8825
		internal bool _bReEntrant;

		// Token: 0x0400227A RID: 8826
		internal int _flavor;

		// Token: 0x0400227B RID: 8827
		[NonSerialized]
		private SynchronizationAttribute _cliCtxAttr;

		// Token: 0x0400227C RID: 8828
		[NonSerialized]
		private string _syncLcid;

		// Token: 0x0400227D RID: 8829
		[NonSerialized]
		private ArrayList _asyncLcidList;
	}
}
