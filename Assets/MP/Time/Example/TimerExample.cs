namespace MP.Examples
{
    using MP.Time;
    using UnityEngine;

    public class TimerExample : MonoBehaviour
    {
        public float duration;

        private Timer m_timer;

        private void Awake()
        {
            m_timer = new Timer(new TimeSource(3f));
            m_timer.Duration = duration;
            m_timer.Elapsed += () => Debug.Log("Elapsed!");
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                m_timer.Start();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                m_timer.Stop();
            }
        }
    }
}
