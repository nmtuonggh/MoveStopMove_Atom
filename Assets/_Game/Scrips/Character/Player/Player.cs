using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;


enum PlayerState
{
    Attacked, Attacking, Dead, Run, Idle
}
public class   Player : Character
{
    private Vector3 moveVector;
    [SerializeField] private FixedJoystick _joystick;

    private PlayerState _state;


    protected override void Start()
    {
        base.Start();
        OnInit();
    }

    void OnInit()
    {
        _state = PlayerState.Idle;
        ChangeAnim(Constan.ANIM_IDLE);
    }

    protected override void Update()
    {
        if (_state is PlayerState.Attacked || _state is PlayerState.Dead)
        {
            return;
        }

        Run();
        if (targetAttack != null && targetAttack.GetComponent<Bot>().CurrentState is DieState)
        {
            L_AttackTarget.Remove(targetAttack);
            if (l_AttackTarget.Count > 0)
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
        }
        if (l_AttackTarget.Count > 0)
        {

            if (!l_AttackTarget.Contains(targetAttack))
                targetAttack = l_AttackTarget[Random.Range(0, l_AttackTarget.Count)];
        }

        if (l_AttackTarget.Contains(targetAttack) && timer >= delayAttack)
        {
            Attack();
            timer = 0;
        }
        
    }

    public override void Run()
    {
        base.Run();
        moveVector = Vector3.zero;
        moveVector.x = _joystick.Horizontal * _moveSpeed * Time.deltaTime;
        moveVector.z = _joystick.Vertical * _moveSpeed * Time.deltaTime;
        
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            StopAllCoroutines();
            isReadyAttack = true;
            _state = PlayerState.Run;
            timer = 0;
            Vector3 direction =
                Vector3.RotateTowards(transform.forward, moveVector, _rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            ChangeAnim(Constan.ANIM_RUN);
        }
        else if (_joystick.Horizontal == 0 && _joystick.Vertical == 0)
        {
            if (_state is PlayerState.Attacked or PlayerState.Attacking)
            {
            }
            else
            {
                timer += Time.deltaTime;
                _state = PlayerState.Idle;
                ChangeAnim(Constan.ANIM_IDLE);
            }

            transform.position = Vector3.Lerp(transform.position, transform.position + moveVector, 1f);
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackTime);
        _state = PlayerState.Idle;
    }

    IEnumerator ActiveAttack()
    {
        yield return new WaitForSeconds(waitThrow);
        _state = PlayerState.Attacked;
    }
    public override void Attack()
    {
        if (!isReadyAttack)
        {
            return;
        }
        base.Attack();
        _state = PlayerState.Attacking;
        StartCoroutine(ActiveAttack());
        StartCoroutine(ResetAttack());
    }
}
