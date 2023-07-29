using UnityEngine;

namespace Framework
{
    public abstract class Actor : MonoBehaviour
    {
        // Reference

        // Property
        protected Actor Owner { get; set; }
        protected Actor Instigator { get; set; }

        // Damage
        protected virtual void ReceiveDamage(Actor _target, float _damage, Actor _damageCauser, Actor _instigator) { }
        
        public static void ApplyDamage(Actor _target, float _damage, Actor _damageCauser, Actor _instigator)
        {
            _target.ReceiveDamage(_target, _damage, _damageCauser, _instigator);
        }
    }
}