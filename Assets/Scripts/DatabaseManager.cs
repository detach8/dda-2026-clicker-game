using System;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
    public string DisplayName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private bool IsAuthenticated() =>
        FirebaseAuth.DefaultInstance.CurrentUser.UserId != "" &&
        FirebaseAuth.DefaultInstance.CurrentUser.UserId != null;

    private string CurrentUserId() =>
        FirebaseAuth.DefaultInstance.CurrentUser.UserId;

    public void SetDisplayName(string displayName, Action<string> onError, Action onSuccess)
    {
        if (!IsAuthenticated())
        {
            Debug.Log("Cannot set display name when user is not logged in!");
            return;
        }

        FirebaseDatabase
            .DefaultInstance
            .RootReference
            .Child("players")
            .Child(CurrentUserId())
            .Child("displayName")
            .SetValueAsync(displayName)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    if (task.Exception != null) Debug.Log(task.Exception);
                    onError(task.Exception.ToString());
                    return;
                }

                onSuccess();
            });
    }

    public void GetDisplayName()
    {
        if (!IsAuthenticated())
        {
            Debug.Log("Cannot set display name when user is not logged in!");
            return;
        }

        FirebaseDatabase
            .DefaultInstance
            .RootReference
            .Child("players")
            .Child(CurrentUserId())
            .Child("displayName")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    if (task.Exception != null) Debug.Log(task.Exception);
                    return;
                }

                DisplayName = (string)task.Result.Value;
            });
    }

    public void UpdateScore(float score)
    {
        if (!IsAuthenticated())
        {
            Debug.Log("Cannot set score when user is not logged in!");
            return;
        }

        FirebaseDatabase
            .DefaultInstance
            .RootReference
            .Child("players")
            .Child(CurrentUserId())
            .Child("score")
            .SetValueAsync(score);
    }
}
