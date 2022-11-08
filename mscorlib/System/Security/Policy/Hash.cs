using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Util;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Policy
{
	// Token: 0x020004C0 RID: 1216
	[ComVisible(true)]
	[Serializable]
	public sealed class Hash : ISerializable, IBuiltInEvidence
	{
		// Token: 0x06003098 RID: 12440 RVA: 0x000A691D File Offset: 0x000A591D
		internal Hash()
		{
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000A6930 File Offset: 0x000A5930
		internal Hash(SerializationInfo info, StreamingContext context)
		{
			this.m_md5 = (byte[])info.GetValueNoThrow("Md5", typeof(byte[]));
			this.m_sha1 = (byte[])info.GetValueNoThrow("Sha1", typeof(byte[]));
			this.m_peFile = SafePEFileHandle.InvalidHandle;
			this.m_rawData = (byte[])info.GetValue("RawData", typeof(byte[]));
			if (this.m_rawData == null)
			{
				IntPtr intPtr = (IntPtr)info.GetValue("PEFile", typeof(IntPtr));
				if (intPtr != IntPtr.Zero)
				{
					Hash._SetPEFileHandle(intPtr, ref this.m_peFile);
				}
			}
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000A69F5 File Offset: 0x000A59F5
		public Hash(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			Hash._GetPEFileFromAssembly(assembly.InternalAssembly, ref this.m_peFile);
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000A6A28 File Offset: 0x000A5A28
		public static Hash CreateSHA1(byte[] sha1)
		{
			if (sha1 == null)
			{
				throw new ArgumentNullException("sha1");
			}
			Hash hash = new Hash();
			hash.m_sha1 = new byte[sha1.Length];
			Array.Copy(sha1, hash.m_sha1, sha1.Length);
			return hash;
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000A6A68 File Offset: 0x000A5A68
		public static Hash CreateMD5(byte[] md5)
		{
			if (md5 == null)
			{
				throw new ArgumentNullException("md5");
			}
			Hash hash = new Hash();
			hash.m_md5 = new byte[md5.Length];
			Array.Copy(md5, hash.m_md5, md5.Length);
			return hash;
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000A6AA8 File Offset: 0x000A5AA8
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			info.AddValue("Md5", this.m_md5);
			info.AddValue("Sha1", this.m_sha1);
			if (context.State != StreamingContextStates.Clone && context.State != StreamingContextStates.CrossAppDomain)
			{
				if (!this.m_peFile.IsInvalid)
				{
					this.m_rawData = this.RawData;
				}
				info.AddValue("PEFile", IntPtr.Zero);
				info.AddValue("RawData", this.m_rawData);
				return;
			}
			info.AddValue("PEFile", this.m_peFile.DangerousGetHandle());
			if (this.m_peFile.IsInvalid)
			{
				info.AddValue("RawData", this.m_rawData);
				return;
			}
			info.AddValue("RawData", null);
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x000A6B84 File Offset: 0x000A5B84
		internal byte[] RawData
		{
			get
			{
				if (this.m_rawData == null)
				{
					if (this.m_peFile.IsInvalid)
					{
						throw new SecurityException(Environment.GetResourceString("Security_CannotGetRawData"));
					}
					byte[] array = Hash._GetRawData(this.m_peFile);
					if (array == null)
					{
						throw new SecurityException(Environment.GetResourceString("Security_CannotGenerateHash"));
					}
					this.m_rawData = array;
				}
				return this.m_rawData;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x000A6BE4 File Offset: 0x000A5BE4
		public byte[] SHA1
		{
			get
			{
				if (this.m_sha1 == null)
				{
					SHA1 sha = new SHA1Managed();
					this.m_sha1 = sha.ComputeHash(this.RawData);
				}
				byte[] array = new byte[this.m_sha1.Length];
				Array.Copy(this.m_sha1, array, this.m_sha1.Length);
				return array;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060030A0 RID: 12448 RVA: 0x000A6C34 File Offset: 0x000A5C34
		public byte[] MD5
		{
			get
			{
				if (this.m_md5 == null)
				{
					MD5 md = new MD5CryptoServiceProvider();
					this.m_md5 = md.ComputeHash(this.RawData);
				}
				byte[] array = new byte[this.m_md5.Length];
				Array.Copy(this.m_md5, array, this.m_md5.Length);
				return array;
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000A6C84 File Offset: 0x000A5C84
		public byte[] GenerateHash(HashAlgorithm hashAlg)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			if (hashAlg is SHA1)
			{
				return this.SHA1;
			}
			if (hashAlg is MD5)
			{
				return this.MD5;
			}
			return hashAlg.ComputeHash(this.RawData);
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000A6CC0 File Offset: 0x000A5CC0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			if (!verbose)
			{
				return position;
			}
			buffer[position++] = '\b';
			IntPtr value = IntPtr.Zero;
			if (!this.m_peFile.IsInvalid)
			{
				value = this.m_peFile.DangerousGetHandle();
			}
			BuiltInEvidenceHelper.CopyLongToCharArray((long)value, buffer, position);
			return position + 4;
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000A6D0A File Offset: 0x000A5D0A
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return 5;
			}
			return 0;
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000A6D14 File Offset: 0x000A5D14
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			this.m_peFile = SafePEFileHandle.InvalidHandle;
			IntPtr inHandle = (IntPtr)BuiltInEvidenceHelper.GetLongFromCharArray(buffer, position);
			Hash._SetPEFileHandle(inHandle, ref this.m_peFile);
			return position + 4;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000A6D48 File Offset: 0x000A5D48
		private SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Hash");
			securityElement.AddAttribute("version", "1");
			securityElement.AddChild(new SecurityElement("RawData", Hex.EncodeHexString(this.RawData)));
			return securityElement;
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000A6D8C File Offset: 0x000A5D8C
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x060030A7 RID: 12455
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte[] _GetRawData(SafePEFileHandle handle);

		// Token: 0x060030A8 RID: 12456
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetPEFileFromAssembly(Assembly assembly, ref SafePEFileHandle handle);

		// Token: 0x060030A9 RID: 12457
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _ReleasePEFile(IntPtr handle);

		// Token: 0x060030AA RID: 12458
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _SetPEFileHandle(IntPtr inHandle, ref SafePEFileHandle outHandle);

		// Token: 0x04001870 RID: 6256
		private SafePEFileHandle m_peFile = SafePEFileHandle.InvalidHandle;

		// Token: 0x04001871 RID: 6257
		private byte[] m_rawData;

		// Token: 0x04001872 RID: 6258
		private byte[] m_sha1;

		// Token: 0x04001873 RID: 6259
		private byte[] m_md5;
	}
}
