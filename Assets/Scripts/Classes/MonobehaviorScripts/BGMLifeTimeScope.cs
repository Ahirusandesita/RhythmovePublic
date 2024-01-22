/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;

public class BGMLifeTimeScope : LifetimeScope {

    [SerializeField]
    private BGMManager BGMManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<BGMManager>(BGMManager);
    }
}