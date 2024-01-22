/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using System;
using VContainer;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class DemonMoveView : MonoBehaviour, IDisposable
{
    public delegate void AutoInputHandler(object sender, InputEventArgs e);
    public event AutoInputHandler OnInput;

    private MoveOrder_AutoOrder moveOrder_AutoOrder;
    private MoveDirectionLogic moveDirectionLogic;
    // private IMoveEventHandler moveEventHandler;
    private Field field;
    private DemonData demonData;

    private IDisposable disposable;

    private CancellationTokenSource cts = new CancellationTokenSource();
    private bool canMove = false;
    private int index = 0;

    [Inject]
    public void Inject(IProgressManagement progressManagement, MoveOrder_AutoOrder moveOrder_AutoOrder, MoveDirectionLogic moveDirectionLogic, IMoveEventHandler moveEventHandler, Field field, DemonData demonData)
    {
        this.moveOrder_AutoOrder = moveOrder_AutoOrder;
        this.moveDirectionLogic = moveDirectionLogic;
        // this.moveEventHandler = moveEventHandler;
        this.field = field;
        this.demonData = demonData;
        field.OnMoveField += MoveStart;

        progressManagement.OnReset += Reset;
    }

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    private async UniTask InputSubscribe()
    {
        await UniTask.WaitUntil(() => InputSystem.Instance is not null);
        //disposable = InputSystem.Instance.readOnlyMoveProperty.Skip(1).Subscribe(_ => this.gameObject.SetActive(false));
    }

    public void Reset(object sender, FinishEventArgs finishEventArgs)
    {
        //Dispose();
    }

    public void Dispose()
    {
        //disposable.Dispose();
        field.OnMoveField -= MoveStart;
        cts.Cancel();
        cts.Dispose();
    }

    private async void MoveStart(object sender, MoveEventArgs moveEventArgs)
    {
        if (!moveEventArgs.player)
        {
            return;
        }

        if (moveEventArgs.player)
        {
            if (moveEventArgs.isCrash)
            {
                return;
            }
            if (moveEventArgs.moveType == MoveType.nonMove)
            {
                return;
            }
        }

        field.OnMoveField -= MoveStart;

        InputEventArgs inputEventArgs = new InputEventArgs();
        inputEventArgs.non = 10;
        OnInput?.Invoke(this, inputEventArgs);

        CancellationToken token = cts.Token;
        canMove = false;

        //Debug
        this.GetComponent<SpriteRenderer>().enabled = true;
        await Task.Delay(2000, token);

        MoveDelay(token).Forget();

        this.moveDirectionLogic.readOnlyMovePropery.
           Where(moveType => this.moveOrder_AutoOrder[index, moveType]).Where(moveType => canMove).
           Subscribe(moveType =>
           {
               OnInput?.Invoke(this, new InputEventArgs());
               canMove = false;
               index++;
               index = this.moveOrder_AutoOrder.OutOfLength(index);
           }
           );

        //this.moveEventHandler.OnGoal += Reset;

        InputSubscribe().Forget();
    }

    private async UniTask MoveDelay(CancellationToken token)
    {

        while (true)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            await UniTask.Delay((TimeSpan.FromSeconds(demonData.demonMoveDelay)));
            canMove = true;
        }
    }
}