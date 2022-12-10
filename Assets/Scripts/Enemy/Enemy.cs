using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //health
    private int health = 3;
    //animator
    private Animator _anim;
    //Ai
    [SerializeField]
    private float attackDistance = 2f;
    private Transform _playerTrans;
    private Transform _trans;
    // weapon colider
    [SerializeField]
    private Collider2D weaponCol;

    public SimpleFlash _flash {get; private set;}

    private void Awake()
    {
        _flash = GetComponent<SimpleFlash>();
        _trans = GetComponent<Transform>();
        _anim = GetComponent<Animator>();
        weaponCol.enabled = false;
    }
    private void Start()
    {
        _playerTrans = PlayerControler._instance.GetComponent<Transform>();
    }

    void Update()
    {
        CheckRotation();
        CheckAttack();
    }

    // check for rotaion
    private void CheckRotation()
    {
        if(_playerTrans == null) return;
        if (_trans.position.x < _playerTrans.position.x)
        {
            _trans.rotation = Quaternion.Euler(0, 180, 0);
            return;
        }
        _trans.rotation = Quaternion.Euler(0, 0, 0);
    }

    #region Attack
    // Attack Methods
    private void CheckAttack()
    {
        if (AttackDistance() < attackDistance)
        {
            _anim.SetBool("Attacking", true);
        }
        else if (AttackDistance() > attackDistance)
        {
            _anim.SetBool("Attacking", false);
        }
        //isTouchingPlayer = Physics2D.OverlapCircle(touchingPlayer.position, .2f);
    }
    private float AttackDistance()
    {
        if(_playerTrans == null) return attackDistance + 1;
        return Vector3.Distance(_playerTrans.position, _trans.position);
    }
    #endregion
    #region GetDamage
    //enemy looses health, when health reaches zero Die() function is called
    public void TakeDamage(int damage)
    {
        health -= damage;
        _flash.Flash();
        if (health <= 0)
        {
            _anim.SetBool("isDead", true);
            var col = GetComponent<Collider2D>();
            col.enabled = false;
            weaponCol.enabled = false;
        }
    }
    #endregion
    #region Animator events

    private void ActivateWeapon()
    {
        weaponCol.enabled = true;
    }

    private void DisableWeapon()
    {
        weaponCol.enabled = false;
    }

    //object is destroyed
    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
