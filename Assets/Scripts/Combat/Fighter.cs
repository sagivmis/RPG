using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 3f;
        [SerializeField] float timeBetweenAttacks = 1.05f;
        [SerializeField] float primaryWeaponDamage = 5f;

        Animator animator;
        Transform target;
        float timeSinceLastAttack = 0f;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateTimers();

            if (target == null) return;
            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();

                    AttackBehaviour();
            }
        }

        private void UpdateTimers()
        {

                timeSinceLastAttack += Time.deltaTime;

        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                animator.SetTrigger("attack");
                //Triggers player's attack's animation event
                
                timeSinceLastAttack = 0f;
            }
        }

        //Animation event
        void HalfOrcSwordRegHit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(primaryWeaponDamage);
        }
        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<Scheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }

}