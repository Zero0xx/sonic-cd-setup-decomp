using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200071C RID: 1820
	[ComVisible(true)]
	[CLSCompliant(false)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class MethodCall : IMethodCallMessage, IMethodMessage, IMessage, ISerializable, IInternalMessage, ISerializationRootObject
	{
		// Token: 0x060040FF RID: 16639 RVA: 0x000DD071 File Offset: 0x000DC071
		public MethodCall(Header[] h1)
		{
			this.Init();
			this.fSoap = true;
			this.FillHeaders(h1);
			this.ResolveMethod();
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x000DD094 File Offset: 0x000DC094
		public MethodCall(IMessage msg)
		{
			if (msg == null)
			{
				throw new ArgumentNullException("msg");
			}
			this.Init();
			IDictionaryEnumerator enumerator = msg.Properties.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.FillHeader(enumerator.Key.ToString(), enumerator.Value);
			}
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			if (methodCallMessage != null)
			{
				this.MI = methodCallMessage.MethodBase;
			}
			this.ResolveMethod();
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x000DD104 File Offset: 0x000DC104
		internal MethodCall(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.Init();
			this.SetObjectData(info, context);
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x000DD128 File Offset: 0x000DC128
		internal MethodCall(SmuggledMethodCallMessage smuggledMsg, ArrayList deserializedArgs)
		{
			this.uri = smuggledMsg.Uri;
			this.typeName = smuggledMsg.TypeName;
			this.methodName = smuggledMsg.MethodName;
			this.methodSignature = (Type[])smuggledMsg.GetMethodSignature(deserializedArgs);
			this.args = smuggledMsg.GetArgs(deserializedArgs);
			this.instArgs = smuggledMsg.GetInstantiation(deserializedArgs);
			this.callContext = smuggledMsg.GetCallContext(deserializedArgs);
			this.ResolveMethod();
			if (smuggledMsg.MessagePropertyCount > 0)
			{
				smuggledMsg.PopulateMessageProperties(this.Properties, deserializedArgs);
			}
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x000DD1B4 File Offset: 0x000DC1B4
		internal MethodCall(object handlerObject, BinaryMethodCallMessage smuggledMsg)
		{
			if (handlerObject != null)
			{
				this.uri = (handlerObject as string);
				if (this.uri == null)
				{
					MarshalByRefObject marshalByRefObject = handlerObject as MarshalByRefObject;
					if (marshalByRefObject != null)
					{
						bool flag;
						this.srvID = (MarshalByRefObject.GetIdentity(marshalByRefObject, out flag) as ServerIdentity);
						this.uri = this.srvID.URI;
					}
				}
			}
			this.typeName = smuggledMsg.TypeName;
			this.methodName = smuggledMsg.MethodName;
			this.methodSignature = (Type[])smuggledMsg.MethodSignature;
			this.args = smuggledMsg.Args;
			this.instArgs = smuggledMsg.InstantiationArgs;
			this.callContext = smuggledMsg.LogicalCallContext;
			this.ResolveMethod();
			if (smuggledMsg.HasProperties)
			{
				smuggledMsg.PopulateMessageProperties(this.Properties);
			}
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x000DD273 File Offset: 0x000DC273
		public void RootSetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			this.SetObjectData(info, ctx);
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x000DD280 File Offset: 0x000DC280
		internal void SetObjectData(SerializationInfo info, StreamingContext context)
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
			while (enumerator.MoveNext())
			{
				this.FillHeader(enumerator.Name, enumerator.Value);
			}
			if (context.State == StreamingContextStates.Remoting && context.Context != null)
			{
				Header[] array = context.Context as Header[];
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						this.FillHeader(array[i].Name, array[i].Value);
					}
				}
			}
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x000DD318 File Offset: 0x000DC318
		internal Type ResolveType()
		{
			Type type = null;
			if (this.srvID == null)
			{
				this.srvID = (IdentityHolder.CasualResolveIdentity(this.uri) as ServerIdentity);
			}
			if (this.srvID != null)
			{
				Type type2 = this.srvID.GetLastCalledType(this.typeName);
				if (type2 != null)
				{
					return type2;
				}
				int num = 0;
				if (string.CompareOrdinal(this.typeName, 0, "clr:", 0, 4) == 0)
				{
					num = 4;
				}
				int num2 = this.typeName.IndexOf(',', num);
				if (num2 == -1)
				{
					num2 = this.typeName.Length;
				}
				type2 = this.srvID.ServerType;
				type = Type.ResolveTypeRelativeTo(this.typeName, num, num2 - num, type2);
			}
			if (type == null)
			{
				type = RemotingServices.InternalGetTypeFromQualifiedTypeName(this.typeName);
			}
			if (this.srvID != null)
			{
				this.srvID.SetLastCalledType(this.typeName, type);
			}
			return type;
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x000DD3E3 File Offset: 0x000DC3E3
		public void ResolveMethod()
		{
			this.ResolveMethod(true);
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x000DD3EC File Offset: 0x000DC3EC
		internal void ResolveMethod(bool bThrowIfNotResolved)
		{
			if (this.MI == null && this.methodName != null)
			{
				RuntimeType runtimeType = this.ResolveType() as RuntimeType;
				if (this.methodName.Equals(".ctor"))
				{
					return;
				}
				if (runtimeType == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
					{
						this.typeName
					}));
				}
				if (this.methodSignature != null)
				{
					try
					{
						this.MI = runtimeType.GetMethod(this.methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, this.methodSignature, null);
					}
					catch (AmbiguousMatchException)
					{
						MemberInfo[] array = runtimeType.FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, Type.FilterName, this.methodName);
						int num = (this.instArgs == null) ? 0 : this.instArgs.Length;
						int num2 = 0;
						for (int i = 0; i < array.Length; i++)
						{
							MethodInfo methodInfo = (MethodInfo)array[i];
							int num3 = methodInfo.IsGenericMethod ? methodInfo.GetGenericArguments().Length : 0;
							if (num3 == num)
							{
								if (i > num2)
								{
									array[num2] = array[i];
								}
								num2++;
							}
						}
						MethodInfo[] array2 = new MethodInfo[num2];
						for (int j = 0; j < num2; j++)
						{
							array2[j] = (MethodInfo)array[j];
						}
						this.MI = Type.DefaultBinder.SelectMethod(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, array2, this.methodSignature, null);
					}
					if (this.instArgs != null && this.instArgs.Length > 0)
					{
						this.MI = ((MethodInfo)this.MI).MakeGenericMethod(this.instArgs);
					}
				}
				else
				{
					RemotingTypeCachedData remotingTypeCachedData = null;
					if (this.instArgs == null)
					{
						remotingTypeCachedData = InternalRemotingServices.GetReflectionCachedData(runtimeType);
						this.MI = remotingTypeCachedData.GetLastCalledMethod(this.methodName);
						if (this.MI != null)
						{
							return;
						}
					}
					bool flag = false;
					try
					{
						this.MI = runtimeType.GetMethod(this.methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (this.instArgs != null && this.instArgs.Length > 0)
						{
							this.MI = ((MethodInfo)this.MI).MakeGenericMethod(this.instArgs);
						}
					}
					catch (AmbiguousMatchException)
					{
						flag = true;
						this.ResolveOverloadedMethod(runtimeType);
					}
					if (this.MI != null && !flag && remotingTypeCachedData != null)
					{
						remotingTypeCachedData.SetLastCalledMethod(this.methodName, this.MI);
					}
				}
				if (this.MI == null && bThrowIfNotResolved)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[]
					{
						this.methodName,
						this.typeName
					}));
				}
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x000DD684 File Offset: 0x000DC684
		private void ResolveOverloadedMethod(RuntimeType t)
		{
			if (this.args == null)
			{
				return;
			}
			MemberInfo[] member = t.GetMember(this.methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			int num = member.Length;
			if (num == 1)
			{
				this.MI = (member[0] as MethodBase);
				return;
			}
			if (num == 0)
			{
				return;
			}
			int num2 = this.args.Length;
			MethodBase methodBase = null;
			for (int i = 0; i < num; i++)
			{
				MethodBase methodBase2 = member[i] as MethodBase;
				if (methodBase2.GetParameters().Length == num2)
				{
					if (methodBase != null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_AmbiguousMethod"));
					}
					methodBase = methodBase2;
				}
			}
			if (methodBase != null)
			{
				this.MI = methodBase;
			}
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x000DD718 File Offset: 0x000DC718
		private void ResolveOverloadedMethod(RuntimeType t, string methodName, ArrayList argNames, ArrayList argValues)
		{
			MemberInfo[] member = t.GetMember(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			int num = member.Length;
			if (num == 1)
			{
				this.MI = (member[0] as MethodBase);
				return;
			}
			if (num == 0)
			{
				return;
			}
			MethodBase methodBase = null;
			for (int i = 0; i < num; i++)
			{
				MethodBase methodBase2 = member[i] as MethodBase;
				ParameterInfo[] parameters = methodBase2.GetParameters();
				if (parameters.Length == argValues.Count)
				{
					bool flag = true;
					for (int j = 0; j < parameters.Length; j++)
					{
						Type type = parameters[j].ParameterType;
						if (type.IsByRef)
						{
							type = type.GetElementType();
						}
						if (type != argValues[j].GetType())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						methodBase = methodBase2;
						break;
					}
				}
			}
			if (methodBase == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_AmbiguousMethod"));
			}
			this.MI = methodBase;
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x000DD7E7 File Offset: 0x000DC7E7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x000DD7F8 File Offset: 0x000DC7F8
		internal void SetObjectFromSoapData(SerializationInfo info)
		{
			this.methodName = info.GetString("__methodName");
			ArrayList arrayList = (ArrayList)info.GetValue("__paramNameList", typeof(ArrayList));
			Hashtable keyToNamespaceTable = (Hashtable)info.GetValue("__keyToNamespaceTable", typeof(Hashtable));
			if (this.MI == null)
			{
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = arrayList;
				for (int i = 0; i < arrayList3.Count; i++)
				{
					arrayList2.Add(info.GetValue((string)arrayList3[i], typeof(object)));
				}
				RuntimeType runtimeType = this.ResolveType() as RuntimeType;
				if (runtimeType == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
					{
						this.typeName
					}));
				}
				this.ResolveOverloadedMethod(runtimeType, this.methodName, arrayList3, arrayList2);
				if (this.MI == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[]
					{
						this.methodName,
						this.typeName
					}));
				}
			}
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.MI);
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			int[] marshalRequestArgMap = reflectionCachedData.MarshalRequestArgMap;
			int[] outOnlyArgMap = reflectionCachedData.OutOnlyArgMap;
			object obj = (this.InternalProperties == null) ? null : this.InternalProperties["__UnorderedParams"];
			this.args = new object[parameters.Length];
			if (obj != null && obj is bool && (bool)obj)
			{
				for (int j = 0; j < arrayList.Count; j++)
				{
					string text = (string)arrayList[j];
					int num = -1;
					for (int k = 0; k < parameters.Length; k++)
					{
						if (text.Equals(parameters[k].Name))
						{
							num = parameters[k].Position;
							break;
						}
					}
					if (num == -1)
					{
						if (!text.StartsWith("__param", StringComparison.Ordinal))
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
						}
						num = int.Parse(text.Substring(7), CultureInfo.InvariantCulture);
					}
					if (num >= this.args.Length)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
					}
					this.args[num] = Message.SoapCoerceArg(info.GetValue(text, typeof(object)), parameters[num].ParameterType, keyToNamespaceTable);
				}
				return;
			}
			for (int l = 0; l < arrayList.Count; l++)
			{
				string name = (string)arrayList[l];
				this.args[marshalRequestArgMap[l]] = Message.SoapCoerceArg(info.GetValue(name, typeof(object)), parameters[marshalRequestArgMap[l]].ParameterType, keyToNamespaceTable);
			}
			foreach (int num2 in outOnlyArgMap)
			{
				Type elementType = parameters[num2].ParameterType.GetElementType();
				if (elementType.IsValueType)
				{
					this.args[num2] = Activator.CreateInstance(elementType, true);
				}
			}
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x000DDB19 File Offset: 0x000DCB19
		public virtual void Init()
		{
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x0600410E RID: 16654 RVA: 0x000DDB1B File Offset: 0x000DCB1B
		public int ArgCount
		{
			get
			{
				if (this.args != null)
				{
					return this.args.Length;
				}
				return 0;
			}
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x000DDB2F File Offset: 0x000DCB2F
		public object GetArg(int argNum)
		{
			return this.args[argNum];
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x000DDB3C File Offset: 0x000DCB3C
		public string GetArgName(int index)
		{
			this.ResolveMethod();
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.MI);
			return reflectionCachedData.Parameters[index].Name;
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x000DDB68 File Offset: 0x000DCB68
		public object[] Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06004112 RID: 16658 RVA: 0x000DDB70 File Offset: 0x000DCB70
		public int InArgCount
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, false);
				}
				return this.argMapper.ArgCount;
			}
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x000DDB92 File Offset: 0x000DCB92
		public object GetInArg(int argNum)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, false);
			}
			return this.argMapper.GetArg(argNum);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x000DDBB5 File Offset: 0x000DCBB5
		public string GetInArgName(int index)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, false);
			}
			return this.argMapper.GetArgName(index);
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x000DDBD8 File Offset: 0x000DCBD8
		public object[] InArgs
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, false);
				}
				return this.argMapper.Args;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06004116 RID: 16662 RVA: 0x000DDBFA File Offset: 0x000DCBFA
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x000DDC02 File Offset: 0x000DCC02
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06004118 RID: 16664 RVA: 0x000DDC0A File Offset: 0x000DCC0A
		public object MethodSignature
		{
			get
			{
				if (this.methodSignature != null)
				{
					return this.methodSignature;
				}
				if (this.MI != null)
				{
					this.methodSignature = Message.GenerateMethodSignature(this.MethodBase);
				}
				return null;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x000DDC35 File Offset: 0x000DCC35
		public MethodBase MethodBase
		{
			get
			{
				if (this.MI == null)
				{
					this.MI = RemotingServices.InternalGetMethodBaseFromMethodMessage(this);
				}
				return this.MI;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600411A RID: 16666 RVA: 0x000DDC51 File Offset: 0x000DCC51
		// (set) Token: 0x0600411B RID: 16667 RVA: 0x000DDC59 File Offset: 0x000DCC59
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

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x0600411C RID: 16668 RVA: 0x000DDC62 File Offset: 0x000DCC62
		public bool HasVarArgs
		{
			get
			{
				return this.fVarArgs;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x0600411D RID: 16669 RVA: 0x000DDC6C File Offset: 0x000DCC6C
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
						this.ExternalProperties = new MCMDictionary(this, this.InternalProperties);
					}
					externalProperties = this.ExternalProperties;
				}
				return externalProperties;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600411E RID: 16670 RVA: 0x000DDCD0 File Offset: 0x000DCCD0
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000DDCD8 File Offset: 0x000DCCD8
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this.callContext == null)
			{
				this.callContext = new LogicalCallContext();
			}
			return this.callContext;
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000DDCF4 File Offset: 0x000DCCF4
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			LogicalCallContext result = this.callContext;
			this.callContext = ctx;
			return result;
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004121 RID: 16673 RVA: 0x000DDD10 File Offset: 0x000DCD10
		// (set) Token: 0x06004122 RID: 16674 RVA: 0x000DDD18 File Offset: 0x000DCD18
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return this.srvID;
			}
			set
			{
				this.srvID = value;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x000DDD21 File Offset: 0x000DCD21
		// (set) Token: 0x06004124 RID: 16676 RVA: 0x000DDD29 File Offset: 0x000DCD29
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return this.identity;
			}
			set
			{
				this.identity = value;
			}
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x000DDD32 File Offset: 0x000DCD32
		void IInternalMessage.SetURI(string val)
		{
			this.uri = val;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x000DDD3B File Offset: 0x000DCD3B
		void IInternalMessage.SetCallContext(LogicalCallContext newCallContext)
		{
			this.callContext = newCallContext;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x000DDD44 File Offset: 0x000DCD44
		bool IInternalMessage.HasProperties()
		{
			return this.ExternalProperties != null || this.InternalProperties != null;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x000DDD5C File Offset: 0x000DCD5C
		internal void FillHeaders(Header[] h)
		{
			this.FillHeaders(h, false);
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x000DDD68 File Offset: 0x000DCD68
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

		// Token: 0x0600412A RID: 16682 RVA: 0x000DDDF0 File Offset: 0x000DCDF0
		internal virtual bool FillSpecialHeader(string key, object value)
		{
			if (key != null)
			{
				if (key.Equals("__Uri"))
				{
					this.uri = (string)value;
				}
				else if (key.Equals("__MethodName"))
				{
					this.methodName = (string)value;
				}
				else if (key.Equals("__MethodSignature"))
				{
					this.methodSignature = (Type[])value;
				}
				else if (key.Equals("__TypeName"))
				{
					this.typeName = (string)value;
				}
				else if (key.Equals("__Args"))
				{
					this.args = (object[])value;
				}
				else
				{
					if (!key.Equals("__CallContext"))
					{
						return false;
					}
					if (value is string)
					{
						this.callContext = new LogicalCallContext();
						this.callContext.RemotingData.LogicalCallID = (string)value;
					}
					else
					{
						this.callContext = (LogicalCallContext)value;
					}
				}
			}
			return true;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x000DDED9 File Offset: 0x000DCED9
		internal void FillHeader(string key, object value)
		{
			if (!this.FillSpecialHeader(key, value))
			{
				if (this.InternalProperties == null)
				{
					this.InternalProperties = new Hashtable();
				}
				this.InternalProperties[key] = value;
			}
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x000DDF08 File Offset: 0x000DCF08
		public virtual object HeaderHandler(Header[] h)
		{
			SerializationMonkey serializationMonkey = (SerializationMonkey)FormatterServices.GetUninitializedObject(typeof(SerializationMonkey));
			Header[] array;
			if (h != null && h.Length > 0 && h[0].Name == "__methodName")
			{
				this.methodName = (string)h[0].Value;
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
			this.FillHeaders(array, true);
			this.ResolveMethod(false);
			serializationMonkey._obj = this;
			if (this.MI != null)
			{
				ArgMapper argMapper = new ArgMapper(this.MI, false);
				serializationMonkey.fieldNames = argMapper.ArgNames;
				serializationMonkey.fieldTypes = argMapper.ArgTypes;
			}
			return serializationMonkey;
		}

		// Token: 0x040020BC RID: 8380
		private const BindingFlags LookupAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x040020BD RID: 8381
		private const BindingFlags LookupPublic = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x040020BE RID: 8382
		private string uri;

		// Token: 0x040020BF RID: 8383
		private string methodName;

		// Token: 0x040020C0 RID: 8384
		private MethodBase MI;

		// Token: 0x040020C1 RID: 8385
		private string typeName;

		// Token: 0x040020C2 RID: 8386
		private object[] args;

		// Token: 0x040020C3 RID: 8387
		private Type[] instArgs;

		// Token: 0x040020C4 RID: 8388
		private LogicalCallContext callContext;

		// Token: 0x040020C5 RID: 8389
		private Type[] methodSignature;

		// Token: 0x040020C6 RID: 8390
		protected IDictionary ExternalProperties;

		// Token: 0x040020C7 RID: 8391
		protected IDictionary InternalProperties;

		// Token: 0x040020C8 RID: 8392
		private ServerIdentity srvID;

		// Token: 0x040020C9 RID: 8393
		private Identity identity;

		// Token: 0x040020CA RID: 8394
		private bool fSoap;

		// Token: 0x040020CB RID: 8395
		private bool fVarArgs;

		// Token: 0x040020CC RID: 8396
		private ArgMapper argMapper;
	}
}
