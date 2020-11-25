using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class PerkTreeNodeEditor : EditorWindow
{
    private List<Node> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    // Rect for buttons to Clear, Save and Load 
    private Rect rectButtonClear;
    private Rect rectButtonSave;
    private Rect rectButtonLoad;

    // Count for nodes created
    private int nodeCount;

    // Where we store the perktree that we are managing with this tool
    private PerkTree perkTree;

    // Dictionary with the perks in our perktree
    private Dictionary<int, Perk> perkDictionary;

    [MenuItem("Window/Perk Tree Node Editor")]
    private static void OpenWindow()
    {
        PerkTreeNodeEditor window = GetWindow<PerkTreeNodeEditor>();
        window.titleContent = new GUIContent("Perk Tree Node Editor");
    }

    private void OnEnable()
    {
        // Create the perktree
        perkTree = new PerkTree();

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/lightskin/images/node5.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node5 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = Resources.Load("green") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = Resources.Load("red") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        // Create buttons for clear, save and load
        rectButtonClear = new Rect(new Vector2(10, 10), new Vector2(60, 20));
        rectButtonSave = new Rect(new Vector2(80, 10), new Vector2(60, 20));
        rectButtonLoad = new Rect(new Vector2(150, 10), new Vector2(60, 20));

        // Initialize nodes with saved data
        LoadNodes();
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        // We draw our new buttons (Clear, Load and Save)
        DrawButtons();

        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed)
            Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

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

    private void DrawNodes()
    {
        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    // Draw our new buttons for managing the perk tree
    private void DrawButtons()
    {
        if (GUI.Button(rectButtonClear, "Clear"))
            ClearNodes();
        if (GUI.Button(rectButtonSave, "Save"))
            SavePerkTree();
        if (GUI.Button(rectButtonLoad, "Load"))
            LoadNodes();
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
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

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        // We create the node with the default info for the node
        nodes.Add(new Node(mousePosition, 200, 120, nodeStyle, selectedNodeStyle,
            inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
            //nodeCount, "perk name" ,false, PerkType.passive , 0 , 0, null));
            nodeCount, "perk name", false, 0, 0, 0, null));
        ++nodeCount;
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    // Function for clearing data from the editor window
    private void ClearNodes()
    {
        nodeCount = 0;
        if (nodes != null && nodes.Count > 0)
        {
            Node node;
            while (nodes.Count > 0)
            {
                node = nodes[0];

                OnClickRemoveNode(node);
            }
        }
    }

    // Save data from the window to the perk tree
    private void SavePerkTree()
    {
        if (nodes.Count > 0)
        {
            // We fill with as many perks as nodes we have
            perkTree.perktree = new Perk[nodes.Count];
            int[] dependencies;
            List<int> dependenciesList = new List<int>();

            // Iterate over all of the nodes. Populating the perks with the node info
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (connections != null)
                {
                    List<Connection> connectionsToRemove = new List<Connection>();
                    List<ConnectionPoint> connectionsPointsToCheck = new List<ConnectionPoint>();

                    for (int j = 0; j < connections.Count; j++)
                    {
                        if (connections[j].inPoint == nodes[i].inPoint)
                        {
                            for (int k = 0; k < nodes.Count; ++k)
                            {
                                if (connections[j].outPoint == nodes[k].outPoint)
                                {
                                    dependenciesList.Add(k);
                                    break;
                                }
                            }
                            connectionsToRemove.Add(connections[j]);
                            connectionsPointsToCheck.Add(connections[j].outPoint);
                        }
                    }
                }
                dependencies = dependenciesList.ToArray();
                dependenciesList.Clear();
                perkTree.perktree[i] = nodes[i].perk;
                perkTree.perktree[i].perk_Dependencies = dependencies;
            }

            string json = JsonUtility.ToJson(perkTree);
            string path = null;

            path = "Assets/Data/perktree.json";

            // Finally, we write the JSON string with the PerkTree data in our file
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(json);
                }
            }
            UnityEditor.AssetDatabase.Refresh();

            SaveNodes();
        }
    }

    // Save data from the nodes (position in our custom editor window)
    private void SaveNodes()
    {
        NodeDataCollection nodeData = new NodeDataCollection();
        nodeData.nodeDataCollection = new NodeData[nodes.Count];

        for (int i = 0; i < nodes.Count; ++i)
        {
            nodeData.nodeDataCollection[i] = new NodeData();
            nodeData.nodeDataCollection[i].id_Node = nodes[i].perk.id_Perk;
            nodeData.nodeDataCollection[i].position = nodes[i].rect.position;
        }

        string json = JsonUtility.ToJson(nodeData);
        string path = "Assets/Data/nodeData.json";

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }
        }
        UnityEditor.AssetDatabase.Refresh();
        PlayerPrefs.DeleteKey("PerkTree");
    }

    private void LoadNodes()
    {
        ClearNodes();

        string path = "Assets/Data/nodeData.json";
        string dataAsJson;
        NodeDataCollection loadedData;
        if (File.Exists(path))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a PerkTree object from it
            loadedData = JsonUtility.FromJson<NodeDataCollection>(dataAsJson);

            Perk[] _perkTree;
            List<Perk> originNode = new List<Perk>();
            perkDictionary = new Dictionary<int, Perk>();
            path = "Assets/Data/perktree.json";
            Vector2 pos = Vector2.zero;
            if (File.Exists(path))
            {
                // Read the json from the file into a string
                dataAsJson = File.ReadAllText(path);

                // Pass the json to JsonUtility, and tell it to create a PerkTree object from it
                PerkTree perkData = JsonUtility.FromJson<PerkTree>(dataAsJson);

                // Store the PerkTree as an array of Perk
                _perkTree = new Perk[perkData.perktree.Length];
                _perkTree = perkData.perktree;

                // Create nodes
                for (int i = 0; i < _perkTree.Length; ++i)
                {
                    for (int j = 0; j < loadedData.nodeDataCollection.Length; ++j)
                    {
                        if (loadedData.nodeDataCollection[j].id_Node == _perkTree[i].id_Perk)
                        {
                            pos = loadedData.nodeDataCollection[j].position;
                            break;
                        }
                    }
                    LoadPerkCreateNode(_perkTree[i], pos);
                    if (_perkTree[i].perk_Dependencies.Length == 0)
                    {
                        originNode.Add(_perkTree[i]);
                    }
                    perkDictionary.Add(_perkTree[i].id_Perk, _perkTree[i]);
                }

                Perk outPerk;
                Node outNode = null;
                // Create connections
                for (int i = 0; i < nodes.Count; ++i)
                {
                    for (int j = 0; j < nodes[i].perk.perk_Dependencies.Length; ++j)
                    {
                        if (perkDictionary.TryGetValue(nodes[i].perk.perk_Dependencies[j], out outPerk))
                        {
                            for (int k = 0; k < nodes.Count; ++k)
                            {
                                if (nodes[k].perk.id_Perk == outPerk.id_Perk)
                                {
                                    outNode = nodes[k];
                                    OnClickOutPoint(outNode.outPoint);
                                    break;
                                }
                            }
                            OnClickInPoint(nodes[i].inPoint);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Cannot load game data!");
            }
        }
    }

    private void LoadPerkCreateNode(Perk perk, Vector2 position)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        nodes.Add(new Node(position, 200, 120, nodeStyle, selectedNodeStyle,
            inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
            perk.id_Perk, perk.perkName, perk.unlocked, perk.perktype, perk.perkLevel, perk.cost, perk.perk_Dependencies));
            //perk.id_Perk, perk.perkName, perk.unlocked, perk.type, perk.perkLevel, perk.cost, perk.perk_Dependencies));
        ++nodeCount;
    }
}