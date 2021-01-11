using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleTweenEngine
{
    [CreateAssetMenu(menuName = "SimpleTweenEngine/Int Field")]

    public class IntField : ScriptableObject
    {
        [SerializeField]
        private int value;
        public event Action<int> OnValueChanged;

        public virtual int Value
        {
            get
            {
                return value;
            }

            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    if (OnValueChanged != null)
                        OnValueChanged(value);
                }
            }
        }

        public static implicit operator int(IntField b)
        {
            return b.Value;
        }
    }
}
