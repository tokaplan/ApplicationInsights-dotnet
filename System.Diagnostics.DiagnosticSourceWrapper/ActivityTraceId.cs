using System.Linq.Expressions;
using System.Reflection;

namespace System.Diagnostics
{
    public readonly partial struct ActivityTraceId : System.IEquatable<ActivityTraceId>
    {
        public void CopyTo(System.Span<byte> destination) { }
        public static ActivityTraceId CreateFromBytes(System.ReadOnlySpan<byte> idData) { throw null; }
        public static ActivityTraceId CreateFromString(System.ReadOnlySpan<char> idData)
        {
            //var method = Activity.asm.GetType("System.Diagnostics.ActivityTraceId").GetMethod("CreateFromString", new[] { typeof(ReadOnlySpan<char>) }/*, BindingFlags.Public | BindingFlags.Static*/);
            //var activityTraceIdToWrap = method.Invoke(null, new object[] { idData });
            //var activityTraceId = ActivityTraceId.WrapInstance(activityTraceIdToWrap);
            var @type = Activity.asm.GetType("System.Diagnostics.ActivityTraceId");
            var method = @type.GetMethod("CreateFromString", new[] { typeof(ReadOnlySpan<char>) }/*, BindingFlags.Public | BindingFlags.Static*/);

            ParameterExpression param = Expression.Parameter(typeof(ReadOnlySpan<char>));
            var callExpr = Expression.Call(method, param);
            var convertExpr = Expression.Convert(callExpr, typeof(object));
            //var lambda = Expression.Lambda<Func<ReadOnlySpan<char>, object>>(callExpr, param).Compile();
            var lambda = Expression.Lambda<MyFunc>(convertExpr, param).Compile(); // (ReadOnlySpan<char> span) => (object)CreateFromString(span)
            var activityTraceIdToWrap = lambda(idData);
            var activityTraceId = ActivityTraceId.WrapInstance(activityTraceIdToWrap);

            return activityTraceId;
        }

        public delegate object MyFunc(ReadOnlySpan<char> span);

        public static ActivityTraceId CreateFromUtf8String(System.ReadOnlySpan<byte> idData) { throw null; }
        public static ActivityTraceId CreateRandom()
        {
            var method = Activity.asm.GetType("System.Diagnostics.ActivityTraceId").GetMethod("CreateRandom", BindingFlags.Public | BindingFlags.Static);
            var activityTraceIdToWrap = method.Invoke(null, new object[] { });
            var activityTraceId = ActivityTraceId.WrapInstance(activityTraceIdToWrap);
            return activityTraceId;
        }
        public bool Equals(ActivityTraceId traceId) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(ActivityTraceId traceId1, ActivityTraceId traceId2) { throw null; }
        public static bool operator !=(ActivityTraceId traceId1, ActivityTraceId traceId2) { throw null; }
        public string ToHexString()
        {
            return (string)this.wrappedInstance.GetType().GetMethod("ToHexString", BindingFlags.Public | BindingFlags.Instance).Invoke(this.wrappedInstance, new object[] { });
        }
        public override string ToString()
        {
            return this.wrappedInstance.ToString();
        }

        internal static ActivityTraceId WrapInstance(object traceIdToWrap)
        {
            return new ActivityTraceId(traceIdToWrap);
        }

        internal readonly object wrappedInstance;

        private ActivityTraceId(object traceIdToWrap)
        {
            this.wrappedInstance = traceIdToWrap;
        }
    }
}
