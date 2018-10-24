﻿namespace SIS.Framework.Api.Contracts
{
    using Services;

    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IDependencyContainer dependencyContainer);
    }
}
