/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;

public class CurveRootLifeTimeScope : LifetimeScope {
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<CurveRoot>(Lifetime.Singleton).WithParameter(this.transform).Build();
    }
}