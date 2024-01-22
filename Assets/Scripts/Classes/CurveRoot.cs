/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;

public interface IRoot
{
    public MoveType[] MovableDirections { get; }
}

public class CurveRoot : IReadOnlyPositionAdapter, IStartable, IRoot
{

    private Transform transform;
    private Field field;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    public MoveType[] MovableDirections => movableDirections;
    private MoveType[] movableDirections;

    [Inject]
    public CurveRoot(Transform transform, Field field)
    {
        this.transform = transform;
        this.field = field;

        if (this.transform.rotation == Quaternion.Euler((new Vector3(0f, 0f, 0f))))
        {
            movableDirections = new MoveType[] { MoveType.left, MoveType.up };
        }

        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0f, 0f, 90f)))
        {
            movableDirections = new MoveType[] { MoveType.left, MoveType.down };
        }

        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0f, 0f, 180f)))
        {
            movableDirections = new MoveType[] { MoveType.right, MoveType.down };
        }

        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0f, 0f, 270f)))
        {
            movableDirections = new MoveType[] { MoveType.right, MoveType.up };
        }

    }

    void IStartable.Start()
    {
        field.AddObject(this, this);
    }
}