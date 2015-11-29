using UnityEngine;

using OptionalText = InspectorEditable.Score.OptionalText;
using OptionalSound = InspectorEditable.Score.OptionalSound;
using OptionalAnimation = InspectorEditable.Score.OptionalAnimation;

public sealed class ScoreUI : MonoBehaviour
{
    [SerializeField]
    Color textColor = Color.white;

    [SerializeField]
    OptionalText optionalText;

    [SerializeField]
    OptionalSound optionalSound;

    [SerializeField]
    OptionalAnimation optionalAnimation;

    void OnEnable()
    {
        optionalText.SetText(Score.earned);
        optionalText.Show();
        Score.onEarn += ScoreEarned;
    }

    void LateUpdate()
    {
        optionalText.SetColor(textColor);
    }

    void OnDisable()
    {
        optionalText.Hide();
        Score.onEarn -= ScoreEarned;
    }

    void ScoreEarned(int points)
    {
        optionalText.SetText(Score.earned);
        optionalAnimation.Trigger();
        optionalSound.Play();
    }

    void Reset()
    {
        InitialComponentsConstruction();
        ResetComponents();
        ProvideComponentsGameObject();
    }

    void Validate()
    {
        ProvideComponentsGameObject();
    }

    void InitialComponentsConstruction()
    {
        if (optionalText == null) optionalText = new OptionalText();
        if (optionalAnimation == null) optionalAnimation = new OptionalAnimation();
        if (optionalSound == null) optionalSound = new OptionalSound();
    }

    void ResetComponents()
    {
        optionalText.Reset();
        optionalAnimation.Reset();
        optionalSound.Reset();
    }

    void ProvideComponentsGameObject()
    {
        optionalText.ChanceToGetDefaultReferences(gameObject);
        optionalAnimation.ChanceToGetDefaultReferences(gameObject);
        optionalSound.ChanceToGetDefaultReferences(gameObject);
    }
}

namespace InspectorEditable.Score
{
    using UnityEngine.UI;
    using System;

    [Serializable]
    public class OptionalText
    {
        [SerializeField]
        Text text;

        [SerializeField]
        string scoreFormat = DefaultFormat;

        const string DefaultFormat = "Total score: {0}";

        public void ChanceToGetDefaultReferences(GameObject gameObject)
        {
            if (!text) text = gameObject.GetComponentInChildren<Text>();
        }

        public void Reset()
        {
            scoreFormat = DefaultFormat;
        }

        public void SetText(int score)
        {
            if (text)
                text.text = TryFormatScoreString(score);
        }

        string TryFormatScoreString(int score)
        {
            try
            {
                return FormatScoreString(score);
            }
            catch
            {
                return FormatDefaultScoreString(score);
            }
        }

        string FormatScoreString(int score)
        {
            return string.Format(scoreFormat, score);
        }

        static string FormatDefaultScoreString(int score)
        {
            return string.Format(DefaultFormat, score);
        }

        public void Show()
        {
            if (text)
                text.enabled = true;
        }

        public void Hide()
        {
            if (text)
                text.enabled = false;
        }

        public void SetColor(Color textColor)
        {
            if (text)
                text.color = textColor;
        }
    }
}

namespace InspectorEditable.Score
{
    using UnityEngine;
    using System;

    [Serializable]
    public class OptionalAnimation
    {
        const string DefaultAnimatorTriggerName = "Scored";

        [SerializeField]
        Animator animator;

        [SerializeField]
        string trigger = DefaultAnimatorTriggerName;

        public void ChanceToGetDefaultReferences(GameObject gameObject)
        {
            if (!animator) animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void Reset()
        {
            trigger = DefaultAnimatorTriggerName;
        }

        public void Trigger()
        {
            if (animator)
                animator.SetTrigger(trigger);
        }
    }
}

namespace InspectorEditable.Score
{
    using UnityEngine;
    using System;

    [Serializable]
    public class OptionalSound
    {
        [SerializeField]
        AudioSource source;

#pragma warning disable 0649
        [SerializeField]
        AudioClip optionalClip;
#pragma warning restore 0649

        [SerializeField]
        bool forceNoLoop = true;

        public void ChanceToGetDefaultReferences(GameObject gameObject)
        {
            if (!source)
                source = gameObject.GetComponentInChildren<AudioSource>();
        }

        public void Reset()
        {
            forceNoLoop = true;
        }

        public void Play()
        {
            OptionallyDisableLooping();
            TryPlayWithOptionalClip();
        }

        void TryPlayWithOptionalClip()
        {
            if (!source || source.isPlaying)
                return;

            if (optionalClip)
                source.PlayOneShot(optionalClip);
            else
                source.Play();
        }

        void OptionallyDisableLooping()
        {
            if (source && forceNoLoop)
                source.loop = false;
        }
    }
}
