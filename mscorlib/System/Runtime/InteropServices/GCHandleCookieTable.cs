using System;
using System.Collections;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050F RID: 1295
	internal class GCHandleCookieTable
	{
		// Token: 0x060031D0 RID: 12752 RVA: 0x000A9DC8 File Offset: 0x000A8DC8
		internal GCHandleCookieTable()
		{
			this.m_HandleList = new IntPtr[10];
			this.m_CycleCounts = new byte[10];
			this.m_HandleToCookieMap = new Hashtable();
			this.m_FreeIndex = 1;
			for (int i = 0; i < 10; i++)
			{
				this.m_HandleList[i] = IntPtr.Zero;
				this.m_CycleCounts[i] = 0;
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x000A9E34 File Offset: 0x000A8E34
		internal IntPtr FindOrAddHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			object obj = this.m_HandleToCookieMap[handle];
			if (obj != null)
			{
				return (IntPtr)obj;
			}
			IntPtr intPtr = IntPtr.Zero;
			int i = this.m_FreeIndex;
			if (i < this.m_HandleList.Length && this.m_HandleList[i] == IntPtr.Zero && Interlocked.CompareExchange(ref this.m_HandleList[i], handle, IntPtr.Zero) == IntPtr.Zero)
			{
				intPtr = this.GetCookieFromData((uint)i, this.m_CycleCounts[i]);
				if (i + 1 < this.m_HandleList.Length)
				{
					this.m_FreeIndex = i + 1;
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				i = 1;
				while (i < 16777215)
				{
					if (this.m_HandleList[i] == IntPtr.Zero && Interlocked.CompareExchange(ref this.m_HandleList[i], handle, IntPtr.Zero) == IntPtr.Zero)
					{
						intPtr = this.GetCookieFromData((uint)i, this.m_CycleCounts[i]);
						if (i + 1 < this.m_HandleList.Length)
						{
							this.m_FreeIndex = i + 1;
							break;
						}
						break;
					}
					else
					{
						if (i + 1 >= this.m_HandleList.Length)
						{
							lock (this)
							{
								if (i + 1 >= this.m_HandleList.Length)
								{
									this.GrowArrays();
								}
							}
						}
						i++;
					}
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("OutOfMemory_GCHandleMDA"));
			}
			lock (this)
			{
				obj = this.m_HandleToCookieMap[handle];
				if (obj != null)
				{
					this.m_HandleList[i] = IntPtr.Zero;
					intPtr = (IntPtr)obj;
				}
				else
				{
					this.m_HandleToCookieMap[handle] = intPtr;
				}
			}
			return intPtr;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000AA048 File Offset: 0x000A9048
		internal IntPtr GetHandle(IntPtr cookie)
		{
			IntPtr zero = IntPtr.Zero;
			if (!this.ValidateCookie(cookie))
			{
				return IntPtr.Zero;
			}
			return this.m_HandleList[this.GetIndexFromCookie(cookie)];
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000AA084 File Offset: 0x000A9084
		internal void RemoveHandleIfPresent(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			object obj = this.m_HandleToCookieMap[handle];
			if (obj != null)
			{
				IntPtr cookie = (IntPtr)obj;
				if (!this.ValidateCookie(cookie))
				{
					return;
				}
				int indexFromCookie = this.GetIndexFromCookie(cookie);
				byte[] cycleCounts = this.m_CycleCounts;
				int num = indexFromCookie;
				cycleCounts[num] += 1;
				this.m_HandleList[indexFromCookie] = IntPtr.Zero;
				this.m_HandleToCookieMap.Remove(handle);
				this.m_FreeIndex = indexFromCookie;
			}
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000AA118 File Offset: 0x000A9118
		private bool ValidateCookie(IntPtr cookie)
		{
			int num;
			byte b;
			this.GetDataFromCookie(cookie, out num, out b);
			if (num >= 16777215)
			{
				return false;
			}
			if (num >= this.m_HandleList.Length)
			{
				return false;
			}
			if (this.m_HandleList[num] == IntPtr.Zero)
			{
				return false;
			}
			byte b2 = (byte)(AppDomain.CurrentDomain.Id % 255);
			byte b3 = this.m_CycleCounts[num] ^ b2;
			return b == b3;
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000AA18C File Offset: 0x000A918C
		private void GrowArrays()
		{
			int num = this.m_HandleList.Length;
			IntPtr[] array = new IntPtr[num * 2];
			byte[] array2 = new byte[num * 2];
			Array.Copy(this.m_HandleList, array, num);
			Array.Copy(this.m_CycleCounts, array2, num);
			this.m_HandleList = array;
			this.m_CycleCounts = array2;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000AA1DC File Offset: 0x000A91DC
		private IntPtr GetCookieFromData(uint index, byte cycleCount)
		{
			byte b = (byte)(AppDomain.CurrentDomain.Id % 255);
			return (IntPtr)((long)((long)(cycleCount ^ b) << 24) + (long)((ulong)index) + 1L);
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000AA210 File Offset: 0x000A9210
		private void GetDataFromCookie(IntPtr cookie, out int index, out byte xorData)
		{
			uint num = (uint)((int)cookie);
			index = (int)((num & 16777215U) - 1U);
			xorData = (byte)((num & 4278190080U) >> 24);
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x000AA23C File Offset: 0x000A923C
		private int GetIndexFromCookie(IntPtr cookie)
		{
			uint num = (uint)((int)cookie);
			return (int)((num & 16777215U) - 1U);
		}

		// Token: 0x040019C5 RID: 6597
		private const int MaxListSize = 16777215;

		// Token: 0x040019C6 RID: 6598
		private const uint CookieMaskIndex = 16777215U;

		// Token: 0x040019C7 RID: 6599
		private const uint CookieMaskSentinal = 4278190080U;

		// Token: 0x040019C8 RID: 6600
		private Hashtable m_HandleToCookieMap;

		// Token: 0x040019C9 RID: 6601
		private IntPtr[] m_HandleList;

		// Token: 0x040019CA RID: 6602
		private byte[] m_CycleCounts;

		// Token: 0x040019CB RID: 6603
		private int m_FreeIndex;
	}
}
