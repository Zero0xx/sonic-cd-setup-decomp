using System;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x0200072E RID: 1838
	public class RenamedEventArgs : FileSystemEventArgs
	{
		// Token: 0x0600381A RID: 14362 RVA: 0x000ED243 File Offset: 0x000EC243
		public RenamedEventArgs(WatcherChangeTypes changeType, string directory, string name, string oldName) : base(changeType, directory, name)
		{
			if (!directory.EndsWith("\\", StringComparison.Ordinal))
			{
				directory += "\\";
			}
			this.oldName = oldName;
			this.oldFullPath = directory + oldName;
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x000ED27F File Offset: 0x000EC27F
		public string OldFullPath
		{
			get
			{
				new FileIOPermission(FileIOPermissionAccess.Read, Path.GetPathRoot(this.oldFullPath)).Demand();
				return this.oldFullPath;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x000ED29D File Offset: 0x000EC29D
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x04003217 RID: 12823
		private string oldName;

		// Token: 0x04003218 RID: 12824
		private string oldFullPath;
	}
}
