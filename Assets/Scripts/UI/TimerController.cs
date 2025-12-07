using UnityEngine;

namespace UI
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] private GameCanvasView view;
    
        private StopwatchConstructor timer = new StopwatchConstructor();

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
        }
    
        public void StartTimer() => timer.Start();
        public void StopTimer() => timer.Stop();

        public void ResetTimer()
        {
            timer.Reset();
        }
    }
}
