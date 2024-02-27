using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleSigninDemo : MonoBehaviour
    {
    public string webClientId = "683089056633-93v0k1nean8n41nkb0bafiaq3n8f59lu.apps.googleusercontent.com";
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    private void Awake()
    {   RudderStackHelper.Intialise();
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {  
                    auth = FirebaseAuth.DefaultInstance;
                    SignInWithGoogle();
                }

                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }
    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }
    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }
    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                    
	                Dictionary<string,object> eventProperties = new Dictionary<string, object>();
                    eventProperties.Add("ErrorStatus",error.Status );
                    eventProperties.Add("ErrorMessage",error.Message );
                    
                    RudderStackHelper.SendTrackEvent("SigninError",eventProperties);

                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
            RudderStackHelper.SendTrackEvent("SigninCanceled");

        }
        else{
            
            // Debug.Log($"Google Name :  {task.Result.DisplayName}  :  {task.Result.Email} : {task.Result.ImageUrl} : {task.Result.UserId} ");
            SignInWithGoogleOnFirebase(task.Result.IdToken);
            GlobalConstants.Instance = task.Result;
            RudderStackHelper.SendIdentityEvent();
            // Dictionary<string,object> screenprperties = new Dictionary<string, object>();
            // screenprperties.Add("email",GlobalConstants.Instance.Email);
           // RudderStackHelper.SendScreenEvent("Login Screen",screenprperties);

            SceneManager.LoadScene("Game");
        }


    }
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");
            }
        });
    }
    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");
        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }
    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;
        AddToInformation("Calling Games SignIn");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    private void AddToInformation(string str) { Debug.Log($"Console GoogleSign in  : {str}"); }
}
