using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetup : MonoBehaviour {
    public Sprite backgroundSprite;
    public float scaleMultiplier = 5f; // Constant to increase the scale

    void Start() {
        GameObject background = new GameObject("Background");
        SpriteRenderer renderer = background.AddComponent<SpriteRenderer>();
        renderer.sprite = backgroundSprite;
        renderer.sortingLayerName = "Background";
        background.transform.position = new Vector3(0, 0, 10); // Behind everything

        // Scale the sprite to fit the screen and increase by a constant
        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        Vector2 spriteSize = renderer.sprite.bounds.size;
        background.transform.localScale = new Vector3(
            (screenWidth / spriteSize.x) * scaleMultiplier, 
            (screenHeight / spriteSize.y) * scaleMultiplier, 
            1
        );
    }
}