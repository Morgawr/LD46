using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour
{
    public ControllableComponent controller;
    public Player Player;
    public string tooltip = "Press up to interact";

    protected GameObject MessageBox;
    protected Text Lore;

    protected bool isOnTooltip = false;


    SpriteRenderer spriteRenderer;

    public GUIStyle TooltipStyle;

    void OnTriggerEnter2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(this);
        isOnTooltip = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        controller.SignalIsOnInteractable(null);
        isOnTooltip = false;
    }

    public virtual void Interact(){

        if(spriteRenderer == null || !spriteRenderer.enabled || spriteRenderer.sprite == null) 
            return;
        if(!controller.SFXManager.IsFXPlaying("Interaction"))
            controller.SFXManager.PlayFX("Interaction"); 
    }

    protected virtual void OnGUI()
    {
        if (isOnTooltip && (spriteRenderer == null || spriteRenderer.enabled))
        {
            GUI.Label(new Rect((Screen.width - 150f) / 2, (Screen.height / 10) * 8, 200f, 200f), tooltip, TooltipStyle);
        }
    }

    protected virtual void Update() {

    }

    protected virtual void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MessageBox = Player.MessageBox;
        Lore = Player.Lore;
    }
}
