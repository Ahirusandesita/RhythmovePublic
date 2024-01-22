/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using System;

public enum NoteTimingType
{
    early,
    just,
    late
}
 
public class NoteEventArgs : EventArgs
{
    public NoteTimingType noteTiming;
}