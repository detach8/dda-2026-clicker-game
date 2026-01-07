using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;

public class SignupPanelManager : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField emailField;
    [SerializeField]
    private TMP_InputField passwordField;
    [SerializeField]
    private TMP_InputField displayNameField;
    [SerializeField]
    private TMP_Text errorText;

    public void Signup()
    {
        // Obtain text from input fields
        var email = emailField.text;
        var password = passwordField.text;
        var displayName = displayNameField.text;

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
        else if (displayName.Length < 3)
        {
            ShowError("Display Name must be at least 3 characters");
            return;
        }
        else
        {
            ShowError(""); // Clear error
        }

        FirebaseAuth
            .DefaultInstance
            .CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ShowError("Error signing up");
                    if (task.Exception != null) Debug.Log(task.Exception);
                    return;
                }

                DatabaseManager.Instance.SetDisplayName(displayName, ShowError, UIManager.Instance.ShowLogin);
            });
    }

    void ShowError(string error)
    {
        errorText.text = error;
    }
}
