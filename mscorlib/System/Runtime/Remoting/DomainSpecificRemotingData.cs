using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x020006C0 RID: 1728
	internal class DomainSpecificRemotingData
	{
		// Token: 0x06003E1B RID: 15899 RVA: 0x000D44C4 File Offset: 0x000D34C4
		internal DomainSpecificRemotingData()
		{
			this._flags = 0;
			this._ConfigLock = new object();
			this._ChannelServicesData = new ChannelServicesData();
			this._IDTableLock = new ReaderWriterLock();
			this._appDomainProperties = new IContextProperty[1];
			this._appDomainProperties[0] = new LeaseLifeTimeServiceProperty();
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003E1C RID: 15900 RVA: 0x000D4518 File Offset: 0x000D3518
		// (set) Token: 0x06003E1D RID: 15901 RVA: 0x000D4520 File Offset: 0x000D3520
		internal LeaseManager LeaseManager
		{
			get
			{
				return this._LeaseManager;
			}
			set
			{
				this._LeaseManager = value;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06003E1E RID: 15902 RVA: 0x000D4529 File Offset: 0x000D3529
		internal object ConfigLock
		{
			get
			{
				return this._ConfigLock;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06003E1F RID: 15903 RVA: 0x000D4531 File Offset: 0x000D3531
		internal ReaderWriterLock IDTableLock
		{
			get
			{
				return this._IDTableLock;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06003E20 RID: 15904 RVA: 0x000D4539 File Offset: 0x000D3539
		// (set) Token: 0x06003E21 RID: 15905 RVA: 0x000D4541 File Offset: 0x000D3541
		internal LocalActivator LocalActivator
		{
			get
			{
				return this._LocalActivator;
			}
			set
			{
				this._LocalActivator = value;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x000D454A File Offset: 0x000D354A
		// (set) Token: 0x06003E23 RID: 15907 RVA: 0x000D4552 File Offset: 0x000D3552
		internal ActivationListener ActivationListener
		{
			get
			{
				return this._ActivationListener;
			}
			set
			{
				this._ActivationListener = value;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06003E24 RID: 15908 RVA: 0x000D455B File Offset: 0x000D355B
		// (set) Token: 0x06003E25 RID: 15909 RVA: 0x000D4568 File Offset: 0x000D3568
		internal bool InitializingActivation
		{
			get
			{
				return (this._flags & 1) == 1;
			}
			set
			{
				if (value)
				{
					this._flags |= 1;
					return;
				}
				this._flags &= -2;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003E26 RID: 15910 RVA: 0x000D458B File Offset: 0x000D358B
		// (set) Token: 0x06003E27 RID: 15911 RVA: 0x000D4598 File Offset: 0x000D3598
		internal bool ActivationInitialized
		{
			get
			{
				return (this._flags & 2) == 2;
			}
			set
			{
				if (value)
				{
					this._flags |= 2;
					return;
				}
				this._flags &= -3;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x000D45BB File Offset: 0x000D35BB
		// (set) Token: 0x06003E29 RID: 15913 RVA: 0x000D45C8 File Offset: 0x000D35C8
		internal bool ActivatorListening
		{
			get
			{
				return (this._flags & 4) == 4;
			}
			set
			{
				if (value)
				{
					this._flags |= 4;
					return;
				}
				this._flags &= -5;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x000D45EB File Offset: 0x000D35EB
		internal IContextProperty[] AppDomainContextProperties
		{
			get
			{
				return this._appDomainProperties;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x000D45F3 File Offset: 0x000D35F3
		internal ChannelServicesData ChannelServicesData
		{
			get
			{
				return this._ChannelServicesData;
			}
		}

		// Token: 0x04001FAC RID: 8108
		private const int ACTIVATION_INITIALIZING = 1;

		// Token: 0x04001FAD RID: 8109
		private const int ACTIVATION_INITIALIZED = 2;

		// Token: 0x04001FAE RID: 8110
		private const int ACTIVATOR_LISTENING = 4;

		// Token: 0x04001FAF RID: 8111
		private LocalActivator _LocalActivator;

		// Token: 0x04001FB0 RID: 8112
		private ActivationListener _ActivationListener;

		// Token: 0x04001FB1 RID: 8113
		private IContextProperty[] _appDomainProperties;

		// Token: 0x04001FB2 RID: 8114
		private int _flags;

		// Token: 0x04001FB3 RID: 8115
		private object _ConfigLock;

		// Token: 0x04001FB4 RID: 8116
		private ChannelServicesData _ChannelServicesData;

		// Token: 0x04001FB5 RID: 8117
		private LeaseManager _LeaseManager;

		// Token: 0x04001FB6 RID: 8118
		private ReaderWriterLock _IDTableLock;
	}
}
