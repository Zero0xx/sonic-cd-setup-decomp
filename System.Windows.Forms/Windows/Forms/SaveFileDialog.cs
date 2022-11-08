using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020005F7 RID: 1527
	[Designer("System.Windows.Forms.Design.SaveFileDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionSaveFileDialog")]
	public sealed class SaveFileDialog : FileDialog
	{
		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x0012670C File Offset: 0x0012570C
		// (set) Token: 0x06005075 RID: 20597 RVA: 0x00126719 File Offset: 0x00125719
		[SRCategory("CatBehavior")]
		[SRDescription("SaveFileDialogCreatePrompt")]
		[DefaultValue(false)]
		public bool CreatePrompt
		{
			get
			{
				return base.GetOption(8192);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				base.SetOption(8192, value);
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x00126731 File Offset: 0x00125731
		// (set) Token: 0x06005077 RID: 20599 RVA: 0x0012673A File Offset: 0x0012573A
		[SRDescription("SaveFileDialogOverWritePrompt")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool OverwritePrompt
		{
			get
			{
				return base.GetOption(2);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				base.SetOption(2, value);
			}
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x00126750 File Offset: 0x00125750
		public Stream OpenFile()
		{
			IntSecurity.FileDialogSaveFile.Demand();
			string text = base.FileNamesInternal[0];
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("FileName");
			}
			Stream result = null;
			new FileIOPermission(FileIOPermissionAccess.AllAccess, IntSecurity.UnsafeGetFullPath(text)).Assert();
			try
			{
				result = new FileStream(text, FileMode.Create, FileAccess.ReadWrite);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x001267BC File Offset: 0x001257BC
		private bool PromptFileCreate(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.GetString("FileDialogCreatePrompt", new object[]
			{
				fileName
			}), base.DialogCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x001267F0 File Offset: 0x001257F0
		private bool PromptFileOverwrite(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.GetString("FileDialogOverwritePrompt", new object[]
			{
				fileName
			}), base.DialogCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x00126824 File Offset: 0x00125824
		internal override bool PromptUserIfAppropriate(string fileName)
		{
			return base.PromptUserIfAppropriate(fileName) && ((this.options & 2) == 0 || !FileDialog.FileExists(fileName) || base.UseVistaDialogInternal || this.PromptFileOverwrite(fileName)) && ((this.options & 8192) == 0 || FileDialog.FileExists(fileName) || this.PromptFileCreate(fileName));
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x00126883 File Offset: 0x00125883
		public override void Reset()
		{
			base.Reset();
			base.SetOption(2, true);
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x00126893 File Offset: 0x00125893
		internal override void EnsureFileDialogPermission()
		{
			IntSecurity.FileDialogSaveFile.Demand();
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x001268A0 File Offset: 0x001258A0
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			IntSecurity.FileDialogSaveFile.Demand();
			bool saveFileName = UnsafeNativeMethods.GetSaveFileName(ofn);
			if (!saveFileName)
			{
				int num = SafeNativeMethods.CommDlgExtendedError();
				int num2 = num;
				if (num2 == 12290)
				{
					throw new InvalidOperationException(SR.GetString("FileDialogInvalidFileName", new object[]
					{
						base.FileName
					}));
				}
			}
			return saveFileName;
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001268F4 File Offset: 0x001258F4
		internal override string[] ProcessVistaFiles(FileDialogNative.IFileDialog dialog)
		{
			FileDialogNative.IFileSaveDialog fileSaveDialog = (FileDialogNative.IFileSaveDialog)dialog;
			FileDialogNative.IShellItem item;
			dialog.GetResult(out item);
			return new string[]
			{
				FileDialog.GetFilePathFromShellItem(item)
			};
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x00126921 File Offset: 0x00125921
		internal override FileDialogNative.IFileDialog CreateVistaDialog()
		{
			return (FileDialogNative.NativeFileSaveDialog)new FileDialogNative.FileSaveDialogRCW();
		}
	}
}
