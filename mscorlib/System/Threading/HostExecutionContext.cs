using System;

namespace System.Threading
{
	// Token: 0x02000158 RID: 344
	public class HostExecutionContext
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00033329 File Offset: 0x00032329
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x00033331 File Offset: 0x00032331
		protected internal object State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0003333A File Offset: 0x0003233A
		public HostExecutionContext()
		{
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00033342 File Offset: 0x00032342
		public HostExecutionContext(object state)
		{
			this.state = state;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00033351 File Offset: 0x00032351
		public virtual HostExecutionContext CreateCopy()
		{
			if (this.state is IUnknownSafeHandle)
			{
				((IUnknownSafeHandle)this.state).Clone();
			}
			return new HostExecutionContext(this.state);
		}

		// Token: 0x0400065A RID: 1626
		private object state;
	}
}
