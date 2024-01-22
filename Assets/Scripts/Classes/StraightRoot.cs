/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer.Unity;
using VContainer;

public class StraightRoot : IReadOnlyPositionAdapter, IStartable, IRoot
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
    public StraightRoot(Transform transform, Field field)
    {
        this.transform = transform;
        this.field = field;

        if (this.transform.rotation == Quaternion.Euler((new Vector3(0f, 0f, 0f))))
        {
            movableDirections = new MoveType[] { MoveType.left, MoveType.right };
        }

        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0f, 0f, 90f)))
        {
            movableDirections = new MoveType[] { MoveType.up, MoveType.down };
        }

        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0f, 0f, 180f)))
        {
            movableDirections = new MoveType[] { MoveType.right, MoveType.right };
        }

        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0f, 0f, 270f)))
        {
            movableDirections = new MoveType[] { MoveType.down, MoveType.up };
        }

    }

    void IStartable.Start()
    {
        field.AddObject(this, this);
    }
}