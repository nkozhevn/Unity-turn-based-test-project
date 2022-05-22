using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBased
{
    public class Damageable : MonoBehaviour
    {
        public event Action Dead = () => { };

        [SerializeField] private Slider slider;
        [SerializeField] private int maxHealth;

        private int _health;
        private MeshRenderer _renderer;
        private SkeletonAnimation _animator;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health > maxHealth)
                    _health = maxHealth;
                else if (_health <= 0)
                {
                    _health = 0;
                    Die();
                }

                slider.value = (float)_health / maxHealth;
            }
        }

        private void Awake()
        {
            Health = maxHealth;
            _renderer = GetComponent<MeshRenderer>();
            _animator = GetComponent<SkeletonAnimation>();
        }

        public void Damage(int amount)
        {
            var coroutine = Highlight();
            StartCoroutine(coroutine);
            Health -= amount;
        }

        private IEnumerator Highlight()
        {
            _renderer.sortingOrder = 5;
            transform.localScale *= 1.5f;
            
            _animator.AnimationState.SetAnimation(0, "Damage", false);
            _animator.AnimationState.AddAnimation(0, "Idle", true, 0.6f);
            yield return new WaitForSeconds(1.2f);
            
            transform.localScale /= 1.5f;
            _renderer.sortingOrder = 0;
        }

        public void Heal(int amount)
        {
            Health += amount;
        }

        private void Die()
        {
            Dead();
            Destroy(gameObject);
        }
    }
}

