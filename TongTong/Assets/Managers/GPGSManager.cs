using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GPGSManager : ManagerBase
{
    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        await base.InitManager();
    }
}
