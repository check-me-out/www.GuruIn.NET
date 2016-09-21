using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Http.Dependencies;

namespace WebFramework.App_Start
{
    public class DiDependencyScope : IDependencyScope
    {
        private IResolutionRoot _resolver;

        internal DiDependencyScope(IResolutionRoot resolver)
        {
            Contract.Assert(resolver != null);

            this._resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (this._resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }

            return this._resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this._resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }

            return this._resolver.GetAll(serviceType);
        }

        public void Dispose()
        {
            var disposable = this._resolver as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            this._resolver = null;
        }
    }
}