using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SuperPieceSpawner : MonoBehaviour
{
    public SuperPiece superPiece;
    public GameObject superPiecePrefab;
    public GameGrid grid;

    public GameObject[] spawnablePieces;

    public float timer = 5.0f;
    public float waitTime = 5.0f;

    private const int TOP = 0;
    private const int RIGHT = 1;
    private const int BOTTOM = 2;
    private const int LEFT = 3;
    
    public enum ConnectionType
    {
        PRESENT,
        MISSING,
        ANY,
    }

    
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

        Piece bottomLeftPiece = GenerateRandomPiece(new []{ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY});
        superPiece.bottomLeftPiece = bottomLeftPiece;
        bottomLeftPiece.transform.parent = superPiece.transform;
        bottomLeftPiece.transform.position = new Vector3(spawnerPosition.x - 0.5f * grid.cellWidth, spawnerPosition.y - 0.5f * grid.cellWidth, spawnerPosition.z);
        bool[] bottomLeftPieceConnections = bottomLeftPiece.GetConnections();

        ConnectionType[] validConnections = new[] {ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY};
        validConnections[LEFT] = bottomLeftPieceConnections[RIGHT] ? ConnectionType.PRESENT : ConnectionType.MISSING;
        Piece bottomRightPiece = GenerateRandomPiece(validConnections);
        superPiece.bottomRightPiece = bottomRightPiece;
        bottomRightPiece.transform.parent = superPiece.transform;
        bottomRightPiece.transform.position = new Vector3(spawnerPosition.x + 0.5f * grid.cellWidth, spawnerPosition.y - 0.5f * grid.cellWidth, spawnerPosition.z);
        bool[] bottomRightPieceConnections = bottomRightPiece.GetConnections();
        
        validConnections = new[] {ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY};
        validConnections[BOTTOM] = bottomLeftPieceConnections[TOP] ? ConnectionType.PRESENT : ConnectionType.MISSING;
        Piece topLeftPiece = GenerateRandomPiece(validConnections);
        superPiece.topLeftPiece = topLeftPiece;
        topLeftPiece.transform.parent = superPiece.transform;
        topLeftPiece.transform.position = new Vector3(spawnerPosition.x - 0.5f * grid.cellWidth, spawnerPosition.y + 0.5f * grid.cellWidth, spawnerPosition.z);
        bool[] topLeftPieceConnections = topLeftPiece.GetConnections();
        
        validConnections = new[] {ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY, ConnectionType.ANY};
        validConnections[BOTTOM] = bottomRightPieceConnections[TOP] ? ConnectionType.PRESENT : ConnectionType.MISSING;
        validConnections[LEFT] = topLeftPieceConnections[RIGHT] ? ConnectionType.PRESENT : ConnectionType.MISSING;
        Piece topRightPiece = GenerateRandomPiece(validConnections);
        superPiece.topRightPiece = topRightPiece;
        topRightPiece.transform.parent = superPiece.transform;
        topRightPiece.transform.position = new Vector3(spawnerPosition.x + 0.5f * grid.cellWidth, spawnerPosition.y + 0.5f * grid.cellWidth, spawnerPosition.z);
        
        superPiece.spawner = this;
    }

    // Generate random piece. Check if [Top, Right, Bottom, Left] have to be there, can't be there, or either is valid.
    private Piece GenerateRandomPiece(ConnectionType[] validConnections)
    {
        if (validConnections.Length != 4)
        {
            Debug.LogException(new Exception());
            return null;
        }
        GameObject gameObjectToSpawn = null;
        int rotation = 0;
        while (gameObjectToSpawn is null)
        {
            gameObjectToSpawn = spawnablePieces[Random.Range(0, spawnablePieces.Length)];
            Piece pieceToSpawn = gameObjectToSpawn.GetComponent(typeof(Piece)) as Piece;
            bool topConnected = false;
            bool rightConnected = false;
            bool bottomConnected = false;
            bool leftConnected = false;
            foreach (var subPiece in pieceToSpawn.subPieces)
            {
                if (subPiece.topConnection) topConnected = true;
                if (subPiece.rightConnection) rightConnected = true;
                if (subPiece.bottomConnection) bottomConnected = true;
                if (subPiece.leftConnection) leftConnected = true;
            }
            rotation = Random.Range(0, 4);
            foreach (var connectionType in validConnections)
            {
                bool pieceConnection = false;
                switch(rotation % 4)
                {
                    case 0:
                        pieceConnection = topConnected;
                        break;
                    case 1:
                        pieceConnection = rightConnected;
                        break;
                    case 2:
                        pieceConnection = bottomConnected;
                        break;
                    case 3:
                        pieceConnection = leftConnected;
                        break;
                }
                if (!isValidConnection(connectionType, pieceConnection))
                {
                    gameObjectToSpawn = null;
                    break;
                }
                rotation++;
            }
            rotation -= 4;
        }
        GameObject pieceObject = Instantiate(gameObjectToSpawn);
        Piece result = pieceObject.GetComponent(typeof(Piece)) as Piece;
        if(result is null) Debug.LogException(new Exception());
        for (int i = 0; i < rotation; i++)
        {
            result.Rotate();
        }
        return result;
    }

    private bool isValidConnection(ConnectionType connectionType, bool connected)
    {
        if (connectionType == ConnectionType.PRESENT && !connected) return false; 
        if (connectionType == ConnectionType.MISSING && connected) return false;
        return true;
    }
}