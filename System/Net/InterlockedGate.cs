using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003EC RID: 1004
	internal struct InterlockedGate
	{
		// Token: 0x0600207B RID: 8315 RVA: 0x0008020A File Offset: 0x0007F20A
		internal void Reset()
		{
			this.m_State = 0;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00080214 File Offset: 0x0007F214
		internal bool Trigger(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 2, 0);
			if (exclusive && (num == 1 || num == 2))
			{
				throw new InternalException();
			}
			return num == 0;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00080244 File Offset: 0x0007F244
		internal bool StartTrigger(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 1, 0);
			if (exclusive && (num == 1 || num == 2))
			{
				throw new InternalException();
			}
			return num == 0;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00080274 File Offset: 0x0007F274
		internal void FinishTrigger()
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 2, 1);
			if (num != 1)
			{
				throw new InternalException();
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x0008029C File Offset: 0x0007F29C
		internal bool Complete()
		{
			int num;
			while ((num = Interlocked.CompareExchange(ref this.m_State, 3, 2)) != 2)
			{
				if (num == 3)
				{
					return false;
				}
				if (num == 0)
				{
					if (Interlocked.CompareExchange(ref this.m_State, 3, 0) == 0)
					{
						return false;
					}
				}
				else
				{
					Thread.SpinWait(1);
				}
			}
			return true;
		}

		// Token: 0x04001FB5 RID: 8117
		internal const int Open = 0;

		// Token: 0x04001FB6 RID: 8118
		internal const int Held = 1;

		// Token: 0x04001FB7 RID: 8119
		internal const int Triggered = 2;

		// Token: 0x04001FB8 RID: 8120
		internal const int Closed = 3;

		// Token: 0x04001FB9 RID: 8121
		private int m_State;
	}
}
