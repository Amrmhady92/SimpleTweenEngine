using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleTweenEngine
{
    [CreateAssetMenu(menuName = "SimpleTweenEngine/Float Field")]
    public class FloatField : ScriptableObject
    {

        [SerializeField]
        private float value;
        public event System.Action<float> OnValueChanged;
        public float accuracy = 0.001f;
        public float Value
        {
            get
            {
                return value;
            }

            set
            {
                if (Mathf.Abs(value - this.value) > 0.0001)
                {
                    if (value < accuracy && value > -accuracy)
                    {
                        value = 0;
                    }
                    if(OnValueChanged!=null)
                        OnValueChanged(value);
                    this.value = value;

                }
            }
        }

        public static implicit operator float(FloatField b)
        {
            return b.Value;

        }

    }
}
