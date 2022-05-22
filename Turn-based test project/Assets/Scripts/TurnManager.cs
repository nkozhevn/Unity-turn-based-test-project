using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBased
{
    public class TurnManager : MonoBehaviour
    {
        public Queue<IFightable> charactersInOrder = new Queue<IFightable>();
        [HideInInspector] public bool gameFinished = false;

        public void SetOrder(List<IFightable> characters)
        {
            System.Random random = new System.Random();
            
            int n = characters.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (characters[k], characters[n]) = (characters[n], characters[k]);
            }
            
            foreach (var character in characters)
            {
                charactersInOrder.Enqueue(character);
            }
        }

        public void MakeTurn()
        {
            while (charactersInOrder.Peek().GetGameObject() == null)
            {
                charactersInOrder.Dequeue();
            }

            charactersInOrder.Peek().TurnEnded += NextTurn;
            charactersInOrder.Peek().SetTurn();
        }

        private void NextTurn()
        {
            charactersInOrder.Peek().TurnEnded -= NextTurn;
            if (charactersInOrder.Count > 0 && !gameFinished)
            {
                charactersInOrder.Enqueue(charactersInOrder.Dequeue());
                MakeTurn();
            }
        }
    }
}

