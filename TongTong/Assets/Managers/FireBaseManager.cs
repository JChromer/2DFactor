using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Linq;
//using Firebase;
//using Firebase.Firestore;
//using Firebase.Extensions;
//using Firebase.Functions;
//using Newtonsoft.Json;
//using Firebase.Analytics;
//using Firebase.Crashlytics;

public class FireBaseManager : ManagerBase
{
    //private FirebaseApp fireBaseApp = null;
    //private FirebaseFirestore db = null;
    public bool isActive { get; private set; }

    public Dictionary<string, int> serverData = new Dictionary<string, int>();

    [HideInInspector]
    public bool invalidUser = false;

    [HideInInspector]
    public bool isOutFocus = false;

    public override async UniTask ManagerInitProcessing()
    {
        await InitManager();

        await base.ManagerInitProcessing();
    }

    public override async UniTask InitManager()
    {
        //await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    var dependencyStatus = task.Result;
        //    if (dependencyStatus == DependencyStatus.Available)
        //    {
        //        // Create and hold a reference to your FirebaseApp,
        //        // where app is a Firebase.FirebaseApp property of your application class.
        //        fireBaseApp = FirebaseApp.DefaultInstance;
        //        db = FirebaseFirestore.DefaultInstance;
        //        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //        Crashlytics.ReportUncaughtExceptionsAsFatal = true;

        //        isActive = true;
        //    }
        //    else
        //    {
        //        Debug.LogError(System.String.Format(
        //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Firebase Unity SDK is not safe to use here.
        //    }
        //}, cancellationToken: this.GetCancellationTokenOnDestroy());
         
        await SendRequestAsync();

        await base.InitManager();
    }

    public override void AfterInitProcessing()
    {
        CheckInvalidUser();

        if (invalidUser)
        {
            Debug.Log("Is invalid user.");
            Application.Quit();
        }

        if (invalidUser)
        {
            Application.Quit();
        }

        //if (!Core.instance.CanInternetAccess())
        //{
        //    ToastMessage.MessageError("Shop_Warning_Offline_Title");
        //    Application.Quit();
        //    return;
        //}

        base.AfterInitProcessing();
    }

    public void CheckInvalidUser()
    {
        //invalidUser = false;
        //if (serverData["year"] != Core.TIME.GetDateTime().Year)
        //    invalidUser = true;
        //if (serverData["month"] + 1 != Core.TIME.GetDateTime().Month)
        //    invalidUser = true;
        //if (serverData["day"] != Core.TIME.GetDateTime().Day)
        //    invalidUser = true;
        //if (Mathf.Abs(serverData["minutes"] - (Core.TIME.GetDateTime().Hour * 60 + Core.TIME.GetDateTime().Minute)) > 10)
        //    invalidUser = true;

        if (invalidUser)
        {
            Debug.Log("Is invalid user.");
            Application.Quit();
        }

        isOutFocus = false;
    }
    //public void SaveUserData(UserData userData)
    //{
    //    if (isActive == false)
    //    {
    //        Debug.LogError("Firebase is not ready.");
    //        return;
    //    }

    //    FDUserData data = new FDUserData(userData);
        
    //    db.Collection("users").Document(userData.pid).SetAsync(data).ContinueWithOnMainThread(task => 
    //    {
    //        if (task.Exception == null)
    //        {
    //            Debug.Log("Saved");
    //        }
    //        else
    //        {
    //            Debug.LogError("Exception occured : " + task.Exception.ToString());
    //        }
    //    });
    //}

    //public async UniTask<UserData> LoadUserDataAsync(string pid)
    //{
    //    if (isActive == false)
    //    {
    //        GameLogger.LogError("Firebase is not ready.");
    //        throw new OperationCanceledException();
    //    }

    //    UserData result = null;
    //    DocumentReference docRef = db.Collection("users").Document(pid);
    //    await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //    {
    //        DocumentSnapshot snapshot = task.Result;
    //        if (snapshot.Exists) 
    //        {
    //            FDUserData data = snapshot.ConvertTo<FDUserData>();
    //            result = data.ConvertToGameData();
    //        }
    //        else 
    //        {
    //            Debug.LogError(String.Format("Document {0} does not exist!", snapshot.Id));
    //        }
    //    });

    //    return result;
    //}

    private async UniTask SendRequestAsync()
    {
        string url = "";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Cloud Function Result: " + www.downloadHandler.text);
                //serverData = JsonConvert.DeserializeObject<Dictionary<string, int>>(www.downloadHandler.text);
            }
        }
    }

    private async UniTask SendRequestAsyncRealTime()
    {
        string url = "";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Cloud Function Result: " + www.downloadHandler.text);
                //serverData = JsonConvert.DeserializeObject<Dictionary<string, int>>(www.downloadHandler.text);
                CheckInvalidUser();
            }
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        if (!Core.instance.isCoreReady)
            return;

        if(!focus)
        {
            isOutFocus = true;
            return;
        }

        //if (!Core.instance.CanInternetAccess())
        //{
        //    ToastMessage.MessageError("Shop_Warning_Offline_Title");
        //    Application.Quit();
        //}

        if (focus)
        {
            SendRequestAsyncRealTime().Forget();
        }
    }
}