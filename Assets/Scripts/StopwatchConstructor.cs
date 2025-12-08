using System;

public class StopwatchConstructor
{
    private float _elapsedTime = 0f;
    private bool _isRunning = false;
    private int _lastSecond = 0;

    public Action<int> OnTick;

    public int ElapsedTime => (int)Math.Floor(_elapsedTime);

    public void Start()
    {
        if (_isRunning) return;
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    public void Reset()
    {
        Stop();
        _elapsedTime = 0f;
        _lastSecond = 0;
        OnTick?.Invoke(0);
    }

    public void Tick(float deltaTime)
    {
        if (!_isRunning) return;
        
        _elapsedTime += deltaTime;
        
        int currentSecond = (int)_elapsedTime;
        if (currentSecond > _lastSecond)
        {
            _lastSecond = currentSecond;
            OnTick?.Invoke(currentSecond);
        }
    }
}