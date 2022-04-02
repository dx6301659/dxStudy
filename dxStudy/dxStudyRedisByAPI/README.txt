Step 1: Open CMD or Powershell by administrator and run WSL.
Step 2: Install and config Redis Server on local
	sudo apt-add-repository ppa:redislabs/redis
	sudo apt-get update
	sudo apt-get upgrade
	sudo apt-get install redis-server
Step 3: Restart redis server and check server status
	sudo service redis-server restart
	$ redis-cli 
	 127.0.0.1:6379> set user:1 "dingxu1"
	 127.0.0.1:6379> get user:1
	"dingxu1"

## Notice: CMD of start server: sudo service redis-server start
           CMD of stop server: sudo service redis-server stop
		   The seted User Name: dingxu, Password: dx6301659
## Notice: Add StackExchange.Redis and Serilog.AspNetCore by Manage NuGet