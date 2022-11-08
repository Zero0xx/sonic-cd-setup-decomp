using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000E5 RID: 229
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ObsoleteAttribute : Attribute
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x00025368 File Offset: 0x00024368
		public ObsoleteAttribute()
		{
			this._message = null;
			this._error = false;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0002537E File Offset: 0x0002437E
		public ObsoleteAttribute(string message)
		{
			this._message = message;
			this._error = false;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00025394 File Offset: 0x00024394
		public ObsoleteAttribute(string message, bool error)
		{
			this._message = message;
			this._error = error;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x000253AA File Offset: 0x000243AA
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x000253B2 File Offset: 0x000243B2
		public bool IsError
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x0400046D RID: 1133
		private string _message;

		// Token: 0x0400046E RID: 1134
		private bool _error;
	}
}
