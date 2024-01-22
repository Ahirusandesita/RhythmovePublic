/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
 
public interface IReadOnlyPositionAdapter {
    Vector3 Position { get; }
}

public interface IMoveEventHandler
{
    event MoveLogic.MoveEventHandler OnMove;
    event GoalEventHanlder OnGoal;
}