using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TurnBased
{
    public interface IFightable
    {
        public event Action TurnEnded;
        public void Init(GameUI ui, GameObject panel);
        public GameObject GetGameObject();
        public void SetTurn();
    }
}

