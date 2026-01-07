using TMPro;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using Unity.VisualScripting;

public class LoginPanelManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_InputField emailField;
    [SerializeField]
    private TMP_InputField passwordField;
    [SerializeField]
    private TMP_Text errorText;

    public void Login()
    {
        // Obtain text from input fields
        var email = emailField.text;
        var password = passwordField.text;

        // Input validation
        if (email == "" || !email.Contains("@") || !email.Contains("."))
        {
            ShowError("Empty or invalid e-mail address");
            return;
        }
        else if (password.Length < 6)
        {
            ShowError("Password must be at least 6 characters long");
            return;
        }
        else
        {
            ShowError(""); // Clear error
        }

        FirebaseAuth
            .DefaultInstance
            .SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    if (task.Exception != null) Debug.Log(task.Exception);
                    ShowError("Error logging in");
                    return;
                }

                UIManager.Instance.ShowGame();
            });
    }

    void ShowError(string error)
    {
        errorText.text = error;
    }

    private Coroutine titleAnimationCoroutine;

    void OnEnable()
    {
        // Animate the title colours when GameObject is active
        StartCoroutine(ColourAnimator.Animate(title));
    }

    void OnDisable()
    {
        // Stop animating the title colours when GameObject is disabled
        if (titleAnimationCoroutine != null) StopCoroutine(titleAnimationCoroutine);
    }
}