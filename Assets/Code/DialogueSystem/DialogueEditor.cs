using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    public class DialogueEditor : EditorWindow
    {
        public class DialogueSave
        {
            public List<Node> Nodes = new List<Node>();
            public List<Connection> Connections = new List<Connection>();
        }

        private int NodeCount = 0;

        public List<Node> Nodes = new List<Node>();
        public List<Connection> Connections = new List<Connection>();
        public ConnectionPoint SelectedInPoint;
        public ConnectionPoint SelectedOutputPoint;

        private Vector2 _drag;
        private Vector2 _offset;

        private float menuBarHeight = 20f;
        private Rect menuBar;

        private bool _loading;
        private string _saveName;


        [MenuItem("Window/Dialogue Editor")]
        private static void OpenWindow()
        {
            DialogueEditor window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
        }
            
        private void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            if (!_loading)
            {
                BeginWindows();
                foreach (var node in Nodes)
                {
                    node.Draw();
                    node.ProcessEvents(Event.current);
                }
                EndWindows();

                foreach (var connection in Connections)
                {
                    connection.Draw();
                }

                DrawConnectionLine(Event.current);
            }

            DrawMenuBar();

            ProcessEvents(Event.current);
            if (GUI.changed) Repaint();
        }

        private void ProcessEvents(Event e)
        {
            _drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        SelectedOutputPoint = null;
                        SelectedInPoint = null;
                    }
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.Delete)
                    {
                        RemoveNode(Nodes.SingleOrDefault(x => x.Selected));
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }
            
        public void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () =>
            {
                NodeCount++;
                Nodes.Add(new Node(NodeCount, mousePosition.x, mousePosition.y, 200, 100, OnNodeConnectorClicked));

                Repaint();
            });
            genericMenu.ShowAsContext();
        }

        private void OnDrag(Vector2 delta)
        {
            _drag = delta;
            foreach (var node in Nodes)
            {
                node.Drag(delta);
            }
            Repaint();
        }

        private void DrawMenuBar()
        {
            menuBar = new Rect(0, 0, position.width, menuBarHeight);

            GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
            {
                Save();
            }
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
            {
                Load();
            }
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Clear"), EditorStyles.toolbarButton, GUILayout.Width(45)))
            {
                Connections.Clear();
                Nodes.Clear();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void Save()
        {
            var save = new DialogueSave()
            {
                Connections = Connections,
                Nodes = Nodes,
            };
           var path = EditorUtility.SaveFilePanel(
                "Save Dialogue",
                Application.dataPath + "/Resources/",
                "Diagloue.json",
                "json");
            if (path == "")
                return;
            var json = JsonConvert.SerializeObject(save);
            File.WriteAllText(path, json);
        }

        private void Load()
        {
            _loading = true;
            Nodes.Clear();
            Connections.Clear();
            var path = EditorUtility.OpenFilePanel("Open Dialogue", "/Resources/", "json");
            if (path == "")
                return; 
            string json = File.ReadAllText(path);
            DialogueSave save = JsonConvert.DeserializeObject<DialogueSave>(json);
            foreach (var node in save.Nodes)
            {
                var tmpNode = new Node(
                    node.Id,
                    node.Rect.position.x,
                    node.Rect.position.y,
                    node.Rect.width,
                    node.Rect.height,
                    OnNodeConnectorClicked
                );
                tmpNode.Character = node.Character;
                tmpNode.Options = node.Options;
                tmpNode.Outputs = node.Outputs;
                tmpNode.Input = node.Input;
                tmpNode.Input.OwnerNode = tmpNode;
                tmpNode.Input.OnClickConnectionPoint = OnNodeConnectorClicked;
                foreach (var output in tmpNode.Outputs)
                {
                    output.OwnerNode = tmpNode;
                    output.OnClickConnectionPoint = OnNodeConnectorClicked;
                }
                Nodes.Add(tmpNode);
            }

            foreach (var connection in save.Connections)
            {
                var inPoint = Nodes.First(n => n.Input.Id == connection.InputPoint.Id).Input;
                foreach (var node in Nodes)
                {
                    foreach (var output in node.Outputs)
                    {
                        if (output.Id != connection.OutputPoint.Id) continue;
                        Connections.Add(new Connection(inPoint, output, OnClickRemoveConnection));
                        break;
                    }
                }
            }

            NodeCount = Nodes.Last().Id;
            _loading = false;
        }

        private void DrawConnectionLine(Event e)
        {
            if (SelectedInPoint != null && SelectedOutputPoint == null)
            {
                Handles.DrawBezier(
                    SelectedInPoint.Rect.center,
                    e.mousePosition,
                    SelectedInPoint.Rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (SelectedOutputPoint != null && SelectedInPoint == null)
            {
                Handles.DrawBezier(
                    SelectedOutputPoint.Rect.center,
                    e.mousePosition,
                    SelectedOutputPoint.Rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            _offset += _drag * 0.5f;
            Vector3 newOffset = new Vector3(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void RemoveNode(Node node)
        {
            Nodes.Remove(node);
            Connection connectionToRemove = null;
            foreach (var connection in Connections)
            {
                if (connection.OutputPoint.OwnerNode == node || connection.InputPoint.OwnerNode == node)
                    connectionToRemove = connection;
            }

            if(connectionToRemove!=null)
                Connections.Remove(connectionToRemove);

            Repaint();
        }

        public void OnNodeConnectorClicked(ConnectionPoint point)
        {
            switch (point.Type)
            {
                case ConnectionType.In:
                    SelectedInPoint = point;
                    if (SelectedOutputPoint == null)
                        return;
                    CreateConnection();
                    break;
                case ConnectionType.Out:
                    SelectedOutputPoint = point;
                    if (SelectedInPoint == null)
                        return;
                    CreateConnection();
                    break;
            }
            GUI.changed = true;
        }

        public void CreateConnection()
        {
            var connectionExists = false;

            foreach (var connection in Connections)
            {
                if (connection.OutputPoint == SelectedOutputPoint || connection.InputPoint == SelectedOutputPoint)
                {
                    connectionExists = true;
                    break;
                }
            }   
            Connections.Add(new Connection(SelectedInPoint, SelectedOutputPoint, OnClickRemoveConnection));
            SelectedInPoint = null;
            SelectedOutputPoint = null;
            Repaint();
        }

        private void OnClickRemoveConnection(Connection connection)
        {
            Connections.Remove(connection);
            Repaint();
        }
    }   
}
    