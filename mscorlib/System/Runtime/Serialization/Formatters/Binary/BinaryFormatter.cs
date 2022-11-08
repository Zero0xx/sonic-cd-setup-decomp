using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CF RID: 1999
	[ComVisible(true)]
	public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
	{
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600469E RID: 18078 RVA: 0x000F0A7C File Offset: 0x000EFA7C
		// (set) Token: 0x0600469F RID: 18079 RVA: 0x000F0A84 File Offset: 0x000EFA84
		public FormatterTypeStyle TypeFormat
		{
			get
			{
				return this.m_typeFormat;
			}
			set
			{
				this.m_typeFormat = value;
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x000F0A8D File Offset: 0x000EFA8D
		// (set) Token: 0x060046A1 RID: 18081 RVA: 0x000F0A95 File Offset: 0x000EFA95
		public FormatterAssemblyStyle AssemblyFormat
		{
			get
			{
				return this.m_assemblyFormat;
			}
			set
			{
				this.m_assemblyFormat = value;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x000F0A9E File Offset: 0x000EFA9E
		// (set) Token: 0x060046A3 RID: 18083 RVA: 0x000F0AA6 File Offset: 0x000EFAA6
		public TypeFilterLevel FilterLevel
		{
			get
			{
				return this.m_securityLevel;
			}
			set
			{
				this.m_securityLevel = value;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x000F0AAF File Offset: 0x000EFAAF
		// (set) Token: 0x060046A5 RID: 18085 RVA: 0x000F0AB7 File Offset: 0x000EFAB7
		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				return this.m_surrogates;
			}
			set
			{
				this.m_surrogates = value;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x060046A6 RID: 18086 RVA: 0x000F0AC0 File Offset: 0x000EFAC0
		// (set) Token: 0x060046A7 RID: 18087 RVA: 0x000F0AC8 File Offset: 0x000EFAC8
		public SerializationBinder Binder
		{
			get
			{
				return this.m_binder;
			}
			set
			{
				this.m_binder = value;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x060046A8 RID: 18088 RVA: 0x000F0AD1 File Offset: 0x000EFAD1
		// (set) Token: 0x060046A9 RID: 18089 RVA: 0x000F0AD9 File Offset: 0x000EFAD9
		public StreamingContext Context
		{
			get
			{
				return this.m_context;
			}
			set
			{
				this.m_context = value;
			}
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x000F0AE2 File Offset: 0x000EFAE2
		public BinaryFormatter()
		{
			this.m_surrogates = null;
			this.m_context = new StreamingContext(StreamingContextStates.All);
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x000F0B0F File Offset: 0x000EFB0F
		public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
		{
			this.m_surrogates = selector;
			this.m_context = context;
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x000F0B33 File Offset: 0x000EFB33
		public object Deserialize(Stream serializationStream)
		{
			return this.Deserialize(serializationStream, null);
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x000F0B3D File Offset: 0x000EFB3D
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck)
		{
			return this.Deserialize(serializationStream, null, fCheck, null);
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x000F0B49 File Offset: 0x000EFB49
		public object Deserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.Deserialize(serializationStream, handler, true, null);
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x000F0B55 File Offset: 0x000EFB55
		public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, true, methodCallMessage);
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x000F0B61 File Offset: 0x000EFB61
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.Deserialize(serializationStream, handler, false, null);
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x000F0B6D File Offset: 0x000EFB6D
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, false, methodCallMessage);
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x000F0B79 File Offset: 0x000EFB79
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, fCheck, false, methodCallMessage);
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x000F0B88 File Offset: 0x000EFB88
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, bool isCrossAppDomain, IMethodCallMessage methodCallMessage)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_WithParamName"), new object[]
				{
					serializationStream
				}));
			}
			if (serializationStream.CanSeek && serializationStream.Length == 0L)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_Stream"));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			internalFE.FEsecurityLevel = this.m_securityLevel;
			ObjectReader objectReader = new ObjectReader(serializationStream, this.m_surrogates, this.m_context, internalFE, this.m_binder);
			objectReader.crossAppDomainArray = this.m_crossAppDomainArray;
			return objectReader.Deserialize(handler, new __BinaryParser(serializationStream, objectReader), fCheck, isCrossAppDomain, methodCallMessage);
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x000F0C4F File Offset: 0x000EFC4F
		public void Serialize(Stream serializationStream, object graph)
		{
			this.Serialize(serializationStream, graph, null);
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x000F0C5A File Offset: 0x000EFC5A
		public void Serialize(Stream serializationStream, object graph, Header[] headers)
		{
			this.Serialize(serializationStream, graph, headers, true);
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x000F0C68 File Offset: 0x000EFC68
		internal void Serialize(Stream serializationStream, object graph, Header[] headers, bool fCheck)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_WithParamName"), new object[]
				{
					serializationStream
				}));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			ObjectWriter objectWriter = new ObjectWriter(this.m_surrogates, this.m_context, internalFE);
			__BinaryWriter serWriter = new __BinaryWriter(serializationStream, objectWriter, this.m_typeFormat);
			objectWriter.Serialize(graph, headers, serWriter, fCheck);
			this.m_crossAppDomainArray = objectWriter.crossAppDomainArray;
		}

		// Token: 0x040023B2 RID: 9138
		internal ISurrogateSelector m_surrogates;

		// Token: 0x040023B3 RID: 9139
		internal StreamingContext m_context;

		// Token: 0x040023B4 RID: 9140
		internal SerializationBinder m_binder;

		// Token: 0x040023B5 RID: 9141
		internal FormatterTypeStyle m_typeFormat = FormatterTypeStyle.TypesAlways;

		// Token: 0x040023B6 RID: 9142
		internal FormatterAssemblyStyle m_assemblyFormat;

		// Token: 0x040023B7 RID: 9143
		internal TypeFilterLevel m_securityLevel = TypeFilterLevel.Full;

		// Token: 0x040023B8 RID: 9144
		internal object[] m_crossAppDomainArray;
	}
}
