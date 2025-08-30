using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoulEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject InteractionPanelObject;
    [SerializeField] private Selectable deafaultInteractionPanelObject;
    [SerializeField] private GameObject ActionsPanelObject;
    [SerializeField] private Selectable deafaultActionPanelObject;
    [SerializeField] private SpriteRenderer EnemySpriteRenderer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image bow, sword;
    private SoulWeakness _soulWeakness;
    private int points = 10, bonusPoints = 0;

    private SpawnPoint _enemyPosition;

    public void SetupEnemy(Sprite sprite, SpawnPoint spawnPoint)
    {
        bonusPoints = 0;
        EnemySpriteRenderer.sprite = sprite;
        _enemyPosition = spawnPoint;
        gameObject.SetActive(true);
        GameControlller.Instance.OnPaused += OnPaused;
        _soulWeakness = (SoulWeakness)Random.Range(0, 2);
        
    }

    private void OnPaused(bool value)
    {
        canvasGroup.interactable = !value;
    }

    public void OnDestroyed()
    {
        GameControlller.Instance.OnPaused -= OnPaused;
    }

    public SpawnPoint GetEnemyPosition()
    {
        return _enemyPosition;
    }

    public GameObject GetEnemyObject()
    {
        return this.gameObject;
    }

    public int GetEnemyPoints()
    {
        return points + bonusPoints;
    }

    private void ActiveCombatWithEnemy()
    {
        ActiveInteractionPanel(false);
        ActiveActionPanel(true);

        if (_soulWeakness == SoulWeakness.BOW)
        {
            bow.rectTransform.DOKill(true);
            bow.rectTransform.DOPunchRotation(new Vector3(5,5,5), 2f).SetLoops(3, LoopType.Restart);
        }
        else
        {
            sword.rectTransform.DOKill(true);
            sword.rectTransform.DOPunchRotation(new Vector3(5,5,5), 2f).SetLoops(3, LoopType.Restart);
        }
    }

    public void DeactivateCombatWithEnemy()
    {
        ActiveInteractionPanel(true);
        ActiveActionPanel(false);
    }

    private void ActiveInteractionPanel(bool active)
    {
        InteractionPanelObject.SetActive(active);
        
        if(!active)
            return;
        
        NavigationManager.Instance.SelectGameObject(deafaultInteractionPanelObject);
    }

    private void ActiveActionPanel(bool active)
    {
        ActionsPanelObject.SetActive(active);
        
        if(!active)
            return;
        
        NavigationManager.Instance.SelectGameObject(deafaultActionPanelObject);
    }
    
    public void Select()
    {
        NavigationManager.Instance.SelectGameObject(deafaultInteractionPanelObject);
    }

    private void UseBow()
    {
        // USE BOW
        if (_soulWeakness == SoulWeakness.BOW)
            bonusPoints = points / 2;
        GameEvents.EnemyKilled?.Invoke(this);
    }

    private void UseSword()
    {
        if (_soulWeakness == SoulWeakness.SWORD)
            bonusPoints = points / 2;
        GameEvents.EnemyKilled?.Invoke(this);
        // USE SWORD
    }

    #region OnClicks

    public void Combat_OnClick()
    {
        ActiveCombatWithEnemy();
    }

    public void Bow_OnClick()
    {
        UseBow();
    }

    public void Sword_OnClick()
    {
        UseSword();
    }

    #endregion
    
}


public interface IEnemy
{
    SpawnPoint GetEnemyPosition();
    GameObject GetEnemyObject();
    int GetEnemyPoints();
    void OnDestroyed();
}
