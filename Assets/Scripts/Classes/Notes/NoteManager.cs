/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using VContainer;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class NoteManager : IDisposable
{
    /// <summary>
    /// Note発生ハンドラ
    /// </summary>
    /// <param name="sender">発行者</param>
    /// <param name="e">イベント情報</param>
    public delegate void NoteOccurrenceHandler(object sender, NoteEventArgs e);
    /// <summary>
    /// Note発生時に発行される
    /// </summary>
    public event NoteOccurrenceHandler OnNoteOccurrence;

    private CancellationTokenSource cts;

    private IProgressManagement progressManagement;

    private List<float> noteTimes = new List<float>();

    private List<float> noteEarlyTimes = new List<float>();

    private List<float> noteLateTimes = new List<float>();

    private string songFileName;

    /// <summary>
    /// DI
    /// </summary>
    /// <param name="progressManagement"></param>
    /// <param name="songFileName"></param>
    [Inject] //  <--なくてもいける？なんで？
    public NoteManager(IProgressManagement progressManagement, StageInformationAsset stageInformationAsset)
    {
        this.progressManagement = progressManagement;
        this.songFileName = stageInformationAsset.BGMRhytmInformation;
        Load();

        progressManagement.OnStart += Play;
    }

    /// <summary>
    /// Jsonをロードする
    /// </summary>
    /// <returns></returns>
    private NoteManager Load()
    {
        string inputString = Resources.Load<TextAsset>(songFileName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        int noteNum = inputJson.notes.Length;

        float lastDelayTime = 0f;
        float lastDelayEarlyTime = 0f;
        float lastDelayLateTime = 0f;

        for (int i = 0; i < noteNum; i++)
        {

            float interval = 60 / (inputJson.BPM * (float)inputJson.notes[i].LPB);
            float beatSecond = interval * (float)inputJson.notes[i].LPB;
            float time = (beatSecond * inputJson.notes[i].num / (float)inputJson.notes[i].LPB) + inputJson.offset / 44100f;

            if (inputJson.notes[i].block == 0)
            {
                float workTime = time;
                time -= lastDelayTime;
                lastDelayTime = workTime;
                noteTimes.Add(time);
            }
            else if(inputJson.notes[i].block == 1)
            {
                float workTime = time;
                time -= lastDelayEarlyTime;
                lastDelayEarlyTime = workTime;
                noteEarlyTimes.Add(time);
            }
            else if(inputJson.notes[i].block == 2)
            {
                float workTime = time;
                time -= lastDelayLateTime;
                lastDelayLateTime = workTime;
                noteLateTimes.Add(time);
            }

        }

        return this;
    }

    /// <summary>
    /// Note再生
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="startEventArgs"></param>
    private void Play(object sender, StartEventArgs startEventArgs)
    {
        cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        NoteAsync(token).Forget();
        NoteEarlyAsync(token).Forget();
        NoteLateAsync(token).Forget();
    }

    /// <summary>
    /// Note
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask NoteAsync(CancellationToken token)
    {
        //NoteTime待ってイベントを発行する
        for (int i = 0; i < noteTimes.Count; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(noteTimes[i]));

            NoteEventArgs noteEventArgs = new NoteEventArgs();
            noteEventArgs.noteTiming = NoteTimingType.just;
            OnNoteOccurrence?.Invoke(this, noteEventArgs);
            noteEventArgs = null;

            if (token.IsCancellationRequested)
                return;
        }
    }

    private async UniTask NoteEarlyAsync(CancellationToken token)
    {
        for (int i = 0; i < noteEarlyTimes.Count; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(noteEarlyTimes[i]));

            NoteEventArgs noteEventArgs = new NoteEventArgs();
            noteEventArgs.noteTiming = NoteTimingType.early;
            OnNoteOccurrence?.Invoke(this, noteEventArgs);
            noteEventArgs = null;

            if (token.IsCancellationRequested)
                return;
        }
    }
    private async UniTask NoteLateAsync(CancellationToken token)
    {
        for (int i = 0; i < noteLateTimes.Count; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(noteLateTimes[i]));

            NoteEventArgs noteEventArgs = new NoteEventArgs();
            noteEventArgs.noteTiming = NoteTimingType.late;
            OnNoteOccurrence?.Invoke(this, noteEventArgs);
            noteEventArgs = null;

            if (token.IsCancellationRequested)
                return;
        }
    }



    public void Dispose()
    {
        OnNoteOccurrence = null;
        cts.Cancel();
    }
}