/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using VContainer.Unity;

public class Goal : IReadOnlyPositionAdapter, IStartable, IRoot
{

    private Transform transform;
    private Field field;

    public MoveType[] MovableDirections => movableDirections;
    private MoveType[] movableDirections = new MoveType[0];

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    [Inject]
    public Goal(Transform transform, Field field)
    {
        this.transform = transform;
        this.field = field;
    }

    void IStartable.Start()
    {
        field.AddGoal(this);
    }
}