namespace System.Diagnostics
{
    public abstract class DiagnosticSource
    {
        protected object wrappedInstance;
        protected DiagnosticSource(object wrappedInstance)
        {
            this.wrappedInstance = wrappedInstance;
        }

        public abstract void Write(string name, object value);

        public abstract bool IsEnabled(string name);

        public virtual bool IsEnabled(string name, object arg1, object arg2 = null)
        {
            throw new NotImplementedException();
        }

        public Activity StartActivity(Activity activity, object args)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("StartActivity", new[] { activity.wrappedActivity.GetType(), typeof(object) });
            var res = sub.Invoke(this.wrappedInstance, new object[] { activity.wrappedActivity, args });
            System.Diagnostics.Debug.Assert(ReferenceEquals(res, activity.wrappedActivity));
            return activity;
        }

        public void StopActivity(Activity activity, object args)
        {
            var sub = this.wrappedInstance.GetType().GetMethod("StopActivity", new[] { activity.wrappedActivity.GetType(), typeof(object) });
            sub.Invoke(this.wrappedInstance, new object[] { activity.wrappedActivity, args });
        }

        public virtual void OnActivityImport(Activity activity, object payload)
        {
            throw new NotImplementedException();
        }

        public virtual void OnActivityExport(Activity activity, object payload)
        {
            throw new NotImplementedException();
        }
    }

}