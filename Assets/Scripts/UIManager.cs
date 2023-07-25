using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   public TextMeshProUGUI stageUI;
   public TextMeshProUGUI moneyUI;
   public TextMeshProUGUI gameOverUI;
   public Canvas canvas;
   public GameObject loadingCanvas;
   public Image hpBar;
   public Button upgradePopupButton;
   public GameObject upgradeUI;
   public GameObject retryButton;
   public List<TextMeshProUGUI> leveltext;
   public bool upgradeUIon = false;
   public string text;
   
   private Button[] _upgradeButtons;
   private Player _player;
   private UnityAction _action;
   private List<string> _buttonString=new();
   public void OnAwake()
   {
      _player = GameManager.Instance.ReturnPlayer().GetComponent<Player>();
      _upgradeButtons = upgradeUI.GetComponentsInChildren<Button>();
      
      for (int i = 0; i < _upgradeButtons.Length; i++)
      {
         _buttonString.Add(_upgradeButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
         _upgradeButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text +="\n"+ _player.upgradeMoney[i];
         leveltext[i].text += " " + _player.upgradLevel[i];
      }
      GameManager.Instance.stageTotal.Subscribe(data =>
      {
         text = "STAGE " + data;
         stageUI.text = text;
         StartCoroutine("FadeInOut",stageUI);
      });
      GameManager.Instance.ReturnPlayer().GetComponent<Player>().hp.Where(x => x <= 0).Subscribe(data =>
      {
         StartCoroutine(FadeIn(gameOverUI));
         StartCoroutine(FadeInButton(retryButton));
      });
   }
   
   public void Init()
   {
      _player.hp.Subscribe(data =>
      {
         hpBar.fillAmount = _player.hp.Value / _player.stat.maxHp;
      });
      GameManager.Instance.money.Subscribe(data =>
      {
         string chageString = "Money: " + GameManager.Instance.money.ToString();
         moneyUI.text=string.Format(" {0}",chageString);
      });
   }
   public void Fade(TextMeshProUGUI txt,float alpha)
   {
      Color targetcolor;
      targetcolor = txt.color;
      targetcolor.a = alpha;
      txt.color = Color.Lerp(txt.color, targetcolor, Time.deltaTime);
   }
   public void Fade(Image img,float alpha)
   {
      Color targetcolor;
      targetcolor = img.color;
      targetcolor.a = alpha;
      img.color = Color.Lerp(img.color, targetcolor, Time.deltaTime);
   }
   public void UpgradeLevelUp(int i)
   {
      if (GameManager.Instance.money.Value != 0)
      {
         if (GameManager.Instance.money.Value >= _player.upgradeMoney[i]+_player.upgradLevel[i].Value * _player.upgradeMoney[i])
         {
            GameManager.Instance.money.Value -= _player.upgradeMoney[i] + _player.upgradLevel[i].Value * _player.upgradeMoney[i];
            _player.upgradLevel[i].Value++;
            _upgradeButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _buttonString[i]+"\n"+
               (_player.upgradeMoney[i] + _player.upgradLevel[i].Value * _player.upgradeMoney[i]);
            leveltext[i].text = _buttonString[i] + " " + _player.upgradLevel[i].Value;
            
            GameManager.Instance.soundpool.Get(3);
         }
      }
   }
   public void UpgradeButtonClick()
   {
      StopCoroutine("MoveUpgradeUI");
      upgradeUIon = !upgradeUIon;
      Vector3 targetpos = upgradeUI.GetComponent<RectTransform>().anchoredPosition;
      if (upgradeUIon)
      {
         targetpos.x = -12f;
         StartCoroutine(MoveUpgradeUI(targetpos));
      }
      else
      {
         targetpos.x = 220f;
         StartCoroutine(MoveUpgradeUI(targetpos));
      }
   }
   public void GotoTitle()
   {
      SceneManager.LoadScene("MainMenu");
   }
   IEnumerator FadeIn(TextMeshProUGUI txt)
   {
      StopCoroutine("FadeOut");
      Color color = txt.color;
      while (txt.color.a<0.8f)
      {
         Fade(txt,0.8f);
         if (txt.color.a>=0.7f)
         {
            color.a = 0.8f;
            txt.color= color;
         }
         yield return new WaitForSeconds(0);
      }
   }
   IEnumerator FadeInButton(GameObject button)
   {
      Color imgColor = button.GetComponent<Image>().color;
      Color textColor = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
      while (button.GetComponent<Image>().color.a<0.8f)
      {
         Fade(button.GetComponent<Image>(),0.8f);
         if (button.GetComponent<Image>().color.a>=0.7f)
         {
            imgColor.a = 0.8f;
            textColor.a = 0.8f;
            button.GetComponent<Image>().color= imgColor;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;
         }
         yield return new WaitForSeconds(0);
      }

      button.GetComponent<Button>().interactable = true;
   }

   IEnumerator FadeOut(TextMeshProUGUI txt)
   {
      StopCoroutine("FadeIn");
      Color color = txt.color;
      while (txt.color.a>0)
      {
         Fade(txt,0);
         if (txt.color.a<=0.01f)
         {
            color.a = 0;
            txt.color= color;
         }
         yield return new WaitForSeconds(0);
      }
   }
   IEnumerator FadeInOut(TextMeshProUGUI txt)
   {
      StartCoroutine(FadeIn(txt));
      yield return new WaitForSeconds(3.0f);
      StartCoroutine(FadeOut(txt));
   }
   IEnumerator MoveUpgradeUI(Vector3 targetPos)
   {
      while (!Mathf.Approximately(upgradeUI.GetComponent<RectTransform>().anchoredPosition.x,targetPos.x))
      {
         upgradePopupButton.GetComponent<Button>().interactable = false;
         upgradeUI.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(upgradeUI.GetComponent<RectTransform>().anchoredPosition, targetPos, 20);
         yield return new WaitForSeconds(0);
      }
      upgradePopupButton.GetComponent<Button>().interactable = true;
   }
}
