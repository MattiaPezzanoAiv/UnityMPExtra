namespace MP.Unity.Variables
{
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// This class holds a value in the project
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VariableReference<T> : ScriptableObject, ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        /// <summary>
        /// Property used for editor code to manipulate the value in tools without using reflection
        /// </summary>
        public T EditorOnlyInitialValue
        {
            get => m_value;
            set => m_value = value;
        }
#endif

        [SerializeField]
        private T m_value;

        private T m_runtimeValue;

        public System.Action<T> OnValueChanged;

        public T Value
        {
            get => m_runtimeValue;
            set
            {
                if(!m_runtimeValue.Equals(value))
                {
                    OnValueChanged?.Invoke(m_runtimeValue);
                    m_runtimeValue = value;
                }
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            m_runtimeValue = m_value;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
    }
}
