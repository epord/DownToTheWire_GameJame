using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private HealthBar healthBar;

    private void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
    }

    public void ReceiveDamage()
    {
        totalLife -= damageAmount;
        healthBar.SetHealth(totalLife);
    }

    // Update is called once per frame
    void Update()
    {
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
                }
                if (selectedSuperPiece != null && grid != null)
                {
                    Debug.Log("Selected the grid with superPiece");
                    bool addedPiece = grid.AddSuperPieceAt(selectedSuperPiece, mousePos2D);
                    if (addedPiece)
                    {
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
