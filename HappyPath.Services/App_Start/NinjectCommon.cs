using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.App_Start
{
    public static class NinjectCommon
    {
        private static IKernel _kernel;

        public static IKernel Kernel()
        {
            if (_kernel != null)
            {
                return _kernel;
            }

            _kernel = new StandardKernel();

            RegisterServices(_kernel);

            return _kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x =>
            {
                x.From("HappyPath.Services") // Scans currently assembly
                 .SelectAllClasses() // Retrieve all non-abstract classes
                 .BindDefaultInterface(); // Binds the default interface to them;
            });
        }

        
    }
}
