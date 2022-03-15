using UnityEngine;
using UnityEngine.Video;

namespace VUPenalty
{
    public class VideoDisplay : MonoBehaviour
    {
        [SerializeField] VideoClip _video;
        [SerializeField] VideoPlayer _videoPlayer;

        public void SetSize(float width, float height)
        {
            transform.localScale = new Vector3(width, height, 1f);
        }

        public void LoadVideo(VideoClip video)
        {
            _videoPlayer.GetComponent<Renderer>().enabled = false;
            _videoPlayer.clip = video;
            // _videoPlayer.Play();
            // _videoPlayer.Pause();
            
        }

        public void Play()
        {
            _videoPlayer.GetComponent<Renderer>().enabled = true;
            _videoPlayer.Play();
        }

        void Awake()
        {
            _videoPlayer.isLooping = true;
        }
    }
}