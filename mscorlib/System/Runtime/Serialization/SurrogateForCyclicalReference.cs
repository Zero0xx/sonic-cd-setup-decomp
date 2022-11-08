using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036C RID: 876
	internal sealed class SurrogateForCyclicalReference : ISerializationSurrogate
	{
		// Token: 0x06002281 RID: 8833 RVA: 0x00057132 File Offset: 0x00056132
		internal SurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
		{
			if (innerSurrogate == null)
			{
				throw new ArgumentNullException("innerSurrogate");
			}
			this.innerSurrogate = innerSurrogate;
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0005714F File Offset: 0x0005614F
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			this.innerSurrogate.GetObjectData(obj, info, context);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x0005715F File Offset: 0x0005615F
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return this.innerSurrogate.SetObjectData(obj, info, context, selector);
		}

		// Token: 0x04000E88 RID: 3720
		private ISerializationSurrogate innerSurrogate;
	}
}
