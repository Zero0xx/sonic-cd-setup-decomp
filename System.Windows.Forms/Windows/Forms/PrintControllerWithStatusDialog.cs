using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x0200079E RID: 1950
	public class PrintControllerWithStatusDialog : PrintController
	{
		// Token: 0x06006639 RID: 26169 RVA: 0x001770F8 File Offset: 0x001760F8
		public PrintControllerWithStatusDialog(PrintController underlyingController) : this(underlyingController, SR.GetString("PrintControllerWithStatusDialog_DialogTitlePrint"))
		{
		}

		// Token: 0x0600663A RID: 26170 RVA: 0x0017710B File Offset: 0x0017610B
		public PrintControllerWithStatusDialog(PrintController underlyingController, string dialogTitle)
		{
			this.underlyingController = underlyingController;
			this.dialogTitle = dialogTitle;
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x0600663B RID: 26171 RVA: 0x00177121 File Offset: 0x00176121
		public override bool IsPreview
		{
			get
			{
				return this.underlyingController != null && this.underlyingController.IsPreview;
			}
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x00177138 File Offset: 0x00176138
		public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			base.OnStartPrint(document, e);
			this.document = document;
			this.pageNumber = 1;
			if (SystemInformation.UserInteractive)
			{
				this.backgroundThread = new PrintControllerWithStatusDialog.BackgroundThread(this);
			}
			try
			{
				this.underlyingController.OnStartPrint(document, e);
			}
			catch
			{
				if (this.backgroundThread != null)
				{
					this.backgroundThread.Stop();
				}
				throw;
			}
			finally
			{
				if (this.backgroundThread != null && this.backgroundThread.canceled)
				{
					e.Cancel = true;
				}
			}
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x001771D0 File Offset: 0x001761D0
		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			base.OnStartPage(document, e);
			if (this.backgroundThread != null)
			{
				this.backgroundThread.UpdateLabel();
			}
			Graphics result = this.underlyingController.OnStartPage(document, e);
			if (this.backgroundThread != null && this.backgroundThread.canceled)
			{
				e.Cancel = true;
			}
			return result;
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x00177224 File Offset: 0x00176224
		public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.underlyingController.OnEndPage(document, e);
			if (this.backgroundThread != null && this.backgroundThread.canceled)
			{
				e.Cancel = true;
			}
			this.pageNumber++;
			base.OnEndPage(document, e);
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x00177270 File Offset: 0x00176270
		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this.underlyingController.OnEndPrint(document, e);
			if (this.backgroundThread != null && this.backgroundThread.canceled)
			{
				e.Cancel = true;
			}
			if (this.backgroundThread != null)
			{
				this.backgroundThread.Stop();
			}
			base.OnEndPrint(document, e);
		}

		// Token: 0x04003CCD RID: 15565
		private PrintController underlyingController;

		// Token: 0x04003CCE RID: 15566
		private PrintDocument document;

		// Token: 0x04003CCF RID: 15567
		private PrintControllerWithStatusDialog.BackgroundThread backgroundThread;

		// Token: 0x04003CD0 RID: 15568
		private int pageNumber;

		// Token: 0x04003CD1 RID: 15569
		private string dialogTitle;

		// Token: 0x0200079F RID: 1951
		private class BackgroundThread
		{
			// Token: 0x06006640 RID: 26176 RVA: 0x001772C1 File Offset: 0x001762C1
			internal BackgroundThread(PrintControllerWithStatusDialog parent)
			{
				this.parent = parent;
				this.thread = new Thread(new ThreadStart(this.Run));
				this.thread.SetApartmentState(ApartmentState.STA);
				this.thread.Start();
			}

			// Token: 0x06006641 RID: 26177 RVA: 0x00177300 File Offset: 0x00176300
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[UIPermission(SecurityAction.Assert, Window = UIPermissionWindow.AllWindows)]
			private void Run()
			{
				try
				{
					lock (this)
					{
						if (this.alreadyStopped)
						{
							return;
						}
						this.dialog = new PrintControllerWithStatusDialog.StatusDialog(this, this.parent.dialogTitle);
						this.ThreadUnsafeUpdateLabel();
						this.dialog.Visible = true;
					}
					if (!this.alreadyStopped)
					{
						Application.Run(this.dialog);
					}
				}
				finally
				{
					lock (this)
					{
						if (this.dialog != null)
						{
							this.dialog.Dispose();
							this.dialog = null;
						}
					}
				}
			}

			// Token: 0x06006642 RID: 26178 RVA: 0x001773BC File Offset: 0x001763BC
			internal void Stop()
			{
				lock (this)
				{
					if (this.dialog != null && this.dialog.IsHandleCreated)
					{
						this.dialog.BeginInvoke(new MethodInvoker(this.dialog.Close));
					}
					else
					{
						this.alreadyStopped = true;
					}
				}
			}

			// Token: 0x06006643 RID: 26179 RVA: 0x00177428 File Offset: 0x00176428
			private void ThreadUnsafeUpdateLabel()
			{
				this.dialog.label1.Text = SR.GetString("PrintControllerWithStatusDialog_NowPrinting", new object[]
				{
					this.parent.pageNumber,
					this.parent.document.DocumentName
				});
			}

			// Token: 0x06006644 RID: 26180 RVA: 0x0017747D File Offset: 0x0017647D
			internal void UpdateLabel()
			{
				if (this.dialog != null && this.dialog.IsHandleCreated)
				{
					this.dialog.BeginInvoke(new MethodInvoker(this.ThreadUnsafeUpdateLabel));
				}
			}

			// Token: 0x04003CD2 RID: 15570
			private PrintControllerWithStatusDialog parent;

			// Token: 0x04003CD3 RID: 15571
			private PrintControllerWithStatusDialog.StatusDialog dialog;

			// Token: 0x04003CD4 RID: 15572
			private Thread thread;

			// Token: 0x04003CD5 RID: 15573
			internal bool canceled;

			// Token: 0x04003CD6 RID: 15574
			private bool alreadyStopped;
		}

		// Token: 0x020007A0 RID: 1952
		private class StatusDialog : Form
		{
			// Token: 0x06006645 RID: 26181 RVA: 0x001774AC File Offset: 0x001764AC
			internal StatusDialog(PrintControllerWithStatusDialog.BackgroundThread backgroundThread, string dialogTitle)
			{
				this.InitializeComponent();
				this.backgroundThread = backgroundThread;
				this.Text = dialogTitle;
				this.MinimumSize = base.Size;
			}

			// Token: 0x1700157C RID: 5500
			// (get) Token: 0x06006646 RID: 26182 RVA: 0x001774D4 File Offset: 0x001764D4
			private static bool IsRTLResources
			{
				get
				{
					return SR.GetString("RTL") != "RTL_False";
				}
			}

			// Token: 0x06006647 RID: 26183 RVA: 0x001774EC File Offset: 0x001764EC
			private void InitializeComponent()
			{
				if (PrintControllerWithStatusDialog.StatusDialog.IsRTLResources)
				{
					this.RightToLeft = RightToLeft.Yes;
				}
				this.label1 = new Label();
				this.button1 = new Button();
				this.label1.Location = new Point(8, 16);
				this.label1.TextAlign = ContentAlignment.MiddleCenter;
				this.label1.Size = new Size(240, 64);
				this.label1.TabIndex = 1;
				this.label1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
				this.button1.Size = new Size(75, 23);
				this.button1.TabIndex = 0;
				this.button1.Text = SR.GetString("PrintControllerWithStatusDialog_Cancel");
				this.button1.Location = new Point(88, 88);
				this.button1.Anchor = AnchorStyles.Bottom;
				this.button1.Click += this.button1_Click;
				base.AutoScaleDimensions = new Size(6, 13);
				base.AutoScaleMode = AutoScaleMode.Font;
				base.MaximizeBox = false;
				base.ControlBox = false;
				base.MinimizeBox = false;
				base.ClientSize = new Size(256, 122);
				base.CancelButton = this.button1;
				base.SizeGripStyle = SizeGripStyle.Hide;
				base.Controls.Add(this.label1);
				base.Controls.Add(this.button1);
			}

			// Token: 0x06006648 RID: 26184 RVA: 0x0017764F File Offset: 0x0017664F
			private void button1_Click(object sender, EventArgs e)
			{
				this.button1.Enabled = false;
				this.label1.Text = SR.GetString("PrintControllerWithStatusDialog_Canceling");
				this.backgroundThread.canceled = true;
			}

			// Token: 0x04003CD7 RID: 15575
			internal Label label1;

			// Token: 0x04003CD8 RID: 15576
			private Button button1;

			// Token: 0x04003CD9 RID: 15577
			private PrintControllerWithStatusDialog.BackgroundThread backgroundThread;
		}
	}
}
