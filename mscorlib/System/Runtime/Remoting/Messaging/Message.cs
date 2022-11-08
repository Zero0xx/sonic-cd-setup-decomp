using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000710 RID: 1808
	[Serializable]
	internal class Message : IMethodCallMessage, IMethodMessage, IMessage, IInternalMessage, ISerializable
	{
		// Token: 0x06004034 RID: 16436 RVA: 0x000DAC54 File Offset: 0x000D9C54
		public virtual Exception GetFault()
		{
			return this._Fault;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x000DAC5C File Offset: 0x000D9C5C
		public virtual void SetFault(Exception e)
		{
			this._Fault = e;
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x000DAC65 File Offset: 0x000D9C65
		internal virtual void SetOneWay()
		{
			this._flags |= 8;
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x000DAC75 File Offset: 0x000D9C75
		public virtual int GetCallType()
		{
			this.InitIfNecessary();
			return this._flags;
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x000DAC83 File Offset: 0x000D9C83
		internal IntPtr GetFramePtr()
		{
			return this._frame;
		}

		// Token: 0x06004039 RID: 16441
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetAsyncBeginInfo(out AsyncCallback acbd, out object state);

		// Token: 0x0600403A RID: 16442
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetThisPtr();

		// Token: 0x0600403B RID: 16443
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IAsyncResult GetAsyncResult();

		// Token: 0x0600403C RID: 16444 RVA: 0x000DAC8B File Offset: 0x000D9C8B
		public void Init()
		{
		}

		// Token: 0x0600403D RID: 16445
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetReturnValue();

		// Token: 0x0600403E RID: 16446 RVA: 0x000DAC8D File Offset: 0x000D9C8D
		internal Message()
		{
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x000DAC98 File Offset: 0x000D9C98
		internal void InitFields(MessageData msgData)
		{
			this._frame = msgData.pFrame;
			this._delegateMD = msgData.pDelegateMD;
			this._methodDesc = msgData.pMethodDesc;
			this._flags = msgData.iFlags;
			this._initDone = true;
			this._metaSigHolder = msgData.pSig;
			this._governingType = msgData.thGoverningType;
			this._MethodName = null;
			this._MethodSignature = null;
			this._MethodBase = null;
			this._URI = null;
			this._Fault = null;
			this._ID = null;
			this._srvID = null;
			this._callContext = null;
			if (this._properties != null)
			{
				((IDictionary)this._properties).Clear();
			}
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x000DAD4A File Offset: 0x000D9D4A
		private void InitIfNecessary()
		{
			if (!this._initDone)
			{
				this.Init();
				this._initDone = true;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x000DAD61 File Offset: 0x000D9D61
		// (set) Token: 0x06004042 RID: 16450 RVA: 0x000DAD69 File Offset: 0x000D9D69
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return this._srvID;
			}
			set
			{
				this._srvID = value;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06004043 RID: 16451 RVA: 0x000DAD72 File Offset: 0x000D9D72
		// (set) Token: 0x06004044 RID: 16452 RVA: 0x000DAD7A File Offset: 0x000D9D7A
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x000DAD83 File Offset: 0x000D9D83
		void IInternalMessage.SetURI(string URI)
		{
			this._URI = URI;
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x000DAD8C File Offset: 0x000D9D8C
		void IInternalMessage.SetCallContext(LogicalCallContext callContext)
		{
			this._callContext = callContext;
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x000DAD95 File Offset: 0x000D9D95
		bool IInternalMessage.HasProperties()
		{
			return this._properties != null;
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x000DADA3 File Offset: 0x000D9DA3
		public IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					Interlocked.CompareExchange(ref this._properties, new MCMDictionary(this, null), null);
				}
				return (IDictionary)this._properties;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x000DADCC File Offset: 0x000D9DCC
		// (set) Token: 0x0600404A RID: 16458 RVA: 0x000DADD4 File Offset: 0x000D9DD4
		public string Uri
		{
			get
			{
				return this._URI;
			}
			set
			{
				this._URI = value;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x000DADE0 File Offset: 0x000D9DE0
		public bool HasVarArgs
		{
			get
			{
				if ((this._flags & 16) == 0 && (this._flags & 32) == 0)
				{
					if (!this.InternalHasVarArgs())
					{
						this._flags |= 16;
					}
					else
					{
						this._flags |= 32;
					}
				}
				return 1 == (this._flags & 32);
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x000DAE37 File Offset: 0x000D9E37
		public int ArgCount
		{
			get
			{
				return this.InternalGetArgCount();
			}
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x000DAE3F File Offset: 0x000D9E3F
		public object GetArg(int argNum)
		{
			return this.InternalGetArg(argNum);
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x000DAE48 File Offset: 0x000D9E48
		public string GetArgName(int index)
		{
			if (index >= this.ArgCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.GetMethodBase());
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			if (index < parameters.Length)
			{
				return parameters[index].Name;
			}
			return "VarArg" + (index - parameters.Length);
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x000DAE9F File Offset: 0x000D9E9F
		public object[] Args
		{
			get
			{
				return this.InternalGetArgs();
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x000DAEA7 File Offset: 0x000D9EA7
		public int InArgCount
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.ArgCount;
			}
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x000DAEC9 File Offset: 0x000D9EC9
		public object GetInArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x000DAEEC File Offset: 0x000D9EEC
		public string GetInArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x000DAF0F File Offset: 0x000D9F0F
		public object[] InArgs
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.Args;
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x000DAF34 File Offset: 0x000D9F34
		private void UpdateNames()
		{
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.GetMethodBase());
			this._typeName = reflectionCachedData.TypeAndAssemblyName;
			this._MethodName = reflectionCachedData.MethodName;
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x000DAF65 File Offset: 0x000D9F65
		public string MethodName
		{
			get
			{
				if (this._MethodName == null)
				{
					this.UpdateNames();
				}
				return this._MethodName;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x000DAF7B File Offset: 0x000D9F7B
		public string TypeName
		{
			get
			{
				if (this._typeName == null)
				{
					this.UpdateNames();
				}
				return this._typeName;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x000DAF91 File Offset: 0x000D9F91
		public object MethodSignature
		{
			get
			{
				if (this._MethodSignature == null)
				{
					this._MethodSignature = Message.GenerateMethodSignature(this.GetMethodBase());
				}
				return this._MethodSignature;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004058 RID: 16472 RVA: 0x000DAFB2 File Offset: 0x000D9FB2
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x000DAFBA File Offset: 0x000D9FBA
		public MethodBase MethodBase
		{
			get
			{
				return this.GetMethodBase();
			}
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x000DAFC2 File Offset: 0x000D9FC2
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x000DAFD4 File Offset: 0x000D9FD4
		internal unsafe MethodBase GetMethodBase()
		{
			if (this._MethodBase == null)
			{
				RuntimeMethodHandle methodHandle = new RuntimeMethodHandle((void*)this._methodDesc);
				RuntimeTypeHandle reflectedTypeHandle = new RuntimeTypeHandle((void*)this._governingType);
				this._MethodBase = RuntimeType.GetMethodBase(reflectedTypeHandle, methodHandle);
			}
			return this._MethodBase;
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x000DB020 File Offset: 0x000DA020
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			LogicalCallContext callContext = this._callContext;
			this._callContext = callCtx;
			return callContext;
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x000DB03C File Offset: 0x000DA03C
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this._callContext == null)
			{
				this._callContext = new LogicalCallContext();
			}
			return this._callContext;
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x000DB058 File Offset: 0x000DA058
		internal static Type[] GenerateMethodSignature(MethodBase mb)
		{
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(mb);
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return array;
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x000DB098 File Offset: 0x000DA098
		internal static object[] CoerceArgs(IMethodMessage m)
		{
			MethodBase methodBase = m.MethodBase;
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
			return Message.CoerceArgs(m, reflectionCachedData.Parameters);
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x000DB0BF File Offset: 0x000DA0BF
		internal static object[] CoerceArgs(IMethodMessage m, ParameterInfo[] pi)
		{
			return Message.CoerceArgs(m.MethodBase, m.Args, pi);
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x000DB0D4 File Offset: 0x000DA0D4
		internal static object[] CoerceArgs(MethodBase mb, object[] args, ParameterInfo[] pi)
		{
			if (pi == null)
			{
				throw new ArgumentNullException("pi");
			}
			if (pi.Length != args.Length)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_ArgMismatch"), new object[]
				{
					mb.DeclaringType.FullName,
					mb.Name,
					args.Length,
					pi.Length
				}));
			}
			for (int i = 0; i < pi.Length; i++)
			{
				ParameterInfo parameterInfo = pi[i];
				Type parameterType = parameterInfo.ParameterType;
				object obj = args[i];
				if (obj != null)
				{
					args[i] = Message.CoerceArg(obj, parameterType);
				}
				else if (parameterType.IsByRef)
				{
					Type elementType = parameterType.GetElementType();
					if (elementType.IsValueType)
					{
						if (parameterInfo.IsOut)
						{
							args[i] = Activator.CreateInstance(elementType, true);
						}
						else if (!elementType.IsGenericType || elementType.GetGenericTypeDefinition() != typeof(Nullable<>))
						{
							throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MissingArgValue"), new object[]
							{
								elementType.FullName,
								i
							}));
						}
					}
				}
				else if (parameterType.IsValueType && (!parameterType.IsGenericType || parameterType.GetGenericTypeDefinition() != typeof(Nullable<>)))
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MissingArgValue"), new object[]
					{
						parameterType.FullName,
						i
					}));
				}
			}
			return args;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x000DB26C File Offset: 0x000DA26C
		internal static object CoerceArg(object value, Type pt)
		{
			object obj = null;
			if (value != null)
			{
				Exception innerException = null;
				try
				{
					if (pt.IsByRef)
					{
						pt = pt.GetElementType();
					}
					if (pt.IsInstanceOfType(value))
					{
						obj = value;
					}
					else
					{
						obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
					}
				}
				catch (Exception ex)
				{
					innerException = ex;
				}
				if (obj == null)
				{
					string text;
					if (RemotingServices.IsTransparentProxy(value))
					{
						text = typeof(MarshalByRefObject).ToString();
					}
					else
					{
						text = value.ToString();
					}
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_CoercionFailed"), new object[]
					{
						text,
						pt
					}), innerException);
				}
			}
			return obj;
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x000DB31C File Offset: 0x000DA31C
		internal static object SoapCoerceArg(object value, Type pt, Hashtable keyToNamespaceTable)
		{
			object obj = null;
			if (value != null)
			{
				try
				{
					if (pt.IsByRef)
					{
						pt = pt.GetElementType();
					}
					if (pt.IsInstanceOfType(value))
					{
						obj = value;
					}
					else
					{
						string text = value as string;
						if (text != null)
						{
							if (pt == typeof(double))
							{
								if (text == "INF")
								{
									obj = double.PositiveInfinity;
								}
								else if (text == "-INF")
								{
									obj = double.NegativeInfinity;
								}
								else
								{
									obj = double.Parse(text, CultureInfo.InvariantCulture);
								}
							}
							else if (pt == typeof(float))
							{
								if (text == "INF")
								{
									obj = float.PositiveInfinity;
								}
								else if (text == "-INF")
								{
									obj = float.NegativeInfinity;
								}
								else
								{
									obj = float.Parse(text, CultureInfo.InvariantCulture);
								}
							}
							else if (SoapType.typeofISoapXsd.IsAssignableFrom(pt))
							{
								if (pt == SoapType.typeofSoapTime)
								{
									obj = SoapTime.Parse(text);
								}
								else if (pt == SoapType.typeofSoapDate)
								{
									obj = SoapDate.Parse(text);
								}
								else if (pt == SoapType.typeofSoapYearMonth)
								{
									obj = SoapYearMonth.Parse(text);
								}
								else if (pt == SoapType.typeofSoapYear)
								{
									obj = SoapYear.Parse(text);
								}
								else if (pt == SoapType.typeofSoapMonthDay)
								{
									obj = SoapMonthDay.Parse(text);
								}
								else if (pt == SoapType.typeofSoapDay)
								{
									obj = SoapDay.Parse(text);
								}
								else if (pt == SoapType.typeofSoapMonth)
								{
									obj = SoapMonth.Parse(text);
								}
								else if (pt == SoapType.typeofSoapHexBinary)
								{
									obj = SoapHexBinary.Parse(text);
								}
								else if (pt == SoapType.typeofSoapBase64Binary)
								{
									obj = SoapBase64Binary.Parse(text);
								}
								else if (pt == SoapType.typeofSoapInteger)
								{
									obj = SoapInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapPositiveInteger)
								{
									obj = SoapPositiveInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNonPositiveInteger)
								{
									obj = SoapNonPositiveInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNonNegativeInteger)
								{
									obj = SoapNonNegativeInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNegativeInteger)
								{
									obj = SoapNegativeInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapAnyUri)
								{
									obj = SoapAnyUri.Parse(text);
								}
								else if (pt == SoapType.typeofSoapQName)
								{
									obj = SoapQName.Parse(text);
									SoapQName soapQName = (SoapQName)obj;
									if (soapQName.Key.Length == 0)
									{
										soapQName.Namespace = (string)keyToNamespaceTable["xmlns"];
									}
									else
									{
										soapQName.Namespace = (string)keyToNamespaceTable["xmlns:" + soapQName.Key];
									}
								}
								else if (pt == SoapType.typeofSoapNotation)
								{
									obj = SoapNotation.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNormalizedString)
								{
									obj = SoapNormalizedString.Parse(text);
								}
								else if (pt == SoapType.typeofSoapToken)
								{
									obj = SoapToken.Parse(text);
								}
								else if (pt == SoapType.typeofSoapLanguage)
								{
									obj = SoapLanguage.Parse(text);
								}
								else if (pt == SoapType.typeofSoapName)
								{
									obj = SoapName.Parse(text);
								}
								else if (pt == SoapType.typeofSoapIdrefs)
								{
									obj = SoapIdrefs.Parse(text);
								}
								else if (pt == SoapType.typeofSoapEntities)
								{
									obj = SoapEntities.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNmtoken)
								{
									obj = SoapNmtoken.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNmtokens)
								{
									obj = SoapNmtokens.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNcName)
								{
									obj = SoapNcName.Parse(text);
								}
								else if (pt == SoapType.typeofSoapId)
								{
									obj = SoapId.Parse(text);
								}
								else if (pt == SoapType.typeofSoapIdref)
								{
									obj = SoapIdref.Parse(text);
								}
								else if (pt == SoapType.typeofSoapEntity)
								{
									obj = SoapEntity.Parse(text);
								}
							}
							else if (pt == typeof(bool))
							{
								if (text == "1" || text == "true")
								{
									obj = true;
								}
								else
								{
									if (!(text == "0") && !(text == "false"))
									{
										throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_CoercionFailed"), new object[]
										{
											text,
											pt
										}));
									}
									obj = false;
								}
							}
							else if (pt == typeof(DateTime))
							{
								obj = SoapDateTime.Parse(text);
							}
							else if (pt.IsPrimitive)
							{
								obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
							}
							else if (pt == typeof(TimeSpan))
							{
								obj = SoapDuration.Parse(text);
							}
							else if (pt == typeof(char))
							{
								obj = text[0];
							}
							else
							{
								obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
							}
						}
						else
						{
							obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
						}
					}
				}
				catch (Exception)
				{
				}
				if (obj == null)
				{
					string text2;
					if (RemotingServices.IsTransparentProxy(value))
					{
						text2 = typeof(MarshalByRefObject).ToString();
					}
					else
					{
						text2 = value.ToString();
					}
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_CoercionFailed"), new object[]
					{
						text2,
						pt
					}));
				}
			}
			return obj;
		}

		// Token: 0x06004064 RID: 16484
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool InternalHasVarArgs();

		// Token: 0x06004065 RID: 16485
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int InternalGetArgCount();

		// Token: 0x06004066 RID: 16486
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object InternalGetArg(int argNum);

		// Token: 0x06004067 RID: 16487
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object[] InternalGetArgs();

		// Token: 0x06004068 RID: 16488
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void PropagateOutParameters(object[] OutArgs, object retVal);

		// Token: 0x06004069 RID: 16489
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Dispatch(object target, bool fExecuteInContext);

		// Token: 0x0600406A RID: 16490 RVA: 0x000DB84C File Offset: 0x000DA84C
		[Conditional("_REMOTING_DEBUG")]
		public static void DebugOut(string s)
		{
			Message.OutToUnmanagedDebugger(string.Concat(new object[]
			{
				"\nRMTING: Thrd ",
				Thread.CurrentThread.GetHashCode(),
				" : ",
				s
			}));
		}

		// Token: 0x0600406B RID: 16491
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OutToUnmanagedDebugger(string s);

		// Token: 0x0600406C RID: 16492 RVA: 0x000DB891 File Offset: 0x000DA891
		internal static LogicalCallContext PropagateCallContextFromMessageToThread(IMessage msg)
		{
			return CallContext.SetLogicalCallContext((LogicalCallContext)msg.Properties[Message.CallContextKey]);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x000DB8B0 File Offset: 0x000DA8B0
		internal static void PropagateCallContextFromThreadToMessage(IMessage msg)
		{
			LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
			msg.Properties[Message.CallContextKey] = logicalCallContext;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x000DB8D4 File Offset: 0x000DA8D4
		internal static void PropagateCallContextFromThreadToMessage(IMessage msg, LogicalCallContext oldcctx)
		{
			Message.PropagateCallContextFromThreadToMessage(msg);
			CallContext.SetLogicalCallContext(oldcctx);
		}

		// Token: 0x0400206F RID: 8303
		internal const int Sync = 0;

		// Token: 0x04002070 RID: 8304
		internal const int BeginAsync = 1;

		// Token: 0x04002071 RID: 8305
		internal const int EndAsync = 2;

		// Token: 0x04002072 RID: 8306
		internal const int Ctor = 4;

		// Token: 0x04002073 RID: 8307
		internal const int OneWay = 8;

		// Token: 0x04002074 RID: 8308
		internal const int CallMask = 15;

		// Token: 0x04002075 RID: 8309
		internal const int FixedArgs = 16;

		// Token: 0x04002076 RID: 8310
		internal const int VarArgs = 32;

		// Token: 0x04002077 RID: 8311
		private string _MethodName;

		// Token: 0x04002078 RID: 8312
		private Type[] _MethodSignature;

		// Token: 0x04002079 RID: 8313
		private MethodBase _MethodBase;

		// Token: 0x0400207A RID: 8314
		private object _properties;

		// Token: 0x0400207B RID: 8315
		private string _URI;

		// Token: 0x0400207C RID: 8316
		private string _typeName;

		// Token: 0x0400207D RID: 8317
		private Exception _Fault;

		// Token: 0x0400207E RID: 8318
		private Identity _ID;

		// Token: 0x0400207F RID: 8319
		private ServerIdentity _srvID;

		// Token: 0x04002080 RID: 8320
		private ArgMapper _argMapper;

		// Token: 0x04002081 RID: 8321
		private LogicalCallContext _callContext;

		// Token: 0x04002082 RID: 8322
		private IntPtr _frame;

		// Token: 0x04002083 RID: 8323
		private IntPtr _methodDesc;

		// Token: 0x04002084 RID: 8324
		private IntPtr _metaSigHolder;

		// Token: 0x04002085 RID: 8325
		private IntPtr _delegateMD;

		// Token: 0x04002086 RID: 8326
		private IntPtr _governingType;

		// Token: 0x04002087 RID: 8327
		private int _flags;

		// Token: 0x04002088 RID: 8328
		private bool _initDone;

		// Token: 0x04002089 RID: 8329
		internal static string CallContextKey = "__CallContext";

		// Token: 0x0400208A RID: 8330
		internal static string UriKey = "__Uri";
	}
}
