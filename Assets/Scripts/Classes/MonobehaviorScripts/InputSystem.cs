/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using UniRx;


public class InputSystem : MonoBehaviour
{

    private ReactiveProperty<InputType> moveProperty = new ReactiveProperty<InputType>();
    public IReadOnlyReactiveProperty<InputType> readOnlyMoveProperty => moveProperty;


    private static InputSystem instance;

    public static InputSystem Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            moveProperty.SetValueAndForceNotify(InputType.Return);
        }

    }

    private void OnDestroy()
    {
        Instance = null;
        moveProperty.Dispose();
    }

}