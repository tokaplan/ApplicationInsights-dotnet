using System.Reflection;

namespace System.Diagnostics
{
    public readonly struct ActivitySpanId : IEquatable<ActivitySpanId>
    {
        internal ActivitySpanId(string hexString)
        {
            throw new NotImplementedException();
        }

        public /*unsafe*/ static ActivitySpanId CreateRandom()
        {
            var method = Activity.asm.GetType("System.Diagnostics.ActivitySpanId").GetMethod("CreateRandom", BindingFlags.Public | BindingFlags.Static);
            var activitySpanIdToWrap = method.Invoke(null, new object[] { });
            var activitySpanId = ActivitySpanId.WrapInstance(activitySpanIdToWrap);
            return activitySpanId;
        }

        public static ActivitySpanId CreateFromBytes(ReadOnlySpan<byte> idData)
        {
            throw new NotImplementedException();
        }

        public static ActivitySpanId CreateFromUtf8String(ReadOnlySpan<byte> idData)
        {
            throw new NotImplementedException();
        }

        public static ActivitySpanId CreateFromString(ReadOnlySpan<char> idData)
        {
            throw new NotImplementedException();
        }

        public string ToHexString()
        {
            return (string)this.wrappedInstance.GetType().GetMethod("ToHexString", BindingFlags.Public | BindingFlags.Instance).Invoke(this.wrappedInstance, new object[] { });
        }

        public override string ToString()
        {
            return this.wrappedInstance.ToString();
        }

        public static bool operator ==(ActivitySpanId spanId1, ActivitySpanId spandId2)
        {
            return Equals(spanId1, spandId2);
        }

        public static bool operator !=(ActivitySpanId spanId1, ActivitySpanId spandId2)
        {
            return !Equals(spanId1, spandId2);
        }

        public static bool Equals(ActivitySpanId spanId1, ActivitySpanId spanId2)
        {
            if (ReferenceEquals(spanId1, spanId2))
            {
                return true;
            }
            if (ReferenceEquals(spanId1, null) || ReferenceEquals(null, spanId2))
            {
                return false;
            }

            return (spanId1.wrappedInstance ?? defaultVal).Equals(spanId2.wrappedInstance ?? defaultVal);
        }

        public bool Equals(ActivitySpanId spanId)
        {
            return Equals(this, spanId);
        }

        private static object defaultVal = Activator.CreateInstance(Activity.asm.GetType("System.Diagnostics.ActivitySpanId"));

        public override bool Equals(object obj)
        {
            if (obj != null && obj is ActivitySpanId)
            {
                return false;
            }

            return Equals(this.wrappedInstance, (ActivitySpanId)obj);
        }

        public override int GetHashCode()
        {
            return this.wrappedInstance.GetHashCode();
        }

        public void CopyTo(Span<byte> destination)
        {
            throw new NotImplementedException();
        }

        internal readonly object wrappedInstance;
        private ActivitySpanId(object instanceToWrap)
        {
            this.wrappedInstance = instanceToWrap;
        }

        public static ActivitySpanId WrapInstance(object instanceToWrap)
        {
            return new ActivitySpanId(instanceToWrap);
        }
    }
}
