using System;
using System.Data.Entity;

namespace WebFramework.Persistence.Helpers
{
    public class CreateInitializer<T> : CreateDatabaseIfNotExists<T> where T : DbContext
    {
        private Action<T> _seedCode { get; set; }

        public CreateInitializer()
        {
            _seedCode = null;
        }

        public CreateInitializer(Action<T> seedCode)
        {
            _seedCode = seedCode;
        }

        protected override void Seed(T context)
        {
            if (_seedCode != null)
            {
                _seedCode(context);
            }

            base.Seed(context);
        }
    }
}
