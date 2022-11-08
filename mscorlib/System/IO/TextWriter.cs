using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.IO
{
	// Token: 0x020005CA RID: 1482
	[ComVisible(true)]
	[Serializable]
	public abstract class TextWriter : MarshalByRefObject, IDisposable
	{
		// Token: 0x06003708 RID: 14088 RVA: 0x000BA460 File Offset: 0x000B9460
		protected TextWriter()
		{
			this.InternalFormatProvider = null;
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x000BA494 File Offset: 0x000B9494
		protected TextWriter(IFormatProvider formatProvider)
		{
			this.InternalFormatProvider = formatProvider;
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x000BA4C6 File Offset: 0x000B94C6
		public virtual IFormatProvider FormatProvider
		{
			get
			{
				if (this.InternalFormatProvider == null)
				{
					return Thread.CurrentThread.CurrentCulture;
				}
				return this.InternalFormatProvider;
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x000BA4E1 File Offset: 0x000B94E1
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x000BA4F0 File Offset: 0x000B94F0
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x000BA4F2 File Offset: 0x000B94F2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000BA501 File Offset: 0x000B9501
		public virtual void Flush()
		{
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x0600370F RID: 14095
		public abstract Encoding Encoding { get; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003710 RID: 14096 RVA: 0x000BA503 File Offset: 0x000B9503
		// (set) Token: 0x06003711 RID: 14097 RVA: 0x000BA510 File Offset: 0x000B9510
		public virtual string NewLine
		{
			get
			{
				return new string(this.CoreNewLine);
			}
			set
			{
				if (value == null)
				{
					value = "\r\n";
				}
				this.CoreNewLine = value.ToCharArray();
			}
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000BA528 File Offset: 0x000B9528
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static TextWriter Synchronized(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer is TextWriter.SyncTextWriter)
			{
				return writer;
			}
			return new TextWriter.SyncTextWriter(writer);
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000BA548 File Offset: 0x000B9548
		public virtual void Write(char value)
		{
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000BA54A File Offset: 0x000B954A
		public virtual void Write(char[] buffer)
		{
			if (buffer != null)
			{
				this.Write(buffer, 0, buffer.Length);
			}
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000BA55C File Offset: 0x000B955C
		public virtual void Write(char[] buffer, int index, int count)
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
			for (int i = 0; i < count; i++)
			{
				this.Write(buffer[index + i]);
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000BA5E2 File Offset: 0x000B95E2
		public virtual void Write(bool value)
		{
			this.Write(value ? "True" : "False");
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000BA5F9 File Offset: 0x000B95F9
		public virtual void Write(int value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000BA60E File Offset: 0x000B960E
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000BA623 File Offset: 0x000B9623
		public virtual void Write(long value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000BA638 File Offset: 0x000B9638
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000BA64D File Offset: 0x000B964D
		public virtual void Write(float value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000BA662 File Offset: 0x000B9662
		public virtual void Write(double value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000BA677 File Offset: 0x000B9677
		public virtual void Write(decimal value)
		{
			this.Write(value.ToString(this.FormatProvider));
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000BA68C File Offset: 0x000B968C
		public virtual void Write(string value)
		{
			if (value != null)
			{
				this.Write(value.ToCharArray());
			}
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000BA6A0 File Offset: 0x000B96A0
		public virtual void Write(object value)
		{
			if (value != null)
			{
				IFormattable formattable = value as IFormattable;
				if (formattable != null)
				{
					this.Write(formattable.ToString(null, this.FormatProvider));
					return;
				}
				this.Write(value.ToString());
			}
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000BA6DC File Offset: 0x000B96DC
		public virtual void Write(string format, object arg0)
		{
			this.Write(string.Format(this.FormatProvider, format, new object[]
			{
				arg0
			}));
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000BA708 File Offset: 0x000B9708
		public virtual void Write(string format, object arg0, object arg1)
		{
			this.Write(string.Format(this.FormatProvider, format, new object[]
			{
				arg0,
				arg1
			}));
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000BA738 File Offset: 0x000B9738
		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			this.Write(string.Format(this.FormatProvider, format, new object[]
			{
				arg0,
				arg1,
				arg2
			}));
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000BA76C File Offset: 0x000B976C
		public virtual void Write(string format, params object[] arg)
		{
			this.Write(string.Format(this.FormatProvider, format, arg));
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000BA781 File Offset: 0x000B9781
		public virtual void WriteLine()
		{
			this.Write(this.CoreNewLine);
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000BA78F File Offset: 0x000B978F
		public virtual void WriteLine(char value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x000BA79E File Offset: 0x000B979E
		public virtual void WriteLine(char[] buffer)
		{
			this.Write(buffer);
			this.WriteLine();
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000BA7AD File Offset: 0x000B97AD
		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			this.Write(buffer, index, count);
			this.WriteLine();
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000BA7BE File Offset: 0x000B97BE
		public virtual void WriteLine(bool value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000BA7CD File Offset: 0x000B97CD
		public virtual void WriteLine(int value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x000BA7DC File Offset: 0x000B97DC
		[CLSCompliant(false)]
		public virtual void WriteLine(uint value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x000BA7EB File Offset: 0x000B97EB
		public virtual void WriteLine(long value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x000BA7FA File Offset: 0x000B97FA
		[CLSCompliant(false)]
		public virtual void WriteLine(ulong value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000BA809 File Offset: 0x000B9809
		public virtual void WriteLine(float value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x000BA818 File Offset: 0x000B9818
		public virtual void WriteLine(double value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x000BA827 File Offset: 0x000B9827
		public virtual void WriteLine(decimal value)
		{
			this.Write(value);
			this.WriteLine();
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x000BA838 File Offset: 0x000B9838
		public virtual void WriteLine(string value)
		{
			if (value == null)
			{
				this.WriteLine();
				return;
			}
			int length = value.Length;
			int num = this.CoreNewLine.Length;
			char[] array = new char[length + num];
			value.CopyTo(0, array, 0, length);
			if (num == 2)
			{
				array[length] = this.CoreNewLine[0];
				array[length + 1] = this.CoreNewLine[1];
			}
			else if (num == 1)
			{
				array[length] = this.CoreNewLine[0];
			}
			else
			{
				Buffer.InternalBlockCopy(this.CoreNewLine, 0, array, length * 2, num * 2);
			}
			this.Write(array, 0, length + num);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x000BA8C0 File Offset: 0x000B98C0
		public virtual void WriteLine(object value)
		{
			if (value == null)
			{
				this.WriteLine();
				return;
			}
			IFormattable formattable = value as IFormattable;
			if (formattable != null)
			{
				this.WriteLine(formattable.ToString(null, this.FormatProvider));
				return;
			}
			this.WriteLine(value.ToString());
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000BA904 File Offset: 0x000B9904
		public virtual void WriteLine(string format, object arg0)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, new object[]
			{
				arg0
			}));
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x000BA930 File Offset: 0x000B9930
		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, new object[]
			{
				arg0,
				arg1
			}));
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x000BA960 File Offset: 0x000B9960
		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, new object[]
			{
				arg0,
				arg1,
				arg2
			}));
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x000BA994 File Offset: 0x000B9994
		public virtual void WriteLine(string format, params object[] arg)
		{
			this.WriteLine(string.Format(this.FormatProvider, format, arg));
		}

		// Token: 0x04001CC1 RID: 7361
		private const string InitialNewLine = "\r\n";

		// Token: 0x04001CC2 RID: 7362
		public static readonly TextWriter Null = new TextWriter.NullTextWriter();

		// Token: 0x04001CC3 RID: 7363
		protected char[] CoreNewLine = new char[]
		{
			'\r',
			'\n'
		};

		// Token: 0x04001CC4 RID: 7364
		private IFormatProvider InternalFormatProvider;

		// Token: 0x020005CB RID: 1483
		[Serializable]
		private sealed class NullTextWriter : TextWriter
		{
			// Token: 0x06003737 RID: 14135 RVA: 0x000BA9B5 File Offset: 0x000B99B5
			internal NullTextWriter() : base(CultureInfo.InvariantCulture)
			{
			}

			// Token: 0x17000951 RID: 2385
			// (get) Token: 0x06003738 RID: 14136 RVA: 0x000BA9C2 File Offset: 0x000B99C2
			public override Encoding Encoding
			{
				get
				{
					return Encoding.Default;
				}
			}

			// Token: 0x06003739 RID: 14137 RVA: 0x000BA9C9 File Offset: 0x000B99C9
			public override void Write(char[] buffer, int index, int count)
			{
			}

			// Token: 0x0600373A RID: 14138 RVA: 0x000BA9CB File Offset: 0x000B99CB
			public override void Write(string value)
			{
			}

			// Token: 0x0600373B RID: 14139 RVA: 0x000BA9CD File Offset: 0x000B99CD
			public override void WriteLine()
			{
			}

			// Token: 0x0600373C RID: 14140 RVA: 0x000BA9CF File Offset: 0x000B99CF
			public override void WriteLine(string value)
			{
			}

			// Token: 0x0600373D RID: 14141 RVA: 0x000BA9D1 File Offset: 0x000B99D1
			public override void WriteLine(object value)
			{
			}
		}

		// Token: 0x020005CC RID: 1484
		[Serializable]
		internal sealed class SyncTextWriter : TextWriter, IDisposable
		{
			// Token: 0x0600373E RID: 14142 RVA: 0x000BA9D3 File Offset: 0x000B99D3
			internal SyncTextWriter(TextWriter t) : base(t.FormatProvider)
			{
				this._out = t;
			}

			// Token: 0x17000952 RID: 2386
			// (get) Token: 0x0600373F RID: 14143 RVA: 0x000BA9E8 File Offset: 0x000B99E8
			public override Encoding Encoding
			{
				get
				{
					return this._out.Encoding;
				}
			}

			// Token: 0x17000953 RID: 2387
			// (get) Token: 0x06003740 RID: 14144 RVA: 0x000BA9F5 File Offset: 0x000B99F5
			public override IFormatProvider FormatProvider
			{
				get
				{
					return this._out.FormatProvider;
				}
			}

			// Token: 0x17000954 RID: 2388
			// (get) Token: 0x06003741 RID: 14145 RVA: 0x000BAA02 File Offset: 0x000B9A02
			// (set) Token: 0x06003742 RID: 14146 RVA: 0x000BAA0F File Offset: 0x000B9A0F
			public override string NewLine
			{
				[MethodImpl(MethodImplOptions.Synchronized)]
				get
				{
					return this._out.NewLine;
				}
				[MethodImpl(MethodImplOptions.Synchronized)]
				set
				{
					this._out.NewLine = value;
				}
			}

			// Token: 0x06003743 RID: 14147 RVA: 0x000BAA1D File Offset: 0x000B9A1D
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Close()
			{
				this._out.Close();
			}

			// Token: 0x06003744 RID: 14148 RVA: 0x000BAA2A File Offset: 0x000B9A2A
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((IDisposable)this._out).Dispose();
				}
			}

			// Token: 0x06003745 RID: 14149 RVA: 0x000BAA3A File Offset: 0x000B9A3A
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Flush()
			{
				this._out.Flush();
			}

			// Token: 0x06003746 RID: 14150 RVA: 0x000BAA47 File Offset: 0x000B9A47
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003747 RID: 14151 RVA: 0x000BAA55 File Offset: 0x000B9A55
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char[] buffer)
			{
				this._out.Write(buffer);
			}

			// Token: 0x06003748 RID: 14152 RVA: 0x000BAA63 File Offset: 0x000B9A63
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(char[] buffer, int index, int count)
			{
				this._out.Write(buffer, index, count);
			}

			// Token: 0x06003749 RID: 14153 RVA: 0x000BAA73 File Offset: 0x000B9A73
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(bool value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600374A RID: 14154 RVA: 0x000BAA81 File Offset: 0x000B9A81
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(int value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600374B RID: 14155 RVA: 0x000BAA8F File Offset: 0x000B9A8F
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(uint value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600374C RID: 14156 RVA: 0x000BAA9D File Offset: 0x000B9A9D
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(long value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600374D RID: 14157 RVA: 0x000BAAAB File Offset: 0x000B9AAB
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(ulong value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600374E RID: 14158 RVA: 0x000BAAB9 File Offset: 0x000B9AB9
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(float value)
			{
				this._out.Write(value);
			}

			// Token: 0x0600374F RID: 14159 RVA: 0x000BAAC7 File Offset: 0x000B9AC7
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(double value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003750 RID: 14160 RVA: 0x000BAAD5 File Offset: 0x000B9AD5
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(decimal value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003751 RID: 14161 RVA: 0x000BAAE3 File Offset: 0x000B9AE3
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003752 RID: 14162 RVA: 0x000BAAF1 File Offset: 0x000B9AF1
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(object value)
			{
				this._out.Write(value);
			}

			// Token: 0x06003753 RID: 14163 RVA: 0x000BAAFF File Offset: 0x000B9AFF
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0)
			{
				this._out.Write(format, arg0);
			}

			// Token: 0x06003754 RID: 14164 RVA: 0x000BAB0E File Offset: 0x000B9B0E
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0, object arg1)
			{
				this._out.Write(format, arg0, arg1);
			}

			// Token: 0x06003755 RID: 14165 RVA: 0x000BAB1E File Offset: 0x000B9B1E
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object arg0, object arg1, object arg2)
			{
				this._out.Write(format, arg0, arg1, arg2);
			}

			// Token: 0x06003756 RID: 14166 RVA: 0x000BAB30 File Offset: 0x000B9B30
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Write(string format, object[] arg)
			{
				this._out.Write(format, arg);
			}

			// Token: 0x06003757 RID: 14167 RVA: 0x000BAB3F File Offset: 0x000B9B3F
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine()
			{
				this._out.WriteLine();
			}

			// Token: 0x06003758 RID: 14168 RVA: 0x000BAB4C File Offset: 0x000B9B4C
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003759 RID: 14169 RVA: 0x000BAB5A File Offset: 0x000B9B5A
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(decimal value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600375A RID: 14170 RVA: 0x000BAB68 File Offset: 0x000B9B68
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char[] buffer)
			{
				this._out.WriteLine(buffer);
			}

			// Token: 0x0600375B RID: 14171 RVA: 0x000BAB76 File Offset: 0x000B9B76
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(char[] buffer, int index, int count)
			{
				this._out.WriteLine(buffer, index, count);
			}

			// Token: 0x0600375C RID: 14172 RVA: 0x000BAB86 File Offset: 0x000B9B86
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(bool value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600375D RID: 14173 RVA: 0x000BAB94 File Offset: 0x000B9B94
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(int value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600375E RID: 14174 RVA: 0x000BABA2 File Offset: 0x000B9BA2
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(uint value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x0600375F RID: 14175 RVA: 0x000BABB0 File Offset: 0x000B9BB0
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(long value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003760 RID: 14176 RVA: 0x000BABBE File Offset: 0x000B9BBE
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(ulong value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003761 RID: 14177 RVA: 0x000BABCC File Offset: 0x000B9BCC
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(float value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003762 RID: 14178 RVA: 0x000BABDA File Offset: 0x000B9BDA
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(double value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003763 RID: 14179 RVA: 0x000BABE8 File Offset: 0x000B9BE8
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003764 RID: 14180 RVA: 0x000BABF6 File Offset: 0x000B9BF6
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(object value)
			{
				this._out.WriteLine(value);
			}

			// Token: 0x06003765 RID: 14181 RVA: 0x000BAC04 File Offset: 0x000B9C04
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0)
			{
				this._out.WriteLine(format, arg0);
			}

			// Token: 0x06003766 RID: 14182 RVA: 0x000BAC13 File Offset: 0x000B9C13
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0, object arg1)
			{
				this._out.WriteLine(format, arg0, arg1);
			}

			// Token: 0x06003767 RID: 14183 RVA: 0x000BAC23 File Offset: 0x000B9C23
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object arg0, object arg1, object arg2)
			{
				this._out.WriteLine(format, arg0, arg1, arg2);
			}

			// Token: 0x06003768 RID: 14184 RVA: 0x000BAC35 File Offset: 0x000B9C35
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void WriteLine(string format, object[] arg)
			{
				this._out.WriteLine(format, arg);
			}

			// Token: 0x04001CC5 RID: 7365
			private TextWriter _out;
		}
	}
}
