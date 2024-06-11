using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

//브릿지 용으로 쓰인다.
public class StateManager : ManagerBase
{
    // LoadingScene
    [HideInInspector]
    public bool isStartGame = false;
    [HideInInspector]
    public bool isEndGame = false;

    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        await base.InitManager();
    }

    public override void AfterInitProcessing()
    {
        base.AfterInitProcessing();
    }
    public void LoadingProcessStart()
    {
        StartCoroutine(LoadingProcess());
    }

    public IEnumerator LoadingProcess()
    {
        yield return true;
    }
}
