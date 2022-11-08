using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x0200013D RID: 317
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[Serializable]
	public class WeakReference : ISerializable
	{
		// Token: 0x06001188 RID: 4488 RVA: 0x00031353 File Offset: 0x00030353
		public WeakReference(object target) : this(target, false)
		{
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0003135D File Offset: 0x0003035D
		public WeakReference(object target, bool trackResurrection)
		{
			this.m_IsLongReference = trackResurrection;
			this.m_handle = GCHandle.InternalAlloc(target, trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00031384 File Offset: 0x00030384
		protected WeakReference(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			object value = info.GetValue("TrackedObject", typeof(object));
			this.m_IsLongReference = info.GetBoolean("TrackResurrection");
			this.m_handle = GCHandle.InternalAlloc(value, this.m_IsLongReference ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x000313E8 File Offset: 0x000303E8
		public virtual bool IsAlive
		{
			get
			{
				IntPtr handle = this.m_handle;
				if (IntPtr.Zero == handle)
				{
					return false;
				}
				bool flag = GCHandle.InternalGet(handle) != null;
				return !(this.m_handle == IntPtr.Zero) && flag;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x00031431 File Offset: 0x00030431
		public virtual bool TrackResurrection
		{
			get
			{
				return this.m_IsLongReference;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x0003143C File Offset: 0x0003043C
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00031480 File Offset: 0x00030480
		public virtual object Target
		{
			get
			{
				IntPtr handle = this.m_handle;
				if (IntPtr.Zero == handle)
				{
					return null;
				}
				object result = GCHandle.InternalGet(handle);
				if (!(this.m_handle == IntPtr.Zero))
				{
					return result;
				}
				return null;
			}
			set
			{
				IntPtr handle = this.m_handle;
				if (handle == IntPtr.Zero)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
				}
				object oldValue = GCHandle.InternalGet(handle);
				handle = this.m_handle;
				if (handle == IntPtr.Zero)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
				}
				GCHandle.InternalCompareExchange(handle, value, oldValue, false);
				GC.KeepAlive(this);
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000314F0 File Offset: 0x000304F0
		protected override void Finalize()
		{
			try
			{
				IntPtr handle = this.m_handle;
				if (handle != IntPtr.Zero && handle == Interlocked.CompareExchange(ref this.m_handle, IntPtr.Zero, handle))
				{
					GCHandle.InternalFree(handle);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0003154C File Offset: 0x0003054C
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("TrackedObject", this.Target, typeof(object));
			info.AddValue("TrackResurrection", this.m_IsLongReference);
		}

		// Token: 0x04000610 RID: 1552
		internal volatile IntPtr m_handle;

		// Token: 0x04000611 RID: 1553
		internal bool m_IsLongReference;
	}
}
