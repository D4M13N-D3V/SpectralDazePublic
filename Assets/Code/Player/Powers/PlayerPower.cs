using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers.InputManager;
using UnityEngine;

namespace SpectralDaze.Player
{
    public enum PowerTypes { Dash, Power }

	public class PlayerPower : ScriptableObject
	{
        /// <summary>
        /// The icon for the power
        /// </summary>
        public Texture2D Icon;
        /// <summary>
        /// The name of power
        /// </summary>
        public string Name;

        /// <summary>
        /// The type of power
        /// </summary>
        public PowerTypes Type;

        public Control Control;

		public bool IsUnlocked = false;

        /// <summary>
        /// Initializes the power.
        /// </summary>
        /// <param name="pc">The player controller.</param>
        public virtual void Init(PlayerController pc) { }
        /// <summary>
        /// Called every frame.
        /// </summary>
        /// <param name="pc">The player controller.</param>
        public virtual void OnUpdate(PlayerController pc) { }
        /// <summary>
        /// Called every gui event.
        /// </summary>
        public virtual void OnGUI() { }
        /// <summary>
        /// Draws gizmo
        /// </summary>
        /// <param name="pc">The player controller.</param>
        public virtual void OnGizmos(PlayerController pc) { }
        /// <summary>
        /// Resets this power.
        /// </summary>
        public virtual void Reset() { }
	}
}