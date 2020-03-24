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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(ActivitySpanId spanId1, ActivitySpanId spandId2)
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(ActivitySpanId spanId1, ActivitySpanId spandId2)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ActivitySpanId spanId)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Span<byte> destination)
        {
            throw new NotImplementedException();
        }
    }
}
