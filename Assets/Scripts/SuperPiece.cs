using UnityEngine;

public class SuperPiece : MonoBehaviour
{
    public Piece bottomLeftPiece;
    public Piece bottomRightPiece;
    public Piece topLeftPiece;
    public Piece topRightPiece;

    public SuperPieceSpawner spawner;
    
    public int rotation { get; set; }
    public bool isSelected = false;

    public Piece[] Pieces()
    {
        Piece[] pieces = new[] {bottomLeftPiece, bottomRightPiece, topLeftPiece, topRightPiece};
        return pieces;
    }
    
    public void Rotate()
    {
        Piece oldTopLeftPiece = topLeftPiece;
        topLeftPiece = topRightPiece;
        topLeftPiece.transform.localPosition = new Vector3(-spawner.grid.cellWidth/2, spawner.grid.cellWidth/2, 1);
        topRightPiece = bottomRightPiece;
        topRightPiece.transform.localPosition = new Vector3(spawner.grid.cellWidth/2, spawner.grid.cellWidth/2, 1);
        bottomRightPiece = bottomLeftPiece;
        bottomRightPiece.transform.localPosition = new Vector3(spawner.grid.cellWidth/2, -spawner.grid.cellWidth/2, 1);
        bottomLeftPiece = oldTopLeftPiece;
        bottomLeftPiece.transform.localPosition = new Vector3(-spawner.grid.cellWidth/2, -spawner.grid.cellWidth/2, 1);
        foreach (var piece in Pieces())
        {
            piece.Rotate();
        }
    }

    void Update()
    {
        if (isSelected)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, 0);
        }
    }
}
