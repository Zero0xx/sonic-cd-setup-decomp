using System;
using System.Collections;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x0200034B RID: 843
	public class ServiceNameCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06001A77 RID: 6775 RVA: 0x0005C714 File Offset: 0x0005B714
		public ServiceNameCollection(ICollection items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			base.InnerList.AddRange(items);
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0005C738 File Offset: 0x0005B738
		public ServiceNameCollection Merge(string serviceName)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.InnerList);
			this.AddIfNew(arrayList, serviceName);
			return new ServiceNameCollection(arrayList);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x0005C768 File Offset: 0x0005B768
		public ServiceNameCollection Merge(IEnumerable serviceNames)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.InnerList);
			foreach (object obj in serviceNames)
			{
				this.AddIfNew(arrayList, obj as string);
			}
			return new ServiceNameCollection(arrayList);
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0005C7DC File Offset: 0x0005B7DC
		private void AddIfNew(ArrayList newServiceNames, string serviceName)
		{
			if (string.IsNullOrEmpty(serviceName))
			{
				throw new ArgumentException(SR.GetString("security_ServiceNameCollection_EmptyServiceName"));
			}
			if (!this.Contains(serviceName, newServiceNames))
			{
				newServiceNames.Add(serviceName);
			}
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0005C808 File Offset: 0x0005B808
		private bool Contains(string searchServiceName, ICollection serviceNames)
		{
			bool result = false;
			foreach (object obj in serviceNames)
			{
				string strA = (string)obj;
				if (string.Compare(strA, searchServiceName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
