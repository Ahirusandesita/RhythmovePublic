/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;
 
public class AutoMovePresenter : MonoBehaviour,IDisposable {

    private IMovable movable;
    private PlayerSE playerSE;

    private MoveDirectionLogic moveDirectionLogic;
    private AutoMoveView autoMoveView;
    private IProgressManagement progressManagement;

    [Inject]
    public void Inject(IProgressManagement progressManagement, IMovable movable,PlayerSE playerSE,MoveDirectionLogic moveDirectionLogic,AutoMoveView autoMoveView)
    {
        this.progressManagement = progressManagement;
        this.movable = movable;
        this.playerSE = playerSE;

        movable.OnMove += playerSE.MoveSE;

        this.moveDirectionLogic = moveDirectionLogic;
        this.autoMoveView = autoMoveView;

        autoMoveView.OnInput += moveDirectionLogic.Move;
        this.progressManagement.OnReset += ResetHandler;
    }

    public void Dispose()
    {
        this.movable.OnMove -= playerSE.MoveSE;
        this.autoMoveView.OnInput -= moveDirectionLogic.Move;
    }
    public void ResetHandler(object sender,FinishEventArgs finishEventArgs)
    {
        Dispose();
    }

}