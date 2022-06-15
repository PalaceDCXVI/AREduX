using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class PlayMovieOnSpace : MonoBehaviour
{
    public UnityEngine.Video.VideoClip videoClip;
    public VideoPlayer videoPlayer;

    private void Start()
    {
        var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        var audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }
    void Update()
    {
        // MovieTexture doesn't work on webgl or phones (?)
#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
        {
            videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
            videoPlayer.targetMaterialProperty = videoPlayer.targetMaterialRenderer.material.mainTexture.name;

            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else {
                videoPlayer.Play();
            }
        }
#endif
    }
}
