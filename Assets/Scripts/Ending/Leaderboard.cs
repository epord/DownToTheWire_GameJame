using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        this.gameObject.SetActive(false);
    }

    [System.Serializable]
    class Entry
    {
        public string name;
        public float score;
    }

    [System.Serializable]
    class Entries
    {
        public Entry[] leaderboard;
    }

    public IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Entries entries = JsonUtility.FromJson<Entries>(webRequest.downloadHandler.text);
                string leaderboardText = "";
                int position = 1;
                foreach (Entry entry in entries.leaderboard)
                {
                    leaderboardText += position + ". " + entry.name + ": " + entry.score + "\n";
                    position++;
                }
                text.text = leaderboardText;
            }
        }
    }
}
