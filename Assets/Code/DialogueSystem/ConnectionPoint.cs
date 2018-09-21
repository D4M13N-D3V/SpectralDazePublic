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
    public enum ConnectionType { In, Out }
    public class ConnectionPoint
    {
        public int Id;
        [JsonIgnore]
        public Node OwnerNode;
        [JsonIgnore]
        public Node AttachedNode;
        public Rect Rect;
        public ConnectionType Type;
        public int OptionId;

        [JsonIgnore]
        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint()
        {

        }

        public ConnectionPoint(int id, Node ownerNode, ConnectionType type, Action<ConnectionPoint> onClickConnectionPoint, int optionId = 0)
        {
            Id = id;
            OwnerNode = ownerNode;
            Type = type;
            OnClickConnectionPoint = onClickConnectionPoint;
            OptionId = optionId;
        }

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
