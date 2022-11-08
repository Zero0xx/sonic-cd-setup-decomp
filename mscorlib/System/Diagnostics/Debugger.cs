using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020002B4 RID: 692
	[ComVisible(true)]
	public sealed class Debugger
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x00046DC8 File Offset: 0x00045DC8
		public static void Break()
		{
			if (!Debugger.IsDebuggerAttached())
			{
				try
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				}
				catch (SecurityException)
				{
					return;
				}
			}
			Debugger.BreakInternal();
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00046E04 File Offset: 0x00045E04
		private static void BreakCanThrow()
		{
			if (!Debugger.IsDebuggerAttached())
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			Debugger.BreakInternal();
		}

		// Token: 0x06001B12 RID: 6930
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BreakInternal();

		// Token: 0x06001B13 RID: 6931 RVA: 0x00046E20 File Offset: 0x00045E20
		public static bool Launch()
		{
			if (Debugger.IsDebuggerAttached())
			{
				return true;
			}
			try
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			catch (SecurityException)
			{
				return false;
			}
			return Debugger.LaunchInternal();
		}

		// Token: 0x06001B14 RID: 6932
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LaunchInternal();

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x00046E60 File Offset: 0x00045E60
		public static bool IsAttached
		{
			get
			{
				return Debugger.IsDebuggerAttached();
			}
		}

		// Token: 0x06001B16 RID: 6934
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDebuggerAttached();

		// Token: 0x06001B17 RID: 6935
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Log(int level, string category, string message);

		// Token: 0x06001B18 RID: 6936
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsLogging();

		// Token: 0x04000A52 RID: 2642
		public static readonly string DefaultCategory;
	}
}
