using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000146 RID: 326
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
	public class SynchronizationContext
	{
		// Token: 0x060011D4 RID: 4564 RVA: 0x0003212C File Offset: 0x0003112C
		protected void SetWaitNotificationRequired()
		{
			RuntimeHelpers.PrepareDelegate(new WaitDelegate(this.Wait));
			this._props |= SynchronizationContextProperties.RequireWaitNotification;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0003214E File Offset: 0x0003114E
		public bool IsWaitNotificationRequired()
		{
			return (this._props & SynchronizationContextProperties.RequireWaitNotification) != SynchronizationContextProperties.None;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0003215E File Offset: 0x0003115E
		public virtual void Send(SendOrPostCallback d, object state)
		{
			d(state);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00032167 File Offset: 0x00031167
		public virtual void Post(SendOrPostCallback d, object state)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(d.Invoke), state);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0003217C File Offset: 0x0003117C
		public virtual void OperationStarted()
		{
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0003217E File Offset: 0x0003117E
		public virtual void OperationCompleted()
		{
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00032180 File Offset: 0x00031180
		[PrePrepareMethod]
		[CLSCompliant(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
		public virtual int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			if (waitHandles == null)
			{
				throw new ArgumentNullException("waitHandles");
			}
			return SynchronizationContext.WaitHelper(waitHandles, waitAll, millisecondsTimeout);
		}

		// Token: 0x060011DB RID: 4571
		[CLSCompliant(false)]
		[PrePrepareMethod]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected static extern int WaitHelper(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout);

		// Token: 0x060011DC RID: 4572 RVA: 0x00032198 File Offset: 0x00031198
		[SecurityPermission(SecurityAction.LinkDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
		public static void SetSynchronizationContext(SynchronizationContext syncContext)
		{
			SynchronizationContext.SetSynchronizationContext(syncContext, Thread.CurrentThread.ExecutionContext.SynchronizationContext);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000321B0 File Offset: 0x000311B0
		internal static SynchronizationContextSwitcher SetSynchronizationContext(SynchronizationContext syncContext, SynchronizationContext prevSyncContext)
		{
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			SynchronizationContextSwitcher result = default(SynchronizationContextSwitcher);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				result._ec = executionContext;
				result.savedSC = prevSyncContext;
				result.currSC = syncContext;
				executionContext.SynchronizationContext = syncContext;
			}
			catch
			{
				result.UndoNoThrow();
				throw;
			}
			return result;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x00032214 File Offset: 0x00031214
		public static SynchronizationContext Current
		{
			get
			{
				ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
				if (executionContextNoCreate != null)
				{
					return executionContextNoCreate.SynchronizationContext;
				}
				return null;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00032237 File Offset: 0x00031237
		public virtual SynchronizationContext CreateCopy()
		{
			return new SynchronizationContext();
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0003223E File Offset: 0x0003123E
		private static int InvokeWaitMethodHelper(SynchronizationContext syncContext, IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			return syncContext.Wait(waitHandles, waitAll, millisecondsTimeout);
		}

		// Token: 0x04000624 RID: 1572
		private SynchronizationContextProperties _props;
	}
}
