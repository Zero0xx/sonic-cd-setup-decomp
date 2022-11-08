using System;
using System.Collections;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000853 RID: 2131
	internal sealed class FieldOnTypeBuilderInstantiation : FieldInfo
	{
		// Token: 0x06004E29 RID: 20009 RVA: 0x0010F700 File Offset: 0x0010E700
		internal static FieldInfo GetField(FieldInfo Field, TypeBuilderInstantiation type)
		{
			FieldOnTypeBuilderInstantiation.Entry entry = new FieldOnTypeBuilderInstantiation.Entry(Field, type);
			if (FieldOnTypeBuilderInstantiation.m_hashtable.Contains(entry))
			{
				return FieldOnTypeBuilderInstantiation.m_hashtable[entry] as FieldInfo;
			}
			FieldInfo fieldInfo = new FieldOnTypeBuilderInstantiation(Field, type);
			FieldOnTypeBuilderInstantiation.m_hashtable[entry] = fieldInfo;
			return fieldInfo;
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0010F758 File Offset: 0x0010E758
		internal FieldOnTypeBuilderInstantiation(FieldInfo field, TypeBuilderInstantiation type)
		{
			this.m_field = field;
			this.m_type = type;
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06004E2B RID: 20011 RVA: 0x0010F76E File Offset: 0x0010E76E
		internal FieldInfo FieldInfo
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06004E2C RID: 20012 RVA: 0x0010F776 File Offset: 0x0010E776
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06004E2D RID: 20013 RVA: 0x0010F779 File Offset: 0x0010E779
		public override string Name
		{
			get
			{
				return this.m_field.Name;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06004E2E RID: 20014 RVA: 0x0010F786 File Offset: 0x0010E786
		public override Type DeclaringType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06004E2F RID: 20015 RVA: 0x0010F78E File Offset: 0x0010E78E
		public override Type ReflectedType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0010F796 File Offset: 0x0010E796
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_field.GetCustomAttributes(inherit);
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0010F7A4 File Offset: 0x0010E7A4
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_field.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0010F7B3 File Offset: 0x0010E7B3
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_field.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06004E33 RID: 20019 RVA: 0x0010F7C2 File Offset: 0x0010E7C2
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x0010F7C9 File Offset: 0x0010E7C9
		public override Module Module
		{
			get
			{
				return this.m_field.Module;
			}
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x0010F7D6 File Offset: 0x0010E7D6
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0010F7DE File Offset: 0x0010E7DE
		public override Type[] GetRequiredCustomModifiers()
		{
			return this.m_field.GetRequiredCustomModifiers();
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0010F7EB File Offset: 0x0010E7EB
		public override Type[] GetOptionalCustomModifiers()
		{
			return this.m_field.GetOptionalCustomModifiers();
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x0010F7F8 File Offset: 0x0010E7F8
		public override void SetValueDirect(TypedReference obj, object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0010F7FF File Offset: 0x0010E7FF
		public override object GetValueDirect(TypedReference obj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x0010F806 File Offset: 0x0010E806
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06004E3B RID: 20027 RVA: 0x0010F80D File Offset: 0x0010E80D
		public override Type FieldType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x0010F814 File Offset: 0x0010E814
		public override object GetValue(object obj)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x0010F81B File Offset: 0x0010E81B
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0010F822 File Offset: 0x0010E822
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_field.Attributes;
			}
		}

		// Token: 0x04002854 RID: 10324
		private static Hashtable m_hashtable = new Hashtable();

		// Token: 0x04002855 RID: 10325
		private FieldInfo m_field;

		// Token: 0x04002856 RID: 10326
		private TypeBuilderInstantiation m_type;

		// Token: 0x02000854 RID: 2132
		private struct Entry
		{
			// Token: 0x06004E40 RID: 20032 RVA: 0x0010F83B File Offset: 0x0010E83B
			public Entry(FieldInfo Field, TypeBuilderInstantiation type)
			{
				this.m_field = Field;
				this.m_type = type;
			}

			// Token: 0x06004E41 RID: 20033 RVA: 0x0010F84B File Offset: 0x0010E84B
			public override int GetHashCode()
			{
				return this.m_field.GetHashCode();
			}

			// Token: 0x06004E42 RID: 20034 RVA: 0x0010F858 File Offset: 0x0010E858
			public override bool Equals(object o)
			{
				return o is FieldOnTypeBuilderInstantiation.Entry && this.Equals((FieldOnTypeBuilderInstantiation.Entry)o);
			}

			// Token: 0x06004E43 RID: 20035 RVA: 0x0010F870 File Offset: 0x0010E870
			public bool Equals(FieldOnTypeBuilderInstantiation.Entry obj)
			{
				return obj.m_field == this.m_field && obj.m_type == this.m_type;
			}

			// Token: 0x04002857 RID: 10327
			public FieldInfo m_field;

			// Token: 0x04002858 RID: 10328
			public TypeBuilderInstantiation m_type;
		}
	}
}
