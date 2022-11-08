using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036D RID: 877
	public sealed class SerializationObjectManager
	{
		// Token: 0x06002284 RID: 8836 RVA: 0x00057171 File Offset: 0x00056171
		public SerializationObjectManager(StreamingContext context)
		{
			this.m_context = context;
			this.m_objectSeenTable = new Hashtable();
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00057198 File Offset: 0x00056198
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void RegisterObject(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			if (serializationEventsForType.HasOnSerializingEvents && this.m_objectSeenTable[obj] == null)
			{
				this.m_objectSeenTable[obj] = true;
				serializationEventsForType.InvokeOnSerializing(obj, this.m_context);
				this.AddOnSerialized(obj);
			}
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000571ED File Offset: 0x000561ED
		public void RaiseOnSerializedEvent()
		{
			if (this.m_onSerializedHandler != null)
			{
				this.m_onSerializedHandler(this.m_context);
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00057208 File Offset: 0x00056208
		private void AddOnSerialized(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			this.m_onSerializedHandler = serializationEventsForType.AddOnSerialized(obj, this.m_onSerializedHandler);
		}

		// Token: 0x04000E89 RID: 3721
		private Hashtable m_objectSeenTable = new Hashtable();

		// Token: 0x04000E8A RID: 3722
		private SerializationEventHandler m_onSerializedHandler;

		// Token: 0x04000E8B RID: 3723
		private StreamingContext m_context;
	}
}
