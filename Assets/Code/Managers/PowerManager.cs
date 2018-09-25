using System;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Player;
using SpectralDaze.Managers;
using SpectralDaze.Managers.InputManager;
using UnityEngine;

namespace SpectralDaze.Managers.PowerManager
{
    /// <summary>
    /// Manages the players powers.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PowerManager : MonoBehaviour
	{
        /// <summary>
        /// Scriptable object that holds information for first power slot.
        /// </summary>
        public Power1 Power1;
	    /// <summary>
	    /// Scriptable object that holds information for second power slot.
	    /// </summary>
	    public Power2 Power2;
	    /// <summary>
	    /// Scriptable object that holds information for dash power slot.
	    /// </summary>
        public DashPower DashPower;

	    private PlayerPower _power1Cache;
	    private PlayerPower _power2Cache;
	    private PlayerPower_Dash _dashCache;

        /// <summary>
        /// Scriptable object holding all the information about what powers the player has access to.
        /// </summary>
        public PowerInventory PowerInventory;
	    private PlayerController pc;

        /// <summary>
        /// The control for the first power
        /// </summary>
        public Control Power1Control;
        /// <summary>
        /// The control for the second power
        /// </summary>
        public Control Power2Control;
        /// <summary>
        /// The control for the dash power
        /// </summary>
        public Control DashControl;

	    /// <inheritdoc />
        private void Start()
	    {
	        pc = GetComponent<PlayerController>();

	        Power2 = Resources.Load<Power2>("Managers/PowerManager/Power2");
	        DashPower = Resources.Load<DashPower>("Managers/PowerManager/DashPower");

	        PowerInventory = Resources.Load<PowerInventory>("Managers/PowerManager/PowerInventory");
            Power1Control = Resources.Load<Control>("Managers/InputManager/Power1");
	        Power2Control = Resources.Load<Control>("Managers/InputManager/Power2");
	        DashControl = Resources.Load<Control>("Managers/InputManager/Dash");

	        Power2.Power = null;

	        if (Power1 != null)
	        {
	            Power1.Power.Init(pc);
	            Power1.Power.Control = Power1Control;
	        }

	        if (Power2 != null && Power2.Power !=null)
	        {
	            Power2.Power.Init(pc);
	            Power2.Power.Control = Power2Control;
            }

	        if (DashPower != null && DashPower.Power != null)
	        {
	            DashPower.Power.Init(pc);
	            DashPower.Power.Control = DashControl;
	        }
	    }

	    /// <inheritdoc />
        private void Update()
	    {

	        if (Power1 != null)
	        {
	            if (Power1 != _power1Cache)
	                Power1.Power.Init(pc);
                _power1Cache = Power1.Power;
	            Power1.Power.OnUpdate(pc);
            }

	        if (Power2.Power != null)
	        {
	            if (Power2.Power != _power2Cache)
	                Power2.Power.Init(pc);
                _power2Cache = Power2.Power;
	            Power2.Power.OnUpdate(pc);
            }

	        if (DashPower.Power != null)
	        {
	            if (DashPower.Power != _dashCache)
	                DashPower.Power.Init(pc);
                _dashCache = DashPower.Power;
	            DashPower.Power.OnUpdate(pc);
            }
        }

	    /// <inheritdoc />
        private void OnGUI()
	    {
	        if (Power1 != null)
                Power1.Power.OnGUI();
	        if (Power2.Power != null)
                Power2.Power.OnGUI();
	        if (DashPower.Power != null)
                DashPower.Power.OnGUI();
        }

	    /// <inheritdoc />
        private void OnDrawGizmos()
		{
			if (pc == null)
				return;
			if (Power1 != null)
				Power1.Power.OnGizmos(pc);
		}
	}
}