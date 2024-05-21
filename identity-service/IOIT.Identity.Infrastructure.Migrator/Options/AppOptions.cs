using System.Collections.Generic;

namespace IOIT.Identity.Infrastructure.Migrator.Options
{
    public class AppOptions
    {
        public IList<AdminOption> Admin { get; set; }
        public IList<string> Roles { get; set; }
    }
}
