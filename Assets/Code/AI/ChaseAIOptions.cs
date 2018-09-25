using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using SpectralDaze.AI;
using SpectralDaze.AI.QuestNPC;
using SpectralDaze.Managers.AudioManager;
using UnityEngine;

namespace SpectralDaze.AI
{
    /// <summary>
    /// Scriptable object to hold the information for the Chase AI
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/AI/Chase AI Settings")]
    public class ChaseAIOptions : ScriptableObject
    {
        /// <summary>
        /// The movement type
        /// </summary>
        public ChaseAI.MovementType MovementType;
        /// <summary>
        /// The wander distance
        /// </summary>
        public float WanderDistance;
        /// <summary>
        /// The idle time
        /// </summary>
        public float IdleTime;
        /// <summary>
        /// The starting patrol point
        /// </summary>
        public int StartingPatrolPoint;
        /// <summary>
        /// The patrol points posistions
        /// </summary>
        public List<Vector3> PatrolPoints;
        /// <summary>
        /// The distance that the AI becomes aggressive.
        /// </summary>
        public float AggroDistance;
        /// <summary>
        /// The distance that the AI looses aggression
        /// </summary>
        public float LoseAggroDistance;
        /// <summary>
        /// The time between charges.
        /// </summary>
        public float TimeBetweenCharges;
        /// <summary>
        /// The velocity to launch at.
        /// </summary>
        public float LaunchVelocity = 10;
        /// <summary>
        /// The movement speed
        /// </summary>
        public float MovementSpeed = 3;
        /// <summary>
        /// Is the AI chasing
        /// </summary>
        public bool Chase;
        /// <summary>
        /// The distance to stop chasing the player
        /// </summary>
        public float ChaseDistance = 20;
        /// <summary>
        /// The death sound
        /// </summary>
        public AudioClipInfo DeathSound;
    }
}
