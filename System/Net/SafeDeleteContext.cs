using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	// Token: 0x02000521 RID: 1313
	internal abstract class SafeDeleteContext : SafeHandle
	{
		// Token: 0x0600284E RID: 10318 RVA: 0x000A5FA1 File Offset: 0x000A4FA1
		protected SafeDeleteContext() : base(IntPtr.Zero, true)
		{
			this._handle = default(SSPIHandle);
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x000A5FBB File Offset: 0x000A4FBB
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this._handle.IsZero;
			}
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000A5FD2 File Offset: 0x000A4FD2
		public override string ToString()
		{
			return this._handle.ToString();
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000A5FE8 File Offset: 0x000A4FE8
		internal unsafe static int InitializeSecurityContext(SecurDll dll, ref SafeFreeCredentials inCredentials, ref SafeDeleteContext refContext, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inSecBuffer, SecurityBuffer[] inSecBuffers, SecurityBuffer outSecBuffer, ref ContextFlags outFlags)
		{
			if (inCredentials == null)
			{
				throw new ArgumentNullException("inCredentials");
			}
			SecurityBufferDescriptor securityBufferDescriptor = null;
			if (inSecBuffer != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(1);
			}
			else if (inSecBuffers != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(inSecBuffers.Length);
			}
			SecurityBufferDescriptor securityBufferDescriptor2 = new SecurityBufferDescriptor(1);
			bool flag = (inFlags & ContextFlags.AllocateMemory) != ContextFlags.Zero;
			int result = -1;
			SSPIHandle sspihandle = default(SSPIHandle);
			if (refContext != null)
			{
				sspihandle = refContext._handle;
			}
			GCHandle[] array = null;
			GCHandle gchandle = default(GCHandle);
			SafeFreeContextBuffer safeFreeContextBuffer = null;
			try
			{
				gchandle = GCHandle.Alloc(outSecBuffer.token, GCHandleType.Pinned);
				SecurityBufferStruct[] array2 = new SecurityBufferStruct[(securityBufferDescriptor == null) ? 1 : securityBufferDescriptor.Count];
				try
				{
					fixed (void* ptr = array2)
					{
						if (securityBufferDescriptor != null)
						{
							securityBufferDescriptor.UnmanagedPointer = ptr;
							array = new GCHandle[securityBufferDescriptor.Count];
							for (int i = 0; i < securityBufferDescriptor.Count; i++)
							{
								SecurityBuffer securityBuffer = (inSecBuffer != null) ? inSecBuffer : inSecBuffers[i];
								if (securityBuffer != null)
								{
									array2[i].count = securityBuffer.size;
									array2[i].type = securityBuffer.type;
									if (securityBuffer.unmanagedToken != null)
									{
										array2[i].token = securityBuffer.unmanagedToken.DangerousGetHandle();
									}
									else if (securityBuffer.token == null || securityBuffer.token.Length == 0)
									{
										array2[i].token = IntPtr.Zero;
									}
									else
									{
										array[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
										array2[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
									}
								}
							}
						}
						SecurityBufferStruct[] array3 = new SecurityBufferStruct[1];
						try
						{
							fixed (void* ptr2 = array3)
							{
								securityBufferDescriptor2.UnmanagedPointer = ptr2;
								array3[0].count = outSecBuffer.size;
								array3[0].type = outSecBuffer.type;
								if (outSecBuffer.token == null || outSecBuffer.token.Length == 0)
								{
									array3[0].token = IntPtr.Zero;
								}
								else
								{
									array3[0].token = Marshal.UnsafeAddrOfPinnedArrayElement(outSecBuffer.token, outSecBuffer.offset);
								}
								if (flag)
								{
									safeFreeContextBuffer = SafeFreeContextBuffer.CreateEmptyHandle(dll);
								}
								switch (dll)
								{
								case SecurDll.SECURITY:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SECURITY();
									}
									if (targetName == null || targetName.Length == 0)
									{
										targetName = " ";
									}
									try
									{
										fixed (char* ptr3 = targetName)
										{
											result = SafeDeleteContext.MustRunInitializeSecurityContext_SECURITY(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), (byte*)((targetName == " ") ? null : ptr3), inFlags, endianness, securityBufferDescriptor, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
											goto IL_44B;
										}
									}
									finally
									{
										string text = null;
									}
									break;
								case SecurDll.SECUR32:
									break;
								case SecurDll.SCHANNEL:
									goto IL_381;
								default:
									goto IL_423;
								}
								if (refContext == null || refContext.IsInvalid)
								{
									refContext = new SafeDeleteContext_SECUR32();
								}
								byte[] array4 = SafeDeleteContext.dummyBytes;
								if (targetName != null && targetName.Length != 0)
								{
									array4 = new byte[targetName.Length + 2];
									Encoding.Default.GetBytes(targetName, 0, targetName.Length, array4, 0);
								}
								try
								{
									fixed (byte* ptr4 = array4)
									{
										result = SafeDeleteContext.MustRunInitializeSecurityContext_SECUR32(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), (array4 == SafeDeleteContext.dummyBytes) ? null : ptr4, inFlags, endianness, securityBufferDescriptor, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
										goto IL_44B;
									}
								}
								finally
								{
									byte* ptr4 = null;
								}
								IL_381:
								if (refContext == null || refContext.IsInvalid)
								{
									refContext = new SafeDeleteContext_SCHANNEL();
								}
								byte[] array5 = SafeDeleteContext.dummyBytes;
								if (targetName != null && targetName.Length != 0)
								{
									array5 = new byte[targetName.Length + 2];
									Encoding.Default.GetBytes(targetName, 0, targetName.Length, array5, 0);
								}
								try
								{
									fixed (byte* ptr5 = array5)
									{
										result = SafeDeleteContext.MustRunInitializeSecurityContext_SCHANNEL(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), (array5 == SafeDeleteContext.dummyBytes) ? null : ptr5, inFlags, endianness, securityBufferDescriptor, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
										goto IL_44B;
									}
								}
								finally
								{
									byte* ptr5 = null;
								}
								IL_423:
								throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
								{
									"SecurDll"
								}), "Dll");
								IL_44B:
								outSecBuffer.size = array3[0].count;
								outSecBuffer.type = array3[0].type;
								if (outSecBuffer.size > 0)
								{
									outSecBuffer.token = new byte[outSecBuffer.size];
									Marshal.Copy(array3[0].token, outSecBuffer.token, 0, outSecBuffer.size);
								}
								else
								{
									outSecBuffer.token = null;
								}
							}
						}
						finally
						{
							void* ptr2 = null;
						}
					}
				}
				finally
				{
					void* ptr = null;
				}
			}
			finally
			{
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].IsAllocated)
						{
							array[j].Free();
						}
					}
				}
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (safeFreeContextBuffer != null)
				{
					safeFreeContextBuffer.Close();
				}
			}
			return result;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000A65A8 File Offset: 0x000A55A8
		private unsafe static int MustRunInitializeSecurityContext_SECURITY(ref SafeFreeCredentials inCredentials, void* inContextPtr, byte* targetName, ContextFlags inFlags, Endianness endianness, SecurityBufferDescriptor inputBuffer, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags attributes, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (!flag)
				{
					inCredentials = null;
				}
				else if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.InitializeSecurityContextW(ref handle, inContextPtr, targetName, inFlags, 0, endianness, inputBuffer, 0, ref outContext._handle, outputBuffer, ref attributes, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)-2147483648)) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000A66EC File Offset: 0x000A56EC
		private unsafe static int MustRunInitializeSecurityContext_SECUR32(ref SafeFreeCredentials inCredentials, void* inContextPtr, byte* targetName, ContextFlags inFlags, Endianness endianness, SecurityBufferDescriptor inputBuffer, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags attributes, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.InitializeSecurityContextA(ref handle, inContextPtr, targetName, inFlags, 0, endianness, inputBuffer, 0, ref outContext._handle, outputBuffer, ref attributes, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)-2147483648)) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000A6824 File Offset: 0x000A5824
		private unsafe static int MustRunInitializeSecurityContext_SCHANNEL(ref SafeFreeCredentials inCredentials, void* inContextPtr, byte* targetName, ContextFlags inFlags, Endianness endianness, SecurityBufferDescriptor inputBuffer, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags attributes, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.InitializeSecurityContextA(ref handle, inContextPtr, targetName, inFlags, 0, endianness, inputBuffer, 0, ref outContext._handle, outputBuffer, ref attributes, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)-2147483648)) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000A695C File Offset: 0x000A595C
		internal unsafe static int AcceptSecurityContext(SecurDll dll, ref SafeFreeCredentials inCredentials, ref SafeDeleteContext refContext, ContextFlags inFlags, Endianness endianness, SecurityBuffer inSecBuffer, SecurityBuffer[] inSecBuffers, SecurityBuffer outSecBuffer, ref ContextFlags outFlags)
		{
			if (inCredentials == null)
			{
				throw new ArgumentNullException("inCredentials");
			}
			SecurityBufferDescriptor securityBufferDescriptor = null;
			if (inSecBuffer != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(1);
			}
			else if (inSecBuffers != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(inSecBuffers.Length);
			}
			SecurityBufferDescriptor securityBufferDescriptor2 = new SecurityBufferDescriptor(1);
			bool flag = (inFlags & ContextFlags.AllocateMemory) != ContextFlags.Zero;
			int result = -1;
			SSPIHandle sspihandle = default(SSPIHandle);
			if (refContext != null)
			{
				sspihandle = refContext._handle;
			}
			GCHandle[] array = null;
			GCHandle gchandle = default(GCHandle);
			SafeFreeContextBuffer safeFreeContextBuffer = null;
			try
			{
				gchandle = GCHandle.Alloc(outSecBuffer.token, GCHandleType.Pinned);
				SecurityBufferStruct[] array2 = new SecurityBufferStruct[(securityBufferDescriptor == null) ? 1 : securityBufferDescriptor.Count];
				try
				{
					fixed (void* ptr = array2)
					{
						if (securityBufferDescriptor != null)
						{
							securityBufferDescriptor.UnmanagedPointer = ptr;
							array = new GCHandle[securityBufferDescriptor.Count];
							for (int i = 0; i < securityBufferDescriptor.Count; i++)
							{
								SecurityBuffer securityBuffer = (inSecBuffer != null) ? inSecBuffer : inSecBuffers[i];
								if (securityBuffer != null)
								{
									array2[i].count = securityBuffer.size;
									array2[i].type = securityBuffer.type;
									if (securityBuffer.unmanagedToken != null)
									{
										array2[i].token = securityBuffer.unmanagedToken.DangerousGetHandle();
									}
									else if (securityBuffer.token == null || securityBuffer.token.Length == 0)
									{
										array2[i].token = IntPtr.Zero;
									}
									else
									{
										array[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
										array2[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
									}
								}
							}
						}
						SecurityBufferStruct[] array3 = new SecurityBufferStruct[1];
						try
						{
							fixed (void* ptr2 = array3)
							{
								securityBufferDescriptor2.UnmanagedPointer = ptr2;
								array3[0].count = outSecBuffer.size;
								array3[0].type = outSecBuffer.type;
								if (outSecBuffer.token == null || outSecBuffer.token.Length == 0)
								{
									array3[0].token = IntPtr.Zero;
								}
								else
								{
									array3[0].token = Marshal.UnsafeAddrOfPinnedArrayElement(outSecBuffer.token, outSecBuffer.offset);
								}
								if (flag)
								{
									safeFreeContextBuffer = SafeFreeContextBuffer.CreateEmptyHandle(dll);
								}
								switch (dll)
								{
								case SecurDll.SECURITY:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SECURITY();
									}
									result = SafeDeleteContext.MustRunAcceptSecurityContext_SECURITY(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor, inFlags, endianness, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
									break;
								case SecurDll.SECUR32:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SECUR32();
									}
									result = SafeDeleteContext.MustRunAcceptSecurityContext_SECUR32(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor, inFlags, endianness, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
									break;
								case SecurDll.SCHANNEL:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SCHANNEL();
									}
									result = SafeDeleteContext.MustRunAcceptSecurityContext_SCHANNEL(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor, inFlags, endianness, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
									break;
								default:
									throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
									{
										"SecurDll"
									}), "Dll");
								}
								outSecBuffer.size = array3[0].count;
								outSecBuffer.type = array3[0].type;
								if (outSecBuffer.size > 0)
								{
									outSecBuffer.token = new byte[outSecBuffer.size];
									Marshal.Copy(array3[0].token, outSecBuffer.token, 0, outSecBuffer.size);
								}
								else
								{
									outSecBuffer.token = null;
								}
							}
						}
						finally
						{
							void* ptr2 = null;
						}
					}
				}
				finally
				{
					void* ptr = null;
				}
			}
			finally
			{
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].IsAllocated)
						{
							array[j].Free();
						}
					}
				}
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (safeFreeContextBuffer != null)
				{
					safeFreeContextBuffer.Close();
				}
			}
			return result;
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000A6DC4 File Offset: 0x000A5DC4
		private unsafe static int MustRunAcceptSecurityContext_SECURITY(ref SafeFreeCredentials inCredentials, void* inContextPtr, SecurityBufferDescriptor inputBuffer, ContextFlags inFlags, Endianness endianness, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags outFlags, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (!flag)
				{
					inCredentials = null;
				}
				else if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcceptSecurityContext(ref handle, inContextPtr, inputBuffer, inFlags, endianness, ref outContext._handle, outputBuffer, ref outFlags, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)-2147483648)) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x000A6F04 File Offset: 0x000A5F04
		private unsafe static int MustRunAcceptSecurityContext_SECUR32(ref SafeFreeCredentials inCredentials, void* inContextPtr, SecurityBufferDescriptor inputBuffer, ContextFlags inFlags, Endianness endianness, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags outFlags, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.AcceptSecurityContext(ref handle, inContextPtr, inputBuffer, inFlags, endianness, ref outContext._handle, outputBuffer, ref outFlags, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)-2147483648)) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000A7038 File Offset: 0x000A6038
		private unsafe static int MustRunAcceptSecurityContext_SCHANNEL(ref SafeFreeCredentials inCredentials, void* inContextPtr, SecurityBufferDescriptor inputBuffer, ContextFlags inFlags, Endianness endianness, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags outFlags, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.AcceptSecurityContext(ref handle, inContextPtr, inputBuffer, inFlags, endianness, ref outContext._handle, outputBuffer, ref outFlags, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)-2147483648)) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)-2147483648)) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000A716C File Offset: 0x000A616C
		internal unsafe static int CompleteAuthToken(SecurDll dll, ref SafeDeleteContext refContext, SecurityBuffer[] inSecBuffers)
		{
			SecurityBufferDescriptor securityBufferDescriptor = new SecurityBufferDescriptor(inSecBuffers.Length);
			int result = -2146893055;
			GCHandle[] array = null;
			SecurityBufferStruct[] array2 = new SecurityBufferStruct[securityBufferDescriptor.Count];
			fixed (void* ptr = array2)
			{
				securityBufferDescriptor.UnmanagedPointer = ptr;
				array = new GCHandle[securityBufferDescriptor.Count];
				for (int i = 0; i < securityBufferDescriptor.Count; i++)
				{
					SecurityBuffer securityBuffer = inSecBuffers[i];
					if (securityBuffer != null)
					{
						array2[i].count = securityBuffer.size;
						array2[i].type = securityBuffer.type;
						if (securityBuffer.unmanagedToken != null)
						{
							array2[i].token = securityBuffer.unmanagedToken.DangerousGetHandle();
						}
						else if (securityBuffer.token == null || securityBuffer.token.Length == 0)
						{
							array2[i].token = IntPtr.Zero;
						}
						else
						{
							array[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
							array2[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
						}
					}
				}
				SSPIHandle sspihandle = default(SSPIHandle);
				if (refContext != null)
				{
					sspihandle = refContext._handle;
				}
				try
				{
					if (dll == SecurDll.SECURITY)
					{
						if (refContext == null || refContext.IsInvalid)
						{
							refContext = new SafeDeleteContext_SECURITY();
						}
						bool flag = false;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							try
							{
								refContext.DangerousAddRef(ref flag);
							}
							catch (Exception ex)
							{
								if (flag)
								{
									refContext.DangerousRelease();
									flag = false;
								}
								if (!(ex is ObjectDisposedException))
								{
									throw;
								}
							}
							goto IL_1CD;
						}
						finally
						{
							if (flag)
							{
								result = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.CompleteAuthToken(sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor);
								refContext.DangerousRelease();
							}
						}
						goto IL_1A5;
						IL_1CD:
						goto IL_201;
					}
					IL_1A5:
					throw new ArgumentException(SR.GetString("net_invalid_enum", new object[]
					{
						"SecurDll"
					}), "Dll");
				}
				finally
				{
					if (array != null)
					{
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j].IsAllocated)
							{
								array[j].Free();
							}
						}
					}
				}
				IL_201:;
			}
			return result;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x000A73A8 File Offset: 0x000A63A8
		// Note: this type is marked as 'beforefieldinit'.
		static SafeDeleteContext()
		{
			byte[] array = new byte[1];
			SafeDeleteContext.dummyBytes = array;
		}

		// Token: 0x04002777 RID: 10103
		private const string dummyStr = " ";

		// Token: 0x04002778 RID: 10104
		private static readonly byte[] dummyBytes;

		// Token: 0x04002779 RID: 10105
		internal SSPIHandle _handle;

		// Token: 0x0400277A RID: 10106
		protected SafeFreeCredentials _EffectiveCredential;
	}
}
