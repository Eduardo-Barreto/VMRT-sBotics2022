public delegate void ActionHandler();

import("config/Classes/timer.cs");


import("config/Classes/servo.cs");
motor leftMotor = new motor("leftMotor");
motor rightMotor = new motor("rightMotor");
motor frontLeftMotor = new motor("frontLeftMotor");
motor frontRightMotor = new motor("frontRightMotor");

import("config/Classes/robot.cs");

myRobot robot = new myRobot("leftMotor", "rightMotor", "frontLeftMotor", "frontRightMotor");

import("config/Classes/light.cs");
lightSensor[] lineSensors ={
    new lightSensor("S0"),
    new lightSensor("S1"),
    new lightSensor("S2"),
    new lightSensor("S3"),
    new lightSensor("S4")
};
