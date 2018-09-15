using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public List<Transform> Waypoints;
    public bool Automatic = true;
    public int StartingWaypoint = 0;
    public float TimeBetweenWaypoins = 5;
    public int _currentWaypoint = 0;
    public bool _currentlyMoving = false;
    public bool _loopingBack = false;
    private void Update()
    {
        if (!_currentlyMoving)
        {
            MoveToWaypoint(_currentWaypoint);

            if (_currentWaypoint >= Waypoints.Count-1)
                _loopingBack = true;

            if (_currentWaypoint <= 0)
                _loopingBack = false;

            if (_loopingBack)
                _currentWaypoint--;
            else
                _currentWaypoint++;
        }
    }

    public void MoveToWaypoint(int waypoint = 0)
    {
        if (Waypoints[waypoint] != null)
        {
            _currentlyMoving = true;
            LeanTween.move(gameObject, Waypoints[waypoint], 5).setOnComplete(() =>
            {
                _currentlyMoving=false;
            });
        }
    }
}
