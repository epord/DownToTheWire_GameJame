using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background: MonoBehaviour
{
    public Transform start;
    public Transform end;
    public GameObject[] starsList;
    public float moveSpeed = 1;
    public float rotationSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject stars in starsList)
        {
            stars.transform.position = stars.transform.position + Time.deltaTime * moveSpeed * Vector3.right;
            if (stars.transform.position.x > end.position.x)
            {
                stars.transform.position = start.position;
            }

            stars.transform.Rotate(Time.deltaTime * rotationSpeed * Vector3.back);
        }
    }
}
