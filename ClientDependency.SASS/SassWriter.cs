﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ClientDependency.Core;
using ClientDependency.Core.CompositeFiles;
using ClientDependency.Core.CompositeFiles.Providers;
using ClientDependency.Core.Config;
using SassAndCoffee.Ruby.Sass;

namespace ClientDependency.SASS
{
    /// <summary>
    /// A file writer for dotLess
    /// </summary>
    public class SassWriter : IFileWriter
    {
        private static readonly ISassCompiler Compiler = new SassCompiler();

        public bool WriteToStream(BaseCompositeFileProcessingProvider provider, StreamWriter sw, FileInfo fi, ClientDependencyType type, string origUrl, HttpContextBase http)
        {
            try
            {
                //NOTE: We don't want this compressed since CDF will do that ourselves
                var output = Compiler.Compile(fi.FullName, false, new List<string>());

                DefaultFileWriter.WriteContentToStream(provider, sw, output, type, http, origUrl);

                return true;
            }
            catch (Exception ex)
            {
                ClientDependencySettings.Instance.Logger.Error(string.Format("Could not write file {0} contents to stream. EXCEPTION: {1}", fi.FullName, ex.Message), ex);
                return false;
            }
        }

        /// <summary>
        /// Get the Sass compiled string output from the file
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public string GetOutput(FileInfo fi)
        {
            return Compiler.Compile(fi.FullName, false, new List<string>());            
        }
    }
}
