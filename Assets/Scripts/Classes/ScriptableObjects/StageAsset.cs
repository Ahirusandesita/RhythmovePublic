/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "StageAsset", menuName = "ScriptableObjects/CreateStageAsset")]
public class StageAsset : ScriptableObject {
    public AutoMoveAsset autoMoveAsset;
    public MoveOrderData moveOrder;
    public FieldManager fieldManager;
    public StageInformationAsset StageInformationAsset;
    public DemonData demonData;
}