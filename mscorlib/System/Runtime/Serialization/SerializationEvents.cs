using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000375 RID: 885
	internal class SerializationEvents
	{
		// Token: 0x06002294 RID: 8852 RVA: 0x000572FC File Offset: 0x000562FC
		private List<MethodInfo> GetMethodsWithAttribute(Type attribute, Type t)
		{
			List<MethodInfo> list = new List<MethodInfo>();
			Type type = t;
			while (type != null && type != typeof(object))
			{
				RuntimeType runtimeType = (RuntimeType)type;
				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.IsDefined(attribute, false))
					{
						list.Add(methodInfo);
					}
				}
				type = type.BaseType;
			}
			list.Reverse();
			if (list.Count != 0)
			{
				return list;
			}
			return null;
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00057378 File Offset: 0x00056378
		internal SerializationEvents(Type t)
		{
			this.m_OnSerializingMethods = this.GetMethodsWithAttribute(typeof(OnSerializingAttribute), t);
			this.m_OnSerializedMethods = this.GetMethodsWithAttribute(typeof(OnSerializedAttribute), t);
			this.m_OnDeserializingMethods = this.GetMethodsWithAttribute(typeof(OnDeserializingAttribute), t);
			this.m_OnDeserializedMethods = this.GetMethodsWithAttribute(typeof(OnDeserializedAttribute), t);
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x000573E7 File Offset: 0x000563E7
		internal bool HasOnSerializingEvents
		{
			get
			{
				return this.m_OnSerializingMethods != null || this.m_OnSerializedMethods != null;
			}
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x00057400 File Offset: 0x00056400
		internal void InvokeOnSerializing(object obj, StreamingContext context)
		{
			if (this.m_OnSerializingMethods != null)
			{
				SerializationEventHandler serializationEventHandler = null;
				foreach (MethodInfo method in this.m_OnSerializingMethods)
				{
					SerializationEventHandler b = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, method);
					serializationEventHandler = (SerializationEventHandler)Delegate.Combine(serializationEventHandler, b);
				}
				serializationEventHandler(context);
			}
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00057484 File Offset: 0x00056484
		internal void InvokeOnDeserializing(object obj, StreamingContext context)
		{
			if (this.m_OnDeserializingMethods != null)
			{
				SerializationEventHandler serializationEventHandler = null;
				foreach (MethodInfo method in this.m_OnDeserializingMethods)
				{
					SerializationEventHandler b = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, method);
					serializationEventHandler = (SerializationEventHandler)Delegate.Combine(serializationEventHandler, b);
				}
				serializationEventHandler(context);
			}
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x00057508 File Offset: 0x00056508
		internal void InvokeOnDeserialized(object obj, StreamingContext context)
		{
			if (this.m_OnDeserializedMethods != null)
			{
				SerializationEventHandler serializationEventHandler = null;
				foreach (MethodInfo method in this.m_OnDeserializedMethods)
				{
					SerializationEventHandler b = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, method);
					serializationEventHandler = (SerializationEventHandler)Delegate.Combine(serializationEventHandler, b);
				}
				serializationEventHandler(context);
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0005758C File Offset: 0x0005658C
		internal SerializationEventHandler AddOnSerialized(object obj, SerializationEventHandler handler)
		{
			if (this.m_OnSerializedMethods != null)
			{
				foreach (MethodInfo method in this.m_OnSerializedMethods)
				{
					SerializationEventHandler b = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, method);
					handler = (SerializationEventHandler)Delegate.Combine(handler, b);
				}
			}
			return handler;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00057608 File Offset: 0x00056608
		internal SerializationEventHandler AddOnDeserialized(object obj, SerializationEventHandler handler)
		{
			if (this.m_OnDeserializedMethods != null)
			{
				foreach (MethodInfo method in this.m_OnDeserializedMethods)
				{
					SerializationEventHandler b = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, method);
					handler = (SerializationEventHandler)Delegate.Combine(handler, b);
				}
			}
			return handler;
		}

		// Token: 0x04000E90 RID: 3728
		private List<MethodInfo> m_OnSerializingMethods;

		// Token: 0x04000E91 RID: 3729
		private List<MethodInfo> m_OnSerializedMethods;

		// Token: 0x04000E92 RID: 3730
		private List<MethodInfo> m_OnDeserializingMethods;

		// Token: 0x04000E93 RID: 3731
		private List<MethodInfo> m_OnDeserializedMethods;
	}
}
