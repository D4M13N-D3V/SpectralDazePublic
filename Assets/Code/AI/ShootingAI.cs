using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Managers.AI;
using SpectralDaze.Etc;
using SpectralDaze.Managers;
using SpectralDaze.Managers.AudioManager;
using SpectralDaze.Player;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.AI
{

    /// <summary>
    /// Shooting AI Mono Behaviour
    /// </summary>
    /// <seealso cref="BaseAI" />
    public class ShootingAI : BaseAI
    {
        /// <summary>
        /// The renderer for the mesh of the AI
        /// </summary>
        public Renderer Renderer;

        /// <summary>
        /// The local time scale for calculating local delta time.
        /// </summary>
        [HideInInspector]
        public float _localTimeScale = 1.0f;
        /// <summary>
        /// The local time scale property.
        /// </summary>
        [HideInInspector]
        public float localTimeScale
        {
            get
            {
                return _localTimeScale;
            }
            set
            {
                float multiplier = value / _localTimeScale;
                paramsInstance.RigidBody.angularDrag *= multiplier;
                paramsInstance.RigidBody.drag *= multiplier;
                paramsInstance.RigidBody.mass /= multiplier;
                paramsInstance.RigidBody.velocity *= multiplier;
                paramsInstance.RigidBody.angularVelocity *= multiplier;

                _localTimeScale = value;
            }
        }
        /// <summary>
        /// The local delta time for the AI.
        /// </summary>
        [HideInInspector]
        public float localDeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime * UnityEngine.Time.timeScale * _localTimeScale;
            }
        }

        /// <summary>
        /// The options for the AI.
        /// </summary>
        public ShootingAIOptions Options;
        /// <summary>
        /// The state machine for running the AI.
        /// </summary>
        private UStateMachine<ShootingAIParams> stateMachine;
        /// <summary>
        /// The parameters passed to the state machine that holds information between states.
        /// </summary>
        private ShootingAIParams paramsInstance;

        /// <summary>
        /// The information about how to change the time based on if its being slowe down or sped up.
        /// </summary>
        public TimeInfo TimeInfo;
        /// <summary>
        /// Represents if time is being manipulated at this current moment or not.
        /// </summary>
        private bool _timeBeingManipulated;
        /// <summary>
        /// If the time is being manipulated this tells you what type of manipulation.
        /// </summary>
        private Manipulations _manipulationType;

        private void Start()
        {
            Setup();
            DeathSound = Options.DeathSound;
            GetAudioQueue();
            paramsInstance = new ShootingAIParams()
            {
                NpcTransform = transform,
                Npc = this,
                NavAgent = GetComponent<NavMeshAgent>(),
                Animator = GetComponent<Animator>(),
                OriginPosistion = transform.position,
                MovementType = Options.MovementType,
                WanderDistance = Options.WanderDistance,
                IdleTime = Options.IdleTime,
                TimeLeftIdle = Options.IdleTime,
                PatrolPoints = Options.PatrolPoints,
                CurrentPatrolPoint = Options.StartingPatorlPoint,
                Player = FindObjectOfType<PlayerController>(),
                AggroDistance = Options.AggroDistance,
                LoseAggroDistance = Options.LoseAggroDistance,
                RigidBody = GetComponent<Rigidbody>(),
                MovementSpeed = Options.MovementSpeed,
                TimeBetweenAttacks = Options.TimeBetweenAttacks,
                BulletPrefab = Options.BulletPrefab,
                ShootDelay = Options.ShootDelay,
                Renderer = Renderer,
                Chase = Options.Chase,
                ChaseDistance = Options.ChaseDistance,
                ShootingSound = Options.ShootingSound,
                DeathSound = Options.DeathSound
            };
            stateMachine = new UStateMachine<ShootingAIParams>(paramsInstance, new Chase(), new Attacking(), new Idle(), new Move());
            stateMachine.SetState(typeof(Idle), paramsInstance);
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
        }

        private void Update()
        {
            stateMachine.Update(paramsInstance);
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate(paramsInstance);
        }
        
        /// <summary>
        /// Starts time manipulation on the gameobject.
        /// </summary>
        /// <param name="type">The integer index of the Manipulations enum that represents the type of manipulation.</param>
        public void StartTimeManipulation(int type)
        {
            _timeBeingManipulated = true;
            _manipulationType = (Manipulations)type;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
        }

        /// <summary>
        /// Stops time manipulation on the gameobject.
        /// </summary>
        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
        }

        /// <summary>
        /// The Attacking state for the state machine.
        /// </summary>
        /// <seealso cref="ShootingAI.ShootingAIParams}" />
        private class Attacking : UState<ShootingAIParams>
        {
            /// <summary>
            /// How long until the next attack occurs.
            /// </summary>
            private float _timeLeftUntilAttack;
            /// <summary>
            /// Is a shot being charged?
            /// </summary>
            private bool _chargingShotInProgress;
            /// <summary>
            /// The original color of the AI
            /// </summary>
            private Color _originalColor;
            /// <summary>
            /// A variable used for calculations in how much time is left
            /// </summary>
            private float t = 0;
            /// <summary>
            /// The project currently being used that was shot recently.
            /// </summary>
            private GameObject _currentProjectile;
            /// <summary>
            /// The amount of time left in the shooting delay.
            /// </summary>
            private float _shootDelayLeft;

            /// <inheritdoc />
            public override void Enter(ShootingAIParams p)
            {
                _shootDelayLeft = p.ShootDelay;
                _originalColor = p.Renderer.material.color;
                p.NavAgent.isStopped = true;
                _timeLeftUntilAttack = p.TimeBetweenAttacks;
            }

            /// <inheritdoc />
            public override void FixedUpdate(ShootingAIParams p)
            {
                /*
                 * CODE FOR IMPLEMENTING AI DIRECTOR
                 */
                if (p.Npc.CurrentToken == null)
                {
                    p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                    p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                    p.Npc.RequestToken(AiDirector.TokenTypes.Shooting);
                    return;
                }

                _timeLeftUntilAttack -= p.Npc.localDeltaTime;
                if (_timeLeftUntilAttack <= 0)
                {
                    if (!_chargingShotInProgress)
                    {
                        if (p.Renderer.material.color != _originalColor)
                        {
                            t += p.Npc.localDeltaTime;
                            p.Renderer.material.color = Color.Lerp(p.Renderer.material.color, _originalColor, t);
                        }
                        else
                        {
                            t = 0;
                            _chargingShotInProgress = true;
                        }
                    }
                    else
                    {
                        if (p.Renderer.material.color == Color.red)
                        {
                            t = 0;
                            _shootDelayLeft -= p.Npc.localDeltaTime;
                            if (_shootDelayLeft <= 0)
                            {
                                CreateProjectile(p);
                                _timeLeftUntilAttack = p.TimeBetweenAttacks;
                                _chargingShotInProgress = false;
                                _shootDelayLeft = p.ShootDelay;
                                p.Npc.AudioQueue.Queue.Enqueue(p.ShootingSound);
                            }
                        }
                        else
                        {
                            t += p.Npc.localDeltaTime;
                            p.Renderer.material.color = Color.Lerp(p.Renderer.material.color, Color.red, t);
                        }
                    }
                }
                else
                {
                    if (p.Renderer.material.color != _originalColor)
                    {
                        t += p.Npc.localDeltaTime;
                        p.Renderer.material.color = Color.Lerp(p.Renderer.material.color, _originalColor, t);
                    }
                    p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                    p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(ShootingAIParams p)
            {
                if (!p.Chase && p.LoseAggroDistance <= Vector3.Distance(p.NpcTransform.position, p.Player.transform.position))
                    Parent.SetState(typeof(Idle), p);
                if (p.Chase && p.LoseAggroDistance <= Vector3.Distance(p.NpcTransform.position, p.Player.transform.position))
                    Parent.SetState(typeof(Chase), p);
            }

            /// <summary>
            /// Create the projectile that is shot.
            /// </summary>
            /// <param name="p">The parameters from the state machine.</param>
            private void CreateProjectile(ShootingAIParams p)
            {
                _currentProjectile = Instantiate(p.BulletPrefab, (p.NpcTransform.position + p.NpcTransform.forward), Quaternion.Euler(p.NpcTransform.eulerAngles));
                _currentProjectile.transform.eulerAngles = new Vector3(0, _currentProjectile.transform.eulerAngles.y, 0);
                _currentProjectile.transform.position = p.NpcTransform.position + p.NpcTransform.transform.forward + p.NpcTransform.transform.up;
                _currentProjectile.GetComponent<Bullet>().Source = p.Npc.gameObject;

            }
        }

        /// <summary>
        /// The Chase state for the state machine.
        /// </summary>
        /// <seealso cref="ShootingAI.ShootingAIParams}" />
        private class Chase : UState<ShootingAIParams>
        {
            /// <summary>
            /// The amount of idle time left before the next move.
            /// </summary>
            private float _idleTimeLeft = 0;

            /// <inheritdoc />
            public override void Enter(ShootingAIParams p)
            {
                if (p.Npc.CurrentToken != null)
                    p.Npc.ReturnToken();
            }

            /// <inheritdoc />
            public override void FixedUpdate(ShootingAIParams p)
            {
                p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);

                _idleTimeLeft -= p.Npc.localDeltaTime;
                if (_idleTimeLeft <= 0)
                {
                    p.NavAgent.isStopped = false;
                    Vector3 randomOffset = Random.insideUnitSphere * p.AggroDistance/2;
                    Vector3 randomWanderPosistion = randomOffset += p.Player.transform.position;

                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.AggroDistance/2;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;
                    }
                    if (p.NavAgent.SetDestination(randomWanderPosistion))
                    {
                        _idleTimeLeft = p.IdleTime / 2;
                    }
                    else
                    {
                        Debug.LogError("Error occured when setting destination of navmesh agent.");
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(ShootingAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                    Parent.SetState(typeof(Attacking), p);

                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) >= p.ChaseDistance)
                    Parent.SetState(typeof(Move), p);
            }
        }

        /// <summary>
        /// The idle state for the state machine.
        /// </summary>
        /// <seealso cref="ShootingAI.ShootingAIParams}" />
        private class Idle : UState<ShootingAIParams>
        {
            /// <summary>
            /// The amount of time before the next action.
            /// </summary>
            private float _timeLeftIdle = 0;

            /// <inheritdoc />
            public override void Enter(ShootingAIParams p)
            {
                if (p.Npc.CurrentToken != null)
                    p.Npc.ReturnToken();
                _timeLeftIdle = p.IdleTime;
            }

            /// <inheritdoc />
            public override void FixedUpdate(ShootingAIParams p)
            {
                if (!p.NavAgent.pathPending && p.NavAgent.remainingDistance <= p.NavAgent.stoppingDistance &&
                    !p.NavAgent.hasPath || p.NavAgent.velocity.sqrMagnitude == 0f)
                {
                    if (_timeLeftIdle > 0)
                    {
                        _timeLeftIdle = _timeLeftIdle - UnityEngine.Time.deltaTime;
                    }
                    else
                    {
                        Parent.SetState(typeof(Move), p);
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(ShootingAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        /// <summary>
        /// The move state for the State Machine.
        /// </summary>
        /// <seealso cref="ShootingAI.ShootingAIParams}" />
        private class Move : UState<ShootingAIParams>
        {
            /// <inheritdoc />
            public override void Enter(ShootingAIParams p)
            {
                if (p.Npc.CurrentToken != null)
                    p.Npc.ReturnToken();
                p.NavAgent.isStopped = false;
                if (p.MovementType == MovementType.Wander || p.MovementType == MovementType.NoLimitsWander)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * p.WanderDistance;
                    Vector3 randomWanderPosistion = randomOffset += p.NpcTransform.position;


                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.WanderDistance;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;

                        if (p.MovementType == MovementType.Wander)
                            while (Vector3.Distance(randomWanderPosistion, p.OriginPosistion) > p.WanderDistance)
                            {
                                randomOffset = Random.insideUnitSphere * p.WanderDistance;
                                randomWanderPosistion = randomOffset += p.NpcTransform.position;
                            }
                    }
                    if (p.NavAgent.SetDestination(randomWanderPosistion))
                    {
                        Parent.SetState(typeof(Idle), p);
                    }
                    else
                    {
                        Debug.LogError("Error occured when setting destination of navmesh agent.");
                    }
                }
                else if (p.MovementType == MovementType.Patrol)
                {
                    p.CurrentPatrolPoint++;
                    if (p.CurrentPatrolPoint > p.PatrolPoints.Count - 1)
                    {
                        p.CurrentPatrolPoint = 0;
                    }
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(p.PatrolPoints[p.CurrentPatrolPoint], out hit, p.WanderDistance, NavMesh.AllAreas))
                    {
                        if (p.NavAgent.SetDestination(p.PatrolPoints[p.CurrentPatrolPoint]))
                        {
                            Parent.SetState(typeof(Idle), p);
                        }
                        else
                        {
                            Debug.LogError("Error occured when setting destination of navmesh agent.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when sampling posistion to get nearest navmesh point.");
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(ShootingAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        /// <summary>
        /// The different types of movement.
        /// </summary>
        public enum MovementType
        {
            Patrol,
            Wander,
            NoLimitsWander
        }

        /// <summary>
        /// Struct for the parameters for the shooting AI state machine.
        /// </summary>
        private struct ShootingAIParams
        {
            /// <summary>
            /// The NPC transform
            /// </summary>
            public Transform NpcTransform;
            /// <summary>
            /// The NPC
            /// </summary>
            public ShootingAI Npc;
            /// <summary>
            /// The nav agent
            /// </summary>
            public NavMeshAgent NavAgent;
            /// <summary>
            /// The animator
            /// </summary>
            public Animator Animator;
            /// <summary>
            /// The origin posistion
            /// </summary>
            public Vector3 OriginPosistion;
            /// <summary>
            /// The movement type
            /// </summary>
            public MovementType MovementType;
            /// <summary>
            /// The distance that the AI can wander.
            /// </summary>
            public float WanderDistance;
            /// <summary>
            /// The idle time
            /// </summary>
            public float IdleTime;
            /// <summary>
            /// The time left in the idle step
            /// </summary>
            public float TimeLeftIdle;
            /// <summary>
            /// The current patrol point
            /// </summary>
            public int CurrentPatrolPoint;
            /// <summary>
            /// The patrol points
            /// </summary>
            public List<Vector3> PatrolPoints;
            /// <summary>
            /// The cached target
            /// </summary>
            public GameObject CachedTarget;
            /// <summary>
            /// The player controller
            /// </summary>
            public PlayerController Player;
            /// <summary>
            /// The distance that the AI becomes aggressive.
            /// </summary>
            public float AggroDistance;
            /// <summary>
            /// The distance which the AI looses aggression.
            /// </summary>
            public float LoseAggroDistance;
            /// <summary>
            /// The rigid body
            /// </summary>
            public Rigidbody RigidBody;
            /// <summary>
            /// The movement speed
            /// </summary>
            public float MovementSpeed;
            /// <summary>
            /// The prefab for the projectile being fired.
            /// </summary>
            public GameObject BulletPrefab;
            /// <summary>
            /// The time between attacks
            /// </summary>
            public float TimeBetweenAttacks;
            /// <summary>
            /// The renderer for the AI
            /// </summary>
            public Renderer Renderer;
            /// <summary>
            /// Represents if the AI is being chased
            /// </summary>
            public bool Chase;
            /// <summary>
            /// The distance where the AI stops chasing
            /// </summary>
            public float ChaseDistance;
            /// <summary>
            /// The delay between shots.
            /// </summary>
            public float ShootDelay;
            /// <summary>
            /// The shooting sound
            /// </summary>
            public AudioClipInfo ShootingSound;
            /// <summary>
            /// The death sound
            /// </summary>
            public AudioClipInfo DeathSound;
        }
    }
}
