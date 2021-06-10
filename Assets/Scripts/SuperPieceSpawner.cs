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
        newSuperPieceObject.transform.position = new Vector3(spawnerPosition.x, spawnerPosition.y, spawnerPosition.z - 2);
        
        GameObject bottomLeftPieceObject = Instantiate(spawnablePieces[Random.Range(0, spawnablePieces.Length)]);
        Piece bottomLeftPiece = bottomLeftPieceObject.GetComponent(typeof(Piece)) as Piece;
        if(bottomLeftPiece is null) Debug.LogException(new Exception());
        superPiece.bottomLeftPiece = bottomLeftPiece;
        bottomLeftPiece.transform.parent = superPiece.transform;
        bottomLeftPiece.transform.position = new Vector3(spawnerPosition.x - 0.5f * grid.cellWidth, spawnerPosition.y - 0.5f * grid.cellWidth, spawnerPosition.z - 1);
        
        GameObject bottomRightPieceObject = Instantiate(spawnablePieces[Random.Range(0, spawnablePieces.Length)]);
        Piece bottomRightPiece = bottomRightPieceObject.GetComponent(typeof(Piece)) as Piece;
        if(bottomRightPiece is null) Debug.LogException(new Exception());
        superPiece.bottomRightPiece = bottomRightPiece;
        bottomRightPiece.transform.parent = superPiece.transform;
        bottomRightPiece.transform.position = new Vector3(spawnerPosition.x + 0.5f * grid.cellWidth, spawnerPosition.y - 0.5f * grid.cellWidth, spawnerPosition.z - 1);
        
        GameObject topLeftPieceObject = Instantiate(spawnablePieces[Random.Range(0, spawnablePieces.Length)]);
        Piece topLeftPiece = topLeftPieceObject.GetComponent(typeof(Piece)) as Piece;
        if(topLeftPiece is null) Debug.LogException(new Exception());
        superPiece.topLeftPiece = topLeftPiece;
        topLeftPiece.transform.parent = superPiece.transform;
        topLeftPiece.transform.position = new Vector3(spawnerPosition.x - 0.5f * grid.cellWidth, spawnerPosition.y + 0.5f * grid.cellWidth, spawnerPosition.z - 1);
        
        GameObject topRightPieceObject = Instantiate(spawnablePieces[Random.Range(0, spawnablePieces.Length)]);
        Piece topRightPiece = topRightPieceObject.GetComponent(typeof(Piece)) as Piece;
        if(topRightPiece is null) Debug.LogException(new Exception());
        superPiece.topRightPiece = topRightPiece;
        topRightPiece.transform.parent = superPiece.transform;
        topRightPiece.transform.position = new Vector3(spawnerPosition.x + 0.5f * grid.cellWidth, spawnerPosition.y + 0.5f * grid.cellWidth, spawnerPosition.z - 1);
        
        superPiece.spawner = this;
    }
}