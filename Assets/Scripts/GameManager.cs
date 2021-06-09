using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Piece selectedPiece { get; set; }

    public Piece blueCrystal;
    public Piece goldCrystal;
    public Piece greenCrystal;
    public Piece redCrystal;
    public Piece orb;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
                if (selectedPiece == null && piece != null)
                {
                    Debug.Log("Selected a piece");
                    GameObject newPiece = Instantiate(piece.prefab, new Vector3(mousePos2D.x, mousePos2D.y, 0), Quaternion.identity);
                    selectedPiece = newPiece.GetComponent(typeof(Piece)) as Piece;
                    selectedPiece.isSelected = true;
                }
                if (selectedPiece != null && grid != null)
                {
                    Debug.Log("Selected the grid");
                    bool addedPiece = grid.AddPieceAt(selectedPiece, mousePos2D);
                    if (addedPiece)
                    {
                        selectedPiece.isSelected = false;
                        selectedPiece = null;
                    }
                }
            }
        }

        // Right click
        if (selectedPiece != null && Input.GetMouseButtonDown(1))
        {
            selectedPiece.Rotate();
        }
    }
}
