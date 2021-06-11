using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public CircleCollider2D shootingRadius;
    public Material laserMaterial;

    private Piece piece;
    private AudioSource laserSound;

    // Start is called before the first frame update
    void Start()
    {
        piece = transform.parent.gameObject.GetComponent<Piece>();
        laserSound = transform.parent.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>() as Enemy;
        MagicColor orbColor = piece.subPieces[0].magicColor;
        if (enemy != null && enemy.color == orbColor)
        {
            StartCoroutine(Shoot(enemy));
        }
    }

    IEnumerator Shoot(Enemy enemy)
    {
        Color laserColor = Color.black;
        switch (piece.subPieces[0].magicColor)
        {
            case MagicColor.RED:
                laserColor = new Color(0.95f, 0.23f, 0.23f);
                break;
            case MagicColor.BLUE:
                laserColor = new Color(0.17f, 0.17f, 0.94f);
                break;
            case MagicColor.GREEN:
                laserColor = new Color(0.29f, 0.65f, 0.20f);
                break;
            case MagicColor.GOLD:
                laserColor = new Color(0.89f, 0.9f, 0.27f);
                break;
        }

        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        List<Vector3> pos = new List<Vector3>();
        pos.Add(transform.position);
        pos.Add(enemy.transform.position);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.material = laserMaterial;
        line.startColor = laserColor;
        line.endColor = laserColor;
        line.SetPositions(pos.ToArray());
        line.useWorldSpace = true;

        enemy.Kill();
        laserSound.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(line);
    }
}
