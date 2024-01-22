/* 制作日
*　製作者
*　最終更新日
*/

using System;
public enum MoverType
{
    player,
    roleModel,
    demon
}

public class MoveEventArgs : EventArgs
{
    public MoveType moveType;
    public bool isCrash;
    public bool isGoal;

    public bool player;
    public MoverType moverType;

    public IDisposable disposable;
    public GoalEventHanlder GoalCallback;
    public IReadOnlyPositionAdapter positionAdapter;
}

public class StartEventArgs : EventArgs
{

}

public class NextStageEventArgs : EventArgs
{

}

public class PauseEventArgs : EventArgs
{

}

public class FinishEventArgs : EventArgs
{

}