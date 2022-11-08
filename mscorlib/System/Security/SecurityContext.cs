using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security
{
	// Token: 0x0200068E RID: 1678
	public sealed class SecurityContext
	{
		// Token: 0x06003C89 RID: 15497 RVA: 0x000CF188 File Offset: 0x000CE188
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal SecurityContext()
		{
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x000CF190 File Offset: 0x000CE190
		internal static SecurityContext FullTrustSecurityContext
		{
			get
			{
				if (SecurityContext._fullTrustSC == null)
				{
					SecurityContext._fullTrustSC = SecurityContext.CreateFullTrustSecurityContext();
				}
				return SecurityContext._fullTrustSC;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (set) Token: 0x06003C8B RID: 15499 RVA: 0x000CF1A8 File Offset: 0x000CE1A8
		internal ExecutionContext ExecutionContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._executionContext = value;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003C8C RID: 15500 RVA: 0x000CF1B1 File Offset: 0x000CE1B1
		// (set) Token: 0x06003C8D RID: 15501 RVA: 0x000CF1B9 File Offset: 0x000CE1B9
		internal WindowsIdentity WindowsIdentity
		{
			get
			{
				return this._windowsIdentity;
			}
			set
			{
				this._windowsIdentity = value;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003C8E RID: 15502 RVA: 0x000CF1C2 File Offset: 0x000CE1C2
		// (set) Token: 0x06003C8F RID: 15503 RVA: 0x000CF1CA File Offset: 0x000CE1CA
		internal CompressedStack CompressedStack
		{
			get
			{
				return this._compressedStack;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._compressedStack = value;
			}
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x000CF1D3 File Offset: 0x000CE1D3
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static AsyncFlowControl SuppressFlow()
		{
			return SecurityContext.SuppressFlow(SecurityContextDisableFlow.All);
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x000CF1DF File Offset: 0x000CE1DF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static AsyncFlowControl SuppressFlowWindowsIdentity()
		{
			return SecurityContext.SuppressFlow(SecurityContextDisableFlow.WI);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x000CF1E8 File Offset: 0x000CE1E8
		internal static AsyncFlowControl SuppressFlow(SecurityContextDisableFlow flags)
		{
			if (SecurityContext.IsFlowSuppressed(flags))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotSupressFlowMultipleTimes"));
			}
			if (Thread.CurrentThread.ExecutionContext.SecurityContext == null)
			{
				Thread.CurrentThread.ExecutionContext.SecurityContext = new SecurityContext();
			}
			AsyncFlowControl result = default(AsyncFlowControl);
			result.Setup(flags);
			return result;
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x000CF244 File Offset: 0x000CE244
		public static void RestoreFlow()
		{
			SecurityContext currentSecurityContextNoCreate = SecurityContext.GetCurrentSecurityContextNoCreate();
			if (currentSecurityContextNoCreate == null || currentSecurityContextNoCreate._disableFlow == SecurityContextDisableFlow.Nothing)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotRestoreUnsupressedFlow"));
			}
			currentSecurityContextNoCreate._disableFlow = SecurityContextDisableFlow.Nothing;
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x000CF279 File Offset: 0x000CE279
		public static bool IsFlowSuppressed()
		{
			return SecurityContext.IsFlowSuppressed(SecurityContextDisableFlow.All);
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x000CF285 File Offset: 0x000CE285
		public static bool IsWindowsIdentityFlowSuppressed()
		{
			return SecurityContext._LegacyImpersonationPolicy || SecurityContext.IsFlowSuppressed(SecurityContextDisableFlow.WI);
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x000CF298 File Offset: 0x000CE298
		internal static bool IsFlowSuppressed(SecurityContextDisableFlow flags)
		{
			SecurityContext currentSecurityContextNoCreate = SecurityContext.GetCurrentSecurityContextNoCreate();
			return currentSecurityContextNoCreate != null && (currentSecurityContextNoCreate._disableFlow & flags) == flags;
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x000CF2BC File Offset: 0x000CE2BC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void Run(SecurityContext securityContext, ContextCallback callback, object state)
		{
			if (securityContext == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullContext"));
			}
			if (!securityContext.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			securityContext.isNewCapture = false;
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (SecurityContext.CurrentlyInDefaultFTSecurityContext(executionContextNoCreate) && securityContext.IsDefaultFTSecurityContext())
			{
				callback(state);
				return;
			}
			SecurityContext.RunInternal(securityContext, callback, state);
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000CF328 File Offset: 0x000CE328
		internal static void RunInternal(SecurityContext securityContext, ContextCallback callBack, object state)
		{
			if (SecurityContext.cleanupCode == null)
			{
				SecurityContext.tryCode = new RuntimeHelpers.TryCode(SecurityContext.runTryCode);
				SecurityContext.cleanupCode = new RuntimeHelpers.CleanupCode(SecurityContext.runFinallyCode);
			}
			SecurityContext.SecurityContextRunData userData = new SecurityContext.SecurityContextRunData(securityContext, callBack, state);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(SecurityContext.tryCode, SecurityContext.cleanupCode, userData);
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x000CF378 File Offset: 0x000CE378
		internal static void runTryCode(object userData)
		{
			SecurityContext.SecurityContextRunData securityContextRunData = (SecurityContext.SecurityContextRunData)userData;
			securityContextRunData.scsw = SecurityContext.SetSecurityContext(securityContextRunData.sc, Thread.CurrentThread.ExecutionContext.SecurityContext);
			securityContextRunData.callBack(securityContextRunData.state);
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x000CF3C0 File Offset: 0x000CE3C0
		[PrePrepareMethod]
		internal static void runFinallyCode(object userData, bool exceptionThrown)
		{
			SecurityContext.SecurityContextRunData securityContextRunData = (SecurityContext.SecurityContextRunData)userData;
			securityContextRunData.scsw.Undo();
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x000CF3E0 File Offset: 0x000CE3E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static SecurityContextSwitcher SetSecurityContext(SecurityContext sc, SecurityContext prevSecurityContext)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return SecurityContext.SetSecurityContext(sc, prevSecurityContext, ref stackCrawlMark);
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x000CF3F8 File Offset: 0x000CE3F8
		internal static SecurityContextSwitcher SetSecurityContext(SecurityContext sc, SecurityContext prevSecurityContext, ref StackCrawlMark stackMark)
		{
			SecurityContextDisableFlow disableFlow = sc._disableFlow;
			sc._disableFlow = SecurityContextDisableFlow.Nothing;
			SecurityContextSwitcher result = default(SecurityContextSwitcher);
			result.currSC = sc;
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			result.currEC = executionContext;
			result.prevSC = prevSecurityContext;
			executionContext.SecurityContext = sc;
			if (sc != null)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					result.wic = null;
					if (!SecurityContext._LegacyImpersonationPolicy)
					{
						if (sc.WindowsIdentity != null)
						{
							result.wic = sc.WindowsIdentity.Impersonate(ref stackMark);
						}
						else if ((disableFlow & SecurityContextDisableFlow.WI) == SecurityContextDisableFlow.Nothing && prevSecurityContext != null && prevSecurityContext.WindowsIdentity != null)
						{
							result.wic = WindowsIdentity.SafeImpersonate(SafeTokenHandle.InvalidHandle, null, ref stackMark);
						}
					}
					result.cssw = CompressedStack.SetCompressedStack(sc.CompressedStack, (prevSecurityContext != null) ? prevSecurityContext.CompressedStack : null);
				}
				catch
				{
					result.UndoNoThrow();
					throw;
				}
			}
			return result;
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x000CF4D8 File Offset: 0x000CE4D8
		public SecurityContext CreateCopy()
		{
			if (!this.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			SecurityContext securityContext = new SecurityContext();
			securityContext.isNewCapture = true;
			securityContext._disableFlow = this._disableFlow;
			securityContext._windowsIdentity = this.WindowsIdentity;
			if (this._compressedStack != null)
			{
				securityContext._compressedStack = this._compressedStack.CreateCopy();
			}
			return securityContext;
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x000CF53C File Offset: 0x000CE53C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static SecurityContext Capture()
		{
			if (SecurityContext.IsFlowSuppressed() || !SecurityManager._IsSecurityOn())
			{
				return null;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityContext securityContext = SecurityContext.Capture(Thread.CurrentThread.GetExecutionContextNoCreate(), ref stackCrawlMark);
			if (securityContext == null)
			{
				securityContext = SecurityContext.CreateFullTrustSecurityContext();
			}
			return securityContext;
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x000CF578 File Offset: 0x000CE578
		internal static SecurityContext Capture(ExecutionContext currThreadEC, ref StackCrawlMark stackMark)
		{
			if (SecurityContext.IsFlowSuppressed() || !SecurityManager._IsSecurityOn())
			{
				return null;
			}
			if (SecurityContext.CurrentlyInDefaultFTSecurityContext(currThreadEC))
			{
				return null;
			}
			SecurityContext securityContext = new SecurityContext();
			securityContext.isNewCapture = true;
			if (!SecurityContext.IsWindowsIdentityFlowSuppressed())
			{
				securityContext._windowsIdentity = SecurityContext.GetCurrentWI(currThreadEC);
			}
			else
			{
				securityContext._disableFlow = SecurityContextDisableFlow.WI;
			}
			securityContext.CompressedStack = CompressedStack.GetCompressedStack(ref stackMark);
			return securityContext;
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x000CF5D8 File Offset: 0x000CE5D8
		internal static SecurityContext CreateFullTrustSecurityContext()
		{
			SecurityContext securityContext = new SecurityContext();
			securityContext.isNewCapture = true;
			if (SecurityContext.IsWindowsIdentityFlowSuppressed())
			{
				securityContext._disableFlow = SecurityContextDisableFlow.WI;
			}
			securityContext.CompressedStack = new CompressedStack(null);
			return securityContext;
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x000CF610 File Offset: 0x000CE610
		internal static SecurityContext GetCurrentSecurityContextNoCreate()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (executionContextNoCreate != null)
			{
				return executionContextNoCreate.SecurityContext;
			}
			return null;
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x000CF634 File Offset: 0x000CE634
		internal static WindowsIdentity GetCurrentWI(ExecutionContext threadEC)
		{
			if (SecurityContext._alwaysFlowImpersonationPolicy)
			{
				return WindowsIdentity.GetCurrentInternal(TokenAccessLevels.MaximumAllowed, true);
			}
			SecurityContext securityContext = (threadEC == null) ? null : threadEC.SecurityContext;
			if (securityContext != null)
			{
				return securityContext.WindowsIdentity;
			}
			return null;
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x000CF66C File Offset: 0x000CE66C
		internal bool IsDefaultFTSecurityContext()
		{
			return this.WindowsIdentity == null && (this.CompressedStack == null || this.CompressedStack.CompressedStackHandle == null);
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x000CF690 File Offset: 0x000CE690
		internal static bool CurrentlyInDefaultFTSecurityContext(ExecutionContext threadEC)
		{
			return SecurityContext.IsDefaultThreadSecurityInfo() && SecurityContext.GetCurrentWI(threadEC) == null;
		}

		// Token: 0x06003CA5 RID: 15525
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern WindowsImpersonationFlowMode GetImpersonationFlowMode();

		// Token: 0x06003CA6 RID: 15526
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsDefaultThreadSecurityInfo();

		// Token: 0x04001F1B RID: 7963
		private static bool _LegacyImpersonationPolicy = SecurityContext.GetImpersonationFlowMode() == WindowsImpersonationFlowMode.IMP_NOFLOW;

		// Token: 0x04001F1C RID: 7964
		private static bool _alwaysFlowImpersonationPolicy = SecurityContext.GetImpersonationFlowMode() == WindowsImpersonationFlowMode.IMP_ALWAYSFLOW;

		// Token: 0x04001F1D RID: 7965
		private ExecutionContext _executionContext;

		// Token: 0x04001F1E RID: 7966
		private WindowsIdentity _windowsIdentity;

		// Token: 0x04001F1F RID: 7967
		private CompressedStack _compressedStack;

		// Token: 0x04001F20 RID: 7968
		private static SecurityContext _fullTrustSC;

		// Token: 0x04001F21 RID: 7969
		internal bool isNewCapture;

		// Token: 0x04001F22 RID: 7970
		internal SecurityContextDisableFlow _disableFlow;

		// Token: 0x04001F23 RID: 7971
		internal static RuntimeHelpers.TryCode tryCode;

		// Token: 0x04001F24 RID: 7972
		internal static RuntimeHelpers.CleanupCode cleanupCode;

		// Token: 0x0200068F RID: 1679
		internal class SecurityContextRunData
		{
			// Token: 0x06003CA8 RID: 15528 RVA: 0x000CF6C0 File Offset: 0x000CE6C0
			internal SecurityContextRunData(SecurityContext securityContext, ContextCallback cb, object state)
			{
				this.sc = securityContext;
				this.callBack = cb;
				this.state = state;
				this.scsw = default(SecurityContextSwitcher);
			}

			// Token: 0x04001F25 RID: 7973
			internal SecurityContext sc;

			// Token: 0x04001F26 RID: 7974
			internal ContextCallback callBack;

			// Token: 0x04001F27 RID: 7975
			internal object state;

			// Token: 0x04001F28 RID: 7976
			internal SecurityContextSwitcher scsw;
		}
	}
}
