using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Assemblies
{
	// Token: 0x0200085D RID: 2141
	[ComVisible(true)]
	[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
	[Serializable]
	public struct AssemblyHash : ICloneable
	{
		// Token: 0x06004E5A RID: 20058 RVA: 0x0010FD1C File Offset: 0x0010ED1C
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyHash(byte[] value)
		{
			this._Algorithm = AssemblyHashAlgorithm.SHA1;
			this._Value = null;
			if (value != null)
			{
				int num = value.Length;
				this._Value = new byte[num];
				Array.Copy(value, this._Value, num);
			}
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x0010FD5C File Offset: 0x0010ED5C
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyHash(AssemblyHashAlgorithm algorithm, byte[] value)
		{
			this._Algorithm = algorithm;
			this._Value = null;
			if (value != null)
			{
				int num = value.Length;
				this._Value = new byte[num];
				Array.Copy(value, this._Value, num);
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x0010FD97 File Offset: 0x0010ED97
		// (set) Token: 0x06004E5D RID: 20061 RVA: 0x0010FD9F File Offset: 0x0010ED9F
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyHashAlgorithm Algorithm
		{
			get
			{
				return this._Algorithm;
			}
			set
			{
				this._Algorithm = value;
			}
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0010FDA8 File Offset: 0x0010EDA8
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public byte[] GetValue()
		{
			return this._Value;
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x0010FDB0 File Offset: 0x0010EDB0
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void SetValue(byte[] value)
		{
			this._Value = value;
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x0010FDB9 File Offset: 0x0010EDB9
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public object Clone()
		{
			return new AssemblyHash(this._Algorithm, this._Value);
		}

		// Token: 0x04002878 RID: 10360
		private AssemblyHashAlgorithm _Algorithm;

		// Token: 0x04002879 RID: 10361
		private byte[] _Value;

		// Token: 0x0400287A RID: 10362
		[Obsolete("The AssemblyHash class has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static readonly AssemblyHash Empty = new AssemblyHash(AssemblyHashAlgorithm.None, null);
	}
}
