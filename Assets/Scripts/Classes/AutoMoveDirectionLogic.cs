/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;

public class AutoMoveDirectionLogic : MoveDirectionLogic
{

    [Inject]
    public AutoMoveDirectionLogic(IMovable movable, IProgressManagement progressManagement, NoteManager noteManager, MoveOrderData moveOrderData) : base(movable, progressManagement, noteManager, moveOrderData)
    {

    }
}