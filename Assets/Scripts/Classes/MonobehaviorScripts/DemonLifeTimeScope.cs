/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;
 
public class DemonLifeTimeScope : LifetimeScope {

    [SerializeField] DemonPresenter demonMovePresenter;
    [SerializeField] DemonMoveView demonMoveView;

    protected override void Configure(IContainerBuilder builder)
    {
        //builder.R
        builder.Register<MoveDirectionLogic>(Lifetime.Singleton);
        builder.Register<IMovable, IMoveEventHandler, DefaultMoveLogic>(Lifetime.Singleton).
            WithParameter(this.transform).WithParameter(MoverType.demon);

        builder.RegisterComponent<DemonPresenter>(demonMovePresenter);
        builder.RegisterComponent<DemonMoveView>(demonMoveView);
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