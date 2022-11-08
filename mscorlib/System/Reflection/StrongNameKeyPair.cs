using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200033F RID: 831
	[ComVisible(true)]
	[Serializable]
	public class StrongNameKeyPair : IDeserializationCallback, ISerializable
	{
		// Token: 0x06001FC3 RID: 8131 RVA: 0x0004FCB4 File Offset: 0x0004ECB4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public StrongNameKeyPair(FileStream keyPairFile)
		{
			if (keyPairFile == null)
			{
				throw new ArgumentNullException("keyPairFile");
			}
			int num = (int)keyPairFile.Length;
			this._keyPairArray = new byte[num];
			keyPairFile.Read(this._keyPairArray, 0, num);
			this._keyPairExported = true;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0004FCFF File Offset: 0x0004ECFF
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public StrongNameKeyPair(byte[] keyPairArray)
		{
			if (keyPairArray == null)
			{
				throw new ArgumentNullException("keyPairArray");
			}
			this._keyPairArray = new byte[keyPairArray.Length];
			Array.Copy(keyPairArray, this._keyPairArray, keyPairArray.Length);
			this._keyPairExported = true;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0004FD39 File Offset: 0x0004ED39
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public StrongNameKeyPair(string keyPairContainer)
		{
			if (keyPairContainer == null)
			{
				throw new ArgumentNullException("keyPairContainer");
			}
			this._keyPairContainer = keyPairContainer;
			this._keyPairExported = false;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0004FD60 File Offset: 0x0004ED60
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected StrongNameKeyPair(SerializationInfo info, StreamingContext context)
		{
			this._keyPairExported = (bool)info.GetValue("_keyPairExported", typeof(bool));
			this._keyPairArray = (byte[])info.GetValue("_keyPairArray", typeof(byte[]));
			this._keyPairContainer = (string)info.GetValue("_keyPairContainer", typeof(string));
			this._publicKey = (byte[])info.GetValue("_publicKey", typeof(byte[]));
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x0004FDF4 File Offset: 0x0004EDF4
		public byte[] PublicKey
		{
			get
			{
				if (this._publicKey == null)
				{
					this._publicKey = this.nGetPublicKey(this._keyPairExported, this._keyPairArray, this._keyPairContainer);
				}
				byte[] array = new byte[this._publicKey.Length];
				Array.Copy(this._publicKey, array, this._publicKey.Length);
				return array;
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0004FE4C File Offset: 0x0004EE4C
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_keyPairExported", this._keyPairExported);
			info.AddValue("_keyPairArray", this._keyPairArray);
			info.AddValue("_keyPairContainer", this._keyPairContainer);
			info.AddValue("_publicKey", this._publicKey);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0004FE9D File Offset: 0x0004EE9D
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0004FE9F File Offset: 0x0004EE9F
		private bool GetKeyPair(out object arrayOrContainer)
		{
			arrayOrContainer = (this._keyPairExported ? this._keyPairArray : this._keyPairContainer);
			return this._keyPairExported;
		}

		// Token: 0x06001FCB RID: 8139
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern byte[] nGetPublicKey(bool exported, byte[] array, string container);

		// Token: 0x04000DBE RID: 3518
		private bool _keyPairExported;

		// Token: 0x04000DBF RID: 3519
		private byte[] _keyPairArray;

		// Token: 0x04000DC0 RID: 3520
		private string _keyPairContainer;

		// Token: 0x04000DC1 RID: 3521
		private byte[] _publicKey;
	}
}
