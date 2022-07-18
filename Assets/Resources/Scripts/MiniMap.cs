using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private TextMeshProUGUI x;
    [SerializeField] private TextMeshProUGUI y;
    [SerializeField] private Image map;
    private RectTransform mapTransform;
    private Texture2D minimapTexture;
    
    void Start()
    {
        mapTransform = map.GetComponent<RectTransform>();
        minimapTexture = GeneratePictureOfTilemap(gameMap);
        Sprite sprite = Sprite.Create(minimapTexture, new Rect(0,0,minimapTexture.width, minimapTexture.height), new Vector2(0.5f, 0.5f));
        sprite.texture.filterMode = FilterMode.Point;
        mapTransform.sizeDelta = new Vector2(minimapTexture.width * 4, minimapTexture.height * 4);
        map.sprite = sprite;
    }
    private Texture2D GeneratePictureOfTilemap(Tilemap map)
    {
        Texture2D tileMapTexture = new Texture2D(map.size.x, map.size.y, TextureFormat.ARGB32, false);
        for (int i = -map.size.y/2; i < map.size.y/2; i++)
        {
            for (int j = -map.size.x/2; j < map.size.x/2; j++)
            {
                Color pixelColor = Color.black;
                try
                {
                    Texture2D aboba = textureFromSprite(map.GetSprite(new Vector3Int(j,i, 0)));
                    pixelColor = AverageColorOfSprite(aboba);
                }
                catch (Exception e)
                {
                    pixelColor = new Color(0,0,0,0);
                }
                tileMapTexture.SetPixel(j+map.size.y/2,i+map.size.y/2, pixelColor);
            }
        }
        tileMapTexture.Apply();
        return tileMapTexture;
    }
    private Color AverageColorOfSprite(Texture2D a)
    {
        if (a == null) return new Color(0, 0, 0, 1);
        
        Color[] pixelColors = a.GetPixels();
        float r = 0;
        float g = 0;
        float b = 0;
        for (int i = 0; i < pixelColors.Length; i++)
        {
            r += pixelColors[i].r;
            g += pixelColors[i].g;
            b += pixelColors[i].b;
        }
        
        return new Color((float)(r / pixelColors.Length) , (float)(g / pixelColors.Length) , (float)(b / pixelColors.Length) , 1);
    }
    public Texture2D textureFromSprite(Sprite sprite)
    {
        if(sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, 
                (int)sprite.textureRect.width, (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
        {
            return sprite.texture;
        }
    }
    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        x.text = "X: " + gameMap.WorldToCell(mousePos).x.ToString();
        y.text = "Y: " + gameMap.WorldToCell(mousePos).y.ToString();
        mapTransform.anchoredPosition = new Vector2(-Camera.main.transform.position.x * 4, -Camera.main.transform.position.y * 4);
    }
}

