using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SuperPieceSpawner : MonoBehaviour
{
    public SuperPiece superPiece;
    public GameObject superPiecePrefab;
    public GameGrid grid;

    public GameObject[] spawnablePieces;

    public float timer = 0.0f;
    public float waitTime = 1.0f;
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime && superPiece is null)
        {
            SpawnSuperPiece();
        }
    }

    public void PieceSelected()
    {
        timer = 0.0f;
        superPiece = null;
    }

    void SpawnSuperPiece()
    {
        GameObject newSuperPieceObject = Instantiate(superPiecePrefab);
        superPiece = newSuperPieceObject.GetComponent(typeof(SuperPiece)) as SuperPiece;
        if(superPiece is null) Debug.LogException(new Exception());

        Vector3 spawnerPosition = this.transform.position;
        newSuperPieceObject.transform.position = spawnerPosition;

        Piece bottomLeftPiece = GenerateRandomPiece();
        superPiece.bottomLeftPiece = bottomLeftPiece;
        bottomLeftPiece.transform.parent = superPiece.transform;
        bottomLeftPiece.transform.position = new Vector3(spawnerPosition.x - 0.5f * grid.cellWidth, spawnerPosition.y - 0.5f * grid.cellWidth, spawnerPosition.z);
        
        Piece bottomRightPiece = GenerateRandomPiece();
        superPiece.bottomRightPiece = bottomRightPiece;
        bottomRightPiece.transform.parent = superPiece.transform;
        bottomRightPiece.transform.position = new Vector3(spawnerPosition.x + 0.5f * grid.cellWidth, spawnerPosition.y - 0.5f * grid.cellWidth, spawnerPosition.z);
        
        Piece topLeftPiece = GenerateRandomPiece();
        superPiece.topLeftPiece = topLeftPiece;
        topLeftPiece.transform.parent = superPiece.transform;
        topLeftPiece.transform.position = new Vector3(spawnerPosition.x - 0.5f * grid.cellWidth, spawnerPosition.y + 0.5f * grid.cellWidth, spawnerPosition.z);
        
        Piece topRightPiece = GenerateRandomPiece();
        superPiece.topRightPiece = topRightPiece;
        topRightPiece.transform.parent = superPiece.transform;
        topRightPiece.transform.position = new Vector3(spawnerPosition.x + 0.5f * grid.cellWidth, spawnerPosition.y + 0.5f * grid.cellWidth, spawnerPosition.z);
        
        superPiece.spawner = this;
    }

    private Piece GenerateRandomPiece()
    {
        GameObject pieceObject = Instantiate(spawnablePieces[Random.Range(0, spawnablePieces.Length)]);
        Piece piece = pieceObject.GetComponent(typeof(Piece)) as Piece;
        if(piece is null) Debug.LogException(new Exception());
        int rotation = Random.Range(0, 4);
        for (int i = 0; i < rotation; i++)
        {
            piece.Rotate();
        }
        return piece;
    }
}