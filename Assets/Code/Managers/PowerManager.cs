using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.Managers
{
	[RequireComponent(typeof(PlayerController))]
	public class PowerManager : MonoBehaviour
	{
	    public static PowerManager Instance;

		[SerializeField]
		private List<PlayerPower> _powers;

		private PlayerController playerController;

        [SerializeField]
        private bool _powersEnabled = true;

		private void Start()
		{

			playerController = GetComponent<PlayerController>();

			foreach (var power in _powers)
				power.Init(playerController);
		}

		private void Update()
		{
			foreach (var power in _powers)
			{
				if (power.IsUnlocked)
					power.OnUpdate(playerController);
			}
		}

        public void SetPowersEnabled(bool enabled)
        {
            _powersEnabled = enabled;
        }

	    public void AddPower(PlayerPower powerToAdd, bool unlockedByDefault)
	    {
	        powerToAdd.IsUnlocked = unlockedByDefault;
            _powers.Add(powerToAdd);
	    }
	}
}