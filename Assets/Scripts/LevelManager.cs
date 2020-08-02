using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<Enemy> enemies;

    private int enemiesKilled = 0;
    private bool finished = false;
    
    public  delegate void OnFinished();
    public OnFinished onFinished;
    
    void Start()
    {
        enemies = transform.GetComponentsInChildren<Enemy>().ToList();
        foreach (Enemy enemy in enemies)
        {
            enemy.onKill += () =>
            {
                enemiesKilled++;
            };
        }
    }

    void Update()
    {
        if (finished) return;
        if (enemiesKilled >= enemies.Count)
        {
            finished = true;
            onFinished?.Invoke();
        }
    }
}
