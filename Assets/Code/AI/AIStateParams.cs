using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace gmtk.AI
{
	public struct AIStateParams
	{
		public Transform Transform;
		public AIController Controller;
		public NavMeshAgent NavMeshAgent;
	}
}