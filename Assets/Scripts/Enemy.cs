using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Direction movingDirection = Direction.RIGHT;
    public MagicColor color = MagicColor.BLACK;
    public float speed = 0.2f;
    
    private Vector3 movingVector;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

        switch (movingDirection)
        {
            case Direction.UP:
                movingVector = Vector3.up;
                break;
            case Direction.RIGHT:
                movingVector = Vector3.right;
                break;
            case Direction.LEFT:
                movingVector = Vector3.left;
                GetComponent<SpriteRenderer>().flipX = true;
                break;
            case Direction.DOWN:
                movingVector = Vector3.down;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Moving
        transform.position += movingVector * speed * Time.deltaTime;

        // Moving animation
        float rotationAngle = Mathf.Sin(Time.time * 10) * 0.07f;
        transform.Rotate(new Vector3(0, 0, rotationAngle));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameGrid grid = collider.gameObject.GetComponent<GameGrid>() as GameGrid;
        if (grid != null)
        {
            gm.ReceiveDamage();
            this.Kill();
        }
    }

    public void Kill()
    {
        // TODO: show a basic animation of hit
        Destroy(gameObject);
    }
}
