# Pi Server
A simple http webserver for raspberry pi

Pi Controller (the android client app) is [available on google play](https://play.google.com/store/apps/details?id=com.dscode.picontroller)!

## Installation

- Install the latest version of [raspian jessie](https://www.raspberrypi.org/downloads/raspbian/) on the PI
- Install mono: `sudo apt-get install mono-complete`
- After building the server app, copy the contents of the bin folder to your raspberry PI via ssh. 
- Run app: `sudo mono rpi-simple-webserver.exe`

If you want to register the webserver as a startup service on your PI [this guide will help.](http://www.stuffaboutcode.com/2012/06/raspberry-pi-run-program-at-start-up.html) Make sure you use absolute paths to mono and the server app in your startup script because init.d runs as root not as pi.
