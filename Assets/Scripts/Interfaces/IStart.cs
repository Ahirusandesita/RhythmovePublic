/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
 
public interface IStart {
    void Play();
}

public static class IStartThis
{
    public static IStart IStartQuery(this IStart start)
    {
        return start;
    }
}