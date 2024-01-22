/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System;

public interface IProgressManagement {

    public event GameProgressManager.StartEventHandler OnStart;
    public event GameProgressManager.PauseEventHandler OnPause;
    public event GameProgressManager.FinishEventHandler OnFinish;
    public event GameProgressManager.NextStageEventHandler OnNextStage;
    public event GameProgressManager.StageClearEventHandler OnStageClear;
    public event GameProgressManager.ResetEventHandler OnReset;
}
public interface IProgressUse
{
    void Start();
    void Pause();
    void Finish();

    void NextStage();
    void Restart();
}