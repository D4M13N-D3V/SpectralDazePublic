using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.Managers
{
	[RequireComponent(typeof(PlayerController))]
	public class PowerManagerNew : MonoBehaviour
	{
		[SerializeField]
		private PlayerPower[] powers;

		private PlayerController playerController;

		private void Start()
		{
			playerController = GetComponent<PlayerController>();

			foreach (var power in powers)
				power.Init(playerController);
		}

		private void Update()
		{
			foreach (var power in powers)
			{
				if (power.IsUnlocked)
					power.OnUpdate(playerController);
			}
		}
	}
}