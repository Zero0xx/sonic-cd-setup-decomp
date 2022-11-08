using System;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x020005CD RID: 1485
	internal class OverlappedCache
	{
		// Token: 0x06002EAB RID: 11947 RVA: 0x000CDFF2 File Offset: 0x000CCFF2
		internal OverlappedCache(Overlapped overlapped, object[] pinnedObjectsArray, IOCompletionCallback callback)
		{
			this.m_Overlapped = overlapped;
			this.m_PinnedObjects = pinnedObjectsArray;
			this.m_PinnedObjectsArray = pinnedObjectsArray;
			this.m_NativeOverlapped = new SafeNativeOverlapped(overlapped.UnsafePack(callback, pinnedObjectsArray));
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000CE022 File Offset: 0x000CD022
		internal OverlappedCache(Overlapped overlapped, object pinnedObjects, IOCompletionCallback callback, bool alreadyTriedCast)
		{
			this.m_Overlapped = overlapped;
			this.m_PinnedObjects = pinnedObjects;
			this.m_PinnedObjectsArray = (alreadyTriedCast ? null : NclConstants.EmptyObjectArray);
			this.m_NativeOverlapped = new SafeNativeOverlapped(overlapped.UnsafePack(callback, pinnedObjects));
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x000CE05D File Offset: 0x000CD05D
		internal Overlapped Overlapped
		{
			get
			{
				return this.m_Overlapped;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x000CE065 File Offset: 0x000CD065
		internal SafeNativeOverlapped NativeOverlapped
		{
			get
			{
				return this.m_NativeOverlapped;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x000CE06D File Offset: 0x000CD06D
		internal object PinnedObjects
		{
			get
			{
				return this.m_PinnedObjects;
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06002EB0 RID: 11952 RVA: 0x000CE078 File Offset: 0x000CD078
		internal object[] PinnedObjectsArray
		{
			get
			{
				object[] array = this.m_PinnedObjectsArray;
				if (array != null && array.Length == 0)
				{
					array = (this.m_PinnedObjects as object[]);
					if (array != null && array.Length == 0)
					{
						this.m_PinnedObjectsArray = null;
					}
					else
					{
						this.m_PinnedObjectsArray = array;
					}
				}
				return this.m_PinnedObjectsArray;
			}
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000CE0BE File Offset: 0x000CD0BE
		internal void Free()
		{
			this.InternalFree();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000CE0CC File Offset: 0x000CD0CC
		private void InternalFree()
		{
			this.m_Overlapped = null;
			this.m_PinnedObjects = null;
			if (this.m_NativeOverlapped != null)
			{
				if (!this.m_NativeOverlapped.IsInvalid)
				{
					this.m_NativeOverlapped.Dispose();
				}
				this.m_NativeOverlapped = null;
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000CE104 File Offset: 0x000CD104
		internal static void InterlockedFree(ref OverlappedCache overlappedCache)
		{
			OverlappedCache overlappedCache2 = (overlappedCache == null) ? null : Interlocked.Exchange<OverlappedCache>(ref overlappedCache, null);
			if (overlappedCache2 != null)
			{
				overlappedCache2.Free();
			}
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000CE12C File Offset: 0x000CD12C
		~OverlappedCache()
		{
			if (!NclUtilities.HasShutdownStarted)
			{
				this.InternalFree();
			}
		}

		// Token: 0x04002C3E RID: 11326
		internal Overlapped m_Overlapped;

		// Token: 0x04002C3F RID: 11327
		internal SafeNativeOverlapped m_NativeOverlapped;

		// Token: 0x04002C40 RID: 11328
		internal object m_PinnedObjects;

		// Token: 0x04002C41 RID: 11329
		internal object[] m_PinnedObjectsArray;
	}
}
