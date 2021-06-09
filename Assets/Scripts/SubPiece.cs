using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SubPiece : MonoBehaviour
{
    public bool leftConnection;
    public bool rightConnection;
    public bool topConnection;
    public bool bottomConnection;

    public Sprite blackSprite;
    public Sprite blueSprite;
    public Sprite goldSprite;
    public Sprite greenSprite;
    public Sprite redSprite;
    
    public MagicColor magicColor;
    public bool isAlwaysOn;

    public Piece parentPiece;

    public SpriteRenderer spriteRenderer;

    public void SetColor(MagicColor newColor)
    {
        magicColor = newColor;
    }

    public void Update()
    {
        Sprite newSprite = null;
        switch (magicColor)
        {
            case MagicColor.RED:
                newSprite = redSprite;
                break;
            case MagicColor.BLUE:
                newSprite = blueSprite;
                break;
            case MagicColor.GREEN:
                newSprite = greenSprite;
                break;
            case MagicColor.BLACK:
                newSprite = blackSprite;
                break;
            case MagicColor.GOLD:
                newSprite = goldSprite;
                break;
            default:
                Debug.LogException(new Exception());
                break;
        }
        spriteRenderer.sprite = newSprite;
    }

    public void Rotate()
    {
        bool oldLeftConnection = leftConnection;
        leftConnection = topConnection;
        topConnection = rightConnection;
        rightConnection = bottomConnection;
        bottomConnection = oldLeftConnection;
    }
}
