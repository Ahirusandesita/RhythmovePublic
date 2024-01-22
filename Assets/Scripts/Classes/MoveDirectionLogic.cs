/* 制作日 2023/11/28
*　製作者 野村侑平
*　最終更新日 2023/11/28
*/
using VContainer;
using UniRx;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClashEventArgs : EventArgs
{
    public bool nowClashTime;
}

[System.Serializable]
public struct MoveOrderData
{
    public MoveType MoveOrder_One;
    public MoveType MoveOrder_Two;
    public MoveType MoveOrder_Three;
    public MoveType MoveOrder_Four;

    public MoveOrderData(MoveType MoveOrder_One, MoveType MoveOrder_Two, MoveType MoveOrder_Three, MoveType MoveOrder_Four)
    {
        this.MoveOrder_One = MoveOrder_One;
        this.MoveOrder_Two = MoveOrder_Two;
        this.MoveOrder_Three = MoveOrder_Three;
        this.MoveOrder_Four = MoveOrder_Four;
    }
}

public class MoveOrder_AutoOrder
{
    private List<MoveType> moveOrder_AutoOrders = new List<MoveType>();

    public MoveOrder_AutoOrder(List<MoveType> moveOrder_AutoOrders)
    {
        this.moveOrder_AutoOrders = moveOrder_AutoOrders;
    }

    public bool this[int index, MoveType moveType]
    {
        get
        {
            if (index >= moveOrder_AutoOrders.Count)
            {
                index = 0;
            }
            if (moveType == moveOrder_AutoOrders[index])
            {
                index++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public int OutOfLength(int index)
    {
        if (index >= moveOrder_AutoOrders.Count)
        {
            return 0;
        }
        return index;
    }
}


/// <summary>
/// 移動方向のロジック
/// </summary>
public class MoveDirectionLogic : IDisposable
{
    /// <summary>
    /// 衝突ハンドラ
    /// </summary>
    /// <param name="sender">発行者</param>
    /// <param name="e">イベント情報</param>
    public delegate void ClashEventHandler(object sender, ClashEventArgs e);
    /// <summary>
    ///  衝突時に発行される
    /// </summary>
    public event ClashEventHandler OnClash;


    protected IMovable movable;
    private NoteManager noteManager;

    //最後の移動方向
    protected MoveType lastMoveType;
    //最後に入力された移動方向
    protected MoveType lastExecutionMoveType = MoveType.nonMove;
    /// <summary>
    /// 現在の移動方向
    /// </summary>
    protected ReactiveProperty<MoveType> moveProperty = new ReactiveProperty<MoveType>();
    public IReadOnlyReactiveProperty<MoveType> readOnlyMovePropery => moveProperty;

    protected ClashEventArgs clashEventArgs = new ClashEventArgs();

    protected MoveOrderData moveOrderData;
    protected MoveOrder_AutoOrder moveOrder_AutoOrder;

    protected IProgressManagement progressManagement;


    protected MoveType DebugMoveType = default;

    /// <summary>
    /// DI
    /// </summary>
    /// <param name="movable"></param>
    /// <param name="progressManagement"></param>
    /// <param name="noteManager"></param>
    [Inject]
    public MoveDirectionLogic(IMovable movable, IProgressManagement progressManagement, NoteManager noteManager, MoveOrderData moveOrderData)
    {
        this.movable = movable;

        this.movable.OnMove += ClashCheck;

        this.noteManager = noteManager;

        this.noteManager.OnNoteOccurrence += MoveDirectionChange;

        this.moveOrderData = moveOrderData;

        moveProperty.Value = MoveType.down;
        lastMoveType = moveProperty.Value;
        this.progressManagement = progressManagement;
        progressManagement.OnStageClear += Reset;

        clashEventArgs.nowClashTime = false;
    }

    public virtual void Move(object sender, InputEventArgs inputEventArgs)
    {

        //if (DebugMoveType == MoveType.nonMove && !clashEventArgs.nowClashTime)
        //{
        //    return;
        //}

        //if (DebugMoveType == lastExecutionMoveType)
        //{
        //    Clash();
        //    return;
        //}

        //if (inputEventArgs.non == 10)
        //{
        //    moveProperty.Value = MoveType.nonMove;
        //}

        //movable.Move(DebugMoveType);
        //lastExecutionMoveType = DebugMoveType;



        if (moveProperty.Value == MoveType.nonMove && !clashEventArgs.nowClashTime)
        {
            return;
        }

        if (moveProperty.Value == lastExecutionMoveType)
        {
            Clash();
            return;
        }

        if (inputEventArgs.non == 10)
        {
            moveProperty.Value = MoveType.nonMove;
        }

        movable.Move(moveProperty.Value);
        lastExecutionMoveType = moveProperty.Value;

    }



    private void Reset(object sender, FinishEventArgs finishEventArgs)
    {
        Stop();
    }

    public void Stop()
    {
        moveProperty.Dispose();
        this.noteManager.OnNoteOccurrence -= MoveDirectionChange;
        progressManagement.OnStageClear -= Reset;
    }

    protected void MoveDirectionChange()
    {
        if (clashEventArgs.nowClashTime)
        {
            moveProperty.Value = MoveType.nonMove;
            return;
        }


        if (lastMoveType == moveOrderData.MoveOrder_One)
        {
            moveProperty.Value = moveOrderData.MoveOrder_Two;
            lastMoveType = moveProperty.Value;
        }
        else if (lastMoveType == moveOrderData.MoveOrder_Two)
        {
            moveProperty.Value = moveOrderData.MoveOrder_Three;
            lastMoveType = moveProperty.Value;
        }
        else if (lastMoveType == moveOrderData.MoveOrder_Three)
        {
            moveProperty.Value = moveOrderData.MoveOrder_Four;
            lastMoveType = moveProperty.Value;
        }
        else if (lastMoveType == moveOrderData.MoveOrder_Four)
        {
            moveProperty.Value = moveOrderData.MoveOrder_One;
            lastMoveType = moveProperty.Value;
        }
        lastExecutionMoveType = MoveType.nonMove;
    }

    protected MoveType NextMoveType(MoveType moveType)
    {
        if (clashEventArgs.nowClashTime)
        {
            moveProperty.Value = MoveType.nonMove;
            return MoveType.nonMove;
        }


        if (moveType == moveOrderData.MoveOrder_One)
        {
            return moveOrderData.MoveOrder_Two;
        }
        else if (moveType == moveOrderData.MoveOrder_Two)
        {
            return moveOrderData.MoveOrder_Three;
        }
        else if (moveType == moveOrderData.MoveOrder_Three)
        {
            return moveOrderData.MoveOrder_Four;
        }
        else if (moveType == moveOrderData.MoveOrder_Four)
        {
            return moveOrderData.MoveOrder_One;
        }

        return MoveType.nonMove;
    }


    protected virtual async void MoveDirectionChange(object sender, NoteEventArgs noteEventArgs)
    {
        switch (noteEventArgs.noteTiming)
        {
            case NoteTimingType.just:
                MoveDirectionChange();
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
                NonMove();
                break;
        }

    }

    protected void NonMove()
    {
        moveProperty.Value = MoveType.nonMove;
    }

    public void Dispose()
    {
        Stop();
    }


    private void ClashCheck(object sender, MoveEventArgs moveEventArgs)
    {
        if (moveEventArgs.isCrash)
        {
            Clash();
        }
    }

    protected async void Clash()
    {

        if (clashEventArgs.nowClashTime)
        {
            return;
        }

        clashEventArgs.nowClashTime = true;
        OnClash?.Invoke(this, clashEventArgs);

        await Task.Delay(1000);
        clashEventArgs.nowClashTime = false;
        OnClash?.Invoke(this, clashEventArgs);
    }

}