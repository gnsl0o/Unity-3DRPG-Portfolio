using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyEvents
{
    public event Action<Enemy> onEnemyDie;

    public void EnemyDie(Enemy enemy)
    {
        if (onEnemyDie != null)
        {
            onEnemyDie(enemy);
        }
    }
}
