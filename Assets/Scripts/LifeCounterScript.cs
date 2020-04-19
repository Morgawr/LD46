using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounterScript : MonoBehaviour
{
    public Player Player;
    public UnityEngine.UI.Text TextLabel;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(int.Parse(TextLabel.text) != Player.AccumulatedMana) {
            TextLabel.text = Player.AccumulatedMana.ToString();
        }
    }
}
