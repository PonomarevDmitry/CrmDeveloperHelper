using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class AssemblyReader : IDisposable
    {
        private const string _assemblyKey = "AssemblyReader_AssemblyPath";
        private const string _resultKey = "AssemblyReader_Result";

        private bool _disposed;
        private AppDomain _domain;

        public AssemblyReader()
        {
            _domain = CreateChildDomain();
        }

        private static AppDomain CreateChildDomain()
        {
            string path = typeof(PluginsAndWorkflowLoader).Assembly.Location;

            string directory = Path.GetDirectoryName(path);

            var setup = new AppDomainSetup
            {
                ApplicationBase = directory,
                CachePath = directory,
                LoaderOptimization = LoaderOptimization.MultiDomain,
            };

            AppDomain childDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence.Clone(), setup);

            return childDomain;
        }

        #region Private Destructors

        ~AssemblyReader()
        {
            Dispose(false);
        }

        #endregion Private Destructors

        #region Public Methods

        public AssemblyReaderResult ReadAssembly(string assemblyPath)
        {
            try
            {
                _domain.SetData(_assemblyKey, assemblyPath);
                _domain.DoCallBack(new CrossAppDomainDelegate(CheckAssembly));

                var check = _domain.GetData(_resultKey);

                if (check != null 
                    && check is byte[] arraySerialized
                    && arraySerialized.Length > 0
                )
                {
                    AssemblyReaderResult result = null;

                    DataContractSerializer ser = new DataContractSerializer(typeof(AssemblyReaderResult));

                    using (var memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(arraySerialized, 0, arraySerialized.Length);

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        result = ser.ReadObject(memoryStream) as AssemblyReaderResult;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }

            return null;
        }

        public static void CheckAssembly()
        {
            try
            {
                var path = (string)AppDomain.CurrentDomain.GetData(_assemblyKey);

                var loader = new PluginsAndWorkflowLoader();

                AssemblyReaderResult result = loader.LoadAssembly(path);

                byte[] fileBody = null;

                DataContractSerializer ser = new DataContractSerializer(typeof(AssemblyReaderResult));

                using (var memoryStream = new MemoryStream())
                {
                    ser.WriteObject(memoryStream, result);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    fileBody = memoryStream.ToArray();
                }

                AppDomain.CurrentDomain.SetData(_resultKey, fileBody);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToLog(ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (null != _domain)
            {
                try
                {
                    AppDomain.Unload(_domain);
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(null, ex);
                }
                _domain = null;
            }

            if (disposing)
            {
                _disposed = true;
            }
        }

        #endregion Private Methods
    }
}
