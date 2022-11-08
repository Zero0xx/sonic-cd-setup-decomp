using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000070 RID: 112
	[StructLayout(LayoutKind.Auto)]
	public struct ArgIterator
	{
		// Token: 0x06000660 RID: 1632
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ArgIterator(IntPtr arglist);

		// Token: 0x06000661 RID: 1633 RVA: 0x00015C26 File Offset: 0x00014C26
		public ArgIterator(RuntimeArgumentHandle arglist)
		{
			this = new ArgIterator(arglist.Value);
		}

		// Token: 0x06000662 RID: 1634
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern ArgIterator(IntPtr arglist, void* ptr);

		// Token: 0x06000663 RID: 1635 RVA: 0x00015C35 File Offset: 0x00014C35
		[CLSCompliant(false)]
		public unsafe ArgIterator(RuntimeArgumentHandle arglist, void* ptr)
		{
			this = new ArgIterator(arglist.Value, ptr);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00015C48 File Offset: 0x00014C48
		[CLSCompliant(false)]
		public unsafe TypedReference GetNextArg()
		{
			TypedReference result = default(TypedReference);
			this.FCallGetNextArg((void*)(&result));
			return result;
		}

		// Token: 0x06000665 RID: 1637
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void FCallGetNextArg(void* result);

		// Token: 0x06000666 RID: 1638 RVA: 0x00015C68 File Offset: 0x00014C68
		[CLSCompliant(false)]
		public unsafe TypedReference GetNextArg(RuntimeTypeHandle rth)
		{
			if (this.sigPtr != IntPtr.Zero)
			{
				return this.GetNextArg();
			}
			if (this.ArgPtr == IntPtr.Zero)
			{
				throw new ArgumentNullException();
			}
			TypedReference result = default(TypedReference);
			this.InternalGetNextArg((void*)(&result), rth);
			return result;
		}

		// Token: 0x06000667 RID: 1639
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void InternalGetNextArg(void* result, RuntimeTypeHandle rth);

		// Token: 0x06000668 RID: 1640 RVA: 0x00015CB9 File Offset: 0x00014CB9
		public void End()
		{
		}

		// Token: 0x06000669 RID: 1641
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetRemainingCount();

		// Token: 0x0600066A RID: 1642
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetNextArgType();

		// Token: 0x0600066B RID: 1643 RVA: 0x00015CBB File Offset: 0x00014CBB
		public RuntimeTypeHandle GetNextArgType()
		{
			return new RuntimeTypeHandle(this._GetNextArgType());
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00015CC8 File Offset: 0x00014CC8
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.ArgCookie);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00015CD5 File Offset: 0x00014CD5
		public override bool Equals(object o)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NYI"));
		}

		// Token: 0x040001F9 RID: 505
		private IntPtr ArgCookie;

		// Token: 0x040001FA RID: 506
		private IntPtr sigPtr;

		// Token: 0x040001FB RID: 507
		private IntPtr sigPtrLen;

		// Token: 0x040001FC RID: 508
		private IntPtr ArgPtr;

		// Token: 0x040001FD RID: 509
		private int RemainingArgs;
	}
}
