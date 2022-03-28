using System;
using System.Collections.Generic;
using UnityEngine;

namespace VUPenalty
{
    public class Goalkeeper : MonoBehaviour
    {
        public JumpDirection JumpDirection;
        [SerializeField] Animator _animator;
        [SerializeField] List<SkinnedMeshRenderer> _bodyMaterial;

        public event Action<KeeperDiveEvent> OnKeeperDive;

        public void SetGoalkeeperColor(Texture texture)
        {
            foreach (var skinnedRenderer in _bodyMaterial)
                skinnedRenderer.material.mainTexture = texture;
        }

        public void Dive()
        {
            var keeperDiveEvent = new KeeperDiveEvent(JumpDirection);
            
            OnKeeperDive?.Invoke(keeperDiveEvent);
            switch (JumpDirection)
            {
                case JumpDirection.Left:
                    _animator.SetTrigger("DiveLeft");
                    break;
                case JumpDirection.Right:
                    _animator.SetTrigger("DiveRight");
                    break;
                default:
                    Debug.LogWarning($"Jump direction {JumpDirection} not implemented");
                    break;
            }
        }
    }
}