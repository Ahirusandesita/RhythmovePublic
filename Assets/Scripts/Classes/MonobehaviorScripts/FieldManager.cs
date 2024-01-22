/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;

public class FieldManager : MonoBehaviour
{
    private GameObject obj;
    [Inject]
    public void Inject(IProgressManagement progressManagement)
    {
        progressManagement.OnNextStage += NextStage;
    }

    public void SetGameObject(GameObject obj)
    {
        this.obj = obj;
    }

    public void NextStage(object sender, NextStageEventArgs nextStageEventArgs)
    {
        Destroy(obj);
    }
}