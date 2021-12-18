using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum EGunType
    {
        AssaultRifle = 0,
    }

	public class AssaultRifle : MonoBehaviour
	{
        private int id;
        private float damage;
        private float durable;
        private EGunType type;

        public int Id { get => id; set => id = value; }
        public float Damage { get => damage; set => damage = value; }
        public float Durable { get => durable; set => durable = value; }
        public EGunType Type { get => type; set => type = value; }


        public void PlayAudio()
        {

        }

        public void PlayEffect()
        {

        }

        public void UseCommand()
        {

        }
    }
}

