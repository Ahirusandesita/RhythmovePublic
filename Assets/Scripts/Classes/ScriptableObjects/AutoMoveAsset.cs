/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AutoMoveAsset", menuName = "ScriptableObjects/CreateAutoMoveAsset")]
public class AutoMoveAsset : ScriptableObject {
    public List<MoveType> autoMoveOrders = new List<MoveType>();
}