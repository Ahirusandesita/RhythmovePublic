/* 制作日 2023/11/28
*　製作者
*　最終更新日 2023/11/28
*/

using UnityEngine;
using VContainer;
using System;

public class GoalEventArgs : EventArgs
{

}
public delegate void GoalEventHanlder(object sender, GoalEventArgs goalEventArgs);


/// <summary>
/// Rhythmoveのオブジェクトの移動ロジック
/// </summary>
public class MoveLogic : IMovable, IPositionAdapter, IReadOnlyPositionAdapter, IMoveEventHandler, IDisposable
{
    /// <summary>
    /// Moveのハンドラ
    /// </summary>
    /// <param name="sender">発行者</param>
    /// <param name="e">Event情報</param>
    public delegate void MoveEventHandler(object sender, MoveEventArgs e);

    /// <summary>
    /// 移動するときに発行する
    /// </summary>
    public event MoveEventHandler OnMove;

    public event GoalEventHanlder OnGoal;

    /// <summary>
    /// 移動する対象のTransform
    /// </summary>
    private Transform transform;

    /// <summary>
    /// 対象Field
    /// </summary>
    private Field field;

    private MoverType moverType;

    /// <summary>
    /// Positionを変更するプロパティ
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

    /// <summary>
    /// DI
    /// </summary>
    /// <param name="transform">対象のTransform</param>
    /// <param name="field">対象のField</param>
    [Inject]
    public MoveLogic(Transform transform, Field field, MoverType moverType)
    {
        this.transform = transform;
        this.field = field;
        this.field.AddObject((IMoveEventHandler)this);
        this.moverType = moverType;
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="moveType">移動方向</param>
    public void Move(MoveType moveType)
    {

        //イベント情報を生成　初期設定はCrashしていない状態
        MoveEventArgs moveEventArgs = new MoveEventArgs();
        moveEventArgs.moveType = moveType;
        moveEventArgs.isCrash = false;
        moveEventArgs.player = true;
        moveEventArgs.disposable = this;
        moveEventArgs.GoalCallback = OnGoal;
        moveEventArgs.positionAdapter = this;
        moveEventArgs.moverType = moverType;

        //移動方向に合わせて移動する　移動先が無ければCrash状態
        switch (moveType)
        {
            case MoveType.down:
                if (field.CanMove(MoveType.down, this))
                {
                    this.transform.position += new Vector3(0f, -1f, 0f);
                }
                else
                {
                    moveEventArgs.isCrash = true;
                }
                break;
            case MoveType.up:
                if (field.CanMove(MoveType.up, this))
                {
                    this.transform.position += new Vector3(0f, 1f, 0f);
                }
                else
                {
                    moveEventArgs.isCrash = true;
                }
                break;
            case MoveType.left:
                if (field.CanMove(MoveType.left, this))
                {
                    this.transform.position += new Vector3(-1f, 0f, 0f);
                }
                else
                {
                    moveEventArgs.isCrash = true;
                }
                break;
            case MoveType.right:
                if (field.CanMove(MoveType.right, this))
                {
                    this.transform.position += new Vector3(1f, 0f, 0f);
                }
                else
                {
                    moveEventArgs.isCrash = true;
                }
                break;
        }

        //移動Eventを発行
        OnMove?.Invoke(this, moveEventArgs);
        moveEventArgs = null;
    }

    public void Dispose()
    {
        transform = null;
    }
}