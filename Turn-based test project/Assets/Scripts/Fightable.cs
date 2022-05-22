using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spine.Unity;
using TurnBased;
using Unity.VisualScripting;
using UnityEngine;

namespace TurnBased
{
    public class Fightable : MonoBehaviour, IFightable
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
            _ui.actionButtons.SetActive(true);
            _ui.attackButton.onClick.AddListener(Attack);
            _ui.skipButton.onClick.AddListener(Skip);
        }

        private void Attack()
        {
            _ui.attackButton.onClick.RemoveListener(Attack);
            _ui.skipButton.onClick.RemoveListener(Skip);
            _ui.actionButtons.SetActive(false);
            var coroutine = WaitForTarget();
            StartCoroutine(coroutine);
        }

        private void Skip()
        {
            _ui.attackButton.onClick.RemoveListener(Attack);
            _ui.skipButton.onClick.RemoveListener(Skip);
            _ui.actionButtons.SetActive(false);
            highlight.SetActive(false);
            TurnEnded();
        }

        private IEnumerator WaitForTarget()
        {
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, 
                        ray.direction, Mathf.Infinity);
                    if (hit.collider != null)
                    {
                        var enemy = hit.collider.gameObject.GetComponent<Damageable>();
                        if (enemy != null)
                        {
                            highlight.SetActive(false);
                            enemy.Damage(power);
                            _renderer.sortingOrder = 5;
                            _panel.SetActive(true);
                            transform.localScale *= 1.5f;

                            _animator.AnimationState.SetAnimation(0, "Miner_1", false);
                            _animator.AnimationState.AddAnimation(0, "Idle", true, 0);
                            yield return new WaitForSeconds(1.2f);
                            
                            transform.localScale /= 1.5f;
                            _panel.SetActive(false);
                            _renderer.sortingOrder = 0;
                            yield return new WaitForSeconds(0.1f);
                            TurnEnded();
                            break;
                        }
                    }
                }
                yield return null;
            }
        }
    }
}

