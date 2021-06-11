using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Direction movingDirection = Direction.RIGHT;
    public MagicColor color = MagicColor.BLACK;
    public float speed = 0.2f;

    public AudioSource monster1;
    public AudioSource monster2;
    public AudioSource monster3;

    private Vector3 movingVector;
    private GameManager gm;
    private SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        sm = FindObjectOfType<SoundManager>();

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

            int soundId = Random.Range(1, 4);
            if (soundId == 1) sm.PlayMonster1();
            else if (soundId == 2) sm.PlayMonster2();
            else sm.PlayMonster3();
        }
    }

    public void Kill()
    {
        // TODO: show a basic animation of hit
        Destroy(gameObject);
    }
}
