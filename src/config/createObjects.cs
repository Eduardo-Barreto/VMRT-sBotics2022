public delegate void ActionHandler();

import("config/Classes/timer.cs");


import("config/Classes/servo.cs");
motor leftMotor = new motor("leftMotor");
motor rightMotor = new motor("rightMotor");
motor frontLeftMotor = new motor("frontLeftMotor");
motor frontRightMotor = new motor("frontRightMotor");


import("config/Classes/ultrasonic.cs");
ultrasonic[] frontUltra ={
    new ultrasonic("centerUltra0"),
    new ultrasonic("centerUltra1")
};

ultrasonic leftUltra = new ultrasonic("leftUltra");


import("config/Classes/robot.cs");
myRobot robot = new myRobot("leftMotor", "rightMotor", "frontLeftMotor", "frontRightMotor", "centerUltra0");

import("config/Classes/light.cs");
lightSensor[] lineSensors ={
    new lightSensor("S0"),
    new lightSensor("S1"),
    new lightSensor("S2"),
    new lightSensor("S3")
};

import("config/Classes/led.cs");
led[][] leds ={
    new led[] {
        new led("L01"),
        new led("L01"),
        new led("L02"),
        new led("L02")
    },
    new led[] {
        new led("L10"),
        new led("L11"),
        new led("L12"),
        new led("L13")
    },
    new led[] {
        new led("L20"),
        new led("L21"),
        new led("L22"),
        new led("L23")
    }
};
