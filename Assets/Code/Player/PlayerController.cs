using SpectralDaze.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.DataTypes;
using SpectralDaze.Managers.InputManager;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SpectralDaze.Player
{
    /// <summary>
    /// The player controller
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// The input rotation
        /// </summary>
        public ScriptableObjectQuartenion InputRotation;
        /// <summary>
        /// The player information
        /// </summary>
        public PlayerInfo PlayerInfo;
        /// <summary>
        /// The animator
        /// </summary>
        public Animator Animator;
        /// <summary>
        /// The rigidbody of the player
        /// </summary>
        public Rigidbody Rbody;
        /// <summary>
        /// The time information for manipulation of time
        /// </summary>
        public TimeInfo TimeInfo;
        /// <summary>
        /// The nav mesh agent
        /// </summary>
        public NavMeshAgent Agent;
        /// <summary>
        /// Is the time being manipulated
        /// </summary>
        private bool _timeBeingManipulated;
        /// <summary>
        /// The manipulation type of time
        /// </summary>
        private Manipulations _manipulationType;
        /// <summary>
        /// The x movement control
        /// </summary>
        public AxisControl XMovementControl;
        /// <summary>
        /// The y movement control
        /// </summary>
        public AxisControl YMovementControl;
        /// <summary>
        /// The local time scale
        /// </summary>
        [HideInInspector]
        public float _localTimeScale = 1.0f;
        /// <summary>
        /// Gets or sets the local time scale.
        /// </summary>
        /// <value>
        /// The local time scale.
        /// </value>
        public float localTimeScale
        {
            get
            {
                return _localTimeScale;
            }
            set
            {
                float multiplier = value / _localTimeScale;
                _localTimeScale = value;
            }
        }
        /// <summary>
        /// Gets the local delta time.
        /// </summary>
        /// <value>
        /// The local delta time.
        /// </value>
        [HideInInspector]
        public float localDeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime * UnityEngine.Time.timeScale * _localTimeScale;
            }
        }

        /// <summary>
        /// Starts the time manipulation.
        /// </summary>
        /// <param name="type">The type.</param>
        public void StartTimeManipulation(int type)
        {
            _timeBeingManipulated = true;
            _manipulationType = (Manipulations)type;
            Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        /// <summary>
        /// Stops the time manipulation.
        /// </summary>
        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        private void Start()
        {
            XMovementControl = Resources.Load<AxisControl>("Managers/InputManager/XMovement");
            YMovementControl = Resources.Load<AxisControl>("Managers/InputManager/YMovement");
            Rbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            PlayerInfo = Resources.Load<PlayerInfo>("Player/DefaultPlayerInfo");
            InputRotation = Resources.Load<ScriptableObjectQuartenion>("Player/InputRotation");
        }

        private void Update()
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(Agent.transform.position, out hit, 0.1f, NavMesh.AllAreas);

            // Get our input
            Vector2 input = new Vector2(XMovementControl.Value, YMovementControl.Value);

            // if we are not allowed to move set input to 0,0
            if (!PlayerInfo.CanMove)
                input = Vector2.zero;


            Vector2 computedInput = input * localDeltaTime * PlayerInfo.MoveSpeed;
            computedInput = Vector2.ClampMagnitude(computedInput, localDeltaTime * PlayerInfo.MoveSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier);

            Animator.SetFloat("RunSpeed", computedInput.magnitude);

            Vector3 pos = transform.position;
            pos += new Vector3(computedInput.x, 0, computedInput.y);
            transform.position = pos;


            // The rigidbody keeps sliding all over the place when your dash in to things
            Rbody.velocity = Vector3.zero;

            // We dont want the player Rotating if we have no motion as it defaults to 0,0,0
            if (computedInput != Vector2.zero)
            {
                InputRotation.Value = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, InputRotation.Value, localDeltaTime * 9f);
            }
        }
    }
}