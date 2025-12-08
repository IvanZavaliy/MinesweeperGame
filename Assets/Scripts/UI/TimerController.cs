using UnityEngine;

namespace UI
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] private GameCanvasView view;
    
        private StopwatchConstructor timer = new StopwatchConstructor();

        public int seconds = 0;

        private void Awake()
        {
            timer.OnTick += seconds =>
            {
                view.UpdateTimeToDisplay(seconds);
            };
        }

        void Update()
        {
            timer.Tick(Time.deltaTime);
            seconds = timer.ElapsedTime;
        }
    
        public void StartTimer() => timer.Start();
        public void StopTimer() => timer.Stop();
        public int GetElapsedTime => timer.ElapsedTime;

        public void ResetTimer()
        {
            timer.Reset();
        }
    }
}
