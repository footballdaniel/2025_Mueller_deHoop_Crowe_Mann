using UnityEngine;
using UnityEngine.Video;

namespace VUPenalty
{
    [ExecuteInEditMode]
    public class VideoDisplay : MonoBehaviour
    {

        public VideoClip Video;
        [SerializeField] VideoPlayer _videoPlayer;

        public void SetSize(float width, float height)
        {
            transform.localScale = new Vector3(width, height, 1f);
        }

        public void Play()
        {
            _videoPlayer.clip = Video;
            _videoPlayer.Play();
        }
    }
}
