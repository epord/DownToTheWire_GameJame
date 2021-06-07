using UnityEngine;
public class GameGrid : MonoBehaviour
{

    public int width = 20;
    public int height = 20;
    public float cellWidth = 0.5f;
    private Cell[] cells;

    // Start is called before the first frame update
    void Start()
    {
        this.cells = new Cell[width * height];
        for(int i = 0; i< width * height; ++i)
        {
            this.cells[i] = new Cell();
        }

        BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(width*cellWidth, height*cellWidth);
        boxCollider2D.offset = boxCollider2D.size / 2;

    }

    public Cell GetCellAt(int row, int col)
    {
        if (row < 0 || row > height || col < 0 || col > width)
        {
            return null;
        }
        return this.cells[row * width + col];
    }

    private Vector2Int GetCellCoordinatesFromWorldPosition(Vector2 worldPoint)
    {
        Vector2 localPoint = worldPoint - new Vector2(transform.position.x, transform.position.y);
        return new Vector2Int(
            (int)Mathf.Floor(localPoint.x / cellWidth),
            (int)Mathf.Floor(localPoint.y / cellWidth)
        );
    }

    public void AddPieceAt(Piece piece, int row, int col)
    {
        Cell cell = GetCellAt(row, col);
        if (cell == null) return;
        if (cell.piece != null)
        {
            Destroy(cell.piece.gameObject);
        }
        cell.piece = piece;
        Vector3 cellWorldPosition = transform.TransformPoint(new Vector3(row * cellWidth + 0.5f * cellWidth, col * cellWidth + +0.5f * cellWidth, 0));
        piece.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 1); // Set z as 1 so pieces are behind the grid  and aren't clickable anymore
    }

    // Finds Cell containing worldPoint and adds the piece to that Cell
    public void AddPieceAt(Piece piece, Vector2 worldPoint)
    {
        Vector2Int cellCoordinates = GetCellCoordinatesFromWorldPosition(worldPoint);
        AddPieceAt(piece, cellCoordinates.x, cellCoordinates.y);
    }

    // Update is called once per frame
    void Update()
    {
        DrawLinesDebug();
    }

    private Vector3 WorldToLocal(Vector3 point)
    {
        return point + transform.position;
    }

    // Draw cells (debug only)
    private void OnDrawGizmos()
    {
        for (int i = 0; i <= height; ++i)
        {
            Gizmos.DrawLine(WorldToLocal(new Vector3(0, i * cellWidth, 0)), WorldToLocal(new Vector3(width * cellWidth, i * cellWidth, 0)));
        }
        for (int i = 0; i <= width; ++i)
        {
            Gizmos.DrawLine(WorldToLocal(new Vector3(i * cellWidth, 0, 0)), WorldToLocal(new Vector3(i * cellWidth, height * cellWidth, 0)));
        }
    }

    private void DrawLinesDebug()
    {
        for (int i = 0; i <= height; ++i)
        {
            Debug.DrawLine(WorldToLocal(new Vector3(0, i * cellWidth, 0)), WorldToLocal(new Vector3(width * cellWidth, i * cellWidth, 0)), Color.green);
        }
        for (int i = 0; i <= width; ++i)
        {
            Debug.DrawLine(WorldToLocal(new Vector3(i * cellWidth, 0, 0)), WorldToLocal(new Vector3(i * cellWidth, height * cellWidth, 0)), Color.green);
        }
    }
}
