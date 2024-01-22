/* 制作日 2023/11/28
*　製作者
*　最終更新日 2023/11/28
*/

using UnityEngine;
using VContainer;
using UniRx;
using System;
public class PlayerPresenter : MonoBehaviour, IDisposable
{
    //仲介する対象クラス
    private PlayerView playerView;
    private IMovable movable;
    private MoveDirectionLogic moveDirectionLogic;
    private PlayerSE playerSE;
    private State state;

    private IProgressManagement progressManagement;

    private IDisposable disposable;

    /// <summary>
    /// VContainer でPlayerLifeTimeScopeからInjectされる
    /// </summary>
    /// <param name="playerView"></param>
    /// <param name="movable"></param>
    /// <param name="moveDirectionLogic"></param>
    /// <param name="playerSE"></param>
    /// <param name="progressManagement"></param>
    [Inject]
    public void Inject(PlayerView playerView, IMovable movable, MoveDirectionLogic moveDirectionLogic, PlayerSE playerSE, IProgressManagement progressManagement, State state)
    {
        this.playerView = playerView;
        this.movable = movable;
        this.moveDirectionLogic = moveDirectionLogic;
        this.playerSE = playerSE;
        this.progressManagement = progressManagement;
        this.state = state;
    }

    private void Start()
    {
        //Moveの方向変化時の仲介
        disposable = moveDirectionLogic.readOnlyMovePropery.Skip(1).Subscribe(moveType =>
        {
            playerView.MoveDirectionSprite(moveType);
        }
        );

        //Clash時の仲介
        moveDirectionLogic.OnClash += playerView.Clash;

        //移動時の仲介
        movable.OnMove += playerSE.MoveSE;

        //ゲーム終了時の仲介
        progressManagement.OnStageClear += playerView.NextStage;

        playerView.OnInput += moveDirectionLogic.Move;

        progressManagement.OnReset += ResetHandler;
    }

    public void Dispose()
    {
        disposable.Dispose();
        moveDirectionLogic.OnClash -= playerView.Clash;
        movable.OnMove -= playerSE.MoveSE;
        progressManagement.OnStageClear -= playerView.NextStage;
        playerView.OnInput -= moveDirectionLogic.Move;
    }

    public void ResetHandler(object sender, FinishEventArgs finishEventArgs)
    {
        Dispose();
    }
}