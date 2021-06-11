using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitScore : MonoBehaviour
{

    [SerializeField] public Button submitButton;
    [SerializeField] public InputField input;
    [SerializeField] public Leaderboard leaderboard;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitScore);
    }

    void OnSubmitScore()
    {
        StartCoroutine(Upload());
    }


    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", input.text);
        form.AddField("score", PlayerPrefs.GetInt("score"));

        using (UnityWebRequest www = UnityWebRequest.Post("https://infinite-headland-70010.herokuapp.com/leaderboard/wires", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Couldn't destroy gameobjects
                submitButton.gameObject.GetComponent<Transform>().position = new Vector2(10000000, 10000000);
                input.gameObject.GetComponent<Transform>().position = new Vector2(10000000, 10000000);

                // Fetch leaders
                StartCoroutine(leaderboard.GetRequest("https://infinite-headland-70010.herokuapp.com/leaderboard/wires?amount=10"));
                leaderboard.gameObject.SetActive(true);

                Debug.Log("Form upload complete!");
            }
        }
    }
}
