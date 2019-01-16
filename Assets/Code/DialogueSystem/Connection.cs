
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// Connection between connection points for dialogue editor.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// The input connection point
        /// </summary>
        public ConnectionPoint InputPoint;
        /// <summary>
        /// The output connecction point
        /// </summary>
        public ConnectionPoint OutputPoint;

        /// <summary>
        /// Method to call to remove connection.
        /// </summary>
        [JsonIgnore]
        public Action<Connection> OnClickRemoveConnection;

        /// <summary>
        /// This parameterless constructor is used for serialization. <see cref="Connection"/>
        /// </summary>
        public Connection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="inputPoint">The input connection point.</param>
        /// <param name="outputPoint">The output connection point.</param>
        /// <param name="onClickRemoveConnection">Method to call to remove connection..</param>
        public Connection( ConnectionPoint inputPoint, ConnectionPoint outputPoint, Action<Connection> onClickRemoveConnection)
        {
            InputPoint = inputPoint;
            OutputPoint = outputPoint;
            OnClickRemoveConnection = onClickRemoveConnection;
            InputPoint.AttachedNode = OutputPoint.OwnerNode;
            OutputPoint.AttachedNode = InputPoint.OwnerNode;
        }

    #if UNITY_EDITOR
        /// <summary>
        /// Draws this connection.
        /// </summary>
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

    #endif
    }
}