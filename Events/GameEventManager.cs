using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set;  }

    public QuestEvents questEvents;
    public PlayerEvents playerEvents;
    public EnemyEvents enemyEvents;
    public InputEvents inputEvents;
    public GoldEvents goldEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
        enemyEvents = new EnemyEvents();
        inputEvents = new InputEvents();
        goldEvents = new GoldEvents();
    }
}

