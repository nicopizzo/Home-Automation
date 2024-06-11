# Home-Automation

# Build and setup on Raspberry Pi
	- Build and publish docker images
	- Image new Pi
	- Install Docker
# Hosting - Will host both Service and Web app on Pi
	- Copy over docker-compose.deploy.yml
	- SSL Certificate is automatically renewed and applied on startup
		- Add a crontab that restarts the services once a week to allow the SSL Cert to be refreshed and applied.
		- sudo crontab -e
		- 0 2 * * 0 docker compose -f /home/pi/docker-compose.yml restart
	- Update entries in docker-compose file
		- TokenKey
		- Certificate Path and Password