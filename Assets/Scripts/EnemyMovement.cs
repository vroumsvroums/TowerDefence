using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Waypoints waypointsPos;

    

    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemy;

    private int counterBranch = 0;

    private void Start()
    {
        enemy = GetComponent<Enemy>();

        waypointsPos = FindObjectOfType<Waypoints>();

        target = Waypoints.points[0];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        else
        {
            
            if (waypointsPos.branchAmount.Length!=0)
            {
                if (waypointIndex == waypointsPos.branchAmount[counterBranch])
                {
                    int x = Random.Range(0, 2);
                    if (x == 0)
                    {
                        waypointIndex++;
                        target = Waypoints.points[waypointIndex];
                        waypointIndex++;
                    }
                    else if (x == 1)
                    {
                        waypointIndex = waypointIndex + 2;
                        target = Waypoints.points[waypointIndex];
                    }
                    if (counterBranch < waypointsPos.branchAmount.Length-1)
                    {
                        counterBranch++;
                    }
                }
                else
                {
                    waypointIndex++;
                    target = Waypoints.points[waypointIndex];
                }
            }
            else
            {
                waypointIndex++;
                target = Waypoints.points[waypointIndex];
            }
        }
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        PlayerStats.Lives = Mathf.Clamp(PlayerStats.Lives, 0, PlayerStats.Lives);
        
        Destroy(gameObject);
        WaveSpawner.EnemiesAlive--;
    }

}
