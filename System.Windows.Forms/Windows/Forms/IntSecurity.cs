using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200073E RID: 1854
	internal static class IntSecurity
	{
		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x060062B0 RID: 25264 RVA: 0x00166C5C File Offset: 0x00165C5C
		public static CodeAccessPermission AdjustCursorClip
		{
			get
			{
				if (IntSecurity.adjustCursorClip == null)
				{
					IntSecurity.adjustCursorClip = IntSecurity.AllWindows;
				}
				return IntSecurity.adjustCursorClip;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x060062B1 RID: 25265 RVA: 0x00166C74 File Offset: 0x00165C74
		public static CodeAccessPermission AdjustCursorPosition
		{
			get
			{
				return IntSecurity.AllWindows;
			}
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x00166C7B File Offset: 0x00165C7B
		public static CodeAccessPermission AffectMachineState
		{
			get
			{
				if (IntSecurity.affectMachineState == null)
				{
					IntSecurity.affectMachineState = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.affectMachineState;
			}
		}

		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x00166C93 File Offset: 0x00165C93
		public static CodeAccessPermission AffectThreadBehavior
		{
			get
			{
				if (IntSecurity.affectThreadBehavior == null)
				{
					IntSecurity.affectThreadBehavior = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.affectThreadBehavior;
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x060062B4 RID: 25268 RVA: 0x00166CAB File Offset: 0x00165CAB
		public static CodeAccessPermission AllPrinting
		{
			get
			{
				if (IntSecurity.allPrinting == null)
				{
					IntSecurity.allPrinting = new PrintingPermission(PrintingPermissionLevel.AllPrinting);
				}
				return IntSecurity.allPrinting;
			}
		}

		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x060062B5 RID: 25269 RVA: 0x00166CC4 File Offset: 0x00165CC4
		public static PermissionSet AllPrintingAndUnmanagedCode
		{
			get
			{
				if (IntSecurity.allPrintingAndUnmanagedCode == null)
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.SetPermission(IntSecurity.UnmanagedCode);
					permissionSet.SetPermission(IntSecurity.AllPrinting);
					IntSecurity.allPrintingAndUnmanagedCode = permissionSet;
				}
				return IntSecurity.allPrintingAndUnmanagedCode;
			}
		}

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x060062B6 RID: 25270 RVA: 0x00166D02 File Offset: 0x00165D02
		public static CodeAccessPermission AllWindows
		{
			get
			{
				if (IntSecurity.allWindows == null)
				{
					IntSecurity.allWindows = new UIPermission(UIPermissionWindow.AllWindows);
				}
				return IntSecurity.allWindows;
			}
		}

		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x060062B7 RID: 25271 RVA: 0x00166D1B File Offset: 0x00165D1B
		public static CodeAccessPermission ClipboardRead
		{
			get
			{
				if (IntSecurity.clipboardRead == null)
				{
					IntSecurity.clipboardRead = new UIPermission(UIPermissionClipboard.AllClipboard);
				}
				return IntSecurity.clipboardRead;
			}
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x060062B8 RID: 25272 RVA: 0x00166D34 File Offset: 0x00165D34
		public static CodeAccessPermission ClipboardOwn
		{
			get
			{
				if (IntSecurity.clipboardOwn == null)
				{
					IntSecurity.clipboardOwn = new UIPermission(UIPermissionClipboard.OwnClipboard);
				}
				return IntSecurity.clipboardOwn;
			}
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x060062B9 RID: 25273 RVA: 0x00166D4D File Offset: 0x00165D4D
		public static PermissionSet ClipboardWrite
		{
			get
			{
				if (IntSecurity.clipboardWrite == null)
				{
					IntSecurity.clipboardWrite = new PermissionSet(PermissionState.None);
					IntSecurity.clipboardWrite.SetPermission(IntSecurity.UnmanagedCode);
					IntSecurity.clipboardWrite.SetPermission(IntSecurity.ClipboardOwn);
				}
				return IntSecurity.clipboardWrite;
			}
		}

		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x060062BA RID: 25274 RVA: 0x00166D86 File Offset: 0x00165D86
		public static CodeAccessPermission ChangeWindowRegionForTopLevel
		{
			get
			{
				if (IntSecurity.changeWindowRegionForTopLevel == null)
				{
					IntSecurity.changeWindowRegionForTopLevel = IntSecurity.AllWindows;
				}
				return IntSecurity.changeWindowRegionForTopLevel;
			}
		}

		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x060062BB RID: 25275 RVA: 0x00166D9E File Offset: 0x00165D9E
		public static CodeAccessPermission ControlFromHandleOrLocation
		{
			get
			{
				if (IntSecurity.controlFromHandleOrLocation == null)
				{
					IntSecurity.controlFromHandleOrLocation = IntSecurity.AllWindows;
				}
				return IntSecurity.controlFromHandleOrLocation;
			}
		}

		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x060062BC RID: 25276 RVA: 0x00166DB6 File Offset: 0x00165DB6
		public static CodeAccessPermission CreateAnyWindow
		{
			get
			{
				if (IntSecurity.createAnyWindow == null)
				{
					IntSecurity.createAnyWindow = IntSecurity.SafeSubWindows;
				}
				return IntSecurity.createAnyWindow;
			}
		}

		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x060062BD RID: 25277 RVA: 0x00166DCE File Offset: 0x00165DCE
		public static CodeAccessPermission CreateGraphicsForControl
		{
			get
			{
				if (IntSecurity.createGraphicsForControl == null)
				{
					IntSecurity.createGraphicsForControl = IntSecurity.SafeSubWindows;
				}
				return IntSecurity.createGraphicsForControl;
			}
		}

		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x060062BE RID: 25278 RVA: 0x00166DE6 File Offset: 0x00165DE6
		public static CodeAccessPermission DefaultPrinting
		{
			get
			{
				if (IntSecurity.defaultPrinting == null)
				{
					IntSecurity.defaultPrinting = new PrintingPermission(PrintingPermissionLevel.DefaultPrinting);
				}
				return IntSecurity.defaultPrinting;
			}
		}

		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x060062BF RID: 25279 RVA: 0x00166DFF File Offset: 0x00165DFF
		public static CodeAccessPermission FileDialogCustomization
		{
			get
			{
				if (IntSecurity.fileDialogCustomization == null)
				{
					IntSecurity.fileDialogCustomization = new FileIOPermission(PermissionState.Unrestricted);
				}
				return IntSecurity.fileDialogCustomization;
			}
		}

		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x060062C0 RID: 25280 RVA: 0x00166E18 File Offset: 0x00165E18
		public static CodeAccessPermission FileDialogOpenFile
		{
			get
			{
				if (IntSecurity.fileDialogOpenFile == null)
				{
					IntSecurity.fileDialogOpenFile = new FileDialogPermission(FileDialogPermissionAccess.Open);
				}
				return IntSecurity.fileDialogOpenFile;
			}
		}

		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x060062C1 RID: 25281 RVA: 0x00166E31 File Offset: 0x00165E31
		public static CodeAccessPermission FileDialogSaveFile
		{
			get
			{
				if (IntSecurity.fileDialogSaveFile == null)
				{
					IntSecurity.fileDialogSaveFile = new FileDialogPermission(FileDialogPermissionAccess.Save);
				}
				return IntSecurity.fileDialogSaveFile;
			}
		}

		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x060062C2 RID: 25282 RVA: 0x00166E4A File Offset: 0x00165E4A
		public static CodeAccessPermission GetCapture
		{
			get
			{
				if (IntSecurity.getCapture == null)
				{
					IntSecurity.getCapture = IntSecurity.AllWindows;
				}
				return IntSecurity.getCapture;
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x060062C3 RID: 25283 RVA: 0x00166E62 File Offset: 0x00165E62
		public static CodeAccessPermission GetParent
		{
			get
			{
				if (IntSecurity.getParent == null)
				{
					IntSecurity.getParent = IntSecurity.AllWindows;
				}
				return IntSecurity.getParent;
			}
		}

		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x060062C4 RID: 25284 RVA: 0x00166E7A File Offset: 0x00165E7A
		public static CodeAccessPermission ManipulateWndProcAndHandles
		{
			get
			{
				if (IntSecurity.manipulateWndProcAndHandles == null)
				{
					IntSecurity.manipulateWndProcAndHandles = IntSecurity.AllWindows;
				}
				return IntSecurity.manipulateWndProcAndHandles;
			}
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x060062C5 RID: 25285 RVA: 0x00166E92 File Offset: 0x00165E92
		public static CodeAccessPermission ModifyCursor
		{
			get
			{
				if (IntSecurity.modifyCursor == null)
				{
					IntSecurity.modifyCursor = IntSecurity.SafeSubWindows;
				}
				return IntSecurity.modifyCursor;
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x060062C6 RID: 25286 RVA: 0x00166EAA File Offset: 0x00165EAA
		public static CodeAccessPermission ModifyFocus
		{
			get
			{
				if (IntSecurity.modifyFocus == null)
				{
					IntSecurity.modifyFocus = IntSecurity.AllWindows;
				}
				return IntSecurity.modifyFocus;
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x060062C7 RID: 25287 RVA: 0x00166EC2 File Offset: 0x00165EC2
		public static CodeAccessPermission ObjectFromWin32Handle
		{
			get
			{
				if (IntSecurity.objectFromWin32Handle == null)
				{
					IntSecurity.objectFromWin32Handle = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.objectFromWin32Handle;
			}
		}

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x060062C8 RID: 25288 RVA: 0x00166EDA File Offset: 0x00165EDA
		public static CodeAccessPermission SafePrinting
		{
			get
			{
				if (IntSecurity.safePrinting == null)
				{
					IntSecurity.safePrinting = new PrintingPermission(PrintingPermissionLevel.SafePrinting);
				}
				return IntSecurity.safePrinting;
			}
		}

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x060062C9 RID: 25289 RVA: 0x00166EF3 File Offset: 0x00165EF3
		public static CodeAccessPermission SafeSubWindows
		{
			get
			{
				if (IntSecurity.safeSubWindows == null)
				{
					IntSecurity.safeSubWindows = new UIPermission(UIPermissionWindow.SafeSubWindows);
				}
				return IntSecurity.safeSubWindows;
			}
		}

		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x060062CA RID: 25290 RVA: 0x00166F0C File Offset: 0x00165F0C
		public static CodeAccessPermission SafeTopLevelWindows
		{
			get
			{
				if (IntSecurity.safeTopLevelWindows == null)
				{
					IntSecurity.safeTopLevelWindows = new UIPermission(UIPermissionWindow.SafeTopLevelWindows);
				}
				return IntSecurity.safeTopLevelWindows;
			}
		}

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x060062CB RID: 25291 RVA: 0x00166F25 File Offset: 0x00165F25
		public static CodeAccessPermission SendMessages
		{
			get
			{
				if (IntSecurity.sendMessages == null)
				{
					IntSecurity.sendMessages = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.sendMessages;
			}
		}

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x060062CC RID: 25292 RVA: 0x00166F3D File Offset: 0x00165F3D
		public static CodeAccessPermission SensitiveSystemInformation
		{
			get
			{
				if (IntSecurity.sensitiveSystemInformation == null)
				{
					IntSecurity.sensitiveSystemInformation = new EnvironmentPermission(PermissionState.Unrestricted);
				}
				return IntSecurity.sensitiveSystemInformation;
			}
		}

		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x060062CD RID: 25293 RVA: 0x00166F56 File Offset: 0x00165F56
		public static CodeAccessPermission TransparentWindows
		{
			get
			{
				if (IntSecurity.transparentWindows == null)
				{
					IntSecurity.transparentWindows = IntSecurity.AllWindows;
				}
				return IntSecurity.transparentWindows;
			}
		}

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x060062CE RID: 25294 RVA: 0x00166F6E File Offset: 0x00165F6E
		public static CodeAccessPermission TopLevelWindow
		{
			get
			{
				if (IntSecurity.topLevelWindow == null)
				{
					IntSecurity.topLevelWindow = IntSecurity.SafeTopLevelWindows;
				}
				return IntSecurity.topLevelWindow;
			}
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x060062CF RID: 25295 RVA: 0x00166F86 File Offset: 0x00165F86
		public static CodeAccessPermission UnmanagedCode
		{
			get
			{
				if (IntSecurity.unmanagedCode == null)
				{
					IntSecurity.unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				}
				return IntSecurity.unmanagedCode;
			}
		}

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x060062D0 RID: 25296 RVA: 0x00166F9F File Offset: 0x00165F9F
		public static CodeAccessPermission UnrestrictedWindows
		{
			get
			{
				if (IntSecurity.unrestrictedWindows == null)
				{
					IntSecurity.unrestrictedWindows = IntSecurity.AllWindows;
				}
				return IntSecurity.unrestrictedWindows;
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x060062D1 RID: 25297 RVA: 0x00166FB7 File Offset: 0x00165FB7
		public static CodeAccessPermission WindowAdornmentModification
		{
			get
			{
				if (IntSecurity.windowAdornmentModification == null)
				{
					IntSecurity.windowAdornmentModification = IntSecurity.AllWindows;
				}
				return IntSecurity.windowAdornmentModification;
			}
		}

		// Token: 0x060062D2 RID: 25298 RVA: 0x00166FD0 File Offset: 0x00165FD0
		internal static string UnsafeGetFullPath(string fileName)
		{
			string result = fileName;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				result = Path.GetFullPath(fileName);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x060062D3 RID: 25299 RVA: 0x00167014 File Offset: 0x00166014
		internal static void DemandFileIO(FileIOPermissionAccess access, string fileName)
		{
			new FileIOPermission(access, IntSecurity.UnsafeGetFullPath(fileName)).Demand();
		}

		// Token: 0x04003B2A RID: 15146
		public static readonly TraceSwitch SecurityDemand = new TraceSwitch("SecurityDemand", "Trace when security demands occur.");

		// Token: 0x04003B2B RID: 15147
		private static CodeAccessPermission adjustCursorClip;

		// Token: 0x04003B2C RID: 15148
		private static CodeAccessPermission affectMachineState;

		// Token: 0x04003B2D RID: 15149
		private static CodeAccessPermission affectThreadBehavior;

		// Token: 0x04003B2E RID: 15150
		private static CodeAccessPermission allPrinting;

		// Token: 0x04003B2F RID: 15151
		private static PermissionSet allPrintingAndUnmanagedCode;

		// Token: 0x04003B30 RID: 15152
		private static CodeAccessPermission allWindows;

		// Token: 0x04003B31 RID: 15153
		private static CodeAccessPermission clipboardRead;

		// Token: 0x04003B32 RID: 15154
		private static CodeAccessPermission clipboardOwn;

		// Token: 0x04003B33 RID: 15155
		private static PermissionSet clipboardWrite;

		// Token: 0x04003B34 RID: 15156
		private static CodeAccessPermission changeWindowRegionForTopLevel;

		// Token: 0x04003B35 RID: 15157
		private static CodeAccessPermission controlFromHandleOrLocation;

		// Token: 0x04003B36 RID: 15158
		private static CodeAccessPermission createAnyWindow;

		// Token: 0x04003B37 RID: 15159
		private static CodeAccessPermission createGraphicsForControl;

		// Token: 0x04003B38 RID: 15160
		private static CodeAccessPermission defaultPrinting;

		// Token: 0x04003B39 RID: 15161
		private static CodeAccessPermission fileDialogCustomization;

		// Token: 0x04003B3A RID: 15162
		private static CodeAccessPermission fileDialogOpenFile;

		// Token: 0x04003B3B RID: 15163
		private static CodeAccessPermission fileDialogSaveFile;

		// Token: 0x04003B3C RID: 15164
		private static CodeAccessPermission getCapture;

		// Token: 0x04003B3D RID: 15165
		private static CodeAccessPermission getParent;

		// Token: 0x04003B3E RID: 15166
		private static CodeAccessPermission manipulateWndProcAndHandles;

		// Token: 0x04003B3F RID: 15167
		private static CodeAccessPermission modifyCursor;

		// Token: 0x04003B40 RID: 15168
		private static CodeAccessPermission modifyFocus;

		// Token: 0x04003B41 RID: 15169
		private static CodeAccessPermission objectFromWin32Handle;

		// Token: 0x04003B42 RID: 15170
		private static CodeAccessPermission safePrinting;

		// Token: 0x04003B43 RID: 15171
		private static CodeAccessPermission safeSubWindows;

		// Token: 0x04003B44 RID: 15172
		private static CodeAccessPermission safeTopLevelWindows;

		// Token: 0x04003B45 RID: 15173
		private static CodeAccessPermission sendMessages;

		// Token: 0x04003B46 RID: 15174
		private static CodeAccessPermission sensitiveSystemInformation;

		// Token: 0x04003B47 RID: 15175
		private static CodeAccessPermission transparentWindows;

		// Token: 0x04003B48 RID: 15176
		private static CodeAccessPermission topLevelWindow;

		// Token: 0x04003B49 RID: 15177
		private static CodeAccessPermission unmanagedCode;

		// Token: 0x04003B4A RID: 15178
		private static CodeAccessPermission unrestrictedWindows;

		// Token: 0x04003B4B RID: 15179
		private static CodeAccessPermission windowAdornmentModification;
	}
}
