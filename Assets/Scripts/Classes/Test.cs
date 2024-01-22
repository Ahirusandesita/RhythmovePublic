/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using VContainer.Unity;

public class Test : IReadOnlyPositionAdapter, IStartable
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

    [Inject]
    public Test(Transform transform, Field field)
    {
        this.transform = transform;
        this.field = field;
    }

    void IStartable.Start()
    {
        //field.AddObject(this);
    }
}