# Home-Automation

Build and setup on Raspberry Pi
	- Build and publish docker images
	- Image new Pi
	- Install Docker
	- Install WiringPi for gpio commands
		- https://github.com/WiringPi/WiringPi
	- Obtain certificate and key
	  - Merge to PFX "certutil -mergepfx mySite.crt mySite.pfx"
Hosting - Will host both Service and Web app on Pi
	- Copy over garage_startup.sh
	- Copy over docker-compose.deploy.yml
	- Configure garage_startup.sh to run on startup
		- sudo crontab -e
		- Add => @reboot /path/to/script.sh
	- Update entries in docker-compose file
		- TokenKey
		- Certificate Path and Password