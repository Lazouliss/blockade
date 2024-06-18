const url = require('url');
const http = require('http');
const jwt = require('jsonwebtoken');

let lstUsers = []
let friendsList = []
let PORT = 27016
const secretKey = 'BLOCKADE-SRV';

http.createServer((req, res) => {
    const parsedUrl = url.parse(req.url, true);
    const path = parsedUrl.pathname;
    if(req.method === 'GET'){
        const queryParams = parsedUrl.query;
        switchUrl(path, queryParams, res);
    }
	if(req.method === 'POST') {
        let body = '';
        req.on('data', chunk => body += chunk.toString());
        req.on('end', () => switchUrl(path, JSON.parse(body), res));
	}
}).listen(PORT, () => {
	console.log(`WebServer running at http://localhost:${PORT}/ V ${'5'}`);
});

function switchUrl(path, params, res){
    res.setHeader('Access-Control-Allow-Origin', '*');
    switch(path){
        case '/signUp':
            if(lstUsers.find(_user => _user.username == params.username)){
                res.writeHead(401, { 'Content-Type': 'application/json' });
                res.end(JSON.stringify({error: "Username is already taken"}));
                return;
            }
            let user = {username: params.username, pwd: params.pwd};
            lstUsers.push(user);
            var jwtoken = jwt.sign({username: params.username}, secretKey);
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(jwtoken));
            break;

        case '/signIn':
            var jwtoken = jwt.sign({username: params.username}, secretKey);
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(jwtoken));
            break;

        case '/makeFriend':
            if(!friendsList[params.userId1])
                friendsList[params.userId1] = [];
            if(!friendsList[params.userId2])
                friendsList[params.userId2] = [];

            if(!friendsList[params.userId1].includes(params.userId2))
                friendsList[params.userId1].push(params.userId2)
            if(!friendsList[params.userId2].includes(params.userId1))
                friendsList[params.userId2].push(params.userId1)

            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({result: 200}));
        
        default:
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({error: path}));
    }
}