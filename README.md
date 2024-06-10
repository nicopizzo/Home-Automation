# Home-Automation

# Build and setup on Raspberry Pi
	- Build and publish docker images
	- Image new Pi
	- Install Docker
	- Obtain certificate and key
	  - Merge to PFX "certutil -mergepfx mySite.crt mySite.pfx"
# Hosting - Will host both Service and Web app on Pi
	- Copy over docker-compose.deploy.yml
	- Update entries in docker-compose file
		- TokenKey
		- Certificate Path and Password
