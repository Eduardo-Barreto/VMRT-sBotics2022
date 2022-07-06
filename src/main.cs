import("config/createObjects.cs");
import("routines/lineFollower.cs");

double startTime;
void setup()
{
    robot.locked = false;
    IO.Timestamp = false;
    IO.ClearWrite();
    IO.ClearPrint();
    startTime = Time.Timestamp;
}


void loop()
{
    runLineFollower();
}

async Task Main()
{
    setup();
    for (; ; )
    {
        loop();
        await Task.Delay(50);
    }
}
