using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200035C RID: 860
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class Formatter : IFormatter
	{
		// Token: 0x060021B0 RID: 8624 RVA: 0x0005431F File Offset: 0x0005331F
		protected Formatter()
		{
			this.m_objectQueue = new Queue();
			this.m_idGenerator = new ObjectIDGenerator();
		}

		// Token: 0x060021B1 RID: 8625
		public abstract object Deserialize(Stream serializationStream);

		// Token: 0x060021B2 RID: 8626 RVA: 0x00054340 File Offset: 0x00053340
		protected virtual object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			bool flag;
			objID = this.m_idGenerator.HasId(obj, out flag);
			if (flag)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_NoID"));
			}
			return obj;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00054390 File Offset: 0x00053390
		protected virtual long Schedule(object obj)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long id = this.m_idGenerator.GetId(obj, out flag);
			if (flag)
			{
				this.m_objectQueue.Enqueue(obj);
			}
			return id;
		}

		// Token: 0x060021B4 RID: 8628
		public abstract void Serialize(Stream serializationStream, object graph);

		// Token: 0x060021B5 RID: 8629
		protected abstract void WriteArray(object obj, string name, Type memberType);

		// Token: 0x060021B6 RID: 8630
		protected abstract void WriteBoolean(bool val, string name);

		// Token: 0x060021B7 RID: 8631
		protected abstract void WriteByte(byte val, string name);

		// Token: 0x060021B8 RID: 8632
		protected abstract void WriteChar(char val, string name);

		// Token: 0x060021B9 RID: 8633
		protected abstract void WriteDateTime(DateTime val, string name);

		// Token: 0x060021BA RID: 8634
		protected abstract void WriteDecimal(decimal val, string name);

		// Token: 0x060021BB RID: 8635
		protected abstract void WriteDouble(double val, string name);

		// Token: 0x060021BC RID: 8636
		protected abstract void WriteInt16(short val, string name);

		// Token: 0x060021BD RID: 8637
		protected abstract void WriteInt32(int val, string name);

		// Token: 0x060021BE RID: 8638
		protected abstract void WriteInt64(long val, string name);

		// Token: 0x060021BF RID: 8639
		protected abstract void WriteObjectRef(object obj, string name, Type memberType);

		// Token: 0x060021C0 RID: 8640 RVA: 0x000543C4 File Offset: 0x000533C4
		protected virtual void WriteMember(string memberName, object data)
		{
			if (data == null)
			{
				this.WriteObjectRef(data, memberName, typeof(object));
				return;
			}
			Type type = data.GetType();
			if (type == typeof(bool))
			{
				this.WriteBoolean(Convert.ToBoolean(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(char))
			{
				this.WriteChar(Convert.ToChar(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(sbyte))
			{
				this.WriteSByte(Convert.ToSByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(byte))
			{
				this.WriteByte(Convert.ToByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(short))
			{
				this.WriteInt16(Convert.ToInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(int))
			{
				this.WriteInt32(Convert.ToInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(long))
			{
				this.WriteInt64(Convert.ToInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(float))
			{
				this.WriteSingle(Convert.ToSingle(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(double))
			{
				this.WriteDouble(Convert.ToDouble(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(DateTime))
			{
				this.WriteDateTime(Convert.ToDateTime(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(decimal))
			{
				this.WriteDecimal(Convert.ToDecimal(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ushort))
			{
				this.WriteUInt16(Convert.ToUInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(uint))
			{
				this.WriteUInt32(Convert.ToUInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ulong))
			{
				this.WriteUInt64(Convert.ToUInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type.IsArray)
			{
				this.WriteArray(data, memberName, type);
				return;
			}
			if (type.IsValueType)
			{
				this.WriteValueType(data, memberName, type);
				return;
			}
			this.WriteObjectRef(data, memberName, type);
		}

		// Token: 0x060021C1 RID: 8641
		[CLSCompliant(false)]
		protected abstract void WriteSByte(sbyte val, string name);

		// Token: 0x060021C2 RID: 8642
		protected abstract void WriteSingle(float val, string name);

		// Token: 0x060021C3 RID: 8643
		protected abstract void WriteTimeSpan(TimeSpan val, string name);

		// Token: 0x060021C4 RID: 8644
		[CLSCompliant(false)]
		protected abstract void WriteUInt16(ushort val, string name);

		// Token: 0x060021C5 RID: 8645
		[CLSCompliant(false)]
		protected abstract void WriteUInt32(uint val, string name);

		// Token: 0x060021C6 RID: 8646
		[CLSCompliant(false)]
		protected abstract void WriteUInt64(ulong val, string name);

		// Token: 0x060021C7 RID: 8647
		protected abstract void WriteValueType(object obj, string name, Type memberType);

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060021C8 RID: 8648
		// (set) Token: 0x060021C9 RID: 8649
		public abstract ISurrogateSelector SurrogateSelector { get; set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060021CA RID: 8650
		// (set) Token: 0x060021CB RID: 8651
		public abstract SerializationBinder Binder { get; set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060021CC RID: 8652
		// (set) Token: 0x060021CD RID: 8653
		public abstract StreamingContext Context { get; set; }

		// Token: 0x04000E3D RID: 3645
		protected ObjectIDGenerator m_idGenerator;

		// Token: 0x04000E3E RID: 3646
		protected Queue m_objectQueue;
	}
}
