using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x020004CD RID: 1229
	[ComVisible(true)]
	[Serializable]
	public class WindowsIdentity : IIdentity, ISerializable, IDeserializationCallback, IDisposable
	{
		// Token: 0x060030EE RID: 12526 RVA: 0x000A7C4F File Offset: 0x000A6C4F
		private WindowsIdentity()
		{
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000A7C69 File Offset: 0x000A6C69
		internal WindowsIdentity(SafeTokenHandle safeTokenHandle) : this(safeTokenHandle.DangerousGetHandle())
		{
			GC.KeepAlive(safeTokenHandle);
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000A7C7D File Offset: 0x000A6C7D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken) : this(userToken, null, -1)
		{
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000A7C88 File Offset: 0x000A6C88
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken, string type) : this(userToken, type, -1)
		{
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000A7C93 File Offset: 0x000A6C93
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType) : this(userToken, type, -1)
		{
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000A7C9E File Offset: 0x000A6C9E
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated) : this(userToken, type, isAuthenticated ? 1 : 0)
		{
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000A7CB0 File Offset: 0x000A6CB0
		private WindowsIdentity(IntPtr userToken, string authType, int isAuthenticated)
		{
			this.CreateFromToken(userToken);
			this.m_authType = authType;
			this.m_isAuthenticated = isAuthenticated;
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000A7CE0 File Offset: 0x000A6CE0
		private void CreateFromToken(IntPtr userToken)
		{
			if (userToken == IntPtr.Zero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TokenZero"));
			}
			uint num = (uint)Marshal.SizeOf(typeof(uint));
			Win32Native.GetTokenInformation(userToken, 8U, SafeLocalAllocHandle.InvalidHandle, 0U, out num);
			if (Marshal.GetLastWin32Error() == 6)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidImpersonationToken"));
			}
			if (!Win32Native.DuplicateHandle(Win32Native.GetCurrentProcess(), userToken, Win32Native.GetCurrentProcess(), ref this.m_safeTokenHandle, 0U, true, 2U))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000A7D6D File Offset: 0x000A6D6D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(string sUserPrincipalName) : this(sUserPrincipalName, null)
		{
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000A7D77 File Offset: 0x000A6D77
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(string sUserPrincipalName, string type)
		{
			this.m_safeTokenHandle = WindowsIdentity.KerbS4ULogon(sUserPrincipalName);
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000A7D9D File Offset: 0x000A6D9D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(SerializationInfo info, StreamingContext context) : this(info)
		{
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000A7DA8 File Offset: 0x000A6DA8
		private WindowsIdentity(SerializationInfo info)
		{
			IntPtr intPtr = (IntPtr)info.GetValue("m_userToken", typeof(IntPtr));
			if (intPtr != IntPtr.Zero)
			{
				this.CreateFromToken(intPtr);
			}
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000A7DFC File Offset: 0x000A6DFC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("m_userToken", this.m_safeTokenHandle.DangerousGetHandle());
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000A7E19 File Offset: 0x000A6E19
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000A7E1B File Offset: 0x000A6E1B
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public static WindowsIdentity GetCurrent()
		{
			return WindowsIdentity.GetCurrentInternal(TokenAccessLevels.MaximumAllowed, false);
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000A7E28 File Offset: 0x000A6E28
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public static WindowsIdentity GetCurrent(bool ifImpersonating)
		{
			return WindowsIdentity.GetCurrentInternal(TokenAccessLevels.MaximumAllowed, ifImpersonating);
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x000A7E35 File Offset: 0x000A6E35
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public static WindowsIdentity GetCurrent(TokenAccessLevels desiredAccess)
		{
			return WindowsIdentity.GetCurrentInternal(desiredAccess, false);
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x000A7E3E File Offset: 0x000A6E3E
		public static WindowsIdentity GetAnonymous()
		{
			return new WindowsIdentity();
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x000A7E48 File Offset: 0x000A6E48
		public string AuthenticationType
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return string.Empty;
				}
				if (this.m_authType != null)
				{
					return this.m_authType;
				}
				Win32Native.LUID logonAuthId = WindowsIdentity.GetLogonAuthId(this.m_safeTokenHandle);
				if (logonAuthId.LowPart == 998U)
				{
					return string.Empty;
				}
				SafeLsaReturnBufferHandle invalidHandle = SafeLsaReturnBufferHandle.InvalidHandle;
				int num = Win32Native.LsaGetLogonSessionData(ref logonAuthId, ref invalidHandle);
				if (num < 0)
				{
					throw WindowsIdentity.GetExceptionFromNtStatus(num);
				}
				string result = Marshal.PtrToStringUni(((Win32Native.SECURITY_LOGON_SESSION_DATA)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(Win32Native.SECURITY_LOGON_SESSION_DATA))).AuthenticationPackage.Buffer);
				invalidHandle.Dispose();
				return result;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000A7EE8 File Offset: 0x000A6EE8
		[ComVisible(false)]
		public TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return TokenImpersonationLevel.Anonymous;
				}
				uint num = 0U;
				SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenType, out num);
				int num2 = Marshal.ReadInt32(tokenInformation.DangerousGetHandle());
				if (num2 == 1)
				{
					return TokenImpersonationLevel.None;
				}
				SafeLocalAllocHandle tokenInformation2 = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenImpersonationLevel, out num);
				num2 = Marshal.ReadInt32(tokenInformation2.DangerousGetHandle());
				tokenInformation.Dispose();
				tokenInformation2.Dispose();
				return num2 + TokenImpersonationLevel.Anonymous;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x000A7F54 File Offset: 0x000A6F54
		public virtual bool IsAuthenticated
		{
			get
			{
				if (!WindowsIdentity.RunningOnWin2K)
				{
					return false;
				}
				if (this.m_isAuthenticated == -1)
				{
					WindowsPrincipal windowsPrincipal = new WindowsPrincipal(this);
					SecurityIdentifier sid = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[]
					{
						11
					});
					this.m_isAuthenticated = (windowsPrincipal.IsInRole(sid) ? 1 : 0);
				}
				return this.m_isAuthenticated == 1;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x000A7FAC File Offset: 0x000A6FAC
		public virtual bool IsGuest
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return false;
				}
				SecurityIdentifier right = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[]
				{
					32,
					501
				});
				return this.User == right;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000A7FF4 File Offset: 0x000A6FF4
		public virtual bool IsSystem
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return false;
				}
				SecurityIdentifier right = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[]
				{
					18
				});
				return this.User == right;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x000A8034 File Offset: 0x000A7034
		public virtual bool IsAnonymous
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return true;
				}
				SecurityIdentifier right = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[]
				{
					7
				});
				return this.User == right;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x000A8070 File Offset: 0x000A7070
		public virtual string Name
		{
			get
			{
				return this.GetName();
			}
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x000A8078 File Offset: 0x000A7078
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string GetName()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (this.m_safeTokenHandle.IsInvalid)
			{
				return string.Empty;
			}
			if (this.m_name == null)
			{
				using (WindowsIdentity.SafeImpersonate(SafeTokenHandle.InvalidHandle, null, ref stackCrawlMark))
				{
					NTAccount ntaccount = this.User.Translate(typeof(NTAccount)) as NTAccount;
					this.m_name = ntaccount.ToString();
				}
			}
			return this.m_name;
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x000A80FC File Offset: 0x000A70FC
		[ComVisible(false)]
		public SecurityIdentifier Owner
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return null;
				}
				if (this.m_owner == null)
				{
					uint num = 0U;
					SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenOwner, out num);
					this.m_owner = new SecurityIdentifier(Marshal.ReadIntPtr(tokenInformation.DangerousGetHandle()), true);
					tokenInformation.Dispose();
				}
				return this.m_owner;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000A815C File Offset: 0x000A715C
		[ComVisible(false)]
		public SecurityIdentifier User
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return null;
				}
				if (this.m_user == null)
				{
					uint num = 0U;
					SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenUser, out num);
					this.m_user = new SecurityIdentifier(Marshal.ReadIntPtr(tokenInformation.DangerousGetHandle()), true);
					tokenInformation.Dispose();
				}
				return this.m_user;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x000A81BC File Offset: 0x000A71BC
		public IdentityReferenceCollection Groups
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return null;
				}
				if (this.m_groups == null)
				{
					IdentityReferenceCollection identityReferenceCollection = new IdentityReferenceCollection();
					uint num = 0U;
					using (SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenGroups, out num))
					{
						int num2 = Marshal.ReadInt32(tokenInformation.DangerousGetHandle());
						IntPtr intPtr = new IntPtr((long)tokenInformation.DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(Win32Native.TOKEN_GROUPS), "Groups"));
						for (int i = 0; i < num2; i++)
						{
							Win32Native.SID_AND_ATTRIBUTES sid_AND_ATTRIBUTES = (Win32Native.SID_AND_ATTRIBUTES)Marshal.PtrToStructure(intPtr, typeof(Win32Native.SID_AND_ATTRIBUTES));
							uint num3 = 3221225492U;
							if ((sid_AND_ATTRIBUTES.Attributes & num3) == 4U)
							{
								identityReferenceCollection.Add(new SecurityIdentifier(sid_AND_ATTRIBUTES.Sid, true));
							}
							intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(Win32Native.SID_AND_ATTRIBUTES)));
						}
					}
					Interlocked.CompareExchange(ref this.m_groups, identityReferenceCollection, null);
				}
				return this.m_groups as IdentityReferenceCollection;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x000A82D8 File Offset: 0x000A72D8
		public virtual IntPtr Token
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeTokenHandle.DangerousGetHandle();
			}
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000A82E8 File Offset: 0x000A72E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual WindowsImpersonationContext Impersonate()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.Impersonate(ref stackCrawlMark);
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000A8300 File Offset: 0x000A7300
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static WindowsImpersonationContext Impersonate(IntPtr userToken)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (userToken == IntPtr.Zero)
			{
				return WindowsIdentity.SafeImpersonate(SafeTokenHandle.InvalidHandle, null, ref stackCrawlMark);
			}
			WindowsIdentity windowsIdentity = new WindowsIdentity(userToken);
			return windowsIdentity.Impersonate(ref stackCrawlMark);
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000A833C File Offset: 0x000A733C
		internal WindowsImpersonationContext Impersonate(ref StackCrawlMark stackMark)
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				return new WindowsImpersonationContext(SafeTokenHandle.InvalidHandle, WindowsIdentity.GetCurrentThreadWI(), false, null);
			}
			if (this.m_safeTokenHandle.IsInvalid)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AnonymousCannotImpersonate"));
			}
			return WindowsIdentity.SafeImpersonate(this.m_safeTokenHandle, this, ref stackMark);
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000A838C File Offset: 0x000A738C
		[ComVisible(false)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.m_safeTokenHandle != null && !this.m_safeTokenHandle.IsClosed)
			{
				this.m_safeTokenHandle.Dispose();
			}
			this.m_name = null;
			this.m_owner = null;
			this.m_user = null;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000A83C6 File Offset: 0x000A73C6
		[ComVisible(false)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003111 RID: 12561 RVA: 0x000A83CF File Offset: 0x000A73CF
		internal SafeTokenHandle TokenHandle
		{
			get
			{
				return this.m_safeTokenHandle;
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000A83D8 File Offset: 0x000A73D8
		internal static WindowsImpersonationContext SafeImpersonate(SafeTokenHandle userToken, WindowsIdentity wi, ref StackCrawlMark stackMark)
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				return new WindowsImpersonationContext(SafeTokenHandle.InvalidHandle, WindowsIdentity.GetCurrentThreadWI(), false, null);
			}
			int num = 0;
			bool isImpersonating;
			SafeTokenHandle currentToken = WindowsIdentity.GetCurrentToken(TokenAccessLevels.MaximumAllowed, false, out isImpersonating, out num);
			if (currentToken == null || currentToken.IsInvalid)
			{
				throw new SecurityException(Win32Native.GetMessage(num));
			}
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, true);
			if (securityObjectForFrame == null && SecurityManager._IsSecurityOn())
			{
				throw new SecurityException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
			}
			WindowsImpersonationContext windowsImpersonationContext = new WindowsImpersonationContext(currentToken, WindowsIdentity.GetCurrentThreadWI(), isImpersonating, securityObjectForFrame);
			if (userToken.IsInvalid)
			{
				num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
				WindowsIdentity.UpdateThreadWI(wi);
				securityObjectForFrame.SetTokenHandles(currentToken, (wi == null) ? null : wi.TokenHandle);
			}
			else
			{
				num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
				num = Win32.ImpersonateLoggedOnUser(userToken);
				if (num < 0)
				{
					windowsImpersonationContext.Undo();
					throw new SecurityException(Environment.GetResourceString("Argument_ImpersonateUser"));
				}
				WindowsIdentity.UpdateThreadWI(wi);
				securityObjectForFrame.SetTokenHandles(currentToken, (wi == null) ? null : wi.TokenHandle);
			}
			return windowsImpersonationContext;
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x000A84E4 File Offset: 0x000A74E4
		internal static WindowsIdentity GetCurrentThreadWI()
		{
			return SecurityContext.GetCurrentWI(Thread.CurrentThread.GetExecutionContextNoCreate());
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x000A84F8 File Offset: 0x000A74F8
		internal static void UpdateThreadWI(WindowsIdentity wi)
		{
			SecurityContext securityContext = SecurityContext.GetCurrentSecurityContextNoCreate();
			if (wi != null && securityContext == null)
			{
				securityContext = new SecurityContext();
				Thread.CurrentThread.ExecutionContext.SecurityContext = securityContext;
			}
			if (securityContext != null)
			{
				securityContext.WindowsIdentity = wi;
			}
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000A8534 File Offset: 0x000A7534
		internal static WindowsIdentity GetCurrentInternal(TokenAccessLevels desiredAccess, bool threadOnly)
		{
			WindowsIdentity windowsIdentity = null;
			if (!WindowsIdentity.RunningOnWin2K)
			{
				if (!threadOnly)
				{
					windowsIdentity = new WindowsIdentity();
					windowsIdentity.m_name = string.Empty;
				}
				return windowsIdentity;
			}
			int errorCode = 0;
			bool flag;
			SafeTokenHandle currentToken = WindowsIdentity.GetCurrentToken(desiredAccess, threadOnly, out flag, out errorCode);
			if (currentToken != null && !currentToken.IsInvalid)
			{
				windowsIdentity = new WindowsIdentity();
				windowsIdentity.m_safeTokenHandle.Dispose();
				windowsIdentity.m_safeTokenHandle = currentToken;
				return windowsIdentity;
			}
			if (threadOnly && !flag)
			{
				return windowsIdentity;
			}
			throw new SecurityException(Win32Native.GetMessage(errorCode));
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x000A85A8 File Offset: 0x000A75A8
		internal static bool RunningOnWin2K
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				if (WindowsIdentity.s_runningOnWin2K == -1)
				{
					Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
					bool versionEx = Win32Native.GetVersionEx(osversioninfo);
					WindowsIdentity.s_runningOnWin2K = ((versionEx && osversioninfo.PlatformId == 2 && osversioninfo.MajorVersion >= 5) ? 1 : 0);
				}
				return WindowsIdentity.s_runningOnWin2K == 1;
			}
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000A85F0 File Offset: 0x000A75F0
		private static int GetHRForWin32Error(int dwLastError)
		{
			if (((long)dwLastError & (long)((ulong)-2147483648)) == (long)((ulong)-2147483648))
			{
				return dwLastError;
			}
			return (dwLastError & 65535) | -2147024896;
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000A8614 File Offset: 0x000A7614
		private static Exception GetExceptionFromNtStatus(int status)
		{
			if (status == -1073741790)
			{
				return new UnauthorizedAccessException();
			}
			if (status == -1073741670 || status == -1073741801)
			{
				return new OutOfMemoryException();
			}
			int errorCode = Win32Native.LsaNtStatusToWinError(status);
			return new SecurityException(Win32Native.GetMessage(errorCode));
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000A8658 File Offset: 0x000A7658
		private static SafeTokenHandle GetCurrentToken(TokenAccessLevels desiredAccess, bool threadOnly, out bool isImpersonating, out int hr)
		{
			isImpersonating = true;
			SafeTokenHandle safeTokenHandle = WindowsIdentity.GetCurrentThreadToken(desiredAccess, out hr);
			if (safeTokenHandle == null && hr == WindowsIdentity.GetHRForWin32Error(1008))
			{
				isImpersonating = false;
				if (!threadOnly)
				{
					safeTokenHandle = WindowsIdentity.GetCurrentProcessToken(desiredAccess, out hr);
				}
			}
			return safeTokenHandle;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000A8690 File Offset: 0x000A7690
		private static SafeTokenHandle GetCurrentProcessToken(TokenAccessLevels desiredAccess, out int hr)
		{
			hr = 0;
			SafeTokenHandle invalidHandle = SafeTokenHandle.InvalidHandle;
			if (!Win32Native.OpenProcessToken(Win32Native.GetCurrentProcess(), desiredAccess, ref invalidHandle))
			{
				hr = WindowsIdentity.GetHRForWin32Error(Marshal.GetLastWin32Error());
			}
			return invalidHandle;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000A86C4 File Offset: 0x000A76C4
		internal static SafeTokenHandle GetCurrentThreadToken(TokenAccessLevels desiredAccess, out int hr)
		{
			SafeTokenHandle result;
			hr = Win32.OpenThreadToken(desiredAccess, WinSecurityContext.Both, out result);
			return result;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000A86E0 File Offset: 0x000A76E0
		private static Win32Native.LUID GetLogonAuthId(SafeTokenHandle safeTokenHandle)
		{
			uint num = 0U;
			SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(safeTokenHandle, TokenInformationClass.TokenStatistics, out num);
			Win32Native.TOKEN_STATISTICS token_STATISTICS = (Win32Native.TOKEN_STATISTICS)Marshal.PtrToStructure(tokenInformation.DangerousGetHandle(), typeof(Win32Native.TOKEN_STATISTICS));
			tokenInformation.Dispose();
			return token_STATISTICS.AuthenticationId;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000A8724 File Offset: 0x000A7724
		private static SafeLocalAllocHandle GetTokenInformation(SafeTokenHandle tokenHandle, TokenInformationClass tokenInformationClass, out uint dwLength)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			dwLength = (uint)Marshal.SizeOf(typeof(uint));
			bool tokenInformation = Win32Native.GetTokenInformation(tokenHandle, (uint)tokenInformationClass, safeLocalAllocHandle, 0U, out dwLength);
			int lastWin32Error = Marshal.GetLastWin32Error();
			int num = lastWin32Error;
			if (num == 6)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidImpersonationToken"));
			}
			if (num != 24 && num != 122)
			{
				throw new SecurityException(Win32Native.GetMessage(lastWin32Error));
			}
			IntPtr sizetdwBytes = new IntPtr((long)((ulong)dwLength));
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle = Win32Native.LocalAlloc(0, sizetdwBytes);
			if (safeLocalAllocHandle == null || safeLocalAllocHandle.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			if (!Win32Native.GetTokenInformation(tokenHandle, (uint)tokenInformationClass, safeLocalAllocHandle, dwLength, out dwLength))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000A87DC File Offset: 0x000A77DC
		private unsafe static SafeTokenHandle KerbS4ULogon(string upn)
		{
			byte[] array = new byte[]
			{
				67,
				76,
				82
			};
			IntPtr sizetdwBytes = new IntPtr((long)((ulong)(array.Length + 1)));
			SafeLocalAllocHandle safeLocalAllocHandle = Win32Native.LocalAlloc(64, sizetdwBytes);
			Marshal.Copy(array, 0, safeLocalAllocHandle.DangerousGetHandle(), array.Length);
			Win32Native.UNICODE_INTPTR_STRING unicode_INTPTR_STRING = new Win32Native.UNICODE_INTPTR_STRING(array.Length, array.Length + 1, safeLocalAllocHandle.DangerousGetHandle());
			SafeLsaLogonProcessHandle invalidHandle = SafeLsaLogonProcessHandle.InvalidHandle;
			Privilege privilege = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
				try
				{
					privilege = new Privilege("SeTcbPrivilege");
					privilege.Enable();
				}
				catch (PrivilegeNotHeldException)
				{
				}
				IntPtr zero = IntPtr.Zero;
				num = Win32Native.LsaRegisterLogonProcess(ref unicode_INTPTR_STRING, ref invalidHandle, ref zero);
				if (5 == Win32Native.LsaNtStatusToWinError(num))
				{
					num = Win32Native.LsaConnectUntrusted(ref invalidHandle);
				}
			}
			catch
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
				throw;
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
			if (num < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num);
			}
			byte[] array2 = new byte["Kerberos".Length + 1];
			Encoding.ASCII.GetBytes("Kerberos", 0, "Kerberos".Length, array2, 0);
			sizetdwBytes = new IntPtr((long)((ulong)array2.Length));
			SafeLocalAllocHandle safeLocalAllocHandle2 = Win32Native.LocalAlloc(0, sizetdwBytes);
			if (safeLocalAllocHandle2 == null || safeLocalAllocHandle2.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			Marshal.Copy(array2, 0, safeLocalAllocHandle2.DangerousGetHandle(), array2.Length);
			Win32Native.UNICODE_INTPTR_STRING unicode_INTPTR_STRING2 = new Win32Native.UNICODE_INTPTR_STRING("Kerberos".Length, "Kerberos".Length + 1, safeLocalAllocHandle2.DangerousGetHandle());
			uint authenticationPackage = 0U;
			num = Win32Native.LsaLookupAuthenticationPackage(invalidHandle, ref unicode_INTPTR_STRING2, ref authenticationPackage);
			if (num < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num);
			}
			Win32Native.TOKEN_SOURCE token_SOURCE = default(Win32Native.TOKEN_SOURCE);
			if (!Win32Native.AllocateLocallyUniqueId(ref token_SOURCE.SourceIdentifier))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			token_SOURCE.Name = new char[8];
			token_SOURCE.Name[0] = 'C';
			token_SOURCE.Name[1] = 'L';
			token_SOURCE.Name[2] = 'R';
			uint num2 = 0U;
			SafeLsaReturnBufferHandle invalidHandle2 = SafeLsaReturnBufferHandle.InvalidHandle;
			Win32Native.LUID luid = default(Win32Native.LUID);
			Win32Native.QUOTA_LIMITS quota_LIMITS = default(Win32Native.QUOTA_LIMITS);
			int num3 = 0;
			SafeTokenHandle invalidHandle3 = SafeTokenHandle.InvalidHandle;
			int num4 = Marshal.SizeOf(typeof(Win32Native.KERB_S4U_LOGON)) + 2 * (upn.Length + 1);
			byte[] array3 = new byte[num4];
			fixed (byte* ptr = array3)
			{
				byte[] array4 = new byte[2 * (upn.Length + 1)];
				Encoding.Unicode.GetBytes(upn, 0, upn.Length, array4, 0);
				Buffer.BlockCopy(array4, 0, array3, Marshal.SizeOf(typeof(Win32Native.KERB_S4U_LOGON)), array4.Length);
				Win32Native.KERB_S4U_LOGON* ptr2 = (Win32Native.KERB_S4U_LOGON*)ptr;
				ptr2->MessageType = 12U;
				ptr2->Flags = 0U;
				ptr2->ClientUpn.Length = (ushort)(2 * upn.Length);
				ptr2->ClientUpn.MaxLength = (ushort)(2 * (upn.Length + 1));
				ptr2->ClientUpn.Buffer = new IntPtr((void*)(ptr2 + 1));
				num = Win32Native.LsaLogonUser(invalidHandle, ref unicode_INTPTR_STRING, 3U, authenticationPackage, new IntPtr((void*)ptr), (uint)array3.Length, IntPtr.Zero, ref token_SOURCE, ref invalidHandle2, ref num2, ref luid, ref invalidHandle3, ref quota_LIMITS, ref num3);
			}
			if (num == -1073741714 && num3 < 0)
			{
				num = num3;
			}
			if (num < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num);
			}
			if (num3 < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num3);
			}
			invalidHandle2.Dispose();
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle2.Dispose();
			invalidHandle.Dispose();
			return invalidHandle3;
		}

		// Token: 0x040018A2 RID: 6306
		private string m_name;

		// Token: 0x040018A3 RID: 6307
		private SecurityIdentifier m_owner;

		// Token: 0x040018A4 RID: 6308
		private SecurityIdentifier m_user;

		// Token: 0x040018A5 RID: 6309
		private object m_groups;

		// Token: 0x040018A6 RID: 6310
		private SafeTokenHandle m_safeTokenHandle = SafeTokenHandle.InvalidHandle;

		// Token: 0x040018A7 RID: 6311
		private string m_authType;

		// Token: 0x040018A8 RID: 6312
		private int m_isAuthenticated = -1;

		// Token: 0x040018A9 RID: 6313
		private static int s_runningOnWin2K = -1;
	}
}
