using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Scripts
{
    public class LoginScript : MonoBehaviour
    {
        //  public TextMeshProUGUI DetailsText;
    // Start is called before the first frame update
    void Start()
    {
        Signin();
        // StartCoroutine(ExampleCoroutine());

    }

    // IEnumerator ExampleCoroutine()
    // {
    //     //Print the time of when the function is first called.
    //     Debug.Log("Started Coroutine at timestamp : " + Time.time);

    //     //yield on a new YieldInstruction that waits for 5 seconds.
    //     yield return new WaitForSeconds(5);
    //     SceneManager.LoadScene("Game");

    //     //After we have waited 5 seconds print the time again.
    //     Debug.Log("Finished Coroutine at timestamp : " + Time.time);

    // }
    public void Signin(){
        // PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }

    // internal void ProcessAuthentication(SignInStatus status) {
    //   if (status == SignInStatus.Success) {
    //         string name = PlayGamesPlatform.Instance.GetUserDisplayName();
    //         string id = PlayGamesPlatform.Instance.GetUserId();
    //         string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

    //         Debug.Log($"Details : name{name} , id {id}");
    //         // DetailsText.text= "Success \n"+name;

    //   } else {
    //         Debug.Log($"Error {status}");
    //     // DetailsText.text= "Signin Failed";
    //     // Disable your integration with Play Games Services or show a login button
    //     // to ask users to sign-in. Clicking it should call
    //     // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
    //   }
    // }
}
}
