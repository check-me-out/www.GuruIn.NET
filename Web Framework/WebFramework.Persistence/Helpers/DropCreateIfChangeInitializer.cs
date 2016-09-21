using System;
using System.Data.Entity;

namespace WebFramework.Persistence.Helpers
{
    public class DropCreateIfChangeInitializer<T> : DropCreateDatabaseIfModelChanges<T> where T : DbContext
    {
        private Action<T> _seedCode { get; set; }

        public DropCreateIfChangeInitializer()
        {
            _seedCode = null;
        }

        public DropCreateIfChangeInitializer(Action<T> seedCode)
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
