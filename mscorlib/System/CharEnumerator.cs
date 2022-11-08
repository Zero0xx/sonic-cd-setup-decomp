using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200007D RID: 125
	[ComVisible(true)]
	[Serializable]
	public sealed class CharEnumerator : ICloneable, IEnumerator<char>, IDisposable, IEnumerator
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x00017C14 File Offset: 0x00016C14
		internal CharEnumerator(string str)
		{
			this.str = str;
			this.index = -1;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00017C2A File Offset: 0x00016C2A
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00017C34 File Offset: 0x00016C34
		public bool MoveNext()
		{
			if (this.index < this.str.Length - 1)
			{
				this.index++;
				this.currentElement = this.str[this.index];
				return true;
			}
			this.index = this.str.Length;
			return false;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00017C8F File Offset: 0x00016C8F
		void IDisposable.Dispose()
		{
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00017C94 File Offset: 0x00016C94
		object IEnumerator.Current
		{
			get
			{
				if (this.index == -1)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (this.index >= this.str.Length)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
				return this.currentElement;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x00017CE8 File Offset: 0x00016CE8
		public char Current
		{
			get
			{
				if (this.index == -1)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (this.index >= this.str.Length)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
				return this.currentElement;
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00017D37 File Offset: 0x00016D37
		public void Reset()
		{
			this.currentElement = '\0';
			this.index = -1;
		}

		// Token: 0x0400022A RID: 554
		private string str;

		// Token: 0x0400022B RID: 555
		private int index;

		// Token: 0x0400022C RID: 556
		private char currentElement;
	}
}
