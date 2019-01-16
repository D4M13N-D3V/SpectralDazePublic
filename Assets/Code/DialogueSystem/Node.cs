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
    /// Node for the dialogue editor
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
        /// <summary>
        /// The rect
        /// </summary>
        public Rect Rect;

        /// <summary>
        /// The character path
        /// </summary>
        public string CharacterPath;

        /// <summary>
        /// The character information scriptable object
        /// </summary>
        [JsonIgnore] public CharacterInformation Character;

        /// <summary>
        /// The message for the node.
        /// </summary>
        public string Message;
        /// <summary>
        /// The options for the dialogue node
        /// </summary>
        public List<string> Options = new List<string>();
        /// <summary>
        /// The output connection points that are generated based on amount of options/
        /// </summary>
        public List<ConnectionPoint> Outputs = new List<ConnectionPoint>();
        /// <summary>
        /// The input connection point
        /// </summary>
        public ConnectionPoint Input;

        /// <summary>
        /// Is this a beggining node
        /// </summary>
        public bool First;
        /// <summary>
        /// Is this a ending node.
        /// </summary>
        public bool Last;

        /// <summary>
        /// Is selected
        /// </summary>
        [JsonIgnore]
        public bool Selected;
        /// <summary>
        /// Is being dragged
        /// </summary>
        [JsonIgnore]
        public bool Dragged;
        /// <summary>
        /// Method for when a connection point is clicked.
        /// </summary>
        [JsonIgnore]
        public Action<ConnectionPoint> OnClickConnector;
        /// <summary>
        /// Method to destroy node
        /// </summary>
        [JsonIgnore]
        public Action<Node> DestroyNode;
        /// <summary>
        /// Method to set starting node.
        /// </summary>
        [JsonIgnore]
        public Action<Node> SetStartingNode;
        /// <summary>
        /// Method to set ending node.
        /// </summary>
        [JsonIgnore]
        public Action<Node> SetEndingNode;

        /// <summary>
        /// This parameterless constructor is used for serialization. <see cref="Node"/>
        /// </summary>
        public Node()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="x">The x posistion.</param>
        /// <param name="y">The y posistion.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="onClickConnector">Method for when conncetion point is clicked.</param>
        /// <param name="removeNode">Method for when node is removed.</param>
        /// <param name="setStartingNode">Method for setting starting node.</param>
        /// <param name="setEndingNode">Method for setting ending node.</param>
        public Node(int id, float x, float y, float width, float height, Action<ConnectionPoint> onClickConnector,
            Action<Node> removeNode, Action<Node> setStartingNode, Action<Node> setEndingNode)
        {
            SetStartingNode = setStartingNode;
            SetEndingNode = setEndingNode;
            DestroyNode = removeNode;
            Id = id;
            Rect = new Rect(x, y, width, height);
            OnClickConnector = onClickConnector;
            Input = new ConnectionPoint(Id+1000,this,ConnectionType.In, OnClickConnector);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws anything that the node needs drawn.
        /// </summary>
        public void Draw()
        {
            Rect = GUILayout.Window(Id, Rect, WindowContents, "Dialogue Node (#" + Id + ")");
            Input.Draw();
            foreach (var output in Outputs)
            {
                output.Draw();
            }
        }

        /// <summary>
        /// Drags the node using the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
        }

        /// <summary>
        /// Draws the windows contents.
        /// </summary>
        /// <param name="unusedWindowID">The unused window identifier.</param>
        void WindowContents(int unusedWindowID)
        {
            //EditorGUILayout.LabelField("Unique Identifier");
            //Uid = GUILayout.TextField(Uid);
            if(First)
                EditorGUILayout.LabelField("STARTING NODE");
            if(Last)
                EditorGUILayout.LabelField("ENDING NODE");

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
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
            CharacterPath = AssetDatabase.GetAssetPath(Character);
        }

        /// <summary>
        /// Processes the events for node.
        /// </summary>
        /// <param name="e">The event data.</param>
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
                        else if (e.button == 1)
                        {
                            ProcessContextMenu();
                            e.Use();
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

        /// <summary>
        /// Creates, Generates, & Opens the context menu.
        /// </summary>
        public void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Delete Node"), false, () => { DestroyNode(this); });
            genericMenu.AddItem(new GUIContent("Set Node As Start"), false, () => { SetStartingNode(this); });
            genericMenu.AddItem(new GUIContent("Set Node As End"), false, () => { SetEndingNode(this); });
            genericMenu.ShowAsContext();
        }
        #endif
    }
}