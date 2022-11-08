﻿using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200001C RID: 28
	internal class RegexLWCGCompiler : RegexCompiler
	{
		// Token: 0x06000131 RID: 305 RVA: 0x0000A6CF File Offset: 0x000096CF
		internal RegexLWCGCompiler()
		{
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000A6D8 File Offset: 0x000096D8
		internal RegexRunnerFactory FactoryInstanceFromCode(RegexCode code, RegexOptions options)
		{
			this._code = code;
			this._codes = code._codes;
			this._strings = code._strings;
			this._fcPrefix = code._fcPrefix;
			this._bmPrefix = code._bmPrefix;
			this._anchors = code._anchors;
			this._trackcount = code._trackcount;
			this._options = options;
			string str = Interlocked.Increment(ref RegexLWCGCompiler._regexCount).ToString(CultureInfo.InvariantCulture);
			DynamicMethod go = this.DefineDynamicMethod("Go" + str, null, typeof(CompiledRegexRunner));
			base.GenerateGo();
			DynamicMethod firstChar = this.DefineDynamicMethod("FindFirstChar" + str, typeof(bool), typeof(CompiledRegexRunner));
			base.GenerateFindFirstChar();
			DynamicMethod trackCount = this.DefineDynamicMethod("InitTrackCount" + str, null, typeof(CompiledRegexRunner));
			base.GenerateInitTrackCount();
			return new CompiledRegexRunnerFactory(go, firstChar, trackCount);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000A7D0 File Offset: 0x000097D0
		internal DynamicMethod DefineDynamicMethod(string methname, Type returntype, Type hostType)
		{
			MethodAttributes attributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
			CallingConventions callingConvention = CallingConventions.Standard;
			DynamicMethod dynamicMethod = new DynamicMethod(methname, attributes, callingConvention, returntype, RegexLWCGCompiler._paramTypes, hostType, false);
			this._ilg = dynamicMethod.GetILGenerator();
			return dynamicMethod;
		}

		// Token: 0x04000710 RID: 1808
		private static int _regexCount = 0;

		// Token: 0x04000711 RID: 1809
		private static Type[] _paramTypes = new Type[]
		{
			typeof(RegexRunner)
		};
	}
}
