using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spine.Unity;
using TurnBased;
using UnityEngine;

namespace TurnBased
{
    public class AutoFightable : MonoBehaviour, IFightable
    {
        public event Action TurnEnded = () => { };
        
        [SerializeField] private GameObject highlight;
        [SerializeField] private int power;
        
        private GameUI _ui;
        private GameObject _panel;
        private MeshRenderer _renderer;
        private SkeletonAnimation _animator;

        private void Awake()
        {
            highlight.SetActive(false);
            _renderer = GetComponent<MeshRenderer>();
            _animator = GetComponent<SkeletonAnimation>();
        }

        public void Init(GameUI ui, GameObject panel)
        {
            _ui = ui;
            _panel = panel;
        }
        
        public GameObject GetGameObject()
        {
            try
            {
                return gameObject;
            }
            catch
            {
                return null;
            }
        }
        
        public void SetTurn()
        {
            highlight.SetActive(true);
            var coroutine = Attack();
            StartCoroutine(coroutine);
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(0.5f);
            
            highlight.SetActive(false);
            _renderer.sortingOrder = 5;
            _panel.SetActive(true);
            transform.localScale *= 1.5f;
            
            _animator.AnimationState.SetAnimation(0, "Miner_1", false);
            _animator.AnimationState.AddAnimation(0, "Idle", true, 0);
            yield return new WaitForSeconds(1.2f);
            
            transform.localScale /= 1.5f;
            _panel.SetActive(false);
            _renderer.sortingOrder = 0;
            GameManager.Instance.GetRandomPlayableCharacter().Damage(power);
            yield return new WaitForSeconds(0.1f);
            TurnEnded();
        }
    }
}

