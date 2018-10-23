namespace SIS.Framework.Attributes.Actions
{
    using System;
    using System.Linq;
    using Security.Contracts;

    public class AuthorizeAttribute : Attribute
    {
        private readonly string role;

        public AuthorizeAttribute()
        {

        }

        public AuthorizeAttribute(string role)
        {
            this.role = role;
        }

        private bool IsIdentityPresent(IIdentity identity)
        {
            return identity != null;
        }

        private bool IsIdentityInRole(IIdentity identity)
        {
            if (!this.IsIdentityPresent(identity))
            {
                return false;
            }

            return identity.Roles.Any(r => r == this.role);
        }

        private bool IsAuthorized(IIdentity user)
        {
            if (this.role == null)
            {
                return this.IsIdentityPresent(user);
            }
            else
            {
                return this.IsIdentityInRole(user);
            }
        }
    }
}
