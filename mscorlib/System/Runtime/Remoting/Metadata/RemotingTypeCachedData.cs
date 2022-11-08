using System;
using System.Reflection;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000741 RID: 1857
	internal class RemotingTypeCachedData : RemotingCachedData
	{
		// Token: 0x06004270 RID: 17008 RVA: 0x000E22CC File Offset: 0x000E12CC
		internal RemotingTypeCachedData(object ri) : base(ri)
		{
			this._lastMethodCalled = null;
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x000E22DC File Offset: 0x000E12DC
		internal MethodBase GetLastCalledMethod(string newMeth)
		{
			RemotingTypeCachedData.LastCalledMethodClass lastMethodCalled = this._lastMethodCalled;
			if (lastMethodCalled == null)
			{
				return null;
			}
			string methodName = lastMethodCalled.methodName;
			MethodBase mb = lastMethodCalled.MB;
			if (mb == null || methodName == null)
			{
				return null;
			}
			if (methodName.Equals(newMeth))
			{
				return mb;
			}
			return null;
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x000E2318 File Offset: 0x000E1318
		internal void SetLastCalledMethod(string newMethName, MethodBase newMB)
		{
			this._lastMethodCalled = new RemotingTypeCachedData.LastCalledMethodClass
			{
				methodName = newMethName,
				MB = newMB
			};
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06004273 RID: 17011 RVA: 0x000E2340 File Offset: 0x000E1340
		internal TypeInfo TypeInfo
		{
			get
			{
				if (this._typeInfo == null)
				{
					this._typeInfo = new TypeInfo((Type)this.RI);
				}
				return this._typeInfo;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06004274 RID: 17012 RVA: 0x000E2366 File Offset: 0x000E1366
		internal string QualifiedTypeName
		{
			get
			{
				if (this._qualifiedTypeName == null)
				{
					this._qualifiedTypeName = RemotingServices.DetermineDefaultQualifiedTypeName((Type)this.RI);
				}
				return this._qualifiedTypeName;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004275 RID: 17013 RVA: 0x000E238C File Offset: 0x000E138C
		internal string AssemblyName
		{
			get
			{
				if (this._assemblyName == null)
				{
					this._assemblyName = ((Type)this.RI).Module.Assembly.FullName;
				}
				return this._assemblyName;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004276 RID: 17014 RVA: 0x000E23BC File Offset: 0x000E13BC
		internal string SimpleAssemblyName
		{
			get
			{
				if (this._simpleAssemblyName == null)
				{
					this._simpleAssemblyName = ((Type)this.RI).Module.Assembly.nGetSimpleName();
				}
				return this._simpleAssemblyName;
			}
		}

		// Token: 0x04002147 RID: 8519
		private RemotingTypeCachedData.LastCalledMethodClass _lastMethodCalled;

		// Token: 0x04002148 RID: 8520
		private TypeInfo _typeInfo;

		// Token: 0x04002149 RID: 8521
		private string _qualifiedTypeName;

		// Token: 0x0400214A RID: 8522
		private string _assemblyName;

		// Token: 0x0400214B RID: 8523
		private string _simpleAssemblyName;

		// Token: 0x02000742 RID: 1858
		private class LastCalledMethodClass
		{
			// Token: 0x0400214C RID: 8524
			public string methodName;

			// Token: 0x0400214D RID: 8525
			public MethodBase MB;
		}
	}
}
