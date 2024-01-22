/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;

public class AutoMoveLogic : MoveLogic
{

    [Inject]
    public AutoMoveLogic(Transform transform, Field field, MoverType moverType) : base(transform, field, moverType) { }
}