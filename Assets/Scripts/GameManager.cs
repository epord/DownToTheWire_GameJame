using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Piece selectedPiece { get; set; }
    public SuperPiece selectedSuperPiece { get; set; }

    public Piece blueCrystal;
    public Piece goldCrystal;
    public Piece greenCrystal;
    public Piece redCrystal;
    public Piece orb;

    public float totalLife = 100f;
    public float damageAmount = 5f;

    public AudioSource rewindStart;
    public AudioSource rewindLoop;
    public AudioSource rewindEnd;
    public AudioSource backgroundMusic;
    public AudioSource gameOver;

    public Text score;

    private HealthBar healthBar;
    private SoundManager sm;
    private int killCount;
    private bool isGameOver = false;

    private void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        sm = FindObjectOfType<SoundManager>();
    }

    public void IncreaseKillCount()
    {
        killCount++;
        score.text = killCount.ToString();
    }

    public void ReceiveDamage()
    {
        totalLife -= damageAmount;
        healthBar.SetHealth(totalLife);

        if (totalLife <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        isGameOver = true;
        rewindLoop.Stop();
        backgroundMusic.Stop();
        gameOver.Play();
        Time.timeScale = 1;
        yield return new WaitForSeconds(2);
        Time.timeScale = 1;
        PlayerPrefs.SetInt("score", killCount);
        SceneManager.LoadScene("End");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 30;
                if (!rewindLoop.isPlaying) rewindLoop.Play();
            } else
            {
                Time.timeScale = 1;
                rewindLoop.Stop();
            }
        }


        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                Piece piece = collider.GetComponent(typeof(Piece)) as Piece;
                GameGrid grid = collider.GetComponent(typeof(GameGrid)) as GameGrid;
                SuperPiece superPiece = collider.GetComponent(typeof(SuperPiece)) as SuperPiece;
                if (selectedPiece == null && piece != null)
                {
                    Debug.Log("Selected a piece");
                    GameObject newPiece = Instantiate(piece.prefab, new Vector3(mousePos2D.x, mousePos2D.y, 0), Quaternion.identity);
                    selectedPiece = newPiece.GetComponent(typeof(Piece)) as Piece;
                    selectedPiece.isSelected = true;
                }
                if (selectedPiece != null && grid != null)
                {
                    Debug.Log("Selected the grid with piece");
                    bool addedPiece = grid.AddPieceAt(selectedPiece, mousePos2D);
                    if (addedPiece)
                    {
                        sm.PlayReleasePiece();
                        selectedPiece.isSelected = false;
                        selectedPiece = null;
                    }
                }
                if (selectedSuperPiece == null && superPiece != null)
                {
                    Debug.Log("Selected a superPiece");
                    superPiece.spawner.PieceSelected();
                    superPiece.isSelected = true;
                    selectedSuperPiece = superPiece;
                    superPiece.spriteRenderer.sortingOrder = 5;
                    foreach (var pieceInSuperPiece in superPiece.Pieces())
                    {
                        foreach (var subPieceInSuperPiece in pieceInSuperPiece.subPieces)
                        {
                            subPieceInSuperPiece.spriteRenderer.sortingOrder = 6;
                        }
                    }
                }
                if (selectedSuperPiece != null && grid != null)
                {
                    Debug.Log("Selected the grid with superPiece");
                    bool addedPiece = grid.AddSuperPieceAt(selectedSuperPiece, mousePos2D);
                    if (addedPiece)
                    {
                        sm.PlayReleasePiece();
                        selectedSuperPiece.isSelected = false;
                        selectedSuperPiece = null;
                    }
                }
            }
        }

        // Right click
        if (selectedPiece != null && Input.GetMouseButtonDown(1))
        {
            selectedPiece.Rotate();
        }
        
        if (selectedSuperPiece != null && Input.GetMouseButtonDown(1))
        {
            selectedSuperPiece.Rotate();
        }
    }
}
