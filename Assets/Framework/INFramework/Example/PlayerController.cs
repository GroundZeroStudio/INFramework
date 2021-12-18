using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class PlayerController : MonoBehaviour
	{
		protected Animator animator;

        private float _blendSpeed = 0.5f;  //动画混合速度
        private float _currentBS = 0.0f;   //当前混合值


        private void Start()
        {
            animator = this.gameObject.GetComponent<Animator>();
            _currentBS = _blendSpeed;
            animator.SetFloat("TurnSpeed", _currentBS);
        }

        private void Update()
        {
            float speed = 0;
            if (Input.GetKey(KeyCode.W))
            {
                if(_currentBS > _blendSpeed)
                {
                    _currentBS -= Time.deltaTime * _blendSpeed;
                }
                else if(_currentBS < _blendSpeed)
                {
                    _currentBS += Time.deltaTime * _blendSpeed;
                }
                speed = 0.2f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _currentBS -= Time.deltaTime * _blendSpeed;
                if(_currentBS < 0)
                {
                    _currentBS = 0;
                }
                speed = 0.2f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _currentBS += Time.deltaTime * _blendSpeed;
                if (_currentBS > 1f)
                {
                    _currentBS = 1;
                }
                speed = 0.2f;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 1.1f;
            }
            Debug.Log(_currentBS);
            animator.SetFloat("Speed", speed);
            animator.SetFloat("TurnSpeed", _currentBS);
        }
    }
}

