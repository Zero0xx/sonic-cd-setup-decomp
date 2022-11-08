﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004EE RID: 1262
	internal class SSPIAuthType : SSPIInterface
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x000A26B5 File Offset: 0x000A16B5
		// (set) Token: 0x06002772 RID: 10098 RVA: 0x000A26BC File Offset: 0x000A16BC
		public SecurityPackageInfoClass[] SecurityPackages
		{
			get
			{
				return SSPIAuthType.m_SecurityPackages;
			}
			set
			{
				SSPIAuthType.m_SecurityPackages = value;
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000A26C4 File Offset: 0x000A16C4
		public int EnumerateSecurityPackages(out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			return SafeFreeContextBuffer.EnumeratePackages(SSPIAuthType.Library, out pkgnum, out pkgArray);
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000A26D2 File Offset: 0x000A16D2
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000A26E3 File Offset: 0x000A16E3
		public int AcquireDefaultCredential(string moduleName, CredentialUse usage, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireDefaultCredential(SSPIAuthType.Library, moduleName, usage, out outCredential);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000A26F2 File Offset: 0x000A16F2
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000A2704 File Offset: 0x000A1704
		public int AcceptSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer inputBuffer, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPIAuthType.Library, ref credential, ref context, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000A2728 File Offset: 0x000A1728
		public int AcceptSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPIAuthType.Library, ref credential, ref context, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000A274C File Offset: 0x000A174C
		public int InitializeSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPIAuthType.Library, ref credential, ref context, targetName, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000A2774 File Offset: 0x000A1774
		public int InitializeSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPIAuthType.Library, ref credential, ref context, targetName, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000A279C File Offset: 0x000A179C
		private int EncryptMessageHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int result = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					result = UnsafeNclNativeMethods.NativeNTSSPI.EncryptMessage(ref context._handle, 0U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000A282C File Offset: 0x000A182C
		public int EncryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.EncryptMessageHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000A2844 File Offset: 0x000A1844
		private unsafe int DecryptMessageHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			uint num2 = 0U;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, &num2);
					context.DangerousRelease();
				}
			}
			if (num == 0 && num2 == 2147483649U)
			{
				throw new InvalidOperationException(SR.GetString("net_auth_message_not_encrypted"));
			}
			return num;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000A28F4 File Offset: 0x000A18F4
		public int DecryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.DecryptMessageHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000A290C File Offset: 0x000A190C
		private int MakeSignatureHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int result = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					result = UnsafeNclNativeMethods.NativeNTSSPI.EncryptMessage(ref context._handle, 2147483649U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000A29A0 File Offset: 0x000A19A0
		public int MakeSignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.MakeSignatureHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000A29B8 File Offset: 0x000A19B8
		private unsafe int VerifySignatureHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int result = -2146893055;
			bool flag = false;
			uint num = 0U;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					result = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, &num);
					context.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000A2A4C File Offset: 0x000A1A4C
		public int VerifySignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.VerifySignatureHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000A2A64 File Offset: 0x000A1A64
		public int QueryContextChannelBinding(SafeDeleteContext context, ContextAttribute attribute, out SafeFreeContextBufferChannelBinding binding)
		{
			binding = null;
			throw new NotSupportedException();
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000A2A70 File Offset: 0x000A1A70
		public unsafe int QueryContextAttributes(SafeDeleteContext context, ContextAttribute attribute, byte[] buffer, Type handleType, out SafeHandle refHandle)
		{
			refHandle = null;
			if (handleType != null)
			{
				if (handleType == typeof(SafeFreeContextBuffer))
				{
					refHandle = SafeFreeContextBuffer.CreateEmptyHandle(SSPIAuthType.Library);
				}
				else
				{
					if (handleType != typeof(SafeFreeCertContext))
					{
						throw new ArgumentException(SR.GetString("SSPIInvalidHandleType", new object[]
						{
							handleType.FullName
						}), "handleType");
					}
					refHandle = new SafeFreeCertContext();
				}
			}
			fixed (byte* ptr = buffer)
			{
				return SafeFreeContextBuffer.QueryContextAttributes(SSPIAuthType.Library, context, attribute, ptr, refHandle);
			}
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000A2B0D File Offset: 0x000A1B0D
		public int QuerySecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle phToken)
		{
			if (ComNetOS.IsWin9x)
			{
				throw new NotSupportedException();
			}
			return SafeCloseHandle.GetSecurityContextToken(phContext, out phToken);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000A2B23 File Offset: 0x000A1B23
		public int CompleteAuthToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			if (ComNetOS.IsWin9x)
			{
				throw new NotSupportedException();
			}
			return SafeDeleteContext.CompleteAuthToken(SSPIAuthType.Library, ref refContext, inputBuffers);
		}

		// Token: 0x040026B3 RID: 9907
		private static readonly SecurDll Library = ComNetOS.IsWin9x ? SecurDll.SECUR32 : SecurDll.SECURITY;

		// Token: 0x040026B4 RID: 9908
		private static SecurityPackageInfoClass[] m_SecurityPackages;
	}
}
