using System;

namespace System.Configuration
{
	// Token: 0x02000711 RID: 1809
	public class SettingsProperty
	{
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x000EADE9 File Offset: 0x000E9DE9
		// (set) Token: 0x0600374A RID: 14154 RVA: 0x000EADF1 File Offset: 0x000E9DF1
		public virtual string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x000EADFA File Offset: 0x000E9DFA
		// (set) Token: 0x0600374C RID: 14156 RVA: 0x000EAE02 File Offset: 0x000E9E02
		public virtual bool IsReadOnly
		{
			get
			{
				return this._IsReadOnly;
			}
			set
			{
				this._IsReadOnly = value;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x000EAE0B File Offset: 0x000E9E0B
		// (set) Token: 0x0600374E RID: 14158 RVA: 0x000EAE13 File Offset: 0x000E9E13
		public virtual object DefaultValue
		{
			get
			{
				return this._DefaultValue;
			}
			set
			{
				this._DefaultValue = value;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x0600374F RID: 14159 RVA: 0x000EAE1C File Offset: 0x000E9E1C
		// (set) Token: 0x06003750 RID: 14160 RVA: 0x000EAE24 File Offset: 0x000E9E24
		public virtual Type PropertyType
		{
			get
			{
				return this._PropertyType;
			}
			set
			{
				this._PropertyType = value;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003751 RID: 14161 RVA: 0x000EAE2D File Offset: 0x000E9E2D
		// (set) Token: 0x06003752 RID: 14162 RVA: 0x000EAE35 File Offset: 0x000E9E35
		public virtual SettingsSerializeAs SerializeAs
		{
			get
			{
				return this._SerializeAs;
			}
			set
			{
				this._SerializeAs = value;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x000EAE3E File Offset: 0x000E9E3E
		// (set) Token: 0x06003754 RID: 14164 RVA: 0x000EAE46 File Offset: 0x000E9E46
		public virtual SettingsProvider Provider
		{
			get
			{
				return this._Provider;
			}
			set
			{
				this._Provider = value;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x000EAE4F File Offset: 0x000E9E4F
		public virtual SettingsAttributeDictionary Attributes
		{
			get
			{
				return this._Attributes;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x000EAE57 File Offset: 0x000E9E57
		// (set) Token: 0x06003757 RID: 14167 RVA: 0x000EAE5F File Offset: 0x000E9E5F
		public bool ThrowOnErrorDeserializing
		{
			get
			{
				return this._ThrowOnErrorDeserializing;
			}
			set
			{
				this._ThrowOnErrorDeserializing = value;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003758 RID: 14168 RVA: 0x000EAE68 File Offset: 0x000E9E68
		// (set) Token: 0x06003759 RID: 14169 RVA: 0x000EAE70 File Offset: 0x000E9E70
		public bool ThrowOnErrorSerializing
		{
			get
			{
				return this._ThrowOnErrorSerializing;
			}
			set
			{
				this._ThrowOnErrorSerializing = value;
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000EAE79 File Offset: 0x000E9E79
		public SettingsProperty(string name)
		{
			this._Name = name;
			this._Attributes = new SettingsAttributeDictionary();
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000EAE94 File Offset: 0x000E9E94
		public SettingsProperty(string name, Type propertyType, SettingsProvider provider, bool isReadOnly, object defaultValue, SettingsSerializeAs serializeAs, SettingsAttributeDictionary attributes, bool throwOnErrorDeserializing, bool throwOnErrorSerializing)
		{
			this._Name = name;
			this._PropertyType = propertyType;
			this._Provider = provider;
			this._IsReadOnly = isReadOnly;
			this._DefaultValue = defaultValue;
			this._SerializeAs = serializeAs;
			this._Attributes = attributes;
			this._ThrowOnErrorDeserializing = throwOnErrorDeserializing;
			this._ThrowOnErrorSerializing = throwOnErrorSerializing;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000EAEEC File Offset: 0x000E9EEC
		public SettingsProperty(SettingsProperty propertyToCopy)
		{
			this._Name = propertyToCopy.Name;
			this._IsReadOnly = propertyToCopy.IsReadOnly;
			this._DefaultValue = propertyToCopy.DefaultValue;
			this._SerializeAs = propertyToCopy.SerializeAs;
			this._Provider = propertyToCopy.Provider;
			this._PropertyType = propertyToCopy.PropertyType;
			this._ThrowOnErrorDeserializing = propertyToCopy.ThrowOnErrorDeserializing;
			this._ThrowOnErrorSerializing = propertyToCopy.ThrowOnErrorSerializing;
			this._Attributes = new SettingsAttributeDictionary(propertyToCopy.Attributes);
		}

		// Token: 0x040031B9 RID: 12729
		private string _Name;

		// Token: 0x040031BA RID: 12730
		private bool _IsReadOnly;

		// Token: 0x040031BB RID: 12731
		private object _DefaultValue;

		// Token: 0x040031BC RID: 12732
		private SettingsSerializeAs _SerializeAs;

		// Token: 0x040031BD RID: 12733
		private SettingsProvider _Provider;

		// Token: 0x040031BE RID: 12734
		private SettingsAttributeDictionary _Attributes;

		// Token: 0x040031BF RID: 12735
		private Type _PropertyType;

		// Token: 0x040031C0 RID: 12736
		private bool _ThrowOnErrorDeserializing;

		// Token: 0x040031C1 RID: 12737
		private bool _ThrowOnErrorSerializing;
	}
}
