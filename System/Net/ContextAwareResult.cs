using System;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003A2 RID: 930
	internal class ContextAwareResult : LazyAsyncResult
	{
		// Token: 0x06001D27 RID: 7463 RVA: 0x0006F8ED File Offset: 0x0006E8ED
		internal ContextAwareResult(object myObject, object myState, AsyncCallback myCallBack) : this(false, false, myObject, myState, myCallBack)
		{
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x0006F8FA File Offset: 0x0006E8FA
		internal ContextAwareResult(bool captureIdentity, bool forceCaptureContext, object myObject, object myState, AsyncCallback myCallBack) : this(captureIdentity, forceCaptureContext, false, myObject, myState, myCallBack)
		{
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0006F90A File Offset: 0x0006E90A
		internal ContextAwareResult(bool captureIdentity, bool forceCaptureContext, bool threadSafeContextCopy, object myObject, object myState, AsyncCallback myCallBack) : base(myObject, myState, myCallBack)
		{
			if (forceCaptureContext)
			{
				this._Flags = ContextAwareResult.StateFlags.CaptureContext;
			}
			if (captureIdentity)
			{
				this._Flags |= ContextAwareResult.StateFlags.CaptureIdentity;
			}
			if (threadSafeContextCopy)
			{
				this._Flags |= ContextAwareResult.StateFlags.ThreadSafeContextCopy;
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x0006F944 File Offset: 0x0006E944
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private void SafeCaptureIdenity()
		{
			this._Wi = WindowsIdentity.GetCurrent();
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x0006F954 File Offset: 0x0006E954
		internal ExecutionContext ContextCopy
		{
			get
			{
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				ExecutionContext context = this._Context;
				if (context != null)
				{
					return context.CreateCopy();
				}
				if ((this._Flags & ContextAwareResult.StateFlags.PostBlockFinished) == ContextAwareResult.StateFlags.None)
				{
					lock (this._Lock)
					{
					}
				}
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				context = this._Context;
				if (context != null)
				{
					return context.CreateCopy();
				}
				return null;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x0006F9EC File Offset: 0x0006E9EC
		internal WindowsIdentity Identity
		{
			get
			{
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				if (this._Wi != null)
				{
					return this._Wi;
				}
				if ((this._Flags & ContextAwareResult.StateFlags.PostBlockFinished) == ContextAwareResult.StateFlags.None)
				{
					lock (this._Lock)
					{
					}
				}
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				return this._Wi;
			}
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0006FA70 File Offset: 0x0006EA70
		internal object StartPostingAsyncOp()
		{
			return this.StartPostingAsyncOp(true);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0006FA79 File Offset: 0x0006EA79
		internal object StartPostingAsyncOp(bool lockCapture)
		{
			this._Lock = (lockCapture ? new object() : null);
			this._Flags |= ContextAwareResult.StateFlags.PostBlockStarted;
			return this._Lock;
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0006FAA0 File Offset: 0x0006EAA0
		internal bool FinishPostingAsyncOp()
		{
			if ((this._Flags & (ContextAwareResult.StateFlags.PostBlockStarted | ContextAwareResult.StateFlags.PostBlockFinished)) != ContextAwareResult.StateFlags.PostBlockStarted)
			{
				return false;
			}
			this._Flags |= ContextAwareResult.StateFlags.PostBlockFinished;
			ExecutionContext executionContext = null;
			return this.CaptureOrComplete(ref executionContext, false);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x0006FAD8 File Offset: 0x0006EAD8
		internal bool FinishPostingAsyncOp(ref CallbackClosure closure)
		{
			if ((this._Flags & (ContextAwareResult.StateFlags.PostBlockStarted | ContextAwareResult.StateFlags.PostBlockFinished)) != ContextAwareResult.StateFlags.PostBlockStarted)
			{
				return false;
			}
			this._Flags |= ContextAwareResult.StateFlags.PostBlockFinished;
			CallbackClosure callbackClosure = closure;
			ExecutionContext executionContext;
			if (callbackClosure == null)
			{
				executionContext = null;
			}
			else if (!callbackClosure.IsCompatible(base.AsyncCallback))
			{
				closure = null;
				executionContext = null;
			}
			else
			{
				base.AsyncCallback = callbackClosure.AsyncCallback;
				executionContext = callbackClosure.Context;
			}
			bool result = this.CaptureOrComplete(ref executionContext, true);
			if (closure == null && base.AsyncCallback != null && executionContext != null)
			{
				closure = new CallbackClosure(executionContext, base.AsyncCallback);
			}
			return result;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0006FB5C File Offset: 0x0006EB5C
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this._Wi != null)
			{
				this._Wi.Dispose();
				this._Wi = null;
			}
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0006FB80 File Offset: 0x0006EB80
		private bool CaptureOrComplete(ref ExecutionContext cachedContext, bool returnContext)
		{
			bool flag = base.AsyncCallback != null || (this._Flags & ContextAwareResult.StateFlags.CaptureContext) != ContextAwareResult.StateFlags.None;
			if ((this._Flags & ContextAwareResult.StateFlags.CaptureIdentity) != ContextAwareResult.StateFlags.None && !base.InternalPeekCompleted && (!flag || SecurityContext.IsWindowsIdentityFlowSuppressed()))
			{
				this.SafeCaptureIdenity();
			}
			if (flag && !base.InternalPeekCompleted)
			{
				if (cachedContext == null)
				{
					cachedContext = ExecutionContext.Capture();
				}
				if (cachedContext != null)
				{
					if (!returnContext)
					{
						this._Context = cachedContext;
						cachedContext = null;
					}
					else
					{
						this._Context = cachedContext.CreateCopy();
					}
				}
			}
			else
			{
				cachedContext = null;
			}
			if (base.CompletedSynchronously)
			{
				base.Complete(IntPtr.Zero);
				return true;
			}
			return false;
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x0006FC20 File Offset: 0x0006EC20
		protected override void Complete(IntPtr userToken)
		{
			if ((this._Flags & ContextAwareResult.StateFlags.PostBlockStarted) == ContextAwareResult.StateFlags.None)
			{
				base.Complete(userToken);
				return;
			}
			if (base.CompletedSynchronously)
			{
				return;
			}
			ExecutionContext context = this._Context;
			if (userToken != IntPtr.Zero || context == null)
			{
				base.Complete(userToken);
				return;
			}
			ExecutionContext.Run(((this._Flags & ContextAwareResult.StateFlags.ThreadSafeContextCopy) != ContextAwareResult.StateFlags.None) ? context.CreateCopy() : context, new ContextCallback(this.CompleteCallback), null);
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x0006FC8E File Offset: 0x0006EC8E
		private void CompleteCallback(object state)
		{
			base.Complete(IntPtr.Zero);
		}

		// Token: 0x04001D64 RID: 7524
		private volatile ExecutionContext _Context;

		// Token: 0x04001D65 RID: 7525
		private object _Lock;

		// Token: 0x04001D66 RID: 7526
		private ContextAwareResult.StateFlags _Flags;

		// Token: 0x04001D67 RID: 7527
		private WindowsIdentity _Wi;

		// Token: 0x020003A3 RID: 931
		[Flags]
		private enum StateFlags
		{
			// Token: 0x04001D69 RID: 7529
			None = 0,
			// Token: 0x04001D6A RID: 7530
			CaptureIdentity = 1,
			// Token: 0x04001D6B RID: 7531
			CaptureContext = 2,
			// Token: 0x04001D6C RID: 7532
			ThreadSafeContextCopy = 4,
			// Token: 0x04001D6D RID: 7533
			PostBlockStarted = 8,
			// Token: 0x04001D6E RID: 7534
			PostBlockFinished = 16
		}
	}
}
