using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

public class WispController : MonoBehaviour
{
	private ParticleSystem ps;

	private NativeArray<Particle> particles;
	private NativeArray<Particle> particleOutputBuffer;
	private JobHandle handle;

	private const float NEIGHBOR_RADIUS = 10f;

	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
		particles = new NativeArray<Particle>(5 * 5 * 5, Allocator.Persistent);
		SetParticles();
	}

	private void Update()
	{
		particleOutputBuffer = new NativeArray<Particle>(particles.Length, Allocator.TempJob);
		SteerBoidsJob steerJob = new SteerBoidsJob()
		{
			BaseForward = transform.forward,
			BasePosition = transform.position,
			DeltaTime = Time.deltaTime,
			Particles = particles,
			ParticleOut = particleOutputBuffer
		};

		handle = steerJob.Schedule(particles.Length, 32);
	}

	private void LateUpdate()
	{
		handle.Complete();

		particles.CopyFrom(particleOutputBuffer);
		particleOutputBuffer.Dispose();

		ps.SetParticles(particles.ToArray(), particles.Length);
	}

	private void OnDisable()
	{
		particles.Dispose();
		particleOutputBuffer.Dispose();
	}

	private void SetParticles()
	{
		int i = 0;
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				for (int z = 0; z < 5; z++)
				{
					particles[i] = new Particle()
					{
						position = new Vector3(x, y, z) * 0.05f,
						startSize = 0.1f,
						startColor = Color.cyan
					};
					i++;
				}
			}
		}

		ps.SetParticles(particles.ToArray(), particles.Length);
	}

	private struct SteerBoidsJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<Particle> Particles;

		[WriteOnly]
		public NativeArray<Particle> ParticleOut;

		public Vector3 BaseForward, BasePosition;

		public float DeltaTime;

		public void Execute(int index)
		{
			Vector3 separation = Vector3.zero;
			Vector3 alignment = Vector3.zero;
			Vector3 cohesion = BasePosition;

			Particle self = Particles[index];

			int neighborCount = 0;
			for (int i = 0; i < Particles.Length; i++)
			{
				if (i == index)
					continue;

				Particle neighbor = Particles[i];

				float dist = math.distance(self.position, neighbor.position);
				if (dist > NEIGHBOR_RADIUS)
					continue;

				// Apply influence from this neighbor
				separation += GetSeparationFloat3(self, neighbor);
				alignment += (Quaternion.Euler(neighbor.rotation3D) * Vector3.forward);
				cohesion += neighbor.position;

				neighborCount++;
			}

			// Normalize
			float div = 1f / (neighborCount + 1);
			alignment *= div;
			cohesion = (cohesion * div - self.position).normalized;

			cohesion += (BasePosition - self.position) / 2f;

			// Calc target direction
			Vector3 steering = separation + (alignment*0.666f) + cohesion;
			Vector3 desiredVelocity = (BasePosition - self.position).normalized;
			steering = steering + desiredVelocity;
			Vector3 rotation = Quaternion.LookRotation(steering.normalized).eulerAngles;

			if (self.rotation3D != rotation)
			{
				float ip = math.exp(-4 * DeltaTime);
				rotation = Vector3.Slerp(rotation, self.rotation3D, ip);

				// Apply rot
				self.rotation3D = rotation;
			}

			self.position = self.position + ((Quaternion.Euler(self.rotation3D) * Vector3.forward) * DeltaTime);

			ParticleOut[index] = self;
		}

		private Vector3 GetSeparationFloat3(Particle a, Particle b)
		{
			Vector3 diff = b.position - a.position;
			float diffLen = math.length(diff);
			float scalar = Mathf.Clamp01(1 - diffLen / NEIGHBOR_RADIUS);
			return diff * scalar / diffLen;
		}
	}
}