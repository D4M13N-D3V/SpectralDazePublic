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
    public class Node
    {
        public int Id;
        public Rect Rect;
        public CharacterInformation Character;
        public string Message;
        public List<string> Options = new List<string>();
        public List<ConnectionPoint> Outputs = new List<ConnectionPoint>();
        public ConnectionPoint Input;

        [JsonIgnore]
        public bool Selected;
        [JsonIgnore]
        public bool Dragged;
        [JsonIgnore]
        public Action<ConnectionPoint> OnClickConnector;
        [JsonIgnore]
        public Action<Node> DestroyNode;

        public Node()
        {

        }

        public Node(int id, float x, float y, float width, float height, Action<ConnectionPoint> onClickConnector)
        {
            Id = id;
            Rect = new Rect(x, y, width, height);
            OnClickConnector = onClickConnector;
            Input = new ConnectionPoint(Id+1000,this,ConnectionType.In, OnClickConnector);
        }

        public void Draw()
        {
            Rect = GUILayout.Window(Id, Rect, WindowContents, "Dialogue Node (#" + Id + ")");
            Input.Draw();
            foreach (var output in Outputs)
            {
                output.Draw();
            }
        }
            
        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
        }

        void WindowContents(int unusedWindowID)
        {
            //EditorGUILayout.LabelField("Unique Identifier");
            //Uid = GUILayout.TextField(Uid);
            EditorGUILayout.LabelField("Character");
            Character = (CharacterInformation)EditorGUILayout.ObjectField(Character, typeof(CharacterInformation), false, GUILayout.Width(220), GUILayout.Height(15));
            EditorGUILayout.Space();
            int newCount = Mathf.Max(0, EditorGUILayout.IntField("Amount of Options", Options.Count));
            while (newCount < Options.Count)
            {
                var i = Options.Count - 1;
                Options.RemoveAt(i);
                Outputs.RemoveAt(i);
            }
            while (newCount > Options.Count)
            {
                Options.Add(null);
                Outputs.Add(new ConnectionPoint(Id+1000+1000*Options.Count,this,ConnectionType.Out, OnClickConnector, Options.Count));
            }
            for (int i = 0; i < Options.Count; i++)
            {
                Options[i] = EditorGUILayout.TextField(Options[i]);
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Message");
            Message = GUILayout.TextArea(Message);
        }

        public void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (Rect.Contains(e.mousePosition))
                    {
                        if (e.button == 0)
                        {
                            Selected = true;
                            GUI.changed = true;
                        }
                    }
                    else
                    {
                        if (e.button == 0)
                            Selected = false;
                    }
                    break;
                case EventType.MouseDrag:
                    if (Rect.Contains(e.mousePosition) && Selected && e.button == 0)
                    {
                        Drag(e.delta);
                        e.Use();
                        Dragged = true;
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseUp:
                    if (Dragged && e.button == 0)
                        Dragged = false;
                    break;
            }
        }
    }
}
