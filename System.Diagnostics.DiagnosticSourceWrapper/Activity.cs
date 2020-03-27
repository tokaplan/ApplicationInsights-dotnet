using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        internal static Assembly asm;
        internal static Type ActivityReflectedType;
        internal static Type ActivitySpanIdReflectedType;
        internal static Type ActivityTraceIdReflectedType;
        internal static Type DiagnosticSourceReflectedType;
        internal static Type DiagnosticListenerReflectedType;

        public static void InitWrapper()
        {
            try
            {
#if NETSTANDARD2_0
                asm = Assembly.LoadFrom(@"S:\src\github\packages\System.Diagnostics.DiagnosticSource.4.6.0\lib\netstandard1.3\System.Diagnostics.DiagnosticSource.dll");
                Assembly.LoadFrom(@"C:\Users\mikp\.nuget\packages\system.memory\4.5.2\lib\netstandard2.0\System.Memory.dll");
#else
            asm = Assembly.LoadFrom(@"S:\src\github\packages\System.Diagnostics.DiagnosticSource.4.6.0\lib\net45\System.Diagnostics.DiagnosticSource.dll");
            Assembly.LoadFrom(@"C:\Users\mikp\.nuget\packages\system.memory\4.5.2\lib\netstandard1.1\System.Memory.dll");
#endif
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Failed to InitWrapper with {e.ToString()}");
                throw;
            }
            ActivityReflectedType = asm.GetType("System.Diagnostics.Activity");
            ActivityTraceIdReflectedType = asm.GetType("System.Diagnostics.ActivityTraceId");
            ActivitySpanIdReflectedType = asm.GetType("System.Diagnostics.ActivitySpanId");
            DiagnosticSourceReflectedType = asm.GetType("System.Diagnostics.DiagnosticSource");
            DiagnosticListenerReflectedType = asm.GetType("System.Diagnostics.DiagnosticListener");
            var @class = ActivityReflectedType;
            PropertyInfo property = @class.GetProperty("DefaultIdFormat", BindingFlags.Public | BindingFlags.Static);
            get_DefaultIdFormatDelegate = CreateFuncDelegate<ActivityIdFormat>(property.GetGetMethod(), null);
            set_DefaultIdFormatDelegate = CreateActionDelegate<ActivityIdFormat>(property.GetSetMethod(), null);

            
            property = @class.GetProperty("ForceDefaultIdFormat", BindingFlags.Public | BindingFlags.Static);
            get_ForceDefaultIdFormatDelegate = CreateFuncDelegate<bool>(property.GetGetMethod(), null);
            set_ForceDefaultIdFormatDelegate = CreateActionDelegate<bool>(property.GetSetMethod(), null);
            
            //property = @class.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);
            //get_CurrentDelegate = () => {
            //    Expression.Lambda<Func<object>>(
            //        Expression.TypeAs(
            //            Expression.Call(
            //                )))
            //    return null;
            //});
            //get_CurrentDelegate = CreateFuncDelegate<Activity>(property.GetGetMethod(), null);
            //set_CurrentDelegate = CreateActionDelegate<Activity>(property.GetSetMethod(), null);
        }

        internal static Activity WrapActivity(object activityToWrap)
        {
            if (activityToWrap == null)
            {
                return null;
            }

            Activity wrapper;
            Activity cachedWrapperActivity;
            if (wrappedToWrapperActivityMap.TryGetValue(activityToWrap, out cachedWrapperActivity))
            {
                wrapper = cachedWrapperActivity;
            }
            else
            {
                wrapper = new Activity(activityToWrap);
                wrappedToWrapperActivityMap.Add(activityToWrap, wrapper);
            }

            return wrapper;
        }

        private Activity InitInstance()
        {
            PropertyInfo property;

            var @class = this.wrappedActivity.GetType();
            
            property = @class.GetProperty("OperationName", BindingFlags.Public | BindingFlags.Instance);
            get_OperationNameDelegate = CreateFuncDelegate<string>(property.GetGetMethod(), this.wrappedActivity);
            // set_OperationNameDelegate = CreateActionDelegate<string>(property.GetSetMethod());

            //property = @class.GetProperty("Parent", BindingFlags.Public | BindingFlags.Instance);
            //get_ParentDelegate = CreateFuncDelegate<Activity>(property.GetGetMethod(), this.wrappedActivity);
            // set_ParentDelegate = CreateActionDelegate<Activity>(property.GetSetMethod());

            property = @class.GetProperty("Duration", BindingFlags.Public | BindingFlags.Instance);
            get_DurationDelegate = CreateFuncDelegate<TimeSpan>(property.GetGetMethod(), this.wrappedActivity);
            // set_DurationDelegate = CreateActionDelegate<TimeSpan>(property.GetSetMethod());

            property = @class.GetProperty("StartTimeUtc", BindingFlags.Public | BindingFlags.Instance);
            get_StartTimeUtcDelegate = CreateFuncDelegate<DateTime>(property.GetGetMethod(), this.wrappedActivity);
            // set_StartTimeUtcDelegate = CreateActionDelegate<DateTime>(property.GetSetMethod());

            property = @class.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            get_IdDelegate = CreateFuncDelegate<string>(property.GetGetMethod(), this.wrappedActivity);
            // set_IdDelegate = CreateActionDelegate<string>(property.GetSetMethod());

            property = @class.GetProperty("ParentId", BindingFlags.Public | BindingFlags.Instance);
            get_ParentIdDelegate = CreateFuncDelegate<string>(property.GetGetMethod(), this.wrappedActivity);
            // set_ParentIdDelegate = CreateActionDelegate<string>(property.GetSetMethod());

            property = @class.GetProperty("RootId", BindingFlags.Public | BindingFlags.Instance);
            get_RootIdDelegate = CreateFuncDelegate<string>(property.GetGetMethod(), this.wrappedActivity);
            // set_RootIdDelegate = CreateActionDelegate<string>(property.GetSetMethod());

            property = @class.GetProperty("Tags", BindingFlags.Public | BindingFlags.Instance);
            get_TagsDelegate = CreateFuncDelegate<IEnumerable<KeyValuePair<string, string>>>(property.GetGetMethod(), this.wrappedActivity);
            // set_TagsDelegate = CreateActionDelegate<IEnumerable<KeyValuePair<string, string>>>(property.GetSetMethod());

            property = @class.GetProperty("Baggage", BindingFlags.Public | BindingFlags.Instance);
            get_BaggageDelegate = CreateFuncDelegate<IEnumerable<KeyValuePair<string, string>>>(property.GetGetMethod(), this.wrappedActivity);
            // set_BaggageDelegate = CreateActionDelegate<IEnumerable<KeyValuePair<string, string>>>(property.GetSetMethod());

            property = @class.GetProperty("TraceStateString", BindingFlags.Public | BindingFlags.Instance);
            get_TraceStateStringDelegate = CreateFuncDelegate<string>(property.GetGetMethod(), this.wrappedActivity);
            set_TraceStateStringDelegate = CreateActionDelegate<string>(property.GetSetMethod(), this.wrappedActivity);

            //property = @class.GetProperty("SpanId", BindingFlags.Public | BindingFlags.Instance);
            //get_SpanIdDelegate = CreateFuncDelegate<ActivitySpanId>(property.GetGetMethod(), this.wrappedActivity);
            // set_SpanIdDelegate = CreateActionDelegate<ActivitySpanId>(property.GetSetMethod());

            //property = @class.GetProperty("TraceId", BindingFlags.Public | BindingFlags.Instance);
            //get_TraceIdDelegate = CreateFuncDelegate<ActivityTraceId>(property.GetGetMethod(), this.wrappedActivity);
            // set_TraceIdDelegate = CreateActionDelegate<ActivityTraceId>(property.GetSetMethod());

            property = @class.GetProperty("Recorded", BindingFlags.Public | BindingFlags.Instance);
            get_RecordedDelegate = CreateFuncDelegate<bool>(property.GetGetMethod(), this.wrappedActivity);
            // set_RecordedDelegate = CreateActionDelegate<bool>(property.GetSetMethod());

            //property = @class.GetProperty("ActivityTraceFlags", BindingFlags.Public | BindingFlags.Instance);
            //get_ActivityTraceFlagsDelegate = CreateFuncDelegate<ActivityTraceFlags>(property.GetGetMethod(), this.wrappedActivity);
            // set_ActivityTraceFlagsDelegate = CreateActionDelegate<ActivityTraceFlags>(property.GetSetMethod());

            //property = @class.GetProperty("ParentSpanId", BindingFlags.Public | BindingFlags.Instance);
            //get_ParentSpanIdDelegate = CreateFuncDelegate<ActivitySpanId>(property.GetGetMethod(), this.wrappedActivity);
            // set_ParentSpanIdDelegate = CreateActionDelegate<ActivitySpanId>(property.GetSetMethod());

            //property = @class.GetProperty("DefaultIdFormat", BindingFlags.Public | BindingFlags.Instance);
            //get_DefaultIdFormatDelegate = CreateFuncDelegate<ActivityIdFormat>(property.GetGetMethod(), this.wrappedActivity);
            //set_DefaultIdFormatDelegate = CreateActionDelegate<ActivityIdFormat>(property.GetSetMethod(), this.wrappedActivity);

            property = @class.GetProperty("IdFormat", BindingFlags.Public | BindingFlags.Instance);
            get_IdFormatDelegate = CreateFuncDelegate<ActivityIdFormat>(property.GetGetMethod(), this.wrappedActivity);
            // set_IdFormatDelegate = CreateActionDelegate<ActivityIdFormat>(property.GetSetMethod());
            
            return this;
        }

        static Action<T> CreateActionDelegate<T>(MethodInfo mi, object instance) => (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instance, mi);
        static Func<T> CreateFuncDelegate<T>(MethodInfo mi, object instance) => (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), instance, mi);

        static Activity()
        {
            InitWrapper();
        }

        private static Func<bool> get_ForceDefaultIdFormatDelegate;
        private static Action<bool> set_ForceDefaultIdFormatDelegate;
        public static bool ForceDefaultIdFormat
        {
            get { return get_ForceDefaultIdFormatDelegate(); }
            set { set_ForceDefaultIdFormatDelegate(value); }
        }

        private Func<string> get_OperationNameDelegate;
        public string OperationName
        {
            get { return this.get_OperationNameDelegate(); }
        }

        // private Func<Activity> get_ParentDelegate;
        public Activity Parent
        {
            get
            {
                // return get_ParentDelegate();
                var prop = ActivityReflectedType.GetProperty("Parent");
                var parentWrappedActivity = prop.GetValue(this.wrappedActivity);
                var activity = Activity.WrapActivity(parentWrappedActivity);
                return activity;
            }
        }

        private Func<TimeSpan> get_DurationDelegate;
        public TimeSpan Duration
        {
            get { return this.get_DurationDelegate(); }
        }

        private Func<DateTime> get_StartTimeUtcDelegate;
        public DateTime StartTimeUtc
        {
            get { return this.get_StartTimeUtcDelegate(); }
        }

        private Func<string> get_IdDelegate;
        public /*unsafe*/ string Id
        {
            get
            {
                //var id = (string)this.wrappedActivity.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
                //    .GetValue(this.wrappedActivity);
                //return id;
                return this.get_IdDelegate();
            }
        }

        private Func<string> get_ParentIdDelegate;
        public string ParentId
        {
            get { return this.get_ParentIdDelegate(); }
        }

        private Func<string> get_RootIdDelegate;
        public string RootId
        {
            get { return this.get_RootIdDelegate(); }
        }

        private Func<IEnumerable<KeyValuePair<string, string>>> get_TagsDelegate;
        public IEnumerable<KeyValuePair<string, string>> Tags
        {
            get { return this.get_TagsDelegate(); }
        }

        private Func<IEnumerable<KeyValuePair<string, string>>> get_BaggageDelegate;
        public IEnumerable<KeyValuePair<string, string>> Baggage
        {
            get { return this.get_BaggageDelegate(); }
        }

        private Func<string> get_TraceStateStringDelegate;
        private Action<string> set_TraceStateStringDelegate;
        public string TraceStateString
        {
            get { return this.get_TraceStateStringDelegate(); }
            set { this.set_TraceStateStringDelegate(value); }
        }

        // private static Func<ActivitySpanId> get_SpanIdDelegate;
        public ActivitySpanId SpanId
        {
            get
            {
                var spanIdToWrap = this.wrappedActivity.GetType().GetProperty("SpanId", BindingFlags.Public | BindingFlags.Instance).GetValue(this.wrappedActivity);
                var spanId = ActivitySpanId.WrapInstance(spanIdToWrap);
                return spanId;
            }
        }

        //private static Func<ActivityTraceId> get_TraceIdDelegate;
        public ActivityTraceId TraceId
        {
            get
            {
                var traceIdToWrap = this.wrappedActivity.GetType().GetProperty("TraceId", BindingFlags.Public | BindingFlags.Instance).GetValue(this.wrappedActivity);
                var traceId = ActivityTraceId.WrapInstance(traceIdToWrap);
                return traceId;
            }
        }

        private Func<bool> get_RecordedDelegate;
        public bool Recorded
        {
            get { return this.get_RecordedDelegate(); }
        }

        //private Func<ActivityTraceFlags> get_ActivityTraceFlagsDelegate;
        //private Action<ActivityTraceFlags> set_ActivityTraceFlagsDelegate;
        public ActivityTraceFlags ActivityTraceFlags
        {
            get
            {
                return (ActivityTraceFlags)this.wrappedActivity.GetType().GetProperty("ActivityTraceFlags").GetValue(this.wrappedActivity);
            }
            set
            {
                this.wrappedActivity.GetType().GetProperty("ActivityTraceFlags").SetValue(this.wrappedActivity, (int)value);
            }
        }

        // private Func<ActivitySpanId> get_ParentSpanIdDelegate;
        public ActivitySpanId ParentSpanId
        {
            get
            {
                var prop = ActivityReflectedType.GetProperty("ParentSpanId", BindingFlags.Public | BindingFlags.Instance);
                var val = prop.GetValue(this.wrappedActivity);
                var convertedSpanId = ActivitySpanId.WrapInstance(val);
                return convertedSpanId;
            }
        }

        private static Func<ActivityIdFormat> get_DefaultIdFormatDelegate;
        private static Action<ActivityIdFormat> set_DefaultIdFormatDelegate;
        public static ActivityIdFormat DefaultIdFormat
        {
            get { return get_DefaultIdFormatDelegate(); }
            set { set_DefaultIdFormatDelegate(value); }
        }

        private Func<ActivityIdFormat> get_IdFormatDelegate;
        public ActivityIdFormat IdFormat
        {
            get { return this.get_IdFormatDelegate(); }
        }

        //private static Func<Activity> get_CurrentDelegate;
        //private static Action<Activity> set_CurrentDelegate;
        //private static Activity cachedCurrentActivity = null;
        private static ConditionalWeakTable<object, Activity> wrappedToWrapperActivityMap = new ConditionalWeakTable<object, Activity>();
        public static Activity Current
        {
            //get { return get_CurrentDelegate(); }
            //set { set_CurrentDelegate(value); }
            get
            {
                var prop = ActivityReflectedType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);
                var activityToWrap = prop.GetValue(null);
                return Activity.WrapActivity(activityToWrap);
            }
            set
            {
                var wrapper = value;
                var prop = ActivityReflectedType.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);
                prop.SetValue(null, wrapper?.wrappedActivity);
            }
        }

        public string GetBaggageItem(string key)
        {
            throw new NotImplementedException();
        }

        internal object wrappedActivity;

        public Activity(string operationName)
        {
            this.wrappedActivity = Activator.CreateInstance(ActivityReflectedType, new[] { operationName });
            InitInstance();
            wrappedToWrapperActivityMap.Add(this.wrappedActivity, this);
        }

        private Activity(object wrappedActivity)
        {
            this.wrappedActivity = wrappedActivity;
            InitInstance();
        }


        public Activity AddTag(string key, string value)
        {
            var method = ActivityReflectedType.GetMethod("AddTag", new[] { key.GetType(), value.GetType() });
            var res = method.Invoke(this.wrappedActivity, new object[] { key, value });
            Debug.Assert(ReferenceEquals(res, this.wrappedActivity));
            return this;
        }

        public Activity AddBaggage(string key, string value)
        {
            var method = ActivityReflectedType.GetMethod("AddBaggage", new[] { key.GetType(), value.GetType() });
            var res = method.Invoke(this.wrappedActivity, new object[] { key, value });
            Debug.Assert(ReferenceEquals(res, this.wrappedActivity));
            return this;
        }

        public Activity SetParentId(string parentId)
        {
            var method = ActivityReflectedType.GetMethod("SetParentId", new[] { parentId.GetType() });
            var res = method.Invoke(this.wrappedActivity, new object[] { parentId });
            Debug.Assert(ReferenceEquals(res, this.wrappedActivity));
            return this;
        }

        public Activity SetParentId(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags activityTraceFlags = ActivityTraceFlags.None)
        {
            var method = ActivityReflectedType.GetMethod("SetParentId", new[] { ActivityTraceIdReflectedType, ActivitySpanIdReflectedType, asm.GetType("System.Diagnostics.ActivityTraceFlags") });
//            var convertedVal = Enum.ToObject(asm.GetType("System.Diagnostics.ActivityTraceFlags"), (int)activityTraceFlags);
            var res = method.Invoke(this.wrappedActivity, new object[] { traceId.wrappedInstance, spanId.wrappedInstance, (int)activityTraceFlags });
            Debug.Assert(ReferenceEquals(res, this.wrappedActivity));
            return this;
        }

        public Activity SetStartTime(DateTime startTimeUtc)
        {
            var thisObj = ActivityReflectedType.GetMethod("SetStartTime", new[] { typeof(DateTime) }).Invoke(this.wrappedActivity, new object[] { startTimeUtc });
            System.Diagnostics.Debug.Assert(ReferenceEquals(thisObj, this.wrappedActivity));
            return this;
        }

        public Activity SetEndTime(DateTime endTimeUtc)
        {
            var thisObj = ActivityReflectedType.GetMethod("SetEndTime", new[] { typeof(DateTime) }).Invoke(this.wrappedActivity, new object[] { endTimeUtc });
            System.Diagnostics.Debug.Assert(ReferenceEquals(thisObj, this.wrappedActivity));
            return this;
        }

        public Activity Start()
        {
            // TODO: here is assumption that Start will "return this":
            var thisObj = ActivityReflectedType.GetMethod("Start").Invoke(this.wrappedActivity, new object[] { });
            System.Diagnostics.Debug.Assert(ReferenceEquals(thisObj, this.wrappedActivity));
            return this;
        }

        public void Stop()
        {
            ActivityReflectedType.GetMethod("Stop").Invoke(this.wrappedActivity, new object[] { });
        }

        public Activity SetIdFormat(ActivityIdFormat format)
        {
            throw new NotImplementedException();
        }

        // from aspnet telemetry correlation:
        public bool Extract(NameValueCollection requestHeaders)
        {
            var asm = Assembly.LoadFrom(@"S:\src\github\aiall\bin\Debug\Src\Web\Web.Net45.Tests\Microsoft.AspNet.TelemetryCorrelation.dll");
            var method = asm.GetType("Microsoft.AspNet.TelemetryCorrelation.ActivityExtensions").GetMethod("Extract", BindingFlags.Public | BindingFlags.Static);
            bool res = (bool)method.Invoke(null, new object[] { this.wrappedActivity, requestHeaders });
            return res;
        }
    }
}
