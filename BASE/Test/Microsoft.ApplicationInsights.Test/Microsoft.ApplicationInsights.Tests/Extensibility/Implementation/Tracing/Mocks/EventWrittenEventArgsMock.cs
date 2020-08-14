using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    /// <summary>
    /// This class provides an instance of <see cref="EventWrittenEventArgs"/> for use in unit testing.
    /// We can't create a new derived class because the constructor is marked internal.
    /// A tool like Moq can create instances, but because the properties are not virtual, we can't stub any of the values.
    /// The only option is to use reflection to build an instance. This works because our tests don't use the object, we only read properties.
    /// </summary>
    /// <remarks>
    /// The source code for <see cref="EventWrittenEventArgs"/> is available on ReferenceSource and on GitHub.
    /// https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,9317b6858407017e,references
    /// </remarks>
    public static class EventWrittenEventArgsMock// : EventWrittenEventArgs
    {
        public static EventWrittenEventArgs GetInstance(EventLevel level, string message, ReadOnlyCollection<object> payload, string eventSourceName = null)
        {
            var instance = (EventWrittenEventArgs)FormatterServices.GetUninitializedObject(typeof(EventWrittenEventArgs));

            // EventId -1 is important for the internal implementation of EventWrittenEventArgs. See ReferenceSource for details.
            // https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,4781
            SetPropertyValue(instance, nameof(EventWrittenEventArgs.EventId), -1);

            // https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,4818
            SetPropertyValue(instance, nameof(EventWrittenEventArgs.Payload), payload);

            // https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,4959
            SetFieldValue(instance, "m_level", level);

            // https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,4912
            SetFieldValue(instance, "m_message", message);

            if (eventSourceName != null)
            {
                var eventSource = GetEventSourceInstance(eventSourceName);

                // https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,4853
                SetFieldValue(instance, "m_eventSource", eventSource);
            }

            return instance;
        }

        private static EventSource GetEventSourceInstance(string name)
        {
            var instance = (EventSource)FormatterServices.GetUninitializedObject(typeof(EventSource));

            // https://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/eventsource.cs,249
            SetFieldValue(instance, "m_name", name);

            return instance;
        }

        private static void SetPropertyValue(Object instance, string propertyName, object val)
        {
            PropertyInfo propInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            propInfo.SetValue(instance, val);
        }

        private static void SetFieldValue(Object instance, string propertyName, object val)
        {
            FieldInfo fieldInfo = instance.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            fieldInfo.SetValue(instance, val);
        }
    }
}
