using System;

namespace System.IO
{
	// Token: 0x02000726 RID: 1830
	public class FileSystemEventArgs : EventArgs
	{
		// Token: 0x060037D1 RID: 14289 RVA: 0x000EBF49 File Offset: 0x000EAF49
		public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name)
		{
			this.changeType = changeType;
			this.name = name;
			if (!directory.EndsWith("\\", StringComparison.Ordinal))
			{
				directory += "\\";
			}
			this.fullPath = directory + name;
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x060037D2 RID: 14290 RVA: 0x000EBF87 File Offset: 0x000EAF87
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x060037D3 RID: 14291 RVA: 0x000EBF8F File Offset: 0x000EAF8F
		public string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x000EBF97 File Offset: 0x000EAF97
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x040031E8 RID: 12776
		private WatcherChangeTypes changeType;

		// Token: 0x040031E9 RID: 12777
		private string name;

		// Token: 0x040031EA RID: 12778
		private string fullPath;
	}
}
