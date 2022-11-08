using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.IO
{
	// Token: 0x020005C5 RID: 1477
	[ComVisible(true)]
	[Serializable]
	public abstract class TextReader : MarshalByRefObject, IDisposable
	{
		// Token: 0x060036C9 RID: 14025 RVA: 0x000B95CF File Offset: 0x000B85CF
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000B95D8 File Offset: 0x000B85D8
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000B95E1 File Offset: 0x000B85E1
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000B95E3 File Offset: 0x000B85E3
		public virtual int Peek()
		{
			return -1;
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000B95E6 File Offset: 0x000B85E6
		public virtual int Read()
		{
			return -1;
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x000B95EC File Offset: 0x000B85EC
		public virtual int Read([In] [Out] char[] buffer, int index, int count)
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
			int num = 0;
			do
			{
				int num2 = this.Read();
				if (num2 == -1)
				{
					break;
				}
				buffer[index + num++] = (char)num2;
			}
			while (num < count);
			return num;
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x000B9678 File Offset: 0x000B8678
		public virtual string ReadToEnd()
		{
			char[] array = new char[4096];
			StringBuilder stringBuilder = new StringBuilder(4096);
			int charCount;
			while ((charCount = this.Read(array, 0, array.Length)) != 0)
			{
				stringBuilder.Append(array, 0, charCount);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x000B96BC File Offset: 0x000B86BC
		public virtual int ReadBlock([In] [Out] char[] buffer, int index, int count)
		{
			int num = 0;
			int num2;
			do
			{
				num += (num2 = this.Read(buffer, index + num, count - num));
			}
			while (num2 > 0 && num < count);
			return num;
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x000B96E8 File Offset: 0x000B86E8
		public virtual string ReadLine()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (;;)
			{
				num = this.Read();
				if (num == -1)
				{
					goto IL_43;
				}
				if (num == 13 || num == 10)
				{
					break;
				}
				stringBuilder.Append((char)num);
			}
			if (num == 13 && this.Peek() == 10)
			{
				this.Read();
			}
			return stringBuilder.ToString();
			IL_43:
			if (stringBuilder.Length > 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x000B9749 File Offset: 0x000B8749
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static TextReader Synchronized(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader is TextReader.SyncTextReader)
			{
				return reader;
			}
			return new TextReader.SyncTextReader(reader);
		}

		// Token: 0x04001CAC RID: 7340
		public static readonly TextReader Null = new TextReader.NullTextReader();

		// Token: 0x020005C6 RID: 1478
		[Serializable]
		private sealed class NullTextReader : TextReader
		{
			// Token: 0x060036D4 RID: 14036 RVA: 0x000B9775 File Offset: 0x000B8775
			public override int Read(char[] buffer, int index, int count)
			{
				return 0;
			}

			// Token: 0x060036D5 RID: 14037 RVA: 0x000B9778 File Offset: 0x000B8778
			public override string ReadLine()
			{
				return null;
			}
		}

		// Token: 0x020005C7 RID: 1479
		[Serializable]
		internal sealed class SyncTextReader : TextReader
		{
			// Token: 0x060036D7 RID: 14039 RVA: 0x000B9783 File Offset: 0x000B8783
			internal SyncTextReader(TextReader t)
			{
				this._in = t;
			}

			// Token: 0x060036D8 RID: 14040 RVA: 0x000B9792 File Offset: 0x000B8792
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Close()
			{
				this._in.Close();
			}

			// Token: 0x060036D9 RID: 14041 RVA: 0x000B979F File Offset: 0x000B879F
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((IDisposable)this._in).Dispose();
				}
			}

			// Token: 0x060036DA RID: 14042 RVA: 0x000B97AF File Offset: 0x000B87AF
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Peek()
			{
				return this._in.Peek();
			}

			// Token: 0x060036DB RID: 14043 RVA: 0x000B97BC File Offset: 0x000B87BC
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Read()
			{
				return this._in.Read();
			}

			// Token: 0x060036DC RID: 14044 RVA: 0x000B97C9 File Offset: 0x000B87C9
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Read([In] [Out] char[] buffer, int index, int count)
			{
				return this._in.Read(buffer, index, count);
			}

			// Token: 0x060036DD RID: 14045 RVA: 0x000B97D9 File Offset: 0x000B87D9
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int ReadBlock([In] [Out] char[] buffer, int index, int count)
			{
				return this._in.ReadBlock(buffer, index, count);
			}

			// Token: 0x060036DE RID: 14046 RVA: 0x000B97E9 File Offset: 0x000B87E9
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override string ReadLine()
			{
				return this._in.ReadLine();
			}

			// Token: 0x060036DF RID: 14047 RVA: 0x000B97F6 File Offset: 0x000B87F6
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override string ReadToEnd()
			{
				return this._in.ReadToEnd();
			}

			// Token: 0x04001CAD RID: 7341
			internal TextReader _in;
		}
	}
}
