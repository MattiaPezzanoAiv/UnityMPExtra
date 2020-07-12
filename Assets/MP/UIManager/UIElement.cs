using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    public RectTransform RectTransform { get; private set; }

    [SerializeField]
    private RectTransform m_animationRoot;

    protected virtual void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    protected virtual void OnStartShowing()
    {

    }

    protected virtual void OnEndShowing()
    {

    }

    protected virtual void OnStartHiding()
    {

    }

    protected virtual void OnEndHiding()
    {

    }

    protected virtual void ShowAnimation()
    {
        UIAnimationsConstants.EnterVertical(m_animationRoot, OnEndShowing);
    }

    protected virtual void HideAnimation()
    {
        UIAnimationsConstants.ExitVertical(m_animationRoot,
            () =>
            {
                OnEndHiding();
                gameObject.SetActive(false);
            });
    }

    public void Show()
    {
        gameObject.SetActive(true);
        OnStartShowing();
        ShowAnimation();
    }

    public void Hide()
    {
        OnStartHiding();
        HideAnimation();
    }
}
