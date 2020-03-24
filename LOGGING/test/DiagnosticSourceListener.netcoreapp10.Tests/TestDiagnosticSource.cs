//-----------------------------------------------------------------------
// <copyright file="TestDiagnosticSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener.Tests
{
#if UseDiagSrcAlias
    extern alias DiagSrcWrapper;
    using DiagSrcWrapper::System.Diagnostics;
#else
    using System.Diagnostics;
#endif

    internal class TestDiagnosticSource : DiagnosticListener
    {
        public const string ListenerName = nameof(TestDiagnosticSource);

        public TestDiagnosticSource(string listenerName = ListenerName) : base(listenerName)
        {
        }
    }
}
