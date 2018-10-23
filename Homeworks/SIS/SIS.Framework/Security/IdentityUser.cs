namespace SIS.Framework.Security
{
    using System;

    public class IdentityUser : IIdentityUser<string>
    {
        public IdentityUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}