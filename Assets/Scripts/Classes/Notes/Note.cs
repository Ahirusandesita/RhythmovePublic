/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
 
[System.Serializable]
public class Data
{
    public string name;
    public int maxBlock;
    public int BPM;
    public int offset;
    public Note[] notes;
}

[System.Serializable]
public struct Note
{
    public int type;
    public int num;
    public int block;
    public int LPB;
}