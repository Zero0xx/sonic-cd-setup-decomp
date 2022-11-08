using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x0200073D RID: 1853
	public sealed class WindowsFormsSynchronizationContext : SynchronizationContext, IDisposable
	{
		// Token: 0x060062A3 RID: 25251 RVA: 0x001669F0 File Offset: 0x001659F0
		public WindowsFormsSynchronizationContext()
		{
			this.DestinationThread = Thread.CurrentThread;
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext != null)
			{
				this.controlToSendTo = threadContext.MarshalingControl;
			}
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x00166A23 File Offset: 0x00165A23
		private WindowsFormsSynchronizationContext(Control marshalingControl, Thread destinationThread)
		{
			this.controlToSendTo = marshalingControl;
			this.DestinationThread = destinationThread;
		}

		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x060062A5 RID: 25253 RVA: 0x00166A39 File Offset: 0x00165A39
		// (set) Token: 0x060062A6 RID: 25254 RVA: 0x00166A62 File Offset: 0x00165A62
		private Thread DestinationThread
		{
			get
			{
				if (this.destinationThreadRef != null && this.destinationThreadRef.IsAlive)
				{
					return this.destinationThreadRef.Target as Thread;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.destinationThreadRef = new WeakReference(value);
				}
			}
		}

		// Token: 0x060062A7 RID: 25255 RVA: 0x00166A73 File Offset: 0x00165A73
		public void Dispose()
		{
			if (this.controlToSendTo != null)
			{
				if (!this.controlToSendTo.IsDisposed)
				{
					this.controlToSendTo.Dispose();
				}
				this.controlToSendTo = null;
			}
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x00166A9C File Offset: 0x00165A9C
		public override void Send(SendOrPostCallback d, object state)
		{
			Thread destinationThread = this.DestinationThread;
			if (destinationThread == null || !destinationThread.IsAlive)
			{
				throw new InvalidAsynchronousStateException(SR.GetString("ThreadNoLongerValid"));
			}
			if (this.controlToSendTo != null)
			{
				this.controlToSendTo.Invoke(d, new object[]
				{
					state
				});
			}
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x00166AEC File Offset: 0x00165AEC
		public override void Post(SendOrPostCallback d, object state)
		{
			if (this.controlToSendTo != null)
			{
				this.controlToSendTo.BeginInvoke(d, new object[]
				{
					state
				});
			}
		}

		// Token: 0x060062AA RID: 25258 RVA: 0x00166B1A File Offset: 0x00165B1A
		public override SynchronizationContext CreateCopy()
		{
			return new WindowsFormsSynchronizationContext(this.controlToSendTo, this.DestinationThread);
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x060062AB RID: 25259 RVA: 0x00166B2D File Offset: 0x00165B2D
		// (set) Token: 0x060062AC RID: 25260 RVA: 0x00166B37 File Offset: 0x00165B37
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static bool AutoInstall
		{
			get
			{
				return !WindowsFormsSynchronizationContext.dontAutoInstall;
			}
			set
			{
				WindowsFormsSynchronizationContext.dontAutoInstall = !value;
			}
		}

		// Token: 0x060062AD RID: 25261 RVA: 0x00166B44 File Offset: 0x00165B44
		internal static void InstallIfNeeded()
		{
			if (!WindowsFormsSynchronizationContext.AutoInstall || WindowsFormsSynchronizationContext.inSyncContextInstallation)
			{
				return;
			}
			if (SynchronizationContext.Current == null)
			{
				WindowsFormsSynchronizationContext.previousSyncContext = null;
			}
			if (WindowsFormsSynchronizationContext.previousSyncContext != null)
			{
				return;
			}
			WindowsFormsSynchronizationContext.inSyncContextInstallation = true;
			try
			{
				SynchronizationContext synchronizationContext = AsyncOperationManager.SynchronizationContext;
				if (synchronizationContext == null || synchronizationContext.GetType() == typeof(SynchronizationContext))
				{
					WindowsFormsSynchronizationContext.previousSyncContext = synchronizationContext;
					new PermissionSet(PermissionState.Unrestricted).Assert();
					try
					{
						AsyncOperationManager.SynchronizationContext = new WindowsFormsSynchronizationContext();
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			finally
			{
				WindowsFormsSynchronizationContext.inSyncContextInstallation = false;
			}
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x00166BE0 File Offset: 0x00165BE0
		public static void Uninstall()
		{
			WindowsFormsSynchronizationContext.Uninstall(true);
		}

		// Token: 0x060062AF RID: 25263 RVA: 0x00166BE8 File Offset: 0x00165BE8
		internal static void Uninstall(bool turnOffAutoInstall)
		{
			if (WindowsFormsSynchronizationContext.AutoInstall)
			{
				WindowsFormsSynchronizationContext windowsFormsSynchronizationContext = AsyncOperationManager.SynchronizationContext as WindowsFormsSynchronizationContext;
				if (windowsFormsSynchronizationContext != null)
				{
					try
					{
						new PermissionSet(PermissionState.Unrestricted).Assert();
						if (WindowsFormsSynchronizationContext.previousSyncContext == null)
						{
							AsyncOperationManager.SynchronizationContext = new SynchronizationContext();
						}
						else
						{
							AsyncOperationManager.SynchronizationContext = WindowsFormsSynchronizationContext.previousSyncContext;
						}
					}
					finally
					{
						WindowsFormsSynchronizationContext.previousSyncContext = null;
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			if (turnOffAutoInstall)
			{
				WindowsFormsSynchronizationContext.AutoInstall = false;
			}
		}

		// Token: 0x04003B25 RID: 15141
		private Control controlToSendTo;

		// Token: 0x04003B26 RID: 15142
		private WeakReference destinationThreadRef;

		// Token: 0x04003B27 RID: 15143
		[ThreadStatic]
		private static bool dontAutoInstall;

		// Token: 0x04003B28 RID: 15144
		[ThreadStatic]
		private static bool inSyncContextInstallation;

		// Token: 0x04003B29 RID: 15145
		[ThreadStatic]
		private static SynchronizationContext previousSyncContext;
	}
}
