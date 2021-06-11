using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource monster1;
    public AudioSource monster2;
    public AudioSource monster3;
    public AudioSource releasePiece;

    public void PlayMonster1()
    {
        monster1.Play();
    }

    public void PlayMonster2()
    {
        monster2.Play();
    }

    public void PlayMonster3()
    {
        monster3.Play();
    }

    public void PlayReleasePiece()
    {
        releasePiece.Play();
    }
}
