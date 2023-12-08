#include <DHT.h>

#define dht_apin A0

DHT DHTOB; 

void setup(){
  Serial.begin(9600); 
  delay(500); 
  Serial.println("DHT11 Sensor\n\n");
  delay(1000);
}

void loop(){
  DHTOB.readll(dht_apin);

    
}
