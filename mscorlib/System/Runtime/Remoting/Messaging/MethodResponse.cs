using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200071E RID: 1822
	[CLSCompliant(false)]
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class MethodResponse : IMethodReturnMessage, IMethodMessage, IMessage, ISerializable, ISerializationRootObject, IInternalMessage
	{
		// Token: 0x06004138 RID: 16696 RVA: 0x000DE150 File Offset: 0x000DD150
		public MethodResponse(Header[] h1, IMethodCallMessage mcm)
		{
			if (mcm == null)
			{
				throw new ArgumentNullException("mcm");
			}
			Message message = mcm as Message;
			if (message != null)
			{
				this.MI = message.GetMethodBase();
			}
			else
			{
				this.MI = mcm.MethodBase;
			}
			if (this.MI == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[]
				{
					mcm.MethodName,
					mcm.TypeName
				}));
			}
			this._methodCache = InternalRemotingServices.GetReflectionCachedData(this.MI);
			this.argCount = this._methodCache.Parameters.Length;
			this.fSoap = true;
			this.FillHeaders(h1);
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x000DE204 File Offset: 0x000DD204
		internal MethodResponse(IMethodCallMessage msg, SmuggledMethodReturnMessage smuggledMrm, ArrayList deserializedArgs)
		{
			this.MI = msg.MethodBase;
			this._methodCache = InternalRemotingServices.GetReflectionCachedData(this.MI);
			this.methodName = msg.MethodName;
			this.uri = msg.Uri;
			this.typeName = msg.TypeName;
			if (this._methodCache.IsOverloaded())
			{
				this.methodSignature = (Type[])msg.MethodSignature;
			}
			this.retVal = smuggledMrm.GetReturnValue(deserializedArgs);
			this.outArgs = smuggledMrm.GetArgs(deserializedArgs);
			this.fault = smuggledMrm.GetException(deserializedArgs);
			this.callContext = smuggledMrm.GetCallContext(deserializedArgs);
			if (smuggledMrm.MessagePropertyCount > 0)
			{
				smuggledMrm.PopulateMessageProperties(this.Properties, deserializedArgs);
			}
			this.argCount = this._methodCache.Parameters.Length;
			this.fSoap = false;
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x000DE2DC File Offset: 0x000DD2DC
		internal MethodResponse(IMethodCallMessage msg, object handlerObject, BinaryMethodReturnMessage smuggledMrm)
		{
			if (msg != null)
			{
				this.MI = msg.MethodBase;
				this._methodCache = InternalRemotingServices.GetReflectionCachedData(this.MI);
				this.methodName = msg.MethodName;
				this.uri = msg.Uri;
				this.typeName = msg.TypeName;
				if (this._methodCache.IsOverloaded())
				{
					this.methodSignature = (Type[])msg.MethodSignature;
				}
				this.argCount = this._methodCache.Parameters.Length;
			}
			this.retVal = smuggledMrm.ReturnValue;
			this.outArgs = smuggledMrm.Args;
			this.fault = smuggledMrm.Exception;
			this.callContext = smuggledMrm.LogicalCallContext;
			if (smuggledMrm.HasProperties)
			{
				smuggledMrm.PopulateMessageProperties(this.Properties);
			}
			this.fSoap = false;
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x000DE3AF File Offset: 0x000DD3AF
		internal MethodResponse(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.SetObjectData(info, context);
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x000DE3D0 File Offset: 0x000DD3D0
		public virtual object HeaderHandler(Header[] h)
		{
			SerializationMonkey serializationMonkey = (SerializationMonkey)FormatterServices.GetUninitializedObject(typeof(SerializationMonkey));
			Header[] array;
			if (h != null && h.Length > 0 && h[0].Name == "__methodName")
			{
				if (h.Length > 1)
				{
					array = new Header[h.Length - 1];
					Array.Copy(h, 1, array, 0, h.Length - 1);
				}
				else
				{
					array = null;
				}
			}
			else
			{
				array = h;
			}
			Type type = null;
			MethodInfo methodInfo = this.MI as MethodInfo;
			if (methodInfo != null)
			{
				type = methodInfo.ReturnType;
			}
			ParameterInfo[] parameters = this._methodCache.Parameters;
			int num = this._methodCache.MarshalResponseArgMap.Length;
			if (type != null && type != typeof(void))
			{
				num++;
			}
			Type[] array2 = new Type[num];
			string[] array3 = new string[num];
			int num2 = 0;
			if (type != null && type != typeof(void))
			{
				array2[num2++] = type;
			}
			foreach (int num3 in this._methodCache.MarshalResponseArgMap)
			{
				array3[num2] = parameters[num3].Name;
				if (parameters[num3].ParameterType.IsByRef)
				{
					array2[num2++] = parameters[num3].ParameterType.GetElementType();
				}
				else
				{
					array2[num2++] = parameters[num3].ParameterType;
				}
			}
			((IFieldInfo)serializationMonkey).FieldTypes = array2;
			((IFieldInfo)serializationMonkey).FieldNames = array3;
			this.FillHeaders(array, true);
			serializationMonkey._obj = this;
			return serializationMonkey;
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x000DE548 File Offset: 0x000DD548
		public void RootSetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			this.SetObjectData(info, ctx);
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x000DE554 File Offset: 0x000DD554
		internal void SetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.fSoap)
			{
				this.SetObjectFromSoapData(info);
				return;
			}
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			bool flag = false;
			bool flag2 = false;
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("__return"))
				{
					flag = true;
					break;
				}
				if (enumerator.Name.Equals("__fault"))
				{
					flag2 = true;
					this.fault = (Exception)enumerator.Value;
					break;
				}
				this.FillHeader(enumerator.Name, enumerator.Value);
			}
			if (flag2 && flag)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
			}
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x000DE5F9 File Offset: 0x000DD5F9
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x000DE60C File Offset: 0x000DD60C
		internal void SetObjectFromSoapData(SerializationInfo info)
		{
			Hashtable keyToNamespaceTable = (Hashtable)info.GetValue("__keyToNamespaceTable", typeof(Hashtable));
			ArrayList arrayList = (ArrayList)info.GetValue("__paramNameList", typeof(ArrayList));
			SoapFault soapFault = (SoapFault)info.GetValue("__fault", typeof(SoapFault));
			if (soapFault != null)
			{
				ServerFault serverFault = soapFault.Detail as ServerFault;
				if (serverFault != null)
				{
					if (serverFault.Exception != null)
					{
						this.fault = serverFault.Exception;
						return;
					}
					Type type = Type.GetType(serverFault.ExceptionType, false, false);
					if (type == null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("\nException Type: ");
						stringBuilder.Append(serverFault.ExceptionType);
						stringBuilder.Append("\n");
						stringBuilder.Append("Exception Message: ");
						stringBuilder.Append(serverFault.ExceptionMessage);
						stringBuilder.Append("\n");
						stringBuilder.Append(serverFault.StackTrace);
						this.fault = new ServerException(stringBuilder.ToString());
						return;
					}
					object[] args = new object[]
					{
						serverFault.ExceptionMessage
					};
					this.fault = (Exception)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null, null);
					return;
				}
				else
				{
					if (soapFault.Detail != null && soapFault.Detail.GetType() == typeof(string) && ((string)soapFault.Detail).Length != 0)
					{
						this.fault = new ServerException((string)soapFault.Detail);
						return;
					}
					this.fault = new ServerException(soapFault.FaultString);
					return;
				}
			}
			else
			{
				MethodInfo methodInfo = this.MI as MethodInfo;
				int num = 0;
				if (methodInfo != null)
				{
					Type returnType = methodInfo.ReturnType;
					if (returnType != typeof(void))
					{
						num++;
						object value = info.GetValue((string)arrayList[0], typeof(object));
						if (value is string)
						{
							this.retVal = Message.SoapCoerceArg(value, returnType, keyToNamespaceTable);
						}
						else
						{
							this.retVal = value;
						}
					}
				}
				ParameterInfo[] parameters = this._methodCache.Parameters;
				object obj = (this.InternalProperties == null) ? null : this.InternalProperties["__UnorderedParams"];
				if (obj != null && obj is bool && (bool)obj)
				{
					for (int i = num; i < arrayList.Count; i++)
					{
						string text = (string)arrayList[i];
						int num2 = -1;
						for (int j = 0; j < parameters.Length; j++)
						{
							if (text.Equals(parameters[j].Name))
							{
								num2 = parameters[j].Position;
							}
						}
						if (num2 == -1)
						{
							if (!text.StartsWith("__param", StringComparison.Ordinal))
							{
								throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
							}
							num2 = int.Parse(text.Substring(7), CultureInfo.InvariantCulture);
						}
						if (num2 >= this.argCount)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
						}
						if (this.outArgs == null)
						{
							this.outArgs = new object[this.argCount];
						}
						this.outArgs[num2] = Message.SoapCoerceArg(info.GetValue(text, typeof(object)), parameters[num2].ParameterType, keyToNamespaceTable);
					}
					return;
				}
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, true);
				}
				for (int k = num; k < arrayList.Count; k++)
				{
					string name = (string)arrayList[k];
					if (this.outArgs == null)
					{
						this.outArgs = new object[this.argCount];
					}
					int num3 = this.argMapper.Map[k - num];
					this.outArgs[num3] = Message.SoapCoerceArg(info.GetValue(name, typeof(object)), parameters[num3].ParameterType, keyToNamespaceTable);
				}
				return;
			}
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x000DE9F2 File Offset: 0x000DD9F2
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this.callContext == null)
			{
				this.callContext = new LogicalCallContext();
			}
			return this.callContext;
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x000DEA10 File Offset: 0x000DDA10
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			LogicalCallContext result = this.callContext;
			this.callContext = ctx;
			return result;
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004143 RID: 16707 RVA: 0x000DEA2C File Offset: 0x000DDA2C
		// (set) Token: 0x06004144 RID: 16708 RVA: 0x000DEA34 File Offset: 0x000DDA34
		public string Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004145 RID: 16709 RVA: 0x000DEA3D File Offset: 0x000DDA3D
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06004146 RID: 16710 RVA: 0x000DEA45 File Offset: 0x000DDA45
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x000DEA4D File Offset: 0x000DDA4D
		public object MethodSignature
		{
			get
			{
				return this.methodSignature;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06004148 RID: 16712 RVA: 0x000DEA55 File Offset: 0x000DDA55
		public MethodBase MethodBase
		{
			get
			{
				return this.MI;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06004149 RID: 16713 RVA: 0x000DEA5D File Offset: 0x000DDA5D
		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x0600414A RID: 16714 RVA: 0x000DEA60 File Offset: 0x000DDA60
		public int ArgCount
		{
			get
			{
				if (this.outArgs == null)
				{
					return 0;
				}
				return this.outArgs.Length;
			}
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x000DEA74 File Offset: 0x000DDA74
		public object GetArg(int argNum)
		{
			return this.outArgs[argNum];
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x000DEA80 File Offset: 0x000DDA80
		public string GetArgName(int index)
		{
			if (this.MI == null)
			{
				return "__param" + index;
			}
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.MI);
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			if (index < 0 || index >= parameters.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return reflectionCachedData.Parameters[index].Name;
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x000DEADB File Offset: 0x000DDADB
		public object[] Args
		{
			get
			{
				return this.outArgs;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600414E RID: 16718 RVA: 0x000DEAE3 File Offset: 0x000DDAE3
		public int OutArgCount
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, true);
				}
				return this.argMapper.ArgCount;
			}
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x000DEB05 File Offset: 0x000DDB05
		public object GetOutArg(int argNum)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, true);
			}
			return this.argMapper.GetArg(argNum);
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x000DEB28 File Offset: 0x000DDB28
		public string GetOutArgName(int index)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, true);
			}
			return this.argMapper.GetArgName(index);
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x000DEB4B File Offset: 0x000DDB4B
		public object[] OutArgs
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, true);
				}
				return this.argMapper.Args;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x000DEB6D File Offset: 0x000DDB6D
		public Exception Exception
		{
			get
			{
				return this.fault;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x000DEB75 File Offset: 0x000DDB75
		public object ReturnValue
		{
			get
			{
				return this.retVal;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004154 RID: 16724 RVA: 0x000DEB80 File Offset: 0x000DDB80
		public virtual IDictionary Properties
		{
			get
			{
				IDictionary externalProperties;
				lock (this)
				{
					if (this.InternalProperties == null)
					{
						this.InternalProperties = new Hashtable();
					}
					if (this.ExternalProperties == null)
					{
						this.ExternalProperties = new MRMDictionary(this, this.InternalProperties);
					}
					externalProperties = this.ExternalProperties;
				}
				return externalProperties;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004155 RID: 16725 RVA: 0x000DEBE4 File Offset: 0x000DDBE4
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x000DEBEC File Offset: 0x000DDBEC
		internal void FillHeaders(Header[] h)
		{
			this.FillHeaders(h, false);
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x000DEBF8 File Offset: 0x000DDBF8
		private void FillHeaders(Header[] h, bool bFromHeaderHandler)
		{
			if (h == null)
			{
				return;
			}
			if (bFromHeaderHandler && this.fSoap)
			{
				foreach (Header header in h)
				{
					if (header.HeaderNamespace == "http://schemas.microsoft.com/clr/soap/messageProperties")
					{
						this.FillHeader(header.Name, header.Value);
					}
					else
					{
						string propertyKeyForHeader = LogicalCallContext.GetPropertyKeyForHeader(header);
						this.FillHeader(propertyKeyForHeader, header);
					}
				}
				return;
			}
			for (int j = 0; j < h.Length; j++)
			{
				this.FillHeader(h[j].Name, h[j].Value);
			}
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x000DEC80 File Offset: 0x000DDC80
		internal void FillHeader(string name, object value)
		{
			if (name.Equals("__MethodName"))
			{
				this.methodName = (string)value;
				return;
			}
			if (name.Equals("__Uri"))
			{
				this.uri = (string)value;
				return;
			}
			if (name.Equals("__MethodSignature"))
			{
				this.methodSignature = (Type[])value;
				return;
			}
			if (name.Equals("__TypeName"))
			{
				this.typeName = (string)value;
				return;
			}
			if (name.Equals("__OutArgs"))
			{
				this.outArgs = (object[])value;
				return;
			}
			if (name.Equals("__CallContext"))
			{
				if (value is string)
				{
					this.callContext = new LogicalCallContext();
					this.callContext.RemotingData.LogicalCallID = (string)value;
					return;
				}
				this.callContext = (LogicalCallContext)value;
				return;
			}
			else
			{
				if (name.Equals("__Return"))
				{
					this.retVal = value;
					return;
				}
				if (this.InternalProperties == null)
				{
					this.InternalProperties = new Hashtable();
				}
				this.InternalProperties[name] = value;
				return;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06004159 RID: 16729 RVA: 0x000DED88 File Offset: 0x000DDD88
		// (set) Token: 0x0600415A RID: 16730 RVA: 0x000DED8B File Offset: 0x000DDD8B
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600415B RID: 16731 RVA: 0x000DED8D File Offset: 0x000DDD8D
		// (set) Token: 0x0600415C RID: 16732 RVA: 0x000DED90 File Offset: 0x000DDD90
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x000DED92 File Offset: 0x000DDD92
		void IInternalMessage.SetURI(string val)
		{
			this.uri = val;
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x000DED9B File Offset: 0x000DDD9B
		void IInternalMessage.SetCallContext(LogicalCallContext newCallContext)
		{
			this.callContext = newCallContext;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x000DEDA4 File Offset: 0x000DDDA4
		bool IInternalMessage.HasProperties()
		{
			return this.ExternalProperties != null || this.InternalProperties != null;
		}

		// Token: 0x040020D2 RID: 8402
		private MethodBase MI;

		// Token: 0x040020D3 RID: 8403
		private string methodName;

		// Token: 0x040020D4 RID: 8404
		private Type[] methodSignature;

		// Token: 0x040020D5 RID: 8405
		private string uri;

		// Token: 0x040020D6 RID: 8406
		private string typeName;

		// Token: 0x040020D7 RID: 8407
		private object retVal;

		// Token: 0x040020D8 RID: 8408
		private Exception fault;

		// Token: 0x040020D9 RID: 8409
		private object[] outArgs;

		// Token: 0x040020DA RID: 8410
		private LogicalCallContext callContext;

		// Token: 0x040020DB RID: 8411
		protected IDictionary InternalProperties;

		// Token: 0x040020DC RID: 8412
		protected IDictionary ExternalProperties;

		// Token: 0x040020DD RID: 8413
		private int argCount;

		// Token: 0x040020DE RID: 8414
		private bool fSoap;

		// Token: 0x040020DF RID: 8415
		private ArgMapper argMapper;

		// Token: 0x040020E0 RID: 8416
		private RemotingMethodCachedData _methodCache;
	}
}
