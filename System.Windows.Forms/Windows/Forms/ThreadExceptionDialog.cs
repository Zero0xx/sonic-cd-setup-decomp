using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000662 RID: 1634
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[UIPermission(SecurityAction.Assert, Window = UIPermissionWindow.AllWindows)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public partial class ThreadExceptionDialog : Form
	{
		// Token: 0x060055AF RID: 21935 RVA: 0x0013761C File Offset: 0x0013661C
		public ThreadExceptionDialog(Exception t)
		{
			bool flag = false;
			WarningException ex = t as WarningException;
			string name;
			string text;
			Button[] array;
			if (ex != null)
			{
				name = "ExDlgWarningText";
				text = ex.Message;
				if (ex.HelpUrl == null)
				{
					array = new Button[]
					{
						this.continueButton
					};
				}
				else
				{
					array = new Button[]
					{
						this.continueButton,
						this.helpButton
					};
				}
			}
			else
			{
				text = t.Message;
				flag = true;
				if (Application.AllowQuit)
				{
					if (t is SecurityException)
					{
						name = "ExDlgSecurityErrorText";
					}
					else
					{
						name = "ExDlgErrorText";
					}
					array = new Button[]
					{
						this.detailsButton,
						this.continueButton,
						this.quitButton
					};
				}
				else
				{
					if (t is SecurityException)
					{
						name = "ExDlgSecurityContinueErrorText";
					}
					else
					{
						name = "ExDlgContinueErrorText";
					}
					array = new Button[]
					{
						this.detailsButton,
						this.continueButton
					};
				}
			}
			if (text.Length == 0)
			{
				text = t.GetType().Name;
			}
			if (t is SecurityException)
			{
				text = SR.GetString(name, new object[]
				{
					t.GetType().Name,
					ThreadExceptionDialog.Trim(text)
				});
			}
			else
			{
				text = SR.GetString(name, new object[]
				{
					ThreadExceptionDialog.Trim(text)
				});
			}
			StringBuilder stringBuilder = new StringBuilder();
			string value = "\r\n";
			string @string = SR.GetString("ExDlgMsgSeperator");
			string string2 = SR.GetString("ExDlgMsgSectionSeperator");
			if (Application.CustomThreadExceptionHandlerAttached)
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgHeaderNonSwitchable"));
			}
			else
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgHeaderSwitchable"));
			}
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				SR.GetString("ExDlgMsgExceptionSection")
			}));
			stringBuilder.Append(t.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append(value);
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				SR.GetString("ExDlgMsgLoadedAssembliesSection")
			}));
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			try
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					AssemblyName name2 = assembly.GetName();
					string text2 = SR.GetString("NotAvailable");
					try
					{
						if (name2.EscapedCodeBase != null && name2.EscapedCodeBase.Length > 0)
						{
							Uri uri = new Uri(name2.EscapedCodeBase);
							if (uri.Scheme == "file")
							{
								text2 = FileVersionInfo.GetVersionInfo(NativeMethods.GetLocalPath(name2.EscapedCodeBase)).FileVersion;
							}
						}
					}
					catch (FileNotFoundException)
					{
					}
					stringBuilder.Append(SR.GetString("ExDlgMsgLoadedAssembliesEntry", new object[]
					{
						name2.Name,
						name2.Version,
						text2,
						name2.EscapedCodeBase
					}));
					stringBuilder.Append(@string);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, string2, new object[]
			{
				SR.GetString("ExDlgMsgJITDebuggingSection")
			}));
			if (Application.CustomThreadExceptionHandlerAttached)
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgFooterNonSwitchable"));
			}
			else
			{
				stringBuilder.Append(SR.GetString("ExDlgMsgFooterSwitchable"));
			}
			stringBuilder.Append(value);
			stringBuilder.Append(value);
			string text3 = stringBuilder.ToString();
			Graphics graphics = this.message.CreateGraphicsInternal();
			Size size = Size.Ceiling(graphics.MeasureString(text, this.Font, 356));
			size.Height += 4;
			graphics.Dispose();
			if (size.Width < 180)
			{
				size.Width = 180;
			}
			if (size.Height > 325)
			{
				size.Height = 325;
			}
			int num = size.Width + 84;
			int num2 = Math.Max(size.Height, 40) + 26;
			IntSecurity.GetParent.Assert();
			try
			{
				Form activeForm = Form.ActiveForm;
				if (activeForm == null || activeForm.Text.Length == 0)
				{
					this.Text = SR.GetString("ExDlgCaption");
				}
				else
				{
					this.Text = SR.GetString("ExDlgCaption2", new object[]
					{
						activeForm.Text
					});
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.AcceptButton = this.continueButton;
			base.CancelButton = this.continueButton;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			base.Icon = null;
			base.ClientSize = new Size(num, num2 + 31);
			base.TopMost = true;
			this.pictureBox.Location = new Point(0, 0);
			this.pictureBox.Size = new Size(64, 64);
			this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
			if (t is SecurityException)
			{
				this.pictureBox.Image = SystemIcons.Information.ToBitmap();
			}
			else
			{
				this.pictureBox.Image = SystemIcons.Error.ToBitmap();
			}
			base.Controls.Add(this.pictureBox);
			this.message.SetBounds(64, 8 + (40 - Math.Min(size.Height, 40)) / 2, size.Width, size.Height);
			this.message.Text = text;
			base.Controls.Add(this.message);
			this.continueButton.Text = SR.GetString("ExDlgContinue");
			this.continueButton.FlatStyle = FlatStyle.Standard;
			this.continueButton.DialogResult = DialogResult.Cancel;
			this.quitButton.Text = SR.GetString("ExDlgQuit");
			this.quitButton.FlatStyle = FlatStyle.Standard;
			this.quitButton.DialogResult = DialogResult.Abort;
			this.helpButton.Text = SR.GetString("ExDlgHelp");
			this.helpButton.FlatStyle = FlatStyle.Standard;
			this.helpButton.DialogResult = DialogResult.Yes;
			this.detailsButton.Text = SR.GetString("ExDlgShowDetails");
			this.detailsButton.FlatStyle = FlatStyle.Standard;
			this.detailsButton.Click += this.DetailsClick;
			int num3 = 0;
			if (flag)
			{
				Button button = this.detailsButton;
				this.expandImage = new Bitmap(base.GetType(), "down.bmp");
				this.expandImage.MakeTransparent();
				this.collapseImage = new Bitmap(base.GetType(), "up.bmp");
				this.collapseImage.MakeTransparent();
				button.SetBounds(8, num2, 100, 23);
				button.Image = this.expandImage;
				button.ImageAlign = ContentAlignment.MiddleLeft;
				base.Controls.Add(button);
				num3 = 1;
			}
			int num4 = num - 8 - ((array.Length - num3) * 105 - 5);
			for (int j = num3; j < array.Length; j++)
			{
				Button button = array[j];
				button.SetBounds(num4, num2, 100, 23);
				base.Controls.Add(button);
				num4 += 105;
			}
			this.details.Text = text3;
			this.details.ScrollBars = ScrollBars.Both;
			this.details.Multiline = true;
			this.details.ReadOnly = true;
			this.details.WordWrap = false;
			this.details.TabStop = false;
			this.details.AcceptsReturn = false;
			this.details.SetBounds(8, num2 + 31, num - 16, 154);
			base.Controls.Add(this.details);
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x060055B0 RID: 21936 RVA: 0x00137E4C File Offset: 0x00136E4C
		// (set) Token: 0x060055B1 RID: 21937 RVA: 0x00137E54 File Offset: 0x00136E54
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x1400032C RID: 812
		// (add) Token: 0x060055B2 RID: 21938 RVA: 0x00137E5D File Offset: 0x00136E5D
		// (remove) Token: 0x060055B3 RID: 21939 RVA: 0x00137E66 File Offset: 0x00136E66
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x00137E70 File Offset: 0x00136E70
		private void DetailsClick(object sender, EventArgs eventargs)
		{
			int num = this.details.Height + 8;
			if (this.detailsVisible)
			{
				num = -num;
			}
			base.Height += num;
			this.detailsVisible = !this.detailsVisible;
			this.detailsButton.Image = (this.detailsVisible ? this.collapseImage : this.expandImage);
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x00137ED4 File Offset: 0x00136ED4
		private static string Trim(string s)
		{
			if (s == null)
			{
				return s;
			}
			int num = s.Length;
			while (num > 0 && s[num - 1] == '.')
			{
				num--;
			}
			return s.Substring(0, num);
		}

		// Token: 0x0400371C RID: 14108
		private const int MAXWIDTH = 440;

		// Token: 0x0400371D RID: 14109
		private const int MAXHEIGHT = 325;

		// Token: 0x0400371E RID: 14110
		private PictureBox pictureBox = new PictureBox();

		// Token: 0x0400371F RID: 14111
		private Label message = new Label();

		// Token: 0x04003720 RID: 14112
		private Button continueButton = new Button();

		// Token: 0x04003721 RID: 14113
		private Button quitButton = new Button();

		// Token: 0x04003722 RID: 14114
		private Button detailsButton = new Button();

		// Token: 0x04003723 RID: 14115
		private Button helpButton = new Button();

		// Token: 0x04003724 RID: 14116
		private TextBox details = new TextBox();

		// Token: 0x04003725 RID: 14117
		private Bitmap expandImage;

		// Token: 0x04003726 RID: 14118
		private Bitmap collapseImage;

		// Token: 0x04003727 RID: 14119
		private bool detailsVisible;
	}
}
