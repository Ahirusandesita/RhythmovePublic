/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer; 

public class PlayerSE : MonoBehaviour {

    [SerializeField]
    private AudioClip upMoveSE;
    [SerializeField]
    private AudioClip downMoveSE;
    [SerializeField]
    private AudioClip leftMoveSE;
    [SerializeField]
    private AudioClip rightMoveSE;

    private AudioSource audioSource;

    /// <summary>
    /// DI
    /// </summary>
    /// <param name="audioSource"></param>
    [Inject]
    public void Inject(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    public void MoveSE(object sender,MoveEventArgs moveEventArgs)
    {
        if (moveEventArgs.isCrash)
        {
            return;
        }

        switch (moveEventArgs.moveType)
        {
            case MoveType.up:
                audioSource.PlayOneShot(upMoveSE);
                break;
            case MoveType.down:
                audioSource.PlayOneShot(downMoveSE);
                break;
            case MoveType.left:
                audioSource.PlayOneShot(leftMoveSE);
                break;
            case MoveType.right:
                audioSource.PlayOneShot(rightMoveSE);
                break;
        }
    }


}