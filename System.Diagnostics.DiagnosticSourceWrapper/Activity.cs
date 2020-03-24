using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Diagnostics
{
    [System.FlagsAttribute]
    public enum ActivityTraceFlags
    {
        None = 0,
        Recorded = 1,
    }

    public class Activity
    {
        public static void InitWrapper()
        {
            var asm = Assembly.LoadFrom(@"S:\src\github\packages\System.Diagnostics.DiagnosticSource.4.6.0\lib\net45\System.Diagnostics.DiagnosticSource.dll");
            var @class = asm.GetType("System.Diagnostics.Activity");
            PropertyInfo property = @class.GetProperty("DefaultIdFormat", BindingFlags.Public | BindingFlags.Static);
            get_DefaultIdFormatDelegate = CreateFuncDelegate<ActivityIdFormat>(property.GetGetMethod());
            set_DefaultIdFormatDelegate = CreateActionDelegate<ActivityIdFormat>(property.GetSetMethod());
        }

        static Action<T> CreateActionDelegate<T>(MethodInfo mi) => (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), mi);
        static Func<T> CreateFuncDelegate<T>(MethodInfo mi) => (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), mi);

        static Activity()
        {
            InitWrapper();
        }

        public static bool ForceDefaultIdFormat
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string OperationName
        {
            get { throw new NotImplementedException(); }
        }

        public Activity Parent
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan Duration
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime StartTimeUtc
        {
            get { throw new NotImplementedException(); }
        }

        public /*unsafe*/ string Id
        {
            get { throw new NotImplementedException(); }
        }

        public string ParentId
        {
            get { throw new NotImplementedException(); }
        }

        public string RootId
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<KeyValuePair<string, string>> Tags
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<KeyValuePair<string, string>> Baggage
        {
            get { throw new NotImplementedException(); }
        }

        public string TraceStateString
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ActivitySpanId SpanId
        {
            get { throw new NotImplementedException(); }
        }

        public ActivityTraceId TraceId
        {
            get { throw new NotImplementedException(); }
        }

        public bool Recorded => (ActivityTraceFlags & ActivityTraceFlags.Recorded) != 0;

        public ActivityTraceFlags ActivityTraceFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ActivitySpanId ParentSpanId
        {
            get { throw new NotImplementedException(); }
        }

        private static Func<ActivityIdFormat> get_DefaultIdFormatDelegate;
        private static Action<ActivityIdFormat> set_DefaultIdFormatDelegate;
        public static ActivityIdFormat DefaultIdFormat
        {
            get { return get_DefaultIdFormatDelegate(); }
            set { set_DefaultIdFormatDelegate(value); }
        }

        public ActivityIdFormat IdFormat
        {
            get { throw new NotImplementedException(); }
        }

        public static Activity Current
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string GetBaggageItem(string key)
        {
            throw new NotImplementedException();
        }

        public Activity(string operationName)
        {
            throw new NotImplementedException();
        }

        public Activity AddTag(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Activity AddBaggage(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Activity SetParentId(string parentId)
        {
            throw new NotImplementedException();
        }

        public Activity SetParentId(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags activityTraceFlags = ActivityTraceFlags.None)
        {
            throw new NotImplementedException();
        }

        public Activity SetStartTime(DateTime startTimeUtc)
        {
            throw new NotImplementedException();
        }

        public Activity SetEndTime(DateTime endTimeUtc)
        {
            throw new NotImplementedException();
        }

        public Activity Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public Activity SetIdFormat(ActivityIdFormat format)
        {
            throw new NotImplementedException();
        }
    }
}
