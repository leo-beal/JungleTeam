using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class UIButton : MonoBehaviour
{
    public Text text;
    public Color hover;
    public Color color;
    public Interactable item;

    public void Highlight()
    {
        text.color = hover;
    }

    public void RemoveHighlight()
    {
        text.color = color;
    }
}
