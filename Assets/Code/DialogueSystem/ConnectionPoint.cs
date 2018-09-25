using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectralDaze.DialogueSystem;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// The connection point types.
    /// </summary>
    public enum ConnectionType { In, Out }

    /// <summary>
    /// The connection point for dialogue editor nodes.
    /// </summary>
    public class ConnectionPoint
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
        /// <summary>
        /// The owner node
        /// </summary>
        [JsonIgnore]
        public Node OwnerNode;
        /// <summary>
        /// The attached node
        /// </summary>
        [JsonIgnore]
        public Node AttachedNode;
        /// <summary>
        /// The rect
        /// </summary>
        public Rect Rect;
        /// <summary>
        /// The connection point type
        /// </summary>
        public ConnectionType Type;
        /// <summary>
        /// The option identifier
        /// </summary>
        public int OptionId;

        /// <summary>
        /// Method to be called for when connection point is called
        /// </summary>
        [JsonIgnore]
        public Action<ConnectionPoint> OnClickConnectionPoint;

        /// <summary>
        /// This parameterless constructor is used for serialization. <see cref="ConnectionPoint"/>
        /// </summary>
        public ConnectionPoint()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionPoint"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="ownerNode">The owner node.</param>
        /// <param name="type">The type.</param>
        /// <param name="onClickConnectionPoint">Method to be called for when connection point is called.</param>
        /// <param name="optionId">The option identifier.</param>
        public ConnectionPoint(int id, Node ownerNode, ConnectionType type, Action<ConnectionPoint> onClickConnectionPoint, int optionId = 0)
        {
            Id = id;
            OwnerNode = ownerNode;
            Type = type;
            OnClickConnectionPoint = onClickConnectionPoint;
            OptionId = optionId;
        }

        /// <summary>
        /// Draw the connection point;
        /// </summary>
        public void Draw()
        {
            switch (Type)
            {
                case ConnectionType.In:
                    Rect = new Rect(OwnerNode.Rect.x - 18, OwnerNode.Rect.y + 20, 18, 18);
                    if (GUI.Button(Rect, "o"))
                    {
                        OnClickConnectionPoint(this);
                    }
                    break;
                case ConnectionType.Out:
                    Rect = new Rect(OwnerNode.Rect.x + OwnerNode.Rect.width, OwnerNode.Rect.y + 62 + (OptionId * 18), 18, 18);
                    if (GUI.Button(Rect, "+"))
                    {
                        OnClickConnectionPoint(this);
                    }
                    break;
            }
        }
    }
}
