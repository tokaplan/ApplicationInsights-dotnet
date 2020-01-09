using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildInfraTests
{
    [TestClass]
    public class PreprocessorSymbolTests
    {
        /// <summary>
        /// It is expected that these Preprocessors are set by the build system.
        /// https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        /// </summary>
        [TestMethod]
        public void VerifyFrameworksAreAvailable()
        {
            string name = null;

#if NET45
            name = "net45";
#elif NET451
            name = "net451";
#elif NET46
            name = "net46";
#elif NETCOREAPP2_0
            name = "netcoreapp2_0";
#else
#error Unknown Framework
#endif
            Assert.IsNotNull(name);
        }
    }
}
