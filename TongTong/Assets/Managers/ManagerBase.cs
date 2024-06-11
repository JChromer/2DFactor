using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    protected bool isReady = false;

    public virtual async UniTask InitManager()
    {
        await UniTask.CompletedTask;
    }

    public virtual void ResetManager()
    {

    }

    public virtual async UniTask ManagerInitProcessing()
    {
        isReady = true;

        Debug.Log(gameObject.name + " Init Complete");

        await UniTask.CompletedTask;
    }

    public virtual void AfterInitProcessing()
    {
       
    }
}
