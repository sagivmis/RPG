using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat{
    public class Fighter : MonoBehaviour, IAction
    {
        [Header("Weapon Configuration")]
        [SerializeField] float weaponRange = 3f;
        [SerializeField] float timeBetweenAttacks = 1.05f;
        [SerializeField] float primaryWeaponDamage = 5f;

        Animator animator;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        private void OnDrawGizmosSelected()
        {
            if (gameObject.tag == "Player")
            {

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, weaponRange);
            }
        }
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateTimers();

            if (target == null) return;

            if (target.IsDead()) return;

            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
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
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                //Triggers player's attack's animation event

                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        //Animation event
        void HalfOrcSwordRegHit()
        {
            if (target == null) return;
            //print($"hit {target.name}, by {gameObject.name}");
            target.TakeDamage(primaryWeaponDamage);
        }
        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<Scheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest != null && !targetToTest.IsDead());
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }
    }

}