using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x02000412 RID: 1042
	[Serializable]
	internal sealed class MLangCodePageEncoding : ISerializable, IObjectReference
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x00087C8C File Offset: 0x00086C8C
		internal MLangCodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_codePage = (int)info.GetValue("m_codePage", typeof(int));
			try
			{
				this.m_isReadOnly = (bool)info.GetValue("m_isReadOnly", typeof(bool));
				this.encoderFallback = (EncoderFallback)info.GetValue("encoderFallback", typeof(EncoderFallback));
				this.decoderFallback = (DecoderFallback)info.GetValue("decoderFallback", typeof(DecoderFallback));
			}
			catch (SerializationException)
			{
				this.m_deserializedFromEverett = true;
				this.m_isReadOnly = true;
			}
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x00087D50 File Offset: 0x00086D50
		public object GetRealObject(StreamingContext context)
		{
			this.realEncoding = Encoding.GetEncoding(this.m_codePage);
			if (!this.m_deserializedFromEverett && !this.m_isReadOnly)
			{
				this.realEncoding = (Encoding)this.realEncoding.Clone();
				this.realEncoding.EncoderFallback = this.encoderFallback;
				this.realEncoding.DecoderFallback = this.decoderFallback;
			}
			return this.realEncoding;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x00087DBC File Offset: 0x00086DBC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
		}

		// Token: 0x040014D7 RID: 5335
		[NonSerialized]
		private int m_codePage;

		// Token: 0x040014D8 RID: 5336
		[NonSerialized]
		private bool m_isReadOnly;

		// Token: 0x040014D9 RID: 5337
		[NonSerialized]
		private bool m_deserializedFromEverett;

		// Token: 0x040014DA RID: 5338
		[NonSerialized]
		private EncoderFallback encoderFallback;

		// Token: 0x040014DB RID: 5339
		[NonSerialized]
		private DecoderFallback decoderFallback;

		// Token: 0x040014DC RID: 5340
		[NonSerialized]
		private Encoding realEncoding;

		// Token: 0x02000413 RID: 1043
		[Serializable]
		internal sealed class MLangEncoder : ISerializable, IObjectReference
		{
			// Token: 0x06002AAE RID: 10926 RVA: 0x00087DCD File Offset: 0x00086DCD
			internal MLangEncoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.realEncoding = (Encoding)info.GetValue("m_encoding", typeof(Encoding));
			}

			// Token: 0x06002AAF RID: 10927 RVA: 0x00087E03 File Offset: 0x00086E03
			public object GetRealObject(StreamingContext context)
			{
				return this.realEncoding.GetEncoder();
			}

			// Token: 0x06002AB0 RID: 10928 RVA: 0x00087E10 File Offset: 0x00086E10
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
			}

			// Token: 0x040014DD RID: 5341
			[NonSerialized]
			private Encoding realEncoding;
		}

		// Token: 0x02000414 RID: 1044
		[Serializable]
		internal sealed class MLangDecoder : ISerializable, IObjectReference
		{
			// Token: 0x06002AB1 RID: 10929 RVA: 0x00087E21 File Offset: 0x00086E21
			internal MLangDecoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.realEncoding = (Encoding)info.GetValue("m_encoding", typeof(Encoding));
			}

			// Token: 0x06002AB2 RID: 10930 RVA: 0x00087E57 File Offset: 0x00086E57
			public object GetRealObject(StreamingContext context)
			{
				return this.realEncoding.GetDecoder();
			}

			// Token: 0x06002AB3 RID: 10931 RVA: 0x00087E64 File Offset: 0x00086E64
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
			}

			// Token: 0x040014DE RID: 5342
			[NonSerialized]
			private Encoding realEncoding;
		}
	}
}
