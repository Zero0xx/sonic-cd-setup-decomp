using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x02000105 RID: 261
	internal struct Utf8String
	{
		// Token: 0x06000EB7 RID: 3767
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool EqualsCaseSensitive(void* szLhs, void* szRhs, int cSz);

		// Token: 0x06000EB8 RID: 3768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool EqualsCaseInsensitive(void* szLhs, void* szRhs, int cSz);

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0002C16C File Offset: 0x0002B16C
		private unsafe static int GetUtf8StringByteLength(void* pUtf8String)
		{
			int num = 0;
			byte* ptr = (byte*)pUtf8String;
			while (*ptr != 0)
			{
				num++;
				ptr++;
			}
			return num;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0002C18D File Offset: 0x0002B18D
		internal unsafe Utf8String(void* pStringHeap)
		{
			this.m_pStringHeap = pStringHeap;
			if (pStringHeap != null)
			{
				this.m_StringHeapByteLength = Utf8String.GetUtf8StringByteLength(pStringHeap);
				return;
			}
			this.m_StringHeapByteLength = 0;
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0002C1AF File Offset: 0x0002B1AF
		internal unsafe Utf8String(void* pUtf8String, int cUtf8String)
		{
			this.m_pStringHeap = pUtf8String;
			this.m_StringHeapByteLength = cUtf8String;
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0002C1C0 File Offset: 0x0002B1C0
		internal bool Equals(Utf8String s)
		{
			if (this.m_pStringHeap == null)
			{
				return s.m_StringHeapByteLength == 0;
			}
			return s.m_StringHeapByteLength == this.m_StringHeapByteLength && this.m_StringHeapByteLength != 0 && Utf8String.EqualsCaseSensitive(s.m_pStringHeap, this.m_pStringHeap, this.m_StringHeapByteLength);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0002C214 File Offset: 0x0002B214
		internal bool EqualsCaseInsensitive(Utf8String s)
		{
			if (this.m_pStringHeap == null)
			{
				return s.m_StringHeapByteLength == 0;
			}
			return s.m_StringHeapByteLength == this.m_StringHeapByteLength && this.m_StringHeapByteLength != 0 && Utf8String.EqualsCaseInsensitive(s.m_pStringHeap, this.m_pStringHeap, this.m_StringHeapByteLength);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0002C268 File Offset: 0x0002B268
		public unsafe override string ToString()
		{
			byte* ptr = stackalloc byte[1 * this.m_StringHeapByteLength];
			byte* ptr2 = (byte*)this.m_pStringHeap;
			for (int i = 0; i < this.m_StringHeapByteLength; i++)
			{
				ptr[i] = *ptr2;
				ptr2++;
			}
			if (this.m_StringHeapByteLength == 0)
			{
				return "";
			}
			int charCount = Encoding.UTF8.GetCharCount(ptr, this.m_StringHeapByteLength);
			char* ptr3 = stackalloc char[2 * charCount];
			Encoding.UTF8.GetChars(ptr, this.m_StringHeapByteLength, ptr3, charCount);
			return new string(ptr3, 0, charCount);
		}

		// Token: 0x04000532 RID: 1330
		private unsafe void* m_pStringHeap;

		// Token: 0x04000533 RID: 1331
		private int m_StringHeapByteLength;
	}
}
