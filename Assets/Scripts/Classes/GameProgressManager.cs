/* 制作日
*　製作者
*　最終更新日
*/
using VContainer.Unity;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;

public class GameProgressManager : IProgressManagement, IProgressUse, IStartable, IDisposable
{

    public delegate void StartEventHandler(object sender, StartEventArgs e);
    public event StartEventHandler OnStart;

    public delegate void PauseEventHandler(object sender, PauseEventArgs e);
    public event PauseEventHandler OnPause;

    public delegate void FinishEventHandler(object sender, FinishEventArgs e);
    public event FinishEventHandler OnFinish;

    public delegate void NextStageEventHandler(object sender, NextStageEventArgs e);
    public event NextStageEventHandler OnNextStage;

    public delegate void StageClearEventHandler(object sender, FinishEventArgs e);
    public event StageClearEventHandler OnStageClear;

    public delegate void ResetEventHandler(object sender, FinishEventArgs e);
    public event ResetEventHandler OnReset;

    private CancellationTokenSource cts = new CancellationTokenSource();
    private CancellationToken token;


    private InGameOrderAsset inGameOrderAsset;
    private InGameTemplateAsset inGameTemplateAsset;

    private List<GameObject> gameObjects = new List<GameObject>();
    private int index;

    public GameProgressManager(InGameOrderAsset inGameOrderAsset, InGameTemplateAsset inGameTemplateAsset)
    {
        this.inGameOrderAsset = inGameOrderAsset;
        this.inGameTemplateAsset = inGameTemplateAsset;
    }

    /// <summary>
    /// InGame Start
    /// </summary>
    public void Start()
    {
        CancellationToken token = cts.Token;
        StartEventArgs startEventArgs = new StartEventArgs();
        OnStart?.Invoke(this, startEventArgs);
    }
    /// <summary>
    /// Game Pause
    /// </summary>
    public void Pause()
    {
        PauseEventArgs pauseEventArgs = new PauseEventArgs();
        OnPause?.Invoke(this, pauseEventArgs);
    }
    /// <summary>
    /// InGame Finish
    /// </summary>
    public void Finish()
    {
        FinishEventArgs finishEventArgs = new FinishEventArgs();
        OnFinish?.Invoke(this, finishEventArgs);
    }

    public async void NextStage()
    {
        OnStageClear?.Invoke(this, new FinishEventArgs());
        OnReset = null;
        await Task.Delay(4000, token);
        for (int i = 0; i < gameObjects.Count; i++)
        {
            MonoBehaviour.Destroy(gameObjects[i]);
        }
        gameObjects.Clear();
        OnNextStage?.Invoke(this, new NextStageEventArgs());
        await Task.Delay(1000, token);

        index++;
        if (index >= inGameOrderAsset.inGameOrders.Count)
        {
            Finish();
            return;
        }

        if (cts.IsCancellationRequested)
        {
            return;
        }

        for (int i = 0; i < inGameTemplateAsset.inGameTemplates.Count; i++)
        {
            gameObjects.Add(MonoBehaviour.Instantiate(inGameTemplateAsset.inGameTemplates[i]));
        }
        gameObjects.Add(MonoBehaviour.Instantiate(inGameOrderAsset.inGameOrders[index]).gameObject);


        Start();
    }

    public async void Restart()
    {
        OnReset?.Invoke(this, new FinishEventArgs());
        OnReset = null;

        await Task.Delay(2000, token);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            MonoBehaviour.Destroy(gameObjects[i]);
        }
        gameObjects.Clear();
        OnNextStage?.Invoke(this, new NextStageEventArgs());
        await Task.Delay(1000, token);

        if (index >= inGameOrderAsset.inGameOrders.Count)
        {
            Finish();
            return;
        }

        if (cts.IsCancellationRequested)
        {
            return;
        }

        for (int i = 0; i < inGameTemplateAsset.inGameTemplates.Count; i++)
        {
            gameObjects.Add(MonoBehaviour.Instantiate(inGameTemplateAsset.inGameTemplates[i]));
        }
        gameObjects.Add(MonoBehaviour.Instantiate(inGameOrderAsset.inGameOrders[index]).gameObject);


        Start();
    }

    async void IStartable.Start()
    {
        await Task.Delay(1000, token);

        for (int i = 0; i < inGameTemplateAsset.inGameTemplates.Count; i++)
        {
            gameObjects.Add(MonoBehaviour.Instantiate(inGameTemplateAsset.inGameTemplates[i]));
        }
        gameObjects.Add(MonoBehaviour.Instantiate(inGameOrderAsset.inGameOrders[index]).gameObject);

        Start();
    }

    public void Dispose()
    {
        cts.Cancel();
        cts.Dispose();
    }
}