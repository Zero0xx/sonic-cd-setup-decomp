using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200033B RID: 827
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class Pointer : ISerializable
	{
		// Token: 0x06001FB4 RID: 8116 RVA: 0x0004FA56 File Offset: 0x0004EA56
		private Pointer()
		{
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0004FA60 File Offset: 0x0004EA60
		private Pointer(SerializationInfo info, StreamingContext context)
		{
			this._ptr = ((IntPtr)info.GetValue("_ptr", typeof(IntPtr))).ToPointer();
			this._ptrType = (Type)info.GetValue("_ptrType", typeof(Type));
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0004FABC File Offset: 0x0004EABC
		public unsafe static object Box(void* ptr, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsPointer)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBePointer"), "ptr");
			}
			return new Pointer
			{
				_ptr = ptr,
				_ptrType = type
			};
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0004FB09 File Offset: 0x0004EB09
		public unsafe static void* Unbox(object ptr)
		{
			if (!(ptr is Pointer))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBePointer"), "ptr");
			}
			return ((Pointer)ptr)._ptr;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0004FB33 File Offset: 0x0004EB33
		internal Type GetPointerType()
		{
			return this._ptrType;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0004FB3B File Offset: 0x0004EB3B
		internal object GetPointerValue()
		{
			return (IntPtr)this._ptr;
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0004FB4D File Offset: 0x0004EB4D
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_ptr", new IntPtr(this._ptr));
			info.AddValue("_ptrType", this._ptrType);
		}

		// Token: 0x04000DAE RID: 3502
		private unsafe void* _ptr;

		// Token: 0x04000DAF RID: 3503
		private Type _ptrType;
	}
}
