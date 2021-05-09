#!/bin/bash
# Garage Startup Script

gpio write 7 1
gpio mode 7 out

cd /home/pi
docker-compose up -d