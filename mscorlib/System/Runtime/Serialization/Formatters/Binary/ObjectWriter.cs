using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Text;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007F6 RID: 2038
	internal sealed class ObjectWriter
	{
		// Token: 0x060047EF RID: 18415 RVA: 0x000F7CAD File Offset: 0x000F6CAD
		internal ObjectWriter(ISurrogateSelector selector, StreamingContext context, InternalFE formatterEnums)
		{
			this.m_currentId = 1;
			this.m_surrogates = selector;
			this.m_context = context;
			this.formatterEnums = formatterEnums;
			this.m_objectManager = new SerializationObjectManager(context);
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x000F7CF0 File Offset: 0x000F6CF0
		internal void Serialize(object graph, Header[] inHeaders, __BinaryWriter serWriter, bool fCheck)
		{
			if (fCheck)
			{
				CodeAccessPermission.DemandInternal(PermissionType.SecuritySerialization);
			}
			if (graph == null)
			{
				throw new ArgumentNullException("graph", Environment.GetResourceString("ArgumentNull_Graph"));
			}
			if (serWriter == null)
			{
				throw new ArgumentNullException("serWriter", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_WithParamName"), new object[]
				{
					"serWriter"
				}));
			}
			this.serWriter = serWriter;
			this.headers = inHeaders;
			serWriter.WriteBegin();
			long headerId = 0L;
			bool flag = false;
			bool flag2 = false;
			IMethodCallMessage methodCallMessage = graph as IMethodCallMessage;
			if (methodCallMessage != null)
			{
				flag = true;
				graph = this.WriteMethodCall(methodCallMessage);
			}
			else
			{
				IMethodReturnMessage methodReturnMessage = graph as IMethodReturnMessage;
				if (methodReturnMessage != null)
				{
					flag2 = true;
					graph = this.WriteMethodReturn(methodReturnMessage);
				}
			}
			if (graph == null)
			{
				this.WriteSerializedStreamHeader(this.topId, headerId);
				if (flag)
				{
					serWriter.WriteMethodCall();
				}
				else if (flag2)
				{
					serWriter.WriteMethodReturn();
				}
				serWriter.WriteSerializationHeaderEnd();
				serWriter.WriteEnd();
				return;
			}
			this.m_idGenerator = new ObjectIDGenerator();
			this.m_objectQueue = new Queue();
			this.m_formatterConverter = new FormatterConverter();
			this.serObjectInfoInit = new SerObjectInfoInit();
			bool flag3;
			this.topId = this.InternalGetId(graph, false, null, out flag3);
			if (this.headers != null)
			{
				headerId = this.InternalGetId(this.headers, false, null, out flag3);
			}
			else
			{
				headerId = -1L;
			}
			this.WriteSerializedStreamHeader(this.topId, headerId);
			if (flag)
			{
				serWriter.WriteMethodCall();
			}
			else if (flag2)
			{
				serWriter.WriteMethodReturn();
			}
			if (this.headers != null && this.headers.Length > 0)
			{
				this.m_objectQueue.Enqueue(this.headers);
			}
			if (graph != null)
			{
				this.m_objectQueue.Enqueue(graph);
			}
			long objectId;
			object next;
			while ((next = this.GetNext(out objectId)) != null)
			{
				WriteObjectInfo writeObjectInfo;
				if (next is WriteObjectInfo)
				{
					writeObjectInfo = (WriteObjectInfo)next;
				}
				else
				{
					writeObjectInfo = WriteObjectInfo.Serialize(next, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, this);
					writeObjectInfo.assemId = this.GetAssemblyId(writeObjectInfo);
				}
				writeObjectInfo.objectId = objectId;
				NameInfo nameInfo = this.TypeToNameInfo(writeObjectInfo);
				this.Write(writeObjectInfo, nameInfo, nameInfo);
				this.PutNameInfo(nameInfo);
				writeObjectInfo.ObjectEnd();
			}
			serWriter.WriteSerializationHeaderEnd();
			serWriter.WriteEnd();
			this.m_objectManager.RaiseOnSerializedEvent();
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x000F7F24 File Offset: 0x000F6F24
		private object[] WriteMethodCall(IMethodCallMessage mcm)
		{
			string uri = mcm.Uri;
			string methodName = mcm.MethodName;
			string typeName = mcm.TypeName;
			object methodSignature = null;
			object[] properties = null;
			Type[] instArgs = null;
			if (mcm.MethodBase.IsGenericMethod)
			{
				instArgs = mcm.MethodBase.GetGenericArguments();
			}
			object[] args = mcm.Args;
			IInternalMessage internalMessage = mcm as IInternalMessage;
			if (internalMessage == null || internalMessage.HasProperties())
			{
				properties = ObjectWriter.StoreUserPropertiesForMethodMessage(mcm);
			}
			if (mcm.MethodSignature != null && RemotingServices.IsMethodOverloaded(mcm))
			{
				methodSignature = mcm.MethodSignature;
			}
			LogicalCallContext logicalCallContext = mcm.LogicalCallContext;
			object callContext;
			if (logicalCallContext == null)
			{
				callContext = null;
			}
			else if (logicalCallContext.HasInfo)
			{
				callContext = logicalCallContext;
			}
			else
			{
				callContext = logicalCallContext.RemotingData.LogicalCallID;
			}
			return this.serWriter.WriteCallArray(uri, methodName, typeName, instArgs, args, methodSignature, callContext, properties);
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x000F7FEC File Offset: 0x000F6FEC
		private object[] WriteMethodReturn(IMethodReturnMessage mrm)
		{
			object returnValue = mrm.ReturnValue;
			object[] args = mrm.Args;
			Exception exception = mrm.Exception;
			object[] properties = null;
			ReturnMessage returnMessage = mrm as ReturnMessage;
			if (returnMessage == null || returnMessage.HasProperties())
			{
				properties = ObjectWriter.StoreUserPropertiesForMethodMessage(mrm);
			}
			LogicalCallContext logicalCallContext = mrm.LogicalCallContext;
			object callContext;
			if (logicalCallContext == null)
			{
				callContext = null;
			}
			else if (logicalCallContext.HasInfo)
			{
				callContext = logicalCallContext;
			}
			else
			{
				callContext = logicalCallContext.RemotingData.LogicalCallID;
			}
			return this.serWriter.WriteReturnArray(returnValue, args, exception, callContext, properties);
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x000F806C File Offset: 0x000F706C
		private static object[] StoreUserPropertiesForMethodMessage(IMethodMessage msg)
		{
			ArrayList arrayList = null;
			IDictionary properties = msg.Properties;
			if (properties == null)
			{
				return null;
			}
			MessageDictionary messageDictionary = properties as MessageDictionary;
			if (messageDictionary != null)
			{
				if (messageDictionary.HasUserData())
				{
					int num = 0;
					foreach (object obj in messageDictionary.InternalDictionary)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(dictionaryEntry);
						num++;
					}
					return arrayList.ToArray();
				}
				return null;
			}
			else
			{
				int num2 = 0;
				foreach (object obj2 in properties)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(dictionaryEntry2);
					num2++;
				}
				if (arrayList != null)
				{
					return arrayList.ToArray();
				}
				return null;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060047F4 RID: 18420 RVA: 0x000F817C File Offset: 0x000F717C
		internal SerializationObjectManager ObjectManager
		{
			get
			{
				return this.m_objectManager;
			}
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x000F8184 File Offset: 0x000F7184
		private void Write(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			object obj = objectInfo.obj;
			if (obj == null)
			{
				throw new ArgumentNullException("objectInfo.obj", Environment.GetResourceString("ArgumentNull_Obj"));
			}
			Type objectType = objectInfo.objectType;
			long objectId = objectInfo.objectId;
			if (objectType == Converter.typeofString)
			{
				memberNameInfo.NIobjectId = objectId;
				this.serWriter.WriteObjectString((int)objectId, obj.ToString());
				return;
			}
			if (objectInfo.isArray)
			{
				this.WriteArray(objectInfo, memberNameInfo, null);
				return;
			}
			string[] array;
			Type[] array2;
			object[] array3;
			objectInfo.GetMemberInfo(out array, out array2, out array3);
			if (objectInfo.isSi || this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways))
			{
				memberNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NIisParentTypeOnObject = true;
				typeNameInfo.NItransmitTypeOnObject = true;
				typeNameInfo.NIisParentTypeOnObject = true;
			}
			WriteObjectInfo[] array4 = new WriteObjectInfo[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				Type type;
				if (array2[i] != null)
				{
					type = array2[i];
				}
				else if (array3[i] != null)
				{
					type = this.GetType(array3[i]);
				}
				else
				{
					type = Converter.typeofObject;
				}
				if (this.ToCode(type) == InternalPrimitiveTypeE.Invalid && type != Converter.typeofString)
				{
					if (array3[i] != null)
					{
						array4[i] = WriteObjectInfo.Serialize(array3[i], this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, this);
						array4[i].assemId = this.GetAssemblyId(array4[i]);
					}
					else
					{
						array4[i] = WriteObjectInfo.Serialize(array2[i], this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter);
						array4[i].assemId = this.GetAssemblyId(array4[i]);
					}
				}
			}
			this.Write(objectInfo, memberNameInfo, typeNameInfo, array, array2, array3, array4);
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x000F8338 File Offset: 0x000F7338
		private void Write(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo, string[] memberNames, Type[] memberTypes, object[] memberData, WriteObjectInfo[] memberObjectInfos)
		{
			int num = memberNames.Length;
			NameInfo nameInfo = null;
			if (memberNameInfo != null)
			{
				memberNameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObject(memberNameInfo, typeNameInfo, num, memberNames, memberTypes, memberObjectInfos);
			}
			else if (objectInfo.objectId == this.topId && this.topName != null)
			{
				nameInfo = this.MemberToNameInfo(this.topName);
				nameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObject(nameInfo, typeNameInfo, num, memberNames, memberTypes, memberObjectInfos);
			}
			else if (objectInfo.objectType != Converter.typeofString)
			{
				typeNameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObject(typeNameInfo, null, num, memberNames, memberTypes, memberObjectInfos);
			}
			if (memberNameInfo.NIisParentTypeOnObject)
			{
				memberNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NIisParentTypeOnObject = false;
			}
			else
			{
				memberNameInfo.NItransmitTypeOnObject = false;
			}
			for (int i = 0; i < num; i++)
			{
				this.WriteMemberSetup(objectInfo, memberNameInfo, typeNameInfo, memberNames[i], memberTypes[i], memberData[i], memberObjectInfos[i]);
			}
			if (memberNameInfo != null)
			{
				memberNameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObjectEnd(memberNameInfo, typeNameInfo);
				return;
			}
			if (objectInfo.objectId == this.topId && this.topName != null)
			{
				this.serWriter.WriteObjectEnd(nameInfo, typeNameInfo);
				this.PutNameInfo(nameInfo);
				return;
			}
			if (objectInfo.objectType != Converter.typeofString)
			{
				objectInfo.GetTypeFullName();
				this.serWriter.WriteObjectEnd(typeNameInfo, typeNameInfo);
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x000F8490 File Offset: 0x000F7490
		private void WriteMemberSetup(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo, string memberName, Type memberType, object memberData, WriteObjectInfo memberObjectInfo)
		{
			NameInfo nameInfo = this.MemberToNameInfo(memberName);
			if (memberObjectInfo != null)
			{
				nameInfo.NIassemId = memberObjectInfo.assemId;
			}
			nameInfo.NItype = memberType;
			NameInfo nameInfo2;
			if (memberObjectInfo == null)
			{
				nameInfo2 = this.TypeToNameInfo(memberType);
			}
			else
			{
				nameInfo2 = this.TypeToNameInfo(memberObjectInfo);
			}
			nameInfo.NItransmitTypeOnObject = memberNameInfo.NItransmitTypeOnObject;
			nameInfo.NIisParentTypeOnObject = memberNameInfo.NIisParentTypeOnObject;
			this.WriteMembers(nameInfo, nameInfo2, memberData, objectInfo, typeNameInfo, memberObjectInfo);
			this.PutNameInfo(nameInfo);
			this.PutNameInfo(nameInfo2);
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x000F8510 File Offset: 0x000F7510
		private void WriteMembers(NameInfo memberNameInfo, NameInfo memberTypeNameInfo, object memberData, WriteObjectInfo objectInfo, NameInfo typeNameInfo, WriteObjectInfo memberObjectInfo)
		{
			Type type = memberNameInfo.NItype;
			bool assignUniqueIdToValueType = false;
			if (type == Converter.typeofObject || Nullable.GetUnderlyingType(type) != null)
			{
				memberTypeNameInfo.NItransmitTypeOnMember = true;
				memberNameInfo.NItransmitTypeOnMember = true;
			}
			if (this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways) || objectInfo.isSi)
			{
				memberTypeNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NIisParentTypeOnObject = true;
			}
			if (this.CheckForNull(objectInfo, memberNameInfo, memberTypeNameInfo, memberData))
			{
				return;
			}
			Type type2 = null;
			if (memberTypeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Invalid)
			{
				type2 = this.GetType(memberData);
				if (type != type2)
				{
					memberTypeNameInfo.NItransmitTypeOnMember = true;
					memberNameInfo.NItransmitTypeOnMember = true;
				}
			}
			if (type == Converter.typeofObject)
			{
				assignUniqueIdToValueType = true;
				type = this.GetType(memberData);
				if (memberObjectInfo == null)
				{
					this.TypeToNameInfo(type, memberTypeNameInfo);
				}
				else
				{
					this.TypeToNameInfo(memberObjectInfo, memberTypeNameInfo);
				}
			}
			if (memberObjectInfo == null || !memberObjectInfo.isArray)
			{
				if (!this.WriteKnownValueClass(memberNameInfo, memberTypeNameInfo, memberData))
				{
					if (type2 == null)
					{
						type2 = this.GetType(memberData);
					}
					long num = this.Schedule(memberData, assignUniqueIdToValueType, type2, memberObjectInfo);
					if (num < 0L)
					{
						memberObjectInfo.objectId = num;
						NameInfo nameInfo = this.TypeToNameInfo(memberObjectInfo);
						nameInfo.NIobjectId = num;
						this.Write(memberObjectInfo, memberNameInfo, nameInfo);
						this.PutNameInfo(nameInfo);
						memberObjectInfo.ObjectEnd();
						return;
					}
					memberNameInfo.NIobjectId = num;
					this.WriteObjectRef(memberNameInfo, num);
				}
				return;
			}
			if (type2 == null)
			{
				type2 = this.GetType(memberData);
			}
			long num2 = this.Schedule(memberData, false, null, memberObjectInfo);
			if (num2 > 0L)
			{
				memberNameInfo.NIobjectId = num2;
				this.WriteObjectRef(memberNameInfo, num2);
				return;
			}
			this.serWriter.WriteMemberNested(memberNameInfo);
			memberObjectInfo.objectId = num2;
			memberNameInfo.NIobjectId = num2;
			this.WriteArray(memberObjectInfo, memberNameInfo, memberObjectInfo);
			objectInfo.ObjectEnd();
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x000F86B8 File Offset: 0x000F76B8
		private void WriteArray(WriteObjectInfo objectInfo, NameInfo memberNameInfo, WriteObjectInfo memberObjectInfo)
		{
			bool flag = false;
			if (memberNameInfo == null)
			{
				memberNameInfo = this.TypeToNameInfo(objectInfo);
				flag = true;
			}
			memberNameInfo.NIisArray = true;
			long objectId = objectInfo.objectId;
			memberNameInfo.NIobjectId = objectInfo.objectId;
			Array array = (Array)objectInfo.obj;
			Type objectType = objectInfo.objectType;
			Type elementType = objectType.GetElementType();
			WriteObjectInfo writeObjectInfo = null;
			if (!elementType.IsPrimitive)
			{
				writeObjectInfo = WriteObjectInfo.Serialize(elementType, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter);
				writeObjectInfo.assemId = this.GetAssemblyId(writeObjectInfo);
			}
			NameInfo nameInfo;
			if (writeObjectInfo == null)
			{
				nameInfo = this.TypeToNameInfo(elementType);
			}
			else
			{
				nameInfo = this.TypeToNameInfo(writeObjectInfo);
			}
			nameInfo.NIisArray = nameInfo.NItype.IsArray;
			NameInfo nameInfo2 = memberNameInfo;
			nameInfo2.NIobjectId = objectId;
			nameInfo2.NIisArray = true;
			nameInfo.NIobjectId = objectId;
			nameInfo.NItransmitTypeOnMember = memberNameInfo.NItransmitTypeOnMember;
			nameInfo.NItransmitTypeOnObject = memberNameInfo.NItransmitTypeOnObject;
			nameInfo.NIisParentTypeOnObject = memberNameInfo.NIisParentTypeOnObject;
			int rank = array.Rank;
			int[] array2 = new int[rank];
			int[] array3 = new int[rank];
			int[] array4 = new int[rank];
			for (int i = 0; i < rank; i++)
			{
				array2[i] = array.GetLength(i);
				array3[i] = array.GetLowerBound(i);
				array4[i] = array.GetUpperBound(i);
			}
			InternalArrayTypeE internalArrayTypeE;
			if (nameInfo.NIisArray)
			{
				if (rank == 1)
				{
					internalArrayTypeE = InternalArrayTypeE.Jagged;
				}
				else
				{
					internalArrayTypeE = InternalArrayTypeE.Rectangular;
				}
			}
			else if (rank == 1)
			{
				internalArrayTypeE = InternalArrayTypeE.Single;
			}
			else
			{
				internalArrayTypeE = InternalArrayTypeE.Rectangular;
			}
			nameInfo.NIarrayEnum = internalArrayTypeE;
			if (elementType == Converter.typeofByte && rank == 1 && array3[0] == 0)
			{
				this.serWriter.WriteObjectByteArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, array2[0], array3[0], (byte[])array);
				return;
			}
			if (elementType == Converter.typeofObject || Nullable.GetUnderlyingType(elementType) != null)
			{
				memberNameInfo.NItransmitTypeOnMember = true;
				nameInfo.NItransmitTypeOnMember = true;
			}
			if (this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways))
			{
				memberNameInfo.NItransmitTypeOnObject = true;
				nameInfo.NItransmitTypeOnObject = true;
			}
			if (internalArrayTypeE == InternalArrayTypeE.Single)
			{
				this.serWriter.WriteSingleArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, array2[0], array3[0], array);
				if (!Converter.IsWriteAsByteArray(nameInfo.NIprimitiveTypeEnum) || array3[0] != 0)
				{
					object[] array5 = null;
					if (!elementType.IsValueType)
					{
						array5 = (object[])array;
					}
					int num = array4[0] + 1;
					for (int j = array3[0]; j < num; j++)
					{
						if (array5 == null)
						{
							this.WriteArrayMember(objectInfo, nameInfo, array.GetValue(j));
						}
						else
						{
							this.WriteArrayMember(objectInfo, nameInfo, array5[j]);
						}
					}
					this.serWriter.WriteItemEnd();
				}
			}
			else if (internalArrayTypeE == InternalArrayTypeE.Jagged)
			{
				nameInfo2.NIobjectId = objectId;
				this.serWriter.WriteJaggedArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, array2[0], array3[0]);
				object[] array6 = (object[])array;
				for (int k = array3[0]; k < array4[0] + 1; k++)
				{
					this.WriteArrayMember(objectInfo, nameInfo, array6[k]);
				}
				this.serWriter.WriteItemEnd();
			}
			else
			{
				nameInfo2.NIobjectId = objectId;
				this.serWriter.WriteRectangleArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, rank, array2, array3);
				bool flag2 = false;
				for (int l = 0; l < rank; l++)
				{
					if (array2[l] == 0)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					this.WriteRectangle(objectInfo, rank, array2, array, nameInfo, array3);
				}
				this.serWriter.WriteItemEnd();
			}
			this.serWriter.WriteObjectEnd(memberNameInfo, nameInfo2);
			this.PutNameInfo(nameInfo);
			if (flag)
			{
				this.PutNameInfo(memberNameInfo);
			}
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x000F8A3C File Offset: 0x000F7A3C
		private void WriteArrayMember(WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, object data)
		{
			arrayElemTypeNameInfo.NIisArrayItem = true;
			if (this.CheckForNull(objectInfo, arrayElemTypeNameInfo, arrayElemTypeNameInfo, data))
			{
				return;
			}
			Type type = null;
			bool flag = false;
			if (arrayElemTypeNameInfo.NItransmitTypeOnMember)
			{
				flag = true;
			}
			if (!flag && !arrayElemTypeNameInfo.IsSealed)
			{
				type = this.GetType(data);
				if (arrayElemTypeNameInfo.NItype != type)
				{
					flag = true;
				}
			}
			NameInfo nameInfo;
			if (flag)
			{
				if (type == null)
				{
					type = this.GetType(data);
				}
				nameInfo = this.TypeToNameInfo(type);
				nameInfo.NItransmitTypeOnMember = true;
				nameInfo.NIobjectId = arrayElemTypeNameInfo.NIobjectId;
				nameInfo.NIassemId = arrayElemTypeNameInfo.NIassemId;
				nameInfo.NIisArrayItem = true;
			}
			else
			{
				nameInfo = arrayElemTypeNameInfo;
				nameInfo.NIisArrayItem = true;
			}
			if (!this.WriteKnownValueClass(arrayElemTypeNameInfo, nameInfo, data))
			{
				bool assignUniqueIdToValueType = false;
				if (arrayElemTypeNameInfo.NItype == Converter.typeofObject)
				{
					assignUniqueIdToValueType = true;
				}
				long num = this.Schedule(data, assignUniqueIdToValueType, nameInfo.NItype);
				arrayElemTypeNameInfo.NIobjectId = num;
				nameInfo.NIobjectId = num;
				if (num < 1L)
				{
					WriteObjectInfo writeObjectInfo = WriteObjectInfo.Serialize(data, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, this);
					writeObjectInfo.objectId = num;
					if (arrayElemTypeNameInfo.NItype != Converter.typeofObject && Nullable.GetUnderlyingType(arrayElemTypeNameInfo.NItype) == null)
					{
						writeObjectInfo.assemId = nameInfo.NIassemId;
					}
					else
					{
						writeObjectInfo.assemId = this.GetAssemblyId(writeObjectInfo);
					}
					NameInfo nameInfo2 = this.TypeToNameInfo(writeObjectInfo);
					nameInfo2.NIobjectId = num;
					writeObjectInfo.objectId = num;
					this.Write(writeObjectInfo, nameInfo, nameInfo2);
					writeObjectInfo.ObjectEnd();
				}
				else
				{
					this.serWriter.WriteItemObjectRef(arrayElemTypeNameInfo, (int)num);
				}
			}
			if (arrayElemTypeNameInfo.NItransmitTypeOnMember)
			{
				this.PutNameInfo(nameInfo);
			}
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x000F8BD0 File Offset: 0x000F7BD0
		private void WriteRectangle(WriteObjectInfo objectInfo, int rank, int[] maxA, Array array, NameInfo arrayElemNameTypeInfo, int[] lowerBoundA)
		{
			int[] array2 = new int[rank];
			int[] array3 = null;
			bool flag = false;
			if (lowerBoundA != null)
			{
				for (int i = 0; i < rank; i++)
				{
					if (lowerBoundA[i] != 0)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				array3 = new int[rank];
			}
			bool flag2 = true;
			while (flag2)
			{
				flag2 = false;
				if (flag)
				{
					for (int j = 0; j < rank; j++)
					{
						array3[j] = array2[j] + lowerBoundA[j];
					}
					this.WriteArrayMember(objectInfo, arrayElemNameTypeInfo, array.GetValue(array3));
				}
				else
				{
					this.WriteArrayMember(objectInfo, arrayElemNameTypeInfo, array.GetValue(array2));
				}
				for (int k = rank - 1; k > -1; k--)
				{
					if (array2[k] < maxA[k] - 1)
					{
						array2[k]++;
						if (k < rank - 1)
						{
							for (int l = k + 1; l < rank; l++)
							{
								array2[l] = 0;
							}
						}
						flag2 = true;
						break;
					}
				}
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x000F8CB8 File Offset: 0x000F7CB8
		[Conditional("SER_LOGGING")]
		private void IndexTraceMessage(string message, int[] index)
		{
			StringBuilder stringBuilder = new StringBuilder(10);
			stringBuilder.Append("[");
			for (int i = 0; i < index.Length; i++)
			{
				stringBuilder.Append(index[i]);
				if (i != index.Length - 1)
				{
					stringBuilder.Append(",");
				}
			}
			stringBuilder.Append("]");
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x000F8D14 File Offset: 0x000F7D14
		private object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			object obj2;
			if (obj is WriteObjectInfo)
			{
				obj2 = ((WriteObjectInfo)obj).obj;
			}
			else
			{
				obj2 = obj;
			}
			bool flag;
			objID = this.m_idGenerator.HasId(obj2, out flag);
			if (flag)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjNoID"), new object[]
				{
					obj2
				}));
			}
			return obj;
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x000F8D94 File Offset: 0x000F7D94
		private long InternalGetId(object obj, bool assignUniqueIdToValueType, Type type, out bool isNew)
		{
			if (obj == this.previousObj)
			{
				isNew = false;
				return this.previousId;
			}
			this.m_idGenerator.m_currentCount = this.m_currentId;
			if (type != null && type.IsValueType && !assignUniqueIdToValueType)
			{
				isNew = false;
				return (long)(-1 * this.m_currentId++);
			}
			this.m_currentId++;
			long id = this.m_idGenerator.GetId(obj, out isNew);
			this.previousObj = obj;
			this.previousId = id;
			return id;
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x000F8E19 File Offset: 0x000F7E19
		private long Schedule(object obj, bool assignUniqueIdToValueType, Type type)
		{
			return this.Schedule(obj, assignUniqueIdToValueType, type, null);
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x000F8E28 File Offset: 0x000F7E28
		private long Schedule(object obj, bool assignUniqueIdToValueType, Type type, WriteObjectInfo objectInfo)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long num = this.InternalGetId(obj, assignUniqueIdToValueType, type, out flag);
			if (flag && num > 0L)
			{
				if (objectInfo == null)
				{
					this.m_objectQueue.Enqueue(obj);
				}
				else
				{
					this.m_objectQueue.Enqueue(objectInfo);
				}
			}
			return num;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x000F8E70 File Offset: 0x000F7E70
		private bool WriteKnownValueClass(NameInfo memberNameInfo, NameInfo typeNameInfo, object data)
		{
			if (typeNameInfo.NItype == Converter.typeofString)
			{
				this.WriteString(memberNameInfo, typeNameInfo, data);
			}
			else
			{
				if (typeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Invalid)
				{
					return false;
				}
				if (typeNameInfo.NIisArray)
				{
					this.serWriter.WriteItem(memberNameInfo, typeNameInfo, data);
				}
				else
				{
					this.serWriter.WriteMember(memberNameInfo, typeNameInfo, data);
				}
			}
			return true;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x000F8EC6 File Offset: 0x000F7EC6
		private void WriteObjectRef(NameInfo nameInfo, long objectId)
		{
			this.serWriter.WriteMemberObjectRef(nameInfo, (int)objectId);
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x000F8ED8 File Offset: 0x000F7ED8
		private void WriteString(NameInfo memberNameInfo, NameInfo typeNameInfo, object stringObject)
		{
			bool flag = true;
			long num = -1L;
			if (!this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.XsdString))
			{
				num = this.InternalGetId(stringObject, false, null, out flag);
			}
			typeNameInfo.NIobjectId = num;
			if (flag || num < 0L)
			{
				this.serWriter.WriteMemberString(memberNameInfo, typeNameInfo, (string)stringObject);
				return;
			}
			this.WriteObjectRef(memberNameInfo, num);
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x000F8F38 File Offset: 0x000F7F38
		private bool CheckForNull(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo, object data)
		{
			bool flag = false;
			if (data == null)
			{
				flag = true;
			}
			if (flag && (this.formatterEnums.FEserializerTypeEnum == InternalSerializerTypeE.Binary || memberNameInfo.NIisArrayItem || memberNameInfo.NItransmitTypeOnObject || memberNameInfo.NItransmitTypeOnMember || objectInfo.isSi || this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways)))
			{
				if (typeNameInfo.NIisArrayItem)
				{
					if (typeNameInfo.NIarrayEnum == InternalArrayTypeE.Single)
					{
						this.serWriter.WriteDelayedNullItem();
					}
					else
					{
						this.serWriter.WriteNullItem(memberNameInfo, typeNameInfo);
					}
				}
				else
				{
					this.serWriter.WriteNullMember(memberNameInfo, typeNameInfo);
				}
			}
			return flag;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x000F8FCD File Offset: 0x000F7FCD
		private void WriteSerializedStreamHeader(long topId, long headerId)
		{
			this.serWriter.WriteSerializationHeader((int)topId, (int)headerId, 1, 0);
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x000F8FE0 File Offset: 0x000F7FE0
		private NameInfo TypeToNameInfo(Type type, WriteObjectInfo objectInfo, InternalPrimitiveTypeE code, NameInfo nameInfo)
		{
			if (nameInfo == null)
			{
				nameInfo = this.GetNameInfo();
			}
			else
			{
				nameInfo.Init();
			}
			if (code == InternalPrimitiveTypeE.Invalid && objectInfo != null)
			{
				nameInfo.NIname = objectInfo.GetTypeFullName();
				nameInfo.NIassemId = objectInfo.assemId;
			}
			nameInfo.NIprimitiveTypeEnum = code;
			nameInfo.NItype = type;
			return nameInfo;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x000F9034 File Offset: 0x000F8034
		private NameInfo TypeToNameInfo(Type type)
		{
			return this.TypeToNameInfo(type, null, this.ToCode(type), null);
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x000F9046 File Offset: 0x000F8046
		private NameInfo TypeToNameInfo(WriteObjectInfo objectInfo)
		{
			return this.TypeToNameInfo(objectInfo.objectType, objectInfo, this.ToCode(objectInfo.objectType), null);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x000F9062 File Offset: 0x000F8062
		private NameInfo TypeToNameInfo(WriteObjectInfo objectInfo, NameInfo nameInfo)
		{
			return this.TypeToNameInfo(objectInfo.objectType, objectInfo, this.ToCode(objectInfo.objectType), nameInfo);
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x000F907E File Offset: 0x000F807E
		private void TypeToNameInfo(Type type, NameInfo nameInfo)
		{
			this.TypeToNameInfo(type, null, this.ToCode(type), nameInfo);
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x000F9094 File Offset: 0x000F8094
		private NameInfo MemberToNameInfo(string name)
		{
			NameInfo nameInfo = this.GetNameInfo();
			nameInfo.NIname = name;
			return nameInfo;
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x000F90B0 File Offset: 0x000F80B0
		internal InternalPrimitiveTypeE ToCode(Type type)
		{
			if (this.previousType == type)
			{
				return this.previousCode;
			}
			InternalPrimitiveTypeE internalPrimitiveTypeE = Converter.ToCode(type);
			if (internalPrimitiveTypeE != InternalPrimitiveTypeE.Invalid)
			{
				this.previousType = type;
				this.previousCode = internalPrimitiveTypeE;
			}
			return internalPrimitiveTypeE;
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x000F90E8 File Offset: 0x000F80E8
		private long GetAssemblyId(WriteObjectInfo objectInfo)
		{
			if (this.assemblyToIdTable == null)
			{
				this.assemblyToIdTable = new Hashtable(5);
			}
			bool isNew = false;
			string assemblyString = objectInfo.GetAssemblyString();
			string assemblyString2 = assemblyString;
			long num;
			if (assemblyString.Length == 0)
			{
				num = 0L;
			}
			else if (assemblyString.Equals(Converter.urtAssemblyString))
			{
				num = 0L;
			}
			else
			{
				if (this.assemblyToIdTable.ContainsKey(assemblyString))
				{
					num = (long)this.assemblyToIdTable[assemblyString];
					isNew = false;
				}
				else
				{
					num = this.InternalGetId("___AssemblyString___" + assemblyString, false, null, out isNew);
					this.assemblyToIdTable[assemblyString] = num;
				}
				this.serWriter.WriteAssembly(objectInfo.GetTypeFullName(), objectInfo.objectType, assemblyString2, (int)num, isNew, false);
			}
			return num;
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x000F91A0 File Offset: 0x000F81A0
		private Type GetType(object obj)
		{
			Type result;
			if (RemotingServices.IsTransparentProxy(obj))
			{
				result = Converter.typeofMarshalByRefObject;
			}
			else
			{
				result = obj.GetType();
			}
			return result;
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x000F91C8 File Offset: 0x000F81C8
		private NameInfo GetNameInfo()
		{
			NameInfo nameInfo;
			if (!this.niPool.IsEmpty())
			{
				nameInfo = (NameInfo)this.niPool.Pop();
				nameInfo.Init();
			}
			else
			{
				nameInfo = new NameInfo();
			}
			return nameInfo;
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x000F9204 File Offset: 0x000F8204
		private bool CheckTypeFormat(FormatterTypeStyle test, FormatterTypeStyle want)
		{
			return (test & want) == want;
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x000F920C File Offset: 0x000F820C
		private void PutNameInfo(NameInfo nameInfo)
		{
			this.niPool.Push(nameInfo);
		}

		// Token: 0x040024D4 RID: 9428
		private Queue m_objectQueue;

		// Token: 0x040024D5 RID: 9429
		private ObjectIDGenerator m_idGenerator;

		// Token: 0x040024D6 RID: 9430
		private int m_currentId;

		// Token: 0x040024D7 RID: 9431
		private ISurrogateSelector m_surrogates;

		// Token: 0x040024D8 RID: 9432
		private StreamingContext m_context;

		// Token: 0x040024D9 RID: 9433
		private __BinaryWriter serWriter;

		// Token: 0x040024DA RID: 9434
		private SerializationObjectManager m_objectManager;

		// Token: 0x040024DB RID: 9435
		private long topId;

		// Token: 0x040024DC RID: 9436
		private string topName;

		// Token: 0x040024DD RID: 9437
		private Header[] headers;

		// Token: 0x040024DE RID: 9438
		private InternalFE formatterEnums;

		// Token: 0x040024DF RID: 9439
		private SerObjectInfoInit serObjectInfoInit;

		// Token: 0x040024E0 RID: 9440
		private IFormatterConverter m_formatterConverter;

		// Token: 0x040024E1 RID: 9441
		internal object[] crossAppDomainArray;

		// Token: 0x040024E2 RID: 9442
		internal ArrayList internalCrossAppDomainArray;

		// Token: 0x040024E3 RID: 9443
		private object previousObj;

		// Token: 0x040024E4 RID: 9444
		private long previousId;

		// Token: 0x040024E5 RID: 9445
		private Type previousType;

		// Token: 0x040024E6 RID: 9446
		private InternalPrimitiveTypeE previousCode;

		// Token: 0x040024E7 RID: 9447
		private Hashtable assemblyToIdTable;

		// Token: 0x040024E8 RID: 9448
		private SerStack niPool = new SerStack("NameInfo Pool");
	}
}
