using DG.Tweening;
using UnityEngine;

public class PointFlyEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private Ease easeType = Ease.InExpo;
    [SerializeField] private RectTransform iconPrefab;

    public void SetTarget(Vector3 startPos, RectTransform target, int points)
    {
        Sequence seq = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < points; i++)
        {
            RectTransform rect = Instantiate(iconPrefab, transform);
            Vector3 randomOffset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f);
            rect.position = startPos + randomOffset;
            
            float delay = Random.Range(0.01f, 0.4f);

            seq.Insert(delay, rect.DOMove(target.position, duration).SetEase(easeType));
            seq.Insert(delay + 0.1f, rect.DOScale(1.4f, duration * 0.6f).SetLoops(2, LoopType.Yoyo));
        }
        
        seq.OnComplete(() =>
        {
            ScorePoints(points);
        });
    }

    private void ScorePoints(int points)
    {
        GUIController.Instance.AddPoints(points);
        Destroy(gameObject);
    }
    
    
}
