void lineLeds(string color){
    for(byte i = 0; i<= 3; i++)
        leds[1][i].on(color);
}

void arrowLeds(string color, byte side){
    lineLeds(color);

    for(byte i = 0; i<= 2; i++)
        leds[i][side].on(color);
}

void turnOnAllLeds(string color){
    foreach(led[] line in leds){
        foreach(led light in line){
            light.on(color);
        }
    }
}

void turnOffAllLeds(){
    foreach(led[] line in leds){
        foreach(led light in line){
            light.off();
        }
    }
}