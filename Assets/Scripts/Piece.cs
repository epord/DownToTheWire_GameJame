using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject prefab;

    public SubPiece[] subPieces;
    
    public bool canBeReplaced = true;

    public int rotation { get; set; }
    public bool isSelected = false;
    [HideInInspector]
    public int col;
    [HideInInspector]
    public int row;
    
    public void Rotate()
    {
        rotation = (rotation + 90) % 360;
        transform.Rotate(new Vector3(0, 0, 90));
        foreach (var subPiece in subPieces)
        {
            subPiece.Rotate();
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
