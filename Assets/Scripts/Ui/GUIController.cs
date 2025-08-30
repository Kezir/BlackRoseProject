using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GUIController : MonoBehaviour
{

    #region singleton

    public static GUIController Instance;

    private void Awake()
    {
        DisableOnStartObject.SetActive(false);
        Instance = this;
    }

    #endregion

    [SerializeField] private GameObject DisableOnStartObject;
    [SerializeField] private RectTransform ViewsParent;
    [SerializeField] private GameObject InGameGuiObject;
    [SerializeField] private PopUpView PopUp;
    [SerializeField] private PopUpScreenBlocker ScreenBlocker;
    [SerializeField] private Selectable defaultSelected;
    [SerializeField] private Text scoreText;
    [SerializeField] private PointFlyEffect pointEffectPrefab;
    private int score;
    
    private void Start()
    {
        if (ScreenBlocker) ScreenBlocker.InitBlocker();

        GameEvents.EnemyKilled += UpdateScore;
    }

    private void OnDestroy()
    {
        GameEvents.EnemyKilled -= UpdateScore;
    }

    private void SelectDefault()
    {
        NavigationManager.Instance.SelectGameObject(defaultSelected);
    }

    private void ActiveInGameGUI(bool active)
    {
        InGameGuiObject.SetActive(active);
        
        if(!active)
            return;
        
        SelectDefault();
    }

    private void UpdateScore(IEnemy enemy)
    {
        UpdateScoreWithEffect(Camera.main.WorldToScreenPoint(enemy.GetEnemyPosition().Position.position),enemy.GetEnemyPoints());
    }

    public void UpdateScoreWithEffect(Vector3 startPosition, int amount)
    {
        var effect = Instantiate(pointEffectPrefab, transform);
        effect.SetTarget(startPosition, scoreText.rectTransform, amount);
    }
    
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void ShowPopUpMessage(PopUpInformation popUpInfo, UiView uiView)
    {
        PopUpView newPopUp = Instantiate(PopUp, ViewsParent) as PopUpView;
        newPopUp.SetParentView(uiView);
        newPopUp.ActivePopUpView(popUpInfo);
    }

    public void ActiveScreenBlocker(bool active, PopUpView popUpView)
    {
        if (active) ScreenBlocker.AddPopUpView(popUpView);
        else ScreenBlocker.RemovePopUpView(popUpView);
    }


    #region IN GAME GUI Clicks

    public void InGameGUIButton_OnClick(UiView viewToActive)
    {
        viewToActive.ActiveView(() => ActiveInGameGUI(true));

        ActiveInGameGUI(false);
        GameControlller.Instance.IsPaused = true;
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
    
    #endregion
    
}