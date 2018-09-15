using System;
using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Camera;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public List<MovablePosistion> Posistions;
    public int StartingPosistion = 0;
    private int _currentPosistion = 0;

    private void Start()
    {
        gameObject.tag = "Movable";
        _currentPosistion = StartingPosistion;
        transform.position = Posistions[_currentPosistion].Posistion.position;
    }

    public void Hit(Transform transformHitting) 
    {
        var xoffset = transform.position.x - transformHitting.position.x;
        var zoffset = transform.position.z - transformHitting.position.z;
        MovablePosistion.Directions hittingDir = MovablePosistion.Directions.Unknown;
        if (xoffset+1.5f < transformHitting.GetComponent<Collider>().bounds.size.x / 2 + transformHitting.GetComponent<Collider>().bounds.size.x)
            hittingDir = MovablePosistion.Directions.Right;
        else if (xoffset- 1.5f > -GetComponent<Collider>().bounds.size.x / 2 + transformHitting.GetComponent<Collider>().bounds.size.x)
            hittingDir = MovablePosistion.Directions.Left;
        else if(zoffset + 1.5f < transformHitting.GetComponent<Collider>().bounds.size.z / 2 + transformHitting.GetComponent<Collider>().bounds.size.z)
            hittingDir = MovablePosistion.Directions.Up;
        else if (zoffset - 1.5f > -GetComponent<Collider>().bounds.size.z / 2 + transformHitting.GetComponent<Collider>().bounds.size.z)
            hittingDir = MovablePosistion.Directions.Down;

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

    [Serializable]
    public class MovablePosistion
    {
        public enum Directions { Left, Right, Up, Down, Unknown}
        public Transform Posistion;
        public Directions DirectionForward = Directions.Right;
        public Directions DirectionBackward = Directions.Right;
        public float TimeToMove = 2f;
    }
}
