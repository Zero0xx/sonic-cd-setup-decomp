using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Timers
{
	// Token: 0x02000734 RID: 1844
	[DefaultEvent("Elapsed")]
	[DefaultProperty("Interval")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Timer : Component, ISupportInitialize
	{
		// Token: 0x06003832 RID: 14386 RVA: 0x000ED358 File Offset: 0x000EC358
		public Timer()
		{
			this.interval = 100.0;
			this.enabled = false;
			this.autoReset = true;
			this.initializing = false;
			this.delayedEnable = false;
			this.callback = new TimerCallback(this.MyTimerCallback);
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x000ED3A8 File Offset: 0x000EC3A8
		public Timer(double interval) : this()
		{
			if (interval <= 0.0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"interval",
					interval
				}));
			}
			int num = (int)Math.Ceiling(interval);
			if (num < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"interval",
					interval
				}));
			}
			this.interval = interval;
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x000ED42C File Offset: 0x000EC42C
		// (set) Token: 0x06003835 RID: 14389 RVA: 0x000ED434 File Offset: 0x000EC434
		[DefaultValue(true)]
		[TimersDescription("TimerAutoReset")]
		[Category("Behavior")]
		public bool AutoReset
		{
			get
			{
				return this.autoReset;
			}
			set
			{
				if (base.DesignMode)
				{
					this.autoReset = value;
					return;
				}
				if (this.autoReset != value)
				{
					this.autoReset = value;
					if (this.timer != null)
					{
						this.UpdateTimer();
					}
				}
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x000ED464 File Offset: 0x000EC464
		// (set) Token: 0x06003837 RID: 14391 RVA: 0x000ED46C File Offset: 0x000EC46C
		[TimersDescription("TimerEnabled")]
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (base.DesignMode)
				{
					this.delayedEnable = value;
					this.enabled = value;
					return;
				}
				if (this.initializing)
				{
					this.delayedEnable = value;
					return;
				}
				if (this.enabled != value)
				{
					if (!value)
					{
						if (this.timer != null)
						{
							this.cookie = null;
							this.timer.Dispose();
							this.timer = null;
						}
						this.enabled = value;
						return;
					}
					this.enabled = value;
					if (this.timer == null)
					{
						if (this.disposed)
						{
							throw new ObjectDisposedException(base.GetType().Name);
						}
						int num = (int)Math.Ceiling(this.interval);
						this.cookie = new object();
						this.timer = new Timer(this.callback, this.cookie, num, this.autoReset ? num : -1);
						return;
					}
					else
					{
						this.UpdateTimer();
					}
				}
			}
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x000ED544 File Offset: 0x000EC544
		private void UpdateTimer()
		{
			int num = (int)Math.Ceiling(this.interval);
			this.timer.Change(num, this.autoReset ? num : -1);
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003839 RID: 14393 RVA: 0x000ED577 File Offset: 0x000EC577
		// (set) Token: 0x0600383A RID: 14394 RVA: 0x000ED580 File Offset: 0x000EC580
		[RecommendedAsConfigurable(true)]
		[Category("Behavior")]
		[DefaultValue(100.0)]
		[TimersDescription("TimerInterval")]
		public double Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException(SR.GetString("TimerInvalidInterval", new object[]
					{
						value,
						0
					}));
				}
				this.interval = value;
				if (this.timer != null)
				{
					this.UpdateTimer();
				}
			}
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x0600383B RID: 14395 RVA: 0x000ED5D8 File Offset: 0x000EC5D8
		// (remove) Token: 0x0600383C RID: 14396 RVA: 0x000ED5F1 File Offset: 0x000EC5F1
		[Category("Behavior")]
		[TimersDescription("TimerIntervalElapsed")]
		public event ElapsedEventHandler Elapsed
		{
			add
			{
				this.onIntervalElapsed = (ElapsedEventHandler)Delegate.Combine(this.onIntervalElapsed, value);
			}
			remove
			{
				this.onIntervalElapsed = (ElapsedEventHandler)Delegate.Remove(this.onIntervalElapsed, value);
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x0600383E RID: 14398 RVA: 0x000ED622 File Offset: 0x000EC622
		// (set) Token: 0x0600383D RID: 14397 RVA: 0x000ED60A File Offset: 0x000EC60A
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (base.DesignMode)
				{
					this.enabled = true;
				}
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x0600383F RID: 14399 RVA: 0x000ED62C File Offset: 0x000EC62C
		// (set) Token: 0x06003840 RID: 14400 RVA: 0x000ED686 File Offset: 0x000EC686
		[TimersDescription("TimerSynchronizingObject")]
		[DefaultValue(null)]
		[Browsable(false)]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if (this.synchronizingObject == null && base.DesignMode)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						object rootComponent = designerHost.RootComponent;
						if (rootComponent != null && rootComponent is ISynchronizeInvoke)
						{
							this.synchronizingObject = (ISynchronizeInvoke)rootComponent;
						}
					}
				}
				return this.synchronizingObject;
			}
			set
			{
				this.synchronizingObject = value;
			}
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000ED68F File Offset: 0x000EC68F
		public void BeginInit()
		{
			this.Close();
			this.initializing = true;
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000ED69E File Offset: 0x000EC69E
		public void Close()
		{
			this.initializing = false;
			this.delayedEnable = false;
			this.enabled = false;
			if (this.timer != null)
			{
				this.timer.Dispose();
				this.timer = null;
			}
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000ED6CF File Offset: 0x000EC6CF
		protected override void Dispose(bool disposing)
		{
			this.Close();
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000ED6E5 File Offset: 0x000EC6E5
		public void EndInit()
		{
			this.initializing = false;
			this.Enabled = this.delayedEnable;
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000ED6FA File Offset: 0x000EC6FA
		public void Start()
		{
			this.Enabled = true;
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000ED703 File Offset: 0x000EC703
		public void Stop()
		{
			this.Enabled = false;
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000ED70C File Offset: 0x000EC70C
		private void MyTimerCallback(object state)
		{
			if (state != this.cookie)
			{
				return;
			}
			if (!this.autoReset)
			{
				this.enabled = false;
			}
			Timer.FILE_TIME file_TIME = default(Timer.FILE_TIME);
			Timer.GetSystemTimeAsFileTime(ref file_TIME);
			ElapsedEventArgs elapsedEventArgs = new ElapsedEventArgs(file_TIME.ftTimeLow, file_TIME.ftTimeHigh);
			try
			{
				ElapsedEventHandler elapsedEventHandler = this.onIntervalElapsed;
				if (elapsedEventHandler != null)
				{
					if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
					{
						this.SynchronizingObject.BeginInvoke(elapsedEventHandler, new object[]
						{
							this,
							elapsedEventArgs
						});
					}
					else
					{
						elapsedEventHandler(this, elapsedEventArgs);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003848 RID: 14408
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern void GetSystemTimeAsFileTime(ref Timer.FILE_TIME lpSystemTimeAsFileTime);

		// Token: 0x04003225 RID: 12837
		private double interval;

		// Token: 0x04003226 RID: 12838
		private bool enabled;

		// Token: 0x04003227 RID: 12839
		private bool initializing;

		// Token: 0x04003228 RID: 12840
		private bool delayedEnable;

		// Token: 0x04003229 RID: 12841
		private ElapsedEventHandler onIntervalElapsed;

		// Token: 0x0400322A RID: 12842
		private bool autoReset;

		// Token: 0x0400322B RID: 12843
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x0400322C RID: 12844
		private bool disposed;

		// Token: 0x0400322D RID: 12845
		private Timer timer;

		// Token: 0x0400322E RID: 12846
		private TimerCallback callback;

		// Token: 0x0400322F RID: 12847
		private object cookie;

		// Token: 0x02000735 RID: 1845
		internal struct FILE_TIME
		{
			// Token: 0x04003230 RID: 12848
			internal int ftTimeLow;

			// Token: 0x04003231 RID: 12849
			internal int ftTimeHigh;
		}
	}
}
