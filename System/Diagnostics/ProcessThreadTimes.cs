using System;

namespace System.Diagnostics
{
	// Token: 0x0200077A RID: 1914
	internal class ProcessThreadTimes
	{
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06003B1E RID: 15134 RVA: 0x000FBB3B File Offset: 0x000FAB3B
		public DateTime StartTime
		{
			get
			{
				return DateTime.FromFileTime(this.create);
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x000FBB48 File Offset: 0x000FAB48
		public DateTime ExitTime
		{
			get
			{
				return DateTime.FromFileTime(this.exit);
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x000FBB55 File Offset: 0x000FAB55
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				return new TimeSpan(this.kernel);
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x000FBB62 File Offset: 0x000FAB62
		public TimeSpan UserProcessorTime
		{
			get
			{
				return new TimeSpan(this.user);
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x000FBB6F File Offset: 0x000FAB6F
		public TimeSpan TotalProcessorTime
		{
			get
			{
				return new TimeSpan(this.user + this.kernel);
			}
		}

		// Token: 0x040033C7 RID: 13255
		internal long create;

		// Token: 0x040033C8 RID: 13256
		internal long exit;

		// Token: 0x040033C9 RID: 13257
		internal long kernel;

		// Token: 0x040033CA RID: 13258
		internal long user;
	}
}
