int ledpin = 10;
int bright = 90;
int lower = 25;

void setup() {
  // put your setup code here, to run once:
pinMode(ledpin, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
analogWrite(ledpin,bright);
delay(1000);
analogWrite(ledpin, lower);
delay(500);
}
