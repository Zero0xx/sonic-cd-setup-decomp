using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DD RID: 2013
	internal sealed class BinaryMethodReturn : IStreamable
	{
		// Token: 0x0600474B RID: 18251 RVA: 0x000F42FC File Offset: 0x000F32FC
		internal BinaryMethodReturn()
		{
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x000F430C File Offset: 0x000F330C
		internal object[] WriteArray(object returnValue, object[] args, Exception exception, object callContext, object[] properties)
		{
			this.returnValue = returnValue;
			this.args = args;
			this.exception = exception;
			this.callContext = callContext;
			this.properties = properties;
			int num = 0;
			if (args == null || args.Length == 0)
			{
				this.messageEnum = MessageEnum.NoArgs;
			}
			else
			{
				this.argTypes = new Type[args.Length];
				this.bArgsPrimitive = true;
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] != null)
					{
						this.argTypes[i] = args[i].GetType();
						if (!BinaryConverter.IsTypePrimitive(this.argTypes[i]) && this.argTypes[i] != Converter.typeofString)
						{
							this.bArgsPrimitive = false;
							break;
						}
					}
				}
				if (this.bArgsPrimitive)
				{
					this.messageEnum = MessageEnum.ArgsInline;
				}
				else
				{
					num++;
					this.messageEnum = MessageEnum.ArgsInArray;
				}
			}
			if (returnValue == null)
			{
				this.messageEnum |= MessageEnum.NoReturnValue;
			}
			else if (returnValue.GetType() == typeof(void))
			{
				this.messageEnum |= MessageEnum.ReturnValueVoid;
			}
			else
			{
				this.returnType = returnValue.GetType();
				if (BinaryConverter.IsTypePrimitive(this.returnType) || this.returnType == Converter.typeofString)
				{
					this.messageEnum |= MessageEnum.ReturnValueInline;
				}
				else
				{
					num++;
					this.messageEnum |= MessageEnum.ReturnValueInArray;
				}
			}
			if (exception != null)
			{
				num++;
				this.messageEnum |= MessageEnum.ExceptionInArray;
			}
			if (callContext == null)
			{
				this.messageEnum |= MessageEnum.NoContext;
			}
			else if (callContext is string)
			{
				this.messageEnum |= MessageEnum.ContextInline;
			}
			else
			{
				num++;
				this.messageEnum |= MessageEnum.ContextInArray;
			}
			if (properties != null)
			{
				num++;
				this.messageEnum |= MessageEnum.PropertyInArray;
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInArray) && num == 1)
			{
				this.messageEnum ^= MessageEnum.ArgsInArray;
				this.messageEnum |= MessageEnum.ArgsIsArray;
				return args;
			}
			if (num > 0)
			{
				int num2 = 0;
				this.callA = new object[num];
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInArray))
				{
					this.callA[num2++] = args;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ReturnValueInArray))
				{
					this.callA[num2++] = returnValue;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ExceptionInArray))
				{
					this.callA[num2++] = exception;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInArray))
				{
					this.callA[num2++] = callContext;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.PropertyInArray))
				{
					this.callA[num2] = properties;
				}
				return this.callA;
			}
			return null;
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x000F45B0 File Offset: 0x000F35B0
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(22);
			sout.WriteInt32((int)this.messageEnum);
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ReturnValueInline))
			{
				IOUtil.WriteWithCode(this.returnType, this.returnValue, sout);
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInline))
			{
				IOUtil.WriteStringWithCode((string)this.callContext, sout);
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInline))
			{
				sout.WriteInt32(this.args.Length);
				for (int i = 0; i < this.args.Length; i++)
				{
					IOUtil.WriteWithCode(this.argTypes[i], this.args[i], sout);
				}
			}
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x000F465C File Offset: 0x000F365C
		public void Read(__BinaryParser input)
		{
			this.messageEnum = (MessageEnum)input.ReadInt32();
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.NoReturnValue))
			{
				this.returnValue = null;
			}
			else if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ReturnValueVoid))
			{
				this.returnValue = BinaryMethodReturn.instanceOfVoid;
			}
			else if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ReturnValueInline))
			{
				this.returnValue = IOUtil.ReadWithCode(input);
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInline))
			{
				this.scallContext = (string)IOUtil.ReadWithCode(input);
				this.callContext = new LogicalCallContext
				{
					RemotingData = 
					{
						LogicalCallID = this.scallContext
					}
				};
			}
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInline))
			{
				this.args = IOUtil.ReadArgs(input);
			}
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x000F4728 File Offset: 0x000F3728
		internal IMethodReturnMessage ReadArray(object[] returnA, IMethodCallMessage methodCallMessage, object handlerObject)
		{
			if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsIsArray))
			{
				this.args = returnA;
			}
			else
			{
				int num = 0;
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInArray))
				{
					if (returnA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.args = (object[])returnA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ReturnValueInArray))
				{
					if (returnA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.returnValue = returnA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ExceptionInArray))
				{
					if (returnA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.exception = (Exception)returnA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInArray))
				{
					if (returnA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.callContext = returnA[num++];
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.PropertyInArray))
				{
					if (returnA.Length < num)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Method"), new object[0]));
					}
					this.properties = returnA[num++];
				}
			}
			return new MethodResponse(methodCallMessage, handlerObject, new BinaryMethodReturnMessage(this.returnValue, this.args, this.exception, (LogicalCallContext)this.callContext, (object[])this.properties));
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x000F48E1 File Offset: 0x000F38E1
		public void Dump()
		{
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x000F48E4 File Offset: 0x000F38E4
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				IOUtil.FlagTest(this.messageEnum, MessageEnum.ReturnValueInline);
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ContextInline))
				{
					string text = this.callContext as string;
				}
				if (IOUtil.FlagTest(this.messageEnum, MessageEnum.ArgsInline))
				{
					for (int i = 0; i < this.args.Length; i++)
					{
					}
				}
			}
		}

		// Token: 0x0400240A RID: 9226
		private object returnValue;

		// Token: 0x0400240B RID: 9227
		private object[] args;

		// Token: 0x0400240C RID: 9228
		private Exception exception;

		// Token: 0x0400240D RID: 9229
		private object callContext;

		// Token: 0x0400240E RID: 9230
		private string scallContext;

		// Token: 0x0400240F RID: 9231
		private object properties;

		// Token: 0x04002410 RID: 9232
		private Type[] argTypes;

		// Token: 0x04002411 RID: 9233
		private bool bArgsPrimitive = true;

		// Token: 0x04002412 RID: 9234
		private MessageEnum messageEnum;

		// Token: 0x04002413 RID: 9235
		private object[] callA;

		// Token: 0x04002414 RID: 9236
		private Type returnType;

		// Token: 0x04002415 RID: 9237
		private static object instanceOfVoid = FormatterServices.GetUninitializedObject(Converter.typeofSystemVoid);
	}
}
