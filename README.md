# Automatically create DNS A-record, SSL certificate and IIS Bindings
A proof of concept for my internship. 
Enter a business name and automatically generate a subdomain for this client, get a SSL certificate and create a http and https binding in IIS

Auto DNS/SSL/Binding:

1. Install iis (internet information services) on your machine (or activate it using Windows Features)
2. [optional] Create a new site in iis
3. Create a new file named transip.key with the private transip key string in it formatted like: "-----BEGIN PRIVATE KEY-----x-----END PRIVATE KEY-----" and put this in the same folder as the site. i.e "C:\\localiis\\transip.key"
4. Install win-acme plugin version from their site, add wacs.exe to your $PATH
5. Run wacs.exe once as administrator with standard settings (N), just so you can agree to their terms and enter your email address (otherwise the program can’t run in 		 unattended mode)
6. Run Visual studio code as administrator, as win-acme needs admin rights (this will get replaced by a process with elevated rights in production)
7. Go to appsettings.json in the API project, check the settings in the “DomainRegistrationSettings” section.
	- url: The API url from your host of choice
	- stringIp: the IP address you want to point the new DNS A-record to
	- accessToken: the accesstoken requested by the Transip API
	- transipPrivateKeyFile: the location of your Transip private key stored in a file
	- transipLogin: the login/user name used on Transip
	- siteid: the id of your site in iis (Tip: you can find this by running win-acme and go through the process of creating a new binding manually, during this process 		  win-acme will show the available sites and their corresponding ids)
	- iisSiteName: the name of the site in iis
	- mainDomain: the name of the primary domain for which the subdomain will be made
	- wacsFileName: the name of wacs.exe, in case you would like to change the name of the file/process

