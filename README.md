<h1><b>Arduino camera controller</b></h1>

This project in 2012. We wanted to build something (cheap) that shoots 6 simultaneous photos (kind of Google Street View) and take photos all over Albania. We didn't have too much money to spend on this project and that's why we took a look at the Arduino project.
<br><br>
The controller runs an arduino program (trackview) which controls 6 GoPro cameras to shoot instantaneous photos at regular intervals (parametrized). <br>First you have to download and install [Arduino IDE](https://www.arduino.cc/en/main/software) and than deploy the script trackview to the Arduino device.<br>
It performs 3 main tasks, first it records a bunch of sensor data like GPS, Barometer, Pitch, Roll, Heading. <br>
Second it triggers the GoPro cameras at regular intervals. <br>
Third it updates a graphical LCD will sensor values and camera status.

The last task is the stitching process, source code are found in the folder stitching. <br>
It's a C#.NET app developed by me that does the stitching of 6 photos using the following libraries/apps:
- [Hugin - Panorama photo stitcher](http://hugin.sourceforge.net/) to do the stitching process. Hugin needs a pto file which defines the control points used by Hugin during the stitching process.
- [ImageMagick](https://www.imagemagick.org/) to do the crop, convert, resize stuff
- [Krpanotools](https://krpano.com/tools/) to generate a viewer, some xml files etc.

I made two versions of this app:
- stitiching several panos
- stitching just one pano (6 photos)

<h2><b>Hardware</b></h2>

- [Seeduino Mega](https://www.seeedstudio.com/Seeeduino-Mega-ATmega256-p-717.html) running at 3.3v
- [MicroSD shield](https://www.sparkfun.com/products/12761) you'll need to modify this to work on the mega as the SPI pins on the mega are different
- [ITG3200 ADXL345 combo board](https://www.sparkfun.com/products/retired/10121) - optional
- [HMC5843 Triple Axis Magnetometer Breakout](https://www.sparkfun.com/products/retired/9371) - optional
- [HMC6352 Compass Module](https://www.sparkfun.com/products/retired/7915) - optional
- LDR - optional
- Piezo Speaker - optional
- [ST7920 based 128x64 graphical LCD](https://www.sparkfun.com/products/retired/9351) - optional
- Any serial GPS (module) outputting NMEA sentences - optional
- 6 x GoPro Hero HD cameras

<h2><b>Software</b></h2>

The code in this repo has a bunch of options at the top. You should be able to comment out and of the ENABLE_ defines at the top if you're not using that particular piece of hardware.
The code relies on a number of other libraries to hand the sensors. They are:

- [TinyGPS](http://arduiniana.org/libraries/tinygps/)
- [ADXL345](http://code.google.com/p/adxl345driver/source/browse/#svn%2Fbranches%2Ffvaresano)
- [HMC58X3](https://launchpad.net/hmc58x3)
- [ITG3200](http://code.google.com/p/itg-3200driver/source/browse/#svn%2Ftrunk)
- [FreeIMU](http://www.varesano.net/projects/hardware/FreeIMU)
- [hmc6352](http://rubenlaguna.com/wp/2009/03/19/arduino-library-for-hmc6352/index.html)
- [SDCARDmodded](http://supertechman.blogspot.com/2011/02/sdcard-library.html)

Download all these and put them in you sketchbook/libraries folder (create this if it doesn't already exist). 
<br>
The data is written directly to the SD card, from start to end. The format is:
Date+ _ +Time+ _ +Lat+ _ +Lon+ _ +pitch+ _ +roll+ _ +yaw

We managed to take some pics (around 2000-3000 panos). We created a website [Albumi - Albania Virtual Tour](http://www.albumi.com) 