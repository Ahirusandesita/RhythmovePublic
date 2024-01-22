/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;
using VContainer;
using System;

public struct StateData
{
    public int hp;
    public StateData(int hp)
    {
        this.hp = hp;
    }
}

public class DamageEventArgs : EventArgs
{
    public StateData stateData;
    //public event State.DamageEventHandler OnDamage;
    public bool aLive;
}


public class State : IDamageable
{
    public delegate void DamageEventHandler(object sender, DamageEventArgs e);
    public event DamageEventHandler OnDamage;

    private StateData stateData;
    private Field field;

    private bool TentativeBool = true;
    private object TentativeObject = new object();

    [Inject]
    public State(Field field, StateData stateData)
    {
        this.field = field;
        this.stateData = stateData;

        this.field.AddDamage(this);
    }

    public void Damage(int damage)
    {
        lock (TentativeObject)
        {
            if (!TentativeBool)
            {
                return;
            }
            TentativeBool = false;
            DamageEventArgs damageEventArgs = new DamageEventArgs();
            damageEventArgs.aLive = true;
            stateData.hp -= damage;

            if (stateData.hp <= 0)
            {
                stateData.hp = 0;
                damageEventArgs.aLive = false;
            }
            damageEventArgs.stateData = stateData;
            OnDamage?.Invoke(this, damageEventArgs);
        }
    }

}