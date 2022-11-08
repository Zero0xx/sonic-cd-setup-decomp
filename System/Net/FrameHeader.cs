using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200054C RID: 1356
	internal class FrameHeader
	{
		// Token: 0x06002924 RID: 10532 RVA: 0x000AC314 File Offset: 0x000AB314
		public FrameHeader()
		{
			this._MessageId = 22;
			this._MajorV = 1;
			this._MinorV = 0;
			this._PayloadSize = -1;
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000AC339 File Offset: 0x000AB339
		public FrameHeader(int messageId, int majorV, int minorV)
		{
			this._MessageId = messageId;
			this._MajorV = majorV;
			this._MinorV = minorV;
			this._PayloadSize = -1;
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x000AC35D File Offset: 0x000AB35D
		public int Size
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x000AC360 File Offset: 0x000AB360
		public int MaxMessageSize
		{
			get
			{
				return 65535;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x000AC367 File Offset: 0x000AB367
		// (set) Token: 0x06002929 RID: 10537 RVA: 0x000AC36F File Offset: 0x000AB36F
		public int MessageId
		{
			get
			{
				return this._MessageId;
			}
			set
			{
				this._MessageId = value;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x000AC378 File Offset: 0x000AB378
		public int MajorV
		{
			get
			{
				return this._MajorV;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600292B RID: 10539 RVA: 0x000AC380 File Offset: 0x000AB380
		public int MinorV
		{
			get
			{
				return this._MinorV;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600292C RID: 10540 RVA: 0x000AC388 File Offset: 0x000AB388
		// (set) Token: 0x0600292D RID: 10541 RVA: 0x000AC390 File Offset: 0x000AB390
		public int PayloadSize
		{
			get
			{
				return this._PayloadSize;
			}
			set
			{
				if (value > this.MaxMessageSize)
				{
					throw new ArgumentException(SR.GetString("net_frame_max_size", new object[]
					{
						this.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
						value.ToString(NumberFormatInfo.InvariantInfo)
					}), "PayloadSize");
				}
				this._PayloadSize = value;
			}
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000AC3F0 File Offset: 0x000AB3F0
		public void CopyTo(byte[] dest, int start)
		{
			dest[start++] = (byte)this._MessageId;
			dest[start++] = (byte)this._MajorV;
			dest[start++] = (byte)this._MinorV;
			dest[start++] = (byte)(this._PayloadSize >> 8 & 255);
			dest[start] = (byte)(this._PayloadSize & 255);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000AC454 File Offset: 0x000AB454
		public void CopyFrom(byte[] bytes, int start, FrameHeader verifier)
		{
			this._MessageId = (int)bytes[start++];
			this._MajorV = (int)bytes[start++];
			this._MinorV = (int)bytes[start++];
			this._PayloadSize = ((int)bytes[start++] << 8 | (int)bytes[start]);
			if (verifier.MessageId != -1 && this.MessageId != verifier.MessageId)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[]
				{
					"MessageId",
					this.MessageId,
					verifier.MessageId
				}));
			}
			if (verifier.MajorV != -1 && this.MajorV != verifier.MajorV)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[]
				{
					"MajorV",
					this.MajorV,
					verifier.MajorV
				}));
			}
			if (verifier.MinorV != -1 && this.MinorV != verifier.MinorV)
			{
				throw new InvalidOperationException(SR.GetString("net_io_header_id", new object[]
				{
					"MinorV",
					this.MinorV,
					verifier.MinorV
				}));
			}
		}

		// Token: 0x0400283D RID: 10301
		public const int IgnoreValue = -1;

		// Token: 0x0400283E RID: 10302
		public const int HandshakeDoneId = 20;

		// Token: 0x0400283F RID: 10303
		public const int HandshakeErrId = 21;

		// Token: 0x04002840 RID: 10304
		public const int HandshakeId = 22;

		// Token: 0x04002841 RID: 10305
		public const int DefaultMajorV = 1;

		// Token: 0x04002842 RID: 10306
		public const int DefaultMinorV = 0;

		// Token: 0x04002843 RID: 10307
		private int _MessageId;

		// Token: 0x04002844 RID: 10308
		private int _MajorV;

		// Token: 0x04002845 RID: 10309
		private int _MinorV;

		// Token: 0x04002846 RID: 10310
		private int _PayloadSize;
	}
}
