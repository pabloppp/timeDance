var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io').listen(server, { log: false });

app.get('/', function(req, res){
    res.send("it works");
});


io.on('connection', function(socket){
    console.log('a user connected');
    socket.on('disconnect', function(){
        console.log('user disconnected');
    });

    socket.on('SEND', function(msg){
        console.log('message: ' + JSON.stringify(msg));
    });

    socket.on('keyPressed', function(msg){
        console.log('keyPressed: ' + msg);
        socket.emit("keyPressed", msg);
        //socket.broadcast.emit("keyPressed", msg);
    });

});


server.listen(3000, function(){
    console.log('listening on *:3000');
});