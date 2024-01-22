/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;
using System.Collections.Generic;

public class AutoMoveLifeTimeScope : LifetimeScope {

    [SerializeField] AutoMovePresenter autoMovePresenter;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerSE playerSE;
    [SerializeField] AutoMoveView autoMoveView;

    protected override void Configure(IContainerBuilder builder)
    {
        //builder.R
        builder.Register<MoveDirectionLogic>(Lifetime.Singleton);
        builder.Register<IMovable,IMoveEventHandler, DefaultMoveLogic>(Lifetime.Singleton).
            WithParameter(this.transform).WithParameter(MoverType.roleModel);

        builder.RegisterComponent<AutoMovePresenter>(autoMovePresenter);
        builder.RegisterComponent<PlayerSE>(playerSE).WithParameter(audioSource);
        builder.RegisterComponent<AutoMoveView>(autoMoveView);
    }

    private void Start()
    {
        Container.Resolve<MoveDirectionLogic>();
    }
    private void OnDisable()
    {
        Destroy(this);
    }
}