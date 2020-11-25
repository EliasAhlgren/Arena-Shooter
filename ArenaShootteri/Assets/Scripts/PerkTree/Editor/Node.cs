using System;
using UnityEditor;
using UnityEngine;
using System.Text;

public class Node
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    // Rect for the title of the node 
    public Rect rectID;

    // Two Rect for the name field (1 for the label and other for the textfield)
    public Rect rectNameLabel;
    public Rect rectName;

    // Two Rect for the unlock field (1 for the label and other for the checkbox)
    public Rect rectUnlockLabel;
    public Rect rectUnlocked;

    // Two Rect for the cost field (1 for the label and other for the text field)
    public Rect rectCostLabel;
    public Rect rectCost;

    // Two Rect for the type field (1 for the label and other for the text field)
    public Rect rectTypeLabel;
    public Rect rectType;

    // Two Rect for the level field (1 for the label and other for the text field)
    public Rect rectLevelLabel;
    public Rect rectLevel;


    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    // GUI Style for the title
    public GUIStyle styleID;

    // GUI Style for the fields
    public GUIStyle styleField;

    public Action<Node> OnRemoveNode;

    // Perk linked with the node
    public Perk perk;

    //private string perkName = "Perk Name";
    //private int type = 0;
    //private PerkType perkType = PerkType.passive;
    //private int perkLevel = 0;

    // Bool for checking if the node is whether unlocked or not
    private bool unlocked = false;

    // StringBuilder to create the node's title
    private StringBuilder nodeTitle;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle,
        GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
        Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint,
        Action<Node> OnClickRemoveNode, int id, string perkName, bool unlocked, PerkType perkType, int perkLevel, int cost, int[] dependencies)
        //Action<Node> OnClickRemoveNode, int id, string perkName, bool unlocked, int type, int perkLevel, int cost, int[] dependencies)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, 
            inPointStyle, OnClickInPoint);

        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, 
            outPointStyle, OnClickOutPoint);

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        // Create new Rect and GUIStyle for our title and custom fields
        float rowHeight = height / 8;

        rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);
        styleID = new GUIStyle();
        styleID.alignment = TextAnchor.UpperCenter;

        rectName = new Rect(position.x + width / 3, position.y + 2 * rowHeight, width / 3, rowHeight);
        rectNameLabel = new Rect(position.x, position.y + 2 * rowHeight, width / 3, rowHeight);

        rectUnlocked = new Rect(position.x + width / 3, position.y + 3 * rowHeight, width / 3, rowHeight);
        rectUnlockLabel = new Rect(position.x, position.y + 3 * rowHeight, width / 3, rowHeight);

        styleField = new GUIStyle();
        styleField.alignment = TextAnchor.UpperRight;

        rectType = new Rect(position.x + width / 3, position.y + 4 * rowHeight, width / 3, rowHeight);
        rectTypeLabel = new Rect(position.x, position.y + 4 * rowHeight, width / 3, rowHeight);

        rectLevel = new Rect(position.x + width / 3, position.y + 5 * rowHeight, 20, rowHeight);
        rectLevelLabel = new Rect(position.x, position.y + 5 * rowHeight, width / 3, rowHeight);

        rectCost = new Rect(position.x + width / 3, position.y + 6 * rowHeight, 20, rowHeight);
        rectCostLabel = new Rect(position.x, position.y + 6 * rowHeight, width / 3, rowHeight);
        

        this.unlocked = unlocked;
        //this.perkName = perkName;
        //this.type = type;
        //this.perkType = perkType;
        //this.perkLevel = perkLevel;

        // We create the perk with current node info
        perk = new Perk();
        perk.id_Perk = id;
        perk.perkName = perkName;
        perk.unlocked = unlocked;
        //perk.type = type;
        perk.perktype = perkType;
        perk.perkLevel = perkLevel;
        perk.cost = cost;
        perk.perk_Dependencies = dependencies;

        // Create string with ID info
        nodeTitle = new StringBuilder();
        nodeTitle.Append("ID: ");
        nodeTitle.Append(id);

    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        rectID.position += delta;
        rectName.position += delta;
        rectNameLabel.position += delta;
        rectUnlocked.position += delta;
        rectUnlockLabel.position += delta;
        rectType.position += delta;
        rectTypeLabel.position += delta;
        rectLevel.position += delta;
        rectLevelLabel.position += delta;
        rectCost.position += delta;
        rectCostLabel.position += delta;
    }

    public void MoveTo(Vector2 pos)
    {
        rect.position = pos;
        rectID.position = pos;
        rectName.position = pos;
        rectNameLabel.position = pos;
        rectUnlocked.position = pos;
        rectUnlockLabel.position = pos;
        rectType.position = pos;
        rectTypeLabel.position = pos;
        rectLevel.position = pos;
        rectLevelLabel.position = pos;
        rectCost.position = pos;
        rectCostLabel.position = pos;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);

        // Print the title
        GUI.Label(rectID, nodeTitle.ToString(), styleID);

        // Print the name
        GUI.Label(rectNameLabel, "Name: ", styleField);
        perk.perkName = GUI.TextField(rectName, perk.perkName.ToString());

        // Print the unlock field
        GUI.Label(rectUnlockLabel, "Unlocked: ", styleField);
        if (GUI.Toggle(rectUnlocked, unlocked, ""))
            unlocked = true;
        else
            unlocked = false;

        perk.unlocked = unlocked;

        // Print the Type
        GUI.Label(rectTypeLabel, "Type: ", styleField);
        perk.perktype = (PerkType)EditorGUI.EnumPopup(rectType, perk.perktype);
        //perk.type = int.Parse(GUI.TextField(rectType, perk.type.ToString()));

        // Print the level field
        GUI.Label(rectLevelLabel, "Level: ", styleField);
        perk.perkLevel = int.Parse(GUI.TextField(rectLevel, perk.perkLevel.ToString()));

        // Print the cost field
        GUI.Label(rectCostLabel, "Cost: ", styleField);
        perk.cost = int.Parse(GUI.TextField(rectCost, perk.cost.ToString()));
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}