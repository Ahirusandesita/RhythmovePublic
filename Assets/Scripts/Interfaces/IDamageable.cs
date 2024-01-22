/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
 
public interface IDamageable {
    void Damage(int damage);
    event State.DamageEventHandler OnDamage;
}