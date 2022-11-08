using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x020003F7 RID: 1015
	[Serializable]
	internal sealed class CodePageEncoding : ISerializable, IObjectReference
	{
		// Token: 0x060029DB RID: 10715 RVA: 0x00082910 File Offset: 0x00081910
		internal CodePageEncoding(SerializationInfo info, StreamingContext context)
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

		// Token: 0x060029DC RID: 10716 RVA: 0x000829D4 File Offset: 0x000819D4
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

		// Token: 0x060029DD RID: 10717 RVA: 0x00082A40 File Offset: 0x00081A40
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
		}

		// Token: 0x04001470 RID: 5232
		[NonSerialized]
		private int m_codePage;

		// Token: 0x04001471 RID: 5233
		[NonSerialized]
		private bool m_isReadOnly;

		// Token: 0x04001472 RID: 5234
		[NonSerialized]
		private bool m_deserializedFromEverett;

		// Token: 0x04001473 RID: 5235
		[NonSerialized]
		private EncoderFallback encoderFallback;

		// Token: 0x04001474 RID: 5236
		[NonSerialized]
		private DecoderFallback decoderFallback;

		// Token: 0x04001475 RID: 5237
		[NonSerialized]
		private Encoding realEncoding;

		// Token: 0x020003F8 RID: 1016
		[Serializable]
		internal sealed class Decoder : ISerializable, IObjectReference
		{
			// Token: 0x060029DE RID: 10718 RVA: 0x00082A51 File Offset: 0x00081A51
			internal Decoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.realEncoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			}

			// Token: 0x060029DF RID: 10719 RVA: 0x00082A87 File Offset: 0x00081A87
			public object GetRealObject(StreamingContext context)
			{
				return this.realEncoding.GetDecoder();
			}

			// Token: 0x060029E0 RID: 10720 RVA: 0x00082A94 File Offset: 0x00081A94
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
			}

			// Token: 0x04001476 RID: 5238
			[NonSerialized]
			private Encoding realEncoding;
		}
	}
}
