/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using System.Collections.Generic;
using VContainer;

public class BlockLifeTimeScope : LifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<Test>(Lifetime.Singleton).WithParameter(this.transform).Build();
    }
}