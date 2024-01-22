/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VContainer;

public class Field
{

    public delegate void MoveFieldEventHandler(object sender, MoveEventArgs e);
    public event MoveFieldEventHandler OnMoveField;


    private List<IReadOnlyPositionAdapter> objects = new List<IReadOnlyPositionAdapter>();
    private object lockObject = new object();

    private List<IReadOnlyPositionAdapter> goals = new List<IReadOnlyPositionAdapter>();
    private object lockGoal = new object();

    private List<IRoot> roots = new List<IRoot>();
    private object lockRoots = new object();

    private List<IDamageable> damageables = new List<IDamageable>();
    private object lockDamageable = new object();

    private IReadOnlyPositionAdapter positionAdapter;
    private IReadOnlyPositionAdapter demon;

    private IProgressUse progressUse;

    [Inject]
    public Field(IProgressUse progressUse)
    {
        this.progressUse = progressUse;
    }


    public void AddObject(IMoveEventHandler moveEventHandler)
    {
        lock (lockObject)
        {
            moveEventHandler.OnMove += IsGoal;
        }
    }
    public void AddObject(IReadOnlyPositionAdapter positionAdapter, IRoot root)
    {
        lock (lockRoots)
        {
            objects.Add(positionAdapter);
            roots.Add(root);
        }
    }
    public void AddGoal(IReadOnlyPositionAdapter positionAdapter)
    {
        lock (lockGoal)
        {
            goals.Add(positionAdapter);
        }
    }
    public void AddDamage(IDamageable damageable)
    {
        lock (lockDamageable)
        {
            damageables.Add(damageable);
            damageable.OnDamage += ALive;
        }
    }

    public bool CanMove(MoveType moveType, IReadOnlyPositionAdapter positionAdapter)
    {
        int x = default;
        int y = default;

        switch (moveType)
        {
            case MoveType.down:
                x = 0;
                y = -1;
                break;
            case MoveType.up:
                x = 0;
                y = 1;
                break;
            case MoveType.left:
                x = -1;
                y = 0;
                break;
            case MoveType.right:
                x = 1;
                y = 0;
                break;
        }

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] != positionAdapter)
            {
                if (objects[i].Position.x == positionAdapter.Position.x + x && objects[i].Position.y == positionAdapter.Position.y + y)
                {
                    for (int j = 0; j < roots[i].MovableDirections.Length; j++)
                    {
                        if (!MoveTypeOperation.CanMoveDirection(moveType, roots[i].MovableDirections[j]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        return false;
    }


    public void IsGoal(object sender, MoveEventArgs moveEventArgs)
    {
        if (!moveEventArgs.isCrash)
        {
            OnMoveField?.Invoke(this, moveEventArgs);
        }
        if (moveEventArgs.player)
        {
            this.positionAdapter = moveEventArgs.positionAdapter;
        }
        if (moveEventArgs.moverType == MoverType.demon)
        {
            this.demon = moveEventArgs.positionAdapter;
        }



        if (moveEventArgs.moverType == MoverType.demon)
        {
            //Debug if
            if (this.positionAdapter is not null)
            {
                if (moveEventArgs.positionAdapter.Position.x == this.positionAdapter.Position.x && moveEventArgs.positionAdapter.Position.y == this.positionAdapter.Position.y)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(1);//デバッグ
                    }
                }
            }
        }

        if (moveEventArgs.moverType == MoverType.player)
        {
            //Debug if
            if (!(demon is null))
            {
                if (moveEventArgs.positionAdapter.Position.x == this.demon.Position.x && moveEventArgs.positionAdapter.Position.y == this.demon.Position.y)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(1);//デバッグ
                    }
                }
            }
        }




        IReadOnlyPositionAdapter positionAdapter = (IReadOnlyPositionAdapter)sender;

        for (int i = 0; i < goals.Count; i++)
        {
            if (goals[i].Position.x == positionAdapter.Position.x && goals[i].Position.y == positionAdapter.Position.y)
            {
                if (moveEventArgs.player)
                {
                    progressUse.NextStage();
                }

                moveEventArgs.GoalCallback?.Invoke(this, new GoalEventArgs());
            }
        }
    }

    private void ALive(object sender, DamageEventArgs damageEventArgs)
    {
        if (!damageEventArgs.aLive)
        {
            progressUse.Restart();
        }
    }


    public void DebugDemon(IReadOnlyPositionAdapter readOnlyPositionAdapter)
    {
        demon = readOnlyPositionAdapter;
    }
}