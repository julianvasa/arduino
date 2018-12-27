#include <SD.h>

// On the Ethernet Shield, CS is pin 4. Note that even if it's not
// used as the CS pin, the hardware CS pin (10 on most Arduino boards,
// 53 on the Mega) must be left as an output or the SD library
// functions will not work.
const int chipSelect = 53;
char line[20];

//Kohe pune
float h,m,s,ms;
unsigned long over;
unsigned long elapsed;
boolean restart = false;
    
/* Sensors */
#define ENABLE_FREEIMU  // FreeIMU
#define ENABLE_TINYGPS  // Tiny GPS serial NMEA
//#define ENABLE_SIRFGPS  // Sirf Binary GPS serial
#define ENABLE_HMC6352  // HMC6352 I2C compass
//#define ENABLE_BMP085   // BMP085 I2C Barometer
//#define ENABLE_LDR      // LDR for relative light level
#define ENABLE_BEEP     // Piezo Speaker for audio feedback
#define ENABLE_LCD12864 // Serial LCD12864 128x64 Grahpical display
#define ENABLE_LEDBUTTONS // Go/Reset/Stop buttons

/* Cameras */
#define NUM_SLAVES      5
//unsigned char GOPRO_SLAVES[] = {30};
unsigned char GOPRO_SLAVES[] = {30, 31, 32, 33, 34};


#define PHOTO_DELAY     2000 // milli seconds between finishing a photo and starting the next

/* GPS */
#define GPS_SERIAL      Serial3
#define GPS_BAUD        4800

/* Pins */
#define GOPRO_TRIG      27
#define GOPRO_ID2       28
#define GOPRO_ID3       29
#define SS_PIN          53 // SPI pin
#define LDR_PIN         A15
#define BEEP_PIN        4  // Any PWM pin you choose
#define GOLED_PIN       22
#define RSTLED_PIN      23
#define STPLED_PIN      24
#define GOBTN_PIN       39
#define RSTBTN_PIN      40
#define STPBTN_PIN      38

/*** End of config ***/

#include "floatToString.h"

/* Sd Card */
#include <avr/eeprom.h>

//byte buffer[512];
uint32_t sector = 2;
int bufferPos = 0;

/* Global Variables */
bool goprotrigger = false;
uint32_t photo = 0;
unsigned int retryCount = 0;
uint32_t lastPhoto = 0;

/* I2C */
#if defined(ENABLE_FREEIMU) || defined(ENABLE_BMP085) || defined(ENABLE_HMC6352)
    #include <Wire.h>
#endif

/* GPS */
#ifdef ENABLE_TINYGPS
    #include <TinyGPS.h>
    TinyGPS gps;
    bool newGPSData = false;
    bool timeUpdated = false;
    int year;
    byte month, day, hour, minute, second, hundredths;
    float lat, lon, alt, course, speed,satellites;
    unsigned long age,date,time,age2;
#endif /* ENABLE_TINYGPS */

#ifdef ENABLE_SIRFGPS
    #include "SirfGPS.h"
    SirfGPS gps;
    bool newGPSData = false;
    bool timeUpdated = false;
    int year;
    byte month, day, hour, minute, second, hundredths;
    float lat, lon, alt, course, speed,satellites;
    unsigned long age;
#endif /* ENABLE_SIRFGPS */


/* LCD */
#ifdef ENABLE_LCD12864
    #include <avr/pgmspace.h>
    #include "LCD12864RSPI.h"
    #include "font.h";
  //  #include "floatToString.h"
    #define FONT_HEIGHT 9
    unsigned char screenBuffer[1024];
    uint32_t lastTime = 0;
    unsigned long kohe_pune_fillon;
    unsigned long kohe_pune_mbaron;
    bool fullRedraw = false;
#endif /* ENABLE_LCD12864 */

/* IMU */
#ifdef ENABLE_FREEIMU
    #include <ADXL345.h>
    #include <HMC58X3.h>
    #include <ITG3200.h>
    #include <FreeIMU.h>
    #include <CommunicationUtils.h>
    float imu_val[9];
    float ypr[3];
    FreeIMU my3IMU = FreeIMU();
#endif /* ENABLE_FREEIMU */

/* Compass */
#ifdef ENABLE_HMC6352
    #include <hmc6352.h>
    float heading;
#endif /* ENABLE_HMC6352 */

void setup()
{    
    /* Setup the LCD */
    #ifdef ENABLE_LCD12864
        LCDA.Initialise();
        LCDClearScreen();
        LCDPrintString(0, 0, "Albumi.com", true);
        lastTime = millis() - 1000;
    #endif /* ENABLE_LCD12864 */

    /* Start GPS */
    #ifdef ENABLE_TINYGPS
        GPS_SERIAL.begin(GPS_BAUD);
    #endif /* ENABLE_TINYGPS */
    #ifdef ENABLE_SIRFGPS
        /* Switch to 9600 in SIRF Binary mode */
        GPS_SERIAL.begin(GPS_BAUD);  
        GPS_SERIAL.print("$PSRF100,1,4800,8,1,0*0C\r\n");
        GPS_SERIAL.end();  
        GPS_SERIAL.begin(4800);  
    #endif /* ENABLE_SIRFGPS */

    
    /* I2C */
    #if defined(ENABLE_FREEIMU) || defined(ENABLE_BMP085) || defined(ENABLE_HMC6352)
        Wire.begin();
    #endif

    /* Start IMU */
    #ifdef ENABLE_FREEIMU
        #ifdef ENABLE_LCD12864
            LCDPrintString(3, 0, "Po ndizet busulla       ", true);
        #endif /* ENABLE_LCD12864 */
        //delay(100);
        my3IMU.init(false);
        //delay(500);
        #ifdef ENABLE_LCD12864
            LCDPrintString(3, 0, "Busulla gati       ", true);
        #endif /* ENABLE_LCD12864 */
    #endif /* ENABLE_FREEIMU */

    /* Continue where we were */
    //sector = eeprom_read_dword((uint32_t *) 0x01);
    photo  = eeprom_read_dword((uint32_t *) 0x05);
    Serial.print("photo eeprom");
    Serial.println(photo);
    #ifdef ENABLE_LCD12864
        // Pamje
        sprintf(line, "%04d      ", photo);
        LCDPrintString(0, 0, line, true);
    #endif /* ENABLE_LCD12864 */

    /* Wait for GPS initial data */
    #if defined(ENABLE_TINYGPS) || defined(ENABLE_SIRFGPS)
        #ifdef ENABLE_LCD12864
            LCDPrintString(3, 0, "Po lidhem me GPS     ", true);
        #endif /* ENABLE_LCD12864 */
        
        while (!newGPSData) {
            while (GPS_SERIAL.available()) {
                if (gps.encode(GPS_SERIAL.read())) {
                    newGPSData = true;
                }
            }
        }
        
        #ifdef ENABLE_LCD12864
            LCDPrintString(3, 0, "                      ", true);
        #endif /* ENABLE_LCD12864 */

        #if defined(ENABLE_BMP085) && !defined(ENABLE_LEDBUTTONS)
            /* Initialise with actual starting altitude */
            dps.init(MODE_ULTRA_LOW_POWER, gps.altitude(), true);
            delay(500);
        #endif /* ENABLE_BMP085 */
    #endif /* ENABLE_TINYGPS || ENABLE_SIRFGPS */

    /* Go Pro Cameras */
    pinMode(GOPRO_TRIG, OUTPUT);
    pinMode(GOPRO_ID2, OUTPUT);
    pinMode(GOPRO_ID3, OUTPUT);

    /* Input Pins */
    for (int i = 0; i < NUM_SLAVES; i++) {
        pinMode(GOPRO_SLAVES[i], INPUT);
    }

    /* Set initial state */
    digitalWrite(GOPRO_TRIG, HIGH);
    digitalWrite(GOPRO_ID2, HIGH);
    digitalWrite(GOPRO_ID3, LOW);

    /* Beep */
    #ifdef ENABLE_BEEP
        tone(BEEP_PIN, 2200, 200);
        delay(250);
        tone(BEEP_PIN, 2200, 200);
    #endif /* ENABLE_BEEP */

    Serial.begin(4800);
        
    #ifdef ENABLE_LEDBUTTONS
        pinMode(GOLED_PIN, OUTPUT);
        pinMode(RSTLED_PIN, OUTPUT);
        pinMode(STPLED_PIN, OUTPUT);
        pinMode(GOBTN_PIN, INPUT);
        pinMode(RSTBTN_PIN, INPUT);
        pinMode(STPBTN_PIN, INPUT);

        digitalWrite(GOLED_PIN, HIGH);
        digitalWrite(RSTLED_PIN, HIGH);
        digitalWrite(STPLED_PIN, LOW);


        //pinMode(SS_PIN, OUTPUT);    
        //pinMode(10, OUTPUT);           //it's a good idea to do this (see SPI lib)
        //SPI.begin();                   //the SDCARDmodded lib already sets pin 10 as output

        #ifdef ENABLE_LCD12864
            LCDPrintString(3, 0, "Fillo me JESHIL       ", true);
            LCDPrintString(4, 0, "Me BLU rifillon       ", true);
        #endif /* ENABLE_LCD12864 */

        while(true) {
            if (digitalRead(RSTBTN_PIN) == 1) {
                /* Reset */
                photo = 0;
                //sector = 0;
                //eeprom_write_dword((uint32_t *) 0x01, sector);
                eeprom_write_dword((uint32_t *) 0x05, photo);
                #ifdef ENABLE_LCD12864
                // Pamje                
                    sprintf(line, "%04d      ", photo);
                    LCDPrintString(0, 0, line, true);
                    LCDPrintString(4, 0, "                     ", true);
                #endif /* ENABLE_LCD12864 */
                digitalWrite(RSTLED_PIN, LOW);
            }
            if (digitalRead(GOBTN_PIN) == 1) {
                digitalWrite(GOLED_PIN, LOW);
                digitalWrite(RSTLED_PIN, LOW);
                digitalWrite(STPLED_PIN, HIGH);
                break;
            }

            /* Feed GPS and IMU */
            usefulDelay(1);
        }

        #if defined(ENABLE_BMP085)
            /* Initialise with GPS altitude */
            dps.init(MODE_ULTRA_LOW_POWER, gps.altitude(), true);
            usefulDelay(500);
        #endif /* ENABLE_BMP085 */
    #endif /* ENABLE_LEDBUTTONS */


  Serial.print("Initializing SD card...");
  // make sure that the default chip select pin is set to
  // output, even if you don't use it:
  pinMode(53, OUTPUT);
  
  // see if the card is present and can be initialized:
  if (!SD.begin(chipSelect)) {
    Serial.println("Card failed, or not present");
    // don't do anything more:
    return;
  }
  Serial.println("card initialized.");


    /* Start timer */
    lastPhoto = millis();
}

void loop()
{

    String dataString = "";
    File dataFile;
    float temp;
      
    start:  
    #ifdef ENABLE_LCD12864
        if(restart == true){
            
            #ifdef ENABLE_BEEP
            tone(BEEP_PIN, 2200, 200);
            delay(250);
            tone(BEEP_PIN, 2200, 200);
            #endif /* ENABLE_BEEP */
      
            #ifdef ENABLE_LEDBUTTONS
            pinMode(GOLED_PIN, OUTPUT);
            pinMode(RSTLED_PIN, OUTPUT);
            pinMode(STPLED_PIN, OUTPUT);
            pinMode(GOBTN_PIN, INPUT);
            pinMode(RSTBTN_PIN, INPUT);
            pinMode(STPBTN_PIN, INPUT);
    
            digitalWrite(GOLED_PIN, HIGH);
            digitalWrite(RSTLED_PIN, HIGH);
            digitalWrite(STPLED_PIN, LOW);
            #endif
            
            LCDPrintString(0, 0, "                     ", true);
            LCDPrintString(1, 0, "                     ", true);
            LCDPrintString(2, 0, "                     ", true);
            LCDPrintString(2, 0, "NDALUR               ", true);
            LCDPrintString(3, 0, "Fillo me JESHIL      ", true);
            LCDPrintString(4, 0, "Me BLU rifillon      ", true);
            LCDPrintString(5, 0, "                     ", true);
            LCDPrintString(6, 0, "                     ", true);
            
            while(true){
            if (digitalRead(GOBTN_PIN) == 1 && restart==true) {
                digitalWrite(GOLED_PIN, LOW);
                digitalWrite(RSTLED_PIN, LOW);
                digitalWrite(STPLED_PIN, HIGH);
                
            restart = false;
            break;
            }
            }
 
            /* Feed GPS and IMU */
            usefulDelay(1);

        }    
    #endif /* ENABLE_LCD12864 */
  
    #ifdef ENABLE_LEDBUTTONS
        #ifdef ENABLE_LCD12864
            if (digitalRead(GOBTN_PIN) == 1) {
                if (!fullRedraw) {
                // Pamje                  
                    kohe_pune_fillon = millis();
                    sprintf(line, "%04d    ", photo);
                    LCDA.Initialise();
                    
                    LCDPrintString(1, 0, "                     ", true);
                    LCDPrintString(2, 0, "                     ", true);
                    LCDPrintString(3, 0, "                     ", true);
                    LCDPrintString(4, 0, "                     ", true);
                    LCDPrintString(5, 0, "                     ", true);
                    LCDPrintString(6, 0, "                     ", true);
                    //LCDClearScreen();
                    //LCDA.CLEAR();
                    /*LCDA.DisplayString(0, 0, line, 16);
                    LCDA.DisplayString(1, 0, line, 16);
                    LCDA.DisplayString(2, 0, line, 16);
                    LCDA.DisplayString(3, 0, line, 16);
                    */
                    fullRedraw = true;
                }
                lastTime = millis();
            }
        #endif /* ENABLE_LCD12864 */


        if (!goprotrigger && digitalRead(STPBTN_PIN) == 1) {
            /* Stop */
            digitalWrite(STPLED_PIN, LOW);

            /* Back to normal state */
            digitalWrite(GOPRO_ID2, HIGH);
            digitalWrite(GOPRO_ID3, LOW);

            /* Remember where we were up to */
            //eeprom_update_dword((uint32_t *) 0x01, sector);
            eeprom_write_dword((uint32_t *) 0x05, photo);

            /* Clear screen */
            /*#ifdef ENABLE_LCD12864
                LCDPrintString(1, 0, "                     ", true);
                LCDPrintString(2, 0, "                     ", true);
                LCDPrintString(3, 0, "       NDALUR        ", true);
                LCDPrintString(4, 0, "                     ", true);
                LCDPrintString(5, 0, "                     ", true);
                LCDPrintString(6, 0, "                     ", true);
            #endif  ENABLE_LCD12864 */

            //while (true) {};
            restart = true;
            goto start;
        }
    #endif /* ENABLE_LEDBUTTONS */

  
    /* Update LCD Display */
    #ifdef ENABLE_LCD12864
        /* Update the display every second */
        if (millis() - lastTime > 1000) {
            int len, pos = 0;

            #if defined(ENABLE_TINYGPS) || defined(ENABLE_SIRFGPS)
                /* Update time */
                sprintf(line, "%02d:%02d\0", (hour+1), minute);
                LCDPrintString(0, 16, line, true);

                /* Update GPS Data */
                //temp = lat;
                len = floatToString(line, lat, 7);
                LCDPrintString(2, 0, line, true);
                //pos += len - 1;
              //  LCDPrintString(2, pos - 1, "@", false);
                //temp *= -1;
/*
                temp = (temp - int(temp)) * 60;
                len = floatToString(line, int(temp), 0);
                LCDPrintString(2, pos, line, false);
                pos += len - 1;
                LCDPrintString(2, pos - 1, "'", false);

                temp = (temp - int(temp)) * 60;
                len = floatToString(line, int(temp), 0);
                LCDPrintString(2, pos, line, false);
                pos += len - 1;
                LCDPrintString(2, pos - 1, "\"", false);
                LCDPrintString(2, pos, " ", false);
                pos += 1;
*/
                //temp = lon;
                len = floatToString(line, lon, 7);
                LCDPrintString(3, 0, line, true);
                //pos += len - 1;
               // LCDPrintString(3, 0, line, false);
  /*
                temp = (temp - int(temp)) * 60;
                len = floatToString(line, int(temp), 0);
                LCDPrintString(2, pos, line, false);
                pos += len - 1;
                LCDPrintString(2, pos - 1, "'", false);

                temp = (temp - int(temp)) * 60;
                len = floatToString(line, int(temp), 0);
                LCDPrintString(2, pos, line, false);
                pos += len - 1;
                LCDPrintString(2, pos - 1, "\"", true);
*/
                alt = (alt > 9999) ? 999 : alt;
                len = floatToString(line, alt, 0);
                //LCDPrintString(3, 0, "Alt: 0000m", false);
                LCDPrintString(4, 0, line, true);

                speed = (speed > 9999) ? 999 : speed;
                len = floatToString(line, speed, 0);
                LCDPrintString(5, 0, line, true);

                age = (age > 9999) ? 999 : age;
                len = floatToString(line, age, 0);
                //LCDPrintString(6, 0, line, true);
                
                course = (course > 360) ? 999 : course;
                len = floatToString(line, course, 0);
                //LCDPrintString(3, 10, " Crse: 000@", false);
                LCDPrintString(5, 18, line, true);
                Serial.print("GPS hdg: ");
                Serial.println(line);
                
                satellites = (satellites > 999) ? 999 : satellites;
                len = floatToString(line, satellites, 0);
                LCDPrintString(1, 0 , line, true);
                #endif /* ENABLE_TINYGPS || ENABLE_SIRFGPS */

            #ifdef ENABLE_FREEIMU
                len, pos = 0;
               // LCDPrintString(6, pos, "Yaw: ", false);
               // pos += 5;
                len = floatToString(line, ypr[0], 1);
                //sprintf(line, "%04d", (ypr[0]));
                LCDPrintString(6, 0, line, true);
                //LCDPrintString(6, 5, line, true);
                Serial.print("Yaw: ");
                Serial.println(line);
                //pos += len - 1;

                //LCDPrintString(5, pos, " Pch: ", false);
                //pos += 6;
                len = floatToString(line, ypr[1], 1);
                LCDPrintString(6, 8, line, true);
                //LCDPrintString(6, 13, line, true);                
                Serial.print("Pitch: ");
                Serial.println(line);
                //pos += len - 1;
                //while (pos < 21) {
                  //  LCDPrintString(5, pos++, " ", false);
                //}
                //LCDPrintString(5, pos, "\0", true);

                //pos = 0;
                //LCDPrintString(6, pos, "Roll: ", false);
                //pos += 6;
                len = floatToString(line, ypr[2], 2);
                LCDPrintString(6, 14, line, true);
                //LCDPrintString(6, 18, line, true);                
                // LCDPrintString(6, 22-len, line, true);
                Serial.print("Roll: ");
                Serial.println(line);
                //pos += len;
            #endif /* ENABLE_FREEIMU */

            #ifdef ENABLE_HMC6352
            hmc6352.wake();
        heading = hmc6352.getHeading();
        hmc6352.sleep();
                sprintf(line, "%03d", int(heading));
                //LCDPrintString(6, 11, " Hdg: ", false);
                LCDPrintString(4, 18, line, true);
                Serial.print("Busulla hdg: ");
                Serial.println(line);
          
                //LCDPrintString(6, 20, "@", true);
                
            
            kohe_pune_mbaron = millis();
            elapsed=kohe_pune_mbaron-kohe_pune_fillon;
            h=int(elapsed/3600000);
            over=elapsed%3600000;
            m=int(over/60000);
            over=over%60000;
            s=int(over/1000);
            sprintf(line, "%02d:%02d", int(h), int(m));
            LCDPrintString(2, 16, line, true);

            //ms=over%1000;
            /*
            Serial.print("Raw elapsed time: ");
            Serial.println(elapsed);
            Serial.print("Elapsed time: ");
            Serial.print(h,0);
            Serial.print("h ");
            Serial.print(m,0);
            Serial.print("m ");
            Serial.print(s,0);
            Serial.print("s ");
            Serial.print(ms,0);
            Serial.println("ms");
            Serial.println();
            */
            
            #endif /* ENABLE_HMC6352 */

            lastTime = millis();
            if (fullRedraw) {
                LCDA.CLEAR();
                // Pamje
                sprintf(line, "%04d      ", photo);
                LCDPrintString(0, 0, line, false);

                LCDA.DrawFullScreen(screenBuffer);
                fullRedraw = false;
            }
        }
    #endif /* ENABLE_LCD12864 */

    usefulDelay(0);

    /* Take a photo */
    if (goprotrigger) {
        /* See if the cameras are ready to take the photo */
        int slavesReady = 0;
        #ifdef ENABLE_LCD12864
            LCDPrintString(1, 3, "          ", false);
            delay(100);
            LCDPrintString(1, 3, "Shkrepje: ", false);
        #endif /* ENABLE_LCD12864 */
        //Serial.print("Shkrepje: ");
        //Serial.println(digitalRead(30));
        for (int i = 0; i < NUM_SLAVES; i++) {
            bool ready = digitalRead(GOPRO_SLAVES[i]) == LOW;
            slavesReady += (ready) ? 1 : 0;
            #ifdef ENABLE_LCD12864
                if (ready) {
                    LCDPlaceCharacter(1, 15 + i + 1, i + '1');
                } else {
                    LCDPlaceNegCharacter(1, 15 + i + 1, i + '1');
                }
            #endif /* ENABLE_LCD12864 */
        }
        #ifdef ENABLE_LCD12864
            //LCDPrintString(1, 15, "\0", true);
        #endif /* ENABLE_LCD12864 */

        if (slavesReady == NUM_SLAVES) {
            /* Trigger camera */
            digitalWrite(GOPRO_TRIG, LOW);
            Serial.println("GOPRO_TRIG: LOW");
            delay(1);
            digitalWrite(GOPRO_TRIG, HIGH);
            Serial.println("GOPRO_TRIG: HIGH");

            for (int i = 0; i < NUM_SLAVES; i++) {
            bool ready = digitalRead(GOPRO_SLAVES[i]) == LOW;
            Serial.print("SLAVE READY ");
            Serial.print(i);
            Serial.print(" : ");
            Serial.print(ready);
            Serial.print("  ");
            Serial.println(digitalRead(GOPRO_SLAVES[i]));
            }

            /* Record to SD Card */
            //stringToSDCard("P");
            //longToSDCard(++photo);
            photo = ++photo;


          
              /* GPS */
    #if defined(ENABLE_TINYGPS) || defined(ENABLE_SIRFGPS)
        /* Feed the TinyGPS with new data from GPS */
        usefulDelay(600);
        dataString += String(photo);
        dataString += "_";        
        /* Get time */
       gps.crack_datetime(&year, &month, &day, &hour, &minute, &second, &hundredths, &age);
        
        //byte datedata[6] = {byte(month), byte(day), byte(hour), byte(minute), byte(second), byte(hundredths)};
        sprintf(line, "%02d:%02d\0", (hour+1), minute);     
        //dataString += "Dt ";             
        dataString += String(int(day));
        dataString += ".";        
        dataString += String(int(month));
        dataString += ".";
        dataString += String(int(year));
        dataString += "_";
        //dataString += ";Tm ";
        dataString += String(int(hour+1));
        dataString += ":";
        dataString += String(int(minute));
        dataString += ":";
        dataString += String(int(second));
     
        
       //byte datedata[6] = {byte(month), byte(day), byte(hour), byte(minute), byte(second), byte(hundredths)};
       /* stringToSDCard("D");
        intToSDCard(year);
        addToSDCard(datedata, 6);
        longToSDCard(age);
*/
        if (newGPSData) {
            /* Write new GPS data to SD card */
            //gps.f_get_position(&lat, &lon, &age);
            gps.f_get_position(&lat, &lon, &age);
            alt = gps.f_altitude();
            satellites = gps.satellites();
            course = gps.f_course();
            speed = gps.f_speed_kmph();

            //dataString += ";Lat "; 
            dataString += "_";
            int len = floatToString(line, lat, 7);
            dataString += line;
            //dataString += ";Lon ";   
            dataString += "_";
            len = floatToString(line, lon, 7);     
            dataString += line;
      
            //dataString += ";Alt ";
            dataString += "_";
            dataString += String(long(alt));
            //dataString += ";Crs ";
            dataString += "_";
            dataString += String(long(course));
            //dataString += ";Spd ";
            dataString += "_";
            dataString += String(long(speed));
            //dataString += ";Sats ";
            dataString += "_";
            dataString += String(int(satellites));
         
            /*
            stringToSDCard("G");
            floatToSDCard(lat);
            floatToSDCard(lon);
            floatToSDCard(alt);
            floatToSDCard(course);
            floatToSDCard(speed);
            longToSDCard(age);
*/
            newGPSData = false;
        }
    #endif /* ENABLE_TINYGPS || ENABLE_SIRFGPS */

    /* Update the IMU */
    #ifdef ENABLE_FREEIMU
        usefulDelay(0);
       
        //dataString += ";Yaw ";      
        dataString += "_";
        dataString += String(long(ypr[0]));
        //dataString += ";Ptch ";        
        dataString += "_";
        dataString += String(long(ypr[1]));
        //dataString += ";Roll ";
        dataString += "_";
        dataString += String(long(ypr[2]));
        /*
        stringToSDCard("I");
        floatToSDCard(ypr[0]);
        floatToSDCard(ypr[1]);
        floatToSDCard(ypr[2]);
        */
    #endif /* ENABLE_FREEIMU */

    /* Update heading */
    #ifdef ENABLE_HMC6352
        hmc6352.wake();
        heading = hmc6352.getHeading();
        hmc6352.sleep();
        
        //dataString += ";Hdg ";        
        dataString += "_";        
        dataString += String(long(heading));
        /*
        stringToSDCard("C");
        floatToSDCard(heading);
        */
    #endif /* ENABLE_HMC6352 */


        //dataFile = SD.open("track.txt", FILE_WRITE);
        dataFile = SD.open("track.txt", O_CREAT | O_APPEND | O_WRITE);
        
        // if the file is available, write to it:
        if (dataFile) {
          dataFile.println(dataString);
         
          dataFile.close();
          LCDPrintString(3, 19, "  ", false);
          // print to the serial port too:
          Serial.println(dataString);
                
        }  
        // if the file isn't open, pop up an error:
        else {
          Serial.println("error opening track.txt");
          LCDPrintString(3, 19, "JO", false);
        }
        
    usefulDelay(0);

          
            /* Record to EEPROM */
           // eeprom_update_dword((uint32_t *) 0x01, sector);
            eeprom_write_dword((uint32_t *) 0x05, photo);

            /* Update LCD */
            #ifdef ENABLE_LCD12864
                            // Pamje
                sprintf(line, "%04d      ", photo);
                LCDPrintString(0, 0, line, true);
            #endif /* ENABLE_LCD12864 */

            #ifdef ENABLE_BEEP
            Serial.println("BEEP TRIG");
            tone(BEEP_PIN, 2200, 50);
            #endif /* ENABLE_BEEP */

            usefulDelay(300);

            /* Back to normal state */
            digitalWrite(GOPRO_ID2, HIGH);
            digitalWrite(GOPRO_ID3, LOW);
            lastPhoto = millis();
            goprotrigger = false;
            retryCount = 0;

            #ifdef ENABLE_LCD12864
                //LCDPrintString(1, 3, "          ", false);
            #endif /* ENABLE_LCD12864 */
        } else if (millis() - lastPhoto > 5000) {
            /* A camera has taken too long, retry */
            goprotrigger = false;
            retryCount++;
            Serial.println("CAMERA DOES NOT RESPOND");
            //Serial.println(millis()-lastPhoto);
            
            for (int i = 0; i < NUM_SLAVES; i++) {
            bool ready = digitalRead(GOPRO_SLAVES[i]) == LOW;
            Serial.print("SLAVE ");
            Serial.print(i);
            Serial.print(" ready: ");
            Serial.print(ready);
            Serial.print(" GOPRO_SLAVE: ");
            Serial.println(digitalRead(GOPRO_SLAVES[i]));
            }
            
            /* Back to normal state */
            digitalWrite(GOPRO_ID2, HIGH);
            digitalWrite(GOPRO_ID3, LOW);

            if (retryCount < 3) {
              //Serial.println("retryCount<3");
              /* Retry again */
                //stringToSDCard("R");
                //intToSDCard(retryCount);
                
            Serial.print("IDs: ");    
            Serial.print(digitalRead(GOPRO_ID2));
            Serial.print(" ");
            Serial.println(digitalRead(GOPRO_ID3));
        
                #ifdef ENABLE_LCD12864
                    LCDPrintString(0, 0, "Gabim ne aparat", false);
                #endif /* ENABLE_LCD12864 */
                
                #ifdef ENABLE_BEEP
                    tone(BEEP_PIN, 2200, 50);
                    delay(55);
                    tone(BEEP_PIN, 2200, 50);
                    delay(55);
                    tone(BEEP_PIN, 2200, 50);
                    delay(55);
                    tone(BEEP_PIN, 2200, 50);
                    usefulDelay(2000);
                #else
                    usefulDelay(2170);
                #endif /* ENABLE_BEEP */
            } else {
                /* Remember where we were up to */
           //     eeprom_update_dword((uint32_t *) 0x01, sector);
                  eeprom_write_dword((uint32_t *) 0x05, photo);

				#ifdef ENABLE_LCD12864

                /* Clear screen */
                LCDPrintString(1, 0, "                     ", true);
                LCDPrintString(2, 0, "                     ", true);
                LCDPrintString(3, 0, "  APARATI NUK PUNON  ", true);
                LCDPrintString(4, 0, "                     ", true);
                LCDPrintString(5, 0, "                     ", true);
                LCDPrintString(6, 0, "                     ", true);
               
			   #endif /* ENABLE_LCD12864 */

                /* Annoying "i'm dead" beep */
                while (true) {
                    #ifdef ENABLE_BEEP
                        tone(BEEP_PIN, 1800, 800);
                        delay(1000);
                    #endif /* ENABLE_BEEP */
                }
            }

            lastPhoto = millis();
        }
    } else if (millis() > lastPhoto + PHOTO_DELAY) {
        /*  Check all cameras are ready */
        #ifdef ENABLE_LCD12864
            //LCDPrintString(1, 0, "Aparatet gati: ", false);
        #endif /* ENABLE_LCD12864 */
        
        Serial.println("All cameras are ready");
        /*            
        Serial.print("IDs: ");    
        Serial.print(digitalRead(GOPRO_ID2));
        Serial.print(" ");
        Serial.println(digitalRead(GOPRO_ID3));
        */
        int slavesReady = 0;
        for (int i = 0; i < NUM_SLAVES; i++) {
            bool ready = digitalRead(GOPRO_SLAVES[i]) == HIGH;
            slavesReady += (ready) ? 1 : 0;
            #ifdef ENABLE_LCD12864
                if (ready) {
                    LCDPlaceCharacter(1, 15 + i + 1, i + '1');
                } else {
                    LCDPlaceNegCharacter(1, 15 + i + 1, i + '1');
                }
            #endif /* ENABLE_LCD12864 */
        }
        #ifdef ENABLE_LCD12864
           // LCDPrintString(1, 15, "\0", true);
        #endif /* ENABLE_LCD12864 */
        
         for (int i = 0; i < NUM_SLAVES; i++) {
            bool ready = digitalRead(GOPRO_SLAVES[i]) == HIGH;
            Serial.print("SLAVE ");
            Serial.print(i);
            Serial.print(" : ");
            Serial.print(ready);
            Serial.print("  ");
            Serial.println(digitalRead(GOPRO_SLAVES[i]));
            }
        
        if (slavesReady == NUM_SLAVES) {
            
            Serial.println("Tell cameras to prepare to take a photo ");
            //Serial.println(millis());
            /* Tell cameras to prepare to take a photo */
            digitalWrite(GOPRO_ID2, LOW);
            digitalWrite(GOPRO_ID3, HIGH);
            
            Serial.print("IDs: ");    
            Serial.print(digitalRead(GOPRO_ID2));
            Serial.print(" ");
            Serial.println(digitalRead(GOPRO_ID3));
            
            goprotrigger = true;
            lastPhoto = millis();
        }
    }
}

/**
 * Delay x ms but do useful things during the delay
 */
void usefulDelay(int ms) {
    unsigned long delayStart = millis();
    do {
        /* Feed TinyGPS with data from GPS */
        #if defined(ENABLE_TINYGPS) || defined(ENABLE_SIRFGPS)
            while (GPS_SERIAL.available()) {
                if (gps.encode(GPS_SERIAL.read())) {
                    newGPSData = true;
                }
            }
        #endif /* ENABLE_TINYGPS || ENABLE_SIRFGPS */

        /* Update IMU data */
        #ifdef ENABLE_FREEIMU
            /* 400kHz I2C on 16Mhz chip */
            my3IMU.getYawPitchRoll(ypr);
        #endif /* ENABLE_FREEIMU */
    } while (millis() - delayStart < ms);
}

#ifdef ENABLE_LCD12864
    void LCDClearScreen()
    {
        for (int i = 0; i < 1024; i++) {
            screenBuffer[i] = 0x00;
        }
        LCDA.DrawFullScreen(screenBuffer);
    }

    void LCDPrintString(unsigned int row, unsigned int col, char* string, bool update)
    {
        int i = 0;
        while (string[i]) {
            LCDPlaceCharacter(row, col, string[i]);
            i++;
            col++;
        }

        /* Redraw the row the changed */
        if (update) {
            LCDA.DrawScreenRow(screenBuffer, (row * FONT_HEIGHT) + 1);
        }
    }

    void LCDPlaceCharacter(unsigned int row, unsigned int col, char character)
    {
        if (row > 6 || col >= 21) {
            /* Off the bottom or side */
            return;
        }

        if (fontLookup[byte(character)] == 0 && byte(character) != ' ') {
            /* No character in font */
            return;
        }

        /* What bit in the start character does this start? */
        int startBit = (col * 6) % 8;

        /* Add all 10 character rows */
        for (int i = 0; i < FONT_HEIGHT; i++) {
            /* Pixel are in the lower 5 bits */
            unsigned char pixels = pgm_read_byte_near(fontPixel + (9 * fontLookup[byte(character)]) + i);

            /* Work out what byte this row starts in  */
            int startByte = (i + (row * FONT_HEIGHT) + 1) * 16;
            if (startBit <= 3) {
                /* All within one byte */
                int bits = pixels << 3 - startBit;
                int mask = 0x1F << 3 - startBit;
                screenBuffer[startByte + ((col * 6) / 8)] &= ~mask; // Clear space
                screenBuffer[startByte + ((col * 6) / 8)] |= bits; // Add character
            } else {
                /* Accross two bytes */
                int bits = pixels >> startBit - 3;
                int mask = 0x1F >> startBit - 3;
                screenBuffer[startByte + ((col * 6) / 8)] &= ~mask; // Clear space
                screenBuffer[startByte + ((col * 6) / 8)] |= bits; // Add character

                bits = pixels << (11 - startBit);
                mask = 0x1F << (11 - startBit);
                screenBuffer[startByte + ((col * 6) / 8) + 1] &= ~mask; // Clear space
                screenBuffer[startByte + ((col * 6) / 8) + 1] |= bits; // Add character
            }
        }
    }

    void LCDPlaceNegCharacter(unsigned int row, unsigned int col, char character)
    {
        if (row > 6) {
            /* Off the bottom or side */
            return;
        }

        if (fontLookup[byte(character)] == 0 && character != ' ') {
            /* No character in font */
            return;
        }

        /* What bit in the start character does this start? */
        int startBit = (col * 6) % 8;

        /* Add all 10 character rows */
        for (int i = 0; i < FONT_HEIGHT; i++) {
            /* Pixel are in the lower 5 bits */
            unsigned char pixels = 0x1F & ~pgm_read_byte_near(fontPixel + (9 * fontLookup[byte(character)]) + i);

            /* Work out what byte this row starts in  */
            int startByte = (i + (row * FONT_HEIGHT) + 1) * 16;
            if (startBit <= 3) {
                /* All within one byte */
                int bits = pixels << 3 - startBit;
                int mask = 0x1F << 3 - startBit;
                screenBuffer[startByte + ((col * 6) / 8)] &= ~mask; // Clear space
                screenBuffer[startByte + ((col * 6) / 8)] |= bits; // Add character
            } else {
                /* Accross two bytes */
                int bits = pixels >> startBit - 3;
                int mask = 0x1F >> startBit - 3;
                screenBuffer[startByte + ((col * 6) / 8)] &= ~mask; // Clear space
                screenBuffer[startByte + ((col * 6) / 8)] |= bits; // Add character

                bits = pixels << (11 - startBit);
                mask = 0x1F << (11 - startBit);
                screenBuffer[startByte + ((col * 6) / 8) + 1] &= ~mask; // Clear space
                screenBuffer[startByte + ((col * 6) / 8) + 1] |= bits; // Add character
            }
        }
    }
#endif /* ENABLE_LCD12864 */

