using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.AI;
using SpectralDaze.AI.QuestNPC;
using SpectralDaze.Managers.AudioManager;
using UnityEngine;

namespace SpectralDaze.AI
{
    /// <summary>
    /// Scriptable object to hold all the information about the options of a AI   ( Shooting AI )
    /// </summary>
    [CreateAssetMenu(menuName = "Spectral Daze/AI/Shooting AI Options")]
    public class ShootingAIOptions : ScriptableObject
    {
        /// <summary>
        /// The type of movement. This can be patroling, wandering within limits, or wandering with no limits.
        /// </summary>
        public ShootingAI.MovementType MovementType;
        /// <summary>
        /// How far the AI is allowed to wonder off from the origin point;
        /// </summary>
        public float WanderDistance;
        /// <summary>
        /// How long the AI should idle between movements.
        /// </summary>
        public float IdleTime;
        /// <summary>
        /// The patrol point the AI should start on.
        /// </summary>
        public int StartingPatorlPoint;
        /// <summary>
        /// A list of vector 3's  for patrol points that the AI can walk from and to.
        /// </summary>
        public List<Vector3> PatrolPoints;
        /// <summary>
        /// The distance that the AI becomes aggressive to the player.
        /// </summary>
        public float AggroDistance;
        /// <summary>
        /// The distance that the AI will have to be to disengage the player;
        /// </summary>
        public float LoseAggroDistance;
        /// <summary>
        /// The amount of time between attacks.
        /// </summary>
        public float TimeBetweenAttacks;
        /// <summary>
        /// How fast the AI moves.
        /// </summary>
        public float MovementSpeed = 3;
        /// <summary>
        /// The prefab for the bullet.
        /// </summary>
        public GameObject BulletPrefab;
        /// <summary>
        /// The delay between when the shooting starts, and when the shot is fired.
        /// </summary>
        public float ShootDelay;
        /// <summary>
        /// If the AI is chasing.
        /// </summary>
        public bool Chase = true;
        /// <summary>
        /// The distance to stop chasing the player at.
        /// </summary>
        public float ChaseDistance = 20;
        /// <summary>
        /// The AudioClipInfo scriptable object for the shooting sound.
        /// </summary>
        public AudioClipInfo ShootingSound;
        /// <summary>
        /// The AudioClipInfo scriptable object for the death sound.
        /// </summary>
        public AudioClipInfo DeathSound;
    }
}
 