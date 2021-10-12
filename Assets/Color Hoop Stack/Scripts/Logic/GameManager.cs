using Firebase.Extensions;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Performance Params")]
    public int framerate = 60;
    public float ratio = 1.5f;

    [Header("Options")]
    public bool SoundEnable = true;
    public bool VibrateEnable = true;

    [Header("Debug")]
    public TextMeshProUGUI statusRemoteText;
    public bool test = false;

    protected bool isFirebaseInitialized = false;

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        GoogleAdMobController.Instance.Init();
#if !UNITY_EDITOR
        Application.targetFrameRate = framerate;
        Screen.SetResolution((int)((float)Screen.width / ratio), (int)((float)Screen.height /ratio), true);
#endif
    }

    // Initialize remote config, and set the default values.
    void InitializeFirebase()
    {
        // [START set_defaults]
        System.Collections.Generic.Dictionary<string, object> defaults =
          new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("config_test_bool", false);

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
          .ContinueWithOnMainThread(task => {
            // [END set_defaults]
            Debug.Log("RemoteConfig configured and ready!");
              isFirebaseInitialized = true;

              Task.Factory.StartNew(() => FetchDataAsync());
          });

    }

    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
#if UNITY_EDITOR
    [Button]
#endif
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task obj)
    {
        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task => {
                    test = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("config_test_bool").BooleanValue;
                    statusRemoteText.text = "Successfull test = " + test.ToString();
                    Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                   info.FetchTime));
                });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
}
