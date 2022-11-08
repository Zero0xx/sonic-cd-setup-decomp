using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000664 RID: 1636
	[DefaultEvent("Tick")]
	[SRDescription("DescriptionTimer")]
	[DefaultProperty("Interval")]
	[ToolboxItemFilter("System.Windows.Forms")]
	public class Timer : Component
	{
		// Token: 0x060055B6 RID: 21942 RVA: 0x00137F0C File Offset: 0x00136F0C
		public Timer()
		{
			this.interval = 100;
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x00137F27 File Offset: 0x00136F27
		public Timer(IContainer container) : this()
		{
			container.Add(this);
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x060055B8 RID: 21944 RVA: 0x00137F36 File Offset: 0x00136F36
		// (set) Token: 0x060055B9 RID: 21945 RVA: 0x00137F3E File Offset: 0x00136F3E
		[SRCategory("CatData")]
		[SRDescription("ControlTagDescr")]
		[Localizable(false)]
		[Bindable(true)]
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x1400032D RID: 813
		// (add) Token: 0x060055BA RID: 21946 RVA: 0x00137F47 File Offset: 0x00136F47
		// (remove) Token: 0x060055BB RID: 21947 RVA: 0x00137F60 File Offset: 0x00136F60
		[SRDescription("TimerTimerDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler Tick
		{
			add
			{
				this.onTimer = (EventHandler)Delegate.Combine(this.onTimer, value);
			}
			remove
			{
				this.onTimer = (EventHandler)Delegate.Remove(this.onTimer, value);
			}
		}

		// Token: 0x060055BC RID: 21948 RVA: 0x00137F79 File Offset: 0x00136F79
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.timerWindow != null)
				{
					this.timerWindow.StopTimer();
				}
				this.Enabled = false;
			}
			this.timerWindow = null;
			base.Dispose(disposing);
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x060055BD RID: 21949 RVA: 0x00137FA6 File Offset: 0x00136FA6
		// (set) Token: 0x060055BE RID: 21950 RVA: 0x00137FC4 File Offset: 0x00136FC4
		[SRDescription("TimerEnabledDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public virtual bool Enabled
		{
			get
			{
				if (this.timerWindow == null)
				{
					return this.enabled;
				}
				return this.timerWindow.IsTimerRunning;
			}
			set
			{
				lock (this.syncObj)
				{
					if (this.enabled != value)
					{
						this.enabled = value;
						if (!base.DesignMode)
						{
							if (value)
							{
								if (this.timerWindow == null)
								{
									this.timerWindow = new Timer.TimerNativeWindow(this);
								}
								this.timerRoot = GCHandle.Alloc(this);
								this.timerWindow.StartTimer(this.interval);
							}
							else
							{
								if (this.timerWindow != null)
								{
									this.timerWindow.StopTimer();
								}
								if (this.timerRoot.IsAllocated)
								{
									this.timerRoot.Free();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060055BF RID: 21951 RVA: 0x00138070 File Offset: 0x00137070
		// (set) Token: 0x060055C0 RID: 21952 RVA: 0x00138078 File Offset: 0x00137078
		[SRCategory("CatBehavior")]
		[SRDescription("TimerIntervalDescr")]
		[DefaultValue(100)]
		public int Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				lock (this.syncObj)
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException("Interval", SR.GetString("TimerInvalidInterval", new object[]
						{
							value,
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.interval != value)
					{
						this.interval = value;
						if (this.Enabled && !base.DesignMode && this.timerWindow != null)
						{
							this.timerWindow.RestartTimer(value);
						}
					}
				}
			}
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x0013811C File Offset: 0x0013711C
		protected virtual void OnTick(EventArgs e)
		{
			if (this.onTimer != null)
			{
				this.onTimer(this, e);
			}
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x00138133 File Offset: 0x00137133
		public void Start()
		{
			this.Enabled = true;
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x0013813C File Offset: 0x0013713C
		public void Stop()
		{
			this.Enabled = false;
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x00138148 File Offset: 0x00137148
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Interval: " + this.Interval.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0400372D RID: 14125
		private int interval;

		// Token: 0x0400372E RID: 14126
		private bool enabled;

		// Token: 0x0400372F RID: 14127
		private EventHandler onTimer;

		// Token: 0x04003730 RID: 14128
		private GCHandle timerRoot;

		// Token: 0x04003731 RID: 14129
		private Timer.TimerNativeWindow timerWindow;

		// Token: 0x04003732 RID: 14130
		private object userData;

		// Token: 0x04003733 RID: 14131
		private object syncObj = new object();

		// Token: 0x02000665 RID: 1637
		private class TimerNativeWindow : NativeWindow
		{
			// Token: 0x060055C5 RID: 21957 RVA: 0x0013817A File Offset: 0x0013717A
			internal TimerNativeWindow(Timer owner)
			{
				this._owner = owner;
			}

			// Token: 0x060055C6 RID: 21958 RVA: 0x0013818C File Offset: 0x0013718C
			~TimerNativeWindow()
			{
				this.StopTimer();
			}

			// Token: 0x170011C8 RID: 4552
			// (get) Token: 0x060055C7 RID: 21959 RVA: 0x001381B8 File Offset: 0x001371B8
			public bool IsTimerRunning
			{
				get
				{
					return this._timerID != 0 && base.Handle != IntPtr.Zero;
				}
			}

			// Token: 0x060055C8 RID: 21960 RVA: 0x001381D4 File Offset: 0x001371D4
			private bool EnsureHandle()
			{
				if (base.Handle == IntPtr.Zero)
				{
					CreateParams createParams = new CreateParams();
					createParams.Style = 0;
					createParams.ExStyle = 0;
					createParams.ClassStyle = 0;
					createParams.Caption = base.GetType().Name;
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						createParams.Parent = (IntPtr)NativeMethods.HWND_MESSAGE;
					}
					this.CreateHandle(createParams);
				}
				return base.Handle != IntPtr.Zero;
			}

			// Token: 0x060055C9 RID: 21961 RVA: 0x00138254 File Offset: 0x00137254
			private bool GetInvokeRequired(IntPtr hWnd)
			{
				if (hWnd != IntPtr.Zero)
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, hWnd), out num);
					int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
					return windowThreadProcessId != currentThreadId;
				}
				return false;
			}

			// Token: 0x060055CA RID: 21962 RVA: 0x0013828C File Offset: 0x0013728C
			public void RestartTimer(int newInterval)
			{
				this.StopTimer(false, IntPtr.Zero);
				this.StartTimer(newInterval);
			}

			// Token: 0x060055CB RID: 21963 RVA: 0x001382A4 File Offset: 0x001372A4
			public void StartTimer(int interval)
			{
				if (this._timerID == 0 && !this._stoppingTimer && this.EnsureHandle())
				{
					this._timerID = (int)SafeNativeMethods.SetTimer(new HandleRef(this, base.Handle), Timer.TimerNativeWindow.TimerID++, interval, IntPtr.Zero);
				}
			}

			// Token: 0x060055CC RID: 21964 RVA: 0x001382F8 File Offset: 0x001372F8
			public void StopTimer()
			{
				this.StopTimer(true, IntPtr.Zero);
			}

			// Token: 0x060055CD RID: 21965 RVA: 0x00138308 File Offset: 0x00137308
			public void StopTimer(bool destroyHwnd, IntPtr hWnd)
			{
				if (hWnd == IntPtr.Zero)
				{
					hWnd = base.Handle;
				}
				if (this.GetInvokeRequired(hWnd))
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, hWnd), 16, 0, 0);
					return;
				}
				lock (this)
				{
					if (!this._stoppingTimer && !(hWnd == IntPtr.Zero) && UnsafeNativeMethods.IsWindow(new HandleRef(this, hWnd)))
					{
						if (this._timerID != 0)
						{
							try
							{
								this._stoppingTimer = true;
								SafeNativeMethods.KillTimer(new HandleRef(this, hWnd), this._timerID);
							}
							finally
							{
								this._timerID = 0;
								this._stoppingTimer = false;
							}
						}
						if (destroyHwnd)
						{
							base.DestroyHandle();
						}
					}
				}
			}

			// Token: 0x060055CE RID: 21966 RVA: 0x001383D4 File Offset: 0x001373D4
			public override void DestroyHandle()
			{
				this.StopTimer(false, IntPtr.Zero);
				base.DestroyHandle();
			}

			// Token: 0x060055CF RID: 21967 RVA: 0x001383E8 File Offset: 0x001373E8
			protected override void OnThreadException(Exception e)
			{
				Application.OnThreadException(e);
			}

			// Token: 0x060055D0 RID: 21968 RVA: 0x001383F0 File Offset: 0x001373F0
			public override void ReleaseHandle()
			{
				this.StopTimer(false, IntPtr.Zero);
				base.ReleaseHandle();
			}

			// Token: 0x060055D1 RID: 21969 RVA: 0x00138404 File Offset: 0x00137404
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 275)
				{
					if ((int)m.WParam == this._timerID)
					{
						this._owner.OnTick(EventArgs.Empty);
						return;
					}
				}
				else if (m.Msg == 16)
				{
					this.StopTimer(true, m.HWnd);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x04003734 RID: 14132
			private Timer _owner;

			// Token: 0x04003735 RID: 14133
			private int _timerID;

			// Token: 0x04003736 RID: 14134
			private static int TimerID = 1;

			// Token: 0x04003737 RID: 14135
			private bool _stoppingTimer;
		}
	}
}
