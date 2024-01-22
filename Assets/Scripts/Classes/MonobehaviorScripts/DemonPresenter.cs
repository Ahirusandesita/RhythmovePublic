/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;
 
public class DemonPresenter : MonoBehaviour,IDisposable {

    private IProgressManagement progressManagement;
    private MoveDirectionLogic moveDirectionLogic;
    private DemonMoveView demonMoveView;



    [Inject]
    public void Inject(IProgressManagement progressManagement, IMovable movable, MoveDirectionLogic moveDirectionLogic, DemonMoveView demonMoveView)
    {
        this.progressManagement = progressManagement;
        this.moveDirectionLogic = moveDirectionLogic;
        this.demonMoveView = demonMoveView;

        this.demonMoveView.OnInput += moveDirectionLogic.Move;
        this.progressManagement.OnReset += ResetHandler;
    }

    public void Dispose()
    {
        this.demonMoveView.OnInput -= moveDirectionLogic.Move;
    }

    public void ResetHandler(object sender,FinishEventArgs finishEventArgs)
    {
        Dispose();
    }


}