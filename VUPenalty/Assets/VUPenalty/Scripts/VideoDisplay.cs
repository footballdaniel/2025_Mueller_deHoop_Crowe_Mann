using System;
using UnityEngine;
using UnityEngine.Video;

namespace VUPenalty
{
    public class VideoDisplay : MonoBehaviour
    {
        [SerializeField] VideoClip _video;
        [SerializeField] VideoPlayer _videoPlayer;

        private void Awake()
        {
            _videoPlayer.isLooping = true;
        }

        public void SetSize(float width, float height)
        {
            transform.localScale = new Vector3(width, height, 1f);
        }

        public void LoadVideo(VideoClip video)
        {
            _videoPlayer.clip = _video;
            _videoPlayer.Play();
            _videoPlayer.Pause();
        }

        public void Play()
        {
            _videoPlayer.Play();
        }
    }
}
