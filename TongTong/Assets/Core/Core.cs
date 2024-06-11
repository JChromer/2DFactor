using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class Core : MonoBehaviour
{
    public static Core instance = null;

    public List<ManagerBase> managerList = new List<ManagerBase>();

    private Dictionary<Type, ManagerBase> managerDic = new Dictionary<Type, ManagerBase>();

    [HideInInspector]
    public static ResourceManager RSS { get { return instance.Get<ResourceManager>(); } }

    [HideInInspector]
    public static SceneLoadingManager LOADING { get { return instance.Get<SceneLoadingManager>(); } }

    [HideInInspector]
    public static SoundManager SOUND { get { return instance.Get<SoundManager>(); } }

    [HideInInspector]
    public static StateManager STATE { get { return instance.Get<StateManager>(); } }

    [HideInInspector]
    public bool isCoreReady = false;

    public Vector2 baseScreenRaio = Vector2.zero;
    private float uiScreenRatio = 1.0f;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Init()
    {
        CoreInit().Forget();
    }

    public async UniTaskVoid CoreInit()
    {
        ScreenCheck();

        for (int i = 0; i < managerList.Count; ++i)
        {
            await managerList[i].ManagerInitProcessing();

            var type = managerList[i].GetType();
            managerDic.Add(type, managerList[i]);
        }

        if (!GameEngine.instance.isDev)
        {
            //if (GameEngine.instance.linkDataBase)
            //   await new WaitUntil(() => BM.bmInit);
        }

        isCoreReady = true;


        // 플레이어 데이터 읽는 것을 타이틀 씬으로 옮긴다.
        //yield return StartCoroutine(STATE.MakePlayerInfo());

        
        if (!GameEngine.instance.isDev)
        {
            await LOADING.SceneLoadingWithAsync("Loading");
        }

        // 추후 위치가 이동될 수 있음 ( 언어 세팅 )
        //Helper.ChangeLanguageSetting(EGameLanuage.Korean);
    }

    public T Get<T>() where T : ManagerBase
    {
        var type = typeof(T);

        return managerDic.ContainsKey(type) ? (T)managerDic[type] : null;
    }

    public void ReleaseManagers()
    {
        managerDic.Clear();
    }

    public void ScreenCheck()
    {
        float currentRatio = (float)Screen.width / (float)Screen.height;

        float baseRatio = baseScreenRaio.x / baseScreenRaio.y;

        if (currentRatio < baseRatio)
            uiScreenRatio = 0.0f;
        else
            uiScreenRatio = 1.0f;
    }

    public float GetUIScreenRatio()
    {
        return uiScreenRatio;
    }
}
