using Models;
using UnityEngine;

namespace Controllers
{
    public class TimerController : ITimerController
    {
        private TimerModel _model;

        public TimerController(TimerModel model)
        {
            _model = model;
        }
        
        public void Tick()
        {
            _model.AddDeltaTime(Time.deltaTime);
        }
    }
}