using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : ManagerBase
{
    [HideInInspector]
    public bool isLoadingComplete = false;
    [HideInInspector]
    public bool isLoadingDataComplete = false;
    [HideInInspector]
    public bool isLoadingUIComplete = false;

    private string targetSceneName = "";

    private float currentProgress = 0.0f;
    private float targetProgress = 0.0f;

    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        await base.InitManager();
    }

    public async UniTask SceneLoadingWithAsync(string targetScene)
    {
        isLoadingDataComplete = false;
        isLoadingComplete = false;
        isLoadingUIComplete = false;

        AsyncOperation async = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);

        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        await SceneLoadingWithAdditive(targetScene);
    }

    public async UniTask SceneLoadingDirectAsync(string targetScene)
    {
        isLoadingDataComplete = false;
        isLoadingComplete = false;
        isLoadingUIComplete = false;

        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);

        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    public void SetTargetProgress(float target)
    {
        targetProgress = target;
    }

    private async UniTask SceneLoadingWithAdditive(string targetScene)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

        Core.STATE.LoadingProcessStart();

        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        await new WaitUntil(() => isLoadingComplete);

        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        if(Core.STATE.isStartGame)
        {
            await new WaitUntil(() => isLoadingDataComplete);
        }

        await new WaitUntil(() => isLoadingUIComplete);

        await SceneUnLoad("Loading");
    }

    public async UniTask SceneUnLoad(string sceneName)
    {
        AsyncOperation async = SceneManager.UnloadSceneAsync(sceneName);

        while (!async.isDone)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        await UniTask.Yield(PlayerLoopTiming.Update);
    }
}
