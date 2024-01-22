/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InGameOrderAsset", menuName = "ScriptableObjects/CreateInGameOrderAsset")]
public class InGameOrderAsset : ScriptableObject {
    public List<FieldLifeTimeScope> inGameOrders = new List<FieldLifeTimeScope>();
}