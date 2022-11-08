using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000784 RID: 1924
	[Designer("System.Diagnostics.Design.ProcessModuleDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ProcessModule : Component
	{
		// Token: 0x06003B54 RID: 15188 RVA: 0x000FD664 File Offset: 0x000FC664
		internal ProcessModule(ModuleInfo moduleInfo)
		{
			this.moduleInfo = moduleInfo;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x000FD679 File Offset: 0x000FC679
		internal void EnsureNtProcessInfo()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x000FD698 File Offset: 0x000FC698
		[MonitoringDescription("ProcModModuleName")]
		public string ModuleName
		{
			get
			{
				return this.moduleInfo.baseName;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06003B57 RID: 15191 RVA: 0x000FD6A5 File Offset: 0x000FC6A5
		[MonitoringDescription("ProcModFileName")]
		public string FileName
		{
			get
			{
				return this.moduleInfo.fileName;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06003B58 RID: 15192 RVA: 0x000FD6B2 File Offset: 0x000FC6B2
		[MonitoringDescription("ProcModBaseAddress")]
		public IntPtr BaseAddress
		{
			get
			{
				return this.moduleInfo.baseOfDll;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06003B59 RID: 15193 RVA: 0x000FD6BF File Offset: 0x000FC6BF
		[MonitoringDescription("ProcModModuleMemorySize")]
		public int ModuleMemorySize
		{
			get
			{
				return this.moduleInfo.sizeOfImage;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06003B5A RID: 15194 RVA: 0x000FD6CC File Offset: 0x000FC6CC
		[MonitoringDescription("ProcModEntryPointAddress")]
		public IntPtr EntryPointAddress
		{
			get
			{
				this.EnsureNtProcessInfo();
				return this.moduleInfo.entryPoint;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003B5B RID: 15195 RVA: 0x000FD6DF File Offset: 0x000FC6DF
		[Browsable(false)]
		public FileVersionInfo FileVersionInfo
		{
			get
			{
				if (this.fileVersionInfo == null)
				{
					this.fileVersionInfo = FileVersionInfo.GetVersionInfo(this.FileName);
				}
				return this.fileVersionInfo;
			}
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x000FD700 File Offset: 0x000FC700
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
			{
				base.ToString(),
				this.ModuleName
			});
		}

		// Token: 0x0400341A RID: 13338
		internal ModuleInfo moduleInfo;

		// Token: 0x0400341B RID: 13339
		private FileVersionInfo fileVersionInfo;
	}
}
