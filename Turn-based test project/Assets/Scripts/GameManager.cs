using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TurnBased
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private List<Transform> playablePlaceholders;
        [SerializeField] private List<Transform> enemyPlaceholders;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private GameUI ui;
        [SerializeField] private GameObject panel;
        
        private List<Damageable> playableCharacters = new List<Damageable>();
        private List<Damageable> enemyCharacters = new List<Damageable>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(gameObject);
            
            panel.SetActive(false);
        }

        private void Start()
        {
            List<IFightable> activeCharacters = new List<IFightable>();
            System.Random random = new System.Random();
            
            Array values = Enum.GetValues(typeof(EPlayableCharacters));
            foreach (var place in playablePlaceholders)
            {
                var randomCharacter = (EPlayableCharacters)values.GetValue(random.Next(values.Length));
                var character = ResourceManager.CreatePrefabInstance<EPlayableCharacters, 
                    Damageable>(randomCharacter, place.position);
                playableCharacters.Add(character);
                character.Dead += () => PlayableDied(character);
                var fightable = character.GetComponent<IFightable>();
                fightable.Init(ui, panel);
                activeCharacters.Add(fightable);
            }
            
            values = Enum.GetValues(typeof(EEnemyCharacters));
            foreach (var place in enemyPlaceholders)
            {
                var randomCharacter = (EEnemyCharacters)values.GetValue(random.Next(values.Length));
                var character = ResourceManager.CreatePrefabInstance<EEnemyCharacters, 
                    Damageable>(randomCharacter, place.position);
                character.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                enemyCharacters.Add(character);
                character.Dead += () => EnemyDied(character);
                var fightable = character.GetComponent<IFightable>();
                fightable.Init(ui, panel);
                activeCharacters.Add(fightable);
            }
            
            turnManager.SetOrder(activeCharacters);
            ui.startButton.onClick.AddListener(StartFight);
        }

        private void StartFight()
        {
            ui.startButton.gameObject.SetActive(false);
            turnManager.MakeTurn();
        }

        private void PlayableDied(Damageable character)
        {
            playableCharacters.Remove(character);
            if (playableCharacters.Count <= 0)
            {
                GameLost();
            }
        }


        private void EnemyDied(Damageable character)
        {
            enemyCharacters.Remove(character);
            if (enemyCharacters.Count <= 0)
            {
                GameWon();
            }
        }

        private void GameLost()
        {
            turnManager.gameFinished = true;
            ui.youLoseText.gameObject.SetActive(true);
        }

        private void GameWon()
        {
            turnManager.gameFinished = true;
            ui.youWinText.gameObject.SetActive(true);
        }

        public Damageable GetRandomPlayableCharacter() => 
            playableCharacters[Random.Range(0, playableCharacters.Count)];
    }
}

