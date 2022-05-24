1. Run project in IIS Express.
2. Create users by using following commands in postman - 
    POST - http://localhost:13486/User
    Body - {"userName":"test3", "firstName":"test", "lastName":"3"}
    Headers: Content-Type: Application/json

    Create as much user needed. But user name should be unique.

    Return object will contain SecretToken.
3. Fetch all users by following request in postman
    GET - http://localhost:13486/User?sortBy=1&pageIndex=1&pageSize=50
    Headers: Content-Type: Application/json
             Authorization: "Secret token from any user"

    List of users will apear along with other information.

4. Update profile picture url by
   PUT - http://localhost:13486/User/updateprofileimage?id=1
    Body - {"profileImageUrl":"desiredurl"}
   Headers: Content-Type: Application/json
            Authorization: "Secret token from user who wants to update"

5. Create Screech by - 
    POST - http://localhost:13486/screech
    Body - {"content":"screeeeeeeeech"}
    Headers: Content-Type: Application/json
            Authorization: "Secret token from user who wants to Screech"

6. Update content of Screech by -
    PUT - http://localhost:13486/screech/updatecontent?id={desiredScreechId}
    Body - {"content":"modified screeeeeech"}
    Headers: Content-Type: Application/json
            Authorization: "Secret token from user who wants to update own Screech"

7. Get list of screech - 
    GET - http://localhost:13486/screech?pageSize=50&pageIndex=1&sortBy=1
    GET - http://localhost:13486/screech?pageSize=50&pageIndex=1&sortBy=1&userName={desiredUserName}
    Headers: Content-Type: Application/json

8. Get specific screech - 
    GET - http://localhost:13486/screech/{id}
    Headers: Content-Type: Application/json