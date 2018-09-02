using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gmtk.AI
{
	public class AIState_Chase : UState<AIStateParams>
	{
		private float memoryTimer = 0f;
		private const float MAXIMUM_MEMORY = 10f;
		private float idleTimer = 0f;
		private const float IDLE_TIME = 3f;

		private Vector3 cachedPosition;

		public override void Enter(AIStateParams ps)
		{
			// Start moving towards the cached enemy
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

			if (CanSeePlayer(ps))
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

		private bool CanSeePlayer(AIStateParams ps)
		{
			Vector3 dir = (ps.Controller.CachedTarget.position - ps.Transform.position).normalized;
			Ray ray = new Ray(ps.Transform.position, dir);
			RaycastHit hit;

			if (!Physics.Raycast(ray, out hit)) return false;

			if (hit.distance > 10f)
				return false;

			return hit.transform == ps.Controller.CachedTarget || hit.transform.IsChildOf(ps.Controller.CachedTarget);
		}
	}
}