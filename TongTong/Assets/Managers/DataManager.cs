using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : ManagerBase
{
    [SerializeField]
    public string stageInfoKeyForDev;

    public HashSet<string> idTables = new HashSet<string>();

    private bool startTimer = false;

    [HideInInspector]
    public bool isLevelUpEffect = false;

    private bool invalidUser = false;

    [HideInInspector]
    public bool isFirstInstall = false;

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
        invalidUser = false;
        startTimer = true;
    }

    public string GetNewID()
    {
        string guid = Guid.NewGuid().ToString();
        while (idTables.Contains(guid))
        {
            guid = Guid.NewGuid().ToString();
        }

        AddHashSet(guid);
        return guid;
    }

    public void MakeHashSet()
    {
        idTables.Clear();
    }

    public void AddHashSet(string newID)
    {
        idTables.Add(newID);
    }

    public void RemoveHashSet(string newID)
    {
        idTables.Remove(newID);
    }

    public void FirstRunSupport()
    {
       
    }
}
