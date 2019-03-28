using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void Die();
    void TakeDamage(int damage);
}
