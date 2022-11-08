using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200004D RID: 77
	[ComVisible(true)]
	public class AssemblyLoadEventArgs : EventArgs
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00010A1E File Offset: 0x0000FA1E
		public Assembly LoadedAssembly
		{
			get
			{
				return this._LoadedAssembly;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00010A26 File Offset: 0x0000FA26
		public AssemblyLoadEventArgs(Assembly loadedAssembly)
		{
			this._LoadedAssembly = loadedAssembly;
		}

		// Token: 0x0400018F RID: 399
		private Assembly _LoadedAssembly;
	}
}
