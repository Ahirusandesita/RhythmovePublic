/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections.Generic;
using VContainer.Unity;
using VContainer;

public class RootLifeTimeScope : LifetimeScope {

    [SerializeField]
    private InGameOrderAsset inGameOrderAsset;
    [SerializeField]
    private InGameTemplateAsset inGameTemplateAsset;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameProgressManager>(Lifetime.Singleton);
        builder.RegisterComponent<InGameOrderAsset>(inGameOrderAsset);
        builder.RegisterComponent<InGameTemplateAsset>(inGameTemplateAsset);
    }
}