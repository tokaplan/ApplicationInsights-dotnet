namespace System.Diagnostics
{
    public abstract class DiagnosticSource
    {
        public abstract void Write(string name, object value);

        public abstract bool IsEnabled(string name);

        public virtual bool IsEnabled(string name, object arg1, object arg2 = null)
        {
            throw new NotImplementedException();
        }

        public Activity StartActivity(Activity activity, object args)
        {
            throw new NotImplementedException();
        }

        public void StopActivity(Activity activity, object args)
        {
            throw new NotImplementedException();
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