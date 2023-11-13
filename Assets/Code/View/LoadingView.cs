using UnityEngine;

namespace Code.View
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private Animation animation;

        public void StartLoadingAnimation()
        {
            animation.Play();
        }
        
        public void StopLoadingAnimation()
        {
            animation.Stop();
        }
    }
}