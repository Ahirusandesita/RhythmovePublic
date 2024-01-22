/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;

public class BGMManager : MonoBehaviour, IDisposable
{

    [SerializeField]
    private AudioSource audioSource;

    private AudioClip BGM;
    private IProgressManagement progressManagement;

    [Inject]
    public void Inject(IProgressManagement progressManagement, StageInformationAsset stageInformationAsset)
    {
        this.progressManagement = progressManagement;

        progressManagement.OnStart += Play;
        progressManagement.OnStageClear += Stop;
        progressManagement.OnReset += ResetHandler;
        BGM = Resources.Load<AudioClip>(stageInformationAsset.BGMRhytmInformation);
    }

    public void Play(object sender, StartEventArgs startEventArgs)
    {
        audioSource.PlayOneShot(BGM);
    }
    public void Stop(object sender, FinishEventArgs finishEventArgs)
    {
        audioSource.Stop();
    }

    public void Dispose()
    {
        progressManagement.OnStart -= Play;
        progressManagement.OnStageClear -= Stop;
    }

    private void Stop()
    {
        audioSource.Stop();
    }

    private void ResetHandler(object sender, FinishEventArgs finishEventArgs)
    {
        Stop();
        Dispose();
    }
}