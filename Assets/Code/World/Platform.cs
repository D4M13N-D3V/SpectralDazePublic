using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Platform : MonoBehaviour
{
    public List<Transform> Waypoints;
    public bool Automatic = true;
    public int StartingWaypoint = 0;
    public float TimeBetweenWaypoins = 5;
    public int _currentWaypoint = 0;
    public bool _currentlyMoving = false;
    public bool _loopingBack = false;

    private NavMeshSurface _surface;

    private void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
        _surface.collectObjects = CollectObjects.Children;
        _surface.defaultArea = NavMesh.GetAreaFromName("Platform");
        _surface.BuildNavMesh();
        _currentWaypoint = StartingWaypoint;
        transform.position = Waypoints[_currentWaypoint].position;
    }

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

    private Transform oldParent;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            oldParent = collider.transform.parent;
            collider.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.parent = oldParent;
        }
    }
}
