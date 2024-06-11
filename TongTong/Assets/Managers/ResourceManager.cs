using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Security;
using System.Linq;
using TMPro;
using Cysharp.Threading.Tasks;

public class ResourceManager : ManagerBase
{
    public Dictionary<string, GameObject> dicPlayerPrefab = new Dictionary<string, GameObject>();
    public Dictionary<string, Sprite> dicUISprite = new Dictionary<string, Sprite>();

    public string playerPrefab_directory;
    public string enemyPrefab_directory;
    public string ingamePrefab_directory;
    public string chapterPrefab_directory;

    public Dictionary<string, CharacterInfo> dicCharacterInfo = new Dictionary<string, CharacterInfo>();
   
    // 추후 일괄 프로퍼티 필드로 통일 한 클래스 상속으로 변경한다. 일단은 각각 넣음.
    // 각각 넣으니까 에디터 만들 때 골치가 아프니 통일하려 하니 참고
    //public CharacterInfoAsset characterInfoAsset;
  
    public SpriteAtlas characterAtlas;


    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        LoadAllPlayerPrefab();
        LoadAllGameData();

        await base.InitManager();
    }

    public void LoadAllPlayerPrefab()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>(playerPrefab_directory);
        for(int i = 0; i < prefabs.Length; ++i)
        {
            dicPlayerPrefab.Add(prefabs[i].name.ToLower(), prefabs[i]);
        }
    }

    public void LoadAllGameData()
    {
       
    }

    public AudioClip LoadAudioClip(string path, string name)
    {
        return Resources.Load(path + name) as AudioClip;
    }

    public Sprite LoadSprite(string path, string name)
    {
        return Resources.Load(path + name) as Sprite;
    }

    public GameObject GetPlayerPrefab(string key)
    {
        if (dicPlayerPrefab.ContainsKey(key) == false)
            return null;
        
        return Instantiate(dicPlayerPrefab[key]);
    }
}