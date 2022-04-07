using System;
using System.Collections.Generic;
using UnityEngine;

namespace VUPenalty
{
    public class Goalkeeper : MonoBehaviour
    {
        public Direction _direction;
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
            var keeperDiveEvent = new KeeperDiveEvent(_direction);
            
            OnKeeperDive?.Invoke(keeperDiveEvent);
            switch (_direction)
            {
                case Direction.Left:
                    _animator.SetTrigger("DiveLeft");
                    break;
                case Direction.Right:
                    _animator.SetTrigger("DiveRight");
                    break;
                default:
                    Debug.LogWarning($"Jump direction {_direction} not implemented");
                    break;
            }
        }
    }
}