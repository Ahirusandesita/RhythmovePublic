/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InGameTemplateAsset", menuName = "ScriptableObjects/CreateInGameTemplateAssetAsset")]
public class InGameTemplateAsset : ScriptableObject {
    public List<GameObject> inGameTemplates = new List<GameObject>();
}