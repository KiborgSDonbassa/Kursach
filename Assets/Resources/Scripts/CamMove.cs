using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamMove : MonoBehaviour
{
    [SerializeField] private GameObject DieScreen;
    public static GameObject DieScreenStatic;
    [SerializeField] private int camera_speed = 1;
    [SerializeField] private int vidstup = 20;
    [SerializeField] private int maxDistance;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject itemMenuGameObject;
    public static GameObject itemMenu;
    [SerializeField] private List<Item> SpellInfo = new List<Item>();
    [SerializeField] private List<Image> inventoryPanel = new List<Image>();
    [SerializeField] private Image Q_reloadIndicator, W_reloadIndicator, E_reloadIndicator, R_reloadIndicator, F_reloadIndicator;
    [SerializeField] private Sprite defaultSpriteObject;
    public static Sprite defaultSprite;
    private GameObject PreEnter, Q, W, E, R, F;
    private Image Qrenderer, Wrenderer, Erenderer, Rrenderer, Frenderer;
    private GameObject _interface;
    private Vector2 mousePos;
    private Material rangeMat;
    
    private void Awake()
    {
        DieScreenStatic = DieScreen;
        defaultSprite = defaultSpriteObject;
        itemMenu = itemMenuGameObject;
        _interface = GameObject.Find("PlayerInterface");
        Q = _interface.transform.GetChild(0).gameObject;
        Qrenderer = Q.GetComponent<Image>();
        Q.GetComponent<Image>().sprite = SpellInfo.Where(x => x.name_ == "LightBolt").FirstOrDefault()?.icon_ ?? defaultSprite;
        
        W = _interface.transform.GetChild(1).gameObject;
        Wrenderer = W.GetComponent<Image>();
        W.GetComponent<Image>().sprite = SpellInfo.Where(x => x.name_ == "Heal").FirstOrDefault()?.icon_ ?? defaultSprite;
        
        E = _interface.transform.GetChild(2).gameObject;
        Erenderer = E.GetComponent<Image>();
        E.GetComponent<Image>().sprite = SpellInfo.Where(x => x.name_ == "Explosion").FirstOrDefault()?.icon_ ?? defaultSprite;
        
        R = _interface.transform.GetChild(3).gameObject;
        Rrenderer = R.GetComponent<Image>();
        R.GetComponent<Image>().sprite = SpellInfo.Where(x => x.name_ == "").FirstOrDefault()?.icon_ ?? defaultSprite;
        
        F = _interface.transform.GetChild(4).gameObject;
        Frenderer = F.GetComponent<Image>();
        F.GetComponent<Image>().sprite = SpellInfo.Where(x => x.name_ == "FireDash").FirstOrDefault()?.icon_ ?? defaultSprite;
        int b = 0;
        for (int i = 0; i < Inventory.itemPanels.GetLength(0); i++)
        {
            for (int j = 0; j < Inventory.itemPanels.GetLength(1); j++)
            {
                Inventory.itemPanels[i, j] = inventoryPanel[b];
                b++;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            RangeShow();
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
            RangeHide();
        KeyCheck(KeyCode.Q, Qrenderer);
        KeyCheck(KeyCode.W, Wrenderer);
        KeyCheck(KeyCode.E, Erenderer);
        KeyCheck(KeyCode.R, Rrenderer);
        KeyCheck(KeyCode.F, Frenderer);
        MouseEnterColorChange();
        mousePos = Input.mousePosition;
        if (mousePos.x >= Screen.width - vidstup && Camera.main.transform.position.x - 
            Hero.Player.transform.position.x < maxDistance) transform.position += (Vector3)new Vector2(camera_speed * 0.01f, 0);
        if (mousePos.x <= vidstup && Camera.main.transform.position.x - 
            Hero.Player.transform.position.x > -maxDistance) transform.position += (Vector3)new Vector2(camera_speed * -0.01f, 0);
        if (mousePos.y >= Screen.height - vidstup && Camera.main.transform.position.y - 
            Hero.Player.transform.position.y < maxDistance) transform.position += (Vector3)new Vector2(0, camera_speed * 0.01f);
        if (mousePos.y <= vidstup && Camera.main.transform.position.y - 
            Hero.Player.transform.position.y > -maxDistance) transform.position += (Vector3)new Vector2(0, camera_speed * -0.01f);
        
        Q_reloadIndicator.fillAmount = Hero.Q_currentReload / Hero.Q_Reload;
        W_reloadIndicator.fillAmount = Hero.W_currentReload / Hero.W_Reload;
        E_reloadIndicator.fillAmount = Hero.E_currentReload / Hero.E_Reload;
        R_reloadIndicator.fillAmount = Hero.R_currentReload / Hero.R_Reload;
        F_reloadIndicator.fillAmount = Hero.F_currentReload / Hero.F_Reload;
    }
    private void KeyCheck(KeyCode a, Image b)
    {
        if(Input.GetKey(a))
            b.color = Color.gray;
        else
            b.color = Color.white;
    }
    private void MouseEnterColorChange()
    {
        if (PreEnter != null)
        {
            PreEnter.GetComponent<SpriteRenderer>().color = Color.white;
            PreEnter = null;
        }
        GameObject Entered;
        if (Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2f) != null)
        {
            Entered = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2f).gameObject;
            if(Entered.tag == "Enemy")
            {
                Entered.GetComponent<SpriteRenderer>().color = Color.red;
                PreEnter = Entered;
            }
        }
    }
    private void RangeShow()
    {
        range.SetActive(true);
        range.transform.position = (Vector2)Hero.Player.transform.position;
        range.transform.localScale = (Vector3)new Vector2(Hero.Player.GetComponent<Hero>().AttackRadius*2, Hero.Player.GetComponent<Hero>().AttackRadius*2);
    }
    private void RangeHide()
    {
        range.SetActive(false);
    }
    public void RestartGame()
    {
        Achives.countOfBandits = 5;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
