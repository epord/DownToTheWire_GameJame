using System;
using System.Collections.Generic;
using UnityEngine;
public class GameGrid : MonoBehaviour
{

    public int width = 20;
    public int height = 20;
    public float cellWidth = 0.5f;
    private Cell[] cells;

    public GameManager gameManager;

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
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        AddPieceAt(Instantiate(gameManager.blueCrystal), 4, 10);
        AddPieceAt(Instantiate(gameManager.goldCrystal), 2, 8);
        AddPieceAt(Instantiate(gameManager.greenCrystal), 4, 6);
        AddPieceAt(Instantiate(gameManager.redCrystal), 6, 8);

        AddPieceAt(InstantiateOrb(1), 0, 0);
        AddPieceAt(InstantiateOrb(1), 3, 0);
        AddPieceAt(InstantiateOrb(1), 6, 0);
        AddPieceAt(InstantiateOrb(1), 9, 0);
        AddPieceAt(InstantiateOrb(3), 0, 16);
        AddPieceAt(InstantiateOrb(3), 3, 16);
        AddPieceAt(InstantiateOrb(3), 6, 16);
        AddPieceAt(InstantiateOrb(3), 9, 16);
        AddPieceAt(InstantiateOrb(0), 10, 2);
        AddPieceAt(InstantiateOrb(0), 10, 5);
        AddPieceAt(InstantiateOrb(0), 10, 8);
        AddPieceAt(InstantiateOrb(0), 10, 11);
        AddPieceAt(InstantiateOrb(0), 10, 14);
    }

    private Piece InstantiateOrb(int rotationCount)
    {
        Piece orb = Instantiate(gameManager.orb);
        for (int i = 0; i < rotationCount; i++)
        {
            orb.Rotate();
        }
        return orb;
    }

    public Cell GetCellAt(int row, int col)
    {
        if (row < 0 || row >= height || col < 0 || col >= width)
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

    public bool AddPieceAt(Piece piece, int row, int col)
    {
        Cell cell = GetCellAt(row, col);
        if (cell == null) return false;
        if (cell.piece != null)
        {
            if (!cell.piece.canBeReplaced)
            {
                return false;
            }
            Destroy(cell.piece.gameObject);
        }
        piece.col = col;
        piece.row = row;
        cell.piece = piece;
        foreach (var pieceSubPiece in cell.piece.subPieces)
        {
            pieceSubPiece.spriteRenderer.sortingOrder = pieceSubPiece.shouldRenderInFront ? 1 : 0;
        }
        Vector3 cellWorldPosition = transform.TransformPoint(new Vector3(col * cellWidth + 0.5f * cellWidth, row * cellWidth + 0.5f * cellWidth, 0));
        piece.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 1); // Set z as 1 so pieces are behind the grid  and aren't clickable anymore
        UpdateGridColors();
        return true;
    }

    // Finds Cell containing worldPoint and adds the piece to that Cell
    public bool AddPieceAt(Piece piece, Vector2 worldPoint)
    {
        Vector2Int cellCoordinates = GetCellCoordinatesFromWorldPosition(worldPoint);
        return AddPieceAt(piece, cellCoordinates.y, cellCoordinates.x);
    }
    
    // Finds Cell containing worldPoint and adds the superPiece to that group of Cells
    public bool AddSuperPieceAt(SuperPiece superPiece, Vector2 worldPoint)
    {
        // Calculate the cell in the bottomLeftCorner
        Vector2 bottomLeftCellPoint = new Vector2(worldPoint.x - cellWidth / 2, worldPoint.y - cellWidth / 2);
        Vector2Int cellCoordinates = GetCellCoordinatesFromWorldPosition(bottomLeftCellPoint);
        Cell[] cells = new[]
        {
            GetCellAt(cellCoordinates.y, cellCoordinates.x),
            GetCellAt(cellCoordinates.y + 1, cellCoordinates.x),
            GetCellAt(cellCoordinates.y, cellCoordinates.x + 1),
            GetCellAt(cellCoordinates.y + 1, cellCoordinates.x + 1)
        };
        foreach (var cell in cells)
        {
            if (cell == null) return false;
            if (cell.piece != null && !cell.piece.canBeReplaced) return false;
        }
        foreach (var piece in superPiece.Pieces())
        {
            piece.transform.parent = null;
        }
        AddPieceAt(superPiece.bottomLeftPiece, cellCoordinates.y, cellCoordinates.x);
        AddPieceAt(superPiece.bottomRightPiece, cellCoordinates.y, cellCoordinates.x + 1);
        AddPieceAt(superPiece.topLeftPiece, cellCoordinates.y + 1, cellCoordinates.x);
        AddPieceAt(superPiece.topRightPiece, cellCoordinates.y + 1, cellCoordinates.x + 1);
        Destroy(superPiece.gameObject);
        return true;
    }
    
    // Updates all the cells so that all the subpieces have their correspondent colors
    public void UpdateGridColors()
    {
        Queue<SubPiece> toColorQ = ResetColors();
        ColorGrid(toColorQ);
    }

    // Reset all the subPieces to black except the sources of color
    private Queue<SubPiece> ResetColors()
    {
        Queue<SubPiece> toColorQ = new Queue<SubPiece>();
        foreach (var cell in cells)
        {
            if (cell.piece is object)
            {
                foreach (var subPiece in cell.piece.subPieces)
                {
                    if (subPiece.isAlwaysOn)
                    {
                        toColorQ.Enqueue(subPiece);
                    }
                    else
                    {
                        subPiece.magicColor = MagicColor.BLACK;
                    }
                }
            }
        }
        return toColorQ;
    }

    // Color all the grid given a queue with the sources of color
    private void ColorGrid(Queue<SubPiece> toColorQ)
    {
        while (toColorQ.Count > 0)
        {
            SubPiece currentSubPiece = toColorQ.Dequeue();
            if (currentSubPiece.rightConnection)
            {
                Cell checkCell = GetCellAt(currentSubPiece.parentPiece.row, currentSubPiece.parentPiece.col + 1);
                CheckIfColorCell(currentSubPiece, toColorQ, checkCell, (subPiece => subPiece.leftConnection));
            }
            if (currentSubPiece.leftConnection)
            {
                Cell checkCell = GetCellAt(currentSubPiece.parentPiece.row, currentSubPiece.parentPiece.col - 1);
                CheckIfColorCell(currentSubPiece, toColorQ, checkCell, (subPiece => subPiece.rightConnection));
            }
            if (currentSubPiece.topConnection)
            {
                Cell checkCell = GetCellAt(currentSubPiece.parentPiece.row + 1, currentSubPiece.parentPiece.col);
                CheckIfColorCell(currentSubPiece, toColorQ, checkCell, (subPiece => subPiece.bottomConnection));
            }
            if (currentSubPiece.bottomConnection)
            {
                Cell checkCell = GetCellAt(currentSubPiece.parentPiece.row - 1, currentSubPiece.parentPiece.col);
                CheckIfColorCell(currentSubPiece, toColorQ, checkCell, (subPiece => subPiece.topConnection));
            }
        }
    }

    // Check if any of the subPieces needs to be colored and if true adds it to the colorQ
    private void CheckIfColorCell(SubPiece currentSubPiece, Queue<SubPiece> toColorQ, Cell toColorCell, Func<SubPiece, bool> newSubPieceTurnOnCondition)
    {
        if (toColorCell is object && toColorCell.piece is object)
        {
            foreach (var subPiece in toColorCell.piece.subPieces)
            {
                if (newSubPieceTurnOnCondition(subPiece) && subPiece.magicColor == MagicColor.BLACK)
                {
                    subPiece.magicColor = currentSubPiece.magicColor;
                    toColorQ.Enqueue(subPiece);
                }
            }
        }
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
