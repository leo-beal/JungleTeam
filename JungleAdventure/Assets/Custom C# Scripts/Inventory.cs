using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Inventory : MonoBehaviour
{
    private Queue<Interactable> objects = new Queue<Interactable>();

    public SteamVR_Action_Boolean addToInventory;
    public SteamVR_Action_Boolean pullFromInventory;
    public SteamVR_Action_Boolean displayInventory;
    public Hand hand;

    private GameObject canvas = null;
    public void Update()
    {
        Interactable toAdd = null;

        if (hand.AttachedObjects.Count == 1)
            toAdd = hand.AttachedObjects[0].interactable;

        if (addToInventory.active && addToInventory.stateDown)
        {
            if (toAdd != null)
            {
                hand.DetachObject(toAdd.gameObject);
                hand.HoverUnlock(toAdd);

                toAdd.gameObject.SetActive(false);

                objects.Enqueue(toAdd);

                toAdd = null;
            }

            if (this.canvas != null)
            {
                DestroyCanvas();
                CreateCanvas();
            }
        }
        else if (pullFromInventory.active && pullFromInventory.stateDown)
        {
            if (toAdd != null)
            {
                hand.DetachObject(toAdd.gameObject);
                hand.HoverUnlock(toAdd);

                toAdd.gameObject.SetActive(false);

                objects.Enqueue(toAdd);

                toAdd = null;
            }

            if (objects.Count > 0)
            {
                var grab = objects.Dequeue();

                grab.gameObject.SetActive(true);

                hand.AttachObject(grab.gameObject, GrabTypes.Grip);
                hand.HoverLock(toAdd);
            }

            if (this.canvas != null)
            {
                DestroyCanvas();
                CreateCanvas();
            }
        }
        
        if (displayInventory.stateUp && this.canvas != null)
            DestroyCanvas();
        else if (displayInventory.stateDown && this.canvas == null)
            CreateCanvas();
    }

    private void DestroyCanvas()
    {
        Destroy(this.canvas);
        this.canvas = null;
    }

    private void CreateCanvas()
    {
        var height = (objects.Count + 2) * 0.1f;

        var size = new Vector2(1, height);

        //declare our canvas game object
        this.canvas = new GameObject();

        //size our canvas
        this.canvas.AddComponent<RectTransform>().sizeDelta = size;

        //create the canvas for the gameobject
        var c = this.canvas.AddComponent<Canvas>();

        c.transform.SetParent(hand.transform, false);
        c.transform.localPosition = new Vector3(0, 0, 1.5f);
        c.renderMode = RenderMode.WorldSpace;

        //create scale for canvas
        var scale = c.gameObject.AddComponent<CanvasScaler>();

        scale.dynamicPixelsPerUnit = 5000;
        scale.referencePixelsPerUnit = 5000;

        //add a panel for background color
        var panel = new GameObject("background");
        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<Image>().color = new Color(100f/255f, 100f/255f, 100f/255f, 200f/255f);
        panel.transform.SetParent(this.canvas.transform, false);
        panel.GetComponent<RectTransform>().sizeDelta = size;

        //add text to canvas
        var newText = new GameObject();
        newText.AddComponent<RectTransform>().sizeDelta = size;
        var text = newText.AddComponent<Text>();
        text.text = $"{hand.name.Replace("Hand", "")} Inventory";
        var fonts = Font.GetOSInstalledFontNames();
        text.font = Font.CreateDynamicFontFromOSFont(fonts[0], 1);
        text.color = Color.black;
        text.alignment = TextAnchor.UpperCenter;
        text.fontStyle = FontStyle.Bold;
        newText.transform.SetParent(c.transform, false);

        //list items that are currently in the inventory
        float offset = 0.1f;

        foreach (var o in this.objects)
        {
            newText = new GameObject();
            newText.AddComponent<RectTransform>().sizeDelta = size;
            text = newText.AddComponent<Text>();
            text.text = o.name;
            text.font = Font.CreateDynamicFontFromOSFont(fonts[0], 1);
            text.color = Color.black;
            newText.transform.SetParent(c.transform, false);
            newText.transform.localPosition = new Vector3(0, -offset, 0);
            offset += 0.1f;
        }
    }
}
