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
        [SerializeField] float bossRange = 1f;
        [SerializeField] float timeBetweenAttacks = 1.05f;
        [SerializeField] float primaryWeaponDamage = 5f;

        Animator animator;
        public Health target;
        Mover mover;
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
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            UpdateTimers();

            if (target == null) return;

            if (target.IsDead()) return;

            if (!IsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
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
            if(target.gameObject.tag == "Boss") return Vector3.Distance(transform.position, target.transform.position) <= weaponRange+bossRange;
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
            mover.Cancel();
            target = null;
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }
    }

}