var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io').listen(server, { log: false });

app.get('/', function(req, res){
    res.send("it works");
});

var users = 0;

var port = process.env.PORT || 8000;



io.on('connection', function(socket){
    console.log('a user connected');
    users++;
    console.log(users+'/2 users');
    if(users == 2){
        console.log("READY");
        io.sockets.emit("startGame","go!");
    }

    socket.on('disconnect', function(){
        console.log('user disconnected');
        users--;
    });

    socket.on('keyPressed', function(msg){
        console.log('keyPressed: ' + msg);
        //socket.emit("keyPressed", msg);
        socket.broadcast.emit("keyPressed", msg);
    });

});


server.listen(port, function(){
    console.log('listening on *:'+port);
});