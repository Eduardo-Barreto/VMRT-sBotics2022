/**
* Gerencia um sensor ultrassônico
*/

public class ultrasonic{
    private UltrasonicSensor sensor; // Sensor ultrassônico a ser gerenciado
    private long lastReadTime = 0;
    private float lastRead = -1;

    /**
    * @brief Construtor da classe
    *
    *
    * @param sensorName: (string) nome do sensor
    */
    public ultrasonic(string sensorName){
        this.sensor = Bot.GetComponent<UltrasonicSensor>(sensorName);
    }

    public float read{
        get{
            if(timer.current < lastReadTime + 100)
                return lastRead;

            lastRead = (float)(sensor.Analog);
            lastReadTime = timer.current;

            return lastRead;
        }
    }
}