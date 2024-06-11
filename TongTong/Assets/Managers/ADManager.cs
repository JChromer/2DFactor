using Cysharp.Threading.Tasks;
using System.Globalization;
using UnityEngine.Events;

public class ADManager : ManagerBase
{
    private bool isLoadRewardVideo;
    public bool isInitialized;

    public UnityEvent unityEvent;

    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        Init();

#if UNITY_EDITOR == false
        await UniTask.WaitUntil(() => isInitialized, cancellationToken: this.GetCancellationTokenOnDestroy());
#endif
        await base.InitManager();
    }


    public void Init()
    {
        AddInitCallback();
        LoadRewardVideo();
    }

    private void AddInitCallback()
    {
      
    }

    private void AddFirebaseLogEvent()
    {

    }

    void LoadRewardVideo()
    {
        
    }

    public void ShowRewardVideo(string placement, System.Action OnRewardAction)
    {
       
    }

   
}
