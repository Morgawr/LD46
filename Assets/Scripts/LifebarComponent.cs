using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifebarComponent : MonoBehaviour
{

    public Player Player;
    public RectTransform LifeFill;
    public RectTransform LifeBegin;
    public RectTransform LifeEnd;
    public RectTransform LifechunkBase;
    List<RectTransform> chunks = new List<RectTransform>();

    // Start is called before the first frame update
    void Start() {
        
    }

    void AddNewChunk() {
        // Create new chunk base
        float newX = 0;
        if(chunks.Count == 0) 
            newX = 100;
        else
            newX = chunks[chunks.Count - 1].localPosition.x + 300;
        var newChunk = Instantiate(LifechunkBase.gameObject, new Vector2(newX, LifechunkBase.localPosition.y), Quaternion.identity);
        newChunk.SetActive(true);
        chunks.Add(newChunk.GetComponent<RectTransform>());
        newChunk.transform.SetParent(this.transform, false);
    }

    // Call this method every time the contents of the Lifebar might change and 
    // we need to reposition everything
    void RepositionLifeBar() {
        LifeEnd.localPosition = new Vector2(LifeBegin.localPosition.x + LifeBegin.rect.width + chunks.Count*(chunks[0].rect.width), LifeEnd.localPosition.y);
    }

    void RefreshLifeFill() {
        var totalUIWidth = chunks.Count * (chunks[0].rect.width) + LifeEnd.rect.width / 2 + LifeBegin.rect.width / 2; 
        var lifePercent = (float)Player.CurrentLife / (float)Player.MaxLife;
        LifeFill.sizeDelta = new Vector2(totalUIWidth * lifePercent, LifeFill.rect.height);
    }

    // Update is called once per frame
    void Update() {
        var neededChunks = CalculateNumberOfChunks();
        while(neededChunks > chunks.Count) {
            AddNewChunk();
            RepositionLifeBar();
        }
        RefreshLifeFill();
    }

    int CalculateNumberOfChunks() {
        return Mathf.FloorToInt(Player.MaxLife / 100);
    }
}
