using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class ObjectMove : MonoBehaviour
{
    [System.Serializable]
    public class Trigger
    {
        public string conversationName;
        public GameObject targetObject;

        public enum TimingType { OnStart, OnEnd }
        public TimingType triggerTiming = TimingType.OnEnd;

        public enum VisibilityType { Show, Hide }  // èoåªÇ©îÒï\é¶Ç©
        public VisibilityType visibility = VisibilityType.Show;

        public enum EffectType { None, Fade, Scale, Slide }
        public EffectType effect = EffectType.None;

        public float duration = 0.5f;

        [Range(0f, 1f)] public float startAlpha = 0f;
        [Range(0f, 1f)] public float endAlpha = 1f;

        public Vector3 startScale = Vector3.zero;
        public Vector3 endScale = Vector3.one;

        public Vector3 slideOffset = new Vector3(-500f, 0f, 0f);
    }

    public Trigger[] triggers;

    void OnEnable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted += OnConversationStarted;
            DialogueManager.instance.conversationEnded += OnConversationEnded;
        }
    }

    void OnDisable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted -= OnConversationStarted;
            DialogueManager.instance.conversationEnded -= OnConversationEnded;
        }
    }

    void OnConversationStarted(Transform actor)
    {
        string startedConversation = DialogueManager.lastConversationStarted;

        foreach (var trigger in triggers)
        {
            if (trigger.triggerTiming == Trigger.TimingType.OnStart &&
                trigger.conversationName == startedConversation)
            {
                ApplyEffect(trigger);
            }
        }
    }

    void OnConversationEnded(Transform actor)
    {
        string endedConversation = DialogueManager.lastConversationStarted;

        foreach (var trigger in triggers)
        {
            if (trigger.triggerTiming == Trigger.TimingType.OnEnd &&
                trigger.conversationName == endedConversation)
            {
                ApplyEffect(trigger);
            }
        }
    }

    void ApplyEffect(Trigger trigger)
    {
        if (trigger.targetObject == null) return;

        bool isShowing = (trigger.visibility == Trigger.VisibilityType.Show);

        if (isShowing)
        {
            trigger.targetObject.SetActive(true);
        }

        switch (trigger.effect)
        {
            case Trigger.EffectType.Fade:
                var cg = trigger.targetObject.GetComponent<CanvasGroup>();
                if (cg == null) cg = trigger.targetObject.AddComponent<CanvasGroup>();

                DOTween.Kill(cg); // à¿ëSÇÃÇΩÇﬂ

                cg.alpha = trigger.startAlpha;
                cg.interactable = true;
                cg.blocksRaycasts = true;

                cg.DOFade(trigger.endAlpha, trigger.duration)
                    .SetEase(Ease.Linear)
                    .SetId(cg)
                    .OnComplete(() =>
                    {
                        if (!isShowing)
                        {
                            cg.interactable = false;
                            cg.blocksRaycasts = false;
                            trigger.targetObject.SetActive(false);
                        }
                    });
                break;

            case Trigger.EffectType.Scale:
                Vector3 fromScale = isShowing ? trigger.startScale : trigger.endScale;
                Vector3 toScale = isShowing ? trigger.endScale : trigger.startScale;

                trigger.targetObject.transform.localScale = fromScale;
                trigger.targetObject.transform
                    .DOScale(toScale, trigger.duration)
                    .SetEase(isShowing ? Ease.OutBack : Ease.InBack)
                    .OnComplete(() =>
                    {
                        if (!isShowing)
                        {
                            trigger.targetObject.SetActive(false);
                        }
                    });
                break;

            case Trigger.EffectType.Slide:
                Vector3 basePos = trigger.targetObject.transform.position;
                Vector3 fromPos = isShowing ? basePos + trigger.slideOffset : basePos;
                Vector3 toPos = isShowing ? basePos : basePos + trigger.slideOffset;

                trigger.targetObject.transform.position = fromPos;
                trigger.targetObject.transform
                    .DOMove(toPos, trigger.duration)
                    .SetEase(isShowing ? Ease.OutCubic : Ease.InCubic)
                    .OnComplete(() =>
                    {
                        if (!isShowing)
                        {
                            trigger.targetObject.SetActive(false);
                        }
                    });
                break;

            case Trigger.EffectType.None:
                trigger.targetObject.SetActive(isShowing);
                break;
        }
    }
}
