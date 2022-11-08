using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005CF RID: 1487
	[ComVisible(true)]
	[Serializable]
	public class StringReader : TextReader
	{
		// Token: 0x06003786 RID: 14214 RVA: 0x000BB3A4 File Offset: 0x000BA3A4
		public StringReader(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			this._s = s;
			this._length = ((s == null) ? 0 : s.Length);
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x000BB3D3 File Offset: 0x000BA3D3
		public override void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x000BB3DC File Offset: 0x000BA3DC
		protected override void Dispose(bool disposing)
		{
			this._s = null;
			this._pos = 0;
			this._length = 0;
			base.Dispose(disposing);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x000BB3FA File Offset: 0x000BA3FA
		public override int Peek()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			if (this._pos == this._length)
			{
				return -1;
			}
			return (int)this._s[this._pos];
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x000BB42C File Offset: 0x000BA42C
		public override int Read()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			if (this._pos == this._length)
			{
				return -1;
			}
			return (int)this._s[this._pos++];
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x000BB474 File Offset: 0x000BA474
		public override int Read([In] [Out] char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			int num = this._length - this._pos;
			if (num > 0)
			{
				if (num > count)
				{
					num = count;
				}
				this._s.CopyTo(this._pos, buffer, index, num);
				this._pos += num;
			}
			return num;
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x000BB52C File Offset: 0x000BA52C
		public override string ReadToEnd()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			string result;
			if (this._pos == 0)
			{
				result = this._s;
			}
			else
			{
				result = this._s.Substring(this._pos, this._length - this._pos);
			}
			this._pos = this._length;
			return result;
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x000BB584 File Offset: 0x000BA584
		public override string ReadLine()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			int i;
			for (i = this._pos; i < this._length; i++)
			{
				char c = this._s[i];
				if (c == '\r' || c == '\n')
				{
					string result = this._s.Substring(this._pos, i - this._pos);
					this._pos = i + 1;
					if (c == '\r' && this._pos < this._length && this._s[this._pos] == '\n')
					{
						this._pos++;
					}
					return result;
				}
			}
			if (i > this._pos)
			{
				string result2 = this._s.Substring(this._pos, i - this._pos);
				this._pos = i;
				return result2;
			}
			return null;
		}

		// Token: 0x04001CD8 RID: 7384
		private string _s;

		// Token: 0x04001CD9 RID: 7385
		private int _pos;

		// Token: 0x04001CDA RID: 7386
		private int _length;
	}
}
