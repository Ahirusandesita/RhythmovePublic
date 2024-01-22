/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

public class AutoMoveView : MonoBehaviour,IDisposable {

    public delegate void AutoInputHandler(object sender, InputEventArgs e);
    public event AutoInputHandler OnInput;

    private MoveOrder_AutoOrder moveOrder_AutoOrder;
    private MoveDirectionLogic moveDirectionLogic;

    private IDisposable disposable;
    private int index = 0;

    [Inject]
    public void Inject(MoveOrder_AutoOrder moveOrder_AutoOrder,MoveDirectionLogic moveDirectionLogic,IMoveEventHandler moveEventHandler)
    {
        this.moveOrder_AutoOrder = moveOrder_AutoOrder;
        this.moveDirectionLogic = moveDirectionLogic;

        this.moveDirectionLogic.readOnlyMovePropery.
            Where(moveType => this.moveOrder_AutoOrder[index,moveType]).
            Subscribe(moveType =>
            {
                OnInput?.Invoke(this, new InputEventArgs());
                index++;
                index = this.moveOrder_AutoOrder.OutOfLength(index);
            }
            );

        moveEventHandler.OnGoal += Reset;

        InputSubscribe().Forget();
    }

    private async UniTask InputSubscribe()
    {
        await UniTask.WaitUntil(() => InputSystem.Instance is not null);
        disposable = InputSystem.Instance.readOnlyMoveProperty.Skip(1).Subscribe(_ => this.gameObject.SetActive(false));
    }

    public void Reset(object sender,GoalEventArgs goalEventArgs)
    {
        //moveOrder_AutoOrder.ClearIndex();
    }

    public void Dispose()
    {
        disposable.Dispose();
    }
}