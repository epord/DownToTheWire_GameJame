using UnityEngine;

public class SuperPiece : MonoBehaviour
{
    public Piece bottomLeftPiece;
    public Piece bottomRightPiece;
    public Piece topLeftPiece;
    public Piece topRightPiece;

    public SuperPieceSpawner spawner;
    
    public SpriteRenderer spriteRenderer;
    
    public int rotation { get; set; }
    public bool isSelected = false;

    public Piece[] Pieces()
    {
        Piece[] pieces = new[] {bottomLeftPiece, bottomRightPiece, topLeftPiece, topRightPiece};
        return pieces;
    }
    
    public void Rotate()
    {
        Vector3 oldTopLeftPosition = topLeftPiece.transform.position;
        topLeftPiece.transform.position = bottomLeftPiece.transform.position;
        bottomLeftPiece.transform.position = bottomRightPiece.transform.position;
        bottomRightPiece.transform.position = topRightPiece.transform.position;
        topRightPiece.transform.position = oldTopLeftPosition;
        
        Piece oldTopLeftPiece = topLeftPiece;
        topLeftPiece = topRightPiece;
        topRightPiece = bottomRightPiece;
        bottomRightPiece = bottomLeftPiece;
        bottomLeftPiece = oldTopLeftPiece;
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
