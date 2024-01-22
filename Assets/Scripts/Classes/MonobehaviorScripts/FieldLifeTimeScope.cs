/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using VContainer.Unity;
using VContainer;
using System;

public class FieldLifeTimeScope : LifetimeScope,IDisposable
{
    [SerializeField]
    private StageSE stageSE;

    [SerializeField]
    private StageAsset stageAsset;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Field>(Lifetime.Singleton);
        builder.Register<NoteManager>(Lifetime.Singleton);
        builder.RegisterComponent<StageSE>(stageSE);

        builder.Register<MoveOrder_AutoOrder>(Lifetime.Singleton).WithParameter(stageAsset.autoMoveAsset.autoMoveOrders);
        builder.RegisterInstance<StageInformationAsset>(stageAsset.StageInformationAsset);
        builder.RegisterInstance<MoveOrderData>(stageAsset.moveOrder);
        builder.RegisterInstance<DemonData>(stageAsset.demonData);
        builder.RegisterComponent<FieldManager>(stageAsset.fieldManager);
    }

    public void Start()
    {
        FieldManager fieldManager = Container.Instantiate<FieldManager>(stageAsset.fieldManager);
        fieldManager.SetGameObject(fieldManager.gameObject);
        
    }


}