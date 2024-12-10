using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
    Slider HPBar;
    TMP_Text KeyCountText;
    GameObject GameOverImage;
    GameObject GameClearImage;
    Button GameStartButton;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        HPBar = canvas.GetComponentInChildren<Slider>();
        KeyCountText = GameObject.Find("KeyCountText").GetComponent<TMP_Text>();
        GameOverImage = GameObject.Find("GameOverImage");
        GameClearImage = GameObject.Find("GameClearImage");
        GameStartButton = canvas.GetComponentInChildren<Button>();
    }

    public void SetInfo(Player owner)
    {
        HPBar.maxValue = owner.MaxHp;
        HPBar.value = owner.Hp;
        KeyCountText.text = Managers.Game.Inventory.KeyCount.ToString();
        GameOverImage.SetActive(false);
        GameClearImage.SetActive(false);
        GameStartButton.gameObject.SetActive(false);

        Managers.Game.KeyCountChanged -= OnKeyCountChanged;
        Managers.Game.KeyCountChanged += OnKeyCountChanged;
    }

    public void SetHpRatio(float ratio)
    {
        StartCoroutine(CoSmoothHpChange(ratio));
    }

    IEnumerator CoSmoothHpChange(float ratio)
    {
        float currentRatio = HPBar.value;
        while (Mathf.Abs(currentRatio - ratio) > 0.01f)
        {
            currentRatio = Mathf.Lerp(currentRatio, ratio, 0.5f);
            HPBar.value = currentRatio;
            yield return null;
        }

        HPBar.value = ratio;
    }

    void OnKeyCountChanged(int keyCount)
    {
        KeyCountText.text = keyCount.ToString();
    }

    public void GameOver()
    {
        GameOverImage.SetActive(true);
        GameStartButton.gameObject.SetActive(true);
    }

    public void GameClear()
    {
        GameClearImage.SetActive(true);
        GameStartButton.gameObject.SetActive(true);
    }

    public void OnClickGameStartButton()
    {
        Managers.Sound.Stop(Define.ESound.Bgm);
        Managers.Scene.LoadScene(Define.EScene.LobbyScene);
    }
}