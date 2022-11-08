using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x02000021 RID: 33
	internal struct IListEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00003EA9 File Offset: 0x00002EA9
		public IListEnumerator(IList<T> sequence)
		{
			this.sequence = sequence;
			this.index = 0;
			this.current = default(T);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003EC5 File Offset: 0x00002EC5
		public void Dispose()
		{
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003EC7 File Offset: 0x00002EC7
		public T Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003ED0 File Offset: 0x00002ED0
		object IEnumerator.Current
		{
			get
			{
				if (this.index == 0)
				{
					throw new InvalidOperationException(Res.GetString("Sch_EnumNotStarted", new object[]
					{
						string.Empty
					}));
				}
				if (this.index > this.sequence.Count)
				{
					throw new InvalidOperationException(Res.GetString("Sch_EnumFinished", new object[]
					{
						string.Empty
					}));
				}
				return this.current;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003F44 File Offset: 0x00002F44
		public bool MoveNext()
		{
			if (this.index < this.sequence.Count)
			{
				this.current = this.sequence[this.index];
				this.index++;
				return true;
			}
			this.current = default(T);
			return false;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003F98 File Offset: 0x00002F98
		void IEnumerator.Reset()
		{
			this.index = 0;
			this.current = default(T);
		}

		// Token: 0x04000481 RID: 1153
		private IList<T> sequence;

		// Token: 0x04000482 RID: 1154
		private int index;

		// Token: 0x04000483 RID: 1155
		private T current;
	}
}
