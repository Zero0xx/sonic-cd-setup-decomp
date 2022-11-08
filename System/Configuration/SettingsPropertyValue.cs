using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace System.Configuration
{
	// Token: 0x02000715 RID: 1813
	public class SettingsPropertyValue
	{
		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x000EB165 File Offset: 0x000EA165
		public string Name
		{
			get
			{
				return this._Property.Name;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x000EB172 File Offset: 0x000EA172
		// (set) Token: 0x0600377A RID: 14202 RVA: 0x000EB17A File Offset: 0x000EA17A
		public bool IsDirty
		{
			get
			{
				return this._IsDirty;
			}
			set
			{
				this._IsDirty = value;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x0600377B RID: 14203 RVA: 0x000EB183 File Offset: 0x000EA183
		public SettingsProperty Property
		{
			get
			{
				return this._Property;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x0600377C RID: 14204 RVA: 0x000EB18B File Offset: 0x000EA18B
		public bool UsingDefaultValue
		{
			get
			{
				return this._UsingDefaultValue;
			}
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000EB193 File Offset: 0x000EA193
		public SettingsPropertyValue(SettingsProperty property)
		{
			this._Property = property;
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x000EB1AC File Offset: 0x000EA1AC
		// (set) Token: 0x0600377F RID: 14207 RVA: 0x000EB223 File Offset: 0x000EA223
		public object PropertyValue
		{
			get
			{
				if (!this._Deserialized)
				{
					this._Value = this.Deserialize();
					this._Deserialized = true;
				}
				if (this._Value != null && !this.Property.PropertyType.IsPrimitive && !(this._Value is string) && !(this._Value is DateTime))
				{
					this._UsingDefaultValue = false;
					this._ChangedSinceLastSerialized = true;
					this._IsDirty = true;
				}
				return this._Value;
			}
			set
			{
				this._Value = value;
				this._IsDirty = true;
				this._ChangedSinceLastSerialized = true;
				this._Deserialized = true;
				this._UsingDefaultValue = false;
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x000EB248 File Offset: 0x000EA248
		// (set) Token: 0x06003781 RID: 14209 RVA: 0x000EB26B File Offset: 0x000EA26B
		public object SerializedValue
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			get
			{
				if (this._ChangedSinceLastSerialized)
				{
					this._ChangedSinceLastSerialized = false;
					this._SerializedValue = this.SerializePropertyValue();
				}
				return this._SerializedValue;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			set
			{
				this._UsingDefaultValue = false;
				this._SerializedValue = value;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x000EB27B File Offset: 0x000EA27B
		// (set) Token: 0x06003783 RID: 14211 RVA: 0x000EB283 File Offset: 0x000EA283
		public bool Deserialized
		{
			get
			{
				return this._Deserialized;
			}
			set
			{
				this._Deserialized = value;
			}
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x000EB28C File Offset: 0x000EA28C
		private bool IsHostedInAspnet()
		{
			return AppDomain.CurrentDomain.GetData(".appDomain") != null;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x000EB2A4 File Offset: 0x000EA2A4
		private object Deserialize()
		{
			object obj = null;
			if (this.SerializedValue != null)
			{
				try
				{
					if (this.SerializedValue is string)
					{
						obj = SettingsPropertyValue.GetObjectFromString(this.Property.PropertyType, this.Property.SerializeAs, (string)this.SerializedValue);
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream((byte[])this.SerializedValue);
						try
						{
							obj = new BinaryFormatter().Deserialize(memoryStream);
						}
						finally
						{
							memoryStream.Close();
						}
					}
				}
				catch (Exception ex)
				{
					try
					{
						if (this.IsHostedInAspnet())
						{
							object[] args = new object[]
							{
								this.Property,
								this,
								ex
							};
							Type type = Type.GetType("System.Web.Management.WebBaseEvent, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
							type.InvokeMember("RaisePropertyDeserializationWebErrorEvent", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, args, CultureInfo.InvariantCulture);
						}
					}
					catch
					{
					}
				}
				if (obj != null && !this.Property.PropertyType.IsAssignableFrom(obj.GetType()))
				{
					obj = null;
				}
			}
			if (obj == null)
			{
				this._UsingDefaultValue = true;
				if (this.Property.DefaultValue == null || this.Property.DefaultValue.ToString() == "[null]")
				{
					if (this.Property.PropertyType.IsValueType)
					{
						return Activator.CreateInstance(this.Property.PropertyType);
					}
					return null;
				}
				else
				{
					if (!(this.Property.DefaultValue is string))
					{
						obj = this.Property.DefaultValue;
					}
					else
					{
						try
						{
							obj = SettingsPropertyValue.GetObjectFromString(this.Property.PropertyType, this.Property.SerializeAs, (string)this.Property.DefaultValue);
						}
						catch (Exception ex2)
						{
							throw new ArgumentException(SR.GetString("Could_not_create_from_default_value", new object[]
							{
								this.Property.Name,
								ex2.Message
							}));
						}
					}
					if (obj != null && !this.Property.PropertyType.IsAssignableFrom(obj.GetType()))
					{
						throw new ArgumentException(SR.GetString("Could_not_create_from_default_value_2", new object[]
						{
							this.Property.Name
						}));
					}
				}
			}
			if (obj == null)
			{
				if (this.Property.PropertyType == typeof(string))
				{
					obj = "";
				}
				else
				{
					try
					{
						obj = Activator.CreateInstance(this.Property.PropertyType);
					}
					catch
					{
					}
				}
			}
			return obj;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x000EB52C File Offset: 0x000EA52C
		private static object GetObjectFromString(Type type, SettingsSerializeAs serializeAs, string attValue)
		{
			if (type == typeof(string) && (attValue == null || attValue.Length < 1 || serializeAs == SettingsSerializeAs.String))
			{
				return attValue;
			}
			if (attValue == null || attValue.Length < 1)
			{
				return null;
			}
			switch (serializeAs)
			{
			case SettingsSerializeAs.String:
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
				{
					return converter.ConvertFromInvariantString(attValue);
				}
				throw new ArgumentException(SR.GetString("Unable_to_convert_type_from_string", new object[]
				{
					type.ToString()
				}), "type");
			}
			case SettingsSerializeAs.Xml:
				break;
			case SettingsSerializeAs.Binary:
			{
				byte[] buffer = Convert.FromBase64String(attValue);
				MemoryStream memoryStream = null;
				try
				{
					memoryStream = new MemoryStream(buffer);
					return new BinaryFormatter().Deserialize(memoryStream);
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
				break;
			}
			default:
				return null;
			}
			StringReader textReader = new StringReader(attValue);
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(textReader);
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x000EB63C File Offset: 0x000EA63C
		private object SerializePropertyValue()
		{
			if (this._Value == null)
			{
				return null;
			}
			if (this.Property.SerializeAs != SettingsSerializeAs.Binary)
			{
				return SettingsPropertyValue.ConvertObjectToString(this._Value, this.Property.PropertyType, this.Property.SerializeAs, this.Property.ThrowOnErrorSerializing);
			}
			MemoryStream memoryStream = new MemoryStream();
			object result;
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, this._Value);
				result = memoryStream.ToArray();
			}
			finally
			{
				memoryStream.Close();
			}
			return result;
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x000EB6C8 File Offset: 0x000EA6C8
		private static string ConvertObjectToString(object propValue, Type type, SettingsSerializeAs serializeAs, bool throwOnError)
		{
			if (serializeAs == SettingsSerializeAs.ProviderSpecific)
			{
				if (type == typeof(string) || type.IsPrimitive)
				{
					serializeAs = SettingsSerializeAs.String;
				}
				else
				{
					serializeAs = SettingsSerializeAs.Xml;
				}
			}
			try
			{
				switch (serializeAs)
				{
				case SettingsSerializeAs.String:
				{
					TypeConverter converter = TypeDescriptor.GetConverter(type);
					if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
					{
						return converter.ConvertToInvariantString(propValue);
					}
					throw new ArgumentException(SR.GetString("Unable_to_convert_type_to_string", new object[]
					{
						type.ToString()
					}), "type");
				}
				case SettingsSerializeAs.Xml:
					break;
				case SettingsSerializeAs.Binary:
				{
					MemoryStream memoryStream = new MemoryStream();
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						binaryFormatter.Serialize(memoryStream, propValue);
						byte[] inArray = memoryStream.ToArray();
						return Convert.ToBase64String(inArray);
					}
					finally
					{
						memoryStream.Close();
					}
					break;
				}
				default:
					goto IL_100;
				}
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				xmlSerializer.Serialize(stringWriter, propValue);
				return stringWriter.ToString();
			}
			catch (Exception)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			IL_100:
			return null;
		}

		// Token: 0x040031C4 RID: 12740
		private object _Value;

		// Token: 0x040031C5 RID: 12741
		private object _SerializedValue;

		// Token: 0x040031C6 RID: 12742
		private bool _Deserialized;

		// Token: 0x040031C7 RID: 12743
		private bool _IsDirty;

		// Token: 0x040031C8 RID: 12744
		private SettingsProperty _Property;

		// Token: 0x040031C9 RID: 12745
		private bool _ChangedSinceLastSerialized;

		// Token: 0x040031CA RID: 12746
		private bool _UsingDefaultValue = true;
	}
}
