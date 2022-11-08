using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Metadata;
using System.Security.Principal;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000799 RID: 1945
	[Serializable]
	internal class StackBuilderSink : IMessageSink
	{
		// Token: 0x0600453F RID: 17727 RVA: 0x000EB7BE File Offset: 0x000EA7BE
		public StackBuilderSink(MarshalByRefObject server)
		{
			this._server = server;
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x000EB7CD File Offset: 0x000EA7CD
		public StackBuilderSink(object server)
		{
			this._server = server;
			if (this._server == null)
			{
				this._bStatic = true;
			}
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x000EB7EB File Offset: 0x000EA7EB
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			return this.SyncProcessMessage(msg, 0, false);
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x000EB7F8 File Offset: 0x000EA7F8
		internal virtual IMessage SyncProcessMessage(IMessage msg, int methodPtr, bool fExecuteInContext)
		{
			IMessage message = InternalSink.ValidateMessage(msg);
			if (message != null)
			{
				return message;
			}
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			LogicalCallContext logicalCallContext = null;
			LogicalCallContext logicalCallContext2 = CallContext.GetLogicalCallContext();
			object data = logicalCallContext2.GetData("__xADCall");
			bool flag = false;
			IMessage message2;
			try
			{
				object server = this._server;
				StackBuilderSink.VerifyIsOkToCallMethod(server, methodCallMessage);
				LogicalCallContext logicalCallContext3;
				if (methodCallMessage != null)
				{
					logicalCallContext3 = methodCallMessage.LogicalCallContext;
				}
				else
				{
					logicalCallContext3 = (LogicalCallContext)msg.Properties["__CallContext"];
				}
				logicalCallContext = CallContext.SetLogicalCallContext(logicalCallContext3);
				flag = true;
				logicalCallContext3.PropagateIncomingHeadersToCallContext(msg);
				StackBuilderSink.PreserveThreadPrincipalIfNecessary(logicalCallContext3, logicalCallContext);
				if (this.IsOKToStackBlt(methodCallMessage, server) && ((Message)methodCallMessage).Dispatch(server, fExecuteInContext))
				{
					message2 = new StackBasedReturnMessage();
					((StackBasedReturnMessage)message2).InitFields((Message)methodCallMessage);
					LogicalCallContext logicalCallContext4 = CallContext.GetLogicalCallContext();
					logicalCallContext4.PropagateOutgoingHeadersToMessage(message2);
					((StackBasedReturnMessage)message2).SetLogicalCallContext(logicalCallContext4);
				}
				else
				{
					MethodBase methodBase = StackBuilderSink.GetMethodBase(methodCallMessage);
					object[] array = null;
					RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
					object[] args = Message.CoerceArgs(methodCallMessage, reflectionCachedData.Parameters);
					object ret = this.PrivateProcessMessage(methodBase.MethodHandle, args, server, methodPtr, fExecuteInContext, out array);
					this.CopyNonByrefOutArgsFromOriginalArgs(reflectionCachedData, args, ref array);
					LogicalCallContext logicalCallContext5 = CallContext.GetLogicalCallContext();
					if (data != null && (bool)data && logicalCallContext5 != null)
					{
						logicalCallContext5.RemovePrincipalIfNotSerializable();
					}
					message2 = new ReturnMessage(ret, array, (array == null) ? 0 : array.Length, logicalCallContext5, methodCallMessage);
					logicalCallContext5.PropagateOutgoingHeadersToMessage(message2);
					CallContext.SetLogicalCallContext(logicalCallContext);
				}
			}
			catch (Exception e)
			{
				message2 = new ReturnMessage(e, methodCallMessage);
				((ReturnMessage)message2).SetLogicalCallContext(methodCallMessage.LogicalCallContext);
				if (flag)
				{
					CallContext.SetLogicalCallContext(logicalCallContext);
				}
			}
			return message2;
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x000EB9B4 File Offset: 0x000EA9B4
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			IMessageCtrl result = null;
			IMessage message = null;
			LogicalCallContext logicalCallContext = null;
			bool flag = false;
			try
			{
				try
				{
					LogicalCallContext logicalCallContext2 = (LogicalCallContext)methodCallMessage.Properties[Message.CallContextKey];
					object server = this._server;
					StackBuilderSink.VerifyIsOkToCallMethod(server, methodCallMessage);
					logicalCallContext = CallContext.SetLogicalCallContext(logicalCallContext2);
					flag = true;
					logicalCallContext2.PropagateIncomingHeadersToCallContext(msg);
					StackBuilderSink.PreserveThreadPrincipalIfNecessary(logicalCallContext2, logicalCallContext);
					ServerChannelSinkStack serverChannelSinkStack = msg.Properties["__SinkStack"] as ServerChannelSinkStack;
					if (serverChannelSinkStack != null)
					{
						serverChannelSinkStack.ServerObject = server;
					}
					MethodBase methodBase = StackBuilderSink.GetMethodBase(methodCallMessage);
					object[] array = null;
					RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
					object[] args = Message.CoerceArgs(methodCallMessage, reflectionCachedData.Parameters);
					object ret = this.PrivateProcessMessage(methodBase.MethodHandle, args, server, 0, false, out array);
					this.CopyNonByrefOutArgsFromOriginalArgs(reflectionCachedData, args, ref array);
					if (replySink != null)
					{
						LogicalCallContext logicalCallContext3 = CallContext.GetLogicalCallContext();
						if (logicalCallContext3 != null)
						{
							logicalCallContext3.RemovePrincipalIfNotSerializable();
						}
						message = new ReturnMessage(ret, array, (array == null) ? 0 : array.Length, logicalCallContext3, methodCallMessage);
						logicalCallContext3.PropagateOutgoingHeadersToMessage(message);
					}
				}
				catch (Exception e)
				{
					if (replySink != null)
					{
						message = new ReturnMessage(e, methodCallMessage);
						((ReturnMessage)message).SetLogicalCallContext((LogicalCallContext)methodCallMessage.Properties[Message.CallContextKey]);
					}
				}
				finally
				{
					if (replySink != null)
					{
						replySink.SyncProcessMessage(message);
					}
				}
			}
			finally
			{
				if (flag)
				{
					CallContext.SetLogicalCallContext(logicalCallContext);
				}
			}
			return result;
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06004544 RID: 17732 RVA: 0x000EBB54 File Offset: 0x000EAB54
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x000EBB58 File Offset: 0x000EAB58
		internal bool IsOKToStackBlt(IMethodMessage mcMsg, object server)
		{
			bool result = false;
			Message message = mcMsg as Message;
			if (message != null)
			{
				IInternalMessage internalMessage = message;
				if (message.GetFramePtr() != IntPtr.Zero && message.GetThisPtr() == server && (internalMessage.IdentityObject == null || (internalMessage.IdentityObject != null && internalMessage.IdentityObject == internalMessage.ServerIdentityObject)))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x000EBBB0 File Offset: 0x000EABB0
		private static MethodBase GetMethodBase(IMethodMessage msg)
		{
			MethodBase methodBase = msg.MethodBase;
			if (methodBase == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[]
				{
					msg.MethodName,
					msg.TypeName
				}));
			}
			return methodBase;
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x000EBBFC File Offset: 0x000EABFC
		private static void VerifyIsOkToCallMethod(object server, IMethodMessage msg)
		{
			bool flag = false;
			MarshalByRefObject marshalByRefObject = server as MarshalByRefObject;
			if (marshalByRefObject != null)
			{
				bool flag2;
				Identity identity = MarshalByRefObject.GetIdentity(marshalByRefObject, out flag2);
				if (identity != null)
				{
					ServerIdentity serverIdentity = identity as ServerIdentity;
					if (serverIdentity != null && serverIdentity.MarshaledAsSpecificType)
					{
						Type serverType = serverIdentity.ServerType;
						if (serverType != null)
						{
							MethodBase methodBase = StackBuilderSink.GetMethodBase(msg);
							Type declaringType = methodBase.DeclaringType;
							if (declaringType != serverType && !declaringType.IsAssignableFrom(serverType))
							{
								throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_InvalidCallingType"), new object[]
								{
									methodBase.DeclaringType.FullName,
									serverType.FullName
								}));
							}
							if (declaringType.IsInterface)
							{
								StackBuilderSink.VerifyNotIRemoteDispatch(declaringType);
							}
							flag = true;
						}
					}
				}
				if (!flag)
				{
					MethodBase methodBase2 = StackBuilderSink.GetMethodBase(msg);
					Type reflectedType = methodBase2.ReflectedType;
					if (!reflectedType.IsInterface)
					{
						if (!reflectedType.IsInstanceOfType(marshalByRefObject))
						{
							throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_InvalidCallingType"), new object[]
							{
								reflectedType.FullName,
								marshalByRefObject.GetType().FullName
							}));
						}
					}
					else
					{
						StackBuilderSink.VerifyNotIRemoteDispatch(reflectedType);
					}
				}
			}
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x000EBD2E File Offset: 0x000EAD2E
		private static void VerifyNotIRemoteDispatch(Type reflectedType)
		{
			if (reflectedType.FullName.Equals(StackBuilderSink.sIRemoteDispatch) && reflectedType.Module.Assembly.nGetSimpleName().Equals(StackBuilderSink.sIRemoteDispatchAssembly))
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_CantInvokeIRemoteDispatch"));
			}
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x000EBD70 File Offset: 0x000EAD70
		internal void CopyNonByrefOutArgsFromOriginalArgs(RemotingMethodCachedData methodCache, object[] args, ref object[] marshalResponseArgs)
		{
			int[] nonRefOutArgMap = methodCache.NonRefOutArgMap;
			if (nonRefOutArgMap.Length > 0)
			{
				if (marshalResponseArgs == null)
				{
					marshalResponseArgs = new object[methodCache.Parameters.Length];
				}
				foreach (int num in nonRefOutArgMap)
				{
					marshalResponseArgs[num] = args[num];
				}
			}
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x000EBDB8 File Offset: 0x000EADB8
		internal static void PreserveThreadPrincipalIfNecessary(LogicalCallContext messageCallContext, LogicalCallContext threadCallContext)
		{
			if (threadCallContext != null && messageCallContext.Principal == null)
			{
				IPrincipal principal = threadCallContext.Principal;
				if (principal != null)
				{
					messageCallContext.Principal = principal;
				}
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x0600454B RID: 17739 RVA: 0x000EBDE1 File Offset: 0x000EADE1
		internal object ServerObject
		{
			get
			{
				return this._server;
			}
		}

		// Token: 0x0600454C RID: 17740
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object _PrivateProcessMessage(IntPtr md, object[] args, object server, int methodPtr, bool fExecuteInContext, out object[] outArgs);

		// Token: 0x0600454D RID: 17741 RVA: 0x000EBDE9 File Offset: 0x000EADE9
		public object PrivateProcessMessage(RuntimeMethodHandle md, object[] args, object server, int methodPtr, bool fExecuteInContext, out object[] outArgs)
		{
			return this._PrivateProcessMessage(md.Value, args, server, methodPtr, fExecuteInContext, out outArgs);
		}

		// Token: 0x0400226B RID: 8811
		private object _server;

		// Token: 0x0400226C RID: 8812
		private static string sIRemoteDispatch = "System.EnterpriseServices.IRemoteDispatch";

		// Token: 0x0400226D RID: 8813
		private static string sIRemoteDispatchAssembly = "System.EnterpriseServices";

		// Token: 0x0400226E RID: 8814
		private bool _bStatic;
	}
}
