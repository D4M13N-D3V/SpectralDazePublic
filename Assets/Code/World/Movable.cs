using System;
using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Camera;
using UnityEngine;

namespace SpectralDaze.World
{
    /// <summary>
    /// A object that can be moved around.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Movable : MonoBehaviour
    {
        /// <summary>
        /// The posistions that the movable object can be move dto.
        /// </summary>
        public List<MovablePosistion> Posistions;
        /// <summary>
        /// The starting posistion
        /// </summary>
        public int StartingPosistion = 0;
        /// <summary>
        /// The current posistion
        /// </summary>
        private int _currentPosistion = 0;

        private void Start()
        {
            gameObject.tag = "Movable";
            _currentPosistion = StartingPosistion;
            transform.position = Posistions[_currentPosistion].Posistion.position;
        }

        /// <summary>
        /// Called when the object is hit.
        /// </summary>
        /// <param name="transformHitting">The transform hitting.</param>
        public void Hit(Transform transformHitting)
        {
            var xoffset = transform.position.x - transformHitting.position.x + 0.5f;
            var zoffset = transform.position.z - transformHitting.position.z + 0.5f;
            MovablePosistion.Directions hittingDir = MovablePosistion.Directions.Unknown;

            Debug.Log(GetComponent<Collider>().bounds.size.x / 2 + transformHitting.GetComponent<Collider>().bounds.size.x);
            Debug.Log(xoffset);

            if (xoffset > GetComponent<Collider>().bounds.size.x / 2 - transformHitting.GetComponent<Collider>().bounds.size.x)
                hittingDir = MovablePosistion.Directions.Left;
            else if (xoffset < -GetComponent<Collider>().bounds.size.x / 2 + transformHitting.GetComponent<Collider>().bounds.size.x)
                hittingDir = MovablePosistion.Directions.Right;
            else if (zoffset > GetComponent<Collider>().bounds.size.z / 2 - transformHitting.GetComponent<Collider>().bounds.size.z)
                hittingDir = MovablePosistion.Directions.Down;
            else if (zoffset < -GetComponent<Collider>().bounds.size.z / 2 + transformHitting.GetComponent<Collider>().bounds.size.z)
                hittingDir = MovablePosistion.Directions.Up;

            if (hittingDir == MovablePosistion.Directions.Unknown)
            {
                return;
            }


            Debug.Log(hittingDir);

            if (hittingDir == Posistions[_currentPosistion].DirectionForward && _currentPosistion + 1 <= Posistions.Count - 1 &&
                Posistions[_currentPosistion + 1].DirectionBackward == Posistions[_currentPosistion].DirectionForward)
            {
                _currentPosistion++;
                LeanTween.move(gameObject, Posistions[_currentPosistion].Posistion.position,
                    Posistions[_currentPosistion].TimeToMove).setOnComplete(() =>
                    {
                        UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.05f, 0.2f);
                        UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().FOVKick(2.0f, 0.2f);
                    });
            }
            else if (hittingDir == GetOppisiteDirection(Posistions[_currentPosistion].DirectionBackward) && _currentPosistion - 1 >= 0 &&
                     Posistions[_currentPosistion - 1].DirectionForward == Posistions[_currentPosistion].DirectionBackward)
            {
                _currentPosistion--;
                LeanTween.move(gameObject, Posistions[_currentPosistion].Posistion.position,
                    Posistions[_currentPosistion].TimeToMove).setOnComplete(() =>
                    {
                        UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.05f, 0.2f);
                        UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().FOVKick(3.0f, 0.2f);
                    });
            }



        }

        /// <summary>
        /// Gets the oppisite direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The oppisite direction</returns>
        private MovablePosistion.Directions GetOppisiteDirection(MovablePosistion.Directions direction)
        {
            switch (direction)
            {
                case MovablePosistion.Directions.Left:
                    return MovablePosistion.Directions.Right;
                case MovablePosistion.Directions.Right:
                    return MovablePosistion.Directions.Left;
                case MovablePosistion.Directions.Down:
                    return MovablePosistion.Directions.Up;
                case MovablePosistion.Directions.Up:
                    return MovablePosistion.Directions.Down;
                default: return MovablePosistion.Directions.Unknown;
            }
        }

        /// <summary>
        /// A posistion information that the moveable object can be moved to.
        /// </summary>
        [Serializable]
        public class MovablePosistion
        {
            /// <summary>
            /// The directions that can be hit/move.
            /// </summary>
            public enum Directions { Left, Right, Up, Down, Unknown }
            /// <summary>
            /// The posistion of the movable posistion
            /// </summary>
            public Transform Posistion;
            /// <summary>
            /// The side that the moveable object needs to be hit on to move it forward
            /// </summary>
            public Directions DirectionForward = Directions.Right;
            /// <summary>
            /// The side that the moveable object needs to be hit on to move it backward
            /// </summary>
            public Directions DirectionBackward = Directions.Right;
            /// <summary>
            /// How long it takes to move between the points.
            /// </summary>
            public float TimeToMove = 2f;
        }
    }

}