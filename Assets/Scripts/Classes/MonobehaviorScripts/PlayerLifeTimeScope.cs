/* 制作日 2023/11/28
*　製作者 野村　侑平
*　最終更新日 2023/11/28
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;

public class PlayerLifeTimeScope : LifetimeScope
{

    [SerializeField] PlayerView playerView;
    [SerializeField] PlayerPresenter playerPresenter;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerSE playerSE;

    protected override void Configure(IContainerBuilder builder)
    {
        //builder.R
        builder.Register<MoveDirectionLogic, MoveDirectionLogicPlayer>(Lifetime.Transient);
        builder.Register<IMovable, MoveLogic>(Lifetime.Singleton).
            WithParameter(playerView.gameObject.transform).WithParameter(MoverType.player);

        builder.Register<State>(Lifetime.Singleton).WithParameter(new StateData(1));

        builder.RegisterComponent<PlayerPresenter>(playerPresenter);
        builder.RegisterComponent<PlayerView>(playerView);
        builder.RegisterComponent<PlayerSE>(playerSE).WithParameter(audioSource);
    }
}