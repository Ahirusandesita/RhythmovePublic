

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;

public class StraightRootLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<StraightRoot>(Lifetime.Singleton).WithParameter(this.transform).Build();
    }
}
