using System;
using System.Collections.Generic;

namespace System.Diagnostics
{
    public class DiagnosticListener : DiagnosticSource, IObservable<KeyValuePair<string, object>>, IDisposable
    {
        public static IObservable<DiagnosticListener> AllListeners
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
            private set { throw new NotImplementedException(); }
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer, Predicate<string> isEnabled)
        {
            throw new NotImplementedException();
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer, Func<string, object, object, bool> isEnabled)
        {
            throw new NotImplementedException();
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer)
        {
            throw new NotImplementedException();
        }

        public DiagnosticListener(string name)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled()
        {
            throw new NotImplementedException();
        }

        public override bool IsEnabled(string name)
        {
            throw new NotImplementedException();
        }

        public override bool IsEnabled(string name, object arg1, object arg2 = null)
        {
            throw new NotImplementedException();
        }

        public override void Write(string name, object value)
        {
            throw new NotImplementedException();
        }

        public override void OnActivityImport(Activity activity, object payload)
        {
            throw new NotImplementedException();
        }

        public override void OnActivityExport(Activity activity, object payload)
        {
            throw new NotImplementedException();
        }

        public virtual IDisposable Subscribe(IObserver<KeyValuePair<string, object>> observer, Func<string, object, object, bool> isEnabled, Action<Activity, object> onActivityImport = null, Action<Activity, object> onActivityExport = null)
        {
            throw new NotImplementedException();
        }
    }
}
