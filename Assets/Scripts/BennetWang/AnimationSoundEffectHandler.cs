using UnityEngine;

namespace BennetWang
{
    public class AnimationSoundEffectHandler : MonoBehaviour
    {
        public void InformManagerPlayOneShot(SoundManager.Clip clip)
        {
            SoundManager.Instance.PlaySoundEffect(clip);
        }
    }
}