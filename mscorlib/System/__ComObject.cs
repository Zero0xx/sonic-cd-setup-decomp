using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000088 RID: 136
	internal class __ComObject : MarshalByRefObject
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x000182B8 File Offset: 0x000172B8
		private __ComObject()
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000182C0 File Offset: 0x000172C0
		internal IntPtr GetIUnknown(out bool fIsURTAggregated)
		{
			fIsURTAggregated = !base.GetType().IsDefined(typeof(ComImportAttribute), false);
			return Marshal.GetIUnknownForObject(this);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000182E4 File Offset: 0x000172E4
		internal object GetData(object key)
		{
			object result = null;
			lock (this)
			{
				if (this.m_ObjectToDataMap != null)
				{
					result = this.m_ObjectToDataMap[key];
				}
			}
			return result;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001832C File Offset: 0x0001732C
		internal bool SetData(object key, object data)
		{
			bool result = false;
			lock (this)
			{
				if (this.m_ObjectToDataMap == null)
				{
					this.m_ObjectToDataMap = new Hashtable();
				}
				if (this.m_ObjectToDataMap[key] == null)
				{
					this.m_ObjectToDataMap[key] = data;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00018390 File Offset: 0x00017390
		internal void ReleaseAllData()
		{
			lock (this)
			{
				if (this.m_ObjectToDataMap != null)
				{
					foreach (object obj in this.m_ObjectToDataMap.Values)
					{
						IDisposable disposable = obj as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
						__ComObject _ComObject = obj as __ComObject;
						if (_ComObject != null)
						{
							Marshal.ReleaseComObject(_ComObject);
						}
					}
					this.m_ObjectToDataMap = null;
				}
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00018438 File Offset: 0x00017438
		internal object GetEventProvider(Type t)
		{
			object obj = this.GetData(t);
			if (obj == null)
			{
				obj = this.CreateEventProvider(t);
			}
			return obj;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00018459 File Offset: 0x00017459
		internal int ReleaseSelf()
		{
			return Marshal.InternalReleaseComObject(this);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00018461 File Offset: 0x00017461
		internal void FinalReleaseSelf()
		{
			Marshal.InternalFinalReleaseComObject(this);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001846C File Offset: 0x0001746C
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private object CreateEventProvider(Type t)
		{
			object obj = Activator.CreateInstance(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[]
			{
				this
			}, null);
			if (!this.SetData(t, obj))
			{
				IDisposable disposable = obj as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				obj = this.GetData(t);
			}
			return obj;
		}

		// Token: 0x04000288 RID: 648
		private Hashtable m_ObjectToDataMap;
	}
}
