/* 制作日 2023/11/28
*　製作者
*　最終更新日 2023/11/28
*/

using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Collections;

public class PlayerView : MonoBehaviour, IDisposable
{
    public delegate void PlayerInputHandler(object sender, InputEventArgs e);
    public event PlayerInputHandler OnInput;


    [SerializeField]
    private SpriteRenderer upImage;
    [SerializeField]
    private SpriteRenderer downImage;
    [SerializeField]
    private SpriteRenderer rightImage;
    [SerializeField]
    private SpriteRenderer leftImage;

    private SpriteRenderer nowImage;
    private Vector2 size;

    private IDisposable disposable;

    private void Awake()
    {
        InputSubscribe().Forget();
    }

    private async UniTask InputSubscribe()
    {
        await UniTask.WaitUntil(() => InputSystem.Instance is not null);
        disposable = InputSystem.Instance.readOnlyMoveProperty.Skip(1).Subscribe(_ => OnInput?.Invoke(this, new InputEventArgs()));
    }

    public void MoveDirectionSprite(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.down:
                SpriteAnimation(downImage);
                break;
            case MoveType.up:
                SpriteAnimation(upImage);
                break;
            case MoveType.right:
                SpriteAnimation(rightImage);
                break;
            case MoveType.left:
                SpriteAnimation(leftImage);
                break;
            case MoveType.nonMove:
                //ArrowColor_Yellow();
                //ResetUI();
                //nowImage = default;
                break;
        }
    }

    private void ArrowSpriteOff()
    {
        downImage.enabled = false;
        upImage.enabled = false;
        rightImage.enabled = false;
        leftImage.enabled = false;
    }

    public void ArrowSpriteOn()
    {
        downImage.enabled = true;
        upImage.enabled = true;
        rightImage.enabled = true;
        leftImage.enabled = true;
    }

    private void ArrowColor_Yellow()
    {
        downImage.color = Color.yellow;
        upImage.color = Color.yellow;
        rightImage.color = Color.yellow;
        leftImage.color = Color.yellow;
    }

    private void SpriteAnimation(SpriteRenderer spriteRenderer)
    {
        nowImage = spriteRenderer;
        ArrowColor_Yellow();
        spriteRenderer.color = new Color(1f, 1f, 0.4f);
        size = nowImage.gameObject.transform.localScale;
        nowImage.gameObject.transform.localScale = new Vector2(size.x * 1.3f, size.y * 1.3f);

        StartCoroutine(Ani(spriteRenderer));
    }
    private void ResetUI()
    {
        nowImage.gameObject.transform.localScale = size;
    }

    private IEnumerator Ani(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.gameObject.transform.localScale = size;
    }

    public void NextStage(object sender, FinishEventArgs finishEventArgs)
    {
        ArrowSpriteOff();
    }


    /// <summary>
    /// 修正必要　MVP
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="clashEventArgs"></param>
    public void Clash(object sender, ClashEventArgs clashEventArgs)
    {
        if (clashEventArgs.nowClashTime)
        {
            ArrowSpriteOff();
        }
        else
        {
            ArrowSpriteOn();
        }
    }

    public void Dispose()
    {
        disposable.Dispose();
    }

    public void OnDisable()
    {
        disposable.Dispose();
    }

}