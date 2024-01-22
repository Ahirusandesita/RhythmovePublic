/* 制作日
*　製作者
*　最終更新日
*/
using UnityEngine;
public enum MoveType
{
    up,
    down,
    left,
    right,
    nonMove
}

public static class MoveTypeOperation
{
    public static bool CanMoveDirection(MoveType moveType1,MoveType moveType2)
    {

        if(moveType1 == MoveType.down && moveType2 == MoveType.up)
        {
            return false;
        }

        if(moveType1 == MoveType.up && moveType2 == MoveType.down)
        {
            return false;
        }

        if(moveType1 == MoveType.left && moveType2 == MoveType.right)
        {
            return false;
        }

        if( moveType1 == MoveType.right && moveType2 == MoveType.left)
        {
            return false;
        }


        return true;
    }
}
