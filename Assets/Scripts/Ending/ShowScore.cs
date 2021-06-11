using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("score", 10);
        GetComponent<Text>().text = "Final score: " + Mathf.CeilToInt(PlayerPrefs.GetFloat("score"));
    }
}
