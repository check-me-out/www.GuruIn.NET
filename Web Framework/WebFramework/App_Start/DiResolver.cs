using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace WebFramework.App_Start
{
    public class DiResolver : DiDependencyScope, IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel _kernel;

        public DiResolver(IKernel kernel) : base(kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new DiDependencyScope(_kernel.BeginBlock());
        }
    }
}