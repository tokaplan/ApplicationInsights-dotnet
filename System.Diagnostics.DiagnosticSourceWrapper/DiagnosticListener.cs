using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Diagnostics
{
    public class ObserverAdapter : IObserver<object>
    {
        private IObserver<DiagnosticListener> observer;

        public ObserverAdapter(IObserver<DiagnosticListener> observer)
        {
            this.observer = observer;
        }

        public void OnCompleted()
        {
            this.observer.OnCompleted();
        }

        public void OnError(Exception error)
        {
            this.observer.OnError(error);
        }

        public void OnNext(object diagnosticListenerReflected)
        {
            var diagListener = DiagnosticListener.CreateWrapper(diagnosticListenerReflected);
            this.observer.OnNext(diagListener);
        }
    }

    public class ObservableAdapter : IObservable<DiagnosticListener>
    {
        // private IObservable<object> IObservableReflected;
        private object reflectedAllListeners;

        public ObservableAdapter(object reflectedAllListeners)
        {
            this.reflectedAllListeners = reflectedAllListeners;
        }

        public IDisposable Subscribe(IObserver<DiagnosticListener> observer)
        {
            // IObservableReflected.Subscribe
            var t = (IObservable<object>)this.reflectedAllListeners;
            var observerAdapter = new ObserverAdapter(observer);
            var subscription = t.Subscribe(observerAdapter);
            return subscription;
        }
    }

    public class DiagnosticListener : DiagnosticSource, IObservable<KeyValuePair<string, object>>, IDisposable
    {
        public static IObservable<DiagnosticListener> AllListeners
        {
            get
            {
                var prop = Activity.DiagnosticListenerReflectedType.GetProperty("AllListeners", BindingFlags.Public | BindingFlags.Static);
                var reflectedAllListeners = prop.GetValue(null);
                var adapter = new ObservableAdapter(reflectedAllListeners);
                return adapter;
            }
        }

        public string Name
        {
            get { return (string)this.wrappedInstance.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance).GetValue(this.wrappedInstance); }
            private set { this.wrappedInstance.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance).SetValue(this.wrappedInstance, value); }
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer, Predicate<string> isEnabled)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("Subscribe", new[] { observer.GetType(), isEnabled.GetType() });
            var res = (IDisposable)sub.Invoke(this.wrappedInstance, new object[] { observer, isEnabled });
            return res;
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer, Func<string, object, object, bool> isEnabled)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("Subscribe", new[] { observer.GetType(), isEnabled.GetType() });
            var res = (IDisposable)sub.Invoke(this.wrappedInstance, new object[] { observer, isEnabled });
            return res;
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("Subscribe", new[] { observer.GetType() });
            var res = (IDisposable)sub.Invoke(this.wrappedInstance, new object[] { observer });
            return res;
        }

        public DiagnosticListener(string name) : base(null)
        {
            this.wrappedInstance = Activator.CreateInstance(Activity.DiagnosticListenerReflectedType, new[] { name });
        }

        private DiagnosticListener(object wrappedInstance) : base (wrappedInstance)
        {
        }

        internal static DiagnosticListener CreateWrapper(object wrappedInstance)
        {
            return new DiagnosticListener(wrappedInstance);
        }

        public virtual void Dispose()
        {
            (this.wrappedInstance as IDisposable)?.Dispose();
        }

        public override string ToString()
        {
            return wrappedInstance.ToString();
        }

        public bool IsEnabled()
        {
            var sub = this.wrappedInstance.GetType().GetMethod("IsEnabled", new Type[] { });
            var res = (bool)sub.Invoke(this.wrappedInstance, new object[] { });
            return res;
        }

        public override bool IsEnabled(string name)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("IsEnabled", new[] { name.GetType() });
            var res = (bool)sub.Invoke(this.wrappedInstance, new object[] { name });
            return res;
        }

        public override bool IsEnabled(string name, object arg1, object arg2 = null)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("IsEnabled", new[] { typeof(string), typeof(object), typeof(object) });
            var res = (bool)sub.Invoke(this.wrappedInstance, new object[] { name, arg1, arg2 });
            return res;
        }

        public override void Write(string name, object value)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("Write", new[] { typeof(string), typeof(object) });
            sub.Invoke(this.wrappedInstance, new object[] { name, value });
        }

        public override void OnActivityImport(Activity activity, object payload)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("OnActivityImport", new[] { Activity.ActivityReflectedType, typeof(object) });
            var res = sub.Invoke(this.wrappedInstance, new object[]{activity.wrappedActivity, payload});
        }

        public override void OnActivityExport(Activity activity, object payload)
        {
            throw new NotImplementedException();
        }

        public class ReflectedActivity { }

        public virtual IDisposable Subscribe(
            IObserver<KeyValuePair<string, object>> observer,
            Func<string, object, object, bool> isEnabled,
            Action<Activity, object> onActivityImport = null,
            Action<Activity, object> onActivityExport = null)
        {
            var generic = typeof(Action<,>);
            // Action<ReflectedActivity, object>:
            var actionActivityObjectType = generic.MakeGenericType(new[] { Activity.ActivityReflectedType, typeof(object) });

            var sub = this.wrappedInstance.GetType().GetMethod(
                "Subscribe",
                new[] { typeof(IObserver<KeyValuePair<string, object>>), typeof(Func<string, object, object, bool>), actionActivityObjectType, actionActivityObjectType });

            Action<object, object> onActivityImportWrapper = (object reflectedActivity, object arg) =>
            {
                var activityWrapper = Activity.WrapActivity(reflectedActivity);
                onActivityImport(activityWrapper, arg);
            };
            var reflectedActivityParam = Expression.Parameter(Activity.ActivityReflectedType, "reflectedActivityParam");
            var argParam = Expression.Parameter(typeof(object));
            // onActivityImportWrapper.Invoke(Activity.ActivityReflectedType, object): 
            var invokeOnActivityImportExpression = Expression.Invoke(
                Expression.Constant(onActivityImportWrapper),
                reflectedActivityParam, argParam);
            var onReflectedActivityImport = Expression.Lambda(
                actionActivityObjectType, // return type: Action<ReflectedActivity, object>
                invokeOnActivityImportExpression, // onActivityImportWrapper.Invoke(Activity.ActivityReflectedType, object)
                reflectedActivityParam, argParam).Compile();

            // onActivityImportWrapper.Invoke(Activity.ActivityReflectedType, object): 
            Action<object, object> onActivityExportWrapper = (object reflectedActivity, object arg) =>
            {
                var activityWrapper = Activity.WrapActivity(reflectedActivity);
                onActivityExport(activityWrapper, arg);
            };
            var invokeOnActivityExportExpression = Expression.Invoke(
                Expression.Constant(onActivityExportWrapper),
                reflectedActivityParam, argParam);
            var onReflectedActivityExport = Expression.Lambda(
                actionActivityObjectType, // return type: Action<ReflectedActivity, object>
                invokeOnActivityExportExpression, // onActivityImportWrapper.Invoke(Activity.ActivityReflectedType, object)
                reflectedActivityParam, argParam).Compile();

            var res = (IDisposable)sub.Invoke(
                this.wrappedInstance,
                new object[] {observer, isEnabled, onReflectedActivityImport, onReflectedActivityExport});
            return res;
        }
    }
}
