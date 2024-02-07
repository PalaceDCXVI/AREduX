using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioPromptManager : MonoBehaviour
{
    public static AudioPromptManager Instance;

    private AudioSource audioSource;

    public AudioClip StartingClip;
    public string StartingClipSubtitles = "";

    public float startingClipDelay = 10.0f;
    private bool startingClipHasPlayed = false;
    private bool tasksUnderway = false;

    public GameObject ScrollViewObject = null;
    private ScrollRect SubtitleView = null;
    public Text SubtitleText = null;
    public float SubtitleViewBackgroundTransparency = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("AudioPromptManager already exists.");
            return;
        }

        audioSource = GetComponent<AudioSource>();
        
        SubtitleView = ScrollViewObject.GetComponent<ScrollRect>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ScenarioManager.Instance.ScenarioHasStarted)
        {
            startingClipDelay -= Time.deltaTime;
            if (startingClipDelay <= 0.0f && !startingClipHasPlayed)
            {
                PlayAudioClip(StartingClip, StartingClipSubtitles);

                startingClipHasPlayed = true;
            }

            if (startingClipDelay <= -StartingClip.length - 1.0f && !tasksUnderway)
            {
                ScenarioManager.Instance.currentTask.StartTask();
                tasksUnderway = true;
            }
        }

        if (audioSource != null && audioSource.isPlaying && SubtitleView != null && SubtitleText.text != "")
        {
            float currentTime = audioSource.time / audioSource.clip.length;
            SubtitleView.verticalScrollbar.value = Mathf.Clamp01(1.0f - (Mathf.Log(currentTime) + 1.1f));
            UnityEngine.UI.Image viewBackground = SubtitleView.GetComponent<UnityEngine.UI.Image>();
            Color fadeColor = new Color(viewBackground.color.r, viewBackground.color.g, viewBackground.color.b, SubtitleViewBackgroundTransparency);
            viewBackground.color = fadeColor;
        }

        if (audioSource != null && !audioSource.isPlaying && SubtitleText != null) 
        {
            SubtitleText.text = "";
            UnityEngine.UI.Image viewBackground = SubtitleView.GetComponent<UnityEngine.UI.Image>();
            Color fadeColor = new Color(viewBackground.color.r, viewBackground.color.g, viewBackground.color.b, 0.0f);
            viewBackground.color = fadeColor;
        }
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.Stop();

        audioSource.clip = audioClip;

        audioSource.Play();

        if (SubtitleText != null)
        {
            SubtitleText.text = "";
        }
    }

    public void SetSubtitles(string subtitle)
    {
        if (SubtitleText != null)
        {
            SubtitleText.text = subtitle;
        }
    }

    public void PlayAudioClip(AudioClip audioClip, string subTitleText = "")
    {
        audioSource.Stop();

        audioSource.clip = audioClip;

        audioSource.Play();

        if (SubtitleText != null)
        {
            SubtitleText.text = subTitleText;
        }
    }

}
