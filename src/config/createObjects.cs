public delegate void ActionHandler();

import("config/Classes/timer.cs");
myTimer timer = new myTimer();

import("config/Classes/servo.cs");
motor leftMotor = new motor("leftMotor");
motor rightMotor = new motor("rightMotor");

import("config/Classes/robot.cs");
robotBase robot = new robotBase("leftMotor", "rightMotor");

import("config/Classes/light.cs");
lightSensor[] lineSensors ={
    new lightSensor("S0"),
    new lightSensor("S1"),
    new lightSensor("S2"),
    new lightSensor("S3"),
    new lightSensor("S4")
};
