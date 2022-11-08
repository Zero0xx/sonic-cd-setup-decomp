using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020003B5 RID: 949
	internal sealed class GlobalizationAssembly
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x00069414 File Offset: 0x00068414
		internal static GlobalizationAssembly DefaultInstance
		{
			get
			{
				if (GlobalizationAssembly.m_defaultInstance == null)
				{
					throw new TypeLoadException("Failure has occurred while loading a type.");
				}
				return GlobalizationAssembly.m_defaultInstance;
			}
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x00069430 File Offset: 0x00068430
		internal static GlobalizationAssembly GetGlobalizationAssembly(Assembly assembly)
		{
			GlobalizationAssembly result;
			if ((result = (GlobalizationAssembly)GlobalizationAssembly.m_assemblyHash[assembly]) == null)
			{
				RuntimeHelpers.TryCode code = new RuntimeHelpers.TryCode(GlobalizationAssembly.CreateGlobalizationAssembly);
				RuntimeHelpers.ExecuteCodeWithLock(typeof(CultureTableRecord), code, assembly);
				result = (GlobalizationAssembly)GlobalizationAssembly.m_assemblyHash[assembly];
			}
			return result;
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x00069484 File Offset: 0x00068484
		[PrePrepareMethod]
		private static void CreateGlobalizationAssembly(object assem)
		{
			Assembly assembly = (Assembly)assem;
			if ((GlobalizationAssembly)GlobalizationAssembly.m_assemblyHash[assembly] == null)
			{
				GlobalizationAssembly globalizationAssembly = new GlobalizationAssembly();
				globalizationAssembly.pNativeGlobalizationAssembly = GlobalizationAssembly.nativeCreateGlobalizationAssembly(assembly);
				Thread.MemoryBarrier();
				GlobalizationAssembly.m_assemblyHash[assembly] = globalizationAssembly;
			}
		}

		// Token: 0x060025C2 RID: 9666
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* _nativeCreateGlobalizationAssembly(Assembly assembly);

		// Token: 0x060025C3 RID: 9667 RVA: 0x000694CF File Offset: 0x000684CF
		private unsafe static void* nativeCreateGlobalizationAssembly(Assembly assembly)
		{
			return GlobalizationAssembly._nativeCreateGlobalizationAssembly(assembly.InternalAssembly);
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000694DC File Offset: 0x000684DC
		internal GlobalizationAssembly()
		{
			this.compareInfoCache = new Hashtable(4);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000694F0 File Offset: 0x000684F0
		internal unsafe static byte* GetGlobalizationResourceBytePtr(Assembly assembly, string tableName)
		{
			Stream manifestResourceStream = assembly.GetManifestResourceStream(tableName);
			UnmanagedMemoryStream unmanagedMemoryStream = manifestResourceStream as UnmanagedMemoryStream;
			if (unmanagedMemoryStream != null)
			{
				byte* positionPointer = unmanagedMemoryStream.PositionPointer;
				if (positionPointer != null)
				{
					return positionPointer;
				}
			}
			throw new ExecutionEngineException();
		}

		// Token: 0x04001128 RID: 4392
		private static Hashtable m_assemblyHash = Hashtable.Synchronized(new Hashtable(4));

		// Token: 0x04001129 RID: 4393
		internal static GlobalizationAssembly m_defaultInstance = GlobalizationAssembly.GetGlobalizationAssembly(Assembly.GetAssembly(typeof(GlobalizationAssembly)));

		// Token: 0x0400112A RID: 4394
		internal Hashtable compareInfoCache;

		// Token: 0x0400112B RID: 4395
		internal unsafe void* pNativeGlobalizationAssembly;
	}
}
