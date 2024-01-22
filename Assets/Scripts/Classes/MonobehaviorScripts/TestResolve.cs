/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;

public class TestResolve : MonoBehaviour
{
    [SerializeField] FieldLifeTimeScope fieldLifeTimeScope;

    private void Start()
    {
        fieldLifeTimeScope.Container.Resolve<Test>();
    }
}