/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;
 
public class StageSE : MonoBehaviour,IDisposable {

    [SerializeField]
    private AudioClip stageClearSE;

    private AudioSource audioSource;

    private IProgressManagement progressManagement;

    [Inject]
    public void Inject(IProgressManagement progressManagement)
    {
        this.progressManagement = progressManagement;
    }

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        progressManagement.OnStageClear += StageClear;
    }

    public void StageClear(object sender,FinishEventArgs finishEventArgs)
    {
        audioSource.PlayOneShot(stageClearSE);
    }

    public void Dispose()
    {
        progressManagement.OnStageClear -= StageClear;
    }
}