using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    public class Connection
    {
        public ConnectionPoint InputPoint;
        public ConnectionPoint OutputPoint;

        [JsonIgnore]
        public Action<Connection> OnClickRemoveConnection;

        public Connection()
        {

        }

        public Connection( ConnectionPoint inputPoint, ConnectionPoint outputPoint, Action<Connection> onClickRemoveConnection)
        {
            InputPoint = inputPoint;
            OutputPoint = outputPoint;
            OnClickRemoveConnection = onClickRemoveConnection;
            InputPoint.AttachedNode = OutputPoint.OwnerNode;
            OutputPoint.AttachedNode = InputPoint.OwnerNode;
        }   

        public void Draw()
        {
            Handles.DrawBezier(
                InputPoint.Rect.center,
                OutputPoint.Rect.center,
                InputPoint.Rect.center + Vector2.left * 50f,
                OutputPoint.Rect.center - Vector2.left * 50f,
                Color.white,
                null,
                5f
            );

            if (Handles.Button((InputPoint.Rect.center + OutputPoint.Rect.center) * 0.5f, Quaternion.identity, 10, 10, Handles.RectangleCap))
            {
                OnClickRemoveConnection(this);
            }
        }
    }
}
