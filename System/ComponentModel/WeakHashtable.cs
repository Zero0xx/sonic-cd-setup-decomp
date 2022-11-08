using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020007A3 RID: 1955
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable
	{
		// Token: 0x06003C17 RID: 15383 RVA: 0x00100E73 File Offset: 0x000FFE73
		internal WeakHashtable() : base(WeakHashtable._comparer)
		{
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00100E80 File Offset: 0x000FFE80
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x00100E98 File Offset: 0x000FFE98
		private void ScavengeKeys()
		{
			int count = this.Count;
			if (count == 0)
			{
				return;
			}
			if (this._lastHashCount == 0)
			{
				this._lastHashCount = count;
				return;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (this._lastGlobalMem == 0L)
			{
				this._lastGlobalMem = totalMemory;
				return;
			}
			float num = (float)(totalMemory - this._lastGlobalMem) / (float)this._lastGlobalMem;
			float num2 = (float)(count - this._lastHashCount) / (float)this._lastHashCount;
			if (num < 0f && num2 >= 0f)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakReference weakReference = obj as WeakReference;
					if (weakReference != null && !weakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(weakReference);
					}
				}
				if (arrayList != null)
				{
					foreach (object key in arrayList)
					{
						this.Remove(key);
					}
				}
			}
			this._lastGlobalMem = totalMemory;
			this._lastHashCount = count;
		}

		// Token: 0x04003504 RID: 13572
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x04003505 RID: 13573
		private long _lastGlobalMem;

		// Token: 0x04003506 RID: 13574
		private int _lastHashCount;

		// Token: 0x020007A4 RID: 1956
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06003C1B RID: 15387 RVA: 0x00100FF0 File Offset: 0x000FFFF0
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return true;
				}
				if (x == null || y == null)
				{
					return false;
				}
				if (x.GetHashCode() == y.GetHashCode())
				{
					WeakReference weakReference = x as WeakReference;
					WeakReference weakReference2 = y as WeakReference;
					if (weakReference != null)
					{
						if (!weakReference.IsAlive)
						{
							return false;
						}
						x = weakReference.Target;
					}
					if (weakReference2 != null)
					{
						if (!weakReference2.IsAlive)
						{
							return false;
						}
						y = weakReference2.Target;
					}
					return object.ReferenceEquals(x, y);
				}
				return false;
			}

			// Token: 0x06003C1C RID: 15388 RVA: 0x0010105F File Offset: 0x0010005F
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x020007A5 RID: 1957
		private sealed class EqualityWeakReference : WeakReference
		{
			// Token: 0x06003C1E RID: 15390 RVA: 0x0010106F File Offset: 0x0010006F
			internal EqualityWeakReference(object o) : base(o)
			{
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x06003C1F RID: 15391 RVA: 0x00101084 File Offset: 0x00100084
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || (this.IsAlive && object.ReferenceEquals(o, this.Target)));
			}

			// Token: 0x06003C20 RID: 15392 RVA: 0x001010B8 File Offset: 0x001000B8
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04003507 RID: 13575
			private int _hashCode;
		}
	}
}
