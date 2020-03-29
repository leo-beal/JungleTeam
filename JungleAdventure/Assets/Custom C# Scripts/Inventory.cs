using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Inventory : MonoBehaviour
{
    private List<Interactable> objects = new List<Interactable>();

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

                objects.Add(toAdd);

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

                objects.Add(toAdd);

                toAdd = null;
            }

            if (objects.Count > 0)
            {
                var grab = objects[0];

                objects.RemoveAt(0);

                grab.gameObject.SetActive(true);

                hand.AttachObject(grab.gameObject, GrabTypes.Grip);
                hand.HoverLock(grab);
            }

            if (this.canvas != null)
            {
                DestroyCanvas();
                CreateCanvas();
            }
        }

        if (displayInventory.stateDown)
        {
            if (this.canvas != null)
                DestroyCanvas();
            else
                CreateCanvas();
        }
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

        var camera = hand.transform.root.Find("SteamVRObjects").Find("VRCamera");

        var playerPosition = camera.position;
        var playerDirection = camera.forward;
        var playerRotation = camera.rotation;

        c.transform.position = playerPosition + (playerDirection * 2);

        if (hand.name.Contains("Right"))
            c.transform.Translate(new Vector3(0.55f, 0, 0), camera);
        else
            c.transform.Translate(new Vector3(-0.55f, 0, 0), camera);

        c.transform.LookAt(hand.transform.position);
        c.transform.rotation = playerRotation;

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
        newText.transform.SetParent(c.transform, false);
        newText.AddComponent<RectTransform>().sizeDelta = size;
        
        var fonts = Font.GetOSInstalledFontNames();
        
        var text = newText.AddComponent<Text>();
        text.color = Color.black;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.UpperCenter;
        text.text = $"{hand.name.Replace("Hand", "")} Inventory";
        text.font = Font.CreateDynamicFontFromOSFont(fonts[0], 1);

        //list items that are currently in the inventory
        float offset = 0.1f;

        foreach (var o in this.objects)
        {
            var buttonObject = new GameObject();
            buttonObject.transform.SetParent(panel.transform, false);

            var button = buttonObject.AddComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Debug.Log("Clicked");

                foreach (var ao in hand.AttachedObjects)
                {
                    var go = ao.attachedObject.gameObject;
                    var inter = go.GetComponent<Interactable>();

                    hand.DetachObject(go);
                    hand.HoverUnlock(inter);

                    go.SetActive(false);

                    objects.Add(inter);
                }

                this.objects.Remove(o);

                hand.AttachObject(o.gameObject, GrabTypes.Grip);
                hand.HoverLock(o);
            });

            newText = new GameObject();
            newText.AddComponent<RectTransform>().sizeDelta = size;
            newText.transform.SetParent(buttonObject.transform, false);
            newText.transform.localPosition = new Vector3(0, -offset, 0);

            text = newText.AddComponent<Text>();
            text.text = o.name;
            text.color = Color.black;
            text.font = Font.CreateDynamicFontFromOSFont(fonts[0], 1);

            offset += 0.1f;
        }
    }
}
