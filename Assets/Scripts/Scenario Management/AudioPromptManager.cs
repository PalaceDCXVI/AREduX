using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPromptManager : MonoBehaviour
{
    public static AudioPromptManager Instance;

    private AudioSource audioSource;

    public AudioClip StartingClip;

    public float startingClipDelay = 10.0f;
    private bool startingClipHasPlayed = false;
    private bool tasksUnderway = false;

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
                audioSource.Stop();

                audioSource.clip = StartingClip;

                audioSource.Play();

                startingClipHasPlayed = true;
            }

            if (startingClipDelay <= -StartingClip.length - 1.0f && !tasksUnderway)
            {
                ScenarioManager.Instance.currentTask.StartTask();
                tasksUnderway = true;
            }
        }
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.Stop();

        audioSource.clip = audioClip;

        audioSource.Play();
    }

}
