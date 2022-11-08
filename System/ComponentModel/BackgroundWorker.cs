using System;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x020000A0 RID: 160
	[SRDescription("BackgroundWorker_Desc")]
	[DefaultEvent("DoWork")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class BackgroundWorker : Component
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x0001767B File Offset: 0x0001667B
		public BackgroundWorker()
		{
			this.threadStart = new BackgroundWorker.WorkerThreadStartDelegate(this.WorkerThreadStart);
			this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);
			this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000176B9 File Offset: 0x000166B9
		private void AsyncOperationCompleted(object arg)
		{
			this.isRunning = false;
			this.cancellationPending = false;
			this.OnRunWorkerCompleted((RunWorkerCompletedEventArgs)arg);
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x000176D5 File Offset: 0x000166D5
		[SRDescription("BackgroundWorker_CancellationPending")]
		[Browsable(false)]
		public bool CancellationPending
		{
			get
			{
				return this.cancellationPending;
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000176DD File Offset: 0x000166DD
		public void CancelAsync()
		{
			if (!this.WorkerSupportsCancellation)
			{
				throw new InvalidOperationException(SR.GetString("BackgroundWorker_WorkerDoesntSupportCancellation"));
			}
			this.cancellationPending = true;
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060005B6 RID: 1462 RVA: 0x000176FE File Offset: 0x000166FE
		// (remove) Token: 0x060005B7 RID: 1463 RVA: 0x00017711 File Offset: 0x00016711
		[SRDescription("BackgroundWorker_DoWork")]
		[SRCategory("PropertyCategoryAsynchronous")]
		public event DoWorkEventHandler DoWork
		{
			add
			{
				base.Events.AddHandler(BackgroundWorker.doWorkKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(BackgroundWorker.doWorkKey, value);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00017724 File Offset: 0x00016724
		[SRDescription("BackgroundWorker_IsBusy")]
		[Browsable(false)]
		public bool IsBusy
		{
			get
			{
				return this.isRunning;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001772C File Offset: 0x0001672C
		protected virtual void OnDoWork(DoWorkEventArgs e)
		{
			DoWorkEventHandler doWorkEventHandler = (DoWorkEventHandler)base.Events[BackgroundWorker.doWorkKey];
			if (doWorkEventHandler != null)
			{
				doWorkEventHandler(this, e);
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001775C File Offset: 0x0001675C
		protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
		{
			RunWorkerCompletedEventHandler runWorkerCompletedEventHandler = (RunWorkerCompletedEventHandler)base.Events[BackgroundWorker.runWorkerCompletedKey];
			if (runWorkerCompletedEventHandler != null)
			{
				runWorkerCompletedEventHandler(this, e);
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001778C File Offset: 0x0001678C
		protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
		{
			ProgressChangedEventHandler progressChangedEventHandler = (ProgressChangedEventHandler)base.Events[BackgroundWorker.progressChangedKey];
			if (progressChangedEventHandler != null)
			{
				progressChangedEventHandler(this, e);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060005BC RID: 1468 RVA: 0x000177BA File Offset: 0x000167BA
		// (remove) Token: 0x060005BD RID: 1469 RVA: 0x000177CD File Offset: 0x000167CD
		[SRDescription("BackgroundWorker_ProgressChanged")]
		[SRCategory("PropertyCategoryAsynchronous")]
		public event ProgressChangedEventHandler ProgressChanged
		{
			add
			{
				base.Events.AddHandler(BackgroundWorker.progressChangedKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(BackgroundWorker.progressChangedKey, value);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000177E0 File Offset: 0x000167E0
		private void ProgressReporter(object arg)
		{
			this.OnProgressChanged((ProgressChangedEventArgs)arg);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000177EE File Offset: 0x000167EE
		public void ReportProgress(int percentProgress)
		{
			this.ReportProgress(percentProgress, null);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000177F8 File Offset: 0x000167F8
		public void ReportProgress(int percentProgress, object userState)
		{
			if (!this.WorkerReportsProgress)
			{
				throw new InvalidOperationException(SR.GetString("BackgroundWorker_WorkerDoesntReportProgress"));
			}
			ProgressChangedEventArgs progressChangedEventArgs = new ProgressChangedEventArgs(percentProgress, userState);
			if (this.asyncOperation != null)
			{
				this.asyncOperation.Post(this.progressReporter, progressChangedEventArgs);
				return;
			}
			this.progressReporter(progressChangedEventArgs);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001784C File Offset: 0x0001684C
		public void RunWorkerAsync()
		{
			this.RunWorkerAsync(null);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00017858 File Offset: 0x00016858
		public void RunWorkerAsync(object argument)
		{
			if (this.isRunning)
			{
				throw new InvalidOperationException(SR.GetString("BackgroundWorker_WorkerAlreadyRunning"));
			}
			this.isRunning = true;
			this.cancellationPending = false;
			this.asyncOperation = AsyncOperationManager.CreateOperation(null);
			this.threadStart.BeginInvoke(argument, null, null);
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060005C3 RID: 1475 RVA: 0x000178A6 File Offset: 0x000168A6
		// (remove) Token: 0x060005C4 RID: 1476 RVA: 0x000178B9 File Offset: 0x000168B9
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_RunWorkerCompleted")]
		public event RunWorkerCompletedEventHandler RunWorkerCompleted
		{
			add
			{
				base.Events.AddHandler(BackgroundWorker.runWorkerCompletedKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(BackgroundWorker.runWorkerCompletedKey, value);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000178CC File Offset: 0x000168CC
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x000178D4 File Offset: 0x000168D4
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_WorkerReportsProgress")]
		[DefaultValue(false)]
		public bool WorkerReportsProgress
		{
			get
			{
				return this.workerReportsProgress;
			}
			set
			{
				this.workerReportsProgress = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000178DD File Offset: 0x000168DD
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x000178E5 File Offset: 0x000168E5
		[SRDescription("BackgroundWorker_WorkerSupportsCancellation")]
		[DefaultValue(false)]
		[SRCategory("PropertyCategoryAsynchronous")]
		public bool WorkerSupportsCancellation
		{
			get
			{
				return this.canCancelWorker;
			}
			set
			{
				this.canCancelWorker = value;
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000178F0 File Offset: 0x000168F0
		private void WorkerThreadStart(object argument)
		{
			object result = null;
			Exception error = null;
			bool cancelled = false;
			try
			{
				DoWorkEventArgs doWorkEventArgs = new DoWorkEventArgs(argument);
				this.OnDoWork(doWorkEventArgs);
				if (doWorkEventArgs.Cancel)
				{
					cancelled = true;
				}
				else
				{
					result = doWorkEventArgs.Result;
				}
			}
			catch (Exception ex)
			{
				error = ex;
			}
			RunWorkerCompletedEventArgs arg = new RunWorkerCompletedEventArgs(result, error, cancelled);
			this.asyncOperation.PostOperationCompleted(this.operationCompleted, arg);
		}

		// Token: 0x040008E2 RID: 2274
		private static readonly object doWorkKey = new object();

		// Token: 0x040008E3 RID: 2275
		private static readonly object runWorkerCompletedKey = new object();

		// Token: 0x040008E4 RID: 2276
		private static readonly object progressChangedKey = new object();

		// Token: 0x040008E5 RID: 2277
		private bool canCancelWorker;

		// Token: 0x040008E6 RID: 2278
		private bool workerReportsProgress;

		// Token: 0x040008E7 RID: 2279
		private bool cancellationPending;

		// Token: 0x040008E8 RID: 2280
		private bool isRunning;

		// Token: 0x040008E9 RID: 2281
		private AsyncOperation asyncOperation;

		// Token: 0x040008EA RID: 2282
		private readonly BackgroundWorker.WorkerThreadStartDelegate threadStart;

		// Token: 0x040008EB RID: 2283
		private readonly SendOrPostCallback operationCompleted;

		// Token: 0x040008EC RID: 2284
		private readonly SendOrPostCallback progressReporter;

		// Token: 0x020000A1 RID: 161
		// (Invoke) Token: 0x060005CC RID: 1484
		private delegate void WorkerThreadStartDelegate(object argument);
	}
}
