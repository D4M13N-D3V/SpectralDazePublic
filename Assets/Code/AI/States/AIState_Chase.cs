using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.AI
{
	public class AIState_Chase : UState<AIStateParams>
	{
		private float memoryTimer = 0f;
		private const float MAXIMUM_MEMORY = 10f;
		private float idleTimer = 0f;
		private const float IDLE_TIME = 3f;

		private const float ATK_DIST = 8f;

		private Vector3 cachedPosition;

		public override void Enter(AIStateParams ps)
		{
			// Start moving towards the cached enemy
			ps.NavMeshAgent.isStopped = false;
			ps.NavMeshAgent.SetDestination(ps.Controller.CachedTarget.transform.position);
		}

		public override void Update(AIStateParams ps)
		{
			if (cachedPosition != Vector3.zero)
			{
				
			}

			cachedPosition = ps.Transform.position;

			if (ps.Controller.CachedTarget == null)
			{
				Parent.SetState(typeof(AIState_Idle), ps);
				return;
			}

			float distanceToPlayer = Vector3.Distance(ps.Controller.transform.position, ps.Controller.CachedTarget.position);

			if (distanceToPlayer <= ATK_DIST)
			{
				// Go ahead and change over to attack charge
				ps.NavMeshAgent.isStopped = true;
				Parent.SetState(typeof(AIState_AttackCharge), ps);
				return;
			}

			if (ps.CanSeePlayer())
				memoryTimer = 0f;
			else
				memoryTimer += Time.deltaTime;

			// If we can still see the player(or if they're in recent memory), continue moving towards their position.
			// If not, go ahead and stop for a second until we transition back into idle.
			if (memoryTimer < MAXIMUM_MEMORY)
			{
				ps.NavMeshAgent.SetDestination(ps.Controller.CachedTarget.transform.position);
				idleTimer = 0f;
			}
			else
			{
				ps.NavMeshAgent.isStopped = true;
				idleTimer += Time.deltaTime;
				if (idleTimer > IDLE_TIME)
				{
					Parent.SetState(typeof(AIState_Idle), ps);
					return;
				}
			}
		}

		public override void Exit(AIStateParams ps)
		{
			memoryTimer = 0f;
			idleTimer = 0f;
		}

		public override void OnDrawGizmos(AIStateParams ps)
		{
			using(new GizmoColorScope(Color.red))
				Gizmos.DrawWireSphere(ps.Transform.position, ATK_DIST);
		}
	}
}