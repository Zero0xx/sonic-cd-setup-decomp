using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007F9 RID: 2041
	internal sealed class ReadObjectInfo
	{
		// Token: 0x06004837 RID: 18487 RVA: 0x000FA37E File Offset: 0x000F937E
		internal ReadObjectInfo()
		{
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x000FA386 File Offset: 0x000F9386
		internal void ObjectEnd()
		{
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x000FA388 File Offset: 0x000F9388
		internal void PrepareForReuse()
		{
			this.lastPosition = 0;
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x000FA394 File Offset: 0x000F9394
		internal static ReadObjectInfo Create(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.Init(objectType, surrogateSelector, context, objectManager, serObjectInfoInit, converter, bSimpleAssembly);
			return objectInfo;
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x000FA3BA File Offset: 0x000F93BA
		internal void Init(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			this.objectType = objectType;
			this.objectManager = objectManager;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.formatterConverter = converter;
			this.bSimpleAssembly = bSimpleAssembly;
			this.InitReadConstructor(objectType, surrogateSelector, context);
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x000FA3F4 File Offset: 0x000F93F4
		internal static ReadObjectInfo Create(Type objectType, string[] memberNames, Type[] memberTypes, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.Init(objectType, memberNames, memberTypes, surrogateSelector, context, objectManager, serObjectInfoInit, converter, bSimpleAssembly);
			return objectInfo;
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x000FA420 File Offset: 0x000F9420
		internal void Init(Type objectType, string[] memberNames, Type[] memberTypes, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			this.objectType = objectType;
			this.objectManager = objectManager;
			this.wireMemberNames = memberNames;
			this.wireMemberTypes = memberTypes;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.formatterConverter = converter;
			this.bSimpleAssembly = bSimpleAssembly;
			if (memberNames != null)
			{
				this.isNamed = true;
			}
			if (memberTypes != null)
			{
				this.isTyped = true;
			}
			if (objectType != null)
			{
				this.InitReadConstructor(objectType, surrogateSelector, context);
			}
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x000FA48C File Offset: 0x000F948C
		private void InitReadConstructor(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context)
		{
			if (objectType.IsArray)
			{
				this.InitNoMembers();
				return;
			}
			ISurrogateSelector surrogateSelector2 = null;
			if (surrogateSelector != null)
			{
				this.serializationSurrogate = surrogateSelector.GetSurrogate(objectType, context, out surrogateSelector2);
			}
			if (this.serializationSurrogate != null)
			{
				this.isSi = true;
			}
			else if (objectType != Converter.typeofObject && Converter.typeofISerializable.IsAssignableFrom(objectType))
			{
				this.isSi = true;
			}
			if (this.isSi)
			{
				this.InitSiRead();
				return;
			}
			this.InitMemberInfo();
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x000FA4FF File Offset: 0x000F94FF
		private void InitSiRead()
		{
			if (this.memberTypesList != null)
			{
				this.memberTypesList = new ArrayList(20);
			}
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x000FA516 File Offset: 0x000F9516
		private void InitNoMembers()
		{
			this.cache = new SerObjectInfoCache();
			this.cache.fullTypeName = this.objectType.FullName;
			this.cache.assemblyString = this.objectType.Assembly.FullName;
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x000FA554 File Offset: 0x000F9554
		private void InitMemberInfo()
		{
			this.cache = new SerObjectInfoCache();
			this.cache.memberInfos = FormatterServices.GetSerializableMembers(this.objectType, this.context);
			this.count = this.cache.memberInfos.Length;
			this.cache.memberNames = new string[this.count];
			this.cache.memberTypes = new Type[this.count];
			for (int i = 0; i < this.count; i++)
			{
				this.cache.memberNames[i] = this.cache.memberInfos[i].Name;
				this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
			}
			this.cache.fullTypeName = this.objectType.FullName;
			this.cache.assemblyString = this.objectType.Assembly.FullName;
			this.isTyped = true;
			this.isNamed = true;
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x000FA658 File Offset: 0x000F9658
		internal MemberInfo GetMemberInfo(string name)
		{
			if (this.cache == null)
			{
				return null;
			}
			if (this.isSi)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MemberInfo"), new object[]
				{
					this.objectType + " " + name
				}));
			}
			if (this.cache.memberInfos == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NoMemberInfo"), new object[]
				{
					this.objectType + " " + name
				}));
			}
			int num = this.Position(name);
			if (num != -1)
			{
				return this.cache.memberInfos[this.Position(name)];
			}
			return null;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x000FA714 File Offset: 0x000F9714
		internal Type GetType(string name)
		{
			int num = this.Position(name);
			if (num == -1)
			{
				return null;
			}
			Type type;
			if (this.isTyped)
			{
				type = this.cache.memberTypes[num];
			}
			else
			{
				type = (Type)this.memberTypesList[num];
			}
			if (type == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ISerializableTypes"), new object[]
				{
					this.objectType + " " + name
				}));
			}
			return type;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x000FA798 File Offset: 0x000F9798
		internal void AddValue(string name, object value, ref SerializationInfo si, ref object[] memberData)
		{
			if (this.isSi)
			{
				si.AddValue(name, value);
				return;
			}
			int num = this.Position(name);
			if (num != -1)
			{
				memberData[num] = value;
			}
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x000FA7CC File Offset: 0x000F97CC
		internal void InitDataStore(ref SerializationInfo si, ref object[] memberData)
		{
			if (this.isSi)
			{
				if (si == null)
				{
					si = new SerializationInfo(this.objectType, this.formatterConverter);
					return;
				}
			}
			else if (memberData == null && this.cache != null)
			{
				memberData = new object[this.cache.memberNames.Length];
			}
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x000FA81C File Offset: 0x000F981C
		internal void RecordFixup(long objectId, string name, long idRef)
		{
			if (this.isSi)
			{
				this.objectManager.RecordDelayedFixup(objectId, name, idRef);
				return;
			}
			int num = this.Position(name);
			if (num != -1)
			{
				this.objectManager.RecordFixup(objectId, this.cache.memberInfos[num], idRef);
			}
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x000FA866 File Offset: 0x000F9866
		internal void PopulateObjectMembers(object obj, object[] memberData)
		{
			if (!this.isSi && memberData != null)
			{
				FormatterServices.PopulateObjectMembers(obj, this.cache.memberInfos, memberData);
			}
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x000FA888 File Offset: 0x000F9888
		[Conditional("SER_LOGGING")]
		private void DumpPopulate(MemberInfo[] memberInfos, object[] memberData)
		{
			for (int i = 0; i < memberInfos.Length; i++)
			{
			}
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x000FA8A3 File Offset: 0x000F98A3
		[Conditional("SER_LOGGING")]
		private void DumpPopulateSi()
		{
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x000FA8A8 File Offset: 0x000F98A8
		private int Position(string name)
		{
			if (this.cache == null)
			{
				return -1;
			}
			if (this.cache.memberNames.Length > 0 && this.cache.memberNames[this.lastPosition].Equals(name))
			{
				return this.lastPosition;
			}
			if (++this.lastPosition < this.cache.memberNames.Length && this.cache.memberNames[this.lastPosition].Equals(name))
			{
				return this.lastPosition;
			}
			for (int i = 0; i < this.cache.memberNames.Length; i++)
			{
				if (this.cache.memberNames[i].Equals(name))
				{
					this.lastPosition = i;
					return this.lastPosition;
				}
			}
			this.lastPosition = 0;
			return -1;
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x000FA974 File Offset: 0x000F9974
		internal Type[] GetMemberTypes(string[] inMemberNames, Type objectType)
		{
			if (this.isSi)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ISerializableTypes"), new object[]
				{
					objectType
				}));
			}
			if (this.cache == null)
			{
				return null;
			}
			if (this.cache.memberTypes == null)
			{
				this.cache.memberTypes = new Type[this.count];
				for (int i = 0; i < this.count; i++)
				{
					this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
				}
			}
			bool flag = false;
			if (inMemberNames.Length < this.cache.memberInfos.Length)
			{
				flag = true;
			}
			Type[] array = new Type[this.cache.memberInfos.Length];
			for (int j = 0; j < this.cache.memberInfos.Length; j++)
			{
				if (!flag && inMemberNames[j].Equals(this.cache.memberInfos[j].Name))
				{
					array[j] = this.cache.memberTypes[j];
				}
				else
				{
					bool flag2 = false;
					for (int k = 0; k < inMemberNames.Length; k++)
					{
						if (this.cache.memberInfos[j].Name.Equals(inMemberNames[k]))
						{
							array[j] = this.cache.memberTypes[j];
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						object[] customAttributes = this.cache.memberInfos[j].GetCustomAttributes(typeof(OptionalFieldAttribute), false);
						if ((customAttributes == null || customAttributes.Length == 0) && !this.bSimpleAssembly)
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MissingMember"), new object[]
							{
								this.cache.memberNames[j],
								objectType,
								typeof(OptionalFieldAttribute).FullName
							}));
						}
					}
				}
			}
			return array;
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x000FAB64 File Offset: 0x000F9B64
		internal Type GetMemberType(MemberInfo objMember)
		{
			Type result;
			if (objMember is FieldInfo)
			{
				result = ((FieldInfo)objMember).FieldType;
			}
			else
			{
				if (!(objMember is PropertyInfo))
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_SerMemberInfo"), new object[]
					{
						objMember.GetType()
					}));
				}
				result = ((PropertyInfo)objMember).PropertyType;
			}
			return result;
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x000FABCC File Offset: 0x000F9BCC
		private static ReadObjectInfo GetObjectInfo(SerObjectInfoInit serObjectInfoInit)
		{
			return new ReadObjectInfo
			{
				objectInfoId = ReadObjectInfo.readObjectInfoCounter++
			};
		}

		// Token: 0x04002528 RID: 9512
		internal int objectInfoId;

		// Token: 0x04002529 RID: 9513
		internal static int readObjectInfoCounter = 1;

		// Token: 0x0400252A RID: 9514
		internal Type objectType;

		// Token: 0x0400252B RID: 9515
		internal ObjectManager objectManager;

		// Token: 0x0400252C RID: 9516
		internal int count;

		// Token: 0x0400252D RID: 9517
		internal bool isSi;

		// Token: 0x0400252E RID: 9518
		internal bool isNamed;

		// Token: 0x0400252F RID: 9519
		internal bool isTyped;

		// Token: 0x04002530 RID: 9520
		internal bool bSimpleAssembly;

		// Token: 0x04002531 RID: 9521
		internal SerObjectInfoCache cache;

		// Token: 0x04002532 RID: 9522
		internal string[] wireMemberNames;

		// Token: 0x04002533 RID: 9523
		internal Type[] wireMemberTypes;

		// Token: 0x04002534 RID: 9524
		private int lastPosition;

		// Token: 0x04002535 RID: 9525
		internal ISurrogateSelector surrogateSelector;

		// Token: 0x04002536 RID: 9526
		internal ISerializationSurrogate serializationSurrogate;

		// Token: 0x04002537 RID: 9527
		internal StreamingContext context;

		// Token: 0x04002538 RID: 9528
		internal ArrayList memberTypesList;

		// Token: 0x04002539 RID: 9529
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x0400253A RID: 9530
		internal IFormatterConverter formatterConverter;
	}
}
