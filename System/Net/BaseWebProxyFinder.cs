using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x0200037E RID: 894
	internal abstract class BaseWebProxyFinder : IWebProxyFinder, IDisposable
	{
		// Token: 0x06001BEE RID: 7150 RVA: 0x0006946E File Offset: 0x0006846E
		public BaseWebProxyFinder(AutoWebProxyScriptEngine engine)
		{
			this.engine = engine;
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0006947D File Offset: 0x0006847D
		public bool IsValid
		{
			get
			{
				return this.state == BaseWebProxyFinder.AutoWebProxyState.Completed || this.state == BaseWebProxyFinder.AutoWebProxyState.Uninitialized;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x00069493 File Offset: 0x00068493
		public bool IsUnrecognizedScheme
		{
			get
			{
				return this.state == BaseWebProxyFinder.AutoWebProxyState.UnrecognizedScheme;
			}
		}

		// Token: 0x06001BF1 RID: 7153
		public abstract bool GetProxies(Uri destination, out IList<string> proxyList);

		// Token: 0x06001BF2 RID: 7154
		public abstract void Abort();

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0006949E File Offset: 0x0006849E
		public virtual void Reset()
		{
			this.State = BaseWebProxyFinder.AutoWebProxyState.Uninitialized;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000694A7 File Offset: 0x000684A7
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x000694B0 File Offset: 0x000684B0
		// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x000694B8 File Offset: 0x000684B8
		protected BaseWebProxyFinder.AutoWebProxyState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x000694C1 File Offset: 0x000684C1
		protected AutoWebProxyScriptEngine Engine
		{
			get
			{
				return this.engine;
			}
		}

		// Token: 0x06001BF8 RID: 7160
		protected abstract void Dispose(bool disposing);

		// Token: 0x04001C8F RID: 7311
		private BaseWebProxyFinder.AutoWebProxyState state;

		// Token: 0x04001C90 RID: 7312
		private AutoWebProxyScriptEngine engine;

		// Token: 0x0200037F RID: 895
		protected enum AutoWebProxyState
		{
			// Token: 0x04001C92 RID: 7314
			Uninitialized,
			// Token: 0x04001C93 RID: 7315
			DiscoveryFailure,
			// Token: 0x04001C94 RID: 7316
			DownloadFailure,
			// Token: 0x04001C95 RID: 7317
			CompilationFailure,
			// Token: 0x04001C96 RID: 7318
			UnrecognizedScheme,
			// Token: 0x04001C97 RID: 7319
			Completed
		}
	}
}
