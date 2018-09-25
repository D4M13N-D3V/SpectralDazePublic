using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.World
{
    /// <summary>
    /// A platform that moves back and forth waypoints. Can be automatic, can be manual.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(NavMeshSurface))]
    public class Platform : MonoBehaviour
    {
        /// <summary>
        /// The waypoints for the platform to move to and from.
        /// </summary>
        public List<Transform> Waypoints;
        /// <summary>
        /// Should the platform automatically loop back and forth through waypoints.
        /// </summary>
        public bool Automatic = true;
        /// <summary>
        /// The starting waypoint
        /// </summary>
        public int StartingWaypoint = 0;
        /// <summary>
        /// The time between waypoint movement.
        /// </summary>
        public float TimeBetweenWaypoints = 5;
        /// <summary>
        /// The current waypoint.
        /// </summary>
        public int _currentWaypoint = 0;
        /// <summary>
        /// The platform is currently moving
        /// </summary>
        public bool _currentlyMoving = false;
        /// <summary>
        /// Is the platform looping back
        /// </summary>
        public bool _loopingBack = false;

        /// <summary>
        /// The navmesh surface attached to the paltform
        /// </summary>
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

                if (_currentWaypoint >= Waypoints.Count - 1)
                    _loopingBack = true;

                if (_currentWaypoint <= 0)
                    _loopingBack = false;

                if (_loopingBack)
                    _currentWaypoint--;
                else
                    _currentWaypoint++;
            }
        }

        /// <summary>
        /// Moves to given waypoint.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        public void MoveToWaypoint(int waypoint = 0)
        {
            if (Waypoints[waypoint] != null)
            {
                _currentlyMoving = true;
                LeanTween.move(gameObject, Waypoints[waypoint], 5).setOnComplete(() =>
                {
                    _currentlyMoving = false;
                });
            }
        }

        /// <summary>
        /// The original parent of the player
        /// </summary>
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


}