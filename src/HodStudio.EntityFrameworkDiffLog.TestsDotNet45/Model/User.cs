using HodStudio.EntityFrameworkDiffLog.Model;

namespace HodStudio.EntityFrameworkDiffLog.TestsDotNet45.Model
{
    [LoggedEntity]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
