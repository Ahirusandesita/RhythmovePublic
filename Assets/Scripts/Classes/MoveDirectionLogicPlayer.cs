/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;

public class MoveDirectionLogicPlayer : MoveDirectionLogic
{


    [Inject]
    public MoveDirectionLogicPlayer(IMovable movable, IProgressManagement progressManagement, NoteManager noteManager, MoveOrderData moveOrderData) : base(movable, progressManagement, noteManager, moveOrderData)
    {
    }

    public override void Move(object sender, InputEventArgs inputEventArgs)
    {
        if (DebugMoveType == MoveType.nonMove && !clashEventArgs.nowClashTime)
        {
            return;
        }

        if (DebugMoveType == lastExecutionMoveType)
        {
            Clash();
            return;
        }

        if (inputEventArgs.non == 10)
        {
            moveProperty.Value = MoveType.nonMove;
        }

        movable.Move(DebugMoveType);
        lastExecutionMoveType = DebugMoveType;
    }

    protected override void MoveDirectionChange(object sender, NoteEventArgs noteEventArgs)
    {
        if (noteEventArgs.noteTiming == NoteTimingType.just)
        {
            return;
        }
        switch (noteEventArgs.noteTiming)
        {
            case NoteTimingType.early:
                MoveDirectionChange();
                DebugMoveType = lastMoveType;
                break;
            case NoteTimingType.late:
                DebugMoveType = MoveType.nonMove;
                NonMove();
                break;
        }
    }
}