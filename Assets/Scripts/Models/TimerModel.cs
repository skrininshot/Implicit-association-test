namespace Models
{
    public class TimerModel
    {
        public double Value { get; private set; }
        
        public TimerModel()
        {
            Value = 0;
        }

        public void AddDeltaTime(double value)
        {
            Value += value;
        }

        public void Reset()
        {
            Value = 0;
        }
    }
}