using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000153 RID: 339
	[Serializable]
	public sealed class ExecutionContext : ISerializable
	{
		// Token: 0x0600123E RID: 4670 RVA: 0x00032C7F File Offset: 0x00031C7F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext()
		{
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00032C87 File Offset: 0x00031C87
		// (set) Token: 0x06001240 RID: 4672 RVA: 0x00032CA2 File Offset: 0x00031CA2
		internal LogicalCallContext LogicalCallContext
		{
			get
			{
				if (this._logicalCallContext == null)
				{
					this._logicalCallContext = new LogicalCallContext();
				}
				return this._logicalCallContext;
			}
			set
			{
				this._logicalCallContext = value;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00032CAB File Offset: 0x00031CAB
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x00032CC6 File Offset: 0x00031CC6
		internal IllogicalCallContext IllogicalCallContext
		{
			get
			{
				if (this._illogicalCallContext == null)
				{
					this._illogicalCallContext = new IllogicalCallContext();
				}
				return this._illogicalCallContext;
			}
			set
			{
				this._illogicalCallContext = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x00032CCF File Offset: 0x00031CCF
		internal Thread Thread
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._thread = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x00032CD8 File Offset: 0x00031CD8
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x00032CE0 File Offset: 0x00031CE0
		internal SynchronizationContext SynchronizationContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._syncContext;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._syncContext = value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x00032CE9 File Offset: 0x00031CE9
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x00032CF1 File Offset: 0x00031CF1
		internal HostExecutionContext HostExecutionContext
		{
			get
			{
				return this._hostExecutionContext;
			}
			set
			{
				this._hostExecutionContext = value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x00032CFA File Offset: 0x00031CFA
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x00032D02 File Offset: 0x00031D02
		internal SecurityContext SecurityContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._securityContext;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._securityContext = value;
				if (value != null)
				{
					this._securityContext.ExecutionContext = this;
				}
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00032D1C File Offset: 0x00031D1C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void Run(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			if (executionContext == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullContext"));
			}
			if (!executionContext.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			executionContext.isNewCapture = false;
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if ((executionContextNoCreate == null || executionContextNoCreate.IsDefaultFTContext()) && SecurityContext.CurrentlyInDefaultFTSecurityContext(executionContextNoCreate) && executionContext.IsDefaultFTContext())
			{
				callback(state);
				return;
			}
			ExecutionContext.RunInternal(executionContext, callback, state);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00032D94 File Offset: 0x00031D94
		internal static void RunInternal(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			if (ExecutionContext.cleanupCode == null)
			{
				ExecutionContext.tryCode = new RuntimeHelpers.TryCode(ExecutionContext.runTryCode);
				ExecutionContext.cleanupCode = new RuntimeHelpers.CleanupCode(ExecutionContext.runFinallyCode);
			}
			ExecutionContext.ExecutionContextRunData userData = new ExecutionContext.ExecutionContextRunData(executionContext, callback, state);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(ExecutionContext.tryCode, ExecutionContext.cleanupCode, userData);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00032DE4 File Offset: 0x00031DE4
		internal static void runTryCode(object userData)
		{
			ExecutionContext.ExecutionContextRunData executionContextRunData = (ExecutionContext.ExecutionContextRunData)userData;
			executionContextRunData.ecsw = ExecutionContext.SetExecutionContext(executionContextRunData.ec);
			executionContextRunData.callBack(executionContextRunData.state);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00032E1C File Offset: 0x00031E1C
		[PrePrepareMethod]
		internal static void runFinallyCode(object userData, bool exceptionThrown)
		{
			ExecutionContext.ExecutionContextRunData executionContextRunData = (ExecutionContext.ExecutionContextRunData)userData;
			executionContextRunData.ecsw.Undo();
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00032E3C File Offset: 0x00031E3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static ExecutionContextSwitcher SetExecutionContext(ExecutionContext executionContext)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			ExecutionContextSwitcher result = default(ExecutionContextSwitcher);
			result.thread = Thread.CurrentThread;
			result.prevEC = Thread.CurrentThread.GetExecutionContextNoCreate();
			result.currEC = executionContext;
			Thread.CurrentThread.SetExecutionContext(executionContext);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (executionContext != null)
				{
					SecurityContext securityContext = executionContext.SecurityContext;
					if (securityContext != null)
					{
						SecurityContext prevSecurityContext = (result.prevEC != null) ? result.prevEC.SecurityContext : null;
						result.scsw = SecurityContext.SetSecurityContext(securityContext, prevSecurityContext, ref stackCrawlMark);
					}
					else if (!SecurityContext.CurrentlyInDefaultFTSecurityContext(result.prevEC))
					{
						SecurityContext prevSecurityContext2 = (result.prevEC != null) ? result.prevEC.SecurityContext : null;
						result.scsw = SecurityContext.SetSecurityContext(SecurityContext.FullTrustSecurityContext, prevSecurityContext2, ref stackCrawlMark);
					}
					SynchronizationContext synchronizationContext = executionContext.SynchronizationContext;
					if (synchronizationContext != null)
					{
						SynchronizationContext prevSyncContext = (result.prevEC != null) ? result.prevEC.SynchronizationContext : null;
						result.sysw = SynchronizationContext.SetSynchronizationContext(synchronizationContext, prevSyncContext);
					}
					HostExecutionContext hostExecutionContext = executionContext.HostExecutionContext;
					if (hostExecutionContext != null)
					{
						result.hecsw = HostExecutionContextManager.SetHostExecutionContextInternal(hostExecutionContext);
					}
				}
			}
			catch
			{
				result.UndoNoThrow();
				throw;
			}
			return result;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00032F70 File Offset: 0x00031F70
		public ExecutionContext CreateCopy()
		{
			if (!this.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotCopyUsedContext"));
			}
			ExecutionContext executionContext = new ExecutionContext();
			executionContext.isNewCapture = true;
			executionContext._syncContext = ((this._syncContext == null) ? null : this._syncContext.CreateCopy());
			executionContext._hostExecutionContext = ((this._hostExecutionContext == null) ? null : this._hostExecutionContext.CreateCopy());
			if (this._securityContext != null)
			{
				executionContext._securityContext = this._securityContext.CreateCopy();
				executionContext._securityContext.ExecutionContext = executionContext;
			}
			if (this._logicalCallContext != null)
			{
				LogicalCallContext logicalCallContext = this.LogicalCallContext;
				executionContext.LogicalCallContext = (LogicalCallContext)logicalCallContext.Clone();
			}
			if (this._illogicalCallContext != null)
			{
				IllogicalCallContext illogicalCallContext = this.IllogicalCallContext;
				executionContext.IllogicalCallContext = (IllogicalCallContext)illogicalCallContext.Clone();
			}
			return executionContext;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00033040 File Offset: 0x00032040
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static AsyncFlowControl SuppressFlow()
		{
			if (ExecutionContext.IsFlowSuppressed())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotSupressFlowMultipleTimes"));
			}
			AsyncFlowControl result = default(AsyncFlowControl);
			result.Setup();
			return result;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00033074 File Offset: 0x00032074
		public static void RestoreFlow()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (executionContextNoCreate == null || !executionContextNoCreate.isFlowSuppressed)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotRestoreUnsupressedFlow"));
			}
			executionContextNoCreate.isFlowSuppressed = false;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x000330B0 File Offset: 0x000320B0
		public static bool IsFlowSuppressed()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			return executionContextNoCreate != null && executionContextNoCreate.isFlowSuppressed;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000330D4 File Offset: 0x000320D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ExecutionContext Capture()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ExecutionContext.Capture(ref stackCrawlMark);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000330EC File Offset: 0x000320EC
		internal static ExecutionContext Capture(ref StackCrawlMark stackMark)
		{
			if (ExecutionContext.IsFlowSuppressed())
			{
				return null;
			}
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			ExecutionContext executionContext = new ExecutionContext();
			executionContext.isNewCapture = true;
			executionContext.SecurityContext = SecurityContext.Capture(executionContextNoCreate, ref stackMark);
			if (executionContext.SecurityContext != null)
			{
				executionContext.SecurityContext.ExecutionContext = executionContext;
			}
			executionContext._hostExecutionContext = HostExecutionContextManager.CaptureHostExecutionContext();
			if (executionContextNoCreate != null)
			{
				executionContext._syncContext = ((executionContextNoCreate._syncContext == null) ? null : executionContextNoCreate._syncContext.CreateCopy());
				if (executionContextNoCreate._logicalCallContext != null)
				{
					LogicalCallContext logicalCallContext = executionContextNoCreate.LogicalCallContext;
					executionContext.LogicalCallContext = (LogicalCallContext)logicalCallContext.Clone();
				}
			}
			return executionContext;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00033186 File Offset: 0x00032186
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this._logicalCallContext != null)
			{
				info.AddValue("LogicalCallContext", this._logicalCallContext, typeof(LogicalCallContext));
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000331BC File Offset: 0x000321BC
		private ExecutionContext(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("LogicalCallContext"))
				{
					this._logicalCallContext = (LogicalCallContext)enumerator.Value;
				}
			}
			this.Thread = Thread.CurrentThread;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0003320E File Offset: 0x0003220E
		internal static void ClearSyncContext(ExecutionContext ec)
		{
			if (ec != null)
			{
				ec.SynchronizationContext = null;
			}
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0003321C File Offset: 0x0003221C
		internal bool IsDefaultFTContext()
		{
			return this._hostExecutionContext == null && this._syncContext == null && (this._securityContext == null || this._securityContext.IsDefaultFTSecurityContext()) && (this._logicalCallContext == null || !this._logicalCallContext.HasInfo) && (this._illogicalCallContext == null || !this._illogicalCallContext.HasUserData);
		}

		// Token: 0x04000649 RID: 1609
		private HostExecutionContext _hostExecutionContext;

		// Token: 0x0400064A RID: 1610
		private SynchronizationContext _syncContext;

		// Token: 0x0400064B RID: 1611
		private SecurityContext _securityContext;

		// Token: 0x0400064C RID: 1612
		private LogicalCallContext _logicalCallContext;

		// Token: 0x0400064D RID: 1613
		private IllogicalCallContext _illogicalCallContext;

		// Token: 0x0400064E RID: 1614
		private Thread _thread;

		// Token: 0x0400064F RID: 1615
		internal bool isNewCapture;

		// Token: 0x04000650 RID: 1616
		internal bool isFlowSuppressed;

		// Token: 0x04000651 RID: 1617
		internal static RuntimeHelpers.TryCode tryCode;

		// Token: 0x04000652 RID: 1618
		internal static RuntimeHelpers.CleanupCode cleanupCode;

		// Token: 0x02000154 RID: 340
		internal class ExecutionContextRunData
		{
			// Token: 0x06001259 RID: 4697 RVA: 0x00033283 File Offset: 0x00032283
			internal ExecutionContextRunData(ExecutionContext executionContext, ContextCallback cb, object state)
			{
				this.ec = executionContext;
				this.callBack = cb;
				this.state = state;
				this.ecsw = default(ExecutionContextSwitcher);
			}

			// Token: 0x04000653 RID: 1619
			internal ExecutionContext ec;

			// Token: 0x04000654 RID: 1620
			internal ContextCallback callBack;

			// Token: 0x04000655 RID: 1621
			internal object state;

			// Token: 0x04000656 RID: 1622
			internal ExecutionContextSwitcher ecsw;
		}
	}
}
