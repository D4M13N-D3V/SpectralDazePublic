using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Time.Information
{
    public class Information
    {
        public float AnimationModifier
        {
            get { return AnimationModifier; }
            set
            {
                if (value > 1 || value<=0 )
                {
                    AnimationModifier = 1;
                }
                else
                {
                    AnimationModifier = value;
                }
            }
        }

        public float PhysicsModifier
        {
            get { return PhysicsModifier; }
            set
            {
                if (value > 1 || value<=0 )
                {
                    PhysicsModifier = 1;
                }
                else
                {
                    PhysicsModifier = value;
                }
            }
        }

        public float MovementModifier
        {
            get { return MovementModifier; }
            set
            {
                if (value > 1 || value<=0 )
                {
                    MovementModifier = 1;
                }
                else
                {
                    MovementModifier = value;
                }
            }
        }
    }
}