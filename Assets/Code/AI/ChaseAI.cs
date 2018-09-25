using System.Collections.Generic;
using System.Linq;
using Managers;
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
    /// Chase AI Mono Behaviour
    /// </summary>
    /// <seealso cref="Assets.Code.AI.BaseAI" />
    public class ChaseAI : BaseAI
    {
        /// <summary>
        /// The local time scale for calculatnig local delta time.
        /// </summary>
        public float _localTimeScale = 1.0f;
        /// <summary>
        /// The local time scale property.
        /// </summary>
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
        /// The local delta time based on the local time scale.
        /// </summary>
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
        public ChaseAIOptions Options;
        /// <summary>
        /// The state machine for the AI.
        /// </summary>
        private UStateMachine<RushAIParams> stateMachine;
        /// <summary>
        /// The parameters instance for the state machine.
        /// </summary>
        private RushAIParams paramsInstance;

        /// <summary>
        /// The time information for values and what they should be based on manipulation type.
        /// </summary>
        public TimeInfo TimeInfo;
        /// <summary>
        /// Is the time being manipulated
        /// </summary>
        private bool _timeBeingManipulated;
        /// <summary>
        /// The manipulation type of time.
        /// </summary>
        private Manipulations _manipulationType;

        private void Start()
        {
            Setup();
            DeathSound = Options.DeathSound;
            GetAudioQueue();
            paramsInstance = new RushAIParams()
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
                CurrentPatrolPoint = Options.StartingPatrolPoint,
                Player = FindObjectOfType<PlayerController>(),
                AggroDistance = Options.AggroDistance,
                TimeBetweenCharges = Options.TimeBetweenCharges,
                RigidBody = GetComponent<Rigidbody>(),
                LaunchVelocity = Options.LaunchVelocity,
                MovementSpeed = Options.MovementSpeed,
                Chase = Options.Chase,
                ChaseDistance = Options.ChaseDistance,
                DeathSound = Options.DeathSound
            };
            stateMachine = new UStateMachine<RushAIParams>(paramsInstance, new Chase(), new Attacking(), new Idle(), new Move());
            stateMachine.SetState(typeof(Idle), paramsInstance);
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed * TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
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
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        /// <summary>
        /// Stops time manipulation on the gameobject.
        /// </summary>
        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed*TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            paramsInstance.MovementModifier = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            _localTimeScale = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.PhysicsModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        /// <summary>
        /// The Chase state for the state machine.
        /// </summary>
        private class Chase : UState<RushAIParams>
        {
            /// <summary>
            /// Time left until next step.
            /// </summary>
            private float _idleTimeLeft = 0;

            /// <inheritdoc />
            public override void Enter(RushAIParams p)
            {
                if (p.Npc.CurrentToken != null)
                    p.Npc.ReturnToken();
            }

            /// <inheritdoc />
            public override void FixedUpdate(RushAIParams p)
            {
                p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);

                _idleTimeLeft -= p.Npc.localDeltaTime;
                if (_idleTimeLeft <= 0)
                {
                    p.NavAgent.isStopped = false;
                    Vector3 randomOffset = Random.insideUnitSphere * p.AggroDistance / 2;
                    Vector3 randomWanderPosistion = randomOffset += p.Player.transform.position;

                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.AggroDistance / 2;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;
                    }
                    if (p.NavAgent.SetDestination(randomWanderPosistion))
                    {
                        _idleTimeLeft = p.IdleTime/2;
                    }
                    else
                    {
                        Debug.LogError("Error occured when setting destination of navmesh agent.");
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(RushAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                    Parent.SetState(typeof(Attacking), p);

                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) >= p.ChaseDistance)
                    Parent.SetState(typeof(Move), p);
            }
        }

        /// <summary>
        /// The Attacking state for the state machine.
        /// </summary>
        private class Attacking : UState<RushAIParams>
        {
            /// <summary>
            /// The cooldown time left until charges.
            /// </summary>
            private float _remainderChargeCooldown;
            /// <summary>
            /// Represents if the AI is currently charging.
            /// </summary>
            private bool _chargeInProgress = false;
            /// <summary>
            /// Timr left in current charge.
            /// </summary>
            private float _remainderOfCurrentCharge = 1.5f;


            /// <inheritdoc />
            public override void Enter(RushAIParams p)
            {
                p.NavAgent.isStopped = true;
                _remainderChargeCooldown = p.TimeBetweenCharges;
            }

            /// <inheritdoc />
            public override void Update(RushAIParams p)
            {

                if (p.Npc.CurrentToken == null)
                {
                    p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                    p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                    p.Npc.RequestToken(AiDirector.TokenTypes.Rushing);
                    return;
                }
                _remainderChargeCooldown -= p.Npc.localDeltaTime;
                if (_remainderChargeCooldown <= 0 && !_chargeInProgress)
                {
                    p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                    p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                    _chargeInProgress = true;
                    p.RigidBody.velocity= p.NpcTransform.forward * p.LaunchVelocity * p.MovementModifier;
                }
                else if (_remainderChargeCooldown > 0 && !_chargeInProgress)
                {
                    p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                    p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                }

                if (_chargeInProgress)
                {
                    _remainderOfCurrentCharge -= p.Npc.localDeltaTime;
                    if (_remainderOfCurrentCharge <= 0)
                    {
                        _remainderChargeCooldown = p.TimeBetweenCharges;
                        _chargeInProgress = false;
                        _remainderOfCurrentCharge = 1.5f;
                    }

                    p.NpcTransform.position += p.NpcTransform.forward * p.Npc.localDeltaTime * p.MovementSpeed*2;
                }


            }

            /// <inheritdoc />
            public override void CheckForTransitions(RushAIParams p)
            {
                if (!p.Chase && p.LoseAggroDistance >= Vector3.Distance(p.NpcTransform.position, p.Player.transform.position))
                    Parent.SetState(typeof(Move), p);
                if (p.Chase && p.LoseAggroDistance >= Vector3.Distance(p.NpcTransform.position, p.Player.transform.position))
                    Parent.SetState(typeof(Chase), p);
            }
        }

        /// <summary>
        /// The Idle state for the state machine
        /// </summary>
        private class Idle : UState<RushAIParams>
        {
            /// <summary>
            /// The amount of time left in the idle.
            /// </summary>
            private float _timeLeftIdle = 0;

            /// <inheritdoc />
            public override void Enter(RushAIParams p)
            {
                if(p.Npc.CurrentToken!=null)
                    p.Npc.ReturnToken();
                _timeLeftIdle = p.IdleTime;
            }

            /// <inheritdoc />
            public override void FixedUpdate(RushAIParams p)
            {
                if (!p.NavAgent.pathPending && p.NavAgent.remainingDistance <= p.NavAgent.stoppingDistance &&
                    !p.NavAgent.hasPath || p.NavAgent.velocity.sqrMagnitude == 0f)
                {
                    if (_timeLeftIdle > 0)
                    {
                        _timeLeftIdle = _timeLeftIdle - p.Npc.localDeltaTime;
                    }
                    else
                    {
                        Parent.SetState(typeof(Move), p);
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(RushAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        /// <summary>
        /// The move state for the state machine
        /// </summary>
        private class Move : UState<RushAIParams>
        {
            /// <inheritdoc />
            public override void Enter(RushAIParams p)
            {
                if (p.Npc.CurrentToken != null)
                    p.Npc.ReturnToken();
                p.NavAgent.isStopped = false;
                if (p.MovementType == MovementType.Wander || p.MovementType == MovementType.NoLimitsWander)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * p.WanderDistance;
                    Vector3 randomWanderPosistion = randomOffset += p.NpcTransform.position;


                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false && Vector3.Distance(randomWanderPosistion, p.OriginPosistion) > p.WanderDistance || NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.WanderDistance;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;
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
            public override void CheckForTransitions(RushAIParams p)
            {
                if (Vector3.Distance(p.Player.transform.position, p.NpcTransform.position) <= p.AggroDistance)
                {
                    Parent.SetState(typeof(Attacking), p);
                }
            }
        }

        /// <summary>
        /// Types of movement.
        /// </summary>
        public enum MovementType
        {
            Patrol,
            Wander,
            NoLimitsWander
        }

        /// <summary>
        /// Parameter struct to hold the information passed to state machine.
        /// </summary>
        private struct RushAIParams
        {
            /// <summary>
            /// The NPC transform
            /// </summary>
            public Transform NpcTransform;
            /// <summary>
            /// The NPC's class instance
            /// </summary>
            public ChaseAI Npc;
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
            /// The time for the AI to idle between actions
            /// </summary>
            public float IdleTime;
            /// <summary>
            /// The time left in the idle
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
            /// The player controller for hte player
            /// </summary>
            public PlayerController Player;
            /// <summary>
            /// The distance that the AI becomes aggressive
            /// </summary>
            public float AggroDistance;
            /// <summary>
            /// The distance that the AI looses aggression.
            /// </summary>
            public float LoseAggroDistance;
            /// <summary>
            /// The time between charges
            /// </summary>
            public float TimeBetweenCharges;
            /// <summary>
            /// The movement speed modifier
            /// </summary>
            public float MovementModifier;
            /// <summary>
            /// The rigid body
            /// </summary>
            public Rigidbody RigidBody;
            /// <summary>
            /// The launch velocity
            /// </summary>
            public float LaunchVelocity;
            /// <summary>
            /// The movement speed
            /// </summary>
            public float MovementSpeed;
            /// <summary>
            /// Is the AI chasing.
            /// </summary>
            public bool Chase;
            /// <summary>
            /// The distance the AI stops chasing.
            /// </summary>
            public float ChaseDistance;
            /// <summary>
            /// The death sound
            /// </summary>
            public AudioClipInfo DeathSound;
        }
    }
}
