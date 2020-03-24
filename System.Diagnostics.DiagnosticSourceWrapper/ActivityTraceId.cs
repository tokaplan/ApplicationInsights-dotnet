namespace System.Diagnostics
{
    public readonly partial struct ActivityTraceId : System.IEquatable<ActivityTraceId>
    {
        public void CopyTo(System.Span<byte> destination) { }
        public static ActivityTraceId CreateFromBytes(System.ReadOnlySpan<byte> idData) { throw null; }
        public static ActivityTraceId CreateFromString(System.ReadOnlySpan<char> idData) { throw null; }
        public static ActivityTraceId CreateFromUtf8String(System.ReadOnlySpan<byte> idData) { throw null; }
        public static ActivityTraceId CreateRandom() { throw null; }
        public bool Equals(ActivityTraceId traceId) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(ActivityTraceId traceId1, ActivityTraceId traceId2) { throw null; }
        public static bool operator !=(ActivityTraceId traceId1, ActivityTraceId traceId2) { throw null; }
        public string ToHexString() { throw null; }
        public override string ToString() { throw null; }
    }
}
