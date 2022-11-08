using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200053E RID: 1342
	internal static class SSPIWrapper
	{
		// Token: 0x060028F6 RID: 10486 RVA: 0x000AA5E0 File Offset: 0x000A95E0
		internal static SecurityPackageInfoClass[] EnumerateSecurityPackages(SSPIInterface SecModule)
		{
			if (SecModule.SecurityPackages == null)
			{
				lock (SecModule)
				{
					if (SecModule.SecurityPackages == null)
					{
						int num = 0;
						SafeFreeContextBuffer safeFreeContextBuffer = null;
						try
						{
							int num2 = SecModule.EnumerateSecurityPackages(out num, out safeFreeContextBuffer);
							if (num2 != 0)
							{
								throw new Win32Exception(num2);
							}
							SecurityPackageInfoClass[] array = new SecurityPackageInfoClass[num];
							if (Logging.On)
							{
								Logging.PrintInfo(Logging.Web, SR.GetString("net_log_sspi_enumerating_security_packages"));
							}
							for (int i = 0; i < num; i++)
							{
								array[i] = new SecurityPackageInfoClass(safeFreeContextBuffer, i);
								if (Logging.On)
								{
									Logging.PrintInfo(Logging.Web, "    " + array[i].Name);
								}
							}
							SecModule.SecurityPackages = array;
						}
						finally
						{
							if (safeFreeContextBuffer != null)
							{
								safeFreeContextBuffer.Close();
							}
						}
					}
				}
			}
			return SecModule.SecurityPackages;
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x000AA6C8 File Offset: 0x000A96C8
		internal static SecurityPackageInfoClass GetVerifyPackageInfo(SSPIInterface secModule, string packageName)
		{
			return SSPIWrapper.GetVerifyPackageInfo(secModule, packageName, false);
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000AA6D4 File Offset: 0x000A96D4
		internal static SecurityPackageInfoClass GetVerifyPackageInfo(SSPIInterface secModule, string packageName, bool throwIfMissing)
		{
			SecurityPackageInfoClass[] array = SSPIWrapper.EnumerateSecurityPackages(secModule);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Compare(array[i].Name, packageName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return array[i];
					}
				}
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_sspi_security_package_not_found", new object[]
				{
					packageName
				}));
			}
			if (throwIfMissing)
			{
				throw new NotSupportedException(SR.GetString("net_securitypackagesupport"));
			}
			return null;
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000AA74C File Offset: 0x000A974C
		public static SafeFreeCredentials AcquireDefaultCredential(SSPIInterface SecModule, string package, CredentialUse intent)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"AcquireDefaultCredential(package = ",
					package,
					", intent  = ",
					intent,
					")"
				}));
			}
			SafeFreeCredentials result = null;
			int num = SecModule.AcquireDefaultCredential(package, intent, out result);
			if (num != 0)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"AcquireDefaultCredential()",
						string.Format(CultureInfo.CurrentCulture, "0X{0:X}", new object[]
						{
							num
						})
					}));
				}
				throw new Win32Exception(num);
			}
			return result;
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000AA808 File Offset: 0x000A9808
		public static SafeFreeCredentials AcquireCredentialsHandle(SSPIInterface SecModule, string package, CredentialUse intent, ref AuthIdentity authdata)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"AcquireCredentialsHandle(package  = ",
					package,
					", intent   = ",
					intent,
					", authdata = ",
					authdata,
					")"
				}));
			}
			SafeFreeCredentials result = null;
			int num = SecModule.AcquireCredentialsHandle(package, intent, ref authdata, out result);
			if (num != 0)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"AcquireCredentialsHandle()",
						string.Format(CultureInfo.CurrentCulture, "0X{0:X}", new object[]
						{
							num
						})
					}));
				}
				throw new Win32Exception(num);
			}
			return result;
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000AA8D8 File Offset: 0x000A98D8
		public static SafeFreeCredentials AcquireCredentialsHandle(SSPIInterface SecModule, string package, CredentialUse intent, SecureCredential scc)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"AcquireCredentialsHandle(package = ",
					package,
					", intent  = ",
					intent,
					", scc     = ",
					scc,
					")"
				}));
			}
			SafeFreeCredentials result = null;
			int num = SecModule.AcquireCredentialsHandle(package, intent, ref scc, out result);
			if (num != 0)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"AcquireCredentialsHandle()",
						string.Format(CultureInfo.CurrentCulture, "0X{0:X}", new object[]
						{
							num
						})
					}));
				}
				throw new Win32Exception(num);
			}
			return result;
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000AA9A4 File Offset: 0x000A99A4
		internal static int InitializeSecurityContext(SSPIInterface SecModule, ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness datarep, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"InitializeSecurityContext(credential = ",
					credential.ToString(),
					", context = ",
					ValidationHelper.ToString(context),
					", targetName = ",
					targetName,
					", inFlags = ",
					inFlags,
					")"
				}));
			}
			int num = SecModule.InitializeSecurityContext(ref credential, ref context, targetName, inFlags, datarep, inputBuffer, outputBuffer, ref outFlags);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_sspi_security_context_input_buffer", new object[]
				{
					"InitializeSecurityContext",
					(inputBuffer == null) ? 0 : inputBuffer.size,
					outputBuffer.size,
					(SecurityStatus)num
				}));
			}
			return num;
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000AAA88 File Offset: 0x000A9A88
		internal static int InitializeSecurityContext(SSPIInterface SecModule, SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness datarep, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"InitializeSecurityContext(credential = ",
					credential.ToString(),
					", context = ",
					ValidationHelper.ToString(context),
					", targetName = ",
					targetName,
					", inFlags = ",
					inFlags,
					")"
				}));
			}
			int num = SecModule.InitializeSecurityContext(credential, ref context, targetName, inFlags, datarep, inputBuffers, outputBuffer, ref outFlags);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_sspi_security_context_input_buffers", new object[]
				{
					"InitializeSecurityContext",
					(inputBuffers == null) ? 0 : inputBuffers.Length,
					outputBuffer.size,
					(SecurityStatus)num
				}));
			}
			return num;
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x000AAB68 File Offset: 0x000A9B68
		internal static int AcceptSecurityContext(SSPIInterface SecModule, ref SafeFreeCredentials credential, ref SafeDeleteContext context, ContextFlags inFlags, Endianness datarep, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"AcceptSecurityContext(credential = ",
					credential.ToString(),
					", context = ",
					ValidationHelper.ToString(context),
					", inFlags = ",
					inFlags,
					")"
				}));
			}
			int num = SecModule.AcceptSecurityContext(ref credential, ref context, inputBuffer, inFlags, datarep, outputBuffer, ref outFlags);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_sspi_security_context_input_buffer", new object[]
				{
					"AcceptSecurityContext",
					(inputBuffer == null) ? 0 : inputBuffer.size,
					outputBuffer.size,
					(SecurityStatus)num
				}));
			}
			return num;
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000AAC3C File Offset: 0x000A9C3C
		internal static int AcceptSecurityContext(SSPIInterface SecModule, SafeFreeCredentials credential, ref SafeDeleteContext context, ContextFlags inFlags, Endianness datarep, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, string.Concat(new object[]
				{
					"AcceptSecurityContext(credential = ",
					credential.ToString(),
					", context = ",
					ValidationHelper.ToString(context),
					", inFlags = ",
					inFlags,
					")"
				}));
			}
			int num = SecModule.AcceptSecurityContext(credential, ref context, inputBuffers, inFlags, datarep, outputBuffer, ref outFlags);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_sspi_security_context_input_buffers", new object[]
				{
					"AcceptSecurityContext",
					(inputBuffers == null) ? 0 : inputBuffers.Length,
					outputBuffer.size,
					(SecurityStatus)num
				}));
			}
			return num;
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000AAD0C File Offset: 0x000A9D0C
		internal static int CompleteAuthToken(SSPIInterface SecModule, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers)
		{
			int num = SecModule.CompleteAuthToken(ref context, inputBuffers);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_operation_returned_something", new object[]
				{
					"CompleteAuthToken()",
					(SecurityStatus)num
				}));
			}
			return num;
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000AAD57 File Offset: 0x000A9D57
		public static int QuerySecurityContextToken(SSPIInterface SecModule, SafeDeleteContext context, out SafeCloseHandle token)
		{
			return SecModule.QuerySecurityContextToken(context, out token);
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000AAD61 File Offset: 0x000A9D61
		public static int EncryptMessage(SSPIInterface secModule, SafeDeleteContext context, SecurityBuffer[] input, uint sequenceNumber)
		{
			return SSPIWrapper.EncryptDecryptHelper(SSPIWrapper.OP.Encrypt, secModule, context, input, sequenceNumber);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000AAD6D File Offset: 0x000A9D6D
		public static int DecryptMessage(SSPIInterface secModule, SafeDeleteContext context, SecurityBuffer[] input, uint sequenceNumber)
		{
			return SSPIWrapper.EncryptDecryptHelper(SSPIWrapper.OP.Decrypt, secModule, context, input, sequenceNumber);
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000AAD79 File Offset: 0x000A9D79
		internal static int MakeSignature(SSPIInterface secModule, SafeDeleteContext context, SecurityBuffer[] input, uint sequenceNumber)
		{
			return SSPIWrapper.EncryptDecryptHelper(SSPIWrapper.OP.MakeSignature, secModule, context, input, sequenceNumber);
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000AAD85 File Offset: 0x000A9D85
		public static int VerifySignature(SSPIInterface secModule, SafeDeleteContext context, SecurityBuffer[] input, uint sequenceNumber)
		{
			return SSPIWrapper.EncryptDecryptHelper(SSPIWrapper.OP.VerifySignature, secModule, context, input, sequenceNumber);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000AAD94 File Offset: 0x000A9D94
		private unsafe static int EncryptDecryptHelper(SSPIWrapper.OP op, SSPIInterface SecModule, SafeDeleteContext context, SecurityBuffer[] input, uint sequenceNumber)
		{
			SecurityBufferDescriptor securityBufferDescriptor = new SecurityBufferDescriptor(input.Length);
			SecurityBufferStruct[] array = new SecurityBufferStruct[input.Length];
			fixed (SecurityBufferStruct* ptr = array)
			{
				securityBufferDescriptor.UnmanagedPointer = (void*)ptr;
				GCHandle[] array2 = new GCHandle[input.Length];
				byte[][] array3 = new byte[input.Length][];
				int result;
				try
				{
					for (int i = 0; i < input.Length; i++)
					{
						SecurityBuffer securityBuffer = input[i];
						array[i].count = securityBuffer.size;
						array[i].type = securityBuffer.type;
						if (securityBuffer.token == null || securityBuffer.token.Length == 0)
						{
							array[i].token = IntPtr.Zero;
						}
						else
						{
							array2[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
							array[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
							array3[i] = securityBuffer.token;
						}
					}
					int num;
					switch (op)
					{
					case SSPIWrapper.OP.Encrypt:
						num = SecModule.EncryptMessage(context, securityBufferDescriptor, sequenceNumber);
						break;
					case SSPIWrapper.OP.Decrypt:
						num = SecModule.DecryptMessage(context, securityBufferDescriptor, sequenceNumber);
						break;
					case SSPIWrapper.OP.MakeSignature:
						num = SecModule.MakeSignature(context, securityBufferDescriptor, sequenceNumber);
						break;
					case SSPIWrapper.OP.VerifySignature:
						num = SecModule.VerifySignature(context, securityBufferDescriptor, sequenceNumber);
						break;
					default:
						throw ExceptionHelper.MethodNotImplementedException;
					}
					for (int j = 0; j < input.Length; j++)
					{
						SecurityBuffer securityBuffer2 = input[j];
						securityBuffer2.size = array[j].count;
						securityBuffer2.type = array[j].type;
						checked
						{
							if (securityBuffer2.size == 0)
							{
								securityBuffer2.offset = 0;
								securityBuffer2.token = null;
							}
							else
							{
								int k;
								for (k = 0; k < input.Length; k++)
								{
									if (array3[k] != null)
									{
										byte* ptr2 = (byte*)((void*)Marshal.UnsafeAddrOfPinnedArrayElement(array3[k], 0));
										if ((void*)array[j].token >= (void*)ptr2 && (byte*)((void*)array[j].token) + securityBuffer2.size == ptr2 + array3[k].Length)
										{
											securityBuffer2.offset = (int)(unchecked((long)((byte*)((void*)array[j].token) - (byte*)ptr2)));
											securityBuffer2.token = array3[k];
											break;
										}
									}
								}
								if (k >= input.Length)
								{
									securityBuffer2.size = 0;
									securityBuffer2.offset = 0;
									securityBuffer2.token = null;
								}
							}
						}
					}
					if (num != 0 && Logging.On)
					{
						if (num == 590625)
						{
							Logging.PrintError(Logging.Web, SR.GetString("net_log_operation_returned_something", new object[]
							{
								op,
								"SEC_I_RENEGOTIATE"
							}));
						}
						else
						{
							Logging.PrintError(Logging.Web, SR.GetString("net_log_operation_failed_with_error", new object[]
							{
								op,
								string.Format(CultureInfo.CurrentCulture, "0X{0:X}", new object[]
								{
									num
								})
							}));
						}
					}
					result = num;
				}
				finally
				{
					for (int l = 0; l < array2.Length; l++)
					{
						if (array2[l].IsAllocated)
						{
							array2[l].Free();
						}
					}
				}
				return result;
			}
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000AB108 File Offset: 0x000AA108
		public static SafeFreeContextBufferChannelBinding QueryContextChannelBinding(SSPIInterface SecModule, SafeDeleteContext securityContext, ContextAttribute contextAttribute)
		{
			SafeFreeContextBufferChannelBinding result;
			int num = SecModule.QueryContextChannelBinding(securityContext, contextAttribute, out result);
			if (num != 0)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000AB128 File Offset: 0x000AA128
		public static object QueryContextAttributes(SSPIInterface SecModule, SafeDeleteContext securityContext, ContextAttribute contextAttribute)
		{
			int num;
			return SSPIWrapper.QueryContextAttributes(SecModule, securityContext, contextAttribute, out num);
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000AB140 File Offset: 0x000AA140
		public unsafe static object QueryContextAttributes(SSPIInterface SecModule, SafeDeleteContext securityContext, ContextAttribute contextAttribute, out int errorCode)
		{
			int num = IntPtr.Size;
			Type handleType = null;
			if (contextAttribute <= ContextAttribute.NegotiationInfo)
			{
				switch (contextAttribute)
				{
				case ContextAttribute.Sizes:
					num = SecSizes.SizeOf;
					goto IL_147;
				case ContextAttribute.Names:
					handleType = typeof(SafeFreeContextBuffer);
					goto IL_147;
				case ContextAttribute.Lifespan:
				case ContextAttribute.DceInfo:
					break;
				case ContextAttribute.StreamSizes:
					num = StreamSizes.SizeOf;
					goto IL_147;
				default:
					switch (contextAttribute)
					{
					case ContextAttribute.PackageInfo:
						handleType = typeof(SafeFreeContextBuffer);
						goto IL_147;
					case ContextAttribute.NegotiationInfo:
						handleType = typeof(SafeFreeContextBuffer);
						num = Marshal.SizeOf(typeof(NegotiationInfo));
						goto IL_147;
					}
					break;
				}
			}
			else
			{
				if (contextAttribute == ContextAttribute.ClientSpecifiedSpn)
				{
					handleType = typeof(SafeFreeContextBuffer);
					goto IL_147;
				}
				switch (contextAttribute)
				{
				case ContextAttribute.RemoteCertificate:
					handleType = typeof(SafeFreeCertContext);
					goto IL_147;
				case ContextAttribute.LocalCertificate:
					handleType = typeof(SafeFreeCertContext);
					goto IL_147;
				default:
					switch (contextAttribute)
					{
					case ContextAttribute.IssuerListInfoEx:
						num = Marshal.SizeOf(typeof(IssuerListInfoEx));
						handleType = typeof(SafeFreeContextBuffer);
						goto IL_147;
					case ContextAttribute.ConnectionInfo:
						num = Marshal.SizeOf(typeof(SslConnectionInfo));
						goto IL_147;
					}
					break;
				}
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
			{
				"ContextAttribute"
			}), "contextAttribute");
			IL_147:
			SafeHandle safeHandle = null;
			object result = null;
			try
			{
				byte[] array = new byte[num];
				errorCode = SecModule.QueryContextAttributes(securityContext, contextAttribute, array, handleType, out safeHandle);
				if (errorCode != 0)
				{
					return null;
				}
				if (contextAttribute <= ContextAttribute.NegotiationInfo)
				{
					switch (contextAttribute)
					{
					case ContextAttribute.Sizes:
						result = new SecSizes(array);
						goto IL_294;
					case ContextAttribute.Names:
						if (ComNetOS.IsWin9x)
						{
							result = Marshal.PtrToStringAnsi(safeHandle.DangerousGetHandle());
							goto IL_294;
						}
						result = Marshal.PtrToStringUni(safeHandle.DangerousGetHandle());
						goto IL_294;
					case ContextAttribute.Lifespan:
					case ContextAttribute.DceInfo:
						goto IL_294;
					case ContextAttribute.StreamSizes:
						result = new StreamSizes(array);
						goto IL_294;
					default:
						switch (contextAttribute)
						{
						case ContextAttribute.PackageInfo:
							result = new SecurityPackageInfoClass(safeHandle, 0);
							goto IL_294;
						case (ContextAttribute)11:
							goto IL_294;
						case ContextAttribute.NegotiationInfo:
							try
							{
								fixed (void* ptr = array)
								{
									result = new NegotiationInfoClass(safeHandle, Marshal.ReadInt32(new IntPtr(ptr), NegotiationInfo.NegotiationStateOffest));
									goto IL_294;
								}
							}
							finally
							{
								void* ptr = null;
							}
							break;
						default:
							goto IL_294;
						}
						break;
					}
				}
				else if (contextAttribute != ContextAttribute.ClientSpecifiedSpn)
				{
					switch (contextAttribute)
					{
					case ContextAttribute.RemoteCertificate:
					case ContextAttribute.LocalCertificate:
						result = safeHandle;
						safeHandle = null;
						goto IL_294;
					default:
						switch (contextAttribute)
						{
						case ContextAttribute.IssuerListInfoEx:
							result = new IssuerListInfoEx(safeHandle, array);
							safeHandle = null;
							goto IL_294;
						case ContextAttribute.ConnectionInfo:
							result = new SslConnectionInfo(array);
							goto IL_294;
						default:
							goto IL_294;
						}
						break;
					}
				}
				result = Marshal.PtrToStringUni(safeHandle.DangerousGetHandle());
				IL_294:;
			}
			finally
			{
				if (safeHandle != null)
				{
					safeHandle.Close();
				}
			}
			return result;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000AB428 File Offset: 0x000AA428
		public static string ErrorDescription(int errorCode)
		{
			if (errorCode == -1)
			{
				return "An exception when invoking Win32 API";
			}
			SecurityStatus securityStatus = (SecurityStatus)errorCode;
			if (securityStatus <= SecurityStatus.MessageAltered)
			{
				switch (securityStatus)
				{
				case SecurityStatus.InvalidHandle:
					return "Invalid handle";
				case SecurityStatus.Unsupported:
				case SecurityStatus.InternalError:
					break;
				case SecurityStatus.TargetUnknown:
					return "Target unknown";
				case SecurityStatus.PackageNotFound:
					return "Package not found";
				default:
					if (securityStatus == SecurityStatus.InvalidToken)
					{
						return "Invalid token";
					}
					if (securityStatus == SecurityStatus.MessageAltered)
					{
						return "Message altered";
					}
					break;
				}
			}
			else
			{
				if (securityStatus == SecurityStatus.IncompleteMessage)
				{
					return "Message incomplete";
				}
				switch (securityStatus)
				{
				case SecurityStatus.BufferNotEnough:
					return "Buffer not enough";
				case SecurityStatus.WrongPrincipal:
					return "Wrong principal";
				case (SecurityStatus)(-2146893021):
				case SecurityStatus.TimeSkew:
					break;
				case SecurityStatus.UntrustedRoot:
					return "Untrusted root";
				default:
					if (securityStatus == SecurityStatus.ContinueNeeded)
					{
						return "Continue needed";
					}
					break;
				}
			}
			return "0x" + errorCode.ToString("x", NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0200053F RID: 1343
		private enum OP
		{
			// Token: 0x040027CE RID: 10190
			Encrypt = 1,
			// Token: 0x040027CF RID: 10191
			Decrypt,
			// Token: 0x040027D0 RID: 10192
			MakeSignature,
			// Token: 0x040027D1 RID: 10193
			VerifySignature
		}
	}
}
