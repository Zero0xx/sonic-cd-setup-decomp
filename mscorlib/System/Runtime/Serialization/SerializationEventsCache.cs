using System;
using System.Collections;

namespace System.Runtime.Serialization
{
	// Token: 0x02000376 RID: 886
	internal static class SerializationEventsCache
	{
		// Token: 0x0600229C RID: 8860 RVA: 0x00057684 File Offset: 0x00056684
		internal static SerializationEvents GetSerializationEventsForType(Type t)
		{
			SerializationEvents serializationEvents;
			if ((serializationEvents = (SerializationEvents)SerializationEventsCache.cache[t]) == null)
			{
				lock (SerializationEventsCache.cache.SyncRoot)
				{
					if ((serializationEvents = (SerializationEvents)SerializationEventsCache.cache[t]) == null)
					{
						serializationEvents = new SerializationEvents(t);
						SerializationEventsCache.cache[t] = serializationEvents;
					}
				}
			}
			return serializationEvents;
		}

		// Token: 0x04000E94 RID: 3732
		private static Hashtable cache = new Hashtable();
	}
}
